using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace BuryOnTheSpot
{
    public class Building_BuriedThing : Building
    {
        public List<string> contentList = new List<string>();

        private static readonly int TicksBeforeDespawn = 60000 * 15;
        private static readonly int MaxContentLength = 50;
        private int age;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref age, "age", 0, false);
            Scribe_Collections.Look(ref contentList, "contentList", LookMode.Value);
            if (contentList == null) contentList = new List<string>();

            string oldVersionContents = string.Empty;
            Scribe_Values.Look(ref oldVersionContents, "contents");
            if (!string.IsNullOrEmpty(oldVersionContents)) AddThing(oldVersionContents);
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
        }
        public override string LabelMouseover
        {
            get
            {
                if (!contentList.Any()) return "Empty hole, something is wrong";

                var contentsString = FormatContentsString();
                return "BuryOnTheSpot_MouseoverText".Translate(contentsString, (TicksBeforeDespawn - age).ToStringTicksToPeriod(allowSeconds: false));
            }
        }

        public void AddThing(string content)
        {
            contentList.Add(content);
        }

        public override void TickRare()
        {
            base.TickRare();
            age += 250;
            if (age >= TicksBeforeDespawn)
            {
                DeSpawn();
            }
        }

        private string FormatContentsString()
        {
            var totalLength = 0;
            var totalShown = 0;
            foreach (var description in contentList)
            {
                if (totalLength + description.Length > MaxContentLength) break;
                totalLength += description.Length + 2;
                totalShown++;
            }
            return totalShown == contentList.Count
                ? new TaggedString(string.Join(", ", contentList.Take(totalShown)))
                : "BuryOnTheSpot_MoreContent".Translate(string.Join(", ", contentList.Take(totalShown)), contentList.Count - totalShown);
        }
    }
}
