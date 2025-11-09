// <copyright file="Scrolls.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086.Items;

using MUnique.OpenMU.DataModel.Configuration;
using MUnique.OpenMU.DataModel.Configuration.Items;
using MUnique.OpenMU.GameLogic.Attributes;
using MUnique.OpenMU.Persistence.Initialization.Version2086.CharacterClasses;
using MUnique.OpenMU.Persistence.Initialization.Version2086.Skills;

/// <summary>
/// Initializer for scroll items which allow a character to learn <see cref="Skill"/>s.
/// </summary>
/// <seealso cref="MUnique.OpenMU.Persistence.Initialization.InitializerBase" />
public class Scrolls : InitializerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Scrolls"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    public Scrolls(IContext context, GameConfiguration gameConfiguration)
        : base(context, gameConfiguration)
    {
    }

    /// <summary>
    /// Initializes the scroll data.
    /// </summary>
    /// <remarks>
    /// Regex: (?m)^\s*(\d+)\s+(-*\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+\"(.+?)\"\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+).*$
    /// Replace by: this.CreateScroll($1, TODO, $5, "$9", $10, $13, $14, $15, $16, $17, $18, $19, $20);.
    /// </remarks>
    public override void Initialize()
    {

        this.CreateScroll(12, 8, 1, 1, (int)SkillNumber.Heal, "Healing Orb", 8, 0, 52, 0, 0, 0, 800, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 9, 1, 1, (int)SkillNumber.GreaterDefense, "Orb of Greater Fortitude", 13, 0, 72, 0, 0, 0, 3000, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 10, 1, 1, (int)SkillNumber.GreaterDamage, "Orb of Greater Damage", 18, 0, 92, 0, 0, 0, 7000, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        var summonOrb = this.CreateScroll(12, 11, 1, 1, (int)SkillNumber.SummonGoblin, "Orb of Summoning", 3, 0, 30, 0, 0, 0, 150, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        summonOrb.MaximumItemLevel = 6;
        this.CreateScroll(12, 14, 1, 1, (int)SkillNumber.SwellLife, "Orb of Greater Fortitude", 60, 120, 0, 0, 0, 0, 43000, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 46, 1, 1, (int)SkillNumber.Recovery, "Crystal of Recovery", 100, 220, 168, 0, 0, 0, 250000, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 87, 1, 1, (int)SkillNumber.IllusionBlade, "Orb of Illusion Blade", 15, 25, 0, 0, 0, 0, 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);
        this.CreateScroll(12, 168, 1, 1, (int)SkillNumber.DexBooster, "Orb of Dex Booster", 90, 400, 0, 0, 500, 0, 850000, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 170, 1, 1, (int)SkillNumber.ElementalAttackPowerImprovement, "Orb of Elemental Attack Enhancement", 95, 400, 1500, 0, 0, 0, 850000, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 171, 1, 1, (int)SkillNumber.ElementalDefenseEnhancement, "Orb of Elemental Defense Enhancement", 95, 400, 1500, 0, 0, 0, 850000, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 478, 1, 1, (int)SkillNumber.Detection, "Orb of Detection", 30, 400, 0, 0, 800, 0, 400000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 482, 1, 1, (int)SkillNumber.Demolish, "Orb of Demolish", 400, 400, 0, 0, 1450, 0, 1600000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 485, 1, 1, (int)SkillNumber.SwordsFury, "Orb of Swordsman's Wrath", 78, 0, 0, 1060, 0, 0, 550000, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 487, 1, 1, (int)SkillNumber.StrongBelief, "Orb of Strong Belief", 78, 0, 1040, 0, 0, 0, 550000, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 488, 1, 1, (int)SkillNumber.SolidProtection, "Orb of Solid Protection", 78, 0, 1052, 0, 0, 0, 600000, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 54, 1, 1, (int)SkillNumber.Cure, "Orb of Basic Healing", 8, 0, 52, 0, 0, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateScroll(15, 55, 1, 1, (int)SkillNumber.BeginnersDefenseImprovement, "Orb of Basic Greater Fortitude", 13, 0, 72, 0, 0, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateScroll(15, 56, 1, 1, (int)SkillNumber.BeginnersAttackPowerEnhancement, "Orb of Basic Greater Damage", 18, 0, 92, 0, 0, 0, 7000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateScroll(15, 57, 1, 1, (int)SkillNumber.BeginnersRecovery, "Orb of Basic Recovery", 100, 220, 168, 0, 0, 0, 250000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0);
        this.CreateScroll(15, 58, 1, 1, (int)SkillNumber.BeginnersBless, "Orb of Basic Bless", 10, 0, 150, 0, 0, 0, 900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateScroll(15, 77, 1, 1, (int)SkillNumber.LugardsGuardianPunishment, "Lugard's Protection: Punishment Bead", 220, 220, 200, 0, 0, 0, 25000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);

        this.CreateScroll(12, 7, 1, 1, (int)SkillNumber.TwistingSlash, "Orb of Twisting Slash", 47, 80, 0, 0, 0, 0, 29000, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1);
        this.CreateScroll(12, 12, 1, 1, (int)SkillNumber.AngerBlow, "Orb of Anger Blow", 78, 170, 0, 0, 0, 0, 150000, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2);

        // this.CreateScroll(12, 13, 1, 1, (int)SkillNumber.Impale, "Orb of Impale", 20, 28, 0, 0, 0, 0, 10000, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 16, 1, 1, (int)SkillNumber.FireSlash, "Orb of Fire Slash", 60, 0, 0, 596, 0, 0, 51000, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 17, 1, 1, (int)SkillNumber.Penetration, "Orb of Penetration", 64, 130, 0, 0, 0, 0, 72000, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 18, 1, 1, (int)SkillNumber.IceArrow, "Orb of Ice Arrow", 81, 0, 0, 0, 646, 0, 195000, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 19, 1, 1, (int)SkillNumber.DeathStab, "Orb of Death Stab", 72, 160, 0, 0, 0, 0, 85000, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 21, 1, 2, (int)SkillNumber.FireBurst, "Scroll of FireBurst", 74, 0, 79, 0, 0, 0, 115000, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 22, 1, 2, (int)SkillNumber.Summon, "Scroll of Summon", 98, 0, 153, 0, 0, 400, 375000, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 23, 1, 2, (int)SkillNumber.LordDignity, "Scroll of Lord Dignity", 82, 0, 102, 0, 0, 300, 220000, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 24, 1, 2, (int)SkillNumber.ElectricSpark, "Scroll of Electric Spark", 92, 0, 126, 0, 0, 340, 295000, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 35, 1, 2, (int)SkillNumber.FireScream, "Scroll of Fire Scream", 102, 0, 150, 0, 0, 70, 300000, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        var strikeOfDestruction = this.CreateScroll(12, 44, 1, 1, (int)SkillNumber.StrikeofDestruction, "Crystal of Destruction", 100, 220, 0, 0, 0, 0, 380000, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);
        this.CreateItemRequirementIfNeeded(strikeOfDestruction, Stats.GainHeroStatusQuestCompleted, 1);
        this.CreateScroll(12, 45, 1, 1, (int)SkillNumber.MultiShot, "Crystal of Multi-Shot", 100, 220, 0, 0, 0, 0, 380000, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 47, 1, 1, (int)SkillNumber.FlameStrike, "Crystal of Flame Strike", 100, 220, 0, 0, 0, 0, 380000, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 48, 1, 2, (int)SkillNumber.ChaoticDiseier, "Scroll of Chaotic Diseier", 100, 220, 84, 0, 0, 0, 380000, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 88, 1, 1, (int)SkillNumber.IllusionAvatar, "Orb of Illusion Avatar", 90, 0, 0, 0, 900, 0, 15000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0);
        this.CreateScroll(12, 89, 1, 1, (int)SkillNumber.ChargeSlash, "Orb of Charging Slash", 50, 0, 0, 0, 80, 0, 17000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);
        this.CreateScroll(12, 90, 1, 1, (int)SkillNumber.WindGlaive, "Orb of Wind Glaive", 80, 0, 0, 0, 650, 0, 45000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);
        this.CreateScroll(12, 91, 1, 1, (int)SkillNumber.BladeStorm, "Orb of Blade Storm", 100, 400, 0, 0, 1150, 0, 900000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateScroll(12, 95, 1, 2, (int)SkillNumber.Oversting, "Oversting Scroll", 100, 0, 0, 0, 1470, 0, 500000, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 98, 1, 2, (int)SkillNumber.WildBreath, "Wild Breath Scroll", 100, 0, 0, 1020, 0, 0, 500000, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 162, 1, 1, (int)SkillNumber.ChaosBlade, "Orb of Chaos Blade", 150, 400, 0, 900, 0, 0, 900000, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 163, 1, 2, (int)SkillNumber.HavokSpear, "Scroll of Havok Spear", 150, 400, 1073, 0, 0, 0, 900000, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 164, 1, 1, (int)SkillNumber.SpiralCharge, "Orb of  Spiral Charge", 150, 400, 0, 900, 0, 0, 550000, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 165, 1, 1, (int)SkillNumber.CrusherCharge, "Orb of Crusher Charge", 150, 400, 0, 900, 0, 0, 550000, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 166, 1, 2, (int)SkillNumber.ElementalCharge, "Scroll of Elemental Charge", 150, 400, 1073, 0, 0, 0, 550000, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 167, 1, 1, (int)SkillNumber.RainingArrow, "Orb of Raining Arrow", 100, 400, 0, 0, 1302, 0, 900000, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 169, 1, 1, (int)SkillNumber.HolyBolt, "Orb of Holy Bolt", 100, 400, 1200, 0, 0, 0, 900000, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 271, 1, 2, (int)SkillNumber.SpinStep, "Spin Step Scroll", 53, 0, 0, 0, 150, 0, 100000, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 272, 1, 2, (int)SkillNumber.Obsidian, "Obsidian Scroll", 74, 0, 200, 0, 0, 0, 120000, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 273, 1, 2, (int)SkillNumber.MagicPin, "Magic Pin Scroll", 80, 0, 0, 200, 200, 0, 200000, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 274, 1, 2, (int)SkillNumber.HarshStrike, "Harsh Strike Scroll", 58, 0, 0, 150, 0, 0, 100000, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 275, 1, 2, (int)SkillNumber.ShiningPeak, "Shining Peak Scroll", 92, 0, 0, 600, 0, 0, 300000, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 276, 1, 2, (int)SkillNumber.Wrath, "Wrath Scroll", 66, 0, 0, 200, 200, 0, 120000, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 277, 1, 2, (int)SkillNumber.Breche, "Breche Scroll", 92, 0, 0, 300, 300, 0, 200000, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 289, 1, 1, (int)SkillNumber.FireBlow, "Orb of Fire Blow", 150, 400, 0, 1090, 0, 0, 900000, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 290, 1, 2, (int)SkillNumber.MeteorStrike, "Scroll of Meteor Strike", 150, 400, 1073, 0, 0, 0, 900000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 291, 1, 2, (int)SkillNumber.MeteorStorm, "Scroll of Meteor Storm", 150, 400, 1160, 0, 0, 0, 900000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 292, 1, 2, (int)SkillNumber.SoulSeeker, "Scroll of Soul Seeker", 150, 400, 1115, 0, 0, 0, 900000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 293, 1, 1, (int)SkillNumber.FocusShot, "Orb of Focus Shot", 150, 400, 0, 0, 1302, 0, 900000, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 295, 1, 2, (int)SkillNumber.FireBeast, "Parchment of Fire Beast", 150, 400, 1220, 0, 0, 0, 900000, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 296, 1, 2, (int)SkillNumber.AquaBeast, "Parchment of Aqua Beast", 150, 400, 1220, 0, 0, 0, 900000, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 297, 1, 1, (int)SkillNumber.IceBlood, "Orb of Ice Blood", 150, 400, 0, 900, 0, 0, 900000, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 298, 1, 1, (int)SkillNumber.FireBlood, "Orb of Fire Blood", 150, 400, 0, 900, 0, 0, 900000, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 299, 1, 2, (int)SkillNumber.DarkBlast, "Scroll of Dark Blast", 150, 400, 1073, 0, 0, 0, 900000, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 300, 1, 2, (int)SkillNumber.MeteorStrike, "Scroll of Meteor Strike", 150, 400, 1073, 0, 0, 0, 900000, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 303, 1, 2, (int)SkillNumber.WindSoul, "Scroll of Wind Soul", 150, 400, 0, 717, 0, 0, 900000, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 305, 1, 2, (int)SkillNumber.DarkPhoenixShot, "Parchment of Dark Phoenix Shot", 150, 400, 0, 0, 987, 0, 900000, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 413, 1, 2, (int)SkillNumber.SpiritHook, "Spirit Hook Parchment", 150, 400, 0, 0, 0, 0, 900000, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 476, 1, 1, (int)SkillNumber.SwordInertia, "Orb of Sword Inertia", 10, 30, 0, 50, 100, 0, 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 477, 1, 1, (int)SkillNumber.BatFlock, "Orb of Bat Flock", 20, 270, 0, 100, 380, 0, 200000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 479, 1, 1, (int)SkillNumber.PierceAttack, "Orb of Pierce Attack", 160, 400, 0, 300, 1100, 0, 850000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 486, 1, 1, (int)SkillNumber.SwordBlow, "Orb of Sword Blow", 78, 0, 0, 1090, 0, 0, 150000, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 491, 1, 1, (int)SkillNumber.DarkPlasma, "Orb of Dark Plasma", 100, 150, 0, 0, 0, 0, 550000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 492, 1, 1, (int)SkillNumber.IceBreak, "Orb of Ice Break", 80, 0, 295, 0, 0, 0, 150000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 493, 1, 1, (int)SkillNumber.IceBlast, "Orb of Ice Blast", 150, 0, 1000, 0, 0, 0, 550000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 494, 1, 1, (int)SkillNumber.DeathFire, "Orb of Death Fire", 80, 0, 100, 0, 0, 0, 600000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 495, 1, 1, (int)SkillNumber.BurstingFlare, "Orb of Bursting Flare", 150, 0, 800, 100, 200, 0, 600000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateScroll(12, 498, 1, 1, (int)SkillNumber.DeathIce, "Orb of Death Ice", 80, 0, 100, 0, 0, 0, 600000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 0, 1, 2, (int)SkillNumber.Poison, "Scroll of Poison", 30, 0, 140, 0, 0, 0, 17000, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0);
        this.CreateScroll(15, 1, 1, 2, (int)SkillNumber.Meteorite, "Scroll of Meteorite", 21, 0, 104, 0, 0, 0, 11000, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0);
        this.CreateScroll(15, 2, 1, 2, (int)SkillNumber.Lightning, "Scroll of Lighting", 13, 0, 72, 0, 0, 0, 3000, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0);
        this.CreateScroll(15, 3, 1, 2, (int)SkillNumber.FireBall, "Scroll of Fire Ball", 5, 0, 40, 0, 0, 0, 300, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0);
        this.CreateScroll(15, 4, 1, 2, (int)SkillNumber.Flame, "Scroll of Flame", 35, 0, 160, 0, 0, 0, 21000, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0);
        this.CreateScroll(15, 5, 1, 2, (int)SkillNumber.Teleport, "Scroll of Teleport", 17, 0, 88, 0, 0, 0, 5000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateScroll(15, 6, 1, 2, (int)SkillNumber.Ice, "Scroll of Ice", 25, 0, 120, 0, 0, 0, 14000, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0);
        this.CreateScroll(15, 7, 1, 2, (int)SkillNumber.Twister, "Scroll of Twister", 40, 0, 180, 0, 0, 0, 25000, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 1, 0);
        this.CreateScroll(15, 8, 1, 2, (int)SkillNumber.EvilSpirit, "Scroll of Evil Spirit", 50, 0, 220, 0, 0, 0, 35000, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 9, 1, 2, (int)SkillNumber.Hellfire, "Scroll of Hellfire", 60, 0, 260, 0, 0, 0, 60000, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0);
        this.CreateScroll(15, 10, 1, 2, (int)SkillNumber.PowerWave, "Scroll of Power Wave", 9, 0, 56, 0, 0, 0, 1150, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0);
        this.CreateScroll(15, 11, 1, 2, (int)SkillNumber.AquaBeam, "Scroll of Aqua Beam", 74, 0, 345, 0, 0, 0, 100000, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0);
        this.CreateScroll(15, 12, 1, 2, (int)SkillNumber.Cometfall, "Scroll of Cometfall", 80, 0, 500, 0, 0, 0, 175000, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0);
        this.CreateScroll(15, 13, 1, 2, (int)SkillNumber.Inferno, "Scroll of Inferno", 88, 0, 724, 0, 0, 0, 265000, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0);
        this.CreateScroll(15, 14, 1, 2, (int)SkillNumber.TeleportAlly, "Scroll of Teleport Ally", 83, 0, 644, 0, 0, 0, 245000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 2, 0);
        this.CreateScroll(15, 15, 1, 2, (int)SkillNumber.SoulBarrier, "Scroll of Soul Barrier", 77, 0, 408, 0, 0, 0, 135000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 16, 1, 2, (int)SkillNumber.Decay, "Scroll of Decay", 96, 0, 953, 0, 0, 0, 345000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0);
        this.CreateScroll(15, 17, 1, 2, (int)SkillNumber.IceStorm, "Scroll of Ice Storm", 93, 0, 849, 0, 0, 0, 315000, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2, 0, 2, 0);
        this.CreateScroll(15, 18, 1, 2, (int)SkillNumber.Nova, "Scroll of Nova", 100, 0, 1052, 0, 0, 0, 410000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0);
        this.CreateScroll(15, 19, 1, 2, (int)SkillNumber.ChainLightning, "Chain Lightning Parchment", 75, 0, 245, 0, 0, 0, 175000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 20, 1, 2, (int)SkillNumber.DrainLife, "Drain Life Parchment", 35, 0, 150, 0, 0, 0, 100000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 21, 1, 2, (int)SkillNumber.LightningShock, "Lightning Shock Parchment", 93, 0, 823, 0, 0, 0, 315000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 22, 1, 2, (int)SkillNumber.DamageReflection, "Damage Reflection Parchment", 80, 0, 375, 0, 0, 0, 245000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 23, 1, 2, (int)SkillNumber.Berserker, "Berserker Parchment", 83, 0, 300, 0, 0, 0, 265000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 24, 1, 2, (int)SkillNumber.Sleep, "Sleep Parchment", 40, 0, 180, 0, 0, 0, 135000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 26, 1, 2, (int)SkillNumber.Weakness, "Weakness Parchment", 93, 0, 663, 0, 0, 0, 410000, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 27, 1, 2, (int)SkillNumber.Innovation, "Innovation Parchment", 111, 0, 912, 0, 0, 0, 450000, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 28, 1, 2, (int)SkillNumber.ExpansionofWizardry, "Scroll of Wizardry Enhance", 100, 220, 1058, 0, 0, 0, 425000, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 2, 0, 2, 0);
        this.CreateScroll(15, 29, 1, 2, (int)SkillNumber.GiganticStorm, "Scroll of Gigantic Storm", 100, 220, 1058, 0, 0, 0, 380000, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 30, 1, 2, (int)SkillNumber.ChainDrive, "Chain Drive Parchment", 70, 150, 0, 0, 0, 0, 100000, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 31, 1, 2, (int)SkillNumber.DarkSide, "Dark Side Parchment", 80, 180, 0, 0, 0, 0, 175000, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 32, 1, 2, (int)SkillNumber.DragonRoar, "Dragon Roar Parchment", 70, 150, 0, 0, 0, 0, 100000, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 33, 1, 2, (int)SkillNumber.DragonSlasher, "Dragon Slasher Parchment", 90, 200, 0, 0, 0, 0, 265000, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 34, 1, 2, (int)SkillNumber.IgnoreDefense, "Ignore Defense Parchment", 80, 120, 404, 0, 0, 0, 175000, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 35, 1, 2, (int)SkillNumber.IncreaseHealth, "Increase Health Parchment", 50, 80, 132, 0, 0, 0, 35000, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 36, 1, 2, (int)SkillNumber.IncreaseBlock, "Increase Block Parchment", 40, 50, 80, 0, 0, 0, 25000, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 37, 1, 2, (int)SkillNumber.MagicArrow, "Scroll of Magic Arrow", 50, 0, 60, 0, 0, 0, 17000, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 38, 1, 2, (int)SkillNumber.PlasmaBall, "Scroll of Plasma Ball", 80, 0, 300, 0, 0, 0, 120000, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 39, 1, 2, (int)SkillNumber.LightningStorm, "Scroll of Lightning Storm", 100, 0, 1080, 0, 0, 0, 900000, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 40, 1, 2, (int)SkillNumber.Burst, "Scroll of Burst", 90, 220, 0, 0, 0, 0, 35000, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 41, 1, 2, (int)SkillNumber.Haste, "Scroll of Haste", 180, 400, 0, 0, 0, 0, 40000, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 42, 1, 2, (int)SkillNumber.Explosion, "Explosion Parchment", 75, 0, 300, 0, 0, 0, 175000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 43, 1, 2, (int)SkillNumber.Requiem, "Requiem Parchment", 75, 0, 416, 0, 0, 0, 175000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 44, 1, 2, (int)SkillNumber.Pollution, "Pollution Parchment", 75, 0, 542, 0, 0, 0, 175000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 45, 1, 2, (int)SkillNumber.DeathScythe, "Death Scythe Parchment", 75, 0, 930, 0, 0, 0, 175000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 46, 1, 2, (int)SkillNumber.Darkness, "Darkness Parchment", 75, 0, 300, 0, 0, 0, 175000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 47, 1, 2, (int)SkillNumber.ShiningBird, "Scroll of Shining Bird", 50, 0, 70, 0, 0, 0, 17000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateScroll(15, 48, 1, 2, (int)SkillNumber.DragonViolent, "Scroll of Violent Dragon", 80, 0, 680, 0, 0, 0, 40000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0);
        this.CreateScroll(15, 49, 1, 2, (int)SkillNumber.SpearStorm, "Scroll of Spearstorm", 100, 400, 1160, 0, 0, 0, 900000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateScroll(15, 50, 1, 2, (int)SkillNumber.ReflectionBarrier, "Scroll of Reflective Barrier", 77, 0, 410, 0, 0, 0, 135000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateScroll(15, 51, 1, 2, (int)SkillNumber.MarvelBurst, "Scroll of Marble Burst", 50, 0, 104, 0, 0, 0, 17000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateScroll(15, 52, 1, 2, (int)SkillNumber.UnleashMarvel, "Scroll of Unleash Marble", 80, 0, 700, 0, 0, 0, 45000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0);
        this.CreateScroll(15, 53, 1, 2, (int)SkillNumber.UltimateForce, "Scroll of Ultimate Force", 100, 400, 1073, 0, 0, 0, 900000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateScroll(15, 59, 1, 1, (int)SkillNumber.FixedFire, "Orb of Fixed Fire", 75, 0, 300, 0, 0, 0, 175000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 63, 1, 2, (int)SkillNumber.SoftnessConfusionStone, "Transmutation: Scroll of Confusion Stone", 50, 380, 850, 0, 0, 0, 17000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateScroll(15, 64, 1, 2, (int)SkillNumber.SoftnessAngelHomunculus, "Transmutation: Scroll of Angel Homunculus", 50, 0, 70, 0, 0, 0, 17000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateScroll(15, 65, 1, 2, (int)SkillNumber.FlexibilityIgnitionBomber, "Transmutation: Scroll of Ignition Bomber", 80, 0, 680, 0, 0, 0, 40000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0);
        this.CreateScroll(15, 66, 1, 2, (int)SkillNumber.SoftnessCountlessWeapon, "Transmutation: Scroll of Countless Weapon", 100, 400, 1025, 0, 0, 0, 900000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateScroll(15, 67, 1, 2, (int)SkillNumber.SpiritBlast, "Scroll of Spirit Blast", 150, 0, 0, 800, 0, 620, 900000, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 68, 1, 2, (int)SkillNumber.CrownForce, "Scroll of Crown Force", 150, 220, 300, 0, 0, 400, 900000, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 69, 1, 2, (int)SkillNumber.BattleGlory, "Scroll of Battle Glory", 150, 0, 0, 200, 0, 200, 900000, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 70, 1, 2, (int)SkillNumber.RunicSpear, "Runic Spear Law Book", 80, 0, 600, 0, 0, 0, 50000, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 71, 1, 2, (int)SkillNumber.RunePhrase, "Rune Phrase Book", 100, 400, 1180, 0, 0, 0, 900000, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateScroll(15, 72, 1, 1, (int)SkillNumber.HolySweep, "Holy Sweep Beads", 100, 0, 0, 420, 168, 0, 15000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);
        this.CreateScroll(15, 73, 1, 1, (int)SkillNumber.DivineFall, "Divine Fall Beads", 30, 0, 0, 60, 29, 0, 50000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
        this.CreateScroll(15, 74, 1, 1, (int)SkillNumber.SacredImpact, "Sacred Impact Beads", 160, 0, 0, 1045, 418, 0, 80000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
    }

    private ItemDefinition CreateScroll(byte group, short number, byte width, byte height, int skillNumber, string name, short dropLevel, int levelRequirement, int energyRequirement, int strengthRequirement, int agilityRequirement, int leadershipRequirement, int money, int wizardClass, int knightClass, int elfClass, int magicGladiatorClass, int darkLordClass, int summonerClass, int ragefighterClass, int growLancerClass, int runeWizardClass, int slayerClass, int gunCrusherClass, int whiteWizardClass, int lemuriaClass, int illusionKnightClass, int alchemistClass, int crusaderClass)
    {
        var scroll = this.Context.CreateNew<ItemDefinition>();
        this.GameConfiguration.Items.Add(scroll);
        scroll.Group = group;
        scroll.Number = number;
        scroll.Skill = this.GameConfiguration.Skills.First(skill => skill.Number == skillNumber);
        scroll.Width = width;
        scroll.Height = height;
        scroll.Name = name;
        scroll.DropLevel = dropLevel;
        scroll.DropsFromMonsters = true;
        scroll.Durability = 1;

        this.CreateItemRequirementIfNeeded(scroll, Stats.Level, levelRequirement);
        this.CreateItemRequirementIfNeeded(scroll, Stats.TotalEnergy, energyRequirement);
        this.CreateItemRequirementIfNeeded(scroll, Stats.TotalStrength, strengthRequirement);
        this.CreateItemRequirementIfNeeded(scroll, Stats.TotalAgility, agilityRequirement);
        this.CreateItemRequirementIfNeeded(scroll, Stats.TotalLeadership, leadershipRequirement);

        scroll.Value = money;
        scroll.SetGuid(scroll.Group, scroll.Number);
        var classes = this.GameConfiguration.DetermineCharacterClasses(false, wizardClass, knightClass, elfClass, magicGladiatorClass, darkLordClass, summonerClass, ragefighterClass, growLancerClass, runeWizardClass, slayerClass, gunCrusherClass, whiteWizardClass, lemuriaClass, illusionKnightClass, alchemistClass, crusaderClass);
        foreach (var characterClass in classes)
        {
            scroll.QualifiedCharacters.Add(characterClass);
        }
        return scroll;
    }
}