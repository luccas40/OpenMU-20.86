// <copyright file="SkillsInitializer.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086.Skills;

// ReSharper disable StringLiteralTypo
using MUnique.OpenMU.AttributeSystem;
using MUnique.OpenMU.DataModel.Configuration;
using MUnique.OpenMU.GameLogic.Attributes;
using MUnique.OpenMU.GameLogic.NPC;
using MUnique.OpenMU.Persistence.Initialization.Skills;
using MUnique.OpenMU.Persistence.Initialization.Version2086.CharacterClasses;
using System;
using System.Reflection.PortableExecutable;

/// <summary>
/// Initialization logic for <see cref="Skill"/>s.
/// </summary>
internal class SkillsInitializer : SkillsInitializerBase
{
    internal const string Formula1204 = "(1 + (((((((level - 30) ^ 3) + 25000) / 499) / 6)))) * 10"; // 17
    internal const string Formula61408 = "(1 + (((((((level - 30) ^ 3) + 25000) / 499) / 50) * 100) / 12)) * 85 * 6"; // 12
    internal const string Formula51173 = "(1 + (((((((level - 30) ^ 3) + 25000) / 499) / 50) * 100) / 12)) * 85 * 5"; // 13
    internal const string Formula181 = "(1 + (((((((level - 30) ^ 3) + 25000) / 499) / 6)))) * 1.5"; // 7
    internal const string FormulaRecoveryIncrease181 = "(1 + (((((((level - 30) ^ 3) + 25000) / 499) / 6)))) * 1.5 * 0.01"; // 7
    internal const string Formula120 = "(1 + (((((((level - 30) ^ 3) + 25000) / 499) / 50) * 100) / 12))"; // 1    // about 1.2 to 9.0
    internal const string Formula120Value = "(1 + (((((((level - 30) ^ 3) + 25000) / 499) / 50) * 100) / 12)) * 0.01"; // 1    // about 0.012 to 0.09
    internal const string FormulaRecoveryIncrease120 = "(1 + (((((((level - 30) ^ 3) + 25000) / 499) / 50) * 100) / 12)) / 100"; // 1
    internal const string FormulaIncreaseMultiplicator120 = "(101 + (((((((level - 30) ^ 3) + 25000) / 499) / 50) * 100) / 12)) / 100"; // 1
    internal const string Formula6020 = "(1 + (((((((level - 30) ^ 3) + 25000) / 499) / 6)))) * 50"; // 16
    internal const string Formula6020Value = "(1 + (((((((level - 30) ^ 3) + 25000) / 499) / 6)))) * 50 / 100"; // 16
    internal const string Formula502 = "(0.8 + (((((((level - 30) ^ 3) + 25000) / 499) / 6)))) * 5";
    internal const string Formula632 = "(0.85 + (((((((level - 30) ^ 3) + 25000) / 499) / 6)))) * 6"; // 3
    internal const string Formula883 = "(0.9 + (((((((level - 30) ^ 3) + 25000) / 499) / 6)))) * 8"; // 4
    internal const string Formula10235 = "(1 + (((((((level - 30) ^ 3) + 25000) / 499) / 50) * 100) / 12)) * 85"; // 9
    internal const string Formula81877 = "(1 + (((((((level - 30) ^ 3) + 25000) / 499) / 50) * 100) / 12)) * 85 * 8"; // 14
    internal const string Formula1154 = "(0.95 + (((((((level - 30) ^ 3) + 25000) / 499) / 6)))) * 10"; // 5
    internal const string Formula803 = "(0.8 + (((((((level - 30) ^ 3) + 25000) / 499) / 50) * 100) / 12)) * 8"; // 10
    internal const string Formula1 = "1 * level";
    internal const string Formula1WhenComplete = "if(level < 10; 0; 1)";
    internal const string Formula722 = "(1 + (((((((level - 30) ^ 3) + 25000) / 499) / 6)))) * 6"; // 18 // 7.22 to 54.09
    internal const string Formula722Value = "((1 + (((((((level - 30) ^ 3) + 25000) / 499) / 6)))) * 6) * 0.01"; // 18   // 0.0722 to 0.5409
    internal const string Formula4319 = "52 / (1 + (((((((level - 30) ^ 3) + 25000) / 499) / 6))))"; // 6
    internal const string Formula914 = "11 / (1 + (((((((level - 30) ^ 3) + 25000) / 499) / 50) * 100) / 12))"; // 11
    internal const string Formula3822 = "40 / (1 + (((((((level - 30) ^ 3) + 25000) / 499) / 6)))) + 5"; // 20
    internal const string Formula25587 = "(1 + ( ( ( ( ( ( (level - 30) ^ 3) + 25000) / 499) / 50) * 100) / 12)) * 85 * 2.5"; // 29
    internal const string Formula30704 = "(1 + ( ( ( ( ( ((level - 30) ^ 3) + 25000) / 499) / 50) * 100) / 12)) * 85 * 3"; // 33
    internal const string Formula3371 = "(1 + ( ( ( ( ( ( (level - 30) ^ 3) + 25000) / 499) / 6) ) ) ) * 28"; // 35
    internal const string Formula20469 = "(1 + ( ( ( ( ( ( (level - 30) ^ 3) + 25000) / 499) / 50) * 100) / 12) ) * 85 * 2"; // 30
    internal const string Formula1806 = "(1 + (((((((level - 30) ^ 3) + 25000) / 499) / 6)))) * 15"; // 15
    internal const string Formula32751 = "(1 + ( ( ( ( ( ( (level - 30) ^ 3) + 25000) / 499) / 50) * 100) / 12)) * 85 * 3.2"; // 31
    internal const string Formula5418 = "(1 + ( ( ( ( ( ( (level - 30) ^ 3) + 25000) / 499) / 50) * 100) / 12)) * 45"; // 34

    private static readonly IDictionary<SkillNumber, MagicEffectNumber> EffectsOfSkills = new Dictionary<SkillNumber, MagicEffectNumber>
    {
        { SkillNumber.SwellLife, MagicEffectNumber.GreaterFortitude },
        { SkillNumber.LordDignity, MagicEffectNumber.CriticalDamageIncrease },
        { SkillNumber.SoulBarrier, MagicEffectNumber.SoulBarrier },
        { SkillNumber.Defense, MagicEffectNumber.ShieldSkill },
        { SkillNumber.GreaterDefense, MagicEffectNumber.GreaterDefense },
        { SkillNumber.GreaterDamage, MagicEffectNumber.GreaterDamage },
        { SkillNumber.Heal, MagicEffectNumber.Heal },
        { SkillNumber.Recovery, MagicEffectNumber.ShieldRecover },
        { SkillNumber.InfinityArrow, MagicEffectNumber.InfiniteArrow },
        { SkillNumber.FireSlash, MagicEffectNumber.DefenseReduction },
        { SkillNumber.IgnoreDefense, MagicEffectNumber.IgnoreDefense },
        { SkillNumber.IncreaseHealth, MagicEffectNumber.IncreaseHealth },
        { SkillNumber.IncreaseBlock, MagicEffectNumber.IncreaseBlock },
        { SkillNumber.ExpansionofWizardry, MagicEffectNumber.WizEnhance },
        { SkillNumber.Berserker, MagicEffectNumber.Berserker },
    };

    private readonly IDictionary<byte, MasterSkillRoot> _masterSkillRoots;

    /// <summary>
    /// Initializes a new instance of the <see cref="SkillsInitializer"/> class.
    /// </summary>
    /// <param name="context">The persistence context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    public SkillsInitializer(IContext context, GameConfiguration gameConfiguration)
        : base(context, gameConfiguration)
    {
        this._masterSkillRoots = new SortedDictionary<byte, MasterSkillRoot>();
    }

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    /// <remarks>
    /// Regex: (?m)^\s*(\d+)\s+\"(.+?)\"\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(-*\d+)\s(-*\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s*$
    /// Replace by: this.CreateSkill($1, "$2", $3, $4, $5, $6, $7, $9, $10, $11, $12, $13, $15, $19, $20, $21, $22, $23, $24, $25, $26, $27, $28);.
    /// </remarks>
    public override void Initialize()
    {
        this.CreateSkill(1, "Poison", DamageType.Wizardry, 12, 6, 0, 42, 30, 140, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0);
        this.CreateSkill(2, "Meteorite", DamageType.Wizardry, 21, 6, 0, 12, 21, 104, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0);
        this.CreateSkill(3, "Lightning", DamageType.Wizardry, 17, 6, 0, 15, 13, 72, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0);
        this.CreateSkill(4, "Fire Ball", DamageType.Wizardry, 8, 6, 0, 3, 5, 40, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0);
        this.CreateSkill(5, "Flame", DamageType.Wizardry, 25, 6, 0, 50, 35, 160, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0);
        this.CreateSkill(6, "Teleport", DamageType.Wizardry, 0, 6, 0, 30, 17, 88, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateSkill(7, "Ice", DamageType.Wizardry, 10, 6, 0, 38, 25, 120, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0);
        this.CreateSkill(8, "Twister", DamageType.Wizardry, 35, 6, 0, 60, 40, 180, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 1, 0);
        this.CreateSkill(9, "Evil Spirit", DamageType.Wizardry, 45, 6, 0, 90, 50, 220, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(10, "Hellfire", DamageType.Wizardry, 120, 3, 0, 160, 60, 260, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0);
        this.CreateSkill(11, "Power Wave", DamageType.Wizardry, 14, 6, 0, 5, 9, 56, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0);
        this.CreateSkill(12, "Aqua Beam", DamageType.Wizardry, 80, 6, 0, 140, 74, 345, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0);
        this.CreateSkill(13, "Cometfall", DamageType.Wizardry, 70, 6, 0, 150, 80, 500, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0);
        this.CreateSkill(14, "Inferno", DamageType.Wizardry, 100, 4, 0, 200, 88, 724, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0);
        this.CreateSkill(15, "Teleport Ally", DamageType.Wizardry, 0, 6, 25, 90, 83, 644, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 2, 0);
        this.CreateSkill(16, "Soul Barrier", DamageType.Wizardry, 0, 6, 22, 70, 77, 408, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(17, "Energy Ball", DamageType.Wizardry, 3, 6, 0, 1, 2, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(18, "Defense", DamageType.Wizardry, 0, 0, 0, 30, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1);
        this.CreateSkill(19, "Falling Slash", DamageType.Wizardry, 0, 3, 0, 9, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1);
        this.CreateSkill(20, "Lunge", DamageType.Wizardry, 0, 2, 0, 9, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(21, "Uppercut", DamageType.Wizardry, 0, 2, 0, 8, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(22, "Cyclone", DamageType.Wizardry, 0, 2, 0, 9, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0);
        this.CreateSkill(23, "Slash", DamageType.Wizardry, 0, 2, 0, 10, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(24, "Triple Shot", DamageType.Wizardry, 0, 6, 0, 5, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(26, "Heal", DamageType.Wizardry, 0, 6, 0, 20, 8, 52, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(27, "Greater Defense", DamageType.Wizardry, 0, 6, 0, 30, 13, 72, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(28, "Greater Damage", DamageType.Wizardry, 0, 6, 0, 40, 18, 92, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(30, "Summon Goblin", DamageType.Wizardry, 0, 6, 0, 40, 0, 30, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(31, "Summon Stone Golem", DamageType.Wizardry, 0, 6, 0, 70, 0, 60, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(32, "Summon Assassin", DamageType.Wizardry, 0, 6, 0, 110, 0, 90, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(33, "Summon Elite Yeti", DamageType.Wizardry, 0, 6, 0, 160, 0, 130, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(34, "Summon Dark Knight", DamageType.Wizardry, 0, 6, 0, 200, 0, 170, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(35, "Summon Bali", DamageType.Wizardry, 0, 6, 0, 250, 0, 210, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(36, "Summon Soldier", DamageType.Wizardry, 0, 6, 0, 350, 0, 300, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(38, "Decay", DamageType.Wizardry, 95, 6, 7, 110, 96, 953, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0);
        this.CreateSkill(39, "Ice Storm", DamageType.Wizardry, 80, 6, 5, 100, 93, 849, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2, 0, 2, 0);
        this.CreateSkill(40, "Nova", DamageType.Wizardry, 0, 6, 45, 180, 100, 1052, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0);
        this.CreateSkill(41, "Twisting Slash", DamageType.Wizardry, 0, 2, 10, 10, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1);
        this.CreateSkill(42, "Anger Strike", DamageType.Wizardry, 60, 3, 20, 25, 170, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2);
        this.CreateSkill(43, "Death Stab", DamageType.Wizardry, 70, 3, 3, 15, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(44, "Crescent Moon Slash", DamageType.Wizardry, 90, 4, 15, 22, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0);
        this.CreateSkill(45, "Lance", DamageType.Wizardry, 90, 6, 10, 150, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 0, 1, 0);
        this.CreateSkill(46, "Starfall", DamageType.Wizardry, 120, 8, 15, 20, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(48, "Swell Life", DamageType.Wizardry, 0, 0, 24, 22, 120, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(49, "Fire Breath", DamageType.Wizardry, 30, 3, 0, 9, 110, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(50, "Flame of Evil (Monster)", DamageType.Wizardry, 120, 0, 0, 160, 60, 260, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(51, "Ice Arrow", DamageType.Wizardry, 105, 8, 12, 10, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(52, "Penetration", DamageType.Wizardry, 70, 6, 9, 7, 130, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(55, "Fire Slash", DamageType.Wizardry, 80, 3, 12, 17, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(56, "Power Slash", DamageType.Wizardry, 0, 5, 0, 15, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(57, "Spiral Slash", DamageType.Wizardry, 75, 5, 15, 20, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(60, "Force", DamageType.Wizardry, 10, 4, 0, 10, 0, 15, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(61, "Fire Burst", DamageType.Wizardry, 150, 6, 0, 8, 74, 79, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(62, "Earth-Shake", DamageType.Wizardry, 150, 10, 50, 0, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(63, "Summon", DamageType.Wizardry, 0, 0, 30, 70, 98, 153, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(64, "Dignity", DamageType.Wizardry, 0, 0, 50, 50, 82, 102, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(65, "Electric Spike", DamageType.Wizardry, 250, 10, 12, 10, 92, 126, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(66, "Force Wave", DamageType.Wizardry, 70, 4, 0, 10, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(67, "Stun", DamageType.Wizardry, 0, 2, 50, 70, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1);
        this.CreateSkill(68, "Cancel Stun", DamageType.Wizardry, 0, 0, 30, 25, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1);
        this.CreateSkill(69, "Swell Mana", DamageType.Wizardry, 0, 0, 30, 35, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1);
        this.CreateSkill(70, "Invisibility", DamageType.Wizardry, 0, 0, 60, 80, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1);
        this.CreateSkill(71, "Cancel Invisibility", DamageType.Wizardry, 0, 0, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1);
        this.CreateSkill(72, "Abolish Magic", DamageType.Wizardry, 0, 0, 70, 90, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(73, "Mana Rays", DamageType.Wizardry, 85, 6, 7, 130, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(74, "Fire Blast", DamageType.Wizardry, 150, 6, 10, 30, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
        this.CreateSkill(76, "Plasma Storm", DamageType.Wizardry, 60, 6, 20, 50, 110, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 2, 2, 2, 1, 1, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2);
        this.CreateSkill(77, "Infinity Arrow", DamageType.Wizardry, 0, 0, 10, 50, 220, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(78, "Fire Scream", DamageType.Wizardry, 180, 6, 3, 10, 102, 150, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(79, "Explosion", DamageType.Wizardry, 0, 2, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(200, "Summon Monster", DamageType.Wizardry, 0, 0, 0, 40, 0, 20, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(201, "Magic Attack Immunity", DamageType.Wizardry, 0, 0, 0, 40, 0, 20, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(202, "Physical Attack Immunity", DamageType.Wizardry, 0, 0, 0, 40, 0, 20, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(203, "Potion of Bless", DamageType.Wizardry, 0, 0, 0, 40, 0, 20, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(204, "Potion of Soul", DamageType.Wizardry, 0, 0, 0, 40, 0, 20, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(210, "Spell of Protection", DamageType.Wizardry, 0, 0, 0, 30, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1);
        this.CreateSkill(211, "Spell of Restriction", DamageType.Wizardry, 0, 3, 0, 30, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1);
        this.CreateSkill(212, "Spell of Pursuit", DamageType.Wizardry, 0, 0, 0, 30, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1);
        this.CreateSkill(213, "Shied-Burn", DamageType.Wizardry, 0, 3, 0, 30, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1);
        this.CreateSkill(214, "Drain Life", DamageType.Wizardry, 35, 6, 0, 50, 35, 150, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(215, "Chain Lightning", DamageType.Wizardry, 70, 6, 0, 85, 75, 245, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(217, "Damage Reflection", DamageType.Wizardry, 0, 5, 10, 40, 80, 375, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(218, "Berserker", DamageType.Wizardry, 0, 5, 50, 100, 83, 300, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(219, "Sleep", DamageType.Wizardry, 0, 6, 3, 20, 40, 180, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(221, "Weakness", DamageType.Wizardry, 0, 8, 15, 50, 93, 663, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(222, "Innovation", DamageType.Wizardry, 0, 8, 15, 50, 111, 663, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(223, "Explosion", DamageType.Wizardry, 40, 4, 0, 35, 50, 300, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(224, "Requiem", DamageType.Wizardry, 65, 4, 4, 60, 75, 416, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(225, "Pollution", DamageType.Wizardry, 80, 4, 8, 70, 85, 542, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(230, "Lightning Shock", DamageType.Wizardry, 95, 6, 7, 115, 93, 823, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(232, "Strike of Destruction", DamageType.Wizardry, 110, 5, 24, 30, 100, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);
        this.CreateSkill(233, "Expansion of Wizardry", DamageType.Wizardry, 0, 6, 50, 200, 220, 1058, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 2, 0, 2, 0);
        this.CreateSkill(234, "Recovery", DamageType.Wizardry, 0, 6, 10, 40, 100, 168, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(235, "Multi-Shot", DamageType.Wizardry, 40, 6, 7, 10, 100, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(236, "Flame Strike", DamageType.Wizardry, 140, 3, 25, 20, 100, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(237, "Gigantic Storm", DamageType.Wizardry, 110, 6, 10, 120, 220, 1058, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(238, "Chaotic Diseier", DamageType.Wizardry, 220, 6, 4, 12, 100, 84, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(239, "Doppeganger Self Explosion", DamageType.Wizardry, 140, 3, 25, 20, 100, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(240, "Magical Shot", DamageType.Wizardry, 30, 6, 0, 1, 2, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 1, 0);
        this.CreateSkill(241, "Shining Bird", DamageType.Wizardry, 130, 6, 0, 5, 30, 70, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateSkill(242, "Dragon Violent", DamageType.Wizardry, 140, 6, 0, 30, 50, 680, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateSkill(243, "Spear Storm", DamageType.Wizardry, 160, 6, 5, 105, 160, 1160, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(244, "Reflection Barrier", DamageType.Wizardry, 0, 6, 22, 70, 77, 408, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateSkill(245, "Marvel Burst", DamageType.Wizardry, 125, 6, 0, 5, 25, 104, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateSkill(246, "Unleash Marvel", DamageType.Wizardry, 135, 6, 0, 50, 45, 700, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateSkill(247, "Ultimate Force", DamageType.Wizardry, 10, 7, 4, 92, 160, 1073, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(248, "Reflection Barrier (Reflection Barrier)", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(249, "Illusion Avatar Attack", DamageType.Wizardry, 300, 7, 0, 0, 50, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(260, "Killing Blow", DamageType.Wizardry, 0, 2, 0, 9, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(261, "Beast Uppercut", DamageType.Wizardry, 0, 2, 0, 9, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(262, "Chain Drive", DamageType.Wizardry, 0, 4, 20, 15, 150, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(263, "Dark Side", DamageType.Wizardry, 0, 5, 0, 70, 180, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(264, "Dragon Roar", DamageType.Wizardry, 0, 3, 30, 50, 150, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(265, "Dragon Slasher", DamageType.Wizardry, 0, 4, 100, 100, 200, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(266, "Ignore Defense", DamageType.Wizardry, 0, 3, 10, 50, 120, 404, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(267, "Increase Health", DamageType.Wizardry, 0, 7, 10, 50, 80, 132, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(268, "Increase Block", DamageType.Wizardry, 0, 7, 10, 50, 50, 80, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(269, "Charge", DamageType.Wizardry, 90, 4, 15, 20, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(270, "Phoenix Shot", DamageType.Wizardry, 0, 4, 0, 30, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(271, "Spin Step", DamageType.Wizardry, 100, 2, 0, 12, 83, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(272, "Circle Shield", DamageType.Wizardry, 0, 0, 50, 100, 220, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(273, "Obsidian", DamageType.Wizardry, 0, 0, 50, 50, 74, 200, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(274, "Magic Pin", DamageType.Wizardry, 80, 2, 3, 5, 30, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(275, "Clash", DamageType.Wizardry, 50, 6, 50, 50, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(276, "Harsh Strike", DamageType.Wizardry, 100, 3, 0, 12, 74, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(277, "Shining Peak", DamageType.Wizardry, 50, 4, 5, 8, 92, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(278, "Wrath", DamageType.Wizardry, 0, 0, 30, 40, 66, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(279, "Breche", DamageType.Wizardry, 230, 5, 6, 15, 300, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(280, "Explosion", DamageType.Wizardry, 50, 2, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(281, "Magic Pin Explosion", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(282, "Spirit Hook", DamageType.Wizardry, 255, 3, 21, 27, 180, 0, 1480, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(283, "Magic Arrow", DamageType.Wizardry, 10, 8, 0, 5, 50, 60, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(284, "Plasma Ball", DamageType.Wizardry, 40, 8, 35, 120, 130, 300, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(285, "Lightning Storm", DamageType.Wizardry, 300, 8, 7, 80, 160, 1080, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(286, "Burst", DamageType.Wizardry, 0, 0, 0, 50, 200, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(287, "Haste", DamageType.Wizardry, 0, 0, 50, 0, 400, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(288, "Death Scythe", DamageType.Wizardry, 75, 6, 20, 120, 85, 930, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(289, "Darkness", DamageType.Wizardry, 0, 5, 50, 100, 83, 300, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(291, "Elite Monster Skill", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(292, "Sword Inertia", DamageType.Wizardry, 10, 6, 0, 5, 30, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(293, "Bat Flock", DamageType.Wizardry, 90, 6, 5, 20, 150, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(294, "Pierce Attack", DamageType.Wizardry, 170, 6, 10, 30, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(295, "Detection", DamageType.Wizardry, 0, 0, 100, 100, 350, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(297, "Demolish", DamageType.Wizardry, 0, 0, 0, 50, 400, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(298, "Blessing of Experience", DamageType.Wizardry, 0, 8, 0, 5, 50, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
        this.CreateSkill(300, "Durability Reduction (1)", DamageType.Wizardry, 37, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(301, "PvP Defense Rate Increase", DamageType.Wizardry, 12, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(302, "Maximum SD increase", DamageType.Wizardry, 13, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(303, "Auto Mana Recovery Increase", DamageType.Wizardry, 7, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(304, "Poison Resistance Increase", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 0, 0, 0, 3, 0, 3);
        this.CreateSkill(305, "Durability Reduction (2)", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(306, "SD Recovery Speed Increase", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(307, "Automatic HP Recovery Increase", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(308, "Lightning Resistance Increase", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 0, 0, 0, 3, 0, 3);
        this.CreateSkill(309, "Defense Increase", DamageType.Wizardry, 16, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(310, "Automatic AG Recovery Increase", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(311, "Ice Resistance Increase", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 0, 0, 0, 3, 0, 3);
        this.CreateSkill(312, "Durability Reduction (3)", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(313, "Defense Success Rate Increase", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(314, "Cast Invincibility", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 0, 0, 0, 3, 0, 3);
        this.CreateSkill(315, "Armor Set Bonus Increase", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(316, "Vengeance", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(317, "Energy Increase", DamageType.Wizardry, 40, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 0, 3, 3, 3, 3, 3);
        this.CreateSkill(318, "Stamina Increase", DamageType.Wizardry, 40, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 0, 3, 3, 3, 3, 3);
        this.CreateSkill(319, "Agility Increase", DamageType.Wizardry, 40, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 0, 3, 3, 3, 3, 3);
        this.CreateSkill(320, "Strength Increase", DamageType.Wizardry, 40, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 0, 3, 3, 3, 3, 3);
        this.CreateSkill(321, "Wing of Storm Absorption PowUp", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(322, "Wing of Storm Defense PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(323, "Iron Defense", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(324, "Wing of Storm Attack PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(325, "Attack Success Rate Increase", DamageType.Wizardry, 13, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(326, "Cyclone Strengthener", DamageType.Wizardry, 4, 2, 0, 9, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(327, "Slash Strengthener", DamageType.Wizardry, 40, 2, 0, 10, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(328, "Falling Slash Strengthener", DamageType.Wizardry, 17, 3, 0, 9, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(329, "Lunge Strengthener", DamageType.Wizardry, 17, 2, 0, 9, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(330, "Twisting Slash Strengthener", DamageType.Wizardry, 40, 2, 10, 10, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(331, "Anger Blow Strengthener", DamageType.Wizardry, 22, 3, 22, 25, 170, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(332, "Twisting Slash Mastery", DamageType.Wizardry, 1, 2, 20, 22, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(333, "Anger Blow Mastery", DamageType.Wizardry, 1, 3, 30, 50, 170, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(334, "Maximum Life Increase", DamageType.Wizardry, 9, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(335, "Weapon Mastery", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 3, 0, 0);
        this.CreateSkill(336, "Death Stab Strengthener", DamageType.Wizardry, 22, 4, 13, 15, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(338, "Maximum Mana Increase", DamageType.Wizardry, 9, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(339, "Death Stab Proficiency", DamageType.Wizardry, 22, 4, 26, 30, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(340, "Strike of Destruction Proficiency", DamageType.Wizardry, 22, 5, 24, 30, 100, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(341, "Maximum AG Increase", DamageType.Wizardry, 8, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(342, "Death Stab Mastery", DamageType.Wizardry, 7, 4, 26, 30, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(343, "Strike of Destruction Mastery", DamageType.Wizardry, 22, 5, 24, 30, 100, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(344, "Blood Storm", DamageType.Wizardry, 25, 3, 29, 87, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(345, "Combo Strengthener", DamageType.Wizardry, 7, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(346, "Blood Storm Strengthener", DamageType.Wizardry, 5, 3, 31, 95, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(347, "Attack Rate", DamageType.Wizardry, 14, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(348, "Two-handed Sword Strengthener", DamageType.Wizardry, 42, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(349, "One-handed Sword Strengthener", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(350, "Mace Strengthener", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(351, "Spear Strengthener", DamageType.Wizardry, 41, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(352, "Two-handed Sword Mastery", DamageType.Wizardry, 41, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(353, "One-handed Sword Mastery", DamageType.Wizardry, 23, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(354, "Mace Mastery", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(355, "Spear Mastery", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(356, "Swell Life Strengthener", DamageType.Wizardry, 7, 0, 26, 24, 120, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(357, "Mana Reduction", DamageType.Wizardry, 18, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(358, "Monster Attack SD Increment", DamageType.Wizardry, 11, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(359, "Monster Attack Life Increment", DamageType.Wizardry, 6, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(360, "Swell Life Proficiency", DamageType.Wizardry, 7, 0, 28, 26, 120, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(361, "Minimum Attack Power Increase", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 3, 3, 0, 0, 3, 0, 3, 3, 0, 0, 3, 0, 3);
        this.CreateSkill(362, "Monster Attack Mana Increment", DamageType.Wizardry, 6, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(363, "Swell Life Mastery", DamageType.Wizardry, 7, 0, 30, 28, 120, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(364, "Maximum Attack Power Increase", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 3, 3, 0, 0, 3, 0, 3, 3, 0, 0, 3, 0, 3);
        this.CreateSkill(366, "Increases critical damage rate", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(367, "Restores all Mana", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(368, "Restores all HP", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(369, "Increases excellent damage rate", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(370, "Increases double damage rate", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(371, "Increases chance of ignore Def", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(372, "Restores all SD", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(373, "Increases triple damage rate", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 0, 0, 3, 0, 0, 3, 3, 3, 3, 3);
        this.CreateSkill(374, "Eternal Wings Absorption PowUp", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(375, "Eternal Wings Defense PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(377, "Eternal Wings Attack PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(378, "Flame Strengthener", DamageType.Wizardry, 17, 6, 0, 55, 35, 160, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 0, 3, 0);
        this.CreateSkill(379, "Lightning Strengthener", DamageType.Wizardry, 40, 6, 0, 20, 13, 72, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 0, 0, 3, 0);
        this.CreateSkill(380, "Expansion of Wizardry Power Up", DamageType.Wizardry, 1, 6, 55, 220, 220, 1058, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 3, 0, 3, 0);
        this.CreateSkill(381, "Inferno Strengthener", DamageType.Wizardry, 4, 4, 0, 220, 88, 724, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 0, 3, 0);
        this.CreateSkill(382, "Blast Strengthener", DamageType.Wizardry, 17, 6, 0, 165, 80, 500, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 0, 0, 3, 0);
        this.CreateSkill(383, "Expansion of Wizardry Mastery", DamageType.Wizardry, 1, 6, 55, 220, 220, 1058, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 3, 0, 3, 0);
        this.CreateSkill(384, "Poison Strengthener", DamageType.Wizardry, 40, 6, 0, 46, 30, 140, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(385, "Evil Spirit Strengthener", DamageType.Wizardry, 4, 6, 0, 108, 50, 220, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(386, "Magic Mastery", DamageType.Wizardry, 22, 0, 0, 0, 50, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3, 3, 3, 0, 3, 0);
        this.CreateSkill(387, "Decay Strengthener", DamageType.Wizardry, 17, 6, 10, 120, 96, 953, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(388, "Hellfire Strengthener", DamageType.Wizardry, 40, 3, 0, 176, 60, 260, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(389, "Ice Strengthener", DamageType.Wizardry, 40, 6, 0, 42, 25, 120, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 3, 0, 3, 0);
        this.CreateSkill(390, "Meteor Strengthener", DamageType.Wizardry, 40, 6, 0, 13, 21, 104, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(391, "Ice Storm Strengthener", DamageType.Wizardry, 17, 6, 5, 110, 93, 849, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 0, 3, 0);
        this.CreateSkill(392, "Nova Strengthener", DamageType.Wizardry, 17, 6, 49, 198, 100, 1052, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 0, 0, 3, 0);
        this.CreateSkill(393, "Ice Storm Mastery", DamageType.Wizardry, 22, 6, 5, 110, 93, 849, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(394, "Meteor Mastery", DamageType.Wizardry, 1, 6, 0, 14, 21, 104, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(395, "Nova Cast Strengthener", DamageType.Wizardry, 22, 6, 49, 198, 100, 1052, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(397, "One-handed Staff Strengthener", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(398, "Two-handed Staff Strengthener", DamageType.Wizardry, 42, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(399, "Shield Strengthener", DamageType.Wizardry, 10, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(400, "One-handed Staff Mastery", DamageType.Wizardry, 23, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(401, "Two-handed Staff Mastery", DamageType.Wizardry, 42, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(402, "Shield Mastery", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(403, "Soul Barrier Strengthener", DamageType.Wizardry, 7, 6, 24, 77, 77, 408, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(404, "Soul Barrier Proficiency", DamageType.Wizardry, 10, 6, 26, 84, 77, 408, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(405, "Minimum Wizardry Increase", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 3, 0, 0, 0, 0, 3, 0, 3, 3, 3, 0, 3, 0);
        this.CreateSkill(406, "Soul Barrier Mastery", DamageType.Wizardry, 7, 6, 28, 92, 77, 408, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(407, "Maximum Wizardry Increase", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 3, 0, 0, 0, 0, 3, 0, 3, 3, 3, 0, 3, 0);
        this.CreateSkill(409, "Illusion Wings Absorption PowUp", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(410, "Illusion Wings Defense PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(411, "Multi-Shot Strengthener", DamageType.Wizardry, 22, 6, 7, 11, 100, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(412, "Illusion Wings Attack PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(413, "Heal Strengthener", DamageType.Wizardry, 22, 6, 0, 22, 8, 52, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(414, "Triple Shot Strengthener", DamageType.Wizardry, 4, 6, 0, 5, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(415, "Summoned Monster Power Up(1)", DamageType.Wizardry, 16, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(416, "Penetration Strengthener", DamageType.Wizardry, 17, 6, 11, 10, 130, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(417, "Defense Increase Strengthener", DamageType.Wizardry, 22, 6, 0, 33, 13, 72, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(418, "Triple Shot Mastery", DamageType.Wizardry, 0, 6, 0, 9, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(419, "Summoned Monster Power Up(2)", DamageType.Wizardry, 16, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(420, "Attack Increase Strengthener", DamageType.Wizardry, 22, 6, 0, 44, 18, 92, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(421, "Weapon Mastery", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(422, "Attack Increase Mastery", DamageType.Wizardry, 22, 6, 0, 48, 18, 92, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(423, "Defense Increase Mastery", DamageType.Wizardry, 22, 6, 0, 36, 13, 72, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(424, "Ice Arrow Strengthener", DamageType.Wizardry, 22, 8, 18, 15, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(425, "Cure", DamageType.Wizardry, 0, 6, 10, 72, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(426, "Party Healing", DamageType.Wizardry, 0, 6, 12, 266, 0, 20, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(427, "Poison Arrow", DamageType.Wizardry, 27, 6, 50, 50, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(428, "Summoned Monster Power Up(3)", DamageType.Wizardry, 16, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(429, "Party Healing Strengthener", DamageType.Wizardry, 22, 6, 13, 272, 0, 20, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(430, "Bless", DamageType.Wizardry, 0, 6, 18, 108, 0, 20, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(431, "Multi-Shot Mastery", DamageType.Wizardry, 1, 6, 8, 12, 100, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(432, "Summon Satyros", DamageType.Wizardry, 0, 6, 52, 525, 0, 300, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(433, "Bless Strengthener", DamageType.Wizardry, 10, 6, 20, 118, 0, 20, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(434, "Poison Arrow Strengthener", DamageType.Wizardry, 40, 6, 50, 50, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(435, "Bow Strengthener", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(436, "Crossbow Strengthener", DamageType.Wizardry, 4, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(437, "Shield Strengthener", DamageType.Wizardry, 10, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(438, "Bow Mastery", DamageType.Wizardry, 23, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(439, "Crossbow Mastery", DamageType.Wizardry, 5, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(440, "Shield Mastery", DamageType.Wizardry, 15, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(441, "Infinity Arrow Strengthener", DamageType.Wizardry, 1, 0, 11, 55, 220, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(442, "Minimum Attack Power Increase", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(443, "Maximum Attack Power Increase", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(445, "Dimension Wings Absorb PowUp", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(446, "Dimension Wings Defense PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(447, "Dimension Wings Attack PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(451, "Fire Tome Mastery", DamageType.Wizardry, 7, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(452, "Earth Tome Mastery", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(453, "Wind Tome Mastery", DamageType.Wizardry, 7, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(454, "Sleep Strengthener", DamageType.Wizardry, 1, 6, 7, 30, 40, 180, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(455, "Chain Lightning Strengthener", DamageType.Wizardry, 22, 6, 0, 103, 75, 245, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(456, "Lightning Shock Strengthener", DamageType.Wizardry, 22, 6, 10, 125, 93, 823, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(457, "Magic Mastery", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(458, "Drain Life Strengthener", DamageType.Wizardry, 22, 6, 0, 57, 35, 150, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(459, "Weakness Strengthener", DamageType.Wizardry, 3, 8, 15, 50, 93, 663, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(460, "Innovation Strengthener", DamageType.Wizardry, 3, 8, 15, 50, 111, 663, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(461, "Blind", DamageType.Wizardry, 0, 3, 25, 115, 0, 20, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(462, "Drain Life Mastery", DamageType.Wizardry, 9, 6, 0, 62, 35, 150, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(463, "Blind Strengthener", DamageType.Wizardry, 1, 3, 27, 126, 0, 20, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(465, "Stick Strengthener", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(466, "Other World Tome Strengthener", DamageType.Wizardry, 4, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(467, "Stick Mastery", DamageType.Wizardry, 5, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(468, "Other World Tome Mastery", DamageType.Wizardry, 23, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(469, "Berserker Strengthener", DamageType.Wizardry, 172, 5, 82, 165, 83, 300, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(470, "Berserker Proficiency", DamageType.Wizardry, 173, 5, 90, 181, 83, 300, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(471, "Minimum Wizardry/Curse Increase", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(473, "Maximum Wizardry/Curse Increase", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(475, "Wing of Ruin Absorption PowUp", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(476, "Wing of Ruin Defense PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(478, "Wing of Ruin Attack PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(479, "Cyclone Strengthener", DamageType.Wizardry, 4, 2, 0, 9, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(480, "Lightning Strengthener", DamageType.Wizardry, 40, 6, 0, 20, 13, 72, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(481, "Twisting Slash Strengthener", DamageType.Wizardry, 40, 2, 10, 10, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(482, "Power Slash Strengthener", DamageType.Wizardry, 17, 5, 0, 15, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(483, "Flame Strengthener", DamageType.Wizardry, 17, 6, 0, 55, 35, 160, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(484, "Blast Strengthener", DamageType.Wizardry, 17, 6, 0, 165, 80, 500, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(485, "Weapon Mastery", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(486, "Inferno Strengthener", DamageType.Wizardry, 4, 4, 0, 220, 88, 724, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(487, "Evil Spirit Strengthener", DamageType.Wizardry, 4, 6, 0, 108, 50, 220, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(488, "Magic Mastery", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(489, "Ice Strengthener", DamageType.Wizardry, 40, 6, 0, 42, 25, 120, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(490, "Fire Slash Strengthener", DamageType.Wizardry, 3, 3, 12, 15, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(491, "Ice Mastery", DamageType.Wizardry, 1, 6, 0, 46, 25, 120, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(492, "Flame Strike Strengthener", DamageType.Wizardry, 4, 3, 37, 30, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(493, "Fire Slash Mastery", DamageType.Wizardry, 7, 3, 12, 17, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(494, "Flame Strike Mastery", DamageType.Wizardry, 7, 3, 40, 33, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(495, "Earth Prison", DamageType.Wizardry, 26, 3, 15, 180, 0, 20, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 3, 0, 0, 0, 0, 3, 0, 0, 3, 3, 0, 3, 0);
        this.CreateSkill(496, "Gigantic Storm Strengthener", DamageType.Wizardry, 3, 6, 10, 132, 220, 1058, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(497, "Earth Prison Strengthener", DamageType.Wizardry, 4, 3, 17, 198, 0, 20, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 3, 0, 0, 0, 0, 3, 0, 0, 3, 3, 0, 3, 0);
        this.CreateSkill(504, "Emperor Cape Absorption PowUp", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(505, "Emperor Cape Defense PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(506, "Adds Command Stat", DamageType.Wizardry, 40, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(507, "Emperor Cape Attack PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(508, "Fire Burst Strengthener", DamageType.Wizardry, 4, 6, 0, 13, 74, 79, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(510, "Horse Strengthener", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(511, "Lord Dignity Strengthener", DamageType.Wizardry, 321, 0, 65, 65, 82, 102, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(512, "Earth-Shake Strengthener", DamageType.Wizardry, 4, 10, 75, 0, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(513, "Weapon Mastery", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(514, "Fire Burst Mastery", DamageType.Wizardry, 1, 6, 0, 13, 74, 79, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(518, "Fire Scream Strengthener", DamageType.Wizardry, 22, 6, 5, 14, 102, 150, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(519, "Electric Spark Strengthener", DamageType.Wizardry, 3, 10, 35, 10, 92, 126, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(520, "Fire Scream Mastery", DamageType.Wizardry, 5, 6, 5, 14, 102, 150, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(523, "Chaotic Diseier Strengthener", DamageType.Wizardry, 22, 6, 8, 15, 100, 84, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(527, "Scepter Strengthener", DamageType.Wizardry, 305, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(528, "Shield Strengthener", DamageType.Wizardry, 10, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(529, "Scepter Strengthener: Spirit", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(530, "Spirit Critical DMG Probability Increase", DamageType.Wizardry, 7, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(531, "Scepter Mastery", DamageType.Wizardry, 5, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(532, "Shield Mastery", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(533, "Command Attack Increase", DamageType.Wizardry, 20, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(534, "Spirit Excellent DMG Probability Increase", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(538, "Spirit Noble", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(539, "Spirit Lord", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(548, "Reigning Cloak Absorption PowUp", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(549, "Reigning Cloak Defense PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(550, "Reigning Cloak Attack PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(551, "Killing Blow Strengthener", DamageType.Wizardry, 22, 2, 0, 10, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(552, "Beast Uppercut Strengthener", DamageType.Wizardry, 22, 2, 0, 10, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(554, "Killing Blow Mastery", DamageType.Wizardry, 1, 2, 0, 10, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(555, "Beast Uppercut Mastery", DamageType.Wizardry, 1, 2, 0, 10, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(556, "Def Success Rate Strengthener", DamageType.Wizardry, 5, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(557, "Weapon Mastery", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(558, "Chain Drive Strengthener", DamageType.Wizardry, 22, 4, 22, 22, 150, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(559, "Dark Side Strengthener", DamageType.Wizardry, 22, 5, 0, 84, 180, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(560, "Dragon Roar Strengthener", DamageType.Wizardry, 22, 3, 33, 60, 150, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(561, "Dragon Roar Mastery", DamageType.Wizardry, 1, 3, 36, 66, 150, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(562, "Chain Drive Mastery", DamageType.Wizardry, 1, 4, 22, 24, 150, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(563, "Dark Side Mastery", DamageType.Wizardry, 0, 6, 0, 92, 180, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(564, "Dragon Slasher Strengthener", DamageType.Wizardry, 22, 4, 110, 110, 200, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(565, "Blood Howling", DamageType.Wizardry, 0, 0, 200, 100, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(566, "Dragon Slasher Mastery", DamageType.Wizardry, 38, 4, 121, 121, 200, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(567, "Blood Howling Strengthener", DamageType.Wizardry, 38, 0, 220, 110, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(568, "Equipped Weapon Strengthener", DamageType.Wizardry, 305, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(569, "Defense Success Rate Increase PowUp", DamageType.Wizardry, 22, 7, 11, 55, 50, 80, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(571, "Equipped Weapon Mastery", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(572, "DefSuccessRate Increase Mastery", DamageType.Wizardry, 22, 7, 12, 60, 50, 80, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(573, "Stamina Increase Strengthener", DamageType.Wizardry, 5, 7, 11, 55, 80, 132, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(574, "Defense Switch", DamageType.Wizardry, 20, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(578, "Durability Reduction (1)", DamageType.Wizardry, 37, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(579, "Increase PvP Defense Rate", DamageType.Wizardry, 29, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(580, "Increase Maximum SD", DamageType.Wizardry, 33, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(581, "Increase Mana Recovery Rate", DamageType.Wizardry, 7, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(582, "Increase Poison Resistance", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(583, "Durability Reduction (2)", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(584, "Increase SD Recovery Rate", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(585, "Increase HP Recovery Rate", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(586, "Increase Lightning Resistance", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(587, "Increases Defense", DamageType.Wizardry, 35, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(588, "Increases AG Recovery Rate", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(589, "Increase Ice Resistance", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(590, "Durability Reduction(3)", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(591, "Increase Defense Success Rate", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(592, "Cast Invincibility", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(593, "Increase Set Defense", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(594, "Vengeance", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(595, "Increase Energy", DamageType.Wizardry, 36, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(596, "Stamina Increases", DamageType.Wizardry, 36, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(597, "Increase Agility", DamageType.Wizardry, 36, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(598, "Increase Strength", DamageType.Wizardry, 36, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(599, "Increase Attack Success Rate", DamageType.Wizardry, 13, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(600, "Increase Maximum HP", DamageType.Wizardry, 34, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(601, "Increase Maximum Mana", DamageType.Wizardry, 34, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(602, "Increase Maximum AG", DamageType.Wizardry, 37, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(603, "Increase PvP Attack Rate", DamageType.Wizardry, 31, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(604, "Decrease Mana", DamageType.Wizardry, 18, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(605, "Recover SD from Monster Kills", DamageType.Wizardry, 11, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(606, "Recover HP from Monster Kills", DamageType.Wizardry, 6, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(607, "Increase Minimum Attack Power", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(608, "Recover Mana from Monster Kills", DamageType.Wizardry, 6, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(609, "Increase Maximum Attack Power", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(610, "Increases Critical DMG Chance", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(611, "Recover Mana Fully", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(612, "Recovers HP Fully", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(613, "Increase Excellent DMG Chance", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(614, "Increase Double Damage Chance", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(615, "Increase Ignore Def Chance", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(616, "Recovers SD Fully", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(617, "Increase Triple Damage Chance", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(618, "Spell of Protection", DamageType.Wizardry, 0, 0, 0, 30, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1);
        this.CreateSkill(619, "Spell of Restriction", DamageType.Wizardry, 0, 3, 0, 30, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1);
        this.CreateSkill(620, "Spell of Pursuit", DamageType.Wizardry, 0, 0, 0, 30, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1);
        this.CreateSkill(621, "Shied-Burn", DamageType.Wizardry, 0, 3, 0, 30, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1);
        this.CreateSkill(623, "Protection Shield", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3);
        this.CreateSkill(624, "Weapon Block", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 0, 3);
        this.CreateSkill(625, "Shield Block", DamageType.Wizardry, 39, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3);
        this.CreateSkill(626, "Steel Armor", DamageType.Wizardry, 35, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 0, 3);
        this.CreateSkill(627, "Strong Mind", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3);
        this.CreateSkill(628, "Absorb Life", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 0, 3);
        this.CreateSkill(629, "Absorb Shield", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 0, 3);
        this.CreateSkill(630, "Battle Mind", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 0, 3);
        this.CreateSkill(631, "Rush", DamageType.Wizardry, 178, 7, 200, 200, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(634, "Protection Shield", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 3, 0);
        this.CreateSkill(635, "Weapon Block", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 3, 0);
        this.CreateSkill(636, "Shield Block", DamageType.Wizardry, 39, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 3, 0);
        this.CreateSkill(637, "Steel Armor", DamageType.Wizardry, 35, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 3, 0);
        this.CreateSkill(638, "Strong Mind", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 3, 0);
        this.CreateSkill(639, "Absorb Life", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 3, 0);
        this.CreateSkill(640, "SD Absorption", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 3, 0);
        this.CreateSkill(641, "Grand Magic PowUp", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 3, 0);
        this.CreateSkill(642, "Illusion", DamageType.Wizardry, 0, 6, 300, 1000, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 0);
        this.CreateSkill(643, "Protection Shield", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(644, "Weapon Block", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(645, "Shield Block", DamageType.Wizardry, 39, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(646, "Steel Armor", DamageType.Wizardry, 35, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(647, "Strong Mind", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(648, "Absorb Life", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(649, "SD Absorption", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(650, "Marksman", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(651, "Shadow Step", DamageType.Wizardry, 0, 5, 100, 150, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(652, "Evasion", DamageType.Wizardry, 0, 0, 200, 200, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(653, "Protection Shield", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(654, "Weapon Block", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(655, "Shield Block", DamageType.Wizardry, 39, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(656, "Steel Armor", DamageType.Wizardry, 35, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(657, "Strong Mind", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(658, "Absorb Life", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(659, "SD Absorption", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(660, "Pain of Curse", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(663, "Protection Shield", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(664, "Weapon Block", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(665, "Shield Block", DamageType.Wizardry, 39, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(666, "Steel Armor", DamageType.Wizardry, 35, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(667, "Strong Mind", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(668, "Absorb Life", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(669, "SD Absorption", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(670, "Protection Shield", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(671, "Weapon Block", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(672, "Shield Block", DamageType.Wizardry, 39, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(673, "Steel Armor", DamageType.Wizardry, 35, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(674, "Strong Mind", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(675, "Absorb Life", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(676, "SD Absorption", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(677, "Protection Shield", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(678, "Weapon Block", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(679, "Shield Block", DamageType.Wizardry, 39, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(680, "Steel Armor", DamageType.Wizardry, 35, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(681, "Strong Mind", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(682, "Absorb Life", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(683, "SD Absorption", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(684, "Cloak of Transcendence Absorption PowUp", DamageType.Wizardry, 38, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(685, "Cloak of Transcendence Defense PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(686, "Cloak of Transcendence Attack PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(687, "Spin Step Strengthener", DamageType.Wizardry, 3, 2, 0, 14, 83, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(688, "Harsh Strike Strengthener", DamageType.Wizardry, 4, 3, 0, 14, 74, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(689, "Weapon Mastery", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(690, "Spin Step Mastery", DamageType.Wizardry, 4, 2, 0, 16, 83, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(691, "Harsh Strike Mastery", DamageType.Wizardry, 0, 3, 0, 16, 74, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(692, "Magic Pin Strengthener", DamageType.Wizardry, 4, 2, 10, 17, 30, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(693, "Obsidian Strengthener", DamageType.Wizardry, 18, 0, 50, 50, 74, 200, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(695, "Magic Pin Mastery", DamageType.Wizardry, 0, 3, 13, 20, 30, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(696, "Breche Strengthener", DamageType.Wizardry, 4, 5, 10, 16, 300, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(698, "Breche Mastery", DamageType.Wizardry, 0, 6, 10, 16, 300, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(699, "Shining Peak Strengthener", DamageType.Wizardry, 116, 4, 7, 10, 220, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(700, "Burst", DamageType.Wizardry, 38, 0, 50, 200, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(701, "Burst PowUp", DamageType.Wizardry, 23, 0, 52, 210, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(702, "Lance PowUp", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(703, "Circle Shield PowUp", DamageType.Wizardry, 10, 0, 50, 100, 220, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(704, "Shield PowUp", DamageType.Wizardry, 10, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(705, "Lance Mastery", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(706, "Circle Shield Mastery", DamageType.Wizardry, 23, 0, 50, 100, 220, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(707, "Shield Mastery", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(708, "Wrath Strengthener", DamageType.Wizardry, 23, 0, 50, 110, 66, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(709, "Wrath Proficiency", DamageType.Wizardry, 23, 0, 60, 130, 66, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(710, "Wrath Mastery", DamageType.Wizardry, 38, 0, 80, 150, 66, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(711, "Increases Retaliation DMG", DamageType.Wizardry, 23, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(712, "Increases Rage DMG", DamageType.Wizardry, 23, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(713, "Protection Shield", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(714, "Weapon Block", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(715, "Shield Block", DamageType.Wizardry, 39, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(716, "Steel Armor", DamageType.Wizardry, 35, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(717, "Strong Mind", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(718, "Absorb Life", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(719, "SD Absorption", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(720, "Immune I", DamageType.Wizardry, 0, 0, 200, 300, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(721, "Immune II", DamageType.Wizardry, 0, 0, 200, 300, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(722, "Berserker I", DamageType.Wizardry, 0, 0, 300, 500, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(723, "Fire Blow", DamageType.Wizardry, 120, 5, 17, 17, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(724, "Meteor Strike", DamageType.Wizardry, 100, 7, 17, 92, 160, 1073, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(725, "Meteor Storm", DamageType.Wizardry, 105, 6, 20, 105, 160, 1160, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(726, "Soul Seeker", DamageType.Wizardry, 125, 7, 16, 90, 160, 1115, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(727, "Focus Shot", DamageType.Wizardry, 110, 7, 9, 12, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(729, "Fire Beast", DamageType.Wizardry, 100, 7, 10, 95, 160, 1220, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(730, "Aqua Beast", DamageType.Wizardry, 100, 7, 10, 95, 160, 1220, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(731, "Ice Blood", DamageType.Wizardry, 330, 3, 14, 19, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(732, "Fire Blood", DamageType.Wizardry, 330, 3, 14, 19, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(733, "Dark Blast", DamageType.Wizardry, 120, 7, 17, 92, 160, 1073, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(734, "Meteor Strike", DamageType.Wizardry, 120, 7, 17, 92, 160, 1073, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(737, "Wind Soul", DamageType.Wizardry, 130, 6, 17, 35, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(739, "Dark Phoenix Shot", DamageType.Wizardry, 650, 7, 10, 30, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(740, "Archangel's will", DamageType.Wizardry, 0, 0, 150, 100, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
        this.CreateSkill(743, "Max HP Boost", DamageType.Wizardry, 43, 0, 0, 0, 160, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(744, "Enhance Phoenix Shot", DamageType.Wizardry, 22, 4, 0, 30, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(745, "Phoenix Shot Mastery", DamageType.Wizardry, 1, 4, 0, 30, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(746, "Elemental DEF Increase", DamageType.Wizardry, 162, 0, 0, 0, 160, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3);
        this.CreateSkill(747, "Pentagram Elemental Defense Increase", DamageType.Wizardry, 163, 0, 0, 0, 160, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3);
        this.CreateSkill(748, "Magic Arrow Strengthener", DamageType.Wizardry, 22, 8, 0, 25, 160, 60, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(749, "Magic Arrow Mastery", DamageType.Wizardry, 22, 8, 0, 35, 160, 60, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(750, "Plasma Ball Strengthener", DamageType.Wizardry, 22, 8, 45, 130, 160, 300, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(751, "Plasma Ball Mastery", DamageType.Wizardry, 22, 8, 55, 135, 160, 300, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(752, "Rune Mace Strengthener", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(753, "Rune Mace Mastery", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(754, "Wings of Disillusion Defense Strengthener", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(755, "Wings of Disillusion Attack Strengthener", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(756, "Shield Block", DamageType.Wizardry, 39, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(758, "Protection Shield", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(759, "Steel Armor", DamageType.Wizardry, 35, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(760, "Strong Mind", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(761, "Absorb Life", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(762, "Absorb Shield", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(763, "Grand Magic Power Up", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(765, "Burst Strengthener", DamageType.Wizardry, 23, 0, 0, 60, 200, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(766, "Burst Mastery", DamageType.Wizardry, 243, 0, 0, 80, 200, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(768, "Haste Strengthener", DamageType.Wizardry, 23, 0, 60, 0, 400, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(769, "Haste Mastery", DamageType.Wizardry, 243, 0, 80, 0, 400, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(770, "Darkness Strengthener", DamageType.Wizardry, 174, 5, 50, 100, 83, 620, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(771, "Darkness Mastery", DamageType.Wizardry, 175, 5, 50, 100, 83, 620, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(772, "Greatness Mastery", DamageType.Wizardry, 177, 8, 15, 50, 111, 663, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(773, "Innovation Mastery", DamageType.Wizardry, 177, 8, 15, 50, 111, 663, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(774, "Explosion Strengthener", DamageType.Wizardry, 3, 4, 0, 35, 0, 80, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(775, "Requiem Strengthener", DamageType.Wizardry, 3, 4, 4, 60, 0, 140, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(776, "Pollution Strengthener", DamageType.Wizardry, 3, 4, 8, 70, 0, 542, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(777, "Pollution Strengthener", DamageType.Wizardry, 22, 5, 8, 70, 85, 542, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(778, "Pollution Mastery", DamageType.Wizardry, 22, 5, 8, 70, 100, 542, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(779, "Sword Inertia Strengthener", DamageType.Wizardry, 22, 6, 5, 7, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(780, "Sword Inertia Mastery", DamageType.Wizardry, 0, 7, 7, 10, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(781, "Bat Flock Strengthener", DamageType.Wizardry, 22, 6, 9, 25, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(782, "Bat Flock Mastery", DamageType.Wizardry, 23, 6, 12, 30, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(783, "Short Sword Strengthener", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(784, "Short Sword Mastery", DamageType.Wizardry, 23, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(785, "Wings of Silence Defense Strengthener", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(786, "Wings of Silence Attack Strengthener", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(787, "Demolish Strengthener", DamageType.Wizardry, 23, 0, 0, 60, 400, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(788, "Demolish Mastery", DamageType.Wizardry, 214, 0, 0, 80, 400, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(789, "Weapon Mastery", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(790, "Steel Armor", DamageType.Wizardry, 35, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(791, "Weapon Block", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 0, 3);
        this.CreateSkill(792, "Life Absorption", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(793, "SD Absorption", DamageType.Wizardry, 3, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 0, 3);
        this.CreateSkill(794, "Detection Strengthener", DamageType.Wizardry, 0, 0, 100, 100, 350, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(801, "Sword's Fury Strengthener", DamageType.Wizardry, 226, 0, 17, 21, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(802, "Sword's Fury Mastery", DamageType.Wizardry, 227, 0, 17, 21, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(803, "Solid Protection Strengthener", DamageType.Wizardry, 228, 0, 20, 65, 0, 1052, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(804, "Solid Protection Proficiency", DamageType.Wizardry, 229, 0, 20, 65, 0, 1052, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(805, "Solid Protection Mastery", DamageType.Wizardry, 230, 0, 20, 65, 0, 1052, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(806, "Solid Protection Mastery", DamageType.Wizardry, 231, 0, 20, 65, 0, 1052, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(807, "Strike of Destruction Strengthener", DamageType.Wizardry, 232, 5, 24, 30, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(809, "Strike of Destruction Mastery", DamageType.Wizardry, 234, 5, 24, 30, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(810, "Strong Belief Strengthener", DamageType.Wizardry, 22, 0, 25, 80, 0, 1040, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(811, "Tornado Strengthener", DamageType.Wizardry, 40, 2, 10, 10, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(812, "Anger Blow Strengthener", DamageType.Wizardry, 22, 3, 22, 25, 170, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(813, "Rush", DamageType.Wizardry, 178, 7, 200, 200, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(814, "Increase Energy Stat", DamageType.Wizardry, 245, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(815, "Increase Vitality Stat", DamageType.Wizardry, 246, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(816, "Increase Agility Stat", DamageType.Wizardry, 247, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(817, "Increase Strength Stat", DamageType.Wizardry, 248, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(818, "Wings of Hit Defense Strengthener", DamageType.Wizardry, 249, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(819, "Wings of Hit Attack Strengthener", DamageType.Wizardry, 250, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(820, "Dark Plasma Strengthener", DamageType.Wizardry, 251, 6, 15, 22, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(821, "Dark Plasma Proficiency", DamageType.Wizardry, 252, 6, 15, 22, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(822, "Dark Plasma Mastery", DamageType.Wizardry, 253, 6, 15, 22, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(823, "Ice Break Strengthener", DamageType.Wizardry, 254, 6, 8, 17, 0, 295, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(824, "Ice Break Mastery", DamageType.Wizardry, 255, 6, 8, 17, 0, 295, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(825, "Death Fire Strengthener", DamageType.Wizardry, 256, 6, 10, 8, 0, 100, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(826, "Death Fire Mastery", DamageType.Wizardry, 257, 7, 10, 8, 0, 100, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(828, "Fixed Fire Strengthener", DamageType.Wizardry, 285, 0, 50, 100, 0, 300, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(829, "Fixed Fire Mastery", DamageType.Wizardry, 286, 0, 50, 100, 0, 300, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(831, "Magic Gun Strengthener", DamageType.Wizardry, 262, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(832, "Magic Gun Mastery", DamageType.Wizardry, 263, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(835, "Death Ice Strengthener", DamageType.Wizardry, 269, 6, 10, 8, 0, 100, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(836, "Death Ice Mastery", DamageType.Wizardry, 270, 7, 10, 8, 0, 100, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(837, "Cloak of Brilliance Defense PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(838, "Cloak of Brilliance Attack PowUp", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(839, "Steel Armor", DamageType.Wizardry, 35, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(840, "Eternal Wings Defense Strengthener", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(841, "Eternal Wings Attack Strengthener", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(842, "Steel Armor", DamageType.Wizardry, 35, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(843, "Shining Bird Strengthener", DamageType.Wizardry, 22, 6, 0, 20, 150, 70, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(844, "Shining Bird Mastery", DamageType.Wizardry, 0, 7, 0, 30, 150, 70, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(845, "Magic Mastery", DamageType.Wizardry, 22, 0, 0, 0, 50, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(846, "Dragon Violent Strengthener", DamageType.Wizardry, 22, 6, 0, 50, 160, 680, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(847, "Dragon Violent Mastery", DamageType.Wizardry, 0, 7, 0, 70, 160, 680, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(848, "Marvel Burst Strengthener", DamageType.Wizardry, 4, 6, 0, 20, 140, 104, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(849, "Marvel Burst Mastery", DamageType.Wizardry, 0, 6, 0, 30, 140, 104, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(850, "Magic Mastery", DamageType.Wizardry, 22, 0, 0, 0, 50, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(851, "Beginner Defense Improvement Strengthener", DamageType.Wizardry, 271, 6, 0, 33, 13, 72, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(852, "Beginner Defense Improvement Mastery", DamageType.Wizardry, 271, 6, 0, 36, 13, 72, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(853, "Beginner Attack Power Improvement Strengthener", DamageType.Wizardry, 271, 6, 0, 44, 18, 92, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(854, "Beginner Attack Improvement Mastery", DamageType.Wizardry, 271, 6, 0, 48, 18, 92, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(855, "Unleash Marvel Strengthener", DamageType.Wizardry, 22, 6, 0, 65, 155, 700, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(856, "Unleash Marvel Mastery", DamageType.Wizardry, 0, 7, 0, 70, 155, 700, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(857, "Beginner Bless Strengthener", DamageType.Wizardry, 272, 6, 20, 118, 10, 150, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(858, "Intensive Care Strengthener", DamageType.Wizardry, 271, 6, 0, 22, 8, 52, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(859, "Magic Book Strengthener", DamageType.Wizardry, 23, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(860, "Magic Book Mastery", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(861, "Reflection Barrier Strengthener", DamageType.Wizardry, 7, 6, 24, 77, 77, 408, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(862, "Reflection Barrier Skills", DamageType.Wizardry, 10, 6, 26, 84, 77, 408, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(863, "Reflection Barrier Mastery", DamageType.Wizardry, 7, 6, 28, 92, 77, 408, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateSkill(864, "Orb Strengthener", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(865, "Spiral Charge Strengthener", DamageType.Wizardry, 275, 0, 17, 21, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(866, "Spiral Charge Mastery", DamageType.Wizardry, 278, 0, 17, 21, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(867, "Crusher Charge Strengthener", DamageType.Wizardry, 276, 0, 17, 21, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(868, "Crusher Charge Mastery", DamageType.Wizardry, 279, 0, 17, 21, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(869, "Elemental Charge Strengthener", DamageType.Wizardry, 277, 0, 20, 65, 0, 1073, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(870, "Elemental Charge Mastery", DamageType.Wizardry, 280, 0, 20, 65, 0, 1073, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(871, "Two-handed Sword Strengthener", DamageType.Wizardry, 281, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(872, "Two-handed Staff Strengthener", DamageType.Wizardry, 282, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(873, "Two-handed Sword Mastery", DamageType.Wizardry, 283, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(874, "Two-handed Staff Mastery", DamageType.Wizardry, 284, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(875, "Orb Mastery", DamageType.Wizardry, 23, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateSkill(876, "Holy Bolt Strengthener", DamageType.Wizardry, 41, 6, 0, 20, 120, 1200, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(877, "Charge Slash Strengthener", DamageType.Wizardry, 294, 6, 0, 3, 25, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(878, "Charge Slash Mastery", DamageType.Wizardry, 295, 7, 0, 3, 25, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(879, "Wind Glaive Strengthener", DamageType.Wizardry, 296, 5, 0, 6, 45, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(880, "Wind Glaive Mastery", DamageType.Wizardry, 297, 6, 0, 6, 45, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(881, "Blade Storm Strengthener", DamageType.Wizardry, 298, 6, 10, 10, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(882, "Blade Storm Mastery", DamageType.Wizardry, 299, 6, 10, 10, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(883, "Illusion Avatar Strengthener", DamageType.Wizardry, 300, 6, 0, 40, 100, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(884, "Illusion Avatar Mastery", DamageType.Wizardry, 301, 6, 0, 40, 100, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(885, "Illusion Blade Strengthener", DamageType.Wizardry, 302, 0, 0, 40, 25, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(886, "Illusion Blade Mastery", DamageType.Wizardry, 303, 0, 0, 40, 25, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(887, "Illusion Blade Mastery", DamageType.Wizardry, 304, 0, 0, 40, 25, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(888, "Stop Weapon", DamageType.Wizardry, 2, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(889, "Steel Armor", DamageType.Wizardry, 35, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(890, "Reinforcement of Cloak of Death Defense", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(891, "Reinforcement of Cloak of Death Attack", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(892, "Blade Strengthener", DamageType.Wizardry, 305, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(893, "Blade Mastery", DamageType.Wizardry, 306, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(894, "Oversting Strengthener", DamageType.Wizardry, 313, 6, 12, 25, 300, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(895, "Wrath Strengthener", DamageType.Wizardry, 312, 0, 40, 50, 66, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(896, "Wild Breath Strengthener", DamageType.Wizardry, 315, 6, 18, 35, 300, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(897, "Reinforcement of Wing of the Guardian Defense", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(898, "Reinforcement of Wing of the Guardian Attack", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(899, "Steel Armor", DamageType.Wizardry, 35, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(900, "Angel Homunculus Strengthener", DamageType.Wizardry, 22, 6, 0, 20, 150, 70, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(901, "Angel Homunculus Mastery", DamageType.Wizardry, 0, 6, 0, 30, 150, 70, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(902, "Magic Mastery", DamageType.Wizardry, 22, 0, 0, 0, 50, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(903, "Ignition Bomber Strengthener", DamageType.Wizardry, 22, 6, 0, 50, 160, 680, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(904, "Ignition Bomber Mastery", DamageType.Wizardry, 0, 7, 5, 70, 160, 680, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(905, "Confusion Stone Strengthener", DamageType.Wizardry, 0, 6, 50, 80, 77, 850, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(906, "Confusion Stone Mastery", DamageType.Wizardry, 23, 6, 55, 100, 77, 850, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(907, "Wand Strengthen", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(909, "Elixir Strengthen", DamageType.Wizardry, 23, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(910, "Elixir Mastery", DamageType.Wizardry, 1, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(911, "Spirit Blast Strengthen", DamageType.Wizardry, 317, 6, 16, 35, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(912, "Crown Force Strengthen", DamageType.Wizardry, 318, 0, 20, 80, 10, 300, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(913, "Divine Force Strengthen", DamageType.Wizardry, 319, 0, 0, 100, 10, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(914, "Divine Aura Strengthen", DamageType.Wizardry, 320, 0, 0, 100, 10, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(915, "Battle Glory Strengthen", DamageType.Wizardry, 323, 0, 40, 50, 66, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(916, "Runic Spear Strengthen", DamageType.Wizardry, 22, 7, 0, 35, 150, 600, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(917, "Cloak of Desire Defense Enhancement", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(918, "Cloak of Desire Attack Enhancement", DamageType.Wizardry, 17, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(919, "Divine Fall Reinforcement", DamageType.Wizardry, 326, 6, 0, 10, 30, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(920, "Divine Fall Mastery", DamageType.Wizardry, 327, 6, 0, 13, 30, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(921, "Holy Sweep Reinforcement", DamageType.Wizardry, 324, 5, 2, 14, 50, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(923, "Sacred Impact Enhancement", DamageType.Wizardry, 328, 7, 16, 22, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(925, "Weapon Mastery", DamageType.Wizardry, 22, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(926, "Hammer reinforcement", DamageType.Wizardry, 330, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(927, "Hammer Mastery", DamageType.Wizardry, 331, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(928, "Lugard's Guardian: Strengthening Punishment", DamageType.Wizardry, 332, 8, 30, 210, 50, 200, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(1001, "Increase Skill DMG", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1002, "DMG Count +1", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1003, "DMG Count +2", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1004, "DMG Count +3", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1005, "DMG Count +4", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1006, "DMG Count +2", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1007, "Increase Additional DMG Rate", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1008, "Increase Attack Speed", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1009, "Increase Range", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1010, "Add Splash DMG", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1011, "Increase Skill Range", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1012, "Increase Target", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1013, "Buff Synergy Effect", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1014, "Buff Synergy Effect", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1015, "Increase Skill Duration", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1016, "Iron Defense (Learned)", DamageType.Wizardry, 3, 0, 31, 70, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1017, "Enhance Iron Defense", DamageType.Wizardry, 3, 0, 31, 70, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1018, "Cooldown Time Reduction", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1019, "Remove Cooldown Time", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1020, "Weapon DMG Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1021, "Weapon Magic DMG Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1022, "Add Penetration Effect", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1023, "Add Arrow Missile", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1024, "Increase chance to create Poison Magic Circles", DamageType.Wizardry, 151, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1025, "Increase chance to create Chilling Magic Circle", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1026, "Increase chance to create Bleeding Magic Circle", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1028, "Poison Damage Enhancement", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1029, "Chilling Effect Enhancement", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1030, "Bleeding Damage Enhancement", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1032, "Increase Poison Magic Circle Duration", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1033, "Increase Chilling Magic Circle Duration", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1034, "Increase Bleeding Magic Circle Duration", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1036, "Addiction Damage Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1037, "Freezing Damage Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1038, "Hemorrhage Damage Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1040, "Increase chance to enhance to Addiction Magic Circle", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1041, "Increase chance to enhance to Freezing Magic Circle", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1042, "Increase chance to enhance to Hemorrhage Magic Circle", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1046, "Additional chance to enhance to Poison Magic Circles", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1047, "Additional chance to enhance to Freezing Magic Circle", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1048, "Additional chance to enhance to Hemorrhage Magic Circle", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1050, "Additional Poison Damage Enhancement", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1051, "Additional Chilling Effect Enhancement", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1052, "Additional Bleeding Damage Enhancement", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1054, "Increase Addiction Magic Circle Duration", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1055, "Increase Freeze Magic Circle Duration", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1056, "Increase Hemorrhage Magic Circle Duration", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1058, "Additional Addiction Damage Enhancement", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1059, "Additional Freezing Effect Enhancement", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1060, "Additional Hemorrhage Effect Enhancement", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1062, "Upgrade Poisoning (Addiction)", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1063, "Upgrade Chilling (Freezing)", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1064, "Upgrade Bleeding (Hemorrhage)", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1069, "Anger Blow Enhancement Skill", DamageType.Wizardry, 60, 3, 22, 25, 170, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0);
        this.CreateSkill(1071, "Death Stab Enhancement Skill", DamageType.Wizardry, 22, 4, 18, 17, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0);
        this.CreateSkill(1072, "Fire Blow Enhancement Skill", DamageType.Wizardry, 120, 5, 17, 17, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0);
        this.CreateSkill(1075, "Meteor Strike Enhancement Skill", DamageType.Wizardry, 100, 7, 17, 92, 160, 1073, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1076, "Meteor Storm Enhancement Skill", DamageType.Wizardry, 105, 6, 20, 105, 160, 1160, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1078, "Evil Spirit Enhancement Skill", DamageType.Wizardry, 45, 6, 7, 108, 50, 220, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1081, "Triple Shot Enhancement Skill", DamageType.Wizardry, 0, 6, 4, 9, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1083, "Multi-Shot Enhancement Skill", DamageType.Wizardry, 22, 6, 7, 11, 100, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1085, "Focus Shot Enhancement Skill", DamageType.Wizardry, 110, 7, 9, 12, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1087, "Gigantic Storm Enhancement Skill", DamageType.Wizardry, 110, 6, 11, 110, 220, 1058, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1088, "Evil Spirit Enhancement Skill", DamageType.Wizardry, 45, 6, 7, 108, 50, 220, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1089, "Dark Blast Enhancement Skill", DamageType.Wizardry, 120, 7, 17, 92, 160, 1073, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1092, "Fire Slash Enhancement Skill", DamageType.Wizardry, 80, 3, 12, 15, 0, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1094, "Fire Blood Enhancement Skill", DamageType.Wizardry, 330, 3, 14, 19, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1095, "Ice Blood Enhancement Skill", DamageType.Wizardry, 330, 3, 14, 19, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1096, "Fire Burst Enhancement Skill", DamageType.Wizardry, 150, 6, 8, 20, 74, 79, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1098, "Chaotic Diseier Enhancement Skill", DamageType.Wizardry, 220, 6, 11, 22, 100, 84, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1099, "Wind Soul Enhancement Skill", DamageType.Wizardry, 130, 6, 19, 38, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1102, "Fire Beast Enhancement Skill", DamageType.Wizardry, 100, 7, 11, 95, 160, 1220, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1103, "Lightning Shock Enhancement Skill", DamageType.Wizardry, 95, 6, 10, 105, 93, 823, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1105, "Aqua Beast Enhancement Skill", DamageType.Wizardry, 100, 7, 11, 95, 160, 1220, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1111, "Dragon Roar Enhancement Skill", DamageType.Wizardry, 0, 3, 17, 42, 150, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1112, "Chain Drive Enhancement Skill", DamageType.Wizardry, 0, 4, 18, 22, 150, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1113, "Dark Side Enhancement Skill", DamageType.Wizardry, 0, 6, 7, 92, 180, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1117, "Magic Pin Enhancement Skill", DamageType.Wizardry, 80, 2, 15, 23, 30, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1118, "Breche Enhancement Skill", DamageType.Wizardry, 230, 6, 12, 20, 300, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1119, "Shining Peak Enhancement Skill", DamageType.Wizardry, 50, 4, 12, 15, 92, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1125, "Poison Storm", DamageType.Wizardry, 25000, 6, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1126, "Frozen Slayer", DamageType.Wizardry, 25000, 6, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1127, "Bloodying Hit", DamageType.Wizardry, 25000, 6, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1129, "Poison Storm Damage Enhancement", DamageType.Wizardry, 100, 6, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1130, "Frozen Slayer Damage Enhancement", DamageType.Wizardry, 100, 6, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1131, "Bloodying Hit Damage Enhancement", DamageType.Wizardry, 100, 6, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1133, "Poison Storm Blast Infection Effect", DamageType.Wizardry, 100, 6, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1134, "Frozen Slayer Explosion Infection Effect", DamageType.Wizardry, 100, 6, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1135, "Bloodying Hit Explosion Infection Effect", DamageType.Wizardry, 100, 6, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1137, "Maximum Life Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1138, "4th Stat Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1139, "Base Defense Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1140, "4th Wing / Cloak Defense Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1141, "Increase DMG", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1142, "Increase Magic", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1143, "Increase Fourth Stats", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1144, "Increase Skill DMG", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1145, "Increase Fourth Wings DMG", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1146, "Increase DMG / Magic", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1147, "Spirit Hook Enhancement Skill", DamageType.Wizardry, 255, 3, 21, 27, 180, 0, 1480, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1148, "Magic Arrow Enhancement Skill", DamageType.Wizardry, 10, 8, 0, 37, 100, 60, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1149, "Plasma Ball Enhancement Skill", DamageType.Wizardry, 40, 8, 35, 120, 160, 300, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1150, "Lightning Storm Enhancement Skill", DamageType.Wizardry, 300, 8, 7, 80, 160, 1080, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1151, "Decrease Move Speed/Move Distance", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1152, "Increase Plasma Attack Speed", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1153, "Increase Splash Damage Area", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1154, "Increase Splash Target", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1155, "Deathside Enhancement", DamageType.Wizardry, 95, 6, 20, 120, 93, 930, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1156, "Weapon Minimum DMG Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1157, "Wizardry / Curse DMG Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1158, "Sword Inertia Enhancement", DamageType.Wizardry, 10, 7, 5, 7, 170, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1159, "Bat Flock Enhancement", DamageType.Wizardry, 90, 6, 7, 15, 170, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1160, "Pierce Attack Enhancement", DamageType.Wizardry, 170, 6, 10, 20, 170, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1166, "Increase Poison debuff target count", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1167, "Increase Poison debuff target count", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1168, "Increase Hemorrhage debuff target count", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1169, "Increase Hemorrhage debuff target count", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1170, "Increase Freezing debuff target count", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1171, "Increase Freezing debuff target count", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1172, "Increase Poison debuff range", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1174, "Increase Hemorrhage debuff range", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1176, "Increase Freezing debuff range", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1178, "Increase Poison Debuff Success Rate", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1179, "Increase Poison Debuff Success Rate", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1180, "Increase Hemorrhage Debuff Success Rate", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1181, "Increase Hemorrhage Debuff Success Rate", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1182, "Increase Freezing Debuff Success Rate", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1183, "Increase Freezing Debuff Success Rate", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1184, "Increase Poison Debuff Success Rate", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1185, "Increase Poison Debuff Success Rate", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1186, "Increase Hemorrhage Debuff Success Rate", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1187, "Increase Hemorrhage Debuff Success Rate", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1188, "Increase Freezing Debuff Success Rate", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1189, "Increase Freezing Debuff Success Rate", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1190, "Increased Poison / Poison Debuff target count", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1191, "Increased Hemorrhage / Hemorrhage Debuff target count", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1192, "Increased Freezing / Freezing Debuff target count", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1193, "4th Wing / Cloak Enchantment", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1194, "4th Wing / Cloak Attack / Power Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1195, "4th Wing / Cloak Power / Curse Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1196, "Poison Storm Damage Enhancement", DamageType.Wizardry, 100, 6, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1197, "Frozen Slayer Damage Enhancement", DamageType.Wizardry, 100, 6, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1198, "Bloodying Hit Damage Enhancement", DamageType.Wizardry, 100, 6, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1199, "Poison Storm Blast Infection Effect", DamageType.Wizardry, 100, 6, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1200, "Frozen Slayer Explosion Infection Effect", DamageType.Wizardry, 100, 6, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1201, "Bloodying Hit Explosion Infection Effect", DamageType.Wizardry, 100, 6, 30, 40, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1202, "Strike of Destruction Enhancement Skill", DamageType.Wizardry, 120, 5, 24, 30, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1203, "Sword Blow Enhancement Skill", DamageType.Wizardry, 30, 5, 17, 19, 150, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1204, "Solid Protection Enhancement Skill", DamageType.Wizardry, 120, 0, 20, 65, 160, 1052, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1205, "Protection - Maximum HP Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 4, 4);
        this.CreateSkill(1206, "Protection - Party Member Attack Power Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 4, 4);
        this.CreateSkill(1207, "Protection - HP Conversion Increase (%)", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 4, 4);
        this.CreateSkill(1208, "Protection - Damage Conversion Increase (%)", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 4, 4);
        this.CreateSkill(1209, "Protection - Party Member Defense Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 4, 4);
        this.CreateSkill(1210, "Protection - Shield Defense Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 4, 4);
        this.CreateSkill(1211, "Dark Plasma Enhancement Skill", DamageType.Wizardry, 50, 6, 15, 19, 100, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0);
        this.CreateSkill(1212, "Ice Blast Enhancement Skill", DamageType.Wizardry, 130, 6, 10, 14, 150, 1000, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0);
        this.CreateSkill(1213, "Busting Flare Enhancement Skill", DamageType.Wizardry, 50, 6, 13, 14, 150, 800, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0);
        this.CreateSkill(1214, "Chaos Blade Enhancement Skill", DamageType.Wizardry, 350, 3, 14, 19, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1215, "Havok Spear Enhancement Skill", DamageType.Wizardry, 150, 6, 20, 92, 160, 1073, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1216, "Spear Storm Enhancement Skill", DamageType.Wizardry, 160, 6, 7, 108, 160, 1160, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateSkill(1217, "Marvel Burst Enhancement Skill", DamageType.Wizardry, 125, 6, 0, 32, 140, 104, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0);
        this.CreateSkill(1218, "Unleash Marvel Enhancement Skill", DamageType.Wizardry, 135, 7, 0, 113, 155, 700, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0);
        this.CreateSkill(1219, "Ultimate Force Enhancement Skill", DamageType.Wizardry, 10, 7, 6, 95, 160, 1073, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0);
        this.CreateSkill(1220, "Shining Bird Enhancement Skill", DamageType.Wizardry, 130, 7, 0, 32, 150, 70, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateSkill(1221, "Dragon Violent Enhancement Skill", DamageType.Wizardry, 140, 7, 0, 95, 160, 680, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateSkill(1222, "Raining Arrow Enhancement Skill", DamageType.Wizardry, 65, 7, 8, 19, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1223, "Holy Bolt Enhancement Skill", DamageType.Wizardry, 777, 6, 0, 30, 120, 1200, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1224, "Elemental Attack Power Enhancement Skill", DamageType.Wizardry, 0, 6, 0, 50, 120, 1500, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1225, "Elemental Defense Enhancement Skill", DamageType.Wizardry, 0, 6, 0, 50, 120, 1500, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1226, "Healing Enhancement Skill", DamageType.Wizardry, 0, 6, 0, 40, 100, 52, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1227, "Party Healing Enhancement Skill", DamageType.Wizardry, 0, 6, 13, 400, 0, 20, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1228, "Attack Power Enhancement Skill", DamageType.Wizardry, 120, 6, 0, 55, 100, 92, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1229, "Defense Enhancement Skill", DamageType.Wizardry, 120, 6, 0, 45, 100, 72, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1230, "Bless Enhancement Skill", DamageType.Wizardry, 0, 6, 22, 130, 0, 20, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1231, "Charge Slash Enhancement Skill", DamageType.Wizardry, 80, 7, 0, 3, 25, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0);
        this.CreateSkill(1232, "Wind Glaive Enhancement Skill", DamageType.Wizardry, 150, 6, 0, 6, 45, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0);
        this.CreateSkill(1233, "Blade Storm Enhancement Skill", DamageType.Wizardry, 300, 6, 10, 10, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0);
        this.CreateSkill(1234, "Illusion Avatar Enhancement Skill", DamageType.Wizardry, 200, 6, 0, 40, 100, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0);
        this.CreateSkill(1235, "Oversting Enhancement Skill", DamageType.Wizardry, 320, 6, 17, 27, 300, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1236, "Wild Breath Enhancement Skill", DamageType.Wizardry, 320, 6, 19, 35, 300, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1237, "Alchemy: Angel Homunculus Enhancement Skill", DamageType.Wizardry, 130, 6, 0, 35, 150, 70, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateSkill(1238, "Alchemy: Ignition Bomber Enhancement Skill", DamageType.Wizardry, 140, 7, 0, 95, 160, 680, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateSkill(1239, "Alchemy: Countless Weapon Enhancement Skill", DamageType.Wizardry, 160, 6, 11, 112, 160, 1025, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateSkill(1240, "Spirit Blast Enhancement Skill", DamageType.Wizardry, 350, 6, 16, 35, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1241, "4th Stat Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1242, "4th Stat Increase", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        this.CreateSkill(1500, "Sword's Fury", DamageType.Wizardry, 0, 0, 17, 21, 120, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1501, "Sword Blow", DamageType.Wizardry, 30, 5, 17, 19, 150, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1502, "Strong Belief", DamageType.Wizardry, 0, 0, 25, 80, 120, 1040, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1503, "Solid Protection", DamageType.Wizardry, 0, 0, 20, 65, 120, 1052, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1504, "Runic Spear Enhancement Skill", DamageType.Wizardry, 20, 7, 0, 42, 150, 600, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1505, "Rune Phrase Enhancement Skill", DamageType.Wizardry, 160, 7, 6, 80, 160, 1180, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(1506, "Divine Fall Enhancement Skill", DamageType.Wizardry, 150, 6, 0, 16, 30, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4);
        this.CreateSkill(1507, "Holy Sweep Enhancement Skill", DamageType.Wizardry, 300, 5, 4, 20, 50, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4);
        this.CreateSkill(1508, "Sacred Impact Enhancement Skill", DamageType.Wizardry, 350, 7, 18, 26, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4);
        this.CreateSkill(2001, "Dark Plasma", DamageType.Wizardry, 30, 6, 10, 22, 130, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0);
        this.CreateSkill(2002, "Ice Break", DamageType.Wizardry, 180, 6, 1, 7, 50, 295, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
        this.CreateSkill(2003, "Ice Blast", DamageType.Wizardry, 140, 6, 10, 13, 160, 1000, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(2004, "Death Fire", DamageType.Wizardry, 160, 6, 1, 5, 50, 100, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
        this.CreateSkill(2005, "Bursting Flare", DamageType.Wizardry, 70, 6, 13, 10, 160, 800, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateSkill(2006, "Death Ice", DamageType.Wizardry, 160, 6, 1, 5, 50, 100, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
        this.CreateSkill(2007, "Beginner Bless", DamageType.Wizardry, 0, 6, 0, 20, 8, 52, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateSkill(2008, "Beginner Recovery", DamageType.Wizardry, 0, 6, 10, 40, 100, 168, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0);
        this.CreateSkill(2009, "Beginner Basic Defense Improvement", DamageType.Wizardry, 0, 6, 0, 30, 13, 72, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateSkill(2010, "Beginner Attack Power Improvement", DamageType.Wizardry, 0, 6, 0, 40, 18, 92, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateSkill(2011, "Beginner Bless", DamageType.Wizardry, 0, 6, 10, 40, 10, 150, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateSkill(2012, "Chaos Blade", DamageType.Wizardry, 350, 3, 14, 19, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2013, "Havok Spear", DamageType.Wizardry, 150, 6, 20, 92, 160, 1073, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2014, "Spiral Charge", DamageType.Wizardry, 0, 0, 17, 21, 120, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2015, "Crusher Charge", DamageType.Wizardry, 0, 0, 17, 21, 120, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2016, "Elemental Charge", DamageType.Wizardry, 0, 0, 20, 65, 120, 1073, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2017, "Chaos Blade Magic Explosion", DamageType.Wizardry, 400, 6, 0, 0, 120, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2018, "Fire Blood Magic Explosion", DamageType.Wizardry, 200, 6, 0, 0, 120, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2019, "Ice Blood Magic Explosion", DamageType.Wizardry, 200, 6, 0, 0, 120, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2020, "Havok Spear Nova", DamageType.Wizardry, 200, 6, 0, 0, 120, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2021, "Fixed Fire", DamageType.Wizardry, 0, 0, 50, 100, 100, 300, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
        this.CreateSkill(2022, "Bond", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2023, "Raining Arrow", DamageType.Wizardry, 65, 7, 7, 15, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2024, "Dex Booster", DamageType.Wizardry, 0, 0, 12, 10, 100, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2025, "Holy Bolt", DamageType.Wizardry, 777, 6, 0, 20, 110, 1200, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2026, "Improve Elemental Attack Power", DamageType.Wizardry, 0, 6, 0, 40, 100, 1500, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2027, "Improve Elemental Defense", DamageType.Wizardry, 0, 6, 0, 40, 100, 1500, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2028, "Charge Slash", DamageType.Wizardry, 80, 6, 0, 3, 25, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);
        this.CreateSkill(2029, "Wind Glaive", DamageType.Wizardry, 150, 5, 0, 6, 45, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);
        this.CreateSkill(2030, "Blade Storm", DamageType.Wizardry, 220, 6, 10, 10, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateSkill(2031, "Illusion Avatar", DamageType.Wizardry, 200, 6, 0, 40, 100, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0);
        this.CreateSkill(2032, "Illusion Blade", DamageType.Wizardry, 0, 0, 0, 40, 25, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);
        this.CreateSkill(2036, "Oversting", DamageType.Wizardry, 320, 6, 12, 25, 300, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2037, "Meteor Storm of Gale", DamageType.Wizardry, 105, 6, 20, 105, 160, 1160, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2040, "Sword Blow of Saturation", DamageType.Wizardry, 30, 5, 17, 19, 150, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2043, "Destruction of Gale", DamageType.Wizardry, 120, 5, 24, 30, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2044, "Raining Arrow of Saturation", DamageType.Wizardry, 65, 7, 8, 19, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2047, "Holy Bolt of Gale", DamageType.Wizardry, 777, 6, 0, 30, 120, 1200, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2049, "Chaos Blade of Saturation", DamageType.Wizardry, 350, 3, 14, 19, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2051, "Havok Spear of Wrath", DamageType.Wizardry, 150, 6, 20, 92, 160, 1073, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2054, "Wind Soul of Saturation", DamageType.Wizardry, 130, 6, 19, 38, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2058, "Fire Beast of Saturation", DamageType.Wizardry, 100, 7, 11, 95, 160, 1220, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2060, "Death Scythe of Fury", DamageType.Wizardry, 95, 6, 20, 120, 93, 930, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2061, "Dark Side of Saturation", DamageType.Wizardry, 0, 6, 7, 92, 180, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2063, "Spirit Hook of Saturation", DamageType.Wizardry, 255, 3, 21, 27, 180, 0, 1480, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2065, "Oversting of Saturation", DamageType.Wizardry, 320, 6, 17, 27, 300, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2068, "Lightning Storm of Gale", DamageType.Wizardry, 300, 8, 7, 80, 160, 1080, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2071, "Pierce Attack of Saturation", DamageType.Wizardry, 170, 6, 10, 20, 170, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2073, "Bursting Flare of Gale", DamageType.Wizardry, 50, 6, 13, 14, 150, 800, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0);
        this.CreateSkill(2076, "Ultimate Storm of Saturation", DamageType.Wizardry, 10, 7, 6, 95, 160, 1073, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0);
        this.CreateSkill(2080, "Spear Storm of Saturation", DamageType.Wizardry, 160, 6, 7, 108, 160, 1160, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0);
        this.CreateSkill(2082, "Blade Storm of Saturation", DamageType.Wizardry, 300, 6, 10, 10, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0);
        this.CreateSkill(2085, "Wild Breath", DamageType.Wizardry, 320, 6, 18, 35, 300, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2086, "Nuke (activation)", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2087, "Bolt (activation)", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2088, "Wide Area (activation)", DamageType.Wizardry, 0, 0, 0, 0, 0, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2089, "Wild Breath of Gale", DamageType.Wizardry, 320, 6, 19, 35, 300, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2090, "Alchemy: Confusion Stone", DamageType.Wizardry, 0, 6, 25, 70, 77, 850, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateSkill(2091, "Alchemy: Angel Homunculus", DamageType.Wizardry, 130, 6, 0, 5, 30, 70, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateSkill(2092, "Alchemy: Ignition Bomber", DamageType.Wizardry, 140, 6, 0, 30, 50, 680, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateSkill(2093, "Alchemy: Countless Weapon", DamageType.Wizardry, 160, 6, 10, 108, 160, 1025, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateSkill(2094, "Fiery Countless Weapon", DamageType.Wizardry, 160, 6, 13, 120, 160, 1025, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0);
        this.CreateSkill(2095, "Spirit Blast", DamageType.Wizardry, 350, 6, 16, 35, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2096, "Spirit Blast of Anger", DamageType.Wizardry, 350, 6, 16, 35, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2097, "Crown Force", DamageType.Wizardry, 0, 0, 20, 80, 10, 300, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2098, "Divine Force", DamageType.Wizardry, 0, 0, 0, 100, 10, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2099, "Divine Aura", DamageType.Wizardry, 0, 0, 0, 100, 10, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2100, "Battle Glory", DamageType.Wizardry, 0, 0, 30, 40, 66, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2101, "Runic Spear", DamageType.Wizardry, 20, 7, 0, 15, 140, 600, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2102, "Rune Phrase", DamageType.Wizardry, 160, 7, 5, 70, 160, 1180, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2103, "Rune Phrase of Saturation", DamageType.Wizardry, 160, 7, 6, 90, 160, 1180, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2104, "Special Praise", DamageType.Wizardry, 160, 7, 0, 0, 160, 0, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateSkill(2105, "Divine Fall", DamageType.Wizardry, 150, 6, 0, 5, 30, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
        this.CreateSkill(2106, "Holy Sweep", DamageType.Wizardry, 300, 5, 1, 13, 50, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);
        this.CreateSkill(2107, "Sacred Impact", DamageType.Wizardry, 350, 7, 15, 20, 160, 0, 0, (ElementalType)6, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateSkill(2108, "Lugard's Guardian: Punishment", DamageType.Wizardry, 0, 8, 30, 200, 50, 200, 0, (ElementalType)0, SkillType.DirectHit, SkillTarget.Explicit, 0, SkillTargetRestriction.Undefined, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);


        // Generic monster skills:
        this.CreateSkill((short)SkillNumber.MonsterSkill, "Generic Monster Skill", distance: 5, skillType: SkillType.Other);

        // Master skills:
        // Common:
        /*
        this.CreateSkill(SkillNumber.DurabilityReduction1, "Durability Reduction (1)", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.PvPDefenceRateInc, "PvP Defence Rate Inc", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 12, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.MaximumSDincrease, "Maximum SD increase", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 13, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.AutomaticManaRecInc, "Automatic Mana Rec Inc", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 7, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.PoisonResistanceInc, "Poison Resistance Inc", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, elementalModifier: ElementalType.Poison, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DurabilityReduction2, "Durability Reduction (2)", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.SdRecoverySpeedInc, "SD Recovery Speed Inc", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.AutomaticHpRecInc, "Automatic HP Rec Inc", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.LightningResistanceInc, "Lightning Resistance Inc", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, elementalModifier: ElementalType.Lightning, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DefenseIncrease, "Defense Increase", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 16, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.AutomaticAgRecInc, "Automatic AG Rec Inc", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IceResistanceIncrease, "Ice Resistance Increase", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, elementalModifier: ElementalType.Ice, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DurabilityReduction3, "Durability Reduction (3)", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DefenseSuccessRateInc, "Defense Success Rate Inc", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.MaximumLifeIncrease, "Maximum Life Increase", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 9, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.ManaReduction, "Mana Reduction", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 18, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.MonsterAttackSdInc, "Monster Attack SD Inc", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 11, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.MonsterAttackLifeInc, "Monster Attack Life Inc", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 6, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.SwellLifeProficiency, "Swell Life Proficiency", [CharacterClassNumber.BladeMaster], damage: 7, abilityConsumption: 28, manaConsumption: 26, levelRequirement: 120);
        this.CreateSkill(SkillNumber.MinimumAttackPowerInc, "Minimum Attack Power Inc", [CharacterClassNumber.BladeMaster, CharacterClassNumber.DuelMaster, CharacterClassNumber.LordEmperor], DamageType.Physical, 22, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.MonsterAttackManaInc, "Monster Attack Mana Inc", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 6, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.PvPAttackRate, "PvP Attack Rate", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 14, skillType: SkillType.PassiveBoost);

        // Blade Master:
        this.CreateSkill(SkillNumber.AttackSuccRateInc, "Attack Succ Rate Inc", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 13, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.CycloneStrengthener, "Cyclone Strengthener", [CharacterClassNumber.BladeMaster], DamageType.Physical, 22, 2, manaConsumption: 9);
        this.CreateSkill(SkillNumber.SlashStrengthener, "Slash Strengthener", [CharacterClassNumber.BladeMaster], DamageType.Physical, 3, 2, manaConsumption: 10);
        this.CreateSkill(SkillNumber.FallingSlashStreng, "Falling Slash Streng", [CharacterClassNumber.BladeMaster], DamageType.Physical, 3, 3, manaConsumption: 9);
        this.CreateSkill(SkillNumber.LungeStrengthener, "Lunge Strengthener", [CharacterClassNumber.BladeMaster], DamageType.Physical, 3, 2, manaConsumption: 9);
        this.CreateSkill(SkillNumber.TwistingSlashStreng, "Twisting Slash Streng", [CharacterClassNumber.BladeMaster], DamageType.Physical, 3, 2, 10, 10);
        this.CreateSkill(SkillNumber.RagefulBlowStreng, "Rageful Blow Streng", [CharacterClassNumber.BladeMaster], DamageType.Physical, 22, 3, 22, 25, 170);
        this.CreateSkill(SkillNumber.TwistingSlashMastery, "Twisting Slash Mastery", [CharacterClassNumber.BladeMaster], DamageType.Physical, 1, 2, 20, 22);
        this.CreateSkill(SkillNumber.RagefulBlowMastery, "Rageful Blow Mastery", [CharacterClassNumber.BladeMaster], DamageType.Physical, 1, 3, 30, 50, 170, elementalModifier: ElementalType.Earth);
        this.CreateSkill(SkillNumber.WeaponMasteryBladeMaster, "Weapon Mastery", [CharacterClassNumber.BladeMaster], damage: 22, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DeathStabStrengthener, "Death Stab Strengthener", [CharacterClassNumber.BladeMaster], DamageType.Physical, 22, 2, 13, 15, 160, elementalModifier: ElementalType.Wind);
        this.CreateSkill(SkillNumber.StrikeofDestrStr, "Strike of Destr Str", [CharacterClassNumber.BladeMaster], DamageType.Physical, 22, 5, 24, 30, 100, elementalModifier: ElementalType.Ice);
        this.CreateSkill(SkillNumber.MaximumManaIncrease, "Maximum Mana Increase", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 9, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.TwoHandedSwordStrengthener, "Two-handed Sword Stren", [CharacterClassNumber.BladeMaster, CharacterClassNumber.DuelMaster], DamageType.Physical, 4, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.OneHandedSwordStrengthener, "One-handed Sword Stren", [CharacterClassNumber.BladeMaster, CharacterClassNumber.DuelMaster], DamageType.Physical, 22, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.MaceStrengthener, "Mace Strengthener", [CharacterClassNumber.BladeMaster], DamageType.Physical, 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.SpearStrengthener, "Spear Strengthener", [CharacterClassNumber.BladeMaster], DamageType.Physical, 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.TwoHandedSwordMaster, "Two-handed Sword Mast", [CharacterClassNumber.BladeMaster, CharacterClassNumber.DuelMaster], DamageType.Physical, 5, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.OneHandedSwordMaster, "One-handed Sword Mast", [CharacterClassNumber.BladeMaster, CharacterClassNumber.DuelMaster], DamageType.Physical, 23, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.MaceMastery, "Mace Mastery", [CharacterClassNumber.BladeMaster], DamageType.Physical, 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.SpearMastery, "Spear Mastery", [CharacterClassNumber.BladeMaster], DamageType.Physical, 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.SwellLifeStrengt, "Swell Life Strengt", [CharacterClassNumber.BladeMaster], damage: 7, abilityConsumption: 26, manaConsumption: 24, levelRequirement: 120);

        // Grand Master:
        this.CreateSkill(SkillNumber.FlameStrengthener, "Flame Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 3, 6, manaConsumption: 55, levelRequirement: 35, energyRequirement: 100, elementalModifier: ElementalType.Fire);
        this.CreateSkill(SkillNumber.LightningStrengthener, "Lightning Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 3, 6, manaConsumption: 20, levelRequirement: 13, energyRequirement: 100, elementalModifier: ElementalType.Lightning);
        this.CreateSkill(SkillNumber.ExpansionofWizStreng, "Expansion of Wiz Streng", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 1, 6, 55, 220, 220, 118);
        this.CreateSkill(SkillNumber.InfernoStrengthener, "Inferno Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 22, manaConsumption: 220, levelRequirement: 88, energyRequirement: 200, elementalModifier: ElementalType.Fire);
        this.CreateSkill(SkillNumber.BlastStrengthener, "Blast Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 22, 3, manaConsumption: 165, levelRequirement: 80, energyRequirement: 150, elementalModifier: ElementalType.Lightning);
        this.CreateSkill(SkillNumber.ExpansionofWizMas, "Expansion of Wiz Mas", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 1, 6, 55, 220, 220, 118);
        this.CreateSkill(SkillNumber.PoisonStrengthener, "Poison Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 3, 6, manaConsumption: 46, levelRequirement: 30, energyRequirement: 100, elementalModifier: ElementalType.Poison);
        this.CreateSkill(SkillNumber.EvilSpiritStreng, "Evil Spirit Streng", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 22, 6, manaConsumption: 108, levelRequirement: 50, energyRequirement: 100);
        this.CreateSkill(SkillNumber.MagicMasteryGrandMaster, "Magic Mastery", [CharacterClassNumber.GrandMaster], damage: 22, levelRequirement: 50, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DecayStrengthener, "Decay Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 22, 6, 10, 120, 96, 243, elementalModifier: ElementalType.Poison);
        this.CreateSkill(SkillNumber.HellfireStrengthener, "Hellfire Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 3, manaConsumption: 176, levelRequirement: 60, energyRequirement: 100, elementalModifier: ElementalType.Fire);
        this.CreateSkill(SkillNumber.IceStrengthener, "Ice Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 3, 6, manaConsumption: 42, levelRequirement: 25, energyRequirement: 100, elementalModifier: ElementalType.Ice);
        this.CreateSkill(SkillNumber.OneHandedStaffStrengthener, "One-handed Staff Stren", [CharacterClassNumber.GrandMaster, CharacterClassNumber.DuelMaster], DamageType.Wizardry, 22, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.TwoHandedStaffStrengthener, "Two-handed Staff Stren", [CharacterClassNumber.GrandMaster, CharacterClassNumber.DuelMaster], DamageType.Wizardry, 4, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.ShieldStrengthenerGrandMaster, "Shield Strengthener", [CharacterClassNumber.GrandMaster], damage: 10, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.OneHandedStaffMaster, "One-handed Staff Mast", [CharacterClassNumber.GrandMaster, CharacterClassNumber.DuelMaster], damage: 23, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.TwoHandedStaffMaster, "Two-handed Staff Mast", [CharacterClassNumber.GrandMaster, CharacterClassNumber.DuelMaster], DamageType.Wizardry, 5, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.ShieldMasteryGrandMaster, "Shield Mastery", [CharacterClassNumber.GrandMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.SoulBarrierStrength, "Soul Barrier Strength", [CharacterClassNumber.GrandMaster], damage: 7, distance: 6, abilityConsumption: 24, manaConsumption: 77, levelRequirement: 77, energyRequirement: 126);
        this.CreateSkill(SkillNumber.SoulBarrierProficie, "Soul Barrier Proficie", [CharacterClassNumber.GrandMaster], damage: 10, distance: 6, abilityConsumption: 26, manaConsumption: 84, levelRequirement: 77, energyRequirement: 126);
        this.CreateSkill(SkillNumber.MinimumWizardryInc, "Minimum Wizardry Inc", [CharacterClassNumber.GrandMaster, CharacterClassNumber.DuelMaster], damage: 22, skillType: SkillType.PassiveBoost);

        // High Elf:
        this.CreateSkill(SkillNumber.HealStrengthener, "Heal Strengthener", [CharacterClassNumber.HighElf], DamageType.Physical, 22, 6, manaConsumption: 22, levelRequirement: 8, energyRequirement: 100);
        this.CreateSkill(SkillNumber.TripleShotStrengthener, "Triple Shot Strengthener", [CharacterClassNumber.HighElf], DamageType.Physical, 22, 6, manaConsumption: 5);
        this.CreateSkill(SkillNumber.SummonedMonsterStr1, "Summoned Monster Str (1)", [CharacterClassNumber.HighElf], damage: 16, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.PenetrationStrengthener, "Penetration Strengthener", [CharacterClassNumber.HighElf], DamageType.Physical, 22, 6, 11, 10, 130, elementalModifier: ElementalType.Wind);
        this.CreateSkill(SkillNumber.DefenseIncreaseStr, "Defense Increase Str", [CharacterClassNumber.HighElf], damage: 22, distance: 6, manaConsumption: 33, levelRequirement: 13, energyRequirement: 100);
        this.CreateSkill(SkillNumber.TripleShotMastery, "Triple Shot Mastery", [CharacterClassNumber.HighElf], DamageType.Physical, distance: 6, manaConsumption: 9);
        this.CreateSkill(SkillNumber.SummonedMonsterStr2, "Summoned Monster Str (2)", [CharacterClassNumber.HighElf], damage: 16, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.AttackIncreaseStr, "Attack Increase Str", [CharacterClassNumber.HighElf], damage: 22, distance: 6, manaConsumption: 44, levelRequirement: 18, energyRequirement: 100);
        this.CreateSkill(SkillNumber.WeaponMasteryHighElf, "Weapon Mastery", [CharacterClassNumber.HighElf], damage: 22, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.AttackIncreaseMastery, "Attack Increase Mastery", [CharacterClassNumber.HighElf], damage: 22, distance: 6, manaConsumption: 48, levelRequirement: 18, energyRequirement: 100);
        this.CreateSkill(SkillNumber.DefenseIncreaseMastery, "Defense Increase Mastery", [CharacterClassNumber.HighElf], damage: 22, distance: 6, manaConsumption: 36, levelRequirement: 13, energyRequirement: 100);
        this.CreateSkill(SkillNumber.IceArrowStrengthener, "Ice Arrow Strengthener", [CharacterClassNumber.HighElf], DamageType.Physical, 22, 8, 18, 15, elementalModifier: ElementalType.Ice);
        this.CreateSkill(SkillNumber.BowStrengthener, "Bow Strengthener", [CharacterClassNumber.HighElf], damage: 22, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.CrossbowStrengthener, "Crossbow Strengthener", [CharacterClassNumber.HighElf], damage: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.ShieldStrengthenerHighElf, "Shield Strengthener", [CharacterClassNumber.HighElf], damage: 10, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.BowMastery, "Bow Mastery", [CharacterClassNumber.HighElf], damage: 23, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.CrossbowMastery, "Crossbow Mastery", [CharacterClassNumber.HighElf], damage: 5, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.ShieldMasteryHighElf, "Shield Mastery", [CharacterClassNumber.HighElf], damage: 15, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.InfinityArrowStr, "Infinity Arrow Str", [CharacterClassNumber.HighElf], damage: 1, distance: 6, abilityConsumption: 11, manaConsumption: 55, levelRequirement: 220, skillType: SkillType.Buff, targetRestriction: SkillTargetRestriction.Self);
        this.CreateSkill(SkillNumber.MinimumAttPowerInc, "Minimum Att Power Inc", [CharacterClassNumber.HighElf], DamageType.Physical, 22, skillType: SkillType.PassiveBoost);

        // Dimension Master (Summoner):
        this.CreateSkill(SkillNumber.FireTomeStrengthener, "Fire Tome Strengthener", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 3, elementalModifier: ElementalType.Fire, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.WindTomeStrengthener, "Wind Tome Strengthener", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 3, elementalModifier: ElementalType.Wind, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.LightningTomeStren, "Lightning Tome Stren", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 3, elementalModifier: ElementalType.Lightning, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.FireTomeMastery, "Fire Tome Mastery", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 7, elementalModifier: ElementalType.Fire, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.WindTomeMastery, "Wind Tome Mastery", [CharacterClassNumber.DimensionMaster], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.LightningTomeMastery, "Lightning Tome Mastery", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 7, elementalModifier: ElementalType.Lightning, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.SleepStrengthener, "Sleep Strengthener", [CharacterClassNumber.DimensionMaster], damage: 1, distance: 6, abilityConsumption: 7, manaConsumption: 30, levelRequirement: 40, energyRequirement: 100);
        this.CreateSkill(SkillNumber.ChainLightningStr, "Chain Lightning Str", [CharacterClassNumber.DimensionMaster], DamageType.Wizardry, 22, 6, manaConsumption: 103, levelRequirement: 75, energyRequirement: 75, skillTarget: SkillTarget.Explicit, skillType: SkillType.AreaSkillExplicitTarget);
        this.CreateSkill(SkillNumber.LightningShockStr, "Lightning Shock Str", [CharacterClassNumber.DimensionMaster], DamageType.Wizardry, 22, 6, 10, 125, 93, 216, elementalModifier: ElementalType.Lightning);
        this.CreateSkill(SkillNumber.MagicMasterySummoner, "Magic Mastery", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 22, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DrainLifeStrengthener, "Drain Life Strengthener", [CharacterClassNumber.DimensionMaster], DamageType.Wizardry, 22, 6, manaConsumption: 57, levelRequirement: 35, energyRequirement: 93);
        this.CreateSkill(SkillNumber.StickStrengthener, "Stick Strengthener", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 22, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.OtherWorldTomeStreng, "Other World Tome Streng", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.StickMastery, "Stick Mastery", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 5, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.OtherWorldTomeMastery, "Other World Tome Mastery", [CharacterClassNumber.DimensionMaster], damage: 23, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.BerserkerStrengthener, "Berserker Strengthener", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 7, 5, 75, 150, 83, 181);
        this.CreateSkill(SkillNumber.BerserkerProficiency, "Berserker Proficiency", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 7, 5, 82, 165, 83, 181);
        this.CreateSkill(SkillNumber.MinimumWizCurseInc, "Minimum Wiz/Curse Inc", [CharacterClassNumber.DimensionMaster], damage: 22, skillType: SkillType.PassiveBoost);

        // Duel Master (MG):
        this.CreateSkill(SkillNumber.CycloneStrengthenerDuelMaster, "Cyclone Strengthener", [CharacterClassNumber.DuelMaster], DamageType.Physical, 22, 2, manaConsumption: 9);
        this.CreateSkill(SkillNumber.LightningStrengthenerDuelMaster, "Lightning Strengthener", [CharacterClassNumber.DuelMaster], DamageType.Physical, 3, 6, manaConsumption: 20, levelRequirement: 13, energyRequirement: 100, elementalModifier: ElementalType.Lightning);
        this.CreateSkill(SkillNumber.TwistingSlashStrengthenerDuelMaster, "Twisting Slash Stren", [CharacterClassNumber.DuelMaster], DamageType.Physical, 3, 2, 10, 10);
        this.CreateSkill(SkillNumber.PowerSlashStreng, "Power Slash Streng", [CharacterClassNumber.DuelMaster], damage: 3, distance: 5, manaConsumption: 15, energyRequirement: 100);
        this.CreateSkill(SkillNumber.FlameStrengthenerDuelMaster, "Flame Strengthener", [CharacterClassNumber.DuelMaster], DamageType.Physical, 3, 6, manaConsumption: 55, levelRequirement: 35, energyRequirement: 100, elementalModifier: ElementalType.Fire);
        this.CreateSkill(SkillNumber.BlastStrengthenerDuelMaster, "Blast Strengthener", [CharacterClassNumber.DuelMaster], DamageType.Physical, 22, 3, manaConsumption: 165, levelRequirement: 80, energyRequirement: 150, elementalModifier: ElementalType.Lightning);
        this.CreateSkill(SkillNumber.WeaponMasteryDuelMaster, "Weapon Mastery", [CharacterClassNumber.DuelMaster], damage: 22, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.InfernoStrengthenerDuelMaster, "Inferno Strengthener", [CharacterClassNumber.DuelMaster], DamageType.Physical, 22, manaConsumption: 220, levelRequirement: 88, energyRequirement: 200, elementalModifier: ElementalType.Fire);
        this.CreateSkill(SkillNumber.EvilSpiritStrengthenerDuelMaster, "Evil Spirit Strengthen", [CharacterClassNumber.DuelMaster], DamageType.Physical, 22, 6, manaConsumption: 108, levelRequirement: 50, energyRequirement: 100);
        this.CreateSkill(SkillNumber.MagicMasteryDuelMaster, "Magic Mastery", [CharacterClassNumber.DuelMaster], damage: 22, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IceStrengthenerDuelMaster, "Ice Strengthener", [CharacterClassNumber.DuelMaster], DamageType.Physical, 3, 6, manaConsumption: 42, levelRequirement: 25, energyRequirement: 100, elementalModifier: ElementalType.Ice);
        this.CreateSkill(SkillNumber.BloodAttackStrengthen, "Blood Attack Strengthen", [CharacterClassNumber.DuelMaster], damage: 22, distance: 3, abilityConsumption: 22, manaConsumption: 15, elementalModifier: ElementalType.Poison);

        // Lord Emperor (DL):
        this.CreateSkill(SkillNumber.FireBurstStreng, "Fire Burst Streng", [CharacterClassNumber.LordEmperor], DamageType.Physical, 22, 6, manaConsumption: 25, levelRequirement: 74, energyRequirement: 20);
        this.CreateSkill(SkillNumber.ForceWaveStreng, "Force Wave Streng", [CharacterClassNumber.LordEmperor], DamageType.Physical, 3, 4, manaConsumption: 15);
        this.CreateSkill(SkillNumber.DarkHorseStreng1, "Dark Horse Streng (1)", [CharacterClassNumber.LordEmperor], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.CriticalDmgIncPowUp, "Critical DMG Inc PowUp", [CharacterClassNumber.LordEmperor], damage: 3, abilityConsumption: 75, manaConsumption: 75, levelRequirement: 82, energyRequirement: 25, leadershipRequirement: 300);
        this.CreateSkill(SkillNumber.EarthshakeStreng, "Earthshake Streng", [CharacterClassNumber.LordEmperor], DamageType.Physical, 22, 10, 75, elementalModifier: ElementalType.Lightning, skillType: SkillType.AreaSkillAutomaticHits);
        this.CreateSkill(SkillNumber.WeaponMasteryLordEmperor, "Weapon Mastery", [CharacterClassNumber.LordEmperor], damage: 22, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.FireBurstMastery, "Fire Burst Mastery", [CharacterClassNumber.LordEmperor], DamageType.Physical, 1, 6, manaConsumption: 27, levelRequirement: 74, energyRequirement: 20);
        this.CreateSkill(SkillNumber.CritDmgIncPowUp2, "Crit DMG Inc PowUp (2)", [CharacterClassNumber.LordEmperor], damage: 10, abilityConsumption: 82, manaConsumption: 82, levelRequirement: 82, energyRequirement: 25, leadershipRequirement: 300);
        this.CreateSkill(SkillNumber.EarthshakeMastery, "Earthshake Mastery", [CharacterClassNumber.LordEmperor], DamageType.Physical, 1, 10, 75, elementalModifier: ElementalType.Lightning, skillType: SkillType.AreaSkillAutomaticHits);
        this.CreateSkill(SkillNumber.CritDmgIncPowUp3, "Crit DMG Inc PowUp (3)", [CharacterClassNumber.LordEmperor], damage: 7, abilityConsumption: 100, manaConsumption: 100, levelRequirement: 82, energyRequirement: 25, leadershipRequirement: 300);
        this.CreateSkill(SkillNumber.FireScreamStren, "Fire Scream Stren", [CharacterClassNumber.LordEmperor], DamageType.Physical, 22, 6, 11, 45, 102, 32, 70);
        this.CreateSkill(SkillNumber.DarkSpiritStr, "Dark Spirit Str", [CharacterClassNumber.LordEmperor], damage: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.ScepterStrengthener, "Scepter Strengthener", [CharacterClassNumber.LordEmperor], DamageType.Physical, 22, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.ShieldStrengthenerLordEmperor, "Shield Strengthener", [CharacterClassNumber.LordEmperor], damage: 10, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.UseScepterPetStr, "Use Scepter : Pet Str", [CharacterClassNumber.LordEmperor], damage: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DarkSpiritStr2, "Dark Spirit Str (2)", [CharacterClassNumber.LordEmperor], damage: 7, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.ScepterMastery, "Scepter Mastery", [CharacterClassNumber.LordEmperor], damage: 5, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.ShieldMastery, "Shield Mastery", [CharacterClassNumber.LordEmperor], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.CommandAttackInc, "Command Attack Inc", [CharacterClassNumber.LordEmperor], damage: 20, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DarkSpiritStr3, "Dark Spirit Str (3)", [CharacterClassNumber.LordEmperor], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.PetDurabilityStr, "Pet Durability Str", [CharacterClassNumber.LordEmperor], damage: 17, skillType: SkillType.PassiveBoost);

        // Fist Master (Rage Fighter):
        this.CreateSkill(SkillNumber.KillingBlowStrengthener, "Killing Blow Strengthener", [CharacterClassNumber.FistMaster], DamageType.Physical, 22, 2, manaConsumption: 10, elementalModifier: ElementalType.Earth);
        this.CreateSkill(SkillNumber.BeastUppercutStrengthener, "Beast Uppercut Strengthener", [CharacterClassNumber.FistMaster], DamageType.Physical, 22, 2, manaConsumption: 10, elementalModifier: ElementalType.Fire);
        this.CreateSkill(SkillNumber.KillingBlowMastery, "Killing Blow Mastery", [CharacterClassNumber.FistMaster], DamageType.Physical, 1, 2, manaConsumption: 10, elementalModifier: ElementalType.Earth);
        this.CreateSkill(SkillNumber.BeastUppercutMastery, "Beast Uppercut Mastery", [CharacterClassNumber.FistMaster], DamageType.Physical, 1, 2, manaConsumption: 10, elementalModifier: ElementalType.Fire);
        this.CreateSkill(SkillNumber.WeaponMasteryFistMaster, "Weapon Mastery", [CharacterClassNumber.FistMaster], damage: 22, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.ChainDriveStrengthener, "Chain Drive Strengthener", [CharacterClassNumber.FistMaster], DamageType.Physical, 22, 4, 22, 22, 150, elementalModifier: ElementalType.Ice);
        this.CreateSkill(SkillNumber.DarkSideStrengthener, "Dark Side Strengthener", [CharacterClassNumber.FistMaster], DamageType.Physical, 22, 4, manaConsumption: 84, levelRequirement: 180, elementalModifier: ElementalType.Wind);
        this.CreateSkill(SkillNumber.DragonRoarStrengthener, "Dragon Roar Strengthener", [CharacterClassNumber.FistMaster], DamageType.Physical, 22, 3, 33, 60, 150, elementalModifier: ElementalType.Earth);
        this.CreateSkill(SkillNumber.EquippedWeaponStrengthener, "Equipped Weapon Strengthener", [CharacterClassNumber.FistMaster], damage: 22, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DefSuccessRateIncPowUp, "Def SuccessRate IncPowUp", [CharacterClassNumber.FistMaster], DamageType.Physical, 22, 7, 11, 55, 50, 30);
        this.CreateSkill(SkillNumber.EquippedWeaponMastery, "Equipped Weapon Mastery", [CharacterClassNumber.FistMaster], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DefSuccessRateIncMastery, "DefSuccessRate IncMastery", [CharacterClassNumber.FistMaster], DamageType.Physical, 22, 7, 12, 60, 50, 30);
        this.CreateSkill(SkillNumber.StaminaIncreaseStrengthener, "Stamina Increase Strengthener", [CharacterClassNumber.FistMaster], DamageType.Physical, 5, 7, 11, 55, 80, 35);
        this.CreateSkill(SkillNumber.DurabilityReduction1FistMaster, "Durability Reduction (1)", [CharacterClassNumber.FistMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreasePvPDefenseRate, "Increase PvP Defense Rate", [CharacterClassNumber.FistMaster], damage: 29, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseMaximumSd, "Increase Maximum SD", [CharacterClassNumber.FistMaster], damage: 33, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseManaRecoveryRate, "Increase Mana Recovery Rate", [CharacterClassNumber.FistMaster], damage: 7, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreasePoisonResistance, "Increase Poison Resistance", [CharacterClassNumber.FistMaster], damage: 1, elementalModifier: ElementalType.Poison, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DurabilityReduction2FistMaster, "Durability Reduction (2)", [CharacterClassNumber.FistMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseSdRecoveryRate, "Increase SD Recovery Rate", [CharacterClassNumber.FistMaster], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseHpRecoveryRate, "Increase HP Recovery Rate", [CharacterClassNumber.FistMaster], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseLightningResistance, "Increase Lightning Resistance", [CharacterClassNumber.FistMaster], damage: 1, elementalModifier: ElementalType.Lightning, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreasesDefense, "Increases Defense", [CharacterClassNumber.FistMaster], damage: 35, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreasesAgRecoveryRate, "Increases AG Recovery Rate", [CharacterClassNumber.FistMaster], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseIceResistance, "Increase Ice Resistance", [CharacterClassNumber.FistMaster], damage: 1, elementalModifier: ElementalType.Ice, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DurabilityReduction3FistMaster, "Durability Reduction(3)", [CharacterClassNumber.FistMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseDefenseSuccessRate, "Increase Defense Success Rate", [CharacterClassNumber.FistMaster], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseAttackSuccessRate, "Increase Attack Success Rate", [CharacterClassNumber.FistMaster], damage: 30, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseMaximumHp, "Increase Maximum HP", [CharacterClassNumber.FistMaster], damage: 34, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseMaximumMana, "Increase Maximum Mana", [CharacterClassNumber.FistMaster], damage: 34, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreasePvPAttackRate, "Increase PvP Attack Rate", [CharacterClassNumber.FistMaster], damage: 31, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DecreaseMana, "Decrease Mana", [CharacterClassNumber.FistMaster], damage: 18, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.RecoverSDfromMonsterKills, "Recover SD from Monster Kills", [CharacterClassNumber.FistMaster], damage: 11, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.RecoverHPfromMonsterKills, "Recover HP from Monster Kills", [CharacterClassNumber.FistMaster], damage: 6, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseMinimumAttackPower, "Increase Minimum Attack Power", [CharacterClassNumber.FistMaster], damage: 22, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.RecoverManaMonsterKills, "Recover Mana Monster Kills", [CharacterClassNumber.FistMaster], damage: 6, skillType: SkillType.PassiveBoost);
        */
        this.InitializeEffects();
        this.MapSkillsToEffects();
        this.InitializeMasterSkillData();
        this.CreateSpecialSummonMonsters();
        this.CreateSkillCombos();
        this.InitializeSkillAttributes();
    }

    // ReSharper disable once UnusedMember.Local
    private void InitializeNextSeasonMasterSkills()
    {
        // Common:
        /*
        this.CreateSkill(SkillNumber.CastInvincibility, "Cast Invincibility", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.ArmorSetBonusInc, "Armor Set Bonus Inc", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.Vengeance, "Vengeance", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.EnergyIncrease, "Energy Increase", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.StaminaIncrease, "Stamina Increase", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.AgilityIncrease, "Agility Increase", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.StrengthIncrease, "Strength Increase", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.SwellLifeMastery, "Swell Life Mastery", [CharacterClassNumber.BladeMaster], damage: 7, abilityConsumption: 30, manaConsumption: 28, levelRequirement: 120);
        this.CreateSkill(SkillNumber.MaximumAttackPowerInc, "Maximum Attack Power Inc", [CharacterClassNumber.BladeMaster, CharacterClassNumber.DuelMaster, CharacterClassNumber.LordEmperor], DamageType.Physical, 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.Inccritdamagerate, "Inc crit damage rate", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 7, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.RestoresallMana, "Restores all Mana", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.RestoresallHp, "Restores all HP", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.Incexcdamagerate, "Inc exc damage rate", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.Incdoubledamagerate, "Inc double damage rate", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncchanceofignoreDef, "Inc chance of ignore Def", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.RestoresallSd, "Restores all SD", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.Inctripledamagerate, "Inc triple damage rate", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);

        // Blade Master:
        this.CreateSkill(SkillNumber.WingofStormAbsPowUp, "Wing of Storm Abs PowUp", [CharacterClassNumber.BladeMaster], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.WingofStormDefPowUp, "Wing of Storm Def PowUp", [CharacterClassNumber.BladeMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IronDefense, "Iron Defense", ClassConsts.ALL_MASTERS, damage: 1);
        this.CreateSkill(SkillNumber.WingofStormAttPowUp, "Wing of Storm Att PowUp", [CharacterClassNumber.BladeMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DeathStabProficiency, "Death Stab Proficiency", [CharacterClassNumber.BladeMaster], DamageType.Physical, 7, 2, 26, 30, 160, elementalModifier: ElementalType.Wind);
        this.CreateSkill(SkillNumber.StrikeofDestrProf, "Strike of Destr Prof", [CharacterClassNumber.BladeMaster], DamageType.Physical, 7, 5, 24, 30, 100, elementalModifier: ElementalType.Ice);
        this.CreateSkill(SkillNumber.MaximumAgIncrease, "Maximum AG Increase", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 8, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DeathStabMastery, "Death Stab Mastery", [CharacterClassNumber.BladeMaster], DamageType.Physical, 7, 2, 26, 30, 160, elementalModifier: ElementalType.Wind);
        this.CreateSkill(SkillNumber.StrikeofDestrMast, "Strike of Destr Mast", [CharacterClassNumber.BladeMaster], DamageType.Physical, 1, 5, 24, 30, 100, elementalModifier: ElementalType.Ice);
        this.CreateSkill(SkillNumber.BloodStorm, "Blood Storm", [CharacterClassNumber.BladeMaster, CharacterClassNumber.DuelMaster], DamageType.Physical, 25, 3, 29, 87);
        this.CreateSkill(SkillNumber.ComboStrengthener, "Combo Strengthener", [CharacterClassNumber.BladeMaster], DamageType.Physical, 7, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.BloodStormStrengthener, "Blood Storm Strengthener", [CharacterClassNumber.BladeMaster, CharacterClassNumber.DuelMaster], DamageType.Physical, 22, 3, 29, 87);

        // Grand Master:
        this.CreateSkill(SkillNumber.EternalWingsAbsPowUp, "Eternal Wings Abs PowUp", [CharacterClassNumber.GrandMaster], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.EternalWingsDefPowUp, "Eternal Wings Def PowUp", [CharacterClassNumber.GrandMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.EternalWingsAttPowUp, "Eternal Wings Att PowUp", [CharacterClassNumber.GrandMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.MeteorStrengthener, "Meteor Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 4, 6, manaConsumption: 13, levelRequirement: 21, energyRequirement: 100, elementalModifier: ElementalType.Earth);
        this.CreateSkill(SkillNumber.IceStormStrengthener, "Ice Storm Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 22, 6, 5, 110, 93, 223, elementalModifier: ElementalType.Ice);
        this.CreateSkill(SkillNumber.NovaStrengthener, "Nova Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 22, 6, 49, 198, 100, 258, elementalModifier: ElementalType.Fire);
        this.CreateSkill(SkillNumber.IceStormMastery, "Ice Storm Mastery", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 1, 6, 5, 110, 93, 223, elementalModifier: ElementalType.Ice);
        this.CreateSkill(SkillNumber.MeteorMastery, "Meteor Mastery", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 1, 6, manaConsumption: 14, levelRequirement: 21, energyRequirement: 100, elementalModifier: ElementalType.Earth);
        this.CreateSkill(SkillNumber.NovaCastStrengthener, "Nova Cast Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 22, 6, 49, 198, 100, 258, elementalModifier: ElementalType.Fire);
        this.CreateSkill(SkillNumber.SoulBarrierMastery, "Soul Barrier Mastery", [CharacterClassNumber.GrandMaster], damage: 7, distance: 6, abilityConsumption: 28, manaConsumption: 92, levelRequirement: 77, energyRequirement: 126);
        this.CreateSkill(SkillNumber.MaximumWizardryInc, "Maximum Wizardry Inc", [CharacterClassNumber.GrandMaster, CharacterClassNumber.DuelMaster], damage: 3, skillType: SkillType.PassiveBoost);

        // High Elf:
        this.CreateSkill(SkillNumber.IllusionWingsAbsPowUp, "Illusion Wings Abs PowUp", [CharacterClassNumber.HighElf], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IllusionWingsDefPowUp, "Illusion Wings Def PowUp", [CharacterClassNumber.HighElf], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.MultiShotStreng, "Multi-Shot Streng", [CharacterClassNumber.HighElf], DamageType.Physical, 22, 6, 7, 11, 100, skillType: SkillType.AreaSkillAutomaticHits);
        this.CreateSkill(SkillNumber.IllusionWingsAttPowUp, "Illusion Wings Att PowUp", [CharacterClassNumber.HighElf], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.Cure, "Cure", [CharacterClassNumber.HighElf], distance: 6, abilityConsumption: 10, manaConsumption: 72);
        this.CreateSkill(SkillNumber.PartyHealing, "Party Healing", [CharacterClassNumber.HighElf], distance: 6, abilityConsumption: 12, manaConsumption: 66, energyRequirement: 100);
        this.CreateSkill(SkillNumber.PoisonArrow, "Poison Arrow", [CharacterClassNumber.HighElf], DamageType.Physical, 27, 6, 27, 22, elementalModifier: ElementalType.Poison);
        this.CreateSkill(SkillNumber.SummonedMonsterStr3, "Summoned Monster Str (3)", [CharacterClassNumber.HighElf], damage: 16, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.PartyHealingStr, "Party Healing Str", [CharacterClassNumber.HighElf], damage: 22, distance: 6, abilityConsumption: 13, manaConsumption: 72, energyRequirement: 100);
        this.CreateSkill(SkillNumber.Bless, "Bless", [CharacterClassNumber.HighElf], distance: 6, abilityConsumption: 18, manaConsumption: 108, energyRequirement: 100);
        this.CreateSkill(SkillNumber.MultiShotMastery, "Multi-Shot Mastery", [CharacterClassNumber.HighElf], DamageType.Physical, 1, 6, 8, 12, 100, skillType: SkillType.AreaSkillAutomaticHits);
        this.CreateSkill(SkillNumber.SummonSatyros, "Summon Satyros", [CharacterClassNumber.HighElf], abilityConsumption: 52, manaConsumption: 525, energyRequirement: 280);
        this.CreateSkill(SkillNumber.BlessStrengthener, "Bless Strengthener", [CharacterClassNumber.HighElf], damage: 10, distance: 6, abilityConsumption: 20, manaConsumption: 118, energyRequirement: 100);
        this.CreateSkill(SkillNumber.PoisonArrowStr, "Poison Arrow Str", [CharacterClassNumber.HighElf], DamageType.Physical, 22, 6, 29, 24, elementalModifier: ElementalType.Poison);
        this.CreateSkill(SkillNumber.MaximumAttPowerInc, "Maximum Att Power Inc", [CharacterClassNumber.HighElf], DamageType.Physical, 3, skillType: SkillType.PassiveBoost);

        // Dimension Master (Summoner):
        this.CreateSkill(SkillNumber.DimensionWingsAbsPowUp, "DimensionWings Abs PowUp", [CharacterClassNumber.DimensionMaster], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DimensionWingsDefPowUp, "DimensionWings Def PowUp", [CharacterClassNumber.DimensionMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DimensionWingsAttPowUp, "DimensionWings Att PowUp", [CharacterClassNumber.DimensionMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.WeaknessStrengthener, "Weakness Strengthener", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 3, 6, 17, 55, 93, 173);
        this.CreateSkill(SkillNumber.InnovationStrengthener, "Innovation Strengthener", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 3, 6, 17, 77, 111, 201);
        this.CreateSkill(SkillNumber.Blind, "Blind", [CharacterClassNumber.DimensionMaster], DamageType.Curse, distance: 3, abilityConsumption: 25, manaConsumption: 115, energyRequirement: 201);
        this.CreateSkill(SkillNumber.DrainLifeMastery, "Drain Life Mastery", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 17, 6, manaConsumption: 62, levelRequirement: 35, energyRequirement: 93);
        this.CreateSkill(SkillNumber.BlindStrengthener, "Blind Strengthener", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 1, 3, 27, 126, energyRequirement: 201);
        this.CreateSkill(SkillNumber.BerserkerMastery, "Berserker Mastery", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 10, 5, 90, 181, 83, 181);
        this.CreateSkill(SkillNumber.MaximumWizCurseInc, "Maximum Wiz/Curse Inc", [CharacterClassNumber.DimensionMaster], damage: 3, skillType: SkillType.PassiveBoost);

        // Duel Master (MG):
        this.CreateSkill(SkillNumber.WingofRuinAbsPowUp, "Wing of Ruin Abs PowUp", [CharacterClassNumber.DuelMaster], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.WingofRuinDefPowUp, "Wing of Ruin Def PowUp", [CharacterClassNumber.DuelMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.WingofRuinAttPowUp, "Wing of Ruin Att PowUp", [CharacterClassNumber.DuelMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IceMasteryDuelMaster, "Ice Mastery", [CharacterClassNumber.DuelMaster], DamageType.Physical, 1, 6, manaConsumption: 46, levelRequirement: 25, energyRequirement: 100, elementalModifier: ElementalType.Ice);
        this.CreateSkill(SkillNumber.FlameStrikeStrengthen, "Flame Strike Strengthen", [CharacterClassNumber.DuelMaster], DamageType.Physical, 22, 3, 37, 30, elementalModifier: ElementalType.Fire);
        this.CreateSkill(SkillNumber.FireSlashMastery, "Fire Slash Mastery", [CharacterClassNumber.DuelMaster], damage: 7, distance: 3, abilityConsumption: 24, manaConsumption: 17, elementalModifier: ElementalType.Poison);
        this.CreateSkill(SkillNumber.FlameStrikeMastery, "Flame Strike Mastery", [CharacterClassNumber.DuelMaster], DamageType.Physical, 7, 3, 40, 33, elementalModifier: ElementalType.Fire);
        this.CreateSkill(SkillNumber.EarthPrison, "Earth Prison", [CharacterClassNumber.GrandMaster, CharacterClassNumber.DuelMaster], DamageType.Physical, 26, 3, 15, 180, energyRequirement: 127, elementalModifier: ElementalType.Earth);
        this.CreateSkill(SkillNumber.GiganticStormStr, "Gigantic Storm Str", [CharacterClassNumber.DuelMaster], DamageType.Physical, 22, 6, 11, 132, 220, 118, elementalModifier: ElementalType.Wind);
        this.CreateSkill(SkillNumber.EarthPrisonStr, "Earth Prison Str", [CharacterClassNumber.GrandMaster, CharacterClassNumber.DuelMaster], DamageType.Physical, 22, 3, 17, 198, energyRequirement: 127, elementalModifier: ElementalType.Earth);

        // Lord Emperor (DL):
        this.CreateSkill(SkillNumber.EmperorCapeAbsPowUp, "Emperor Cape Abs PowUp", [CharacterClassNumber.LordEmperor], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.EmperorCapeDefPowUp, "Emperor Cape Def PowUp", [CharacterClassNumber.LordEmperor], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.AddsCommandStat, "Adds Command Stat", [CharacterClassNumber.LordEmperor], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.EmperorCapeAttPowUp, "Emperor Cape Att PowUp", [CharacterClassNumber.LordEmperor], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.ElectricSparkStreng, "Electric Spark Streng", [CharacterClassNumber.LordEmperor], DamageType.Physical, 3, 10, 150, levelRequirement: 92, energyRequirement: 29, leadershipRequirement: 340, skillType: SkillType.AreaSkillAutomaticHits);
        this.CreateSkill(SkillNumber.FireScreamMastery, "Fire Scream Mastery", [CharacterClassNumber.LordEmperor], DamageType.Physical, 5, 6, 12, 49, 102, 32, 70);
        this.CreateSkill(SkillNumber.IronDefenseLordEmperor, "Iron Defense", [CharacterClassNumber.LordEmperor], damage: 28, abilityConsumption: 29, manaConsumption: 64);
        this.CreateSkill(SkillNumber.CriticalDamageIncM, "Critical Damage Inc M", [CharacterClassNumber.LordEmperor], damage: 1, abilityConsumption: 110, manaConsumption: 110, levelRequirement: 82, energyRequirement: 25, leadershipRequirement: 300);
        this.CreateSkill(SkillNumber.ChaoticDiseierStr, "Chaotic Diseier Str", [CharacterClassNumber.LordEmperor], DamageType.Physical, 22, 6, 22, 75, 100, 16, skillType: SkillType.AreaSkillAutomaticHits);
        this.CreateSkill(SkillNumber.IronDefenseStr, "Iron Defense Str", [CharacterClassNumber.LordEmperor], damage: 3, abilityConsumption: 31, manaConsumption: 70);
        this.CreateSkill(SkillNumber.DarkSpiritStr4, "Dark Spirit Str (4)", [CharacterClassNumber.LordEmperor], damage: 23, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DarkSpiritStr5, "Dark Spirit Str (5)", [CharacterClassNumber.LordEmperor], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.SpiritLord, "Spirit Lord", [CharacterClassNumber.LordEmperor], damage: 1, skillType: SkillType.PassiveBoost);

        // Fist Master (Rage Fighter):
        this.CreateSkill(SkillNumber.CastInvincibilityFistMaster, "Cast Invincibility", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseMaximumAg, "Increase Maximum AG", [CharacterClassNumber.FistMaster], damage: 37, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseMaximumAttackPower, "Increase Maximum Attack Power", [CharacterClassNumber.FistMaster], damage: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreasesCritDamageChance, "Increases Crit Damage Chance", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.RecoverManaFully, "Recover Mana Fully", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.RecoversHpFully, "Recovers HP Fully", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseExcDamageChance, "Increase Exc Damage Chance", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseDoubleDamageChance, "Increase Double Damage Chance", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseIgnoreDefChance, "Increase Ignore Def Chance", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.RecoversSdFully, "Recovers SD Fully", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseTripleDamageChance, "Increase Triple Damage Chance", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        */
    }

    private void CreateSkillCombos()
    {
        var bladeKnightCombo = this.Context.CreateNew<SkillComboDefinition>();
        var bladeKnight = this.GameConfiguration.CharacterClasses.First(c => c.Number == (byte)CharacterClassNumber.BladeKnight);
        bladeKnight.ComboDefinition = bladeKnightCombo;

        bladeKnightCombo.Name = "Blade Knight Combo";

        this.AddComboStep(SkillNumber.Slash, 1, bladeKnightCombo);
        this.AddComboStep(SkillNumber.Cyclone, 1, bladeKnightCombo);
        this.AddComboStep(SkillNumber.Lunge, 1, bladeKnightCombo);
        this.AddComboStep(SkillNumber.FallingSlash, 1, bladeKnightCombo);
        this.AddComboStep(SkillNumber.Uppercut, 1, bladeKnightCombo);

        this.AddComboStep(SkillNumber.TwistingSlash, 2, bladeKnightCombo);
        this.AddComboStep(SkillNumber.AngerBlow, 2, bladeKnightCombo);
        this.AddComboStep(SkillNumber.DeathStab, 2, bladeKnightCombo);
        this.AddComboStep(SkillNumber.StrikeofDestruction, 2, bladeKnightCombo);

        this.AddComboStep(SkillNumber.TwistingSlash, 3, bladeKnightCombo, true);
        this.AddComboStep(SkillNumber.AngerBlow, 3, bladeKnightCombo, true);
        this.AddComboStep(SkillNumber.DeathStab, 3, bladeKnightCombo, true);
    }

    private void AddComboStep(SkillNumber skillNumber, int order, SkillComboDefinition comboDefinition, bool isFinal = false)
    {
        var skill = this.GameConfiguration.Skills.First(s => s.Number == (short)skillNumber);
        var step = this.Context.CreateNew<SkillComboStep>();
        comboDefinition.Steps.Add(step);
        comboDefinition.MaximumCompletionTime = TimeSpan.FromSeconds(3);
        step.Skill = skill;
        step.Order = order;
        step.IsFinalStep = isFinal;
    }

    private void InitializeSkillAttributes()
    {
        this.AddAttributeRelationship(SkillNumber.Nova, Stats.SkillDamageBonus, 1.0f / 2, Stats.TotalStrength);
        this.AddAttributeRelationship(SkillNumber.Nova, Stats.SkillDamageBonus, 1, Stats.NovaStageDamage);

        this.AddAttributeRelationship(SkillNumber.Earthshake, Stats.SkillDamageBonus, 1.0f / 10, Stats.TotalStrength);
        this.AddAttributeRelationship(SkillNumber.Earthshake, Stats.SkillDamageBonus, 1.0f / 5, Stats.TotalLeadership);
        this.AddAttributeRelationship(SkillNumber.Earthshake, Stats.SkillDamageBonus, 10, Stats.HorseLevel);

        this.AddAttributeRelationship(SkillNumber.ElectricSpark, Stats.SkillDamageBonus, 50, Stats.NearbyPartyMemberCount);
        this.AddAttributeRelationship(SkillNumber.ElectricSpark, Stats.SkillDamageBonus, 1.0f / 10, Stats.TotalLeadership);

        this.AddAttributeRelationship(SkillNumber.ChaoticDiseier, Stats.SkillDamageBonus, 1.0f / 30, Stats.TotalStrength);
        this.AddAttributeRelationship(SkillNumber.ChaoticDiseier, Stats.SkillDamageBonus, 1.0f / 55, Stats.TotalEnergy);

        SkillNumber[] lordSkills = [SkillNumber.Force, SkillNumber.FireBlast, SkillNumber.FireBurst, SkillNumber.ForceWave, SkillNumber.FireScream];
        foreach (var lordSkillNumber in lordSkills)
        {
            this.AddAttributeRelationship(lordSkillNumber, Stats.SkillDamageBonus, 1.0f / 25, Stats.TotalStrength);
            this.AddAttributeRelationship(lordSkillNumber, Stats.SkillDamageBonus, 1.0f / 50, Stats.TotalEnergy);
        }

        this.AddAttributeRelationship(SkillNumber.MultiShot, Stats.SkillMultiplier, 0.8f, Stats.SkillMultiplier, AggregateType.Multiplicate);
    }

    private void AddAttributeRelationship(SkillNumber skillNumber, AttributeDefinition targetAttribute, float multiplier, AttributeDefinition sourceAttribute, AggregateType aggregateType = AggregateType.AddRaw)
    {
        var skill = this.GameConfiguration.Skills.First(s => s.Number == (int)skillNumber);
        var relationship = CharacterClassHelper.CreateAttributeRelationship(this.Context, this.GameConfiguration, targetAttribute, multiplier, sourceAttribute, aggregateType: aggregateType);
        skill.AttributeRelationships.Add(relationship);
    }

    private void InitializeEffects()
    {
        new SoulBarrierEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new LifeSwellEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new CriticalDamageIncreaseEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new DefenseEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new GreaterDamageEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new GreaterDefenseEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new HealEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new ShieldRecoverEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new InfiniteArrowEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new DefenseReductionEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new InvisibleEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new IgnoreDefenseEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new IncreaseHealthEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new IncreaseBlockEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new WizardryEnhanceEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new AlcoholEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new SoulPotionEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new BlessPotionEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new BerserkerEffectInitializer(this.Context, this.GameConfiguration).Initialize();
    }

    private void MapSkillsToEffects()
    {
        foreach (var effectOfSkill in EffectsOfSkills)
        {
            var skill = this.GameConfiguration.Skills.First(s => s.Number == (short)effectOfSkill.Key);
            var effect = this.GameConfiguration.MagicEffects.First(e => e.Number == (short)effectOfSkill.Value);
            skill.MagicEffectDef = effect;
        }
    }

    /// <summary>
    /// Initializes the master skill data.
    /// </summary>
    private void InitializeMasterSkillData()
    {
        // Roots:
        var leftRoot = this.Context.CreateNew<MasterSkillRoot>();
        leftRoot.Name = "Left (Common Skills)";
        this._masterSkillRoots.Add(1, leftRoot);
        this.GameConfiguration.MasterSkillRoots.Add(leftRoot);
        var middleRoot = this.Context.CreateNew<MasterSkillRoot>();
        middleRoot.Name = "Middle Root";
        this._masterSkillRoots.Add(2, middleRoot);
        this.GameConfiguration.MasterSkillRoots.Add(middleRoot);
        var rightRoot = this.Context.CreateNew<MasterSkillRoot>();
        rightRoot.Name = "Right Root";
        this._masterSkillRoots.Add(3, rightRoot);
        this.GameConfiguration.MasterSkillRoots.Add(rightRoot);
        /*
        // Universal
        this.AddPassiveMasterSkillDefinition(SkillNumber.DurabilityReduction1, Stats.ItemDurationIncrease, AggregateType.Multiplicate, Formula1204, 1, 1);
        this.AddPassiveMasterSkillDefinition(SkillNumber.PvPDefenceRateInc, Stats.DefenseRatePvp, AggregateType.AddRaw, Formula61408, 1, 1);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MaximumSDincrease, Stats.MaximumShield, AggregateType.AddRaw, Formula51173, 2, 1);
        this.AddPassiveMasterSkillDefinition(SkillNumber.AutomaticManaRecInc, Stats.ManaRecoveryMultiplier, AggregateType.AddRaw, FormulaRecoveryIncrease181, Formula181, 2, 1, SkillNumber.Undefined, SkillNumber.Undefined, 20);
        this.AddPassiveMasterSkillDefinition(SkillNumber.PoisonResistanceInc, Stats.PoisonResistance, AggregateType.AddRaw, Formula120, Formula120, 2, 1);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DurabilityReduction2, Stats.ItemDurationIncrease, AggregateType.Multiplicate, Formula1204, 3, 1, SkillNumber.DurabilityReduction1);
        this.AddMasterSkillDefinition(SkillNumber.SdRecoverySpeedInc, SkillNumber.MaximumSDincrease, SkillNumber.Undefined, 1, 3, SkillNumber.Undefined, 20, Formula120);
        this.AddPassiveMasterSkillDefinition(SkillNumber.AutomaticHpRecInc, Stats.HealthRecoveryMultiplier, AggregateType.AddRaw, FormulaRecoveryIncrease120, Formula120, 3, 1, SkillNumber.AutomaticManaRecInc, SkillNumber.Undefined, 20);
        this.AddPassiveMasterSkillDefinition(SkillNumber.LightningResistanceInc, Stats.LightningResistance, AggregateType.AddRaw, Formula120, Formula120, 2, 1, requiredSkill1: SkillNumber.PoisonResistanceInc);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DefenseIncrease, Stats.DefenseBase, AggregateType.AddFinal, Formula6020, 4, 1);
        this.AddPassiveMasterSkillDefinition(SkillNumber.AutomaticAgRecInc, Stats.AbilityRecoveryMultiplier, AggregateType.AddRaw, FormulaRecoveryIncrease120, Formula120, 4, 1, SkillNumber.AutomaticHpRecInc, SkillNumber.Undefined, 20);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IceResistanceIncrease, Stats.IceResistance, AggregateType.AddRaw, Formula120, Formula120, 2, 1, requiredSkill1: SkillNumber.LightningResistanceInc);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DurabilityReduction3, Stats.ItemDurationIncrease, AggregateType.Multiplicate, Formula1204, 5, 1, SkillNumber.DurabilityReduction2);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DefenseSuccessRateInc, Stats.DefenseRatePvm, AggregateType.Multiplicate, $"1 + {Formula120} / 100", 5, 1, SkillNumber.DefenseIncrease);

        // DK
        this.AddPassiveMasterSkillDefinition(SkillNumber.AttackSuccRateInc, Stats.AttackRatePvm, AggregateType.AddRaw, Formula51173, 1, 2);
        this.AddMasterSkillDefinition(SkillNumber.CycloneStrengthener, SkillNumber.Cyclone, SkillNumber.Undefined, 2, 2, SkillNumber.Cyclone, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.SlashStrengthener, SkillNumber.Slash, SkillNumber.Undefined, 2, 2, SkillNumber.Slash, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.FallingSlashStreng, SkillNumber.FallingSlash, SkillNumber.Undefined, 2, 2, SkillNumber.FallingSlash, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.LungeStrengthener, SkillNumber.Lunge, SkillNumber.Undefined, 2, 2, SkillNumber.Lunge, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.TwistingSlashStreng, SkillNumber.TwistingSlash, SkillNumber.Undefined, 2, 3, SkillNumber.TwistingSlash, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.RagefulBlowStreng, SkillNumber.RagefulBlow, SkillNumber.Undefined, 2, 3, SkillNumber.RagefulBlow, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.TwistingSlashMastery, SkillNumber.TwistingSlashStreng, SkillNumber.Undefined, 2, 4, SkillNumber.TwistingSlash, 20, Formula120);
        this.AddMasterSkillDefinition(SkillNumber.RagefulBlowMastery, SkillNumber.RagefulBlowStreng, SkillNumber.Undefined, 2, 4, SkillNumber.RagefulBlow, 20, Formula120);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MaximumLifeIncrease, Stats.MaximumHealth, AggregateType.AddRaw, Formula10235, 4, 2);
        this.AddPassiveMasterSkillDefinition(SkillNumber.WeaponMasteryBladeMaster, Stats.MasterSkillPhysBonusDmg, AggregateType.AddRaw, Formula502, 4, 2);
        this.AddMasterSkillDefinition(SkillNumber.DeathStabStrengthener, SkillNumber.DeathStab, SkillNumber.Undefined, 2, 5, SkillNumber.DeathStab, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.StrikeofDestrStr, SkillNumber.StrikeofDestruction, SkillNumber.Undefined, 2, 5, SkillNumber.StrikeofDestruction, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MaximumManaIncrease, Stats.MaximumMana, AggregateType.AddRaw, Formula10235, 5, 2, SkillNumber.MaximumLifeIncrease);
        this.AddPassiveMasterSkillDefinition(SkillNumber.PvPAttackRate, Stats.AttackRatePvp, AggregateType.AddRaw, Formula81877, 1, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.TwoHandedSwordStrengthener, Stats.TwoHandedSwordStrBonusDamage, AggregateType.AddRaw, Formula883, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.OneHandedSwordStrengthener, Stats.OneHandedSwordBonusDamage, AggregateType.AddRaw, Formula502, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MaceStrengthener, Stats.MaceBonusDamage, AggregateType.AddRaw, Formula632, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.SpearStrengthener, Stats.SpearBonusDamage, AggregateType.AddRaw, Formula632, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.TwoHandedSwordMaster, Stats.TwoHandedSwordMasteryBonusDamage, AggregateType.AddRaw, Formula1154, 3, 3, SkillNumber.TwoHandedSwordStrengthener);
        this.AddPassiveMasterSkillDefinition(SkillNumber.OneHandedSwordMaster, Stats.WeaponMasteryAttackSpeed, AggregateType.AddRaw, Formula1, 3, 3, SkillNumber.OneHandedSwordStrengthener, SkillNumber.Undefined, 10);

        // todo: Probability of stunning the target for 2 seconds according to the assigned Skill Level while using a Mace.
        this.AddMasterSkillDefinition(SkillNumber.MaceMastery, SkillNumber.MaceStrengthener, SkillNumber.Undefined, 3, 3, SkillNumber.Undefined, 20, Formula120);

        // todo: Increases the probability of Double Damage while using a Spear according to the assigned Skill Level.
        this.AddMasterSkillDefinition(SkillNumber.SpearMastery, SkillNumber.SpearStrengthener, SkillNumber.Undefined, 3, 3, SkillNumber.Undefined, 20, Formula120);

        this.AddMasterSkillDefinition(SkillNumber.SwellLifeStrengt, SkillNumber.SwellLife, SkillNumber.Undefined, 3, 4, SkillNumber.SwellLife, 20, Formula181);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ManaReduction, Stats.ManaUsageReduction, AggregateType.AddRaw, Formula722Value, Formula722,  4, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MonsterAttackSdInc, Stats.ShieldAfterMonsterKillMultiplier, AggregateType.AddFinal, Formula914, 4, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MonsterAttackLifeInc, Stats.HealthAfterMonsterKillMultiplier, AggregateType.AddFinal, Formula4319, 4, 3);
        this.AddMasterSkillDefinition(SkillNumber.SwellLifeProficiency, SkillNumber.SwellLifeStrengt, SkillNumber.Undefined, 3, 5, SkillNumber.SwellLife, 20, Formula181);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MinimumAttackPowerInc, Stats.MinimumPhysBaseDmg, AggregateType.AddRaw, Formula502, 5, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MonsterAttackManaInc, Stats.ManaAfterMonsterKillMultiplier, AggregateType.AddFinal, Formula4319, 5, 3, SkillNumber.MonsterAttackLifeInc);

        // DW
        this.AddMasterSkillDefinition(SkillNumber.FlameStrengthener, SkillNumber.Flame, SkillNumber.Undefined, 2, 2, SkillNumber.Flame, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.LightningStrengthener, SkillNumber.Lightning, SkillNumber.Undefined, 2, 2, SkillNumber.Lightning, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.ExpansionofWizStreng, SkillNumber.ExpansionofWizardry, SkillNumber.Undefined, 2, 2, SkillNumber.ExpansionofWizardry, 20, Formula120Value, Formula120, Stats.MaximumWizBaseDmg, AggregateType.Multiplicate);
        this.AddMasterSkillDefinition(SkillNumber.InfernoStrengthener, SkillNumber.Inferno, SkillNumber.FlameStrengthener, 2, 3, SkillNumber.Inferno, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.BlastStrengthener, SkillNumber.Cometfall, SkillNumber.LightningStrengthener, 2, 3, SkillNumber.Cometfall, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.ExpansionofWizMas, SkillNumber.ExpansionofWizStreng, SkillNumber.Undefined, 2, 3, SkillNumber.ExpansionofWizardry, 20, Formula120Value, Formula120, targetAttribute: Stats.CriticalDamageChance, AggregateType.Multiplicate);
        this.AddMasterSkillDefinition(SkillNumber.PoisonStrengthener, SkillNumber.Poison, SkillNumber.Undefined, 2, 3, SkillNumber.Poison, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.EvilSpiritStreng, SkillNumber.EvilSpirit, SkillNumber.Undefined, 2, 4, SkillNumber.EvilSpirit, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MagicMasteryGrandMaster, Stats.WizardryBaseDmg, AggregateType.AddRaw, Formula502, 4, 2, SkillNumber.EvilSpiritStreng);
        this.AddMasterSkillDefinition(SkillNumber.DecayStrengthener, SkillNumber.Decay, SkillNumber.PoisonStrengthener, 2, 4, SkillNumber.Decay, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.HellfireStrengthener, SkillNumber.Hellfire, SkillNumber.Undefined, 2, 5, SkillNumber.Hellfire, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.IceStrengthener, SkillNumber.Ice, SkillNumber.Undefined, 2, 5, SkillNumber.Ice, 20, Formula632);
        this.AddPassiveMasterSkillDefinition(SkillNumber.OneHandedStaffStrengthener, Stats.OneHandedStaffBonusBaseDamage, AggregateType.AddRaw, Formula502, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.TwoHandedStaffStrengthener, Stats.TwoHandedStaffBonusBaseDamage, AggregateType.AddRaw, Formula883, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ShieldStrengthenerGrandMaster, Stats.BonusDefenseWithShield, AggregateType.AddRaw, Formula803, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.OneHandedStaffMaster, Stats.WeaponMasteryAttackSpeed, AggregateType.AddRaw, Formula1, 3, 3, SkillNumber.OneHandedStaffStrengthener, SkillNumber.Undefined, 10);
        this.AddPassiveMasterSkillDefinition(SkillNumber.TwoHandedStaffMaster, Stats.TwoHandedStaffMasteryBonusDamage, AggregateType.AddRaw, Formula1154, 3, 3, SkillNumber.TwoHandedStaffStrengthener);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ShieldMasteryGrandMaster, Stats.BonusDefenseRateWithShield, AggregateType.AddRaw, Formula1204, 3, 3, SkillNumber.ShieldStrengthenerGrandMaster);
        this.AddMasterSkillDefinition(SkillNumber.SoulBarrierStrength, SkillNumber.SoulBarrier, SkillNumber.Undefined, 3, 4, SkillNumber.SoulBarrier, 20, $"{Formula181} / 100", Formula181, Stats.SoulBarrierReceiveDecrement, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(SkillNumber.SoulBarrierProficie, SkillNumber.SoulBarrierStrength, SkillNumber.Undefined, 3, 5, SkillNumber.SoulBarrierStrength, 20, Formula803, extendsDuration: true);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MinimumWizardryInc, Stats.MinimumWizBaseDmg, AggregateType.AddRaw, Formula502, 5, 3);

        // ELF
        this.AddMasterSkillDefinition(SkillNumber.HealStrengthener, SkillNumber.Heal, SkillNumber.Undefined, 2, 2, SkillNumber.Heal, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.TripleShotStrengthener, SkillNumber.TripleShot, SkillNumber.Undefined, 2, 2, SkillNumber.TripleShot, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.SummonedMonsterStr1, Stats.SummonedMonsterHealthIncrease, AggregateType.AddRaw, Formula6020Value, 2, 2, SkillNumber.SummonGoblin);
        this.AddMasterSkillDefinition(SkillNumber.PenetrationStrengthener, SkillNumber.Penetration, SkillNumber.Undefined, 2, 3, SkillNumber.Penetration, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.DefenseIncreaseStr, SkillNumber.GreaterDefense, SkillNumber.Undefined, 2, 3, SkillNumber.GreaterDefense, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.TripleShotMastery, SkillNumber.TripleShotStrengthener, SkillNumber.Undefined, 2, 3, SkillNumber.TripleShot, 10, Formula1WhenComplete);
        this.AddPassiveMasterSkillDefinition(SkillNumber.SummonedMonsterStr2, Stats.SummonedMonsterDefenseIncrease, AggregateType.AddRaw, Formula6020, 2, 3, SkillNumber.SummonGoblin);
        this.AddMasterSkillDefinition(SkillNumber.AttackIncreaseStr, SkillNumber.GreaterDamage, SkillNumber.Undefined, 2, 4, SkillNumber.GreaterDamage, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.WeaponMasteryHighElf, Stats.MasterSkillPhysBonusDmg, AggregateType.AddRaw, Formula502, 4, 2);
        this.AddMasterSkillDefinition(SkillNumber.AttackIncreaseMastery, SkillNumber.AttackIncreaseStr, SkillNumber.Undefined, 2, 5, SkillNumber.GreaterDamage, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.DefenseIncreaseMastery, SkillNumber.DefenseIncreaseStr, SkillNumber.Undefined, 2, 5, SkillNumber.GreaterDefense, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.IceArrowStrengthener, SkillNumber.IceArrow, SkillNumber.Undefined, 2, 5, SkillNumber.IceArrow, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.BowStrengthener, Stats.BowStrBonusDamage, AggregateType.AddRaw, Formula502, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.CrossbowStrengthener, Stats.CrossBowStrBonusDamage, AggregateType.AddRaw, Formula632, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ShieldStrengthenerHighElf, Stats.BonusDefenseWithShield, AggregateType.AddRaw, Formula803, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.BowMastery, Stats.WeaponMasteryAttackSpeed, AggregateType.AddRaw, Formula1, 3, 3, SkillNumber.BowStrengthener, SkillNumber.Undefined, 10);
        this.AddPassiveMasterSkillDefinition(SkillNumber.CrossbowMastery, Stats.CrossBowMasteryBonusDamage, AggregateType.AddRaw, Formula1154, 3, 3, SkillNumber.CrossbowStrengthener);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ShieldMasteryHighElf, Stats.BonusDefenseRateWithShield, AggregateType.AddRaw, Formula1806, 3, 3, SkillNumber.ShieldStrengthenerHighElf);
        this.AddMasterSkillDefinition(SkillNumber.InfinityArrowStr, SkillNumber.InfinityArrow, SkillNumber.Undefined, 3, 5, SkillNumber.InfinityArrow, 20, $"1 + {Formula120} / 100", Formula120, Stats.AttackDamageIncrease, AggregateType.Multiplicate);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MinimumAttPowerInc, Stats.MinimumPhysBaseDmg, AggregateType.AddRaw, Formula502, 5, 3);

        // SUM
        this.AddMasterSkillDefinition(SkillNumber.FireTomeStrengthener, SkillNumber.Undefined, SkillNumber.Undefined, 2, 2, SkillNumber.Undefined, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.WindTomeStrengthener, SkillNumber.Undefined, SkillNumber.Undefined, 2, 2, SkillNumber.Undefined, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.LightningTomeStren, SkillNumber.Undefined, SkillNumber.Undefined, 2, 2, SkillNumber.Undefined, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.FireTomeMastery, SkillNumber.FireTomeStrengthener, SkillNumber.Undefined, 2, 3, SkillNumber.Undefined, 20, Formula181);
        this.AddMasterSkillDefinition(SkillNumber.WindTomeMastery, SkillNumber.WindTomeStrengthener, SkillNumber.Undefined, 2, 3, SkillNumber.Undefined, 20, Formula120);
        this.AddMasterSkillDefinition(SkillNumber.LightningTomeMastery, SkillNumber.LightningTomeStren, SkillNumber.Undefined, 2, 3, SkillNumber.Undefined, 20, Formula181);
        this.AddMasterSkillDefinition(SkillNumber.SleepStrengthener, SkillNumber.Sleep, SkillNumber.Undefined, 2, 3, SkillNumber.Sleep, 20, Formula120);
        this.AddMasterSkillDefinition(SkillNumber.ChainLightningStr, SkillNumber.ChainLightning, SkillNumber.Undefined, 2, 4, SkillNumber.ChainLightning, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.LightningShockStr, SkillNumber.LightningShock, SkillNumber.Undefined, 2, 4, SkillNumber.LightningShock, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MagicMasterySummoner, Stats.WizardryAndCurseBaseDmgBonus, AggregateType.AddRaw, Formula502, 5, 2);
        this.AddMasterSkillDefinition(SkillNumber.DrainLifeStrengthener, SkillNumber.DrainLife, SkillNumber.Undefined, 2, 5, SkillNumber.DrainLife, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.StickStrengthener, Stats.StickBonusBaseDamage, AggregateType.AddRaw, Formula502, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.OtherWorldTomeStreng, Stats.BookBonusBaseDamage, AggregateType.AddRaw, Formula632, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.StickMastery, Stats.StickMasteryBonusDamage, AggregateType.AddRaw, Formula1154, 3, 3, SkillNumber.StickStrengthener);
        this.AddPassiveMasterSkillDefinition(SkillNumber.OtherWorldTomeMastery, Stats.WeaponMasteryAttackSpeed, AggregateType.AddRaw, Formula1, 3, 3, SkillNumber.OtherWorldTomeStreng, SkillNumber.Undefined, 10);
        this.AddMasterSkillDefinition(SkillNumber.BerserkerStrengthener, SkillNumber.Berserker, SkillNumber.Undefined, 3, 4, SkillNumber.Berserker, 20, $"{Formula181} / 100", Formula181, Stats.BerserkerCurseMultiplier, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(SkillNumber.BerserkerProficiency, SkillNumber.BerserkerStrengthener, SkillNumber.Undefined, 3, 5, SkillNumber.BerserkerStrengthener, 20, $"{Formula181} / 100", Formula181, Stats.BerserkerProficiencyMultiplier, AggregateType.AddRaw);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MinimumWizCurseInc, Stats.MinWizardryAndCurseDmgBonus, AggregateType.AddRaw, Formula502, 5, 3);

        // MG
        this.AddMasterSkillDefinition(SkillNumber.CycloneStrengthenerDuelMaster, SkillNumber.Cyclone, SkillNumber.Undefined, 2, 2, SkillNumber.Cyclone, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.LightningStrengthenerDuelMaster, SkillNumber.Lightning, SkillNumber.Undefined, 2, 2, SkillNumber.Lightning, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.TwistingSlashStrengthenerDuelMaster, SkillNumber.TwistingSlash, SkillNumber.Undefined, 2, 2, SkillNumber.TwistingSlash, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.PowerSlashStreng, SkillNumber.PowerSlash, SkillNumber.Undefined, 2, 2, SkillNumber.PowerSlash, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.FlameStrengthenerDuelMaster, SkillNumber.Flame, SkillNumber.Undefined, 2, 3, SkillNumber.Flame, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.BlastStrengthenerDuelMaster, SkillNumber.Cometfall, SkillNumber.LightningStrengthenerDuelMaster, 2, 3, SkillNumber.Cometfall, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.WeaponMasteryDuelMaster, Stats.MasterSkillPhysBonusDmg, AggregateType.AddRaw, Formula502, 3, 2, SkillNumber.TwistingSlashStrengthenerDuelMaster, SkillNumber.PowerSlashStreng);
        this.AddMasterSkillDefinition(SkillNumber.InfernoStrengthenerDuelMaster, SkillNumber.Inferno, SkillNumber.FlameStrengthenerDuelMaster, 2, 4, SkillNumber.Inferno, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.EvilSpiritStrengthenerDuelMaster, SkillNumber.EvilSpirit, SkillNumber.Undefined, 2, 4, SkillNumber.EvilSpirit, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MagicMasteryDuelMaster, Stats.WizardryBaseDmg, AggregateType.AddRaw, Formula502, 4, 2, SkillNumber.EvilSpiritStrengthenerDuelMaster);
        this.AddMasterSkillDefinition(SkillNumber.IceStrengthenerDuelMaster, SkillNumber.Ice, SkillNumber.Undefined, 2, 5, SkillNumber.Ice, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.BloodAttackStrengthen, SkillNumber.FireSlash, SkillNumber.Undefined, 2, 5, SkillNumber.FireSlash, 20, Formula502);

        // DL
        this.AddMasterSkillDefinition(SkillNumber.FireBurstStreng, SkillNumber.FireBurst, SkillNumber.Undefined, 2, 2, SkillNumber.FireBurst, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.ForceWaveStreng, SkillNumber.Force, SkillNumber.Undefined, 2, 2, SkillNumber.ForceWave, 20, Formula632);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DarkHorseStreng1, Stats.BonusDefenseWithHorse, AggregateType.AddRaw, Formula1204, 2, 2);
        this.AddMasterSkillDefinition(SkillNumber.CriticalDmgIncPowUp, SkillNumber.IncreaseCriticalDamage, SkillNumber.Undefined, 2, 3, SkillNumber.IncreaseCriticalDamage, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.EarthshakeStreng, SkillNumber.Earthshake, SkillNumber.DarkHorseStreng1, 2, 3, SkillNumber.Earthshake, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.WeaponMasteryLordEmperor, Stats.MasterSkillPhysBonusDmg, AggregateType.AddRaw, Formula502, 3, 2);
        this.AddMasterSkillDefinition(SkillNumber.FireBurstMastery, SkillNumber.FireBurstStreng, SkillNumber.Undefined, 2, 4, SkillNumber.FireBurst, 20, Formula120);
        this.AddMasterSkillDefinition(SkillNumber.CritDmgIncPowUp2, SkillNumber.CriticalDmgIncPowUp, SkillNumber.Undefined, 2, 4, SkillNumber.IncreaseCriticalDamage, 20, Formula803);
        this.AddMasterSkillDefinition(SkillNumber.EarthshakeMastery, SkillNumber.EarthshakeStreng, SkillNumber.Undefined, 2, 4, SkillNumber.Earthshake, 20, Formula120);
        this.AddMasterSkillDefinition(SkillNumber.CritDmgIncPowUp3, SkillNumber.CritDmgIncPowUp2, SkillNumber.Undefined, 2, 5, SkillNumber.IncreaseCriticalDamage, 20, Formula181);
        this.AddMasterSkillDefinition(SkillNumber.FireScreamStren, SkillNumber.FireScream, SkillNumber.Undefined, 2, 5, SkillNumber.FireScream, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DarkSpiritStr, Stats.RavenBonusDamage, AggregateType.AddRaw, Formula632, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ScepterStrengthener, Stats.ScepterStrBonusDamage, AggregateType.AddRaw, Formula502, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ShieldStrengthenerLordEmperor, Stats.BonusDefenseWithShield, AggregateType.AddRaw, Formula803, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.UseScepterPetStr, Stats.ScepterPetBonusDamage, AggregateType.AddRaw, Formula632, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DarkSpiritStr2, Stats.RavenCriticalDamageChance, AggregateType.AddRaw, $"{Formula181} / 100", Formula181, 3, 3, SkillNumber.DarkSpiritStr);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ScepterMastery, Stats.ScepterMasteryBonusDamage, AggregateType.AddRaw, Formula1154, 3, 3, SkillNumber.ScepterStrengthener);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ShieldMastery, Stats.BonusDefenseRateWithShield, AggregateType.AddRaw, Formula1204, 3, 3, SkillNumber.ShieldStrengthenerLordEmperor);
        this.AddPassiveMasterSkillDefinition(SkillNumber.CommandAttackInc, Stats.BonusDamageWithScepterCmdDiv, AggregateType.AddRaw, $"1 / ({Formula3822})", Formula3822, 3, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DarkSpiritStr3, Stats.RavenExcDamageChance, AggregateType.AddRaw, $"{Formula120} / 100", Formula120, 5, 3, SkillNumber.DarkSpiritStr2);
        this.AddPassiveMasterSkillDefinition(SkillNumber.PetDurabilityStr, Stats.PetDurationIncrease, AggregateType.Multiplicate, Formula1204, 5, 3);

        // RF
        this.AddPassiveMasterSkillDefinition(SkillNumber.DurabilityReduction1FistMaster, Stats.ItemDurationIncrease, AggregateType.Multiplicate, Formula1204, 1, 1);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreasePvPDefenseRate, Stats.DefenseRatePvp, AggregateType.AddRaw, Formula25587, 1, 1);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseMaximumSd, Stats.MaximumShield, AggregateType.AddRaw, Formula30704, 2, 1);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseManaRecoveryRate, Stats.ManaRecoveryMultiplier, AggregateType.AddRaw, FormulaRecoveryIncrease181, Formula181, 2, 1, SkillNumber.Undefined, SkillNumber.Undefined, 20);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreasePoisonResistance, Stats.PoisonResistance, AggregateType.AddRaw, Formula120Value, Formula120, 2, 1);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DurabilityReduction2FistMaster, Stats.ItemDurationIncrease, AggregateType.Multiplicate, Formula1204, 3, 1, SkillNumber.DurabilityReduction1FistMaster);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseSdRecoveryRate, Stats.ShieldRecoveryMultiplier, AggregateType.AddRaw, FormulaRecoveryIncrease120, Formula120, 3, 1, SkillNumber.IncreaseMaximumSd, SkillNumber.Undefined, 20);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseHpRecoveryRate, Stats.HealthRecoveryMultiplier, AggregateType.AddRaw, FormulaRecoveryIncrease120, Formula120, 3, 1, SkillNumber.IncreaseManaRecoveryRate, SkillNumber.Undefined, 20);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseLightningResistance, Stats.LightningResistance, AggregateType.AddRaw, Formula120Value, Formula120, 3, 1, requiredSkill1: SkillNumber.IncreasePoisonResistance);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreasesDefense, Stats.DefenseBase, AggregateType.AddFinal, Formula3371, 4, 1);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreasesAgRecoveryRate, Stats.AbilityRecoveryMultiplier, AggregateType.AddRaw, FormulaRecoveryIncrease120, Formula120, 4, 1, SkillNumber.IncreaseHpRecoveryRate, SkillNumber.Undefined, 20);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseIceResistance, Stats.IceResistance, AggregateType.AddRaw, Formula120Value, Formula120, 4, 1, requiredSkill1: SkillNumber.IncreaseLightningResistance);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DurabilityReduction3FistMaster, Stats.ItemDurationIncrease, AggregateType.Multiplicate, Formula1204, 5, 1, SkillNumber.DurabilityReduction2FistMaster);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseDefenseSuccessRate, Stats.DefenseRatePvm, AggregateType.Multiplicate, FormulaIncreaseMultiplicator120, Formula120, 5, 1, SkillNumber.IncreasesDefense, SkillNumber.Undefined, 20);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseAttackSuccessRate, Stats.AttackRatePvm, AggregateType.AddRaw, Formula20469, 1, 2);
        this.AddMasterSkillDefinition(SkillNumber.KillingBlowStrengthener, SkillNumber.Undefined, SkillNumber.Undefined, 2, 2, SkillNumber.KillingBlow, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.BeastUppercutStrengthener, SkillNumber.Undefined, SkillNumber.Undefined, 2, 2, SkillNumber.BeastUppercut, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.KillingBlowMastery, SkillNumber.KillingBlowStrengthener, SkillNumber.Undefined, 2, 3, SkillNumber.KillingBlow, 20, Formula120);
        this.AddMasterSkillDefinition(SkillNumber.BeastUppercutMastery, SkillNumber.BeastUppercutStrengthener, SkillNumber.Undefined, 2, 3, SkillNumber.BeastUppercut, 20, Formula120);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseMaximumHp, Stats.MaximumHealth, AggregateType.AddRaw, Formula5418, 4, 2);
        this.AddPassiveMasterSkillDefinition(SkillNumber.WeaponMasteryFistMaster, Stats.MasterSkillPhysBonusDmg, AggregateType.AddRaw, Formula502, 4, 2);
        this.AddMasterSkillDefinition(SkillNumber.ChainDriveStrengthener, SkillNumber.ChainDrive, SkillNumber.Undefined, 2, 5, SkillNumber.ChainDrive, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.DarkSideStrengthener, SkillNumber.DarkSide, SkillNumber.Undefined, 2, 5, SkillNumber.DarkSide, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseMaximumMana, Stats.MaximumMana, AggregateType.AddRaw, Formula5418, 5, 2, SkillNumber.IncreaseMaximumHp);
        this.AddMasterSkillDefinition(SkillNumber.DragonRoarStrengthener, SkillNumber.DragonRoar, SkillNumber.Undefined, 2, 5, SkillNumber.DragonRoar, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreasePvPAttackRate, Stats.AttackRatePvp, AggregateType.AddRaw, Formula32751, 1, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.EquippedWeaponStrengthener, Stats.GloveWeaponBonusDamage, AggregateType.AddRaw, Formula502, 2, 3);
        this.AddMasterSkillDefinition(SkillNumber.DefSuccessRateIncPowUp, SkillNumber.IncreaseBlock, SkillNumber.Undefined, 3, 2, SkillNumber.IncreaseBlock, 20, Formula502);

        // todo: Increases the probability of Double Damage while using gloves according to the assigned Skill Level.
        this.AddMasterSkillDefinition(SkillNumber.EquippedWeaponMastery, SkillNumber.EquippedWeaponStrengthener, SkillNumber.Undefined, 3, 3, SkillNumber.Undefined, 20, Formula120);
        this.AddMasterSkillDefinition(SkillNumber.DefSuccessRateIncMastery, SkillNumber.DefSuccessRateIncPowUp, SkillNumber.Undefined, 3, 3, SkillNumber.IncreaseBlock, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.StaminaIncreaseStrengthener, SkillNumber.IncreaseHealth, SkillNumber.Undefined, 3, 4, SkillNumber.IncreaseHealth, 20, Formula1154);
        this.AddMasterSkillDefinition(SkillNumber.DecreaseMana, SkillNumber.Undefined, SkillNumber.Undefined, 3, 4, SkillNumber.Undefined, 20, Formula722);
        this.AddPassiveMasterSkillDefinition(SkillNumber.RecoverSDfromMonsterKills, Stats.ShieldAfterMonsterKillMultiplier, AggregateType.AddFinal, Formula914, 4, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.RecoverHPfromMonsterKills, Stats.HealthAfterMonsterKillMultiplier, AggregateType.AddFinal, Formula4319, 4, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseMinimumAttackPower, Stats.MinimumPhysBaseDmg, AggregateType.AddRaw, Formula502, 5, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.RecoverManaMonsterKills, Stats.ManaAfterMonsterKillMultiplier, AggregateType.AddFinal, Formula4319, 5, 3, SkillNumber.RecoverHPfromMonsterKills);
        */
    }

    private void AddPassiveMasterSkillDefinition(SkillNumber skillNumber, AttributeDefinition targetAttribute, AggregateType aggregateType, string valueFormula, string displayValueFormula, byte rank, byte root, SkillNumber requiredSkill1 = SkillNumber.Undefined, SkillNumber requiredSkill2 = SkillNumber.Undefined, byte maximumLevel = 20)
    {
        this.AddMasterSkillDefinition(skillNumber, requiredSkill1, requiredSkill2, root, rank, SkillNumber.Undefined, maximumLevel, valueFormula, displayValueFormula, targetAttribute, aggregateType);
    }

    private void AddPassiveMasterSkillDefinition(SkillNumber skillNumber, AttributeDefinition targetAttribute, AggregateType aggregateType, string valueFormula, byte rank, byte root, SkillNumber requiredSkill1 = SkillNumber.Undefined, SkillNumber requiredSkill2 = SkillNumber.Undefined, byte maximumLevel = 20)
    {
        this.AddMasterSkillDefinition(skillNumber, requiredSkill1, requiredSkill2, root, rank, SkillNumber.Undefined, maximumLevel, valueFormula, valueFormula, targetAttribute, aggregateType);
    }

    private void AddMasterSkillDefinition(SkillNumber skillNumber, SkillNumber requiredSkill1, SkillNumber requiredSkill2, byte root, byte rank, SkillNumber regularSkill, byte maximumLevel, string valueFormula, bool extendsDuration = false)
    {
        this.AddMasterSkillDefinition(skillNumber, requiredSkill1, requiredSkill2, root, rank, regularSkill, maximumLevel, valueFormula, valueFormula, null, AggregateType.AddRaw, extendsDuration);
    }

    private void AddMasterSkillDefinition(SkillNumber skillNumber, SkillNumber requiredSkill1, SkillNumber requiredSkill2, byte root, byte rank, SkillNumber regularSkill, byte maximumLevel, string valueFormula, string displayValueFormula, AttributeDefinition? targetAttribute, AggregateType aggregateType, bool extendsDuration = false)
    {
        var skill = this.GameConfiguration.Skills.First(s => s.Number == (short)skillNumber);
        skill.MasterDefinition = this.Context.CreateNew<MasterSkillDefinition>();
        skill.MasterDefinition.Rank = rank;
        skill.MasterDefinition.Root = this._masterSkillRoots[root];
        skill.MasterDefinition.ValueFormula = valueFormula;
        skill.MasterDefinition.DisplayValueFormula = displayValueFormula;
        skill.MasterDefinition.MaximumLevel = maximumLevel;
        skill.MasterDefinition.TargetAttribute = targetAttribute?.GetPersistent(this.GameConfiguration);
        skill.MasterDefinition.Aggregation = aggregateType;
        skill.MasterDefinition.ReplacedSkill = this.GameConfiguration.Skills.FirstOrDefault(s => s.Number == (short)regularSkill);
        skill.MasterDefinition.ExtendsDuration = extendsDuration;
        if (requiredSkill1 != SkillNumber.Undefined)
        {
            skill.MasterDefinition.RequiredMasterSkills.Add(this.GameConfiguration.Skills.First(s => s.Number == (short)requiredSkill1));
        }

        if (requiredSkill2 != SkillNumber.Undefined)
        {
            skill.MasterDefinition.RequiredMasterSkills.Add(this.GameConfiguration.Skills.First(s => s.Number == (short)requiredSkill2));
        }

        if (maximumLevel == 10 && valueFormula == Formula1WhenComplete)
        {
            skill.MasterDefinition.MinimumLevel = maximumLevel;
        }
        else
        {
            skill.MasterDefinition.MinimumLevel = 1;
        }

        var replacedSkill = skill.MasterDefinition.ReplacedSkill;
        if (replacedSkill != null)
        {
            // Because we don't want to duplicate code from the replaced skills to the master skills, we just assign some values from the replaced skill.
            // These describe the skill behavior.
            skill.AttackDamage = replacedSkill.AttackDamage;
            skill.DamageType = replacedSkill.DamageType;
            skill.ElementalModifierTarget = replacedSkill.ElementalModifierTarget;
            skill.ImplicitTargetRange = replacedSkill.ImplicitTargetRange;
            skill.MovesTarget = replacedSkill.MovesTarget;
            skill.MovesToTarget = replacedSkill.MovesToTarget;
            skill.SkillType = replacedSkill.SkillType;
            skill.Target = replacedSkill.Target;
            skill.TargetRestriction = replacedSkill.TargetRestriction;
            skill.MagicEffectDef = replacedSkill.MagicEffectDef;

            if (replacedSkill.AreaSkillSettings is { } areaSkillSettings)
            {
                skill.AreaSkillSettings = this.Context.CreateNew<AreaSkillSettings>();
                var id = skill.AreaSkillSettings.GetId();
                skill.AreaSkillSettings.AssignValuesOf(areaSkillSettings, this.GameConfiguration);
                skill.AreaSkillSettings.SetGuid(id);
            }
        }
    }

    private void CreateSpecialSummonMonsters()
    {
        {
            var monster = this.Context.CreateNew<MonsterDefinition>();
            this.GameConfiguration.Monsters.Add(monster);
            monster.Number = 150;
            monster.Designation = "Bali";
            monster.MoveRange = 3;
            monster.AttackRange = 1;
            monster.ViewRange = 7;
            monster.MoveDelay = new TimeSpan(400 * TimeSpan.TicksPerMillisecond);
            monster.AttackDelay = new TimeSpan(1600 * TimeSpan.TicksPerMillisecond);
            monster.RespawnDelay = new TimeSpan(100 * TimeSpan.TicksPerSecond);
            monster.Attribute = 2;
            monster.NumberOfMaximumItemDrops = 1;
            var attributes = new Dictionary<AttributeDefinition, float>
            {
                { Stats.Level, 52 },
                { Stats.MaximumHealth, 5000 },
                { Stats.MinimumPhysBaseDmg, 165 },
                { Stats.MaximumPhysBaseDmg, 170 },
                { Stats.DefenseBase, 100 },
                { Stats.AttackRatePvm, 260 },
                { Stats.DefenseRatePvm, 75 },
                { Stats.PoisonResistance, 6f / 255 },
                { Stats.IceResistance, 6f / 255 },
                { Stats.WaterResistance, 6f / 255 },
                { Stats.FireResistance, 6f / 255 },
            };

            monster.AddAttributes(attributes, this.Context, this.GameConfiguration);
            monster.SetGuid(monster.Number);
        }

        {
            var monster = this.Context.CreateNew<MonsterDefinition>();
            this.GameConfiguration.Monsters.Add(monster);
            monster.Number = 151;
            monster.Designation = "Soldier";
            monster.MoveRange = 3;
            monster.AttackRange = 4;
            monster.ViewRange = 7;
            monster.MoveDelay = new TimeSpan(400 * TimeSpan.TicksPerMillisecond);
            monster.AttackDelay = new TimeSpan(1600 * TimeSpan.TicksPerMillisecond);
            monster.RespawnDelay = new TimeSpan(100 * TimeSpan.TicksPerSecond);
            monster.Attribute = 2;
            monster.NumberOfMaximumItemDrops = 1;
            var attributes = new Dictionary<AttributeDefinition, float>
            {
                { Stats.Level, 58 },
                { Stats.MaximumHealth, 4000 },
                { Stats.MinimumPhysBaseDmg, 175 },
                { Stats.MaximumPhysBaseDmg, 180 },
                { Stats.DefenseBase, 110 },
                { Stats.AttackRatePvm, 290 },
                { Stats.DefenseRatePvm, 86 },
                { Stats.PoisonResistance, 6f / 255 },
                { Stats.IceResistance, 6f / 255 },
                { Stats.WaterResistance, 6f / 255 },
                { Stats.FireResistance, 6f / 255 },
            };

            monster.AddAttributes(attributes, this.Context, this.GameConfiguration);
            monster.SetGuid(monster.Number);
        }
    }
}
