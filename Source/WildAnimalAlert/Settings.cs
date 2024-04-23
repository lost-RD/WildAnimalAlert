using Verse;

namespace RD_WildAnimalAlert
{
	public class Settings : ModSettings
	{
		internal static bool EnableMod = true;
		internal static int AnimalCount = 5;
		internal static bool PredatorsOnly = false;
		internal static bool DebugMode = false;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref EnableMod, "EnableMod", true);
			Scribe_Values.Look(ref AnimalCount, "AnimalCount", 1);
			Scribe_Values.Look(ref PredatorsOnly, "PredatorsOnly", false);
			Scribe_Values.Look(ref DebugMode, "DebugMode", false);
		}
	}
}