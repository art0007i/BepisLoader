# BepisLoader

App which allows using BepInEx with Resonite.

## Installation

1. Download [BepisLoader-full.zip](https://github.com/art0007i/BepisLoader/releases/latest/BepisLoader-full.zip).
2. Extract it over the resonite folder
3. Launch BepisLoader.exe (should be in the same folder as Resonite.exe)

If you are on steam you can edit your custom launch arguments like this:
(add the full path to BepisLoader.exe at the front and %command% at the end)
```
-Invisible -LoadAssembly "Libraries/ResoniteModLoader.dll" -DoNotAutoLoadHome
```
->
```
E:\Steam\steamapps\common\Resonite\BepisLoader.exe -Invisible -LoadAssembly "Libraries/ResoniteModLoader.dll" -DoNotAutoLoadHome %command%
```