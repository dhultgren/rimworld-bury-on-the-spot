using HarmonyLib;
using Verse;

namespace BuryOnTheSpot.Patches
{
    [HarmonyPatch(typeof(GenSpawn), "SpawningWipes")]
    public class Patch_SpawningWipes
    {
        public static void Postfix(BuildableDef newEntDef, BuildableDef oldEntDef, ref bool __result)
        {
            if (newEntDef == BuryOnTheSpotDefOf.Building_BuriedThing)
            {
                __result = false;
            }
        }
    }
}
