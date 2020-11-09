using UnityEngine;
using Verse;

namespace RD_WildAnimalAlert
{
	public class Controller : Mod
	{
		public Controller(ModContentPack content) : base(content)
		{
			GetSettings<Settings>();
		}

		public override void WriteSettings()
		{
			base.WriteSettings();
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			Listing_Standard list = new Listing_Standard();
			list.ColumnWidth = inRect.width;
			list.Begin(inRect);
			list.Gap();
			list.CheckboxLabeled("WAA_Settings_EnableMod".Translate(), ref Settings.EnableMod, "WAA_Settings_EnableModMouseOver".Translate());
			list.Gap();
			{
				string label = "WAA_Settings_ThresholdLabel".Translate();
				string description = "WAA_Settings_ThresholdDesc".Translate();
				int valueMin = 0;
				int valueMax = 100;

				Rect currentRect = list.GetRect(Text.LineHeight);
				Rect currentRectLeft = currentRect.LeftHalf().Rounded();
				Rect currentRectRight = currentRect.RightHalf().Rounded();

				//Text label for repair threshold, translated, with tooltip.
				Widgets.Label(currentRectLeft, label+Settings.AnimalCount.ToString());
				if (Mouse.IsOver(currentRectLeft))
				{
					Widgets.DrawHighlight(currentRectLeft);
				}
				TooltipHandler.TipRegion(currentRectLeft, description);

				//Increment value by -1 (button).
				if (Widgets.ButtonText(new Rect(currentRectRight.xMin, currentRectRight.y, currentRectRight.height, currentRectRight.height), "-", true, false, true))
				{
					if (Settings.AnimalCount <= valueMax && Settings.AnimalCount > valueMin)
					{
						Settings.AnimalCount--;
					}
				}

				//Set value (slider).
				Settings.AnimalCount = Mathf.RoundToInt(Widgets.HorizontalSlider(new Rect(currentRectRight.xMin + currentRectRight.height + 10f, currentRectRight.y, currentRectRight.width - (currentRectRight.height* 2 + 20f), currentRectRight.height), Settings.AnimalCount, 0, 100, true));

				//Increment value by +1 (button).
				if (Widgets.ButtonText(new Rect(currentRectRight.xMax - currentRectRight.height, currentRectRight.y, currentRectRight.height, currentRectRight.height), "+", true, false, true))
				{
					if (Settings.AnimalCount < valueMax && Settings.AnimalCount >= valueMin)
					{
						Settings.AnimalCount++;
					}
				}
			}
			list.Gap();
			list.CheckboxLabeled("WAA_Settings_PredatorsOnly".Translate(), ref Settings.PredatorsOnly, "WAA_Settings_PredatorsOnlyMouseOver".Translate());
			list.End();
		}

		public override string SettingsCategory()
		{
			return "WAA_Settings".Translate();
		}
	}
}