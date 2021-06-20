using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace BuryOnTheSpot.AI
{
    public class JobDriver_Bury : JobDriver
    {
        private static readonly int Duration = (int)(60000 / 24 * 0.33);

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (!pawn.Reserve(TargetA, job, errorOnFailed: errorOnFailed)) return false;
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            this.FailOn(() => !TargetCanBeBuried());

            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);

            yield return new Toil()
            {
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = Duration
            }.WithProgressBarToilDelay(TargetIndex.A)
            .FailOnDespawnedOrNull(TargetIndex.A);

            yield return Toils_General.Do(() =>
            {
                var holeBuilding = TargetLocA.GetThingList(Map)
                    .FirstOrDefault(t => t is Building_BuriedThing) as Building_BuriedThing;

                if (holeBuilding == null)
                {
                    holeBuilding = ThingMaker.MakeThing(ThingDef.Named("Building_BuriedThing"), null) as Building_BuriedThing;
                    GenSpawn.Spawn(holeBuilding, TargetLocA, Map, Rot4.North, WipeMode.Vanish);
                    pawn.Map.edificeGrid?.Register(holeBuilding);
                }
                holeBuilding.AddThing((TargetThingA as Corpse)?.InnerPawn?.LabelShort ?? TargetThingA.LabelNoCount);
                pawn.Map.designationManager?.TryRemoveDesignationOn(TargetThingA, BuryOnTheSpotDefOf.BuryDesignation);
                TargetThingA.DeSpawn();
            });

            yield break;
        }

        private bool TargetCanBeBuried()
        {
            return TargetThingA is Corpse;
        }
    }
}
