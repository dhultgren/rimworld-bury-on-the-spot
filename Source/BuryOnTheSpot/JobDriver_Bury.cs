using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace BuryOnTheSpot
{
    class JobDriver_Bury : JobDriver
    {
        private static readonly int Duration = 60000 / 24 * 2;

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
                var holeBuilding = (Building_BuriedThing)ThingMaker.MakeThing(ThingDef.Named("Building_BuriedThing"), null);
                holeBuilding.contents = (TargetThingA as Corpse)?.InnerPawn?.LabelShort ?? TargetThingA.LabelNoCount;
                TargetThingA.DeSpawn();
                GenSpawn.Spawn(holeBuilding, TargetLocA, Map, Rot4.Random, WipeMode.Vanish);
            });

            yield break;
        }

        private bool TargetCanBeBuried()
        {
            return TargetThingA is Corpse;
        }
    }
}
