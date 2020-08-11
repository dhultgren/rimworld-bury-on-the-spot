using UnityEngine;
using Verse;

namespace BuryOnTheSpot
{
    public class BuryOnTheSpotSettings : ModSettings
    {
        public bool UpgradeTerrainOnDisappear;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref UpgradeTerrainOnDisappear, "upgradeTerrainOnDisappear", false);
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            var listing = new Listing_Standard();
            listing.ColumnWidth = 500f;
            listing.Begin(inRect);

            listing.Label("BuryOnTheSpot_UpgradeSoilDescription".Translate());
            listing.CheckboxLabeled("BuryOnTheSpot_UpgradeSoilCheckbox".Translate(), ref UpgradeTerrainOnDisappear);

            listing.End();
        }
    }
}
