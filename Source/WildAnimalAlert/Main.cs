using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using static System.Net.Mime.MediaTypeNames;

namespace RD_WildAnimalAlert
{
	[StaticConstructorOnStartup]
	static class Main
	{
		static Main()
		{
			var harmony = new Harmony("org.rd.wildanimalalert");
			harmony.PatchAll();
			Log.Message("[RD_WildAnimalAlert] Patched with transpiler.");
		}
	}

	[HarmonyPatch(typeof(WildAnimalSpawner), nameof(WildAnimalSpawner.SpawnRandomWildAnimalAt))]
	public static class WildAnimalSpawner_SpawnRandomWildAnimalAt_Transpiler
	{
		// Helper: Check if opcode is any variant of stloc
		static bool IsStloc(OpCode opcode)
		{
			return opcode == OpCodes.Stloc_0 || opcode == OpCodes.Stloc_1 || 
			       opcode == OpCodes.Stloc_2 || opcode == OpCodes.Stloc_3 || 
			       opcode == OpCodes.Stloc_S || opcode == OpCodes.Stloc;
		}

		// Helper: Check if opcode is Call or Callvirt
		static bool IsCallOrCallvirt(OpCode opcode)
		{
			return opcode == OpCodes.Call || opcode == OpCodes.Callvirt;
		}

		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodBase original)
		{
			var codes = instructions.ToList();

			var notifyMany = AccessTools.Method(typeof(WildAnimalSpawner_SpawnRandomWildAnimalAt_Transpiler), nameof(NotifySpawnedAnimals));
			var notifyOne = AccessTools.Method(typeof(WildAnimalSpawner_SpawnRandomWildAnimalAt_Transpiler), nameof(NotifySpawnedAnimal));
			var mapField = AccessTools.Field(typeof(WildAnimalSpawner), "map");

			if (Settings.DebugMode)
				Log.Message($"[RD_WildAnimalAlert] Transpiler: Starting transpile with {codes.Count} instructions");

			// ===== Hook 1: After randomInRange is set =====
			int randIndex = codes.FindIndex(ci =>
				IsStloc(ci.opcode) &&
				codes[Math.Max(0, codes.IndexOf(ci) - 1)] != null &&
				IsCallOrCallvirt(codes[Math.Max(0, codes.IndexOf(ci) - 1)].opcode) &&
				codes[Math.Max(0, codes.IndexOf(ci) - 1)].operand is MethodInfo mi &&
				mi.Name == "get_RandomInRange");

			int randomInRangeIndex = -1;

			if (randIndex != -1)
			{
				// Get the local variable index for randomInRange
				randomInRangeIndex = GetLocalIndex(codes[randIndex]);
				
				if (Settings.DebugMode)
					Log.Message($"[RD_WildAnimalAlert] Transpiler: Found randomInRange assignment at index {randIndex}, local index = {randomInRangeIndex}");

				var afterRandLabel = il.DefineLabel();
				codes[randIndex + 1].labels.Add(afterRandLabel);

				if (Settings.DebugMode)
					Log.Message($"[RD_WildAnimalAlert] Transpiler: Using method arguments - arg1=loc, arg3=animalKind");

				// Insert NotifySpawnedAnimals if > 1
				// Method signature: SpawnRandomWildAnimalAt(IntVec3 loc, bool canFlyIn, PawnKindDef animalKind)
				// Args: 0=this, 1=loc, 2=canFlyIn, 3=animalKind
				codes.InsertRange(randIndex + 1, new[]
				{
					CreateLdloc(randomInRangeIndex, codes),
					new CodeInstruction(OpCodes.Ldc_I4_1),
					new CodeInstruction(OpCodes.Ble_S, afterRandLabel),

					CreateLdloc(randomInRangeIndex, codes),
					new CodeInstruction(OpCodes.Ldarg_3),  // animalKind parameter
					new CodeInstruction(OpCodes.Ldarg_1),  // loc parameter
					new CodeInstruction(OpCodes.Ldarg_0),  // this
					new CodeInstruction(OpCodes.Ldfld, mapField),
					new CodeInstruction(OpCodes.Call, notifyMany)
				});

				if (Settings.DebugMode)
					Log.Message($"[RD_WildAnimalAlert] Transpiler: Inserted Hook 1 (NotifySpawnedAnimals for groups)");
			}
			else
			{
				Log.Error("[RD_WildAnimalAlert] Transpiler: Could not find randomInRange assignment!");
			}

			// ===== Hook 2: After pawn is generated =====
			int pawnIndex = codes.FindIndex(ci =>
				IsStloc(ci.opcode) &&
				codes[Math.Max(0, codes.IndexOf(ci) - 1)] != null &&
				IsCallOrCallvirt(codes[Math.Max(0, codes.IndexOf(ci) - 1)].opcode) &&
				codes[Math.Max(0, codes.IndexOf(ci) - 1)].operand is MethodInfo mi &&
				mi.Name.Contains("GeneratePawn"));

			if (pawnIndex != -1 && randomInRangeIndex >= 0)
			{
				if (Settings.DebugMode)
					Log.Message($"[RD_WildAnimalAlert] Transpiler: Found GeneratePawn at index {pawnIndex}");

				var afterPawnLabel = il.DefineLabel();
				codes[pawnIndex + 1].labels.Add(afterPawnLabel);

				var pawnLocal = codes[pawnIndex].operand;
				var locLocal = FindLocalIndexByType<IntVec3>(codes);

				if (Settings.DebugMode)
					Log.Message($"[RD_WildAnimalAlert] Transpiler: pawnLocal = {pawnLocal}, using randomInRangeIndex = {randomInRangeIndex}");

				// Insert NotifySpawnedAnimal if == 1
				codes.InsertRange(pawnIndex + 1, new[]
				{
					CreateLdloc(randomInRangeIndex, codes),
					new CodeInstruction(OpCodes.Ldc_I4_1),
					new CodeInstruction(OpCodes.Bne_Un_S, afterPawnLabel),

					new CodeInstruction(OpCodes.Ldloc, pawnLocal),
					new CodeInstruction(OpCodes.Ldloc, locLocal),
					new CodeInstruction(OpCodes.Ldarg_0),
					new CodeInstruction(OpCodes.Ldfld, mapField),
					CreateLdloc(randomInRangeIndex, codes),
					new CodeInstruction(OpCodes.Call, notifyOne)
				});

				if (Settings.DebugMode)
					Log.Message($"[RD_WildAnimalAlert] Transpiler: Inserted Hook 2 (NotifySpawnedAnimal for singles)");
			}
			else if (pawnIndex == -1)
			{
				Log.Error("[RD_WildAnimalAlert] Transpiler: Could not find GeneratePawn call!");
			}

			if (Settings.DebugMode)
				Log.Message($"[RD_WildAnimalAlert] Transpiler: Completed with {codes.Count} instructions");

			return codes;
		}

		// Helper: Get the local variable index from any stloc/ldloc instruction
		static int GetLocalIndex(CodeInstruction ci)
		{
			if (ci.opcode == OpCodes.Stloc_0 || ci.opcode == OpCodes.Ldloc_0) return 0;
			if (ci.opcode == OpCodes.Stloc_1 || ci.opcode == OpCodes.Ldloc_1) return 1;
			if (ci.opcode == OpCodes.Stloc_2 || ci.opcode == OpCodes.Ldloc_2) return 2;
			if (ci.opcode == OpCodes.Stloc_3 || ci.opcode == OpCodes.Ldloc_3) return 3;
			if (ci.operand is LocalBuilder lb) return lb.LocalIndex;
			return -1;
		}

		// Helper: Create a CodeInstruction to load a local variable by index
		static CodeInstruction CreateLdloc(int index, List<CodeInstruction> codes)
		{
			// For indices 0-3, use short-form opcodes with no operand
			switch (index)
			{
				case 0: return new CodeInstruction(OpCodes.Ldloc_0);
				case 1: return new CodeInstruction(OpCodes.Ldloc_1);
				case 2: return new CodeInstruction(OpCodes.Ldloc_2);
				case 3: return new CodeInstruction(OpCodes.Ldloc_3);
			}

			// For index > 3, try to find a LocalBuilder with that index
			foreach (var code in codes)
			{
				if (code.operand is LocalBuilder lb && lb.LocalIndex == index)
					return new CodeInstruction(OpCodes.Ldloc, lb);
			}

			// Fallback: use Ldloc_S with byte operand
			return new CodeInstruction(OpCodes.Ldloc_S, (byte)index);
		}

		// Helper: Find a local variable index by type
		static object FindLocalIndexByType<T>(List<CodeInstruction> codes)
		{
			var type = typeof(T);
			foreach (var ci in codes)
				if (ci.operand is LocalBuilder lb && lb.LocalType == type)
					return lb;
			throw new InvalidOperationException($"Local of type {type} not found.");
		}

		static float CurrentTotalAnimalNumber(Map map)
		{
			if (Settings.DebugMode)
				Log.Message($"[RD_WildAnimalAlert] CurrentTotalAnimalNumber begin");

			float num = 0f;
			List<Pawn> allPawnsSpawned = (List<Pawn>)map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (allPawnsSpawned[i].Faction == null)
					num++;
			}

			if (Settings.DebugMode)
				Log.Message($"[RD_WildAnimalAlert] CurrentTotalAnimalNumber returning {num}");

			return num;
		}

		// Your callbacks
		public static void NotifySpawnedAnimals(int count, PawnKindDef kind, IntVec3 loc, Map map)
		{
			string text = "WAA_Message_DebugString".Translate();

			if (Settings.DebugMode)
				Log.Message($"[RD_WildAnimalAlert] NotifySpawnedAnimals: {count}x {kind.defName} at {loc} on {map}");

			if (!Settings.EnableMod || CurrentTotalAnimalNumber(map) >= Settings.AnimalCount) {
				return; // bail out early
			}

			text = "WAA_Message_SpawnGroup_v2".Translate(count, kind.label);

			if (kind.RaceProps.predator)
				Messages.Message(text, new TargetInfo(loc, map, false), MessageTypeDefOf.NegativeEvent);
			else if (!Settings.PredatorsOnly)
				Messages.Message(text, new TargetInfo(loc, map, false), MessageTypeDefOf.PositiveEvent);

		}

		public static void NotifySpawnedAnimal(Pawn pawn, IntVec3 loc, Map map, int count)
		{
			string text = "WAA_Message_DebugString".Translate();

			if (Settings.DebugMode)
				Log.Message($"[RD_WildAnimalAlert] NotifySpawnedAnimal: {pawn} at {loc} on {map}, count={count}");

			if (!Settings.EnableMod || CurrentTotalAnimalNumber(map) >= Settings.AnimalCount)
			{
				return; // bail out early
			}

			text = "WAA_Message_SpawnSingle".Translate(pawn.Label, pawn.GetGenderLabel(), pawn.ageTracker.AgeBiologicalYears);

			if (pawn.kindDef.RaceProps.predator)
				Messages.Message(text, new TargetInfo(pawn), MessageTypeDefOf.NegativeEvent);
			else if (!Settings.PredatorsOnly)
				Messages.Message(text, new TargetInfo(pawn), MessageTypeDefOf.PositiveEvent);
		}
	}
}
