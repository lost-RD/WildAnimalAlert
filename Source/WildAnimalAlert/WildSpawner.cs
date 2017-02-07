using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;

namespace RD_WildAnimalAlert
{
	public class _WildSpawner
	{
		private const int AnimalCheckInterval = 1210;

		private const float BaseAnimalSpawnChancePerInterval = 0.0268888883f;

		private const int PlantTrySpawnIntervalAt100EdgeLength = 650;

		private Map map;

		public _WildSpawner(Map map)
		{
			this.map = map;
		}

		public float _CurrentTotalAnimalNumber
		{
			get
			{
				float num = 0f;
				List<Pawn> allPawnsSpawned = this.map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].kindDef.wildSpawn_spawnWild && allPawnsSpawned[i].Faction == null)
					{
						num += 1;
					}
				}
				return num;
			}
		}

		public void _SpawnRandomWildAnimalAt(IntVec3 loc)
		{
			PawnKindDef pawnKindDef = (from a in this.map.Biome.AllWildAnimals
			where this.map.mapTemperature.SeasonAcceptableFor(a.race)
			select a).RandomElementByWeight((PawnKindDef def) => this.map.Biome.CommonalityOfAnimal(def) / def.wildSpawn_GroupSizeRange.Average);
			if (pawnKindDef == null)
			{
				Log.Error("No spawnable animals right now.");
				return;
			}
			int randomInRange = pawnKindDef.wildSpawn_GroupSizeRange.RandomInRange;
			int radius = Mathf.CeilToInt(Mathf.Sqrt((float)pawnKindDef.wildSpawn_GroupSizeRange.max));
			string text = "DEBUG STRING: something went wrong, contact lost_RD with details";
			float animals_before_current_spawns = this._CurrentTotalAnimalNumber;

			if (randomInRange > 1)
			{
				text = String.Concat(new string[] { "A group of ", randomInRange.ToString() , " wild ", pawnKindDef.label, " appeared!"});
			}
				
			//Find.LetterStack.ReceiveLetter("A wild " + newThing.Label + " appeared!", newThing.gender.ToString() + " " + newThing.Label + "\n\n" + newThing.ageTracker.AgeBiologicalYears + " years old\n\nIf you no longer wish to see these messages, disable RD_WildAnimalAlert in the Mods menu.", LetterType.Good, new TargetInfo(loc2, map, false), null);

			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 loc2 = CellFinder.RandomClosewalkCellNear(loc, this.map, radius);
				Pawn newThing = PawnGenerator.GeneratePawn(pawnKindDef, null);
				GenSpawn.Spawn(newThing, loc2, this.map);
				if (randomInRange == 1)
				{
					text = String.Concat(new string[] { "A wild ", newThing.Label, " appeared! ",
					newThing.gender.ToString(), " ", newThing.Label, ", ",
					newThing.ageTracker.AgeBiologicalYears.ToString(), " years old. ",
					});
				}
			}
			if (animals_before_current_spawns <= 5)
			{
				Messages.Message(text, new TargetInfo(loc, map, false), MessageSound.Standard);
			}
		}
	}
}
