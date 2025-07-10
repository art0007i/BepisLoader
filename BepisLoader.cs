using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace BepisLoader;

public class BepisLoader
{
    internal static string resoDir = string.Empty;
    internal static AssemblyLoadContext alc = null!;
    static void Main(string[] args)
    {
#if DEBUG
        File.WriteAllText("Assemblies.log", "BepisLoader started\n");
#endif
        resoDir = Directory.GetCurrentDirectory();

        alc = new BepisLoadContext();

        // TODO: removing this breaks stuff, idk why
        AppDomain.CurrentDomain.AssemblyResolve += ResolveGameDll;

        var asm = alc.LoadFromAssemblyPath(Path.Combine(resoDir, "BepInEx", "core", "BepInEx.NET.CoreCLR.dll"));

        var resoDllPath = Path.Combine(resoDir, "Resonite.dll");

        var t = asm.GetType("StartupHook");
        var m = t.GetMethod("Initialize", BindingFlags.Public | BindingFlags.Static, [typeof(string), typeof(AssemblyLoadContext)]);
        m.Invoke(null, [resoDllPath, alc]);

        // Find and load Resonite
        var resoAsm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "Resonite");
        if (resoAsm == null)
        {
            resoAsm = alc.LoadFromAssemblyPath(Path.Combine(resoDir, "Resonite.dll"));
        }
        try
        {
            var result = resoAsm.EntryPoint!.Invoke(null, [args]);
            if (result is Task task) task.Wait();
        }
        catch (Exception e)
        {
            File.WriteAllLines("BepisCrash.log", [DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - Resonite crashed", e.ToString()]);
        }
    }

    static Assembly? ResolveGameDll(object? sender, ResolveEventArgs args)
    {
        var assemblyName = new AssemblyName(args.Name);

        var found = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == assemblyName.Name);
        if (found != null)
        {
            return found;
        }

        if (assemblyName.Name == "System.Management") return null;

        var targetPath = Path.Combine(resoDir, assemblyName.Name + ".dll");
        if (File.Exists(targetPath))
        {
            var asm = alc.LoadFromAssemblyPath(targetPath);
            return asm;
        }

        return null;
    }

    private class BepisLoadContext : AssemblyLoadContext
    {
        protected override Assembly? Load(AssemblyName assemblyName)
        {
            var found = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == assemblyName.Name);
            if (found != null)
            {
                return found;
            }

            if (assemblyName.Name == "System.Management") return null;

            var targetPath = Path.Combine(resoDir, assemblyName.Name + ".dll");
            if (File.Exists(targetPath))
            {
                var asm = LoadFromAssemblyPath(targetPath);
                return asm;
            }
            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            var rid = RuntimeInformation.RuntimeIdentifier;
            var nativeLibs = Path.Join(resoDir, "runtimes", rid, "native");
            var potential = Path.Combine(nativeLibs, GetUnmanagedLibraryName(unmanagedDllName));
            if (File.Exists(potential))
            {
                return LoadUnmanagedDllFromPath(potential);
            }
            return IntPtr.Zero;
        }

        private static string GetUnmanagedLibraryName(string name)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return $"{name}.dll";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return $"lib{name}.so";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return $"lib{name}.dylib";

            throw new PlatformNotSupportedException();
        }
    }

#if DEBUG
    private static object _lock = new object();
#endif
    public static void Log(string message)  
    {
#if DEBUG
        lock(_lock)
        {
            File.AppendAllLines("Assemblies.log", [message]);
        }
#endif
    }
}
