using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace BuryOnTheSpot.AI
{
    public class WorkGiver_Bury : WorkGiver_Scanner
    {
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
			var things = GetPotentialTargets(pawn);
			return things;
		}

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
			if (!TargetCanBeBuried(t) || !pawn.CanReserveAndReach(t, PathEndMode.Touch, Danger.Deadly))
            {
				return null;
            }
			var job = JobMaker.MakeJob(BuryOnTheSpotDefOf.BuryTarget, t);
			return job;
		}

        private IEnumerable<Thing> GetPotentialTargets(Pawn pawn)
		{
			var targets = pawn.Map.designationManager.SpawnedDesignationsOfDef(BuryOnTheSpotDefOf.BuryDesignation)
				.Select(d => d.target.Thing)
				.Where(t => TargetCanBeBuried(t))
				.ToList();
			return targets;
		}

		private bool TargetCanBeBuried(Thing t)
		{
			return t is Corpse;
		}
	}
}
