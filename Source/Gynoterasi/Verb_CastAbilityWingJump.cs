using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using static Verse.PawnCapacityUtility;

namespace Gynoterasi
{
    /*
    public class Verb_CastAbilityWingJump : Verb_CastAbility
    {
        protected override float EffectiveRange
        {
            get
            {
                //Todo: This might be laggy, because the original function cached this and we never do. Why not? Because I don't know exactly when the cache is
                //dirty, and we want this value to chance every time the pawns mass (inc. equipment), body type, or arms change.
                float range = 21.14375f;
                //Same calculation as manipulation, but with no reduction due to consiousness.
                float armsEfficiency = CalculateFlightEfficiency(CasterPawn.health.hediffSet, BodyPartTagDefOf.ManipulationLimbCore, BodyPartTagDefOf.ManipulationLimbSegment, BodyPartTagDefOf.ManipulationLimbDigit, 0.8f, out float functionalPercentage, null);
                float bodyTypeModifier = 1;
                BodyTypeDef bodyTypeDef = CasterPawn.story.bodyType;
                if (bodyTypeDef == BodyTypeDefOf.Fat) { bodyTypeModifier = .7f; }
                if (bodyTypeDef == BodyTypeDefOf.Hulk) { bodyTypeModifier = .85f; }
                if (bodyTypeDef == BodyTypeDefOf.Thin) { bodyTypeModifier = 1.15f; }
                float mass = CasterPawn.GetStatValue(StatDefOf.Mass);
                float massModifier = 48 / mass; //(The mass of a naked pawn with the x0.8 multiplier from Wing Arms applied.
                //Modifier for body size is built into the mass adjustment in body size.
                //Modifier for child vs adult also built into the age category math adjustments.
                //TODO: Does a thing a pawn's carrying add to its mass? If not, carried things like babies, rescued colonists, etc should be added to the mass.
                return range * armsEfficiency * bodyTypeModifier * massModifier;
            }
        }

        public static float CalculateFlightEfficiency(HediffSet diffSet, BodyPartTagDef limbCoreTag, BodyPartTagDef limbSegmentTag, BodyPartTagDef limbDigitTag, float appendageWeight, out float functionalPercentage, List<CapacityImpactor> impactors)
        {
            BodyDef body = diffSet.pawn.RaceProps.body;
            float sumEfficiencyOfParts = 0f;
            float leastEfficientPart = 1000;
            int numberOfParts = 0;
            int numberOfNonZeroParts = 0;
            foreach (BodyPartRecord item in body.GetPartsWithTag(limbCoreTag))
            {
                float efficiencyOfThisPart = CalculateImmediatePartEfficiencyAndRecord(diffSet, item, impactors);
                foreach (BodyPartRecord connectedPart in item.GetConnectedParts(limbSegmentTag))
                {
                    efficiencyOfThisPart *= CalculateImmediatePartEfficiencyAndRecord(diffSet, connectedPart, impactors);
                }

                if (item.HasChildParts(limbDigitTag))
                {
                    efficiencyOfThisPart = Mathf.Lerp(efficiencyOfThisPart, efficiencyOfThisPart * item.GetChildParts(limbDigitTag).Average((BodyPartRecord digitPart) => CalculateImmediatePartEfficiencyAndRecord(diffSet, digitPart, impactors)), appendageWeight);
                }
                if (efficiencyOfThisPart < leastEfficientPart)
                {
                    leastEfficientPart = efficiencyOfThisPart;
                }
                sumEfficiencyOfParts += efficiencyOfThisPart;
                numberOfParts++;
                if (efficiencyOfThisPart > 0f)
                {
                    numberOfNonZeroParts++;
                }
            }

            if (numberOfParts == 0)
            {
                functionalPercentage = 0f;
                return 0f;
            }

            functionalPercentage = (float)numberOfNonZeroParts / (float)numberOfParts;
            //Log.Warning("LeastEfficientPart: " + leastEfficientPart+", SumEfficiencyOfParts: "+sumEfficiencyOfParts+", numberOfParts: "+numberOfParts+", total: "+ (leastEfficientPart + (sumEfficiencyOfParts / (float)numberOfParts)) / 2f);
            return (leastEfficientPart+(sumEfficiencyOfParts / (float)numberOfParts))/2f;
        }

        protected override bool TryCastShot()
        {
            if (base.TryCastShot())
            {
                return JumpUtility.DoJump(CasterPawn, currentTarget, base.ReloadableCompSource, verbProps);
            }
            return false;
        }

        public override void OnGUI(LocalTargetInfo target)
        {
            //Change CanHitTarget
            if (CanHitTarget(target) && JumpUtility.ValidJumpTarget(caster.Map, target.Cell))
            {
                base.OnGUI(target);
            }
            else
            {
                GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
            }
        }

        public override void OrderForceTarget(LocalTargetInfo target)
        {
            JumpUtility.OrderJump(CasterPawn, target, this, EffectiveRange);
        }

        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            if (caster == null)
            {
                return false;
            }
            if (!CanHitTarget(target) || !JumpUtility.ValidJumpTarget(caster.Map, target.Cell))
            {
                return false;
            }
            if (!ReloadableUtility.CanUseConsideringQueuedJobs(CasterPawn, base.EquipmentSource))
            {
                return false;
            }
            return true;
        }

        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            return JumpUtility.CanHitTargetFrom(CasterPawn, root, targ, EffectiveRange);
        }

        public override void DrawHighlight(LocalTargetInfo target)
        {
            if (target.IsValid && JumpUtility.ValidJumpTarget(caster.Map, target.Cell))
            {
                GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
            }
            GenDraw.DrawRadiusRing(caster.Position, EffectiveRange, Color.white, (IntVec3 c) => GenSight.LineOfSight(caster.Position, c, caster.Map) && JumpUtility.ValidJumpTarget(caster.Map, c));
        }
    }
    */
}
