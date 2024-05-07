# Wild Animal Alert
RimWorld mod originally made for Spartyon7

Alerts the player when wild animals wander onto the map. Configurable via the mod settings menu with some options: choose how many animals must be on the map to disable alerts, and choose whether to alert for all animals or just predators.

## Building

My modding directory structure is as follows:

```
.
├── Libraries
│   ├── ...
│   └── 1.5
|       ├── 0Harmony.dll
│       ├── Assembly-CSharp.dll
|       └── ...
└── WildAnimalAlert
    ├── ...
    └── Source
        └── WildAnimalAlert
            ├── Controller.cs
            ├── Main.cs
            ├── obj
            ├── Properties
            ├── Settings.cs
            ├── WildAnimalAlert.csproj
            └── WildAnimalAlert.sln
```

Relative paths are used in WildAnimalAlert.csproj to look for RimWorld's DLLs in the the corresponding version folder in Libraries.

You will need to replicate this structure. I copied all DLL files from the RimWorldWin64_Data\Managed folder into the 1.5 folder because it's only 30MB but you only need a few specific files.

You will also need to include a copy of Harmony: https://github.com/pardeike/Harmony/releases