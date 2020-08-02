using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BuryOnTheSpot.Patches
{
    [StaticConstructorOnStartup]
    [HarmonyPatch(typeof(ThingWithComps), "GetGizmos")]
    public class Bury_Gizmo
    {
        private static readonly Texture2D BuryCommandTexture = ContentFinder<Texture2D>.Get("UI/Designators/Bury", true);

        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, ThingWithComps __instance)
        {
            IEnumerator<Gizmo> enumerator = __result.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }

            if (__instance is Corpse && __instance.Map.designationManager.DesignationOn(__instance, BuryOnTheSpotDefOf.BuryDesignation) == null)
            {
                var command = new Command_Action
                {
                    defaultLabel = "BuryOnTheSpot_Bury".Translate(),
                    defaultDesc = "BuryOnTheSpot_GizmoDesc".Translate(),
                    icon = BuryCommandTexture,
                    action = () =>
                    {
                        __instance.Map.designationManager.AddDesignation(new Designation(__instance, BuryOnTheSpotDefOf.BuryDesignation));
                    }
                };
                yield return command;
            }
        }
    }
}
