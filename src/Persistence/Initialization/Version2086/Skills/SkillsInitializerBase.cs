// <copyright file="SkillsInitializerBase.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086.Skills;

using MUnique.OpenMU.AttributeSystem;
using MUnique.OpenMU.DataModel.Attributes;
using MUnique.OpenMU.DataModel.Configuration;
using MUnique.OpenMU.GameLogic.Attributes;
using MUnique.OpenMU.Persistence;
using MUnique.OpenMU.Persistence.Initialization;
using MUnique.OpenMU.Persistence.Initialization.Skills;
using MUnique.OpenMU.Persistence.Initialization.Version2086.CharacterClasses;

/// <summary>
/// Base class for a skills initializer.
/// </summary>
internal abstract class SkillsInitializerBase : InitializerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SkillsInitializerBase"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    protected SkillsInitializerBase(IContext context, GameConfiguration gameConfiguration)
        : base(context, gameConfiguration)
    {
    }

    /// <summary>
    /// Creates the skill.
    /// </summary>
    /// <param name="number">The number.</param>
    /// <param name="name">The name of the skill.</param>
    /// <param name="characterClasses">The character classes.</param>
    /// <param name="damageType">Type of the damage.</param>
    /// <param name="damage">The damage.</param>
    /// <param name="distance">The distance.</param>
    /// <param name="abilityConsumption">The ability consumption.</param>
    /// <param name="manaConsumption">The mana consumption.</param>
    /// <param name="levelRequirement">The level requirement.</param>
    /// <param name="energyRequirement">The energy requirement.</param>
    /// <param name="leadershipRequirement">The leadership requirement.</param>
    /// <param name="elementalModifier">The elemental modifier.</param>
    /// <param name="skillType">Type of the skill.</param>
    /// <param name="skillTarget">The skill target.</param>
    /// <param name="implicitTargetRange">The implicit target range.</param>
    /// <param name="targetRestriction">The target restriction.</param>
    /// <param name="movesToTarget">If set to <c>true</c>, the skill moves the player to the target.</param>
    /// <param name="movesTarget">If set to <c>true</c>, it moves target randomly.</param>
    /// <param name="cooldownMinutes">The cooldown minutes.</param>
    protected void CreateSkill(
        short number,
        string name,
        DamageType damageType = DamageType.None,
        int damage = 0,
        short distance = 0,
        int abilityConsumption = 0,
        int manaConsumption = 0,
        int levelRequirement = 0,
        int energyRequirement = 0,
        int leadershipRequirement = 0,
        int strengthRequirement = 0,
        int dexterityRequirement = 0,
        int vitalityRequirement = 0,
        ElementalType elementalModifier = ElementalType.Undefined,
        SkillType skillType = SkillType.DirectHit,
        SkillTarget skillTarget = SkillTarget.Explicit,
        short implicitTargetRange = 0,
        SkillTargetRestriction targetRestriction = SkillTargetRestriction.Undefined,
        bool movesToTarget = false,
        bool movesTarget = false,
        int cooldownMinutes = 0,
        (AttributeDefinition Stat, int Value)? scalingStat1 = default,
        (AttributeDefinition Stat, int Value)? scalingStat2 = default,
        int wizardClass = 0, int knightClass = 0, int elfClass = 0, int magicGladiatorClass = 0, int darkLordClass = 0, int summonerClass = 0, int ragefighterClass = 0, int growLancerClass = 0, int runeWizardClass = 0, int slayerClass = 0, int gunCrusherClass = 0, int whiteWizardClass = 0, int lemuriaClass = 0, int illusionKnightClass = 0, int alchemistClass = 0, int crusaderClass = 0
        )
    {
        var skill = Context.CreateNew<Skill>();
        GameConfiguration.Skills.Add(skill);
        skill.Number = number;
        skill.Name = name;
        skill.MovesToTarget = movesToTarget;
        skill.MovesTarget = movesTarget;
        skill.AttackDamage = damage;

        CreateSkillRequirementIfNeeded(skill, Stats.Level, levelRequirement);
        CreateSkillRequirementIfNeeded(skill, Stats.TotalLeadership, leadershipRequirement);
        CreateSkillRequirementIfNeeded(skill, Stats.TotalEnergy, energyRequirement);
        CreateSkillRequirementIfNeeded(skill, Stats.TotalStrength, strengthRequirement);
        CreateSkillRequirementIfNeeded(skill, Stats.TotalAgility, dexterityRequirement);
        CreateSkillRequirementIfNeeded(skill, Stats.TotalVitality, vitalityRequirement);
        CreateSkillConsumeRequirementIfNeeded(skill, Stats.CurrentMana, manaConsumption);
        CreateSkillConsumeRequirementIfNeeded(skill, Stats.CurrentAbility, abilityConsumption);

        if (scalingStat1 != null && scalingStat1.Value.Value > 0)
        {
            AddAttributeRelationship(skill, Stats.SkillDamageBonus, 1f / scalingStat1.Value.Value, scalingStat1.Value.Stat);
        }

        if (scalingStat2 != null && scalingStat2.Value.Value > 0)
        {
            AddAttributeRelationship(skill, Stats.SkillDamageBonus, 1f / scalingStat2.Value.Value, scalingStat2.Value.Stat);
        }

        skill.Range = distance;
        skill.DamageType = damageType;
        skill.SkillType = skillType;

        skill.ImplicitTargetRange = implicitTargetRange;
        skill.Target = skillTarget;
        skill.TargetRestriction = targetRestriction;
        var classes = this.GameConfiguration.DetermineCharacterClasses(false, wizardClass, knightClass, elfClass, magicGladiatorClass, darkLordClass, summonerClass, ragefighterClass, growLancerClass, runeWizardClass, slayerClass, gunCrusherClass, whiteWizardClass, lemuriaClass, illusionKnightClass, alchemistClass, crusaderClass);
        foreach (var characterClass in classes)
        {
            skill.QualifiedCharacters.Add(characterClass);
        }

        if (elementalModifier != ElementalType.Undefined)
        {
            ApplyElementalModifier(elementalModifier, skill);
        }
        //skill.Requirements.Add(CreateRequirement);
        skill.SetGuid(skill.Number);
    }

    /// <summary>
    /// Adds the area skill settings for the specified skill.
    /// </summary>
    /// <param name="skillNumber">The skill number.</param>
    /// <param name="useFrustumFilter">If set to <c>true</c>, the skill should use a frustum filter.</param>
    /// <param name="frustumStartWidth">Start width of the frustum.</param>
    /// <param name="frustumEndWidth">End width of the frustum.</param>
    /// <param name="frustumDistance">The frustum distance.</param>
    /// <param name="useDeferredHits">If set to <c>true</c>, the skill should use deferred hits.</param>
    /// <param name="delayPerOneDistance">The delay per one distance.</param>
    /// <param name="delayBetweenHits">The delay between hits.</param>
    /// <param name="minimumHitsPerTarget">The minimum hits per target.</param>
    /// <param name="maximumHitsPerTarget">The maximum hits per target.</param>
    /// <param name="maximumHitsPerAttack">The maximum hits per attack.</param>
    /// <param name="hitChancePerDistanceMultiplier">The hit chance per distance multiplier.</param>
    /// <param name="useTargetAreaFilter">If set to <c>true</c>, the skill should use a target area filter.</param>
    /// <param name="targetAreaDiameter">The target area diameter.</param>
    protected void AddAreaSkillSettings(
        SkillNumber skillNumber,
        bool useFrustumFilter,
        float frustumStartWidth,
        float frustumEndWidth,
        float frustumDistance,
        bool useDeferredHits = false,
        TimeSpan delayPerOneDistance = default,
        TimeSpan delayBetweenHits = default,
        int minimumHitsPerTarget = 1,
        int maximumHitsPerTarget = 1,
        int maximumHitsPerAttack = default,
        float hitChancePerDistanceMultiplier = 1.0f,
        bool useTargetAreaFilter = false,
        float targetAreaDiameter = default)
    {
        var skill = GameConfiguration.Skills.First(s => s.Number == (short)skillNumber);
        var areaSkillSettings = Context.CreateNew<AreaSkillSettings>();
        skill.AreaSkillSettings = areaSkillSettings;

        areaSkillSettings.UseFrustumFilter = useFrustumFilter;
        areaSkillSettings.FrustumStartWidth = frustumStartWidth;
        areaSkillSettings.FrustumEndWidth = frustumEndWidth;
        areaSkillSettings.FrustumDistance = frustumDistance;
        areaSkillSettings.UseTargetAreaFilter = useTargetAreaFilter;
        areaSkillSettings.TargetAreaDiameter = targetAreaDiameter;
        areaSkillSettings.UseDeferredHits = useDeferredHits;
        areaSkillSettings.DelayPerOneDistance = delayPerOneDistance;
        areaSkillSettings.DelayBetweenHits = delayBetweenHits;
        areaSkillSettings.MinimumNumberOfHitsPerTarget = minimumHitsPerTarget;
        areaSkillSettings.MaximumNumberOfHitsPerTarget = maximumHitsPerTarget;
        areaSkillSettings.MaximumNumberOfHitsPerAttack = maximumHitsPerAttack;
        areaSkillSettings.HitChancePerDistanceMultiplier = hitChancePerDistanceMultiplier;
    }

    private void ApplyElementalModifier(ElementalType elementalModifier, Skill skill)
    {
        if ((SkillNumber)skill.Number is SkillNumber.IceArrow or SkillNumber.IceArrowStrengthener)
        {
            skill.ElementalModifierTarget = Stats.IceResistance.GetPersistent(GameConfiguration);
            skill.MagicEffectDef = CreateEffect(ElementalType.Ice, MagicEffectNumber.Freeze, Stats.IsFrozen, 5);
            return;
        }

        switch (elementalModifier)
        {
            case ElementalType.Ice:
                skill.ElementalModifierTarget = Stats.IceResistance.GetPersistent(GameConfiguration);
                skill.MagicEffectDef = CreateEffect(ElementalType.Ice, MagicEffectNumber.Iced, Stats.IsIced, 10);
                break;
            case ElementalType.Poison:
                skill.ElementalModifierTarget = Stats.PoisonResistance.GetPersistent(GameConfiguration);

                // Poison Skill applies damage 7 times, while decay three times. We assume that we apply each damage
                // every 3 seconds. We leave one or two extra seconds, so that the damage is applied for sure.
                var durationInSeconds = skill.Number == (short)SkillNumber.Poison ? 20 : 10;
                skill.MagicEffectDef = CreateEffect(ElementalType.Poison, MagicEffectNumber.Poisoned, Stats.IsPoisoned, durationInSeconds);
                break;
            case ElementalType.Lightning:
                skill.ElementalModifierTarget = Stats.LightningResistance.GetPersistent(GameConfiguration);
                break;
            case ElementalType.Fire:
                skill.ElementalModifierTarget = Stats.FireResistance.GetPersistent(GameConfiguration);
                break;
            case ElementalType.Earth:
                skill.ElementalModifierTarget = Stats.EarthResistance.GetPersistent(GameConfiguration);
                break;
            case ElementalType.Wind:
                skill.ElementalModifierTarget = Stats.WindResistance.GetPersistent(GameConfiguration);
                break;
            case ElementalType.Water:
                skill.ElementalModifierTarget = Stats.WaterResistance.GetPersistent(GameConfiguration);
                break;
            default:
                // None
                break;
        }
    }

    private MagicEffectDefinition CreateEffect(ElementalType type, MagicEffectNumber effectNumber, AttributeDefinition targetAttribute, float durationInSeconds)
    {
        if (GameConfiguration.MagicEffects.FirstOrDefault(
                e => e.Number == (short)effectNumber
                     && e.SubType == (byte)(0xFF - type)
                     && Equals(e.Duration?.ConstantValue.Value, durationInSeconds)
                     && e.PowerUpDefinitions.FirstOrDefault()?.TargetAttribute == targetAttribute) is { } existingEffect)
        {
            return existingEffect;
        }

        var effect = Context.CreateNew<MagicEffectDefinition>();
        GameConfiguration.MagicEffects.Add(effect);
        effect.Name = Enum.GetName(effectNumber) ?? string.Empty;
        effect.InformObservers = true;
        effect.Number = (short)effectNumber;
        effect.StopByDeath = true;
        effect.SubType = (byte)(0xFF - type);
        effect.Duration = Context.CreateNew<PowerUpDefinitionValue>();
        effect.Duration.ConstantValue.Value = durationInSeconds;
        var powerUpDefinition = Context.CreateNew<PowerUpDefinition>();
        effect.PowerUpDefinitions.Add(powerUpDefinition);
        powerUpDefinition.Boost = Context.CreateNew<PowerUpDefinitionValue>();
        powerUpDefinition.Boost.ConstantValue.Value = 1;
        powerUpDefinition.TargetAttribute = targetAttribute.GetPersistent(GameConfiguration);
        return effect;
    }

    private void CreateSkillConsumeRequirementIfNeeded(Skill skill, AttributeDefinition attribute, int requiredValue)
    {
        if (requiredValue == 0)
        {
            return;
        }

        var requirement = CreateRequirement(attribute, requiredValue);
        skill.ConsumeRequirements.Add(requirement);
    }

    private void CreateSkillRequirementIfNeeded(Skill skill, AttributeDefinition attribute, int requiredValue)
    {
        if (requiredValue == 0)
        {
            return;
        }

        var requirement = CreateRequirement(attribute, requiredValue);
        skill.Requirements.Add(requirement);
    }

    private void AddAttributeRelationship(Skill skill, AttributeDefinition targetAttribute, float multiplier, AttributeDefinition sourceAttribute, AggregateType aggregateType = AggregateType.AddRaw)
    {
        var relationship = CharacterClassHelper.CreateAttributeRelationship(this.Context, this.GameConfiguration, targetAttribute, multiplier, sourceAttribute, aggregateType: aggregateType);
        skill.AttributeRelationships.Add(relationship);
    }
}