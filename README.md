# BepisLoader

App which allows using BepInEx with Resonite.

## Installation

1. Download [BepisLoader-full.zip](https://github.com/art0007i/BepisLoader/releases/latest/download/BepisLoader-full.zip)
2. Extract it over the resonite folder
3. Download [RML pre-release](https://github.com/resonite-modding-group/ResoniteModLoader/releases)
4. Add it to your launch arguments `-LoadAssembly ResoniteModLoader.dll`
5. (On linux) Change Resonite.sh to launch `BepisLoader.dll` instead of `Resonite.dll`
6. Launch Resonite.exe

## Uninstallation

1. Remove dotnet.exe (this is our entry point on windows)
2. Remove Bepis* files
3. Remove BepInEx folder
4. (On linux) Change Resonite.sh to launch `Resonite.dll` instead of `BepisLoader.dll`
