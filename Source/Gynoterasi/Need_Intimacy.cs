// RimWorld.Gene_Hemogen
using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace Gynoterasi
{
    /*
    public class Need_Intimacy : Need
    {
        public const float FallPerDay = 1f / 15f;
        private const float MinAgeForNeed = 16f;

        private const float sexIntimacyMultiplier = 0.89f;        //Sex is best at meeing this need. Thrice-nightly sex averages you around 95.5% over the course of a day.
        private const float sexIntimacyFlat = 0.016f;
        private const float flirtIntimacyModifier = 0.9f;
        private const float DeepTalkIntimacyMultiplier = 0.975f;
        private const float nuzzleIntimacyModifier = 0.96f;
        private const float watchingSexIntimacyMultiplierPer150 = .994f;
        private const float breastfeedingIntimacyMultiplierPer150 = .993f;

        internal const float critical = .05f;
        internal const float severe = .225f;
        internal const float mild = .4f;
        internal const float normal = .95f;

        private bool wasHavingSex = false;
        private float opinionOfMyParther = -999;
        private float xphiliaModifierOfPartner = 1;

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
                    xphiliaModifierOfPartner = IntimacyHelper.GetXphiliaModifier(pawn, pawn.CurJob.targetA.Pawn);
                }
                else
                {
                    //Whatever you're having sex with isn't a pawn. No intimacy there, minimal returns.
                    opinionOfMyParther = -150;
                    xphiliaModifierOfPartner = 1;
                }
                return; //Might as well. No point in reducing intimacy if you're *actively having* sex.
            }
            else if (wasHavingSex)
            {
                //Successful!
                wasHavingSex = false;

                float relationshipMultiplier = (opinionOfMyParther + 100f) / 400f + .5f;
                CurLevel = 1 - ((1 - CurLevel) * (1 - (1 - sexIntimacyMultiplier) * relationshipMultiplier));
                CurLevel += sexIntimacyFlat * relationshipMultiplier;
                return;
            }
            else if (pawn.CurJob.def == JobDefOf.Breastfeed)
            {
                //Did some research, apparantly this is a great source of oxytocin.  Kinda worried perverts
                //might think the pawn is getting sexual satisfaction from breastfeeding, but nope. Just Oxytocin.
                CurLevel = 1 - ((1 - CurLevel) * breastfeedingIntimacyMultiplierPer150);
            }
            if (!IsFrozen)
            {
                if (IsNearPeopleHavingSex(out float bestRelationship, out float xphiliaModifier))
                {
                    float relationshipMultiplier = (bestRelationship + 100f) / 400f + .5f;
                    float vulnerabilityMultiplier = 1 + .3f * IntimacyHelper.GetVulnerabilityBonus(pawn, pawn);
                    CurLevel = 1 - ((1 - CurLevel) * (1 - (1 - watchingSexIntimacyMultiplierPer150) * vulnerabilityMultiplier * relationshipMultiplier));
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
            if (pawn.Map==null||pawn.Map.mapPawns==null)
            {
                return false;
            }
            List<Pawn> collection = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
            foreach (Pawn otherPawn in collection)
            {
                if (otherPawn.Spawned && InteractionUtility.IsGoodPositionForInteraction(pawn, otherPawn)&&otherPawn.CurJobDef==JobDefOf.Lovin)
                {
                    bestRelationship = Mathf.Max(bestRelationship, pawn.relations.OpinionOf(otherPawn));
                    toReturn = true;
                    if (pawn.genes.HasGene(DefDatabase<GeneDef>.GetNamed("GenePossessiveAndrophilia")))
                    {
                        detracted = true;
                        if (otherPawn.gender==Gender.Male)
                        {
                            enhanced = true;
                        }
                    }
                    if (pawn.genes.HasGene(DefDatabase<GeneDef>.GetNamed("GenePossessiveGynophilia")))
                    {
                        detracted = true;
                        if (otherPawn.gender == Gender.Female)
                        {
                            enhanced = true;
                        }
                    }
                }
            }
            xphiliaModifier = (enhanced ? 1.33f : (detracted ? .67f : 1));
            return toReturn;
        }

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
            if (CurLevel < critical)             { return 2.5f; }
            if (CurLevel < severe)               { return 2f; }
            if (CurLevel < mild)                 { return 1.5f; }
            if (CurLevel < (mild + normal / 2f)) { return 1.3f; }
            if (CurLevel < normal)               { return 1.15f; }
            //If "Dyadically calm": Not wanting sex is just such a novel experience they just want to enjoy the peace for a while
            return .25f;
        }

        internal void NotifyHadDeepTalk(int opinionOfTalker, float vulnerabilityMultiplier, float xphiliaModifier)
        {
            float relationshipMultiplier = (opinionOfTalker + 100f) / 400f + .5f;
            CurLevel = 1 - ((1 - CurLevel) * (1 - (1 - DeepTalkIntimacyMultiplier) * relationshipMultiplier*vulnerabilityMultiplier));
        }

        internal void NotifyInvolvedInRomance(int opinionOfPartner, float vulnerabilityMultiplier, float xphiliaModifier)
        {
            float relationshipMultiplier = (opinionOfPartner + 100f) / 400f + .5f;
            CurLevel = 1 - ((1 - CurLevel) * (1 - (1 - flirtIntimacyModifier) * relationshipMultiplier * vulnerabilityMultiplier));
        }

        internal void NotifyGotNuzzled(bool isBonded)
        {
            float relationshipMultiplier = ((isBonded?100:0) + 100f) / 400f + .5f;
            CurLevel = 1 - ((1 - CurLevel) * (1 - (1 - nuzzleIntimacyModifier) * relationshipMultiplier));
        }
    }

    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        private static readonly Type patchType = typeof(HarmonyPatches);

        static HarmonyPatches()
        {
            Harmony harmony = new Harmony(id: "rimworld.talonos.monstergirls.main");
            harmony.Patch(AccessTools.Method(typeof(InteractionWorker_RomanceAttempt), nameof(InteractionWorker_RomanceAttempt.SuccessChance)), postfix: new HarmonyMethod(patchType, nameof(MG_RomanceSuccessChancePostfix)));
            harmony.Patch(AccessTools.Method(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.GetLovinMtbHours)), postfix: new HarmonyMethod(patchType, nameof(MG_GetLovinMTBHoursPostfix)));
            harmony.Patch(AccessTools.Method(typeof(JobDriver_Lovin), "GenerateRandomMinTicksToNextLovin"), postfix: new HarmonyMethod(patchType, nameof(MG_GenerateRandomMinTicksToNextLovinPostfix)));
            harmony.Patch(AccessTools.Method(typeof(InteractionWorker_RomanceAttempt), nameof(InteractionWorker_RomanceAttempt.RandomSelectionWeight)), postfix: new HarmonyMethod(patchType, nameof(MG_RomanceAttemptRandomSelectionWeightPostfix)));
            harmony.Patch(AccessTools.Method(typeof(Pawn_InteractionsTracker), nameof(Pawn_InteractionsTracker.TryInteractWith)), postfix: new HarmonyMethod(patchType, nameof(MG_TryInteractWithPostfix)));
            harmony.Patch(AccessTools.Method(typeof(Pawn_InteractionsTracker), nameof(Pawn_InteractionsTracker.SocialFightChance)), postfix: new HarmonyMethod(patchType, nameof(MG_SocialFightChancePostfix)));
            harmony.Patch(AccessTools.Method(typeof(InteractionWorker_RomanceAttempt), nameof(InteractionWorker_RomanceAttempt.Interacted)), postfix: new HarmonyMethod(patchType, nameof(MG_RomanceInteractedPostfix)));
            //harmony.Patch(AccessTools.Method(typeof(PregnancyUtility), nameof(PregnancyUtility.GetInheritedGenes), new Type[] { typeof(Pawn), typeof(Pawn), typeof(bool) }), postfix: new HarmonyMethod(patchType, nameof(MG_GetInheritedGenesPostfix)));
            harmony.Patch(AccessTools.Method(typeof(InteractionWorker_Nuzzle), "AddNuzzledThought"), postfix: new HarmonyMethod(patchType, nameof(MG_AddNuzzledThoughtPostfix)));
        }

        public static void MG_RomanceSuccessChancePostfix(Pawn initiator, Pawn recipient, ref float __result)
        {
            Need_Intimacy need_Intimacy = initiator.needs.TryGetNeed<Need_Intimacy>();
            if (need_Intimacy != null)
            {
                //Log.Message("VFMG: Romance Success Postfix. Before: " + __result);
                if (recipient.story == null || recipient.story.traits == null || !recipient.story.traits.HasTrait(TraitDefOf.Masochist))
                {
                    //Pawn is more demanding and aggressive in its attempts to force romance, turning off most people.
                    __result *= need_Intimacy.getRomanceFailureChance();
                }
                else
                {
                    //Masochists find this attractive, however.
                    __result /= need_Intimacy.getRomanceFailureChance();
                }
            }
            __result = Mathf.Clamp01(__result);
            Log.Message("                               After:  " + __result);
        }

        public static void MG_GetLovinMTBHoursPostfix(Pawn pawn, Pawn partner, ref float __result)
        {
            //If I'm a DHR pawn, what's my intimacy need?
            Need_Intimacy need_Intimacy = pawn.needs.TryGetNeed<Need_Intimacy>();
            if (need_Intimacy != null)
            {
                {
                    __result = __result/need_Intimacy.getMTBLovinAdjustment();
                }
            }

            //If my partner is DHR pawn, what's her intimacy need?
            need_Intimacy = partner.needs.TryGetNeed<Need_Intimacy>();
            if (need_Intimacy != null)
            {
                {
                    __result = __result/need_Intimacy.getMTBLovinAdjustment();
                }
            }
        }

        public static void MG_GenerateRandomMinTicksToNextLovinPostfix(Pawn pawn, ref int __result)
        {
            Need_Intimacy need_Intimacy = pawn.needs.TryGetNeed<Need_Intimacy>();
            if (need_Intimacy != null)
            {
                {
                    __result = (int)(__result/need_Intimacy.getMTBLovinAdjustment());
                }
            }
        }

        public static void MG_RomanceAttemptRandomSelectionWeightPostfix(InteractionWorker_RomanceAttempt __instance, Pawn initiator, Pawn recipient, ref float __result)
        {
            Need_Intimacy need_Intimacy = initiator.needs.TryGetNeed<Need_Intimacy>();
            if (need_Intimacy != null)
            {
                //Undo the penalty for being female. Warning, if you already have via some other mod, female DHR pawns will be nearly 7x as likely to insttigate
                //romance as the average male. Maybe I'll add compatibility some day.
                if (initiator.gender == Gender.Female && !initiator.story.traits.HasTrait(TraitDefOf.Gay))
                {
                    __result /= 0.15f;
                }
                __result *= need_Intimacy.GetActingOutMultiplier();
            }
        }

        public static void MG_TryInteractWithPostfix(Pawn_InteractionsTracker __instance, Pawn recipient, InteractionDef intDef, Pawn ___pawn, bool __result)
        {
            if (__result && intDef == InteractionDefOf.DeepTalk)
            {
                Need_Intimacy need_Intimacy = ___pawn.needs.TryGetNeed<Need_Intimacy>();
                if (need_Intimacy != null)
                {
                    //Log.Message("VFMG: Deep Talk. Before: " + need_Intimacy.CurLevel);
                    float vulnerabilityBonus = 1 + IntimacyHelper.GetVulnerabilityBonus(___pawn, ___pawn) + IntimacyHelper.GetVulnerabilityBonus(___pawn, recipient);
                    need_Intimacy.NotifyHadDeepTalk(___pawn.relations.OpinionOf(recipient), vulnerabilityBonus, IntimacyHelper.GetXphiliaModifier(___pawn, recipient));
                    //Log.Message("                 After:  " + need_Intimacy.CurLevel);
                }

                need_Intimacy = recipient.needs.TryGetNeed<Need_Intimacy>();
                if (need_Intimacy != null)
                {
                    //Log.Message("VFMG: Deep Talk. Before: " + need_Intimacy.CurLevel);
                    float vulnerabilityBonus = 1 + IntimacyHelper.GetVulnerabilityBonus(recipient, recipient) + IntimacyHelper.GetVulnerabilityBonus(recipient, ___pawn);
                    need_Intimacy.NotifyHadDeepTalk(recipient.relations.OpinionOf(___pawn), vulnerabilityBonus, IntimacyHelper.GetXphiliaModifier(___pawn, recipient));
                    //Log.Message("                 After:  " + need_Intimacy.CurLevel);
                }
            }
        }

        public static void MG_SocialFightChancePostfix(Pawn_InteractionsTracker __instance, InteractionDef interaction, Pawn initiator, Pawn ___pawn, float __result)
        {
            if (__result>0)
            {
                Need_Intimacy need_Intimacy = ___pawn.needs.TryGetNeed<Need_Intimacy>();
                if (need_Intimacy != null)
                {
                    __result = Mathf.Clamp01(__result * need_Intimacy.GetActingOutMultiplier());
                }
            }
        }

        public static void MG_RomanceInteractedPostfix(InteractionWorker_RomanceAttempt __instance, Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, ref string letterText, ref string letterLabel, ref LetterDef letterDef, ref LookTargets lookTargets)
        {

            Need_Intimacy need_Intimacy = initiator.needs.TryGetNeed<Need_Intimacy>();
            if (need_Intimacy != null)
            {
                initiator.relations.romanceEnableTick = Find.TickManager.TicksGame + 120000; //You can manually assign a flirt every other day instead of once a quadrum.
                float vulnerabilityBonus = 1 + IntimacyHelper.GetVulnerabilityBonus(initiator, initiator) + IntimacyHelper.GetVulnerabilityBonus(initiator, recipient);
                need_Intimacy.NotifyInvolvedInRomance(initiator.relations.OpinionOf(recipient), vulnerabilityBonus, IntimacyHelper.GetXphiliaModifier(initiator,recipient));
            }

            need_Intimacy = recipient.needs.TryGetNeed<Need_Intimacy>();
            if (need_Intimacy != null)
            {
                float vulnerabilityBonus = 1 + IntimacyHelper.GetVulnerabilityBonus(recipient, recipient) + IntimacyHelper.GetVulnerabilityBonus(recipient, initiator);
                need_Intimacy.NotifyInvolvedInRomance(recipient.relations.OpinionOf(initiator), vulnerabilityBonus, IntimacyHelper.GetXphiliaModifier(initiator, recipient));
            }
        }

        public static void MG_GetInheritedGenesPostfix(Pawn father, Pawn mother, ref bool success, ref List<GeneDef> __result)
        {
            if (father.genes.HasGene(DefDatabase<GeneDef>.GetNamed("DominantSperm")) && !mother.genes.HasGene(DefDatabase<GeneDef>.GetNamed("DominantEggs")))
            {
                success = true;
                __result.Clear();
                foreach (Gene gene in father.genes.Endogenes)
                {
                    __result.Add(gene.def);
                }
                return;
            }
            if (mother.genes.HasGene(DefDatabase<GeneDef>.GetNamed("DominantEggs")) && !father.genes.HasGene(DefDatabase<GeneDef>.GetNamed("DominantSperm")))
            {
                success = true;
                __result.Clear();
                foreach (Gene gene in mother.genes.Endogenes)
                {
                    __result.Add(gene.def);
                }
                return;
            }
        }
        public static void MG_AddNuzzledThoughtPostfix(InteractionWorker_Nuzzle __instance, Pawn initiator, Pawn recipient)
        {
            //Todo: Maybe one day I'll make it so Gynoterasic colonists can nuzzle others. (Seems appropriate to the source inspiration.) If so:
            // - Initiator also gets intimacy bonus.
            // - Check for XPhilia, vulnerability, and opinion.
            if (recipient.needs.mood != null)
            {
                Need_Intimacy intimacy = recipient.needs.TryGetNeed<Need_Intimacy>();
                if (intimacy != null)
                {
                    bool isBonded = false;
                    if (initiator.RaceProps.IsFlesh)
                    {
                        foreach (DirectPawnRelation relation in initiator.relations.DirectRelations)
                        {
                            if (relation.def == PawnRelationDefOf.Bond && relation.otherPawn==initiator)
                            {
                                isBonded = true;
                            }
                        }
                    }
                    intimacy.NotifyGotNuzzled(isBonded);
                }
            }
        }
    }
    //Intimacy on deep talk TEST
    // - More if you are naked, more if target is naked. TEST
    //Intimacy on romance attempt
    // - More if you are naked.
    */
}