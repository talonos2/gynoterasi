// RimWorld.Gene_Hemogen
using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace Gynoterasi
{
    public class Need_Intimacy : Need
    {
        public const float FallPerDay = 1f / 15f;
        private const float MinAgeForNeed = 16f;

        private const float sexIntimacyMultiplier = 0.89f;        //Sex is best at meeting this need. Thrice-nightly sex averages you around 95.5% over the course of a day.
        private const float sexIntimacyFlat = 0.016f;
        //private const float flirtIntimacyModifier = 0.9f;
        //private const float DeepTalkIntimacyMultiplier = 0.975f;
        //private const float nuzzleIntimacyModifier = 0.96f;
        private const float watchingSexIntimacyMultiplierPer150 = .994f;
        private const float breastfeedingIntimacyMultiplierPer150 = .993f;

        internal const float critical = .05f;
        internal const float severe = .225f;
        internal const float mild = .4f;
        internal const float normal = .95f;

        private bool wasHavingSex = false;
        float opinionOfMyParther = -999;
        float xphiliaModifierTowardsPartner = 1;

        protected override bool IsFrozen
        {
            get
            {
                if ((float)pawn.ageTracker.AgeBiologicalYears < MinAgeForNeed)
                {
                    return true;
                }
                return base.IsFrozen;
            }
        }

        public override bool ShowOnNeedList
        {
            get
            {
                if ((float)pawn.ageTracker.AgeBiologicalYears < MinAgeForNeed)
                {
                    return false;
                }
                return base.ShowOnNeedList;
            }
        }

        public Need_Intimacy(Pawn newPawn)
            : base(newPawn)
        {
            threshPercents = new List<float> { critical, severe, mild, normal };
        }

        public override void NeedInterval()
        {

            if (pawn.CurJob.def == JobDefOf.Lovin)
            {
                wasHavingSex = true;
                if (pawn.CurJob.targetA.Pawn != null)
                {
                    opinionOfMyParther = pawn.relations.OpinionOf(pawn.CurJob.targetA.Pawn);
                    xphiliaModifierTowardsPartner = IntimacyHelper.GetXphiliaModifier(pawn, pawn.CurJob.targetA.Pawn);
                }
                else
                {
                    //Whatever you're having sex with isn't a pawn. No intimacy there, minimal returns.
                    opinionOfMyParther = -150;
                    xphiliaModifierTowardsPartner = 1;
                }
                return; //Might as well. No point in reducing intimacy if you're *actively having* sex.
            }
            else if (wasHavingSex)
            {
                //Successful!
                wasHavingSex = false; //TODO: Inturrupted sex also counts; it shouldn't. (Or at least should count partial.)

                float relationshipMultiplier = (opinionOfMyParther + 100f) / 400f + .5f;
                CurLevel = 1 - ((1 - CurLevel) * (1 - (1 - sexIntimacyMultiplier) * relationshipMultiplier));
                CurLevel += sexIntimacyFlat * relationshipMultiplier;
                return;
            }
            else if (pawn.CurJob.def == JobDefOf.Breastfeed)
            {
                //Did some research, apparantly this is a great source of oxytocin.  Kinda worried perverts
                //might think the pawn is getting sexual satisfaction from breastfeeding, but nope. Just Oxytocin.
                CurLevel = IntimacyHelper.MultiplyAmountMissingBy(CurLevel, breastfeedingIntimacyMultiplierPer150);
            }
            if (!IsFrozen)
            {
                if (IsNearPeopleHavingSex(out float bestRelationship, out float xphiliaModifier))
                {
                    float relationshipMultiplier = (bestRelationship + 100f) / 400f + .5f;
                    float vulnerabilityMultiplier = 1 + .3f * IntimacyHelper.GetVulnerabilityBonus(pawn, pawn); //Bonus if viewer has sexualized parts uncovered. (Participants are *assumed* to be uncovered.)
                    CurLevel = IntimacyHelper.MultiplyAmountMissingBy(CurLevel, IntimacyHelper.MultiplyAmountMissingBy(watchingSexIntimacyMultiplierPer150, vulnerabilityMultiplier * relationshipMultiplier));
                }
                else
                {
                    CurLevel -= FallPerDay / (60000 / 150);
                }
            }
        }

        private bool IsNearPeopleHavingSex(out float bestRelationship, out float xphiliaModifier)
        {
            bestRelationship = -200;
            xphiliaModifier = 1;
            bool toReturn = false;
            bool enhanced = false;
            bool detracted = false;
            if (pawn.Map == null || pawn.Map.mapPawns == null)
            {
                return false;
            }
            List<Pawn> collection = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
            foreach (Pawn otherPawn in collection)
            {
                if (otherPawn.Spawned && InteractionUtility.IsGoodPositionForInteraction(pawn, otherPawn) && otherPawn.CurJobDef == JobDefOf.Lovin)
                {
                    bestRelationship = Mathf.Max(bestRelationship, pawn.relations.OpinionOf(otherPawn));
                    toReturn = true;
                    if (pawn.genes.HasActiveGene(DefDatabase<GeneDef>.GetNamed("GenePossessiveAndrophilia")))
                    {
                        detracted = true;
                        if (otherPawn.gender == Gender.Male)
                        {
                            enhanced = true;
                        }
                    }
                    if (pawn.genes.HasActiveGene(DefDatabase<GeneDef>.GetNamed("GenePossessiveGynophilia")))
                    {
                        detracted = true;
                        if (otherPawn.gender == Gender.Female)
                        {
                            enhanced = true;
                        }
                    }
                }
            }
            xphiliaModifier = (enhanced ? 1.333f : (detracted ? .667f : 1));
            return toReturn;
        }

        /*
        internal float getRomanceFailureChance()
        {
            if (CurLevel < critical)
            {
                return .33f;
            }
            if (CurLevel < severe)
            {
                return .5f;
            }
            if (CurLevel < mild)
            {
                return .75f;
            }
            if (CurLevel < (mild + normal / 2f))
            {
                return 1f;
            }
            if (CurLevel < normal)
            {
                return 1.1f;
            }
            return 1.15f;
        }

        internal float getMTBLovinAdjustment()
        {
            if (CurLevel < critical)
            {
                return 2.5f;
            }
            if (CurLevel < severe)
            {
                return 2f;
            }
            if (CurLevel < mild)
            {
                return 1.5f;
            }
            if (CurLevel < (mild + normal / 2f))
            {
                return 1.3f;
            }
            if (CurLevel < normal)
            {
                return 1.15f;
            }
            //If "Dyadically calm": Not wanting sex is just such a novel experience they just want to enjoy the peace for a while
            return .25f;
        }

        internal float GetActingOutMultiplier()
        {
            if (CurLevel < critical) { return 2.5f; }
            if (CurLevel < severe) { return 2f; }
            if (CurLevel < mild) { return 1.5f; }
            if (CurLevel < (mild + normal / 2f)) { return 1.3f; }
            if (CurLevel < normal) { return 1.15f; }
            //If "Dyadically calm": Not wanting sex is just such a novel experience they just want to enjoy the peace for a while
            return .25f;
        }

        internal void NotifyHadDeepTalk(int opinionOfTalker, float vulnerabilityMultiplier, float xphiliaModifier)
        {
            float relationshipMultiplier = (opinionOfTalker + 100f) / 400f + .5f;
            CurLevel = 1 - ((1 - CurLevel) * (1 - (1 - DeepTalkIntimacyMultiplier) * relationshipMultiplier * vulnerabilityMultiplier));
        }

        internal void NotifyInvolvedInRomance(int opinionOfPartner, float vulnerabilityMultiplier, float xphiliaModifier)
        {
            float relationshipMultiplier = (opinionOfPartner + 100f) / 400f + .5f;
            CurLevel = 1 - ((1 - CurLevel) * (1 - (1 - flirtIntimacyModifier) * relationshipMultiplier * vulnerabilityMultiplier));
        }

        internal void NotifyGotNuzzled(bool isBonded)
        {
            float relationshipMultiplier = ((isBonded ? 100 : 0) + 100f) / 400f + .5f;
            CurLevel = 1 - ((1 - CurLevel) * (1 - (1 - nuzzleIntimacyModifier) * relationshipMultiplier));
        }
        */
    }


    //Intimacy on deep talk TEST
    // - More if you are naked, more if target is naked. TEST
    //Intimacy on romance attempt
    // - More if you are naked.
}