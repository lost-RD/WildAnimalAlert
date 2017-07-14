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
			list.CheckboxLabeled("Enable mod", ref Settings.EnableMod, "Turn off to disable alerts when wild animals enter the map");
			list.Gap();
			{
				string label = "Threshold amount of animals below which to warn on arrival: ";
				string description = "Defaults to 1 so you'll only be notified if there are no wild animals left alive on the map when a new one arrives.";
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
			list.End();
		}

		public override string SettingsCategory()
		{
			return "Wild Animal Alert";
		}
	}
}