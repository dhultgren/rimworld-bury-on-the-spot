using HarmonyLib;
using System.Reflection;
using Verse;

namespace BuryOnTheSpot
{
    [StaticConstructorOnStartup]
    public class BuryOnTheSpot
    {
        static BuryOnTheSpot()
        {
            new Harmony("BuryOnTheSpot").PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
