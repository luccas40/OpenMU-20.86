// <copyright file="Weapons.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086.Items;

using MUnique.OpenMU.AttributeSystem;
using MUnique.OpenMU.DataModel.Configuration;
using MUnique.OpenMU.DataModel.Configuration.Items;
using MUnique.OpenMU.GameLogic.Attributes;
using MUnique.OpenMU.Persistence.Initialization.CharacterClasses;
using MUnique.OpenMU.Persistence.Initialization.Items;
using MUnique.OpenMU.Persistence.Initialization.Skills;
using MUnique.OpenMU.Persistence.Initialization.Version2086.CharacterClasses;

/// <summary>
/// Helper class to create weapon item definitions.
/// </summary>
internal class Weapons : InitializerBase
{
    /// <summary>
    /// The maximum item level for weapons and armors.
    /// </summary>
    protected const int MaximumItemLevel = 15;

    /// <summary>
    /// The durability increase per level.
    /// </summary>
    protected static readonly float[] DurabilityIncreasePerLevel = { 0, 1, 2, 3, 4, 6, 8, 10, 12, 14, 17, 21, 26, 32, 39, 47 };

    /// <summary>
    /// The weapon damage increase by level.
    /// </summary>
    private static readonly float[] DamageIncreaseByLevel = { 0, 3, 6, 9, 12, 15, 18, 21, 24, 27, 31, 36, 42, 49, 57, 66 };

    private static readonly float[] StaffRiseIncreaseByLevelEven = { 0, 3, 7, 10, 14, 17, 21, 24, 28, 31, 35, 40, 45, 50, 56, 63 }; // Staff/stick with even magic power
    private static readonly float[] StaffRiseIncreaseByLevelOdd = { 0, 4, 7, 11, 14, 18, 21, 25, 28, 32, 36, 40, 45, 51, 57, 63 }; // Staff/stick with odd magic power

    private static readonly float[] ScepterRiseIncreaseByLevelEven = { 0, 1, 3, 4, 6, 7, 9, 10, 12, 13, 15, 18, 21, 24, 28, 33 }; // Scepter with even magic power
    private static readonly float[] ScepterRiseIncreaseByLevelOdd = { 0, 2, 3, 5, 6, 8, 9, 11, 12, 14, 16, 18, 21, 25, 29, 33 }; // Scepter with odd magic power

    private static readonly float[] AmmunitionDamageIncreaseByLevel = { 0, 0.03f, 0.05f, 0.07f }; // Bolts/Arrows
    private static readonly float[] AmmunitionManaLossAfterHitByLevel = { 5, 7, 10, 15 }; // Only if Infinity Arrow effect is active

    private ItemLevelBonusTable? _weaponDamageIncreaseTable;

    private ItemLevelBonusTable? _staffRiseTableEven;
    private ItemLevelBonusTable? _staffRiseTableOdd;

    private ItemLevelBonusTable? _scepterRiseTableEven;
    private ItemLevelBonusTable? _scepterRiseTableOdd;

    private ItemLevelBonusTable? _ammunitionDamageIncreaseTable;
    private ItemLevelBonusTable? _ammunitionManaLossAfterHitTable;

    /// <summary>
    /// Initializes a new instance of the <see cref="Weapons" /> class.
    /// </summary>
    /// <param name="context">The persistence context.</param>
    /// <param name="gameConfiguration">The game configration.</param>
    public Weapons(IContext context, GameConfiguration gameConfiguration)
        : base(context, gameConfiguration)
    {
    }

    /// <summary>
    /// Gets the luck item option definition.
    /// </summary>
    protected ItemOptionDefinition Luck
    {
        get
        {
            return this.GameConfiguration.ItemOptions.First(iod => iod.PossibleOptions.Any(o => o.OptionType == ItemOptionTypes.Luck));
        }
    }

    /// <summary>
    /// Gets the physical damage option definition.
    /// </summary>
    protected ItemOptionDefinition PhysicalDamageOption
    {
        get
        {
            return this.GameConfiguration.ItemOptions.First(iod => iod.PossibleOptions.Any(o => o.OptionType == ItemOptionTypes.Option && o.PowerUpDefinition?.TargetAttribute == Stats.PhysicalBaseDmg));
        }
    }

    /// <summary>
    /// Gets the wizardry damage option definition.
    /// </summary>
    protected ItemOptionDefinition WizardryDamageOption
    {
        get
        {
            return this.GameConfiguration.ItemOptions.First(iod => iod.PossibleOptions.Any(o => o.OptionType == ItemOptionTypes.Option && o.PowerUpDefinition?.TargetAttribute == Stats.WizardryBaseDmg));
        }
    }

    /// <summary>
    /// Gets the curse damage option definition.
    /// </summary>
    protected ItemOptionDefinition CurseDamageOption
    {
        get
        {
            return this.GameConfiguration.ItemOptions.First(iod => iod.PossibleOptions.Any(o => o.OptionType == ItemOptionTypes.Option && o.PowerUpDefinition?.TargetAttribute == Stats.CurseBaseDmg));
        }
    }

    /// <summary>
    /// Gets the physical and wizardry damage option definition.
    /// </summary>
    protected ItemOptionDefinition PhysicalAndWizardryDamageOption
    {
        get
        {
            return this.GameConfiguration.ItemOptions.First(iod => iod.PossibleOptions.Any(o => o.OptionType == ItemOptionTypes.Option && o.PowerUpDefinition?.TargetAttribute == Stats.BaseDamageBonus));
        }
    }

    /// <summary>
    /// Initializes the weapons.
    /// </summary>
    /// <remarks>
    ///   Regex: (?m)^\s*(\d+)\s+(-*\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+\"(.+?)\"\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+).*$
    /// Replace: this.CreateItem(0, $1, $2, $3, $4, $5, $8 == 1, "$9", $10, $11, $12, $13, $14, $16, $17, $18, $19, $20, $21, $24, $25, $26, $27, $28, $29, $30);.
    /// </remarks>
    public override void Initialize()
    {
        this._weaponDamageIncreaseTable = this.CreateItemBonusTable(DamageIncreaseByLevel, "Damage Increase (Weapons)", "The damage increase by weapon level. It increases by 3 per level, and 1 more after level 10.");
        this._staffRiseTableEven = this.CreateItemBonusTable(StaffRiseIncreaseByLevelEven, "Staff Rise (even)", "The staff rise bonus per item level for even magic power staves.");
        this._staffRiseTableOdd = this.CreateItemBonusTable(StaffRiseIncreaseByLevelOdd, "Staff Rise (odd)", "The staff rise bonus per item level for odd magic power staves.");
        this._scepterRiseTableEven = this.CreateItemBonusTable(ScepterRiseIncreaseByLevelEven, "Scepter Rise (even)", "The scepter rise bonus per item level for even magic power scepters.");
        this._scepterRiseTableOdd = this.CreateItemBonusTable(ScepterRiseIncreaseByLevelOdd, "Scepter Rise (odd)", "The scepter rise bonus per item level for odd magic power scepters.");
        this._ammunitionDamageIncreaseTable = this.CreateItemBonusTable(AmmunitionDamageIncreaseByLevel, "Damage Increase % (Bolts/Arrows)", "The damage increase % per ammunition item level.");
        this._ammunitionManaLossAfterHitTable = this.CreateItemBonusTable(AmmunitionManaLossAfterHitByLevel, "Mana Loss After Hit (Bolts/Arrows)", "The mana loss per skill hit per ammunition item level due to infinity arrow efect.");

        this.CreateWeapon(0, 0, 0, 0, 1, 2, true, "Kris", 6, 6, 11, 50, 20, 0, 0, 0, 40, 40, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1);
        this.CreateWeapon(0, 1, 0, 0, 1, 3, true, "Short Sword", 3, 3, 7, 20, 22, 0, 0, 0, 60, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1);
        this.CreateWeapon(0, 2, 0, 0, 1, 3, true, "Rapier", 9, 9, 15, 40, 23, 0, 0, 0, 50, 40, 0, 0, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 3, 0, 20, 1, 3, true, "Katana", 16, 16, 26, 35, 27, 0, 0, 0, 80, 40, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 4, 0, 21, 1, 3, true, "Sword of Assassin", 12, 12, 18, 30, 24, 0, 0, 0, 60, 40, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 5, 0, 22, 1, 3, true, "Blade", 36, 36, 47, 30, 39, 0, 0, 0, 80, 50, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 0);
        this.CreateWeapon(0, 6, 0, 20, 1, 3, true, "Gladius", 20, 20, 30, 20, 30, 0, 0, 0, 110, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 7, 0, 21, 1, 3, true, "Falchion", 24, 24, 34, 25, 34, 0, 0, 0, 120, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 8, 0, 21, 1, 3, true, "Serpent Sword", 30, 30, 40, 20, 36, 0, 0, 0, 130, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 9, 0, 20, 2, 3, true, "Sword of Salamander", 32, 32, 46, 30, 40, 0, 0, 0, 103, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 10, 0, 22, 2, 4, true, "Light Saber", 40, 47, 61, 25, 50, 0, 0, 0, 80, 60, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 11, 0, 20, 2, 3, true, "Legendary Sword", 44, 56, 72, 20, 54, 0, 0, 0, 120, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 12, 0, 19, 2, 3, true, "Heliacal Sword", 56, 73, 98, 25, 66, 0, 0, 0, 140, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 13, 0, 22, 1, 3, true, "Double Blade", 48, 48, 56, 30, 43, 0, 0, 0, 70, 70, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0);
        this.CreateWeapon(0, 14, 0, 22, 1, 3, true, "Lightning Sword", 59, 59, 67, 30, 50, 0, 0, 0, 90, 50, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 15, 0, 23, 2, 3, true, "Giant Sword", 52, 60, 85, 20, 60, 0, 0, 0, 140, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 16, 0, 22, 1, 4, true, "Sword of Destruction", 82, 82, 90, 35, 84, 0, 0, 0, 160, 60, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 17, 0, 23, 2, 4, true, "Dark Breaker", 104, 128, 153, 40, 89, 0, 0, 0, 180, 50, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 18, 0, 23, 2, 3, true, "Thunder Blade", 105, 140, 168, 40, 86, 0, 0, 0, 180, 50, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 19, 0, 22, 1, 4, true, "Divine Sword of the Archangel", 86, 220, 230, 45, 168, 0, 38, 0, 140, 50, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 20, 0, 22, 1, 4, true, "Knight Blade", 140, 107, 115, 35, 90, 0, 0, 0, 116, 38, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 21, 0, 56, 2, 4, true, "Dark Reign Blade", 140, 115, 142, 40, 100, 115, 0, 0, 116, 53, 9, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 22, 0, 22, 1, 4, true, "Bone Blade", 147, 122, 135, 40, 95, 0, 0, 380, 100, 35, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 23, 0, 56, 2, 4, true, "Explosion Blade", 147, 127, 155, 45, 110, 134, 0, 380, 98, 48, 7, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 24, 0, 22, 2, 4, true, "Daybreak", 115, 182, 218, 40, 90, 0, 0, 0, 192, 30, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 25, 0, 56, 2, 4, true, "Sword Dancer", 115, 109, 136, 40, 90, 108, 0, 0, 136, 57, 9, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 26, 0, 22, 1, 4, true, "Flameberge", 137, 115, 126, 40, 90, 0, 34, 380, 193, 53, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 27, 0, 22, 1, 4, true, "Sword Breaker", 133, 91, 99, 35, 90, 0, 30, 380, 53, 176, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 28, 0, 56, 1, 4, true, "Rune Bastard Sword", 139, 98, 122, 45, 90, 109, 30, 380, 91, 73, 17, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 29, 0, 22, 1, 4, true, "Sonic Blade", 149, 109, 116, 35, 80, 0, 42, 400, 49, 162, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 30, 0, 56, 1, 4, true, "Asura", 149, 115, 140, 45, 90, 112, 40, 400, 86, 70, 16, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 31, 0, 56, 2, 4, true, "Rune Blade", 100, 104, 130, 35, 93, 104, 0, 0, 135, 62, 9, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 32, 0, 260, 1, 2, true, "Sacred Glove", 52, 52, 58, 25, 65, 0, 0, 0, 85, 35, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 33, 0, 261, 1, 2, true, "Holy Storm Claws", 82, 82, 88, 30, 77, 0, 0, 0, 100, 50, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 34, 0, 260, 1, 2, true, "Piercing Blade Glove", 105, 95, 101, 35, 86, 0, 0, 0, 120, 60, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 35, 0, 270, 1, 2, true, "Phoenix Soul Star", 147, 122, 128, 40, 98, 0, 0, 380, 101, 51, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 36, 0, 22, 1, 4, true, "Cyclone Sword", 149, 129, 144, 40, 90, 0, 46, 400, 182, 50, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 37, 0, 23, 2, 4, true, "Blast Break", 149, 163, 216, 30, 130, 0, 55, 400, 182, 50, 0, 0, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 41, 0, 0, 2, 2, true, "Pandora Pick (Two-Handed)", 0, 0, 0, 0, 255, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
        this.CreateWeapon(0, 42, 0, 22, 1, 4, true, "Bloodangel Sword", 150, 252, 263, 40, 100, 0, 37, 400, 165, 51, 122, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 44, 0, 56, 1, 4, true, "Bloodangel Magic Sword", 150, 246, 257, 45, 100, 112, 35, 400, 120, 72, 17, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 46, 0, 270, 1, 2, true, "Bloodangel Claws", 150, 147, 157, 35, 100, 0, 48, 400, 95, 30, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 48, 0, 22, 1, 4, true, "[Bound] Sword of Destruction", 82, 101, 115, 35, 84, 0, 0, 0, 50, 50, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 49, 0, 23, 2, 3, true, "[Bound] Thunder Blade", 105, 162, 190, 40, 86, 0, 0, 0, 50, 50, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 50, 0, 261, 1, 2, true, "[Bound] Holy Storm Claws", 82, 87, 95, 30, 77, 0, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 51, 0, 22, 1, 4, true, "Blessed Divine Sword of the Archangel", 200, 265, 275, 45, 170, 0, 70, 0, 60, 30, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 52, 0, 270, 1, 2, true, "Pantera Claws", 139, 26, 34, 40, 95, 0, 42, 380, 78, 25, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 53, 0, 270, 1, 2, true, "Devast Claws", 149, 35, 46, 40, 95, 0, 54, 400, 82, 28, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 54, 0, 22, 1, 4, true, "Darkangel Sword", 160, 302, 313, 40, 100, 0, 47, 600, 165, 51, 117, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 55, 0, 56, 1, 4, true, "Darkangel Magic Sword", 160, 300, 311, 45, 100, 118, 45, 600, 120, 72, 17, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 56, 0, 270, 1, 2, true, "Darkangel Claws", 160, 214, 224, 35, 100, 0, 58, 600, 95, 30, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 57, 0, 22, 1, 4, true, "Holyangel Sword", 180, 365, 376, 40, 100, 0, 57, 800, 152, 47, 108, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 58, 0, 56, 1, 4, true, "Holyangel Magic Sword", 180, 364, 375, 45, 100, 125, 55, 800, 111, 67, 16, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 59, 0, 270, 1, 2, true, "Holyangel Claws", 180, 277, 287, 35, 100, 0, 68, 800, 88, 28, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 60, 0, 270, 1, 3, true, "Divine Claws of the Archangel", 100, 121, 130, 35, 160, 0, 50, 0, 68, 23, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 61, 0, 270, 1, 3, true, "Blessed Divine Claws of the Archangel", 200, 136, 147, 35, 160, 0, 84, 0, 60, 22, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 62, 0, 22, 1, 4, true, "Soul Sword", 200, 428, 439, 40, 100, 0, 67, 900, 141, 44, 100, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 63, 0, 56, 1, 4, true, "Soul Magic Sword", 200, 427, 438, 45, 100, 133, 65, 900, 103, 62, 15, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 64, 0, 270, 1, 2, true, "Soul Claws", 200, 340, 350, 35, 100, 0, 78, 900, 82, 26, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 65, 0, 22, 1, 3, true, "[Bound] Cookery Short Sword", 82, 100, 110, 40, 85, 0, 0, 0, 47, 47, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 66, 0, 22, 1, 3, true, "Flat Short Sword", 1, 1, 6, 20, 20, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 67, 0, 22, 1, 3, true, "Dacia Short Sword", 55, 50, 60, 40, 45, 0, 0, 0, 40, 85, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 68, 0, 22, 1, 3, true, "Cookery Short Sword", 80, 80, 90, 40, 80, 0, 0, 0, 55, 155, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 69, 0, 22, 1, 3, true, "Paring Short Sword", 135, 110, 130, 45, 100, 0, 30, 380, 55, 190, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 70, 0, 22, 1, 3, true, "Novakura Short Sword", 148, 130, 140, 45, 100, 0, 44, 400, 53, 185, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 71, 0, 22, 1, 3, true, "Bloodangel Short Sword", 150, 250, 260, 45, 100, 0, 35, 400, 53, 175, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 72, 0, 22, 1, 3, true, "Darkangel Short Sword", 160, 300, 310, 45, 100, 0, 45, 600, 53, 175, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 73, 0, 22, 1, 3, true, "Holyangel Short Sword", 180, 360, 370, 45, 100, 0, 55, 800, 49, 162, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 74, 0, 22, 1, 3, true, "Soul Short Sword", 200, 420, 440, 45, 100, 0, 65, 900, 46, 151, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 75, 0, 22, 1, 3, true, "Blue Eye Short Sword", 220, 480, 511, 45, 100, 0, 75, 1000, 192, 610, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 76, 0, 22, 1, 3, true, "Divine Short Sword of the Archangel", 86, 220, 230, 45, 160, 0, 36, 0, 50, 145, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 77, 0, 22, 1, 3, true, "Blessed Divine Short Sword of the Archangel", 200, 260, 270, 45, 170, 0, 72, 0, 25, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 78, 0, 22, 1, 4, true, "Blue Eye Sword", 220, 492, 502, 40, 100, 0, 77, 1000, 568, 184, 460, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 79, 0, 56, 1, 4, true, "Blue Eye Magic Sword", 220, 491, 501, 45, 100, 140, 75, 1000, 418, 256, 78, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 80, 0, 270, 1, 2, true, "Blue Eye Claws", 220, 404, 414, 35, 100, 0, 88, 1000, 335, 113, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 81, 0, 22, 1, 4, true, "Silver Heart Sword", 240, 556, 567, 40, 100, 0, 87, 1100, 575, 187, 468, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 82, 0, 56, 1, 4, true, "Silver Heart Magic Sword", 240, 555, 565, 45, 100, 149, 85, 1100, 425, 262, 82, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 83, 0, 270, 1, 2, true, "Silver Heart Claw", 240, 468, 480, 35, 100, 0, 98, 1100, 340, 114, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 84, 0, 22, 1, 3, true, "Silver Heart Short Sword", 240, 540, 583, 45, 100, 0, 85, 1100, 195, 618, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 85, 0, 22, 1, 4, true, "Manticore Sword", 260, 620, 632, 40, 100, 0, 97, 1200, 575, 187, 468, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 86, 0, 56, 1, 4, true, "Manticore Magic Sword", 260, 619, 629, 45, 100, 158, 95, 1200, 425, 262, 82, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 87, 0, 270, 1, 2, true, "Manticore Claws", 260, 532, 546, 35, 100, 0, 108, 1200, 340, 114, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 88, 0, 22, 1, 3, true, "Manticore Short Sword", 260, 600, 655, 45, 100, 0, 95, 1200, 195, 618, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 89, 0, 56, 2, 4, true, "Bloodangel Magic Two-Handed Sword", 150, 296, 325, 45, 100, 112, 50, 400, 120, 72, 17, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 90, 0, 56, 2, 4, true, "Darkangel Magic Two-Handed Sword", 160, 351, 382, 45, 100, 118, 70, 600, 120, 72, 17, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 91, 0, 56, 2, 4, true, "Holyangel Magic Two-Handed Sword", 180, 425, 463, 45, 100, 125, 90, 800, 111, 67, 16, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 92, 0, 56, 2, 4, true, "Soul Magic Two-Handed Sword", 200, 499, 542, 45, 100, 133, 110, 900, 103, 62, 15, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 93, 0, 56, 2, 4, true, "Blue Eye Magic Two-Handed Sword", 220, 606, 656, 45, 100, 140, 130, 1000, 418, 256, 78, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 94, 0, 56, 2, 4, true, "Silver Heart Magic Two-Handed Sword", 240, 711, 761, 45, 100, 149, 150, 1100, 425, 262, 82, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 95, 0, 56, 2, 4, true, "Manticore Magic Two-Handed Sword", 260, 816, 866, 45, 100, 158, 170, 1200, 425, 262, 82, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 96, 0, 22, 1, 4, true, "Brilliant Sword", 280, 684, 697, 40, 100, 0, 107, 1300, 575, 187, 468, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 97, 0, 56, 1, 4, true, "Brilliant Magic Sword", 280, 683, 693, 45, 100, 167, 105, 1300, 425, 262, 82, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 98, 0, 270, 1, 2, true, "Brilliant Claws", 280, 596, 612, 35, 100, 0, 118, 1300, 340, 114, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 99, 0, 22, 1, 3, true, "Brilliant Short Sword", 280, 660, 727, 45, 100, 0, 105, 1300, 195, 618, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 100, 0, 56, 2, 4, true, "Brilliant Magic Two-Handed Sword", 280, 921, 971, 45, 100, 167, 190, 1300, 425, 262, 82, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 101, 0, 0, 1, 4, true, "Silver Blade", 45, 55, 65, 35, 45, 0, 0, 0, 40, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);
        this.CreateWeapon(0, 102, 0, 0, 1, 4, true, "Xeno Blade", 85, 85, 95, 35, 80, 0, 0, 0, 50, 140, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);
        this.CreateWeapon(0, 103, 0, 0, 1, 4, true, "Dragon Blade", 133, 125, 155, 40, 100, 0, 0, 380, 45, 175, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0);
        this.CreateWeapon(0, 104, 0, 0, 1, 4, true, "Blood Blade", 149, 150, 180, 40, 100, 0, 0, 400, 50, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateWeapon(0, 105, 0, 0, 1, 4, true, "Bloodangel Blade", 150, 275, 280, 45, 100, 0, 0, 400, 50, 170, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateWeapon(0, 106, 0, 0, 1, 4, true, "Darkangel Blade", 160, 320, 330, 45, 100, 0, 0, 600, 50, 170, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateWeapon(0, 107, 0, 0, 1, 4, true, "Holyangel Blade", 180, 365, 380, 45, 100, 0, 0, 800, 48, 162, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0);
        this.CreateWeapon(0, 108, 0, 0, 1, 4, true, "Soul Blade", 200, 437, 440, 45, 100, 0, 0, 900, 46, 153, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0);
        this.CreateWeapon(0, 109, 0, 0, 1, 4, true, "Blue Eye Blade", 220, 501, 511, 45, 100, 0, 0, 1000, 201, 625, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0);
        this.CreateWeapon(0, 110, 0, 0, 1, 4, true, "Silver Heart Blade", 240, 565, 574, 45, 100, 0, 0, 1100, 207, 630, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0);
        this.CreateWeapon(0, 111, 0, 0, 1, 4, true, "Manticore Blade", 260, 629, 640, 45, 100, 0, 0, 1200, 207, 630, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0);
        this.CreateWeapon(0, 112, 0, 0, 1, 4, true, "Brilliant Blade", 280, 690, 708, 45, 100, 0, 0, 1300, 207, 630, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0);
        this.CreateWeapon(0, 113, 0, 0, 1, 4, true, "Divine Blade of the Archangel", 95, 255, 265, 45, 160, 0, 0, 0, 55, 95, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);
        this.CreateWeapon(0, 114, 0, 0, 1, 4, true, "Blessed Divine Blade of the Archangel", 200, 305, 315, 45, 170, 0, 0, 0, 50, 85, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);
        this.CreateWeapon(0, 115, 0, 0, 1, 4, true, "[Bound] Xeno Blade", 87, 95, 105, 45, 85, 0, 0, 0, 45, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);
        this.CreateWeapon(0, 116, 0, 22, 1, 4, true, "Apocalypse Sword", 300, 748, 762, 40, 100, 0, 117, 1400, 575, 187, 468, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 117, 0, 56, 1, 4, true, "Apocalypse Magic Sword", 300, 747, 757, 45, 100, 176, 115, 1400, 425, 262, 82, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 118, 0, 270, 1, 2, true, "Apocalypse Claws", 300, 660, 678, 35, 100, 0, 128, 1400, 340, 114, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 119, 0, 22, 1, 3, true, "Apocalypse Short Sword", 300, 720, 799, 45, 100, 0, 115, 1400, 195, 618, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 120, 0, 56, 2, 4, true, "Apocalypse Magic Two-Handed Sword", 300, 1026, 1076, 45, 100, 176, 210, 1400, 425, 262, 82, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 121, 0, 0, 1, 4, true, "Apocalypse Blade", 300, 751, 776, 45, 100, 0, 0, 1400, 207, 630, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0);
        this.CreateWeapon(0, 122, 0, 22, 1, 4, true, "Lightning Sword", 320, 812, 827, 40, 100, 0, 127, 1500, 575, 187, 468, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 123, 0, 56, 1, 4, true, "Lightning Magic Sword", 320, 812, 822, 45, 100, 185, 125, 1500, 425, 262, 82, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 124, 0, 270, 1, 2, true, "Lightning Claws", 320, 725, 745, 35, 100, 0, 138, 1500, 340, 114, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 125, 0, 22, 1, 3, true, "Lightning Short Sword", 320, 781, 872, 45, 100, 0, 125, 1500, 195, 618, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 126, 0, 56, 2, 4, true, "Lightning Magic Two-Handed Sword", 320, 1131, 1181, 45, 100, 185, 230, 1500, 425, 262, 82, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 127, 0, 0, 1, 4, true, "Lightning Blade", 320, 812, 844, 45, 100, 0, 0, 1500, 207, 630, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0);
        this.CreateWeapon(0, 128, 0, 22, 1, 4, true, "Temple Guard Sword", 340, 864, 879, 40, 100, 0, 222, 1500, 575, 187, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 129, 0, 56, 1, 4, true, "Temple Guard Magic Sword", 340, 864, 874, 45, 100, 194, 221, 1500, 425, 262, 82, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 130, 0, 270, 1, 2, true, "Temple Guard Claws", 340, 776, 796, 35, 100, 0, 234, 1500, 340, 114, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 131, 0, 22, 1, 3, true, "Temple Guard Short Sword", 340, 833, 924, 45, 100, 0, 220, 1500, 195, 618, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 132, 0, 56, 2, 4, true, "Temple Guard Magic Two-Hand Sword", 340, 1184, 1234, 45, 100, 194, 327, 1500, 425, 262, 82, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(0, 133, 0, 0, 1, 4, true, "Temple Guard Blade", 340, 864, 896, 45, 100, 0, 0, 1500, 207, 630, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0);
        this.CreateWeapon(0, 134, 0, 22, 1, 4, true, "Temple Guard Faith Sword", 340, 864, 879, 40, 100, 0, 222, 1500, 30, 187, 468, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(1, 0, 0, 0, 1, 3, true, "Small Axe", 1, 1, 6, 20, 18, 0, 0, 0, 50, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 1, 1);
        this.CreateWeapon(1, 1, 0, 0, 1, 3, true, "Hand Axe", 4, 4, 9, 30, 20, 0, 0, 0, 70, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 1, 1);
        this.CreateWeapon(1, 2, 0, 19, 1, 3, true, "Double Axe", 14, 14, 24, 20, 26, 0, 0, 0, 90, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(1, 3, 0, 19, 1, 3, true, "Tomahawk", 18, 18, 28, 30, 28, 0, 0, 0, 100, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(1, 4, 0, 0, 1, 3, true, "Elven Axe", 26, 26, 38, 40, 32, 0, 0, 0, 50, 70, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0);
        this.CreateWeapon(1, 5, 0, 19, 2, 3, true, "Battle Axe", 30, 36, 44, 20, 36, 0, 0, 0, 120, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(1, 6, 0, 19, 2, 3, true, "Nikea Axe", 34, 38, 50, 30, 44, 0, 0, 0, 130, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(1, 7, 0, 19, 2, 3, true, "Larkan Axe", 46, 54, 67, 25, 55, 0, 0, 0, 140, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(1, 8, 0, 19, 2, 3, true, "Crescent Axe", 54, 69, 89, 30, 65, 0, 0, 0, 100, 40, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0);
        this.CreateWeapon(2, 0, 0, 0, 1, 3, true, "Mace", 7, 7, 13, 15, 21, 0, 0, 0, 100, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 1, 0, 19, 1, 3, true, "Morning Star", 13, 13, 22, 15, 25, 0, 0, 0, 100, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 2, 0, 0, 1, 3, true, "Flail", 22, 22, 32, 15, 32, 0, 0, 0, 80, 50, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 3, 0, 19, 2, 3, true, "Great Hammer", 38, 45, 56, 15, 50, 0, 0, 0, 150, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 4, 0, 19, 2, 3, true, "Crystal Morning Star", 66, 78, 107, 30, 72, 0, 0, 0, 130, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0, 1, 0);
        this.CreateWeapon(2, 5, 0, 23, 2, 4, true, "Crystal Sword", 72, 89, 120, 40, 76, 0, 0, 0, 130, 70, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 1, 0);
        this.CreateWeapon(2, 6, 0, 23, 2, 4, true, "Chaos Dragon Axe", 75, 102, 130, 35, 80, 0, 0, 0, 140, 50, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
        this.CreateWeapon(2, 7, 0, 0, 1, 3, true, "Elemental Mace", 90, 62, 80, 50, 50, 0, 0, 0, 15, 42, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 8, 0, 66, 1, 3, true, "Battle Scepter", 54, 41, 52, 45, 40, 3, 0, 0, 80, 17, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 9, 0, 66, 1, 3, true, "Master Scepter", 72, 57, 68, 45, 45, 7, 0, 0, 87, 18, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 10, 0, 66, 1, 4, true, "Great Scepter", 82, 74, 85, 45, 65, 10, 0, 0, 100, 21, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 11, 0, 66, 1, 4, true, "Lord Scepter", 98, 91, 102, 40, 72, 16, 0, 0, 105, 23, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 12, 0, 66, 1, 4, true, "Great Lord Scepter", 140, 108, 120, 40, 84, 47, 0, 0, 90, 20, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 13, 0, 66, 1, 4, true, "Divine Scepter of the Archangel", 104, 200, 223, 45, 90, 60, 45, 0, 75, 16, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 14, 0, 66, 1, 4, true, "Soleil Scepter", 146, 130, 153, 40, 95, 50, 0, 380, 80, 15, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 15, 0, 66, 1, 4, true, "Shining Scepter", 110, 99, 111, 40, 78, 25, 0, 0, 108, 22, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 16, 0, 0, 1, 3, true, "Frost Mace", 121, 106, 146, 50, 80, 0, 0, 380, 27, 19, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 17, 0, 66, 1, 4, true, "Absolute Scepter", 135, 114, 132, 40, 90, 50, 44, 380, 119, 24, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 18, 0, 66, 1, 4, true, "Striker Scepter", 147, 112, 124, 40, 86, 55, 0, 0, 87, 20, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 19, 0, 66, 1, 4, true, "Thunderbolt", 149, 127, 154, 40, 78, 70, 52, 400, 111, 22, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 20, 0, 19, 2, 4, true, "Horn of Steal", 149, 177, 206, 25, 120, 0, 0, 400, 182, 50, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 22, 0, 66, 1, 4, true, "Bloodangel Scepter", 150, 188, 200, 40, 100, 78, 55, 400, 114, 23, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 24, 0, 66, 1, 4, true, "[Bound] Lord's Scepter", 98, 113, 119, 40, 72, 16, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 25, 0, 66, 1, 4, true, "Blessed Divine Scepter of the Archangel", 200, 331, 341, 45, 170, 120, 93, 0, 75, 16, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 26, 0, 66, 1, 4, true, "Darkangel Scepter", 160, 223, 233, 40, 100, 88, 75, 600, 114, 23, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 27, 0, 66, 1, 4, true, "Holyangel Scepter", 180, 318, 330, 40, 100, 98, 95, 800, 105, 22, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 28, 0, 0, 1, 3, true, "Elemental Rune Mace", 52, 22, 23, 50, 0, 58, 0, 0, 10, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 29, 0, 0, 1, 3, true, "El Hazard Rune Mace", 90, 55, 59, 50, 0, 103, 0, 0, 32, 17, 27, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 30, 0, 0, 1, 3, true, "Frost Rune Mace", 122, 62, 66, 50, 0, 135, 0, 380, 32, 12, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 31, 0, 0, 1, 3, true, "Light Lord Rune Mace", 148, 72, 74, 50, 0, 155, 0, 400, 35, 14, 110, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 32, 0, 0, 1, 3, true, "Bloodangel Rune Mace", 150, 95, 101, 50, 0, 165, 0, 400, 42, 14, 129, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 33, 0, 0, 1, 3, true, "Darkangel Rune Mace", 160, 125, 131, 50, 0, 183, 0, 600, 42, 14, 129, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 34, 0, 0, 1, 3, true, "Divine Rune Mace of the Archangel", 104, 158, 170, 50, 0, 166, 0, 0, 32, 4, 64, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 35, 0, 0, 1, 3, true, "Blessed Divine Rune Mace of the Archangel", 200, 178, 190, 50, 0, 200, 0, 0, 28, 4, 79, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 36, 0, 66, 1, 4, true, "Soul Scepter", 200, 403, 416, 40, 100, 108, 115, 900, 98, 21, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 37, 0, 0, 1, 3, true, "[Bound] Elemental Rune Mace", 52, 22, 23, 50, 0, 58, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 38, 0, 0, 1, 3, true, "Holyangel Rune Mace", 180, 135, 141, 50, 0, 208, 0, 800, 42, 14, 129, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 39, 0, 0, 1, 3, true, "Soul Rune Mace", 200, 145, 151, 50, 0, 230, 0, 900, 39, 13, 120, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 40, 0, 66, 1, 4, true, "Blue Eye Scepter", 220, 490, 504, 40, 100, 118, 135, 1000, 398, 94, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 41, 0, 0, 1, 3, true, "Blue Eye Rune Mace", 220, 156, 171, 50, 0, 250, 0, 1000, 165, 62, 551, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 42, 0, 66, 1, 4, true, "Silver Heart Scepter", 240, 577, 593, 40, 100, 128, 155, 1100, 406, 95, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 43, 0, 0, 1, 3, true, "Silver Heart Rune Mace", 240, 185, 200, 50, 0, 275, 0, 1100, 168, 65, 554, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 44, 0, 66, 1, 4, true, "Manticore Scepter", 260, 664, 682, 40, 100, 138, 175, 1200, 406, 95, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 45, 0, 0, 1, 3, true, "Manticore Rune Mace", 260, 210, 225, 50, 0, 300, 0, 1200, 168, 65, 554, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 46, 0, 66, 1, 4, true, "Brilliant Scepter", 280, 751, 771, 40, 100, 148, 195, 1300, 406, 95, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 47, 0, 0, 1, 3, true, "Brilliant Rune Mace", 280, 232, 248, 50, 0, 325, 0, 1300, 168, 65, 554, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 48, 0, 66, 1, 4, true, "Apocalypse Scepter", 300, 838, 860, 40, 100, 158, 215, 1400, 406, 95, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 49, 0, 0, 1, 3, true, "Apocalypse Rune Mace", 300, 260, 271, 50, 0, 350, 0, 1400, 168, 65, 554, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 50, 0, 66, 1, 4, true, "Lightning Scepter", 320, 924, 948, 40, 100, 168, 235, 1500, 406, 95, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 51, 0, 0, 1, 3, true, "Lightning Rune Mace", 320, 277, 295, 50, 0, 375, 0, 1500, 168, 65, 554, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 52, 0, 66, 1, 4, true, "Temple Guard Scepter", 340, 977, 1001, 40, 100, 178, 330, 1500, 406, 95, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 53, 0, 0, 1, 3, true, "Temple Guard Rune Mace", 340, 298, 316, 50, 0, 486, 0, 1500, 168, 65, 554, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(2, 54, 0, 19, 1, 3, true, "플루어해머", 10, 45, 65, 40, 45, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
        this.CreateWeapon(2, 55, 0, 19, 1, 3, true, "컨시퍼드해머", 50, 105, 135, 40, 80, 0, 0, 0, 30, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
        this.CreateWeapon(2, 58, 0, 19, 1, 3, true, "대천사의절대해머", 100, 240, 250, 40, 160, 0, 60, 0, 102, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
        this.CreateWeapon(2, 60, 0, 19, 1, 3, true, "블러드엔젤해머", 150, 242, 253, 40, 100, 0, 65, 400, 122, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        this.CreateWeapon(2, 72, 0, 19, 1, 3, true, "컨시퍼드해머(귀속)", 50, 115, 145, 40, 85, 0, 0, 0, 30, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
        this.CreateWeapon(3, 0, 0, 22, 2, 4, true, "Light Spear", 42, 50, 63, 25, 56, 0, 0, 0, 60, 70, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 1, 0, 0, 2, 4, true, "Spear", 23, 30, 41, 30, 42, 0, 0, 0, 70, 50, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 2, 0, 0, 2, 4, true, "Dragon Lance", 15, 21, 33, 30, 34, 0, 0, 0, 70, 50, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 3, 0, 0, 2, 4, true, "Giant Trident", 29, 35, 43, 25, 44, 0, 0, 0, 90, 30, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 4, 0, 20, 2, 4, true, "Serpent Spear", 46, 58, 80, 20, 58, 0, 0, 0, 90, 30, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 5, 0, 0, 2, 4, true, "Double Poleaxe", 13, 19, 31, 30, 38, 0, 0, 0, 70, 50, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 6, 0, 0, 2, 4, true, "Halberd", 19, 25, 35, 30, 40, 0, 0, 0, 70, 50, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 7, 0, 22, 2, 4, true, "Berdysh", 37, 42, 54, 30, 54, 0, 0, 0, 80, 50, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 8, 0, 22, 2, 4, true, "Great Scythe", 54, 71, 92, 25, 68, 0, 0, 0, 90, 50, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 9, 0, 22, 2, 4, true, "Bill of Balrog", 63, 76, 102, 25, 74, 0, 0, 0, 80, 50, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 10, 0, 22, 2, 4, true, "Dragon Spear", 92, 112, 140, 35, 85, 0, 0, 0, 170, 60, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 11, 0, 22, 2, 4, true, "Brova", 147, 190, 226, 40, 90, 0, 0, 0, 152, 25, 0, 0, 0, 2, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 12, 0, 20, 2, 4, true, "Magmu Spear", 149, 135, 236, 35, 85, 0, 0, 400, 49, 162, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 13, 0, 20, 1, 4, true, "Lapid Lance", 15, 32, 41, 40, 34, 0, 0, 0, 65, 77, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 14, 0, 20, 1, 4, true, "Conmocion Lance", 65, 65, 72, 40, 58, 0, 0, 0, 100, 120, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 15, 0, 20, 1, 4, true, "Pluma Lance", 85, 86, 93, 40, 74, 0, 0, 0, 110, 130, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 16, 0, 20, 1, 4, true, "Vis Lance", 87, 94, 101, 35, 75, 0, 0, 0, 230, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 17, 0, 20, 1, 4, true, "Prickle Lance", 22, 27, 35, 35, 31, 0, 0, 0, 110, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 18, 0, 20, 1, 4, true, "Alacran Lance", 67, 73, 80, 35, 61, 0, 0, 0, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 19, 0, 20, 1, 4, true, "Bloodangel Lance", 150, 134, 145, 40, 100, 0, 42, 400, 70, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 21, 0, 20, 1, 4, true, "[Bound] Pluma Lance", 85, 95, 107, 40, 74, 0, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 22, 0, 20, 1, 4, true, "Rectuus Lance", 139, 78, 89, 40, 95, 0, 34, 380, 70, 51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 23, 0, 20, 1, 4, true, "Russell Light Lance", 149, 99, 110, 40, 95, 0, 41, 400, 74, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 24, 0, 20, 1, 4, true, "Darkangel Lance", 160, 150, 160, 40, 100, 0, 62, 600, 70, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 25, 0, 20, 1, 4, true, "Holyangel Lance", 180, 202, 212, 40, 100, 0, 82, 800, 65, 46, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 26, 0, 20, 1, 4, true, "Divine Lance of the Archangel", 100, 115, 126, 40, 100, 0, 35, 0, 65, 46, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 27, 0, 20, 1, 4, true, "Blessed Divine Lance of the Archangel", 200, 173, 184, 40, 100, 0, 83, 0, 62, 43, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 28, 0, 20, 1, 4, true, "Soul Lance", 200, 254, 264, 40, 100, 0, 102, 900, 61, 43, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 29, 0, 20, 1, 4, true, "Blue Eye Lance", 220, 306, 318, 40, 100, 0, 122, 1000, 252, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 30, 0, 20, 1, 4, true, "Silver Heart Lance", 240, 358, 373, 40, 100, 0, 142, 1100, 256, 183, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 31, 0, 20, 1, 4, true, "Manticore Lance", 260, 410, 428, 40, 100, 0, 162, 1200, 256, 183, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 32, 0, 20, 1, 4, true, "Brilliant Lance", 280, 462, 483, 40, 100, 0, 182, 1300, 256, 183, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 33, 0, 20, 1, 4, true, "Apocalypse Lance", 300, 514, 538, 40, 100, 0, 202, 1400, 256, 183, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 34, 0, 20, 1, 4, true, "Lightning Lance", 320, 565, 592, 40, 100, 0, 222, 1500, 256, 183, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(3, 35, 0, 20, 1, 4, true, "Temple Guard Lance", 340, 616, 643, 40, 100, 0, 318, 1500, 256, 183, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 0, 0, 24, 1, 3, true, "Short Bow", 2, 3, 5, 40, 20, 0, 0, 0, 20, 80, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 1, 0, 24, 1, 3, true, "Bow", 8, 9, 13, 40, 24, 0, 0, 0, 30, 90, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 2, 0, 24, 1, 3, true, "Elven Bow", 16, 17, 24, 40, 28, 0, 0, 0, 30, 90, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 3, 0, 24, 1, 3, true, "Battle Bow", 26, 28, 37, 40, 36, 0, 0, 0, 30, 90, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 4, 0, 24, 1, 4, true, "Tiger Bow", 40, 42, 52, 40, 43, 0, 0, 0, 30, 100, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 5, 0, 24, 1, 4, true, "Silver Bow", 56, 59, 71, 50, 48, 0, 0, 0, 30, 100, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 6, 0, 24, 1, 4, true, "Chaos Nature Bow", 75, 88, 106, 45, 68, 0, 0, 0, 40, 150, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 7, 1, 0, 1, 1, true, "Bolt", 0, 0, 0, 0, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 8, 0, 24, 1, 2, true, "Crossbow", 4, 5, 8, 50, 22, 0, 0, 0, 20, 90, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 9, 0, 24, 1, 2, true, "Golden Crossbow", 12, 13, 19, 50, 26, 0, 0, 0, 30, 90, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 10, 0, 24, 1, 2, true, "Arquebus", 20, 22, 30, 50, 31, 0, 0, 0, 30, 90, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 11, 0, 24, 1, 3, true, "Light Crossbow", 32, 35, 44, 50, 40, 0, 0, 0, 30, 90, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 12, 0, 24, 1, 3, true, "Serpent Crossbow", 48, 50, 61, 50, 45, 0, 0, 0, 30, 100, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 13, 0, 24, 1, 3, true, "Bluewing Crossbow", 68, 68, 82, 50, 56, 0, 0, 0, 40, 110, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 14, 0, 24, 1, 3, true, "Aquagold Crossbow", 72, 78, 92, 40, 60, 0, 0, 0, 50, 130, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 15, 1, 0, 1, 1, true, "Arrow", 0, 0, 0, 0, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 16, 0, 24, 1, 4, true, "Saint Crossbow", 84, 102, 127, 45, 72, 0, 0, 0, 50, 160, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 17, 0, 24, 1, 4, true, "Celestial Bow", 92, 127, 155, 45, 76, 0, 0, 0, 54, 198, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 18, 0, 24, 1, 3, true, "Divine Crossbow of the Archangel", 100, 224, 246, 55, 200, 0, 0, 0, 40, 110, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 19, 0, 24, 1, 3, true, "Great Reign Crossbow", 100, 150, 172, 50, 80, 0, 0, 0, 61, 285, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 20, 0, 24, 1, 4, true, "Arrow Viper Bow", 135, 166, 190, 55, 86, 0, 0, 0, 52, 245, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 21, 0, 24, 1, 4, true, "Sylph Wind Bow", 147, 177, 200, 55, 93, 0, 0, 380, 46, 210, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 22, 0, 24, 1, 4, true, "Albatross Bow", 110, 155, 177, 55, 70, 0, 0, 0, 60, 265, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 23, 0, 24, 1, 4, true, "Dark Stinger", 134, 162, 184, 55, 80, 0, 0, 380, 32, 209, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 24, 0, 24, 1, 4, true, "Aileen Bow", 147, 170, 194, 55, 88, 0, 0, 0, 49, 226, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 25, 0, 24, 1, 4, true, "Angelic Bow", 149, 179, 202, 55, 70, 0, 0, 400, 30, 193, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 26, 0, 24, 1, 3, true, "Devil Crossbow", 149, 167, 218, 50, 90, 0, 0, 400, 30, 193, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 28, 0, 24, 1, 4, true, "Bloodangel Bow", 150, 290, 310, 55, 100, 0, 0, 400, 31, 198, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 29, 0, 24, 1, 4, true, "[Bound] Celestial Bow", 92, 149, 170, 45, 76, 0, 0, 0, 50, 50, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 30, 0, 24, 1, 3, true, "Blessed Divine Crossbow of the Archangel", 200, 345, 380, 55, 200, 0, 0, 0, 40, 70, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 31, 0, 24, 1, 4, true, "Darkangel Bow", 160, 320, 340, 55, 100, 0, 0, 600, 31, 198, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 32, 1, 0, 1, 3, true, "Combat Quiver", 52, 111, 137, 55, 90, 0, 0, 0, 27, 98, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 33, 1, 0, 1, 3, true, "Devilwood Quiver", 149, 181, 207, 60, 95, 0, 0, 400, 29, 186, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 34, 1, 0, 1, 3, true, "Bloodangel Quiver", 150, 239, 266, 60, 100, 0, 0, 400, 30, 196, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 35, 1, 0, 1, 3, true, "Darkangel Quiver", 160, 291, 316, 60, 100, 0, 0, 600, 30, 196, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 36, 0, 24, 1, 4, true, "Holyangel Bow", 180, 375, 392, 55, 100, 0, 0, 800, 29, 183, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 37, 1, 0, 1, 3, true, "Holyangel Quiver", 180, 345, 369, 60, 100, 0, 0, 800, 28, 181, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 38, 0, 24, 1, 4, true, "Soul Bow", 200, 501, 518, 55, 100, 0, 0, 900, 27, 170, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 39, 1, 0, 1, 3, true, "Soul Quiver", 200, 398, 422, 60, 100, 0, 0, 900, 26, 168, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 40, 0, 24, 1, 4, true, "Blue Eye Bow", 220, 628, 646, 55, 100, 0, 0, 1000, 117, 682, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 41, 1, 0, 1, 3, true, "Blue Eye Quiver", 220, 451, 475, 60, 100, 0, 0, 1000, 113, 674, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 42, 0, 0, 1, 3, true, "Entropy Magic Gun", 33, 75, 86, 35, 0, 40, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 43, 0, 0, 1, 3, true, "Frere Magic Gun", 110, 116, 118, 45, 0, 75, 0, 0, 87, 53, 99, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 44, 0, 0, 1, 3, true, "Weiwen Magic Gun", 134, 127, 131, 45, 0, 105, 0, 380, 75, 44, 98, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 45, 0, 0, 1, 3, true, "Cannon Magic Gun", 149, 139, 141, 45, 0, 116, 0, 400, 69, 42, 96, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 46, 0, 0, 1, 3, true, "Divine Magic Gun of the Archangel", 104, 223, 235, 50, 0, 130, 0, 0, 35, 11, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 47, 0, 0, 1, 3, true, "Blessed Divine Magic Gun of the Archangel", 200, 223, 235, 50, 0, 160, 0, 0, 25, 4, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 48, 0, 0, 1, 3, true, "Bloodangel Magic Gun", 150, 160, 166, 50, 0, 124, 0, 400, 60, 50, 110, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 49, 0, 0, 1, 3, true, "Darkangel Magic Gun", 160, 190, 196, 50, 0, 140, 0, 600, 58, 48, 106, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 50, 0, 0, 1, 3, true, "Holyangel Magic Gun", 180, 205, 218, 50, 0, 160, 0, 800, 54, 45, 98, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 51, 0, 0, 1, 3, true, "Soul Magic Gun", 200, 227, 240, 50, 0, 180, 0, 900, 50, 42, 91, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 52, 0, 0, 1, 3, true, "Blue Eye Magic Gun", 220, 249, 263, 50, 0, 200, 0, 1000, 211, 179, 422, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 53, 0, 0, 1, 3, true, "Silver Heart Magic Gun", 240, 271, 285, 50, 0, 218, 0, 1100, 217, 184, 426, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 54, 0, 0, 1, 3, true, "Manticore Magic Gun", 260, 293, 307, 50, 0, 236, 0, 1200, 217, 184, 426, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 55, 0, 0, 1, 3, true, "[Bound] Frere Magic Gun", 110, 116, 118, 45, 0, 75, 0, 0, 30, 20, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 56, 0, 24, 1, 4, true, "Silver Heart Bow", 240, 755, 778, 55, 100, 0, 0, 1100, 118, 688, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 57, 1, 0, 1, 3, true, "Silver Heart Quiver", 240, 504, 527, 60, 100, 0, 0, 1100, 114, 680, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 58, 0, 24, 1, 4, true, "Manticore Bow", 260, 882, 910, 55, 100, 0, 0, 1200, 118, 688, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 59, 1, 0, 1, 3, true, "Manticore Quiver", 260, 557, 579, 60, 100, 0, 0, 1200, 114, 680, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 60, 0, 0, 1, 3, true, "Brilliant Magic Gun", 280, 315, 329, 50, 0, 254, 0, 1300, 217, 184, 426, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 61, 0, 24, 1, 4, true, "Brilliant Bow", 280, 1009, 1042, 55, 100, 0, 0, 1300, 118, 688, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 62, 1, 0, 1, 3, true, "Brilliant Quiver", 280, 610, 631, 60, 100, 0, 0, 1300, 114, 680, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 63, 0, 0, 1, 3, true, "Apocalypse Magic Gun", 300, 337, 351, 50, 0, 272, 0, 1400, 217, 184, 426, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 64, 0, 24, 1, 4, true, "Apocalypse Bow", 300, 1136, 1174, 55, 100, 0, 0, 1400, 118, 688, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 65, 1, 0, 1, 3, true, "Apocalypse Quiver", 300, 663, 683, 60, 100, 0, 0, 1400, 114, 680, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 66, 0, 0, 1, 3, true, "Lightning Magic Gun", 320, 359, 373, 50, 0, 290, 0, 1500, 217, 184, 426, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 67, 0, 24, 1, 4, true, "Lightning Bow", 320, 1263, 1306, 55, 100, 0, 0, 1500, 118, 688, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 68, 1, 0, 1, 3, true, "Lightning Quiver", 320, 717, 736, 60, 100, 0, 0, 1500, 114, 680, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 69, 0, 0, 1, 3, true, "Temple Guard Magic Gun", 340, 409, 423, 50, 0, 401, 0, 1500, 217, 184, 426, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 70, 0, 24, 1, 4, true, "Temple Guard Bow", 340, 1317, 1360, 55, 100, 0, 0, 1500, 118, 688, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(4, 71, 1, 0, 1, 3, true, "Temple Guard Quiver", 340, 769, 788, 60, 100, 0, 0, 1500, 114, 680, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 0, 0, 0, 1, 3, true, "Skull Staff", 6, 3, 4, 20, 0, 6, 0, 0, 40, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 1, 0, 0, 2, 3, true, "Angelic Staff", 18, 10, 12, 25, 0, 20, 0, 0, 50, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 2, 0, 0, 2, 3, true, "Serpent Staff", 30, 17, 18, 25, 0, 34, 0, 0, 50, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 3, 0, 0, 2, 4, true, "Thunder Staff", 42, 23, 25, 25, 0, 46, 0, 0, 40, 10, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 4, 0, 0, 2, 4, true, "Gorgon Staff", 52, 29, 32, 25, 0, 58, 0, 0, 50, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 5, 0, 0, 1, 4, true, "Legendary Staff", 59, 29, 31, 25, 0, 59, 0, 0, 50, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 6, 0, 0, 1, 4, true, "Staff of Resurrection", 70, 35, 39, 25, 0, 70, 0, 0, 60, 10, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 7, 0, 0, 2, 4, true, "Chaos Lightning Staff", 75, 47, 48, 30, 0, 94, 0, 0, 60, 10, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 8, 0, 0, 2, 4, true, "Staff of Destruction", 90, 50, 54, 30, 0, 101, 0, 0, 60, 10, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 9, 0, 0, 1, 4, true, "Dragon Soul Staff", 100, 46, 48, 30, 0, 92, 0, 0, 52, 16, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0);
        this.CreateWeapon(5, 10, 0, 0, 1, 4, true, "Divine Staff of the Archangel", 104, 153, 165, 30, 0, 156, 0, 0, 36, 4, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 11, 0, 0, 1, 4, true, "Staff of Kundun", 140, 55, 61, 30, 0, 110, 0, 0, 45, 16, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0);
        this.CreateWeapon(5, 12, 0, 0, 1, 4, true, "Grand Viper Staff", 147, 66, 74, 30, 0, 130, 0, 380, 39, 13, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0);
        this.CreateWeapon(5, 13, 0, 0, 1, 4, true, "Platina Wing Staff", 110, 51, 53, 30, 0, 120, 0, 0, 50, 16, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0);
        this.CreateWeapon(5, 14, 0, 0, 1, 4, true, "Mystery Stick", 28, 17, 18, 25, 0, 34, 0, 0, 34, 14, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 15, 0, 0, 1, 4, true, "Violent Wind Stick", 42, 23, 25, 25, 0, 46, 0, 0, 33, 17, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 16, 0, 0, 1, 4, true, "Red Wing Stick", 59, 29, 31, 25, 0, 59, 0, 0, 36, 14, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 17, 0, 0, 1, 4, true, "Ancient Stick", 78, 38, 40, 25, 0, 76, 0, 0, 50, 19, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 18, 0, 0, 1, 4, true, "Demonic Stick", 100, 46, 48, 30, 0, 92, 0, 0, 54, 15, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 19, 0, 0, 1, 4, true, "Storm Blitz Stick", 110, 51, 53, 30, 0, 110, 0, 380, 64, 15, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 20, 0, 0, 1, 4, true, "Eternal Wing Stick", 147, 66, 74, 30, 0, 106, 0, 380, 57, 13, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 21, 1, 0, 1, 2, true, "Book of Samut", 52, 0, 0, 25, 0, 46, 0, 0, 0, 20, 135, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 22, 1, 0, 1, 2, true, "Book of Neil", 59, 0, 0, 25, 0, 59, 0, 0, 0, 25, 168, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 23, 1, 0, 1, 2, true, "Book of Lagle", 65, 0, 0, 25, 0, 72, 0, 0, 0, 30, 201, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 30, 0, 0, 1, 4, true, "Deadly Staff", 138, 57, 59, 30, 0, 126, 0, 380, 47, 18, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 31, 0, 0, 1, 4, true, "Inberial Staff", 137, 57, 61, 30, 0, 124, 0, 380, 48, 14, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0);
        this.CreateWeapon(5, 32, 0, 0, 1, 4, true, "Summon Spirit Stick", 149, 59, 61, 30, 0, 176, 0, 400, 56, 14, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 33, 0, 0, 1, 4, true, "Chrome Staff", 147, 55, 57, 30, 0, 124, 0, 0, 50, 12, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0);
        this.CreateWeapon(5, 34, 0, 0, 1, 4, true, "Raven Stick", 147, 70, 78, 30, 0, 130, 0, 0, 50, 14, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 35, 0, 0, 1, 4, true, "Miracle Staff", 149, 67, 69, 30, 0, 130, 0, 400, 46, 13, 0, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateWeapon(5, 36, 0, 0, 1, 4, true, "Divine Stick of the Archangel", 104, 153, 165, 30, 0, 146, 0, 0, 55, 13, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 37, 0, 0, 2, 4, true, "Spite Staff", 149, 69, 71, 25, 0, 136, 0, 400, 48, 11, 0, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateWeapon(5, 41, 0, 0, 1, 4, true, "Bloodangel Staff", 150, 90, 96, 30, 0, 155, 0, 400, 47, 14, 0, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateWeapon(5, 43, 0, 0, 1, 4, true, "Bloodangel Stick", 150, 81, 87, 30, 0, 150, 0, 400, 42, 15, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 45, 0, 0, 1, 4, true, "[Bound] Legendary Staff", 59, 29, 31, 25, 0, 74, 0, 0, 50, 50, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 46, 0, 0, 1, 4, true, "[Bound] Red Winged Stick", 59, 29, 31, 25, 0, 74, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 47, 1, 0, 1, 2, true, "[Bound] Book of Lagle", 65, 0, 0, 25, 0, 75, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 48, 0, 0, 2, 4, true, "[Bound] Staff of Destruction", 90, 50, 54, 30, 0, 121, 0, 0, 50, 50, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 49, 0, 0, 1, 4, true, "Blessed Divine Arch Staff", 200, 153, 165, 30, 0, 189, 0, 0, 30, 4, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 50, 0, 0, 1, 4, true, "Blessed Divine Arch Stick", 200, 153, 165, 30, 0, 184, 0, 0, 30, 13, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 51, 0, 0, 1, 4, true, "Darkangel Staff", 160, 120, 126, 30, 0, 173, 0, 600, 47, 14, 0, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateWeapon(5, 52, 0, 0, 1, 4, true, "Darkangel Stick", 160, 111, 117, 30, 0, 169, 0, 600, 42, 15, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 53, 0, 0, 1, 4, true, "Holyangel Staff", 180, 135, 148, 30, 0, 196, 0, 800, 44, 13, 0, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateWeapon(5, 54, 0, 0, 1, 4, true, "Holyangel Stick", 180, 127, 135, 30, 0, 196, 0, 800, 39, 14, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 55, 0, 0, 1, 4, true, "Soul Staff", 200, 157, 170, 30, 0, 219, 0, 900, 41, 12, 0, 0, 4, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateWeapon(5, 56, 0, 0, 1, 4, true, "Soul Stick", 200, 145, 153, 30, 0, 223, 0, 900, 37, 13, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 57, 1, 0, 1, 2, true, "Book of Bloodangel", 150, 0, 0, 50, 0, 149, 0, 400, 0, 30, 130, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 58, 1, 0, 1, 2, true, "Book of Darkangel", 160, 0, 0, 50, 0, 170, 0, 600, 0, 29, 125, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 59, 1, 0, 1, 2, true, "Book of Holyangel", 180, 0, 0, 50, 0, 192, 0, 800, 0, 27, 115, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 60, 1, 0, 1, 2, true, "Book of Soul", 200, 0, 0, 50, 0, 215, 0, 900, 0, 25, 107, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 61, 0, 0, 1, 4, true, "Blue Eye Staff", 220, 179, 193, 30, 0, 240, 0, 1000, 173, 59, 0, 0, 4, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateWeapon(5, 62, 0, 0, 1, 4, true, "Blue Eye Stick", 220, 164, 172, 30, 0, 244, 0, 1000, 157, 62, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 63, 1, 0, 1, 2, true, "Book of Blue Eye", 220, 0, 0, 50, 0, 238, 0, 1000, 0, 110, 372, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 64, 0, 0, 1, 4, true, "Silver Heart Staff", 240, 199, 212, 30, 0, 264, 0, 1100, 176, 61, 0, 0, 4, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateWeapon(5, 65, 0, 0, 1, 4, true, "Silver Heart Stick", 240, 183, 194, 30, 0, 265, 0, 1100, 159, 64, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 66, 1, 0, 1, 2, true, "Book of Silver Heart", 240, 0, 0, 50, 0, 258, 0, 1100, 0, 118, 380, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 67, 1, 0, 1, 2, true, "[Bound] Attribute Magic Book", 59, 34, 36, 25, 0, 36, 0, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 68, 1, 0, 1, 2, true, "Basic Magic Book", 28, 24, 25, 25, 0, 21, 0, 0, 0, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 69, 1, 0, 1, 2, true, "Element Magic Book", 42, 28, 30, 25, 0, 27, 0, 0, 0, 15, 16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 70, 1, 0, 1, 2, true, "Attribute Magic Book", 60, 34, 36, 25, 0, 34, 0, 0, 0, 15, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 71, 1, 0, 1, 2, true, "Wise Magic Book", 147, 43, 47, 30, 0, 43, 0, 380, 0, 15, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0);
        this.CreateWeapon(5, 72, 1, 0, 1, 2, true, "Legendary Magic Book", 149, 53, 57, 30, 0, 45, 0, 400, 0, 15, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateWeapon(5, 73, 1, 0, 1, 2, true, "Bloodangel Magic Book", 150, 43, 44, 30, 0, 47, 0, 400, 0, 15, 41, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateWeapon(5, 74, 1, 0, 1, 2, true, "Darkangel Magic Book", 160, 55, 58, 30, 0, 52, 0, 600, 0, 16, 41, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateWeapon(5, 75, 1, 0, 1, 2, true, "Holyangel Magic Book", 180, 70, 73, 30, 0, 71, 0, 800, 0, 15, 38, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0);
        this.CreateWeapon(5, 76, 1, 0, 1, 2, true, "Soul Magic Book", 200, 77, 84, 30, 0, 83, 0, 900, 0, 14, 36, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateWeapon(5, 77, 1, 0, 1, 2, true, "Blue Eye Magic Book", 220, 88, 95, 30, 0, 95, 0, 1000, 0, 106, 240, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateWeapon(5, 78, 1, 0, 1, 2, true, "Silver Heart Magic Book", 240, 99, 106, 30, 0, 107, 0, 1100, 0, 108, 245, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateWeapon(5, 79, 1, 0, 1, 2, true, "Manticore Magic Book", 260, 110, 117, 30, 0, 119, 0, 1200, 0, 110, 247, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateWeapon(5, 80, 0, 0, 2, 2, true, "[Bound] Blue Moon Orb", 59, 29, 31, 25, 0, 95, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateWeapon(5, 81, 0, 0, 2, 2, true, "Royal Orb", 30, 17, 18, 25, 0, 41, 0, 0, 10, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateWeapon(5, 82, 0, 0, 2, 2, true, "Myotis Orb", 42, 23, 25, 25, 0, 70, 0, 0, 35, 5, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateWeapon(5, 83, 0, 0, 2, 2, true, "Blue Moon Orb", 60, 29, 31, 25, 0, 95, 0, 0, 33, 5, 28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateWeapon(5, 84, 0, 0, 2, 2, true, "Spinel Orb", 147, 66, 74, 30, 0, 130, 0, 380, 14, 3, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0);
        this.CreateWeapon(5, 85, 0, 0, 2, 2, true, "Almandine Orb", 149, 86, 94, 30, 0, 150, 0, 400, 15, 4, 28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateWeapon(5, 86, 0, 0, 2, 2, true, "Bloodangel Orb", 150, 67, 69, 30, 0, 155, 0, 400, 14, 3, 34, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateWeapon(5, 87, 0, 0, 2, 2, true, "Darkangel Orb", 160, 90, 96, 30, 0, 173, 0, 600, 14, 4, 34, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateWeapon(5, 88, 0, 0, 2, 2, true, "Holyangel Orb", 180, 120, 126, 30, 0, 196, 0, 800, 13, 4, 34, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0);
        this.CreateWeapon(5, 89, 0, 0, 2, 2, true, "Soul Orb", 200, 135, 148, 30, 0, 220, 0, 900, 12, 4, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0);
        this.CreateWeapon(5, 90, 0, 0, 2, 2, true, "Blue Eye Orb", 220, 157, 170, 30, 0, 240, 0, 1000, 94, 46, 230, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0);
        this.CreateWeapon(5, 91, 0, 0, 2, 2, true, "Silver Heart Orb", 240, 179, 193, 30, 0, 265, 0, 1100, 96, 48, 235, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0);
        this.CreateWeapon(5, 92, 0, 0, 2, 2, true, "Manticore Orb", 260, 201, 215, 30, 0, 290, 0, 1200, 98, 50, 237, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0);
        this.CreateWeapon(5, 93, 0, 0, 2, 2, true, "Divine Orb of the Archangel", 104, 153, 165, 30, 0, 156, 0, 0, 17, 4, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateWeapon(5, 94, 0, 0, 2, 2, true, "Blessed Divine Orb of the Archangel", 200, 173, 185, 30, 0, 189, 0, 0, 10, 3, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        this.CreateWeapon(5, 95, 0, 0, 1, 4, true, "Manticore Staff", 260, 219, 231, 30, 0, 288, 0, 1200, 176, 61, 0, 0, 4, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateWeapon(5, 96, 0, 0, 1, 4, true, "Manticore Stick", 260, 202, 216, 30, 0, 280, 0, 1200, 159, 64, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 97, 1, 0, 1, 2, true, "Book of Manticore", 260, 0, 0, 50, 0, 274, 0, 1200, 0, 118, 380, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 98, 1, 0, 1, 2, true, "Shining Feather Magic Book", 91, 39, 41, 30, 0, 39, 0, 0, 0, 15, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        this.CreateWeapon(5, 99, 0, 0, 1, 4, true, "Brilliant Staff", 280, 239, 250, 30, 0, 312, 0, 1300, 176, 61, 0, 0, 4, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateWeapon(5, 100, 0, 0, 1, 4, true, "Brilliant Stick", 280, 221, 238, 30, 0, 295, 0, 1300, 159, 64, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 101, 1, 0, 1, 2, true, "Book of Brilliant", 280, 0, 0, 50, 0, 285, 0, 1300, 0, 118, 380, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 102, 1, 0, 1, 2, true, "Brilliant Magic Book", 280, 121, 128, 30, 0, 131, 0, 1300, 0, 110, 247, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateWeapon(5, 103, 0, 0, 2, 2, true, "Brilliant Orb", 280, 223, 237, 30, 0, 315, 0, 1300, 98, 50, 237, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0);
        this.CreateWeapon(5, 104, 0, 0, 1, 4, true, "Apocalypse Staff", 300, 259, 269, 30, 0, 336, 0, 1400, 176, 61, 0, 0, 4, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateWeapon(5, 105, 0, 0, 1, 4, true, "Apocalypse Stick", 300, 240, 260, 30, 0, 310, 0, 1400, 159, 64, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 106, 1, 0, 1, 2, true, "Book of Apocalypse", 300, 0, 0, 50, 0, 296, 0, 1400, 0, 118, 380, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 107, 1, 0, 1, 2, true, "Apocalypse Magic Book", 300, 132, 139, 30, 0, 143, 0, 1400, 0, 110, 247, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateWeapon(5, 108, 0, 0, 2, 2, true, "Apocalypse Orb", 300, 245, 259, 30, 0, 340, 0, 1400, 98, 50, 237, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0);
        this.CreateWeapon(5, 109, 0, 0, 1, 3, true, "Red Eye Wand", 30, 16, 17, 30, 0, 41, 0, 0, 5, 0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateWeapon(5, 110, 0, 0, 1, 3, true, "Benitoite Wand", 43, 20, 23, 35, 0, 70, 0, 0, 7, 5, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateWeapon(5, 111, 0, 0, 1, 3, true, "Iolite Wand", 62, 40, 45, 35, 0, 95, 0, 0, 10, 5, 28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateWeapon(5, 112, 0, 0, 1, 3, true, "Amber Wand", 147, 52, 60, 35, 0, 130, 0, 380, 11, 3, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0);
        this.CreateWeapon(5, 113, 0, 0, 1, 3, true, "Morganite Wand", 149, 65, 70, 35, 0, 150, 0, 400, 13, 4, 28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateWeapon(5, 114, 0, 0, 1, 3, true, "Bloodangel Wand", 150, 67, 69, 35, 0, 155, 0, 400, 14, 3, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateWeapon(5, 115, 0, 0, 1, 3, true, "Darkangel Wand", 160, 90, 96, 35, 0, 173, 0, 600, 14, 4, 47, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateWeapon(5, 116, 0, 0, 1, 3, true, "Holyangel Wand", 180, 120, 126, 35, 0, 196, 0, 800, 13, 4, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateWeapon(5, 117, 0, 0, 1, 3, true, "Soul Wand", 200, 135, 148, 35, 0, 220, 0, 900, 12, 4, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateWeapon(5, 118, 0, 0, 1, 3, true, "Blue Eye Wand", 220, 157, 170, 35, 0, 240, 0, 1000, 59, 28, 230, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateWeapon(5, 119, 0, 0, 1, 3, true, "Silver Heart Wand", 240, 179, 193, 35, 0, 265, 0, 1100, 60, 29, 231, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateWeapon(5, 120, 0, 0, 1, 3, true, "Manticore Wand", 260, 200, 215, 35, 0, 290, 0, 1200, 60, 29, 231, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateWeapon(5, 121, 0, 0, 1, 3, true, "Brilliant Wand", 280, 223, 237, 35, 0, 315, 0, 1300, 60, 29, 231, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateWeapon(5, 122, 0, 0, 1, 3, true, "Apocalypse Wand", 300, 245, 259, 35, 0, 340, 0, 1400, 60, 29, 231, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateWeapon(5, 123, 0, 0, 1, 3, true, "Lightning Wand", 320, 266, 280, 35, 0, 365, 0, 1500, 60, 29, 231, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateWeapon(5, 124, 0, 0, 1, 3, true, "Divine Wand of the Archangel", 104, 153, 165, 35, 0, 156, 0, 0, 17, 4, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateWeapon(5, 125, 0, 0, 1, 3, true, "Blessed Divine Wand of the Archangel", 200, 173, 185, 35, 0, 189, 0, 0, 11, 3, 16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateWeapon(5, 126, 0, 0, 1, 3, true, "[Bound] Iolite Wand", 62, 40, 45, 35, 0, 95, 0, 0, 50, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateWeapon(5, 127, 1, 0, 2, 2, true, "Performance Elixir", 28, 15, 16, 25, 0, 23, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateWeapon(5, 128, 1, 0, 2, 2, true, "Catalyst Elixir", 42, 19, 22, 25, 0, 28, 0, 0, 0, 5, 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateWeapon(5, 129, 1, 0, 2, 2, true, "Abyss Elixir", 61, 28, 30, 25, 0, 35, 0, 0, 0, 10, 16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateWeapon(5, 130, 1, 0, 2, 2, true, "Atomic Elixir", 90, 40, 46, 25, 0, 40, 0, 0, 0, 15, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateWeapon(5, 131, 1, 0, 2, 2, true, "Amber Elixir", 147, 50, 59, 30, 0, 44, 0, 380, 0, 16, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0);
        this.CreateWeapon(5, 132, 1, 0, 2, 2, true, "Morganite Elixir", 149, 63, 68, 30, 0, 46, 0, 400, 0, 16, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateWeapon(5, 133, 1, 0, 2, 2, true, "Bloodangel Elixir", 150, 66, 68, 30, 0, 48, 0, 400, 0, 15, 41, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateWeapon(5, 134, 1, 0, 2, 2, true, "Darkangel Elixir", 160, 89, 95, 30, 0, 53, 0, 600, 0, 16, 41, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateWeapon(5, 135, 1, 0, 2, 2, true, "Holyangel Elixir", 180, 119, 125, 30, 0, 72, 0, 800, 0, 15, 38, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0);
        this.CreateWeapon(5, 136, 1, 0, 2, 2, true, "Soul Elixir", 200, 134, 147, 30, 0, 84, 0, 900, 0, 14, 36, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateWeapon(5, 137, 1, 0, 2, 2, true, "Blue Eye Elixir", 220, 156, 169, 30, 0, 96, 0, 1000, 0, 67, 175, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateWeapon(5, 138, 1, 0, 2, 2, true, "Silver Heart Elixir", 240, 178, 192, 30, 0, 108, 0, 1100, 0, 68, 176, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateWeapon(5, 139, 1, 0, 2, 2, true, "Manticore Elixir", 260, 199, 214, 30, 0, 120, 0, 1200, 0, 69, 177, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateWeapon(5, 140, 1, 0, 2, 2, true, "Brilliant Elixir", 280, 222, 236, 30, 0, 132, 0, 1300, 0, 69, 177, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateWeapon(5, 141, 1, 0, 2, 2, true, "Apocalypse Elixir", 300, 244, 258, 30, 0, 144, 0, 1400, 0, 69, 177, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateWeapon(5, 142, 1, 0, 2, 2, true, "Lightning Elixir", 320, 264, 278, 30, 0, 155, 0, 1500, 0, 69, 177, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0);
        this.CreateWeapon(5, 143, 1, 0, 2, 2, true, "[Bound] Atomic Elixir", 90, 40, 46, 25, 0, 40, 0, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        this.CreateWeapon(5, 144, 0, 0, 1, 4, true, "Lightning Staff", 320, 279, 288, 30, 0, 361, 0, 1500, 176, 61, 0, 0, 4, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateWeapon(5, 145, 0, 0, 1, 4, true, "Lightning Stick", 320, 259, 282, 30, 0, 323, 0, 1500, 159, 64, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 146, 1, 0, 1, 2, true, "Book of Lightning", 320, 0, 0, 50, 0, 306, 0, 1500, 0, 118, 380, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 147, 1, 0, 1, 2, true, "Lightning Magic Book", 320, 143, 150, 30, 0, 154, 0, 1500, 0, 110, 247, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0);
        this.CreateWeapon(5, 148, 0, 0, 2, 2, true, "Lightning Orb", 320, 267, 281, 30, 0, 365, 0, 1500, 98, 50, 237, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0);
        this.CreateWeapon(5, 151, 0, 0, 1, 4, true, "Temple Guard Wizard Staff", 340, 299, 307, 30, 0, 474, 0, 1500, 176, 61, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 152, 0, 0, 1, 4, true, "Temple Guard Stick", 340, 278, 304, 30, 0, 435, 0, 1500, 159, 64, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 153, 1, 0, 1, 2, true, "Temple Guard's Book", 340, 0, 0, 50, 0, 418, 0, 1500, 0, 118, 380, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 154, 1, 0, 1, 2, true, "Temple Guard Magic Book", 340, 192, 202, 30, 0, 264, 0, 1500, 0, 110, 247, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0);
        this.CreateWeapon(5, 155, 0, 0, 2, 2, true, "Temple Guard Orb", 340, 289, 303, 30, 0, 476, 0, 1500, 98, 50, 237, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0);
        this.CreateWeapon(5, 156, 0, 0, 1, 4, true, "Temple Guard Magic Staff", 340, 299, 307, 30, 0, 474, 0, 1500, 176, 61, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.CreateWeapon(5, 157, 0, 0, 1, 4, true, "Temple Guard White Wizard Staff", 340, 299, 307, 30, 0, 474, 0, 1500, 176, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0);

        this.AddGuardianOptions();
    }

    /// <summary>
    /// Creates the item with the specified parameters.
    /// </summary>
    /// <param name="group">The group number.</param>
    /// <param name="number">The item number inside the group.</param>
    /// <param name="slot">The slot.</param>
    /// <param name="skillNumber">The skill number.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="dropsFromMonsters">if set to <c>true</c> [drops from monsters].</param>
    /// <param name="name">The name.</param>
    /// <param name="dropLevel">The drop level.</param>
    /// <param name="minimumDamage">The minimum damage.</param>
    /// <param name="maximumDamage">The maximum damage.</param>
    /// <param name="attackSpeed">The attack speed.</param>
    /// <param name="durability">The durability.</param>
    /// <param name="magicPower">The magic power.</param>
    /// <param name="levelRequirement">The level requirement.</param>
    /// <param name="strengthRequirement">The strength requirement.</param>
    /// <param name="agilityRequirement">The agility requirement.</param>
    /// <param name="energyRequirement">The energy requirement.</param>
    /// <param name="vitalityRequirement">The vitality requirement.</param>
    /// <param name="wizardClass">The wizard class.</param>
    /// <param name="knightClass">The knight class.</param>
    /// <param name="elfClass">The elf class.</param>
    /// <param name="magicGladiatorClass">The magic gladiator class.</param>
    /// <param name="darkLordClass">The dark lord class.</param>
    /// <param name="summonerClass">The summoner class.</param>
    /// <param name="ragefighterClass">The ragefighter class.</param>
    protected void CreateWeapon(byte @group, byte number, byte slot, int skillNumber, byte width, byte height,
        bool dropsFromMonsters, string name, int dropLevel, int minimumDamage, int maximumDamage, int attackSpeed,
        byte durability, int magicPower, int combatPower, int levelRequirement, int strengthRequirement, int agilityRequirement,
        int energyRequirement, int vitalityRequirement,
        int wizardClass, int knightClass, int elfClass, int magicGladiatorClass, int darkLordClass, int summonerClass, int ragefighterClass, int growLancerClass, int runeWizardClass, int slayerClass, int gunCrusherClass, int whiteWizardClass, int lemuriaClass, int illusionKnightClass, int alchemistClass, int crusaderClass)
    {
        var item = this.Context.CreateNew<ItemDefinition>();
        this.GameConfiguration.Items.Add(item);
        item.Name = name;
        item.Group = group;
        item.Number = number;

        item.Height = height;
        item.Width = width;
        item.DropLevel = dropLevel;
        item.MaximumItemLevel = MaximumItemLevel;
        item.DropsFromMonsters = dropsFromMonsters;
        item.SetGuid(item.Group, item.Number);
        if (slot == 0 && (knightClass > 0 || magicGladiatorClass > 0 || ragefighterClass > 0) && width == 1)
        {
            item.ItemSlot = this.GameConfiguration.ItemSlotTypes.First(t => t.ItemSlots.Contains(0) && t.ItemSlots.Contains(1));
        }
        else
        {
            item.ItemSlot = this.GameConfiguration.ItemSlotTypes.First(t => t.ItemSlots.Contains(slot));
        }

        if (skillNumber > 0)
        {
            var itemSkill = this.GameConfiguration.Skills.First(s => s.Number == skillNumber);
            item.Skill = itemSkill;
        }

        item.Durability = durability;
        var qualifiedCharacterClasses = this.GameConfiguration.DetermineCharacterClasses(false, wizardClass, knightClass, elfClass, magicGladiatorClass, darkLordClass, summonerClass, ragefighterClass, growLancerClass, runeWizardClass, slayerClass, gunCrusherClass, whiteWizardClass, lemuriaClass, illusionKnightClass, alchemistClass, crusaderClass);
        qualifiedCharacterClasses.ToList().ForEach(item.QualifiedCharacters.Add);

        if (height == 1) // bolts and arrows
        {
            var damagePowerUp = this.CreateItemBasePowerUpDefinition(Stats.AmmunitionDamageBonus, 0f, AggregateType.AddRaw);
            damagePowerUp.BonusPerLevelTable = this._ammunitionDamageIncreaseTable;
            item.BasePowerUpAttributes.Add(damagePowerUp);

            var manaCostPowerUp = this.CreateItemBasePowerUpDefinition(Stats.SkillExtraManaCost, 0f, AggregateType.AddRaw);
            manaCostPowerUp.BonusPerLevelTable = this._ammunitionManaLossAfterHitTable;
            item.BasePowerUpAttributes.Add(manaCostPowerUp);

            item.IsAmmunition = true;
            return;
        }

        if (minimumDamage > 0)
        {
            var minDamagePowerUp = this.CreateItemBasePowerUpDefinition(Stats.MinimumPhysBaseDmgByWeapon, minimumDamage, AggregateType.AddRaw);
            minDamagePowerUp.BonusPerLevelTable = this._weaponDamageIncreaseTable;
            item.BasePowerUpAttributes.Add(minDamagePowerUp);

            var maxDamagePowerUp = this.CreateItemBasePowerUpDefinition(Stats.MaximumPhysBaseDmgByWeapon, maximumDamage, AggregateType.AddRaw);
            maxDamagePowerUp.BonusPerLevelTable = this._weaponDamageIncreaseTable;
            item.BasePowerUpAttributes.Add(maxDamagePowerUp);
        }

        if (combatPower > 0)
        {
            var combatPowerUp = this.CreateItemBasePowerUpDefinition(Stats.CombatPower, combatPower, AggregateType.AddRaw);
            item.BasePowerUpAttributes.Add(combatPowerUp);
        }

        var speedPowerUp = this.CreateItemBasePowerUpDefinition(Stats.AttackSpeedByWeapon, attackSpeed, AggregateType.AddRaw);
        item.BasePowerUpAttributes.Add(speedPowerUp);

        this.CreateItemRequirementIfNeeded(item, Stats.Level, levelRequirement);
        this.CreateItemRequirementIfNeeded(item, Stats.TotalStrengthRequirementValue, strengthRequirement);
        this.CreateItemRequirementIfNeeded(item, Stats.TotalAgilityRequirementValue, agilityRequirement);
        this.CreateItemRequirementIfNeeded(item, Stats.TotalEnergyRequirementValue, energyRequirement);
        this.CreateItemRequirementIfNeeded(item, Stats.TotalVitalityRequirementValue, vitalityRequirement);

        item.PossibleItemOptions.Add(this.Luck);

        if (magicPower == 0 || darkLordClass > 0 || group == (int)ItemGroups.Swords)
        {
            item.PossibleItemOptions.Add(this.GameConfiguration.ItemOptions.Single(o => o.Name == ExcellentOptions.PhysicalAttackOptionsName));
            item.PossibleItemOptions.Add(this.GameConfiguration.ItemOptions.Single(o => o.Name == HarmonyOptions.PhysicalAttackOptionsName));

            if (skillNumber == (int)SkillNumber.PowerSlash)
            {
                // MG "magic swords" have a double item option, and wizardry rise, functioning as both sword and staff
                item.PossibleItemOptions.Add(this.PhysicalAndWizardryDamageOption);

                var swordRisePowerUp = this.CreateItemBasePowerUpDefinition(Stats.StaffRise, magicPower / 2.0f, AggregateType.AddRaw);
                swordRisePowerUp.BonusPerLevelTable = magicPower % 2 == 0 ? this._staffRiseTableEven : this._staffRiseTableOdd;
                item.BasePowerUpAttributes.Add(swordRisePowerUp);
            }
            else
            {
                item.PossibleItemOptions.Add(this.PhysicalDamageOption);

                if (skillNumber == (int)SkillNumber.ForceWave)
                {
                    var scepterRisePowerUp = this.CreateItemBasePowerUpDefinition(Stats.ScepterRise, magicPower / 2.0f, AggregateType.AddRaw);
                    scepterRisePowerUp.BonusPerLevelTable = magicPower % 2 == 0 ? this._scepterRiseTableEven : this._scepterRiseTableOdd;
                    item.BasePowerUpAttributes.Add(scepterRisePowerUp);
                }
            }
        }
        else
        {
            item.PossibleItemOptions.Add(this.GameConfiguration.ItemOptions.Single(o => o.Name == ExcellentOptions.WizardryAttackOptionsName));
            item.PossibleItemOptions.Add(this.GameConfiguration.ItemOptions.Single(o => o.Name == HarmonyOptions.WizardryAttackOptionsName));

            if (summonerClass > 0 && slot == 1)
            {
                item.PossibleItemOptions.Add(this.CurseDamageOption);

                var bookRisePowerUp = this.CreateItemBasePowerUpDefinition(Stats.BookRise, magicPower / 2.0f, AggregateType.AddRaw);
                bookRisePowerUp.BonusPerLevelTable = magicPower % 2 == 0 ? this._staffRiseTableEven : this._staffRiseTableOdd;
                item.BasePowerUpAttributes.Add(bookRisePowerUp);
            }
            else
            {
                item.PossibleItemOptions.Add(this.WizardryDamageOption);

                var staffRisePowerUp = this.CreateItemBasePowerUpDefinition(Stats.StaffRise, magicPower / 2.0f, AggregateType.AddRaw);
                staffRisePowerUp.BonusPerLevelTable = magicPower % 2 == 0 ? this._staffRiseTableEven : this._staffRiseTableOdd;
                item.BasePowerUpAttributes.Add(staffRisePowerUp);
            }
        }

        item.BasePowerUpAttributes.Add(this.CreateItemBasePowerUpDefinition(Stats.EquippedWeaponCount, 1, AggregateType.AddRaw));

        if (group < (int)ItemGroups.Spears && width == 1)
        {
            item.BasePowerUpAttributes.Add(this.CreateItemBasePowerUpDefinition(Stats.DoubleWieldWeaponCount, 1, AggregateType.AddRaw));
        }

        if (group == (int)ItemGroups.Bows)
        {
            item.BasePowerUpAttributes.Add(this.CreateItemBasePowerUpDefinition(Stats.AmmunitionConsumptionRate, 1, AggregateType.AddRaw));
            item.BasePowerUpAttributes.Add(this.CreateItemBasePowerUpDefinition(slot == 0 ? Stats.IsCrossBowEquipped : Stats.IsBowEquipped, 1, AggregateType.AddRaw));
        }

        if (group < (int)ItemGroups.Bows && width == 2)
        {
            item.BasePowerUpAttributes.Add(this.CreateItemBasePowerUpDefinition(Stats.IsTwoHandedWeaponEquipped, 1, AggregateType.AddRaw));
        }

        if (group == (int)ItemGroups.Swords || (group == (int)ItemGroups.Scepters && number == 5)) // Crystal Sword
        {
            if (ragefighterClass == 0 || number < 2)
            {
                if (width == 1)
                {
                    item.BasePowerUpAttributes.Add(this.CreateItemBasePowerUpDefinition(Stats.IsOneHandedSwordEquipped, 1, AggregateType.AddRaw));
                }
                else
                {
                    item.BasePowerUpAttributes.Add(this.CreateItemBasePowerUpDefinition(Stats.IsTwoHandedSwordEquipped, 1, AggregateType.AddRaw));
                }
            }
            else
            {
                item.BasePowerUpAttributes.Add(this.CreateItemBasePowerUpDefinition(Stats.IsGloveWeaponEquipped, 1, AggregateType.AddRaw));
            }
        }

        if (group == (int)ItemGroups.Spears)
        {
            item.BasePowerUpAttributes.Add(this.CreateItemBasePowerUpDefinition(Stats.IsSpearEquipped, 1, AggregateType.AddRaw));
        }

        if (group == (int)ItemGroups.Scepters)
        {
            if (skillNumber == (int)SkillNumber.ForceWave)
            {
                item.BasePowerUpAttributes.Add(this.CreateItemBasePowerUpDefinition(Stats.IsScepterEquipped, 1, AggregateType.AddRaw));
            }
            else if (knightClass > 0 && (skillNumber == (int)SkillNumber.FallingSlash || number < 5))
            {
                item.BasePowerUpAttributes.Add(this.CreateItemBasePowerUpDefinition(Stats.IsMaceEquipped, 1, AggregateType.AddRaw));
            }
            else
            {
                // not a relevant mace or scepter ...
            }
        }

        if (group == (int)ItemGroups.Staff)
        {
            if (wizardClass == 0 && summonerClass > 0 && slot == 0)
            {
                item.BasePowerUpAttributes.Add(this.CreateItemBasePowerUpDefinition(Stats.IsStickEquipped, 1, AggregateType.AddRaw));
            }
            else if (wizardClass > 0 || magicGladiatorClass > 0)
            {
                item.BasePowerUpAttributes.Add(this.CreateItemBasePowerUpDefinition(width == 1 ? Stats.IsOneHandedStaffEquipped : Stats.IsTwoHandedStaffEquipped, 1, AggregateType.AddRaw));
            }
            else
            {
                item.BasePowerUpAttributes.Add(this.CreateItemBasePowerUpDefinition(Stats.IsBookEquipped, 1, AggregateType.AddRaw));
            }
        }
    }

    private void AddGuardianOptions()
    {
        var weaponOption = this.GameConfiguration.ItemOptions.First(io => io.PossibleOptions.Any(po => po.OptionType == ItemOptionTypes.GuardianOption && po.Number == (int)ItemGroups.Weapon));

        var boneBlade = this.GameConfiguration.Items.First(i => i.Number == 22 && i.Group == (int)ItemGroups.Swords);
        var explosionBlade = this.GameConfiguration.Items.First(i => i.Number == 23 && i.Group == (int)ItemGroups.Swords);
        var phoenixSoulStar = this.GameConfiguration.Items.First(i => i.Number == 35 && i.Group == (int)ItemGroups.Swords);
        var soleilScepter = this.GameConfiguration.Items.First(i => i.Number == 14 && i.Group == (int)ItemGroups.Scepters);
        var sylphWindBow = this.GameConfiguration.Items.First(i => i.Number == 21 && i.Group == (int)ItemGroups.Bows);
        var viperStaff = this.GameConfiguration.Items.First(i => i.Number == 12 && i.Group == (int)ItemGroups.Staff);
        var stormBlitzStick = this.GameConfiguration.Items.First(i => i.Number == 19 && i.Group == (int)ItemGroups.Staff);

        boneBlade.PossibleItemOptions.Add(weaponOption);
        explosionBlade.PossibleItemOptions.Add(weaponOption);
        phoenixSoulStar.PossibleItemOptions.Add(weaponOption);
        soleilScepter.PossibleItemOptions.Add(weaponOption);
        sylphWindBow.PossibleItemOptions.Add(weaponOption);
        viperStaff.PossibleItemOptions.Add(weaponOption);
        stormBlitzStick.PossibleItemOptions.Add(weaponOption);
    }
}