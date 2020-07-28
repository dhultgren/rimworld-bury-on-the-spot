using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace BuryOnTheSpot
{
    // Add right click menu to bury corpses
    [HarmonyPatch(typeof(FloatMenuMakerMap), "AddHumanlikeOrders")]
    public class FloatMenuMakerMap_AddHumanlikeOrders
    {
        private static void Postfix(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
        {
            if (pawn.jobs == null) return;

            foreach (LocalTargetInfo targetBody in GenUI.TargetsAt_NewTemp(clickPos, TargetParametersBody))
            {
                if (!pawn.CanReach(targetBody, PathEndMode.ClosestTouch, Danger.Deadly)) continue;

                // Add menu option to bury target
                opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Bury".Translate(targetBody.Thing.LabelCap, targetBody.Thing), delegate ()
                {
                    pawn.jobs.TryTakeOrderedJob(new Job(DefDatabase<JobDef>.GetNamed("BuryOnTheSpot"), targetBody));
                }, MenuOptionPriority.High), pawn, targetBody));
            }
        }

        // Only allow targeting corpses
        private static TargetingParameters TargetParametersBody => new TargetingParameters()
        {
            canTargetItems = true,
            mapObjectTargetsMustBeAutoAttackable = false,
            validator = (TargetInfo target) => target.Thing is Corpse corpse
        };
    }
}
