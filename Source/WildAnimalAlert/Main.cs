using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using System.Reflection;
using Verse;
using RimWorld;
using UnityEngine;

namespace RD_WildAnimalAlert
{
	[StaticConstructorOnStartup]
	class Main
	{
		static Main()
		{
			Log.Message("[RD_WildAnimalAlert] Initialising...");
			var harmony = new Harmony("org.rd.wildanimalalert");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
			Log.Message("[RD_WildAnimalAlert] Success! Probably.");
		}
	}

	[HarmonyPatch(typeof(WildAnimalSpawner))]
	[HarmonyPatch("SpawnRandomWildAnimalAt")]
	[HarmonyPatch(new Type[] { typeof(IntVec3)})]
	class Patch
	{
		static float CurrentTotalAnimalNumber(WildAnimalSpawner __instance)
		{
			// map is a private field so we access it with Traverse
			var trv = Traverse.Create(__instance);
			Map map = trv.Field("map").GetValue<Map>();
			// count all animals on the map
			float num = 0f;
			List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (allPawnsSpawned[i].Faction == null)
				{
					num ++;
				}
			}
			// return the amount of animals on the map
			return num;
		}

		static bool Prefix(WildAnimalSpawner __instance, IntVec3 loc)
		{
			// map is private so we access it with Traverse
			var trv = Traverse.Create(__instance);
			Map map = trv.Field("map").GetValue<Map>();
			if (map == null)
			{
				Log.Message("[RD_WildAnimalAlert] Map is null, something is wrong here");
				return false;
			}
			// select a valid pawnkind to spawn
			PawnKindDef pawnKindDef = (from a in map.Biome.AllWildAnimals
									   where map.mapTemperature.SeasonAcceptableFor(a.race)
									   select a).RandomElementByWeight((PawnKindDef def) => map.Biome.CommonalityOfAnimal(def) / def.wildGroupSize.Average);
			if (pawnKindDef == null)
			{
				Log.Error("No spawnable animals right now.");
				return false;
			}
			// choose an amount of pawns to spawn
			int randomInRange = pawnKindDef.wildGroupSize.RandomInRange;
			// and a radius within which to spawn them
			int radius = Mathf.CeilToInt(Mathf.Sqrt((float)pawnKindDef.wildGroupSize.max));
			string text = "DEBUG STRING: something went wrong, contact lost_RD with details";
			// check the amount of animals on the map
			float animals_before_current_spawns = CurrentTotalAnimalNumber(__instance);

			if (randomInRange > 1)
			{
				// text to use when spawning more than one animal
				text = String.Concat(new string[] { "A group of ", randomInRange.ToString(), " wild ", pawnKindDef.label, " appeared!" });
			}
			
			for (int i = 0; i < randomInRange; i++)
			{
				// find a valid place to spawn the pawns
				IntVec3 loc2 = CellFinder.RandomClosewalkCellNear(loc, map, radius);
				Pawn newThing = PawnGenerator.GeneratePawn(pawnKindDef, null);
				GenSpawn.Spawn(newThing, loc2, map);
				if (randomInRange == 1)
				{
					// text to use when spawning only one animal
					text = String.Concat(new string[] { "A wild ", newThing.Label, " appeared! ",
					newThing.gender.ToString(), " ", newThing.Label, ", ",
					newThing.ageTracker.AgeBiologicalYears.ToString(), " years old. ",
					});
				}
			}
			// check whether the alert should be played
			if ((animals_before_current_spawns < Settings.AnimalCount) && (Settings.EnableMod))
			{
				Messages.Message(text, new TargetInfo(loc, map, false), MessageTypeDefOf.PositiveEvent);
			}
			// return false to prevent the vanilla code from running (which would spawn another animal/group of animals)
			return false;
		}
	}
}
