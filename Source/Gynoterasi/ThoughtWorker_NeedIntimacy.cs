using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talonos.Monstergirls;
using Verse;

namespace Gynoterasi
{
    /*
    public class ThoughtWorker_NeedIntimacy : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            Need_Intimacy needIntimacy = p.needs.TryGetNeed<Need_Intimacy>();
            if (needIntimacy == null)
            {
                return ThoughtState.Inactive;
            }

            bool hasLover = false;
            bool hasWife = false;
            bool hasHusband = false;
            foreach (DirectPawnRelation dpr in p.relations.DirectRelations)
            {
                if (dpr.def == PawnRelationDefOf.Lover || dpr.def == PawnRelationDefOf.Fiance) { hasLover = true; }
                if (dpr.def == PawnRelationDefOf.Spouse && dpr.otherPawn.gender == Gender.Male) { hasHusband = true; }
                if (dpr.def == PawnRelationDefOf.Spouse && dpr.otherPawn.gender == Gender.Female) { hasWife = true; }
            }
            if (needIntimacy.CurLevel < Need_Intimacy.critical)
            {
                if (hasHusband) { return ThoughtState.ActiveAtStage(2); };
                if (hasWife) { return ThoughtState.ActiveAtStage(3); };
                if (hasLover) { return ThoughtState.ActiveAtStage(1); };
                return ThoughtState.ActiveAtStage(0);
            }
            else if (needIntimacy.CurLevel < Need_Intimacy.severe)
            {
                if (hasHusband) { return ThoughtState.ActiveAtStage(6); };
                if (hasWife) { return ThoughtState.ActiveAtStage(7); };
                if (hasLover) { return ThoughtState.ActiveAtStage(5); };
                return ThoughtState.ActiveAtStage(4);
            }
            else if (needIntimacy.CurLevel < Need_Intimacy.mild)
            {
                if (hasHusband) { return ThoughtState.ActiveAtStage(10); };
                if (hasWife) { return ThoughtState.ActiveAtStage(11); };
                if (hasLover) { return ThoughtState.ActiveAtStage(9); };
                return ThoughtState.ActiveAtStage(8);
            }
            else if (needIntimacy.CurLevel < Need_Intimacy.normal)
            {
                return ThoughtState.Inactive;
            }
            else
            {
                return ThoughtState.ActiveAtStage(12);
            }
        }
    }
    */
}
