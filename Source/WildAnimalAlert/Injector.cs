using RimWorld;
using System.Reflection;
using System;
using Verse;

namespace RD_WildAnimalAlert
{
    [StaticConstructorOnStartup]
    public class Injector
    {
        const BindingFlags UniversalBindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		static bool Inject()
        {
            if (!Detour.TryDetourFromTo(
				typeof(WildSpawner).GetMethod("SpawnRandomWildAnimalAt", UniversalBindingFlags), 
				typeof(_WildSpawner).GetMethod("_SpawnRandomWildAnimalAt", UniversalBindingFlags)))
            {
                Log.Message("[RD_WildAnimalAlert] Failed to load detour for WildSpawner.SpawnRandomWildAnimalAt");
                return false;
			}
			Log.Message("[RD_WildAnimalAlert] Successfully loaded detour(s)! :)");
            return true;
        }

        static Injector()
        {
			Inject();
        }
    }
}