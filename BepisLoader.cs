using System.Diagnostics;
using System.Reflection;

namespace BepisLoader;

public class BepisLoader
{
    internal static string resoDir = string.Empty;
    static void Main(string[] args)
    {
#if DEBUG
        File.WriteAllText("Assemblies.log", "BepisLoader started\n");
#endif
        var currentPath = Process.GetCurrentProcess().MainModule!.FileName;
        resoDir = Path.GetDirectoryName(currentPath)!;
        var resoDllPath = Path.Combine(resoDir, "Resonite.dll");

        Log($"Loading: {resoDllPath}");

        AppDomain.CurrentDomain.AssemblyResolve += ResolveGameDll;

        StartupHook.Initialize(resoDllPath);

        LoadResonite.Load(args);
    }

    internal static Dictionary<string, Assembly> loadedAssemblies = new();

    static Assembly? ResolveGameDll(object? obj, ResolveEventArgs args)
    {
        var assemblyName = new AssemblyName(args.Name).Name;
        Log($"Resolving assembly: {args.Name}");

        if (assemblyName == null)
        {
            Log($"what?: {args.Name}");
            return null;
        }

        var found = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == assemblyName);
        if (found != null)
        {
            Log($"(already loaded): {args.Name}");
            loadedAssemblies[assemblyName] = found;
            return found;
        }

        if (loadedAssemblies.TryGetValue(assemblyName, out var asm)) {
            Log($"(got from cache): {args.Name}");
            return asm;
        }

        var targetPath = Path.Combine(resoDir, assemblyName + ".dll");
        if (File.Exists(targetPath))
        {
            var asmBytes = File.ReadAllBytes(targetPath);
            asm = Assembly.Load(asmBytes);
            loadedAssemblies[assemblyName] = asm;
            Log($"Loaded assembly: {assemblyName} from {targetPath}");
            return asm;
        }
        //if (File.Exists(Paths.ExecutablePath) && assemblyName.Name == "Resonite")
        //{
        //    File.ReadAllBytes();
        //    return Assembly.LoadFrom(Paths.ExecutablePath);
        //}
        return null;
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
