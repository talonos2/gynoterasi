using System.Collections.Generic;
using Verse;

namespace Talonos.Monstergirls
{
    /*
    public class Hediff_PossessiveXPhilia : Hediff
    {
        public virtual Gender GenderTarget()
        {
            return Gender.None;
        }

        public override void Tick()
        {
            base.Tick();
            if (pawn.IsHashIntervalTick(3500))
            {
                if (GenderTarget()==Gender.None)
                {
                    Log.Error("Warning, genderTarget did not get assigned!");
                }
                if (pawn.gender == GenderTarget())
                {
                    Severity = .20f; //Turns it off.
                    return;
                }
                List<Pawn> collection = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
                int myGender = 0;
                int targetGender = 0;
                foreach (Pawn otherPawn in collection)
                {
                    if (otherPawn.gender == pawn.gender && otherPawn.RaceProps.Humanlike)
                    {
                        myGender++;
                    }
                    if (otherPawn.gender == GenderTarget() && otherPawn.RaceProps.Humanlike)
                    {
                        targetGender++;
                    }
                }
                Severity = (float)targetGender / (float)(targetGender + myGender);

                //Special cases for low pop colonies.
                if ((targetGender == 1 && myGender <= 6) || (targetGender == 2 && myGender == 6))
                {
                    Severity = .20f; //Turns it off.
                }
            }
        }
    }

    public class Hediff_PossessiveGynophilia : Hediff_PossessiveXPhilia
    {
        public override Gender GenderTarget()
        {
        return Gender.Female;
        }
    }

    public class Hediff_PossessiveAndrophilia : Hediff_PossessiveXPhilia
    {
        public override Gender GenderTarget()
        {
            return Gender.Male;
        }
    }
    */
}
