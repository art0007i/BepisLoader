using System.Reflection;
using System.Runtime.InteropServices;

namespace BepisLoader;

public static class LoadResonite
{
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool SetDllDirectory(string lpPathName);

    public static void Load(string[] args)
    {
        var rid = RuntimeInformation.RuntimeIdentifier;
        var nativeLibs = Path.Join(BepisLoader.resoDir, "runtimes", rid, "native");
        BepisLoader.Log($"Setting native library directory: {nativeLibs}");
        // TODO: figure out a cross platform way to do this
        SetDllDirectory(nativeLibs);

        var resoAsm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "Resonite");
        if(resoAsm == null)
        {
            resoAsm = Assembly.Load(File.ReadAllBytes(Path.Combine(BepisLoader.resoDir, "Resonite.dll")));
            BepisLoader.Log($"Loaded Resonite assembly from {BepisLoader.resoDir}");
            BepisLoader.loadedAssemblies["Resonite"] = resoAsm;
        }
        resoAsm.EntryPoint!.Invoke(null, [args]);
    }
}
