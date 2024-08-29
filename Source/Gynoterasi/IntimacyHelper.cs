using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Gynoterasi
{
    public static class IntimacyHelper
    {
        internal static float GetXphiliaModifier(Pawn possessor, Pawn target)
        {
            if (possessor.genes.HasActiveGene(DefDatabase<GeneDef>.GetNamed("GT_GenePossessiveAndrophilia")))
            {
                if (target.gender == Gender.Male)
                {
                    return 1.33f;
                }
                else
                {
                    return 0.67f;
                }
            }
            if (possessor.genes.HasActiveGene(DefDatabase<GeneDef>.GetNamed("GT_GenePossessiveGynophilia")))
            {
                if (target.gender == Gender.Male)
                {
                    return 1.33f;
                }
                else
                {
                    return 0.67f;
                }
            }
            return 1;
        }

        public static float GetVulnerabilityBonus(Pawn looked, Pawn target)
        {
            int sexualizedParts = 0;
            int uncoveredParts = 0;
            if (GroinSexualized(looked, target))
            {
                sexualizedParts++;
                if (ThoughtWorker_Precept_GroinUncovered.HasUncoveredGroin(target)) { uncoveredParts++; }
            }
            if (ChestSexualized(looked, target))
            {
                sexualizedParts++;
                if (ThoughtWorker_Precept_GroinOrChestUncovered.HasUncoveredGroinOrChest(target) &&
                   !ThoughtWorker_Precept_GroinUncovered.HasUncoveredGroin(target)) { uncoveredParts++; }
            }
            if (HairSexualized(looked, target))
            {
                sexualizedParts++;
                if (ThoughtWorker_Precept_GroinChestOrHairUncovered.HasUncoveredGroinChestOrHair(target) &&
                   !ThoughtWorker_Precept_GroinOrChestUncovered.HasUncoveredGroinOrChest(target)) { uncoveredParts++; }
            }
            if (FaceSexualized(looked, target))
            {
                sexualizedParts++;
                if (ThoughtWorker_Precept_GroinChestHairOrFaceUncovered.HasUncoveredGroinChestHairOrFace(target) &&
                   !ThoughtWorker_Precept_GroinChestOrHairUncovered.HasUncoveredGroinChestOrHair(target)) { uncoveredParts++; }
            }
            if (sexualizedParts == 0)
            {
                return 0;
            }
            return (float)uncoveredParts / (float)sexualizedParts;
        }

        public static bool GroinSexualized(Pawn pawn, Pawn target)
        {
            if (pawn.story.traits.HasTrait(TraitDefOf.Nudist))
            {
                return false;
            }
            if (ModsConfig.IdeologyActive)
            {
                if (target.gender.Equals(Gender.Male))
                {
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Male_UncoveredGroinChestHairOrFaceDisapproved"))) { return true; }
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Male_UncoveredGroinChestOrHairDisapproved"))) { return true; }
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Male_UncoveredGroinOrChestDisapproved"))) { return true; }
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Male_UncoveredGroinDisapproved"))) { return true; }
                }
                else if (target.gender.Equals(Gender.Female))
                {
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Female_UncoveredGroinChestHairOrFaceDisapproved"))) { return true; }
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Female_UncoveredGroinChestOrHairDisapproved"))) { return true; }
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Female_UncoveredGroinOrChestDisapproved"))) { return true; }
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Female_UncoveredGroinDisapproved"))) { return true; }
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        public static bool ChestSexualized(Pawn pawn, Pawn target)
        {
            if (pawn.story.traits.HasTrait(TraitDefOf.Nudist))
            {
                return false;
            }
            if (ModsConfig.IdeologyActive)
            {
                if (target.gender.Equals(Gender.Male))
                {
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Male_UncoveredGroinChestHairOrFaceDisapproved"))) { return true; }
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Male_UncoveredGroinChestOrHairDisapproved"))) { return true; }
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Male_UncoveredGroinOrChestDisapproved"))) { return true; }
                }
                else if (target.gender.Equals(Gender.Female))
                {
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Female_UncoveredGroinChestHairOrFaceDisapproved"))) { return true; }
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Female_UncoveredGroinChestOrHairDisapproved"))) { return true; }
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Female_UncoveredGroinOrChestDisapproved"))) { return true; }
                }
            }
            else
            {
                return target.gender == Gender.Female;
            }
            return false;
        }
        public static bool HairSexualized(Pawn pawn, Pawn target)
        {
            if (ModsConfig.IdeologyActive)
            {
                if (pawn.story.traits.HasTrait(TraitDefOf.Nudist))
                {
                    return false;
                }
                if (target.gender.Equals(Gender.Male))
                {
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Male_UncoveredGroinChestHairOrFaceDisapproved"))) { return true; }
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Male_UncoveredGroinChestOrHairDisapproved"))) { return true; }
                }
                else if (target.gender.Equals(Gender.Female))
                {
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Female_UncoveredGroinChestHairOrFaceDisapproved"))) { return true; }
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Female_UncoveredGroinChestOrHairDisapproved"))) { return true; }
                }
            }
            return false;
        }
        public static bool FaceSexualized(Pawn pawn, Pawn target)
        {
            if (ModsConfig.IdeologyActive)
            {
                if (pawn.story.traits.HasTrait(TraitDefOf.Nudist))
                {
                    return false;
                }
                if (target.gender.Equals(Gender.Male))
                {
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Male_UncoveredGroinChestHairOrFaceDisapproved"))) { return true; }
                }
                else if (target.gender.Equals(Gender.Female))
                {
                    if (pawn.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamed("Nudity_Female_UncoveredGroinChestHairOrFaceDisapproved"))) { return true; }
                }
            }
            return false;
        }

        /// <summary>
        /// Multiplies the *missing* part of a value from 0% to 100% by a given value. For instance, multiplying the *missing* part of 60* by 75% returns
        /// 70%, because you're missing 40%, which you multiply by 75%, giving 30%, and if you're missing 30% you must have 70%.
        /// </summary>
        /// <param name="startingPercent">The start</param>
        /// <param name="percentToMultiplyMissingPartBy"></param>
        /// <returns></returns>
        public static float MultiplyAmountMissingBy(float startingPercent, float percentToMultiplyMissingPartBy)
        {
            return 1 - ((1 - startingPercent) * percentToMultiplyMissingPartBy);
        }
    }
}
