// <copyright file="CharacterClassInitialization.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086.CharacterClasses;

using MUnique.OpenMU.AttributeSystem;
using MUnique.OpenMU.DataModel.Configuration;
using MUnique.OpenMU.GameLogic.Attributes;

/// <summary>
/// Initialization of character classes data.
/// </summary>
internal partial class CharacterClassInitialization : InitializerBase
{
    private const int LorenciaMapId = 0;
    private const int NoriaMapId = 3;
    private const int ElvenlandMapId = 51;

    /// <summary>
    /// Initializes a new instance of the <see cref="CharacterClassInitialization" /> class.
    /// </summary>
    /// <param name="context">The persistence context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    public CharacterClassInitialization(IContext context, GameConfiguration gameConfiguration)
        : base(context, gameConfiguration)
    {
    }

    /// <summary>
    /// Gets a value indicating whether to use classic PVP, which uses no shield stats and the same attack/defense rate as PvM.
    /// </summary>
    protected virtual bool UseClassicPvp => false;

    /// <summary>
    /// Creates the character classes.
    /// </summary>
    public override void Initialize()
    {
        var ignitionKnight = this.CreateIgnitionKnight();
        var dragonKnight = this.CreateDragonKnight(ignitionKnight);
        var bladeMaster = this.CreateBladeMaster(dragonKnight);
        var bladeKnight = this.CreateBladeKnight(bladeMaster);
        this.CreateDarkKnight(CharacterClassNumber.DarkKnight, "Dark Knight", false, bladeKnight, true);

        var darknessWizard = this.CreateDarknessWizard();
        var soulWizard = this.CreateSoulWizard(darknessWizard);
        var grandMaster = this.CreateGrandMaster(soulWizard);
        var soulMaster = this.CreateSoulMaster(grandMaster);
        this.CreateDarkWizard(CharacterClassNumber.DarkWizard, "Dark Wizard", false, soulMaster, true);

        var royalElf = this.CreateRoyalElf();
        var nobleElf = this.CreateNobleElf(royalElf);
        var highElf = this.CreateHighElf(nobleElf);
        var museElf = this.CreateMuseElf(highElf);
        this.CreateFairyElf(CharacterClassNumber.FairyElf, "Fairy Elf", false, museElf, true);

        var endlessSummoner = this.CreateEndlessSummoner();
        var dimensionSummoner = this.CreateDimensionSummoner(endlessSummoner);
        var dimensionMaster = this.CreateDimensionMaster(dimensionSummoner);
        var bloodySummoner = this.CreateBloodySummoner(dimensionMaster);
        this.CreateSummoner(CharacterClassNumber.Summoner, "Summoner", false, bloodySummoner, true);

        var dupleKnight = this.CreateDupleKnight();
        var magicKnight = this.CreateMagicKnight(dupleKnight);
        var duelMaster = this.CreateDuelMaster(magicKnight);
        this.CreateMagicGladiator(CharacterClassNumber.MagicGladiator, "Magic Gladiator", false, duelMaster, true);

        var forceEmpire = this.CreateForceEmpire();
        var empireLord = this.CreateEmpireLord(forceEmpire);
        var lordEmperor = this.CreateLordEmperor(empireLord);
        this.CreateDarkLord(CharacterClassNumber.DarkLord, "Dark Lord", false, false, lordEmperor, true);

        var bloodyFighter = this.CreateBloodyFighter();
        var fistBlazer = this.CreateFistBlazer(bloodyFighter);
        var fistMaster = this.CreateFistMaster(fistBlazer);
        this.CreateRageFighter(CharacterClassNumber.RageFighter, "Rage Fighter", false, fistMaster, true);

        var gc5 = this.CreateArcaneLancer();
        var gc4 = this.CreateShiningLancer(gc5);
        var gc3 = this.CreateMirageLancer(gc4);
        this.CreateGrowLancer(CharacterClassNumber.GrowLancer, "Grow Lancer", false, gc3, true);

        var rw5 = this.CreateInfinityRuneWizard();
        var rw4 = this.CreateMajesticRuneWizard(rw5);
        var rw3 = this.CreateGrandRuneMaster(rw4);
        var rw2 = this.CreateRuneSpellMaster(rw3);
        this.CreateRuneWizard(CharacterClassNumber.RuneWizard, "Rune Wizard", false, rw2, true);

        var sl5 = this.CreateRogueSlayer();
        var sl4 = this.CreateSlaughterer(sl5);
        var sl3 = this.CreateMasterSlayer(sl4);
        var sl2 = this.CreateRoyalSlayer(sl3);
        this.CreateSlayer(CharacterClassNumber.Slayer, "Slayer", false, sl2, true);

        var gu5 = this.CreateMagnusGunCrusher();
        var gu4 = this.CreateHeistGunCrusher(gu5);
        var gu3 = this.CreateMasterGunBreaker(gu4);
        var gu2 = this.CreateGunBreaker(gu3);
        this.CreateGunCrusher(CharacterClassNumber.GunCrusher, "Gun Crusher", false, gu2, true);

        var ww5 = this.CreateGloryWizard();
        var ww4 = this.CreateLuminousWizard(ww5);
        var ww3 = this.CreateShineWizard(ww4);
        var ww2 = this.CreateLightMaster(ww3);
        this.CreateWhiteWizard(CharacterClassNumber.WhiteWizard, "White Wizard", false, ww2, true);

        var lm5 = this.CreateBattleMage();
        var lm4 = this.CreateMysticMage(lm5);
        var lm3 = this.CreateArchMage(lm4);
        var lm2 = this.CreateWoMage(lm3);
        this.CreateLemuria(CharacterClassNumber.Lemuria, "Lemuria", false, lm2, true);

        var ik5 = this.CreatePhantomPainKnight();
        var ik4 = this.CreateMysticKnight(ik5);
        var ik3 = this.CreateIllusionMaster(ik4);
        var ik2 = this.CreateMirageKnight(ik3);
        this.CreateIllusionKnight(CharacterClassNumber.IllusionKnight, "Illusion Knight", false, ik2, true);

        var al5 = this.CreateCreator();
        var al4 = this.CreateAlchemicForce(al5);
        var al3 = this.CreateAlchemicMaster(al4);
        var al2 = this.CreateAlchemicMagician(al3);
        this.CreateAlchemist(CharacterClassNumber.Alchemist, "Alchemist", false, al2, true);

        var templarCommander = this.CreateTemplarCommander();
        var sacredPaladin = this.CreateSacredPaladin(templarCommander);
        var masterPaladin = this.CreateMasterPaladin(sacredPaladin);
        var impactCrusader = this.CreateImpactCrusader(masterPaladin);
        this.CreateCrusader(CharacterClassNumber.Crusader, "Crusader", false, impactCrusader, true);

        
    }

    private StatAttributeDefinition CreateStatAttributeDefinition(AttributeDefinition attribute, int value, bool increasableByPlayer)
    {
        var definition = this.Context.CreateNew<StatAttributeDefinition>(attribute.GetPersistent(this.GameConfiguration), value, increasableByPlayer);
        return definition;
    }

    private AttributeRelationship CreateAttributeRelationship(AttributeDefinition targetAttribute, float multiplier, AttributeDefinition sourceAttribute, InputOperator inputOperator = InputOperator.Multiply, AggregateType aggregateType = AggregateType.AddRaw)
    {
        return CharacterClassHelper.CreateAttributeRelationship(this.Context, this.GameConfiguration, targetAttribute, multiplier, sourceAttribute, inputOperator, aggregateType);
    }

    private AttributeRelationship CreateAttributeRelationship(AttributeDefinition targetAttribute, AttributeDefinition multiplierAttribute, AttributeDefinition sourceAttribute, InputOperator inputOperator = InputOperator.Multiply, AggregateType aggregateType = AggregateType.AddRaw)
    {
        return CharacterClassHelper.CreateAttributeRelationship(this.Context, this.GameConfiguration, targetAttribute, multiplierAttribute, sourceAttribute, inputOperator, aggregateType);
    }

    private AttributeRelationship CreateConditionalRelationship(AttributeDefinition targetAttribute, AttributeDefinition conditionalAttribute, AttributeDefinition sourceAttribute, AggregateType aggregateType = AggregateType.AddRaw)
    {
        return CharacterClassHelper.CreateConditionalRelationship(this.Context, this.GameConfiguration, targetAttribute, conditionalAttribute, sourceAttribute, aggregateType);
    }

    private ConstValueAttribute CreateConstValueAttribute(float value, AttributeDefinition attribute)
    {
        return CharacterClassHelper.CreateConstValueAttribute(this.Context, this.GameConfiguration, value, attribute);
    }

    private void AddCommonAttributeRelationships(ICollection<AttributeRelationship> attributeRelationships)
    {
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.TotalLevel, 1, Stats.Level));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.TotalLevel, 1, Stats.MasterLevel));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.TotalLevel, 1, Stats.MajesticLevel));

        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.TotalStrength, 1, Stats.BaseStrength));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.TotalAgility, 1, Stats.BaseAgility));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.TotalVitality, 1, Stats.BaseVitality));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.TotalEnergy, 1, Stats.BaseEnergy));

        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.DefenseBase, 1, Stats.DefenseShield));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.DefenseFinal, 0.5f, Stats.DefenseBase));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.DefensePvm, 1, Stats.DefenseFinal));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.DefensePvp, 1, Stats.DefenseFinal));

        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.AttackSpeedAny, 1, Stats.AttackSpeedByWeapon));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.AttackSpeed, 1, Stats.AttackSpeedAny));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.MagicSpeed, 1, Stats.AttackSpeedAny));

        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.MinimumPhysicalDmg, 1, Stats.MinimumPhysBaseDmgByWeapon));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.MinimumPhysicalDmg, 1, Stats.MinimumPhysBaseDmg));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.MinimumPhysicalDmg, 1, Stats.PhysicalBaseDmg));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.MaximumPhysicalDmg, 1, Stats.MaximumPhysBaseDmgByWeapon));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.MaximumPhysicalDmg, 1, Stats.MaximumPhysBaseDmg));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.MaximumPhysicalDmg, 1, Stats.PhysicalBaseDmg));

        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.FinalMinimumPhysicalDmg, 1, Stats.MinimumPhysicalDmg));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.FinalMinimumPhysicalDmg, 1, Stats.CombatPowerDamage));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.FinalMinimumPhysicalDmg, 1, Stats.PhysicalBaseDmgIncrease, aggregateType: AggregateType.Multiplicate));

        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.FinalMaximumPhysicalDmg, 1, Stats.MinimumPhysicalDmg));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.FinalMaximumPhysicalDmg, 1, Stats.CombatPowerDamage));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.FinalMaximumPhysicalDmg, 1, Stats.PhysicalBaseDmgIncrease, aggregateType: AggregateType.Multiplicate));

        // If two weapons are equipped (DK, MG, Sum, RF) we subtract the half of the sum of the speeds again from the attack speed
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.AreTwoWeaponsEquipped, 1, Stats.EquippedWeaponCount));
        var tempSpeed = this.Context.CreateNew<AttributeDefinition>(Guid.NewGuid(), "Temp Half weapon attack speed", string.Empty);
        this.GameConfiguration.Attributes.Add(tempSpeed);
        attributeRelationships.Add(this.CreateAttributeRelationship(tempSpeed, -0.5f, Stats.AttackSpeedByWeapon));
        attributeRelationships.Add(this.CreateConditionalRelationship(Stats.AttackSpeedAny, Stats.AreTwoWeaponsEquipped, tempSpeed));

        var tempDefense = this.Context.CreateNew<AttributeDefinition>(Guid.NewGuid(), "Temp Defense Bonus multiplier with Shield", string.Empty);
        this.GameConfiguration.Attributes.Add(tempDefense);
        attributeRelationships.Add(this.CreateConditionalRelationship(tempDefense, Stats.IsShieldEquipped, Stats.DefenseIncreaseWithEquippedShield));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.DefenseFinal, 1, tempDefense, InputOperator.Add, AggregateType.Multiplicate));

        attributeRelationships.Add(this.CreateConditionalRelationship(Stats.DefenseFinal, Stats.DefenseShield, Stats.ShieldItemDefenseIncrease));
        attributeRelationships.Add(this.CreateConditionalRelationship(Stats.DefenseFinal, Stats.IsShieldEquipped, Stats.BonusDefenseWithShield, AggregateType.AddFinal));
        attributeRelationships.Add(this.CreateConditionalRelationship(Stats.DefenseRatePvm, Stats.IsShieldEquipped, Stats.BonusDefenseRateWithShield, AggregateType.AddFinal));

        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.HealthRecoveryMultiplier, 0.01f, Stats.IsInSafezone));
        if (this.UseClassicPvp)
        {
            attributeRelationships.Add(this.CreateAttributeRelationship(Stats.DefenseRatePvp, 1, Stats.DefenseRatePvm));
            attributeRelationships.Add(this.CreateAttributeRelationship(Stats.AttackRatePvp, 1, Stats.AttackRatePvm));
        }
        else
        {
            attributeRelationships.Add(this.CreateAttributeRelationship(Stats.ShieldRecoveryMultiplier, 0.01f, Stats.IsInSafezone));
        }

        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.MaximumGuildSize, 0.1f, Stats.Level));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.CanFly, 1, Stats.IsDinorantEquipped));
    }

    private void AddCommonBaseAttributeValues(ICollection<ConstValueAttribute> baseAttributeValues, bool isMaster, bool isMajestic = false)
    {
        baseAttributeValues.Add(this.CreateConstValueAttribute(1.0f / 27.5f, Stats.ManaRecoveryMultiplier));
        baseAttributeValues.Add(this.CreateConstValueAttribute(1, Stats.DamageReceiveDecrement));
        baseAttributeValues.Add(this.CreateConstValueAttribute(1, Stats.AttackDamageIncrease));
        baseAttributeValues.Add(this.CreateConstValueAttribute(1, Stats.MoneyAmountRate));
        baseAttributeValues.Add(this.CreateConstValueAttribute(1, Stats.ExperienceRate));
        baseAttributeValues.Add(this.CreateConstValueAttribute(0.03f, Stats.PoisonDamageMultiplier));
        baseAttributeValues.Add(this.CreateConstValueAttribute(1, Stats.ItemDurationIncrease));
        baseAttributeValues.Add(this.CreateConstValueAttribute(2, Stats.AbilityRecoveryAbsolute));
        baseAttributeValues.Add(this.CreateConstValueAttribute(1, Stats.PhysicalBaseDmgIncrease));
        baseAttributeValues.Add(this.CreateConstValueAttribute(-1, Stats.AreTwoWeaponsEquipped));
        baseAttributeValues.Add(this.CreateConstValueAttribute(-1, Stats.HasDoubleWield));

        if (isMaster)
        {
            baseAttributeValues.Add(this.CreateConstValueAttribute(1, Stats.MasterPointsPerLevelUp));
            baseAttributeValues.Add(this.CreateConstValueAttribute(1, Stats.MasterExperienceRate));
        }

        if (isMajestic)
        {
            baseAttributeValues.Add(this.CreateConstValueAttribute(1, Stats.MajesticPointsPerLevelUp));
            baseAttributeValues.Add(this.CreateConstValueAttribute(100, Stats.MajesticExperienceRate));
        }

        if (!this.UseClassicPvp)
        {
            baseAttributeValues.Add(this.CreateConstValueAttribute(0.01f, Stats.ShieldRecoveryMultiplier));
        }
    }

    /// <summary>
    /// Adds double wield attribute relationships applicable to characters that can double wield (DK, MG, and RF).
    /// A double wield grants 110% physical attack damage (55% base damage, later doubled on damage calculations).
    /// </summary>
    private void AddDoubleWieldAttributeRelationships(ICollection<AttributeRelationship> attributeRelationships)
    {
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.HasDoubleWield, 1, Stats.DoubleWieldWeaponCount, InputOperator.Maximum));
        var tempDoubleWield = this.Context.CreateNew<AttributeDefinition>(Guid.NewGuid(), "Temp Double Wield multiplier", string.Empty);
        this.GameConfiguration.Attributes.Add(tempDoubleWield);
        attributeRelationships.Add(this.CreateAttributeRelationship(tempDoubleWield, -0.45f, Stats.HasDoubleWield));
        attributeRelationships.Add(this.CreateAttributeRelationship(Stats.PhysicalBaseDmgIncrease, 1, tempDoubleWield, InputOperator.Add, AggregateType.Multiplicate));
        attributeRelationships.Add(this.CreateConditionalRelationship(Stats.MinimumPhysBaseDmgByWeapon, Stats.HasDoubleWield, Stats.MinPhysBaseDmgByRightWeapon));
        attributeRelationships.Add(this.CreateConditionalRelationship(Stats.MaximumPhysBaseDmgByWeapon, Stats.HasDoubleWield, Stats.MaxPhysBaseDmgByRightWeapon));
    }
}