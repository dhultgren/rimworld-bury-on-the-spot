using HarmonyLib;
using System.Reflection;
using UnityEngine;
using Verse;

namespace BuryOnTheSpot
{
    [StaticConstructorOnStartup]
    public class BuryOnTheSpot : Mod
    {
        public static BuryOnTheSpotSettings Settings;

        static BuryOnTheSpot()
        {
            new Harmony("BuryOnTheSpot").PatchAll(Assembly.GetExecutingAssembly());
        }

        public BuryOnTheSpot(ModContentPack content) : base(content)
        {
            Settings = GetSettings<BuryOnTheSpotSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Settings.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory() => "Bury On The Spot";
    }
}
