# BepisLoader

App which allows using BepInEx with Resonite.

## Windows Installation

1. Download [BepisLoader-full-windows.zip](https://github.com/art0007i/BepisLoader/releases/latest/download/BepisLoader-full-windows.zip)
2. Extract it over the resonite folder
3. Launch Resonite.exe

## Windows Uninstallation

1. Remove hostfxr.dll (this is our entry point on windows)
2. Remove hostfxr.pdb
3. Remove hookfxr.ini
4. Remove Bepis* files
5. Remove BepInEx folder

## Linux Installation

1. Download [BepisLoader-full-linux.zip](https://github.com/art0007i/BepisLoader/releases/latest/download/BepisLoader-full-linux.zip)
2. Extract it over the resonite folder
3. Change LinuxBootstrap.sh to launch `BepisLoader.dll` instead of `Renderite.Host.dll` (This needs to be re-done every time resonite updates. If anyone has a better idea for an entry point on linux let me know!)
4. Launch Resonite.exe

## Linux Uninstallation

1. Remove Bepis* files
2. Remove BepInEx folder
3. Change LinuxBootstrap.sh to launch `Renderite.Host.dll` instead of `BepisLoader.dll`

## References

BepisLoader makes use of these repos and packages them inside it's releases.
- [BepInEx net9 Fork](https://github.com/art0007i/BepInEx)
- [BepInEx Resonite Shim](https://github.com/art0007i/BepInExResoniteShim)
- [hookfxr](https://github.com/MonkeyModdingTroop/hookfxr)
