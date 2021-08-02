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

			int males = 0;
			int females = 0;
			string malesStr = "WAA_Males".Translate();
			string femalesStr = "WAA_Females".Translate();

			for (int i = 0; i < randomInRange; i++)
			{
				// find a valid place to spawn the pawns
				IntVec3 loc2 = CellFinder.RandomClosewalkCellNear(loc, map, radius);
				Pawn newThing = PawnGenerator.GeneratePawn(pawnKindDef, null);
				if (newThing.gender == Gender.Female) { females++; } else if (newThing.gender == Gender.Male) { males++; }
				GenSpawn.Spawn(newThing, loc2, map);
				if (randomInRange == 1)
				{
					// text to use when spawning only one animal
					text = "WAA_Message_SpawnSingle".Translate(newThing.Label, newThing.GetGenderLabel(), newThing.ageTracker.AgeBiologicalYears);
					//text = String.Concat(new string[] { "A wild ", newThing.Label, " appeared! ",
					//newThing.gender.ToString(), " ", newThing.Label, ", ",
					//newThing.ageTracker.AgeBiologicalYears.ToString(), " years old. ",
					//});
				}
			}
			if (males == 1) { malesStr = "WAA_Male".Translate(); }
			if (females == 1) { femalesStr = "WAA_Female".Translate(); }
			if (randomInRange > 1)
			{
				// text to use when spawning more than one animal
				//text = "WAA_Message_SpawnGroup".Translate(randomInRange, pawnKindDef.label, males, malesStr.Translate(), females, femalesStr.Translate()); I think this line is broken, see next line
				text = "WAA_Message_SpawnGroup".Translate(randomInRange, pawnKindDef.label, males, malesStr, females, femalesStr);
				//text = String.Concat(new string[] { "A group of ", randomInRange.ToString(), " wild ", pawnKindDef.label, " appeared! ", males.ToString(), malesStr, " and ", females.ToString(), femalesStr });
			}

			// check whether the alert should be played
			if ((animals_before_current_spawns < Settings.AnimalCount) && (Settings.EnableMod))
			{
				if (pawnKindDef.RaceProps.predator)
				{
					if (Settings.PredatorsOnly)
					{
						Messages.Message(text, new TargetInfo(loc, map, false), MessageTypeDefOf.NegativeEvent);
					} else
					{
						Messages.Message(text, new TargetInfo(loc, map, false), MessageTypeDefOf.PositiveEvent);
					}
					
				}
				else
				{
					if (!Settings.PredatorsOnly)
					{
						Messages.Message(text, new TargetInfo(loc, map, false), MessageTypeDefOf.PositiveEvent);
					}
				}
			}
			// return false to prevent the vanilla code from running (which would spawn another animal/group of animals)
			return false;
		}
	}
}
