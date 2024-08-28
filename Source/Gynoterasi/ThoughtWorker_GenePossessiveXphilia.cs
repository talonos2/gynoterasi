using Verse;

namespace Talonos.Monstergirls
{
    /*
    public class ThoughtWorker_GenePossessiveXphilia : ThoughtWorker
    {
        public virtual Gender TargetGender()
        {
            return Gender.None;
        }

        protected override ThoughtState CurrentStateInternal(Pawn pawn)
        {
            if (pawn.gender==TargetGender())
            {
                return ThoughtState.Inactive;
            }
            List<Pawn> collection = null;
            if (pawn.Map != null && pawn.Map.mapPawns != null)
            {
                collection = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
            }
            else if (pawn.IsCaravanMember())
            {
                collection = pawn.GetCaravan().pawns.InnerListForReading;
            }
            else
            {
                return ThoughtState.Inactive;
            }
                
            int myGender = 0;
            int targetGender = 0;
            foreach (Pawn otherPawn in collection)
            {
                if (otherPawn.gender==pawn.gender && otherPawn.RaceProps.Humanlike)
                {
                    myGender++;
                }
                if (otherPawn.gender == TargetGender() && otherPawn.RaceProps.Humanlike)
                {
                    targetGender++;
                }
            }
            if (targetGender==0)
            {
                return ThoughtState.ActiveAtStage(0); //No males
            }
            if (targetGender == 1&&myGender <= 6)
            {
                return ThoughtState.ActiveAtStage(1); // Trophy Male
            }
            float ratioOfMineToTarget = (float)targetGender / (targetGender+myGender);
            if (ratioOfMineToTarget < .1501f)
            {
                if (targetGender == 1)
                {
                    return ThoughtState.ActiveAtStage(2); // Single Trophy
                }
                else
                {
                    return ThoughtState.ActiveAtStage(3); // Scarce Trophies
                }
            }
            if (ratioOfMineToTarget < .225 || (targetGender == 2 && myGender == 6))
            {
                return ThoughtState.ActiveAtStage(4); // Trophy Males
            }
            if (ratioOfMineToTarget < .3501f) 
            {
                if (targetGender == 2)
                {
                    return ThoughtState.ActiveAtStage(5); // Two trophies (Only at 5-2)
                }
                else
                {
                    return ThoughtState.ActiveAtStage(6); // Many Trophies
                }
            }
            if (ratioOfMineToTarget < .5001f)
            {
                if (targetGender == 2)
                {
                    return ThoughtState.ActiveAtStage(7); // Two trophies! (2-4 to 2)
                }
                else
                {
                    return ThoughtState.ActiveAtStage(8); // Excessive Trophies
                }
            }
            if (ratioOfMineToTarget < .6667f)
            {
                return ThoughtState.ActiveAtStage(9); //Outnumbered by Trophies
            }
            if (ratioOfMineToTarget < .7501f)
            {
                return ThoughtState.ActiveAtStage(10); //Overwhelmed by Trophies
            }
            if (myGender > 1)
            {
                return ThoughtState.ActiveAtStage(11); //Are we the trophies?
            }
            else
            {
                return ThoughtState.ActiveAtStage(12); //Am I the trophy?
            }
        }
    }

    public class ThoughtWorker_PossessiveAndrophiliaMan : ThoughtWorker
    {
        protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
        {
            if (!p.RaceProps.Humanlike)
            {
                return false;
            }
            if (!RelationsUtility.PawnsKnowEachOther(p, other))
            {
                return false;
            }
            if (other.def != p.def)
            {
                return false;
            }
            if (other.gender != Gender.Male)
            {
                return false;
            }
            return true;
        }
    }

    public class ThoughtWorker_PossessiveGynophiliaWoman : ThoughtWorker
    {
        protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
        {
            if (!p.RaceProps.Humanlike)
            {
                return false;
            }
            if (!p.story.traits.HasTrait(TraitDefOf.DislikesMen))
            {
                return false;
            }
            if (!RelationsUtility.PawnsKnowEachOther(p, other))
            {
                return false;
            }
            if (other.def != p.def)
            {
                return false;
            }
            if (other.gender != Gender.Male)
            {
                return false;
            }
            return true;
        }
    }
    */
}
