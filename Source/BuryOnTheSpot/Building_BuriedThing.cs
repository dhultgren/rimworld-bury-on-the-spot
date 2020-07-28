using RimWorld;
using Verse;

namespace BuryOnTheSpot
{
    public class Building_BuriedThing : Building
    {
        public string contents;

        private static readonly int TicksBeforeDespawn = 60000 * 15;
        private int age;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref age, "age", 0, false);
            Scribe_Values.Look(ref contents, "contents");
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
        }
        public override string LabelMouseover => "BuryOnTheSpot_MouseoverText".Translate(contents, (TicksBeforeDespawn - age).ToStringTicksToPeriod(allowSeconds: false));

        public override void TickRare()
        {
            base.TickRare();
            age += 250;
            if (age >= TicksBeforeDespawn)
            {
                DeSpawn();
            }
        }
    }
}
