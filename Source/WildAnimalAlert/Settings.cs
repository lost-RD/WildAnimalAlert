using Verse;

namespace RD_WildAnimalAlert
{
	public class Settings : ModSettings
	{
		internal static bool EnableMod = true;
		internal static int AnimalCount = 5;
        internal static bool PredatorOnly = false;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref EnableMod, "EnableMod", true);
			Scribe_Values.Look(ref AnimalCount, "AnimalCount", 1);
            Scribe_Values.Look(ref PredatorOnly, "PredatorOnly", false);
		}
	}
}