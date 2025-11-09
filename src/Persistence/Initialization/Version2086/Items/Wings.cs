// <copyright file="Wings.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086.Items;

using MUnique.OpenMU.AttributeSystem;
using MUnique.OpenMU.DataModel.Configuration;
using MUnique.OpenMU.DataModel.Configuration.Items;
using MUnique.OpenMU.GameLogic.Attributes;

using MUnique.OpenMU.Persistence.Initialization;
using MUnique.OpenMU.Persistence.Initialization.Version2086.CharacterClasses;

/// <summary>
/// Initializer for wing items.
/// </summary>
public class Wings : WingsInitializerBase
{
    private ItemLevelBonusTable? _absorbByLevelTable;
    private ItemLevelBonusTable? _defenseBonusByLevelTable;
    private ItemLevelBonusTable? _damageIncreaseByLevelTable;
    private ItemLevelBonusTable? _damageIncreaseByLevelTableSecond;
    private ItemLevelBonusTable? _defenseBonusByLevelTableThird;

    /// <summary>
    /// Initializes a new instance of the <see cref="Wings"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    public Wings(IContext context, GameConfiguration gameConfiguration)
        : base(context, gameConfiguration)
    {
    }

    /// <inheritdoc />
    protected override int MaximumItemLevel => 15;

    /// <summary>
    /// Initializes all wings.
    /// </summary>
    /// <remarks>
    /// Regex: (?m)^\s*(\d+)\s+(-*\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+\"(.+?)\"\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+).*$
    /// Replace by: this.CreateWing($1, $4, $5, "$9", $10, $11, $12, $13, $19, $20, $21, $22, $23, $24, $25);
    /// Sources for stats not provided by the Item.txt:
    /// http://muonlinefanz.com/tools/items/
    /// https://wiki.infinitymu.net/index.php?title=2nd_Level_Wing
    /// https://wiki.infinitymu.net/index.php?title=3rd_Level_Wing
    /// http://www.guiamuonline.com/items-de-mu-online/wings
    /// Item option numbers:
    /// 3rd wings:
    /// 0x11 (3) -> Damage
    /// 0x00 (0) -> Recover
    /// 0x10 (2) -> Defense (for Ruin it's WizDamage)
    /// 2nd wings:
    ///             PDamage Recover WizDamage   CurseDmg
    /// Spirit       0x00    0x10
    /// Soul                 0x00    0x10
    /// Dragon       0x10    0x00
    /// Darkness     0x10            0x00
    /// Warrior Cl   0x10    0x00
    /// Despair              0x00               0x10.
    /// </remarks>
    public override void Initialize()
    {
        this._absorbByLevelTable = this.CreateAbsorbBonusPerLevel();
        this._defenseBonusByLevelTable = this.CreateBonusDefensePerLevel();
        this._defenseBonusByLevelTableThird = this.CreateBonusDefensePerLevelThirdWings();
        this._damageIncreaseByLevelTable = this.CreateDamageIncreaseBonusPerLevelFirstAndThirdWings();
        this._damageIncreaseByLevelTableSecond = this.CreateDamageIncreaseBonusPerLevelSecondWings();


        // Small Wings (lvl 0)
        this.CreateWing(12, 130, 2, 2, "Small Cape of Lord", 1, 0, 15, 200, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0, OptionType.PhysDamage)), 12, 12, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 131, 3, 2, "Small Wing of Curse", 1, 0, 10, 200, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0, OptionType.WizDamage)), 12, 12, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 132, 3, 2, "Small Wings of Elf", 1, 0, 10, 200, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0, OptionType.HealthRecover)), 12, 12, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 133, 3, 2, "Small Wings of Heaven", 1, 0, 10, 200, 1, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, this.BuildOptions((0, OptionType.WizDamage)), 12, 12, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 134, 3, 2, "Small Wings of Satan", 1, 0, 20, 200, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, this.BuildOptions((0, OptionType.PhysDamage)), 12, 12, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 135, 2, 2, "Little Warrior's Cloak", 1, 0, 15, 200, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0, OptionType.PhysDamage)), 12, 12, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 154, 2, 2, "Small Pure White Cloak", 1, 0, 10, 200, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, this.BuildOptions((0, OptionType.WizDamage)), 12, 12, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 172, 2, 2, "Small Steel Cloak", 1, 0, 20, 200, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, this.BuildOptions((0, OptionType.PhysDamage)), 12, 12, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 278, 2, 2, "Small Cloak of Limit", 1, 0, 15, 200, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0, OptionType.PhysDamage)), 12, 12, this._damageIncreaseByLevelTable, null);



        // Wings lvl 1 ok
        this.CreateWing(12, 0, 3, 2, "Wings of Elf", 100, 0, 10, 200, 150, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0, OptionType.HealthRecover)), 12, 12, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 1, 5, 3, "Wings of Heaven", 100, 0, 10, 200, 150, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, this.BuildOptions((0, OptionType.WizDamage)), 12, 12, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 2, 5, 2, "Wings of Satan", 100, 0, 20, 200, 150, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, this.BuildOptions((0, OptionType.PhysDamage)), 12, 12, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 41, 4, 2, "Wings of Curse", 100, 0, 10, 200, 150, 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0, OptionType.WizDamage)), 12, 12, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 155, 2, 3, "Pure White Cloak", 100, 0, 10, 200, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, this.BuildOptions((0, OptionType.WizDamage)), 12, 12, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 173, 2, 3, "Steel Cloak", 100, 0, 20, 200, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, this.BuildOptions((0, OptionType.PhysDamage)), 12, 12, this._damageIncreaseByLevelTable, null);
        this.CreateWing(21, 0, 2, 3, "Cloak of Protection", 100, 0, 20, 200, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, this.BuildOptions((0, OptionType.PhysDamage)), 12, 12, this._damageIncreaseByLevelTable, null); // To Check

        // Wings lvl 2
        var secondWingOptions = this.CreateSecondClassWingOptions();
        this.CreateWing(12, 3, 5, 3, "Wings of Spirits", 150, 0, 30, 200, 215, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b10, OptionType.HealthRecover), (0b00, OptionType.PhysDamage)), 32, 25, this._damageIncreaseByLevelTableSecond, secondWingOptions);
        this.CreateWing(12, 4, 5, 3, "Wings of Soul", 150, 0, 30, 200, 215, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 2, 0, 2, 0, this.BuildOptions((0b10, OptionType.HealthRecover), (0b00, OptionType.WizDamage)), 32, 25, this._damageIncreaseByLevelTableSecond, secondWingOptions);
        this.CreateWing(12, 5, 3, 3, "Wings of Dragon", 150, 0, 45, 200, 215, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b10, OptionType.HealthRecover), (0b00, OptionType.PhysDamage)), 32, 25, this._damageIncreaseByLevelTableSecond, secondWingOptions);
        this.CreateWing(12, 6, 4, 2, "Wings of Darkness", 150, 0, 40, 200, 215, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b11, OptionType.WizDamage), (0b10, OptionType.HealthRecover), (0b00, OptionType.PhysDamage)), 32, 25, this._damageIncreaseByLevelTableSecond, secondWingOptions);
        this.CreateWing(12, 42, 4, 3, "Wings of Despair", 150, 0, 30, 200, 215, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b10, OptionType.HealthRecover), (0b00, OptionType.WizDamage)), 32, 25, this._damageIncreaseByLevelTableSecond, secondWingOptions);
        this.CreateWing(12, 156, 2, 3, "Cloak of Innocence", 150, 0, 30, 200, 215, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, this.BuildOptions((0b10, OptionType.HealthRecover), (0b00, OptionType.WizDamage)), 32, 25, this._damageIncreaseByLevelTableSecond, secondWingOptions);
        this.CreateWing(12, 157, 2, 3, "[Bound] Cloak of Innocence", 150, 0, 30, 200, 215, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, this.BuildOptions((0b10, OptionType.HealthRecover), (0b00, OptionType.WizDamage)), 32, 25, this._damageIncreaseByLevelTableSecond, secondWingOptions);
        this.CreateWing(12, 174, 2, 3, "Pitch Black Cloak", 150, 0, 45, 200, 215, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, this.BuildOptions((0b10, OptionType.HealthRecover), (0b00, OptionType.PhysDamage)), 32, 25, this._damageIncreaseByLevelTableSecond, secondWingOptions);
        this.CreateWing(12, 175, 2, 3, "[Bound] Pitch Black Cloak", 150, 0, 45, 200, 215, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, this.BuildOptions((0b10, OptionType.HealthRecover), (0b00, OptionType.PhysDamage)), 32, 25, this._damageIncreaseByLevelTableSecond, secondWingOptions);
        this.CreateWing(12, 422, 5, 3, "[Bound] Wings of Soul", 150, 0, 30, 200, 215, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 2, 0, 2, 0, this.BuildOptions((0b10, OptionType.HealthRecover), (0b00, OptionType.WizDamage)), 32, 25, this._damageIncreaseByLevelTableSecond, secondWingOptions);
        this.CreateWing(12, 423, 3, 3, "[Bound] Wings of Dragon", 150, 0, 45, 200, 215, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b10, OptionType.HealthRecover), (0b00, OptionType.PhysDamage)), 32, 25, this._damageIncreaseByLevelTableSecond, secondWingOptions);
        this.CreateWing(12, 424, 5, 3, "[Bound] Wings of Spirit", 150, 0, 30, 200, 215, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b10, OptionType.HealthRecover), (0b00, OptionType.PhysDamage)), 32, 25, this._damageIncreaseByLevelTableSecond, secondWingOptions);
        this.CreateWing(12, 425, 4, 2, "[Bound] Wings of Darkness", 150, 0, 40, 200, 215, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b11, OptionType.WizDamage), (0b10, OptionType.HealthRecover), (0b00, OptionType.PhysDamage)), 32, 25, this._damageIncreaseByLevelTableSecond, secondWingOptions);
        this.CreateWing(12, 427, 4, 3, "[Bound] Wings of Despair", 150, 0, 30, 200, 215, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b10, OptionType.HealthRecover), (0b00, OptionType.WizDamage)), 32, 25, this._damageIncreaseByLevelTableSecond, secondWingOptions);

        this.CreateWing(13, 30, 2, 3, "Cape of Lord", 150, 0, 0, 200, 150, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.PhysDamage)), 20, 10, this._damageIncreaseByLevelTable, secondWingOptions);
        this.CreateWing(12, 426, 2, 3, "[Bound] Cape of Lord", 150, 0, 15, 200, 180, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.PhysDamage)), 20, 10, this._damageIncreaseByLevelTable, secondWingOptions);
        this.CreateWing(12, 49, 2, 3, "Cape of Fighter", 150, 0, 15, 200, 180, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b10, OptionType.PhysDamage)), 20, 10, this._damageIncreaseByLevelTable, secondWingOptions);
        this.CreateWing(12, 428, 2, 3, "[Bound] Cape of Fighter", 150, 0, 15, 200, 180, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b10, OptionType.PhysDamage)), 20, 10, this._damageIncreaseByLevelTable, secondWingOptions);
        this.CreateWing(12, 269, 2, 3, "Cloak of Limit", 150, 0, 15, 200, 180, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.PhysDamage)), 20, 10, this._damageIncreaseByLevelTable, secondWingOptions);
        this.CreateWing(12, 429, 2, 3, "[Bound] Cloak of Limit", 150, 0, 15, 200, 180, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.PhysDamage)), 20, 10, this._damageIncreaseByLevelTable, secondWingOptions);

        this.CreateFeather();
        this.CreateFeatherOfCondor();
        this.CreateFlameOfCondor();

        // Monster Wings (lvl 2.5)
        this.CreateWing(12, 262, 2, 3, "Cloak of Death", 150, 0, 27, 210, 290, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 2, 0, 2, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 263, 4, 3, "Wings of Chaos", 150, 0, 46, 210, 290, 0, 2, 0, 1, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 264, 4, 3, "Wings of Magic", 150, 0, 37, 210, 290, 2, 0, 0, 1, 0, 2, 0, 0, 2, 0, 2, 2, 2, 0, 2, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 265, 4, 3, "Wings of Life", 150, 0, 37, 210, 290, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 279, 2, 3, "[Bound] Cloak of Death", 150, 0, 27, 210, 290, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 1, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 280, 4, 3, "[Bound] Wings of Chaos", 150, 0, 46, 210, 290, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 281, 4, 3, "[Bound] Wings of Magic", 150, 0, 37, 210, 290, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 1, 0, 1, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 282, 4, 3, "[Bound] Wings of Life", 150, 0, 37, 210, 290, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 284, 2, 3, "[PC] Cloak of Death", 150, 0, 27, 210, 290, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 2, 0, 2, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 285, 4, 3, "[PC] Wings of Chaos", 150, 0, 46, 210, 290, 0, 2, 0, 1, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 286, 4, 3, "[PC] Wings of Magic", 150, 0, 37, 210, 290, 2, 0, 0, 1, 0, 2, 0, 0, 2, 0, 2, 2, 2, 0, 2, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, null);
        this.CreateWing(12, 287, 4, 3, "[PC] Wings of Life", 150, 0, 37, 210, 290, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, null);

        // Wings lvl 3
        var thirdWingOptions = this.CreateThirdClassWingOptions();

        this.CreateWing(12, 27, 4, 3, "Eternal Wings", 150, 0, 45, 220, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.WizDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 36, 4, 3, "Wings of Storm", 150, 0, 60, 220, 400, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 37, 4, 3, "Wings of Eternal", 150, 0, 45, 220, 400, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 38, 4, 3, "Wings of Illusion", 150, 0, 45, 220, 400, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 39, 4, 3, "Wings of Ruin", 150, 0, 55, 220, 400, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.WizDamage)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 43, 4, 3, "Wings of Dimension", 150, 0, 45, 220, 400, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.WizDamage), (0b10, OptionType.CurseDamage)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 158, 2, 3, "Cloak of Splendor", 150, 0, 45, 220, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 176, 2, 3, "Cloak of Sacrifice", 150, 0, 60, 220, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 194, 4, 3, "Wings of Guardian", 150, 0, 45, 220, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 270, 2, 3, "Cloak of Transcendence", 150, 0, 45, 220, 400, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 467, 4, 3, "Wings of Disillusion", 150, 0, 45, 220, 400, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 472, 4, 3, "Wings of Silence", 150, 0, 60, 220, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 489, 4, 3, "Wings of Hit", 150, 0, 60, 220, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 40, 2, 3, "Cape of Emperor", 150, 0, 45, 220, 400, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 24, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 50, 2, 3, "Cape of Overrule", 150, 0, 45, 220, 400, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 24, this._damageIncreaseByLevelTable, thirdWingOptions);

        this.CreateWing(12, 28, 4, 3, "[Bound] Eternal Wings", 150, 0, 45, 220, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 159, 2, 3, "[Bound] Cloak of Splendor", 150, 0, 45, 220, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 177, 2, 3, "[Bound] Cloak of Sacrifice", 150, 0, 60, 220, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 195, 4, 3, "[Bound] Wings of Guardian", 150, 0, 45, 220, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 430, 4, 3, "[Bound] Wings of Eternal", 150, 0, 45, 220, 400, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 431, 4, 3, "[Bound] Wings of Storm", 150, 0, 60, 220, 400, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 432, 4, 3, "[Bound] Wings of Illusion", 150, 0, 45, 220, 400, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 433, 4, 3, "[Bound] Wings of Ruin", 150, 0, 55, 220, 400, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.WizDamage)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 435, 4, 3, "[Bound] Wings of Curse", 150, 0, 45, 220, 400, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.WizDamage), (0b10, OptionType.CurseDamage)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 437, 2, 3, "[Bound] Cloak of Transcendence", 150, 0, 45, 220, 400, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 468, 4, 3, "[Bound] Wings of Disillusion", 150, 0, 45, 220, 400, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 473, 4, 3, "[Bound] Wings of Silence", 150, 0, 60, 220, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 496, 4, 3, "[Bound] Wings of Hit", 150, 0, 60, 220, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 434, 2, 3, "[Bound] Cape of Emperor", 150, 0, 45, 220, 400, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 24, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 436, 2, 3, "[Bound] Cape of Overrule", 150, 0, 45, 220, 400, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 24, this._damageIncreaseByLevelTable, thirdWingOptions);

        // Wings lvl 4
        this.CreateWing(12, 152, 4, 3, "Storm's Wings", 153, 0, 155, 220, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 160, 2, 3, "Cloak of Radiance", 153, 0, 155, 220, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 178, 2, 3, "Cloak of Hatred", 153, 0, 170, 220, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 196, 4, 3, "Wings of Purity", 153, 0, 155, 220, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 414, 4, 3, "Angel Wings", 153, 0, 155, 220, 800, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 415, 4, 3, "Devil Wings", 153, 0, 170, 220, 800, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 416, 4, 3, "Genius Wings", 153, 0, 170, 220, 800, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 417, 4, 3, "Destruction Wings", 153, 0, 170, 220, 800, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 419, 4, 3, "Eternal Wings", 153, 0, 170, 220, 800, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 421, 2, 3, "Eternity Cloak", 153, 0, 170, 220, 800, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 469, 4, 3, "Wings of Fate", 153, 0, 155, 220, 800, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 474, 4, 3, "Wings of Condemnation", 153, 0, 170, 220, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 490, 4, 3, "Blood Wings", 153, 0, 170, 220, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 420, 2, 3, "Judgment Cloak", 153, 0, 170, 220, 800, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 37, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 418, 2, 3, "Control Wings", 153, 0, 155, 220, 800, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 37, this._damageIncreaseByLevelTable, thirdWingOptions);

        // Wings lvl 5
        this.CreateWing(12, 180, 4, 3, "Wings of Virtue", 156, 0, 175, 220, 1200, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 181, 4, 3, "Wings of Destruction", 156, 0, 175, 220, 1200, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 182, 4, 3, "Wings of Fantasy", 156, 0, 175, 220, 1200, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 183, 4, 3, "Wings of Punishment", 156, 0, 175, 220, 1200, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 185, 4, 3, "Wings of Barrier", 156, 0, 175, 220, 1200, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 187, 2, 3, "Cloak of Discipline", 156, 0, 175, 220, 1200, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 188, 4, 3, "Wings of Inevitability", 156, 0, 175, 220, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 189, 4, 3, "Wings of Jaan", 156, 0, 175, 220, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 190, 4, 3, "Crimson Wings", 156, 0, 175, 220, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 191, 2, 3, "Cloak of Movie", 156, 0, 175, 220, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 192, 4, 3, "Wings of Eternity", 156, 0, 175, 220, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 193, 2, 3, "Cloak of Unsullied", 156, 0, 175, 220, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 198, 4, 3, "Wings of Wisdom", 156, 0, 175, 220, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 184, 2, 3, "Cloak of Youngdo", 156, 0, 175, 220, 1200, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 186, 2, 3, "Cloak of Oath", 156, 0, 175, 220, 1200, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 55, 43, this._damageIncreaseByLevelTable, thirdWingOptions);


        // Event Wings
        this.CreateWing(12, 268, 4, 3, "Wings of Conqueror (I)", 150, 0, 60, 220, 400, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 266, 4, 3, "Wings of Conqueror (II)", 150, 0, 200, 220, 1, 2, 2, 2, 1, 1, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 267, 4, 3, "Wings of Angel and Devil", 350, 0, 100, 220, 350, 2, 2, 2, 1, 1, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 283, 4, 3, "[PC] Wings of Angel and Devil", 350, 0, 100, 220, 350, 2, 2, 2, 1, 1, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
        this.CreateWing(12, 480, 4, 3, "Wings of Power", 770, 0, 700, 220, 750, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, this.BuildOptions((0b00, OptionType.HealthRecover), (0b11, OptionType.PhysDamage), (0b10, OptionType.Defense)), 39, 39, this._damageIncreaseByLevelTable, thirdWingOptions);
    }

    private void CreateFeather()
    {
        var feather = this.Context.CreateNew<ItemDefinition>();
        feather.Name = "Loch's Feather";
        feather.MaximumItemLevel = 1;
        feather.Number = 14;
        feather.Group = 13;
        feather.DropLevel = 78;
        feather.Width = 1;
        feather.Height = 2;
        feather.Durability = 1;
        feather.SetGuid(feather.Group, feather.Number);
        this.GameConfiguration.Items.Add(feather);
    }

    private void CreateFeatherOfCondor()
    {
        var feather = this.Context.CreateNew<ItemDefinition>();
        feather.Name = "Feather of Condor";
        feather.MaximumItemLevel = 1;
        feather.Number = 53;
        feather.Group = 13;
        feather.DropLevel = 120;
        feather.Width = 1;
        feather.Height = 2;
        feather.Durability = 1;
        feather.SetGuid(feather.Group, feather.Number);
        this.GameConfiguration.Items.Add(feather);
    }

    private void CreateFlameOfCondor()
    {
        var feather = this.Context.CreateNew<ItemDefinition>();
        feather.Name = "Flame of Condor";
        feather.MaximumItemLevel = 1;
        feather.Number = 52;
        feather.Group = 13;
        feather.DropLevel = 120;
        feather.Width = 1;
        feather.Height = 2;
        feather.Durability = 1;
        feather.SetGuid(feather.Group, feather.Number);
        this.GameConfiguration.Items.Add(feather);
    }

    private ItemDefinition CreateWing(byte group, short number, byte width, byte height, string name, short dropLevel, int attack, int defense, byte durability, int levelRequirement, int wizardClass, int knightClass, int elfClass, int magicGladiatorClass, int darkLordClass, int summonerClass, int ragefighterClass, int growLancerClass, int runeWizardClass, int slayerClass, int gunCrusherClass, int whiteWizardClass, int lemuriaClass, int illusionKnightClass, int alchemistClass, int crusaderClass, IEnumerable<IncreasableItemOption> possibleOptions, int damageIncreaseInitial, int damageAbsorbInitial, ItemLevelBonusTable damageIncreasePerLevel, ItemOptionDefinition? wingOptionDefinition)
    {
        var wing = this.CreateWing(group, number, width, height, name, dropLevel, defense, durability, levelRequirement, wizardClass, knightClass, elfClass, magicGladiatorClass, darkLordClass, summonerClass, ragefighterClass, growLancerClass, runeWizardClass, slayerClass, gunCrusherClass, whiteWizardClass, lemuriaClass, illusionKnightClass, alchemistClass, crusaderClass);
        if (wingOptionDefinition != null)
        {
            wing.PossibleItemOptions.Add(wingOptionDefinition);
        }

        if (damageAbsorbInitial > 0)
        {
            var powerUp = this.CreateItemBasePowerUpDefinition(Stats.DamageReceiveDecrement, 1f - (damageAbsorbInitial / 100f), AggregateType.Multiplicate);
            powerUp.BonusPerLevelTable = this._absorbByLevelTable;
            wing.BasePowerUpAttributes.Add(powerUp);
        }

        if (damageIncreaseInitial > 0)
        {
            var powerUp = this.CreateItemBasePowerUpDefinition(Stats.AttackDamageIncrease, 1f + (damageIncreaseInitial / 100f), AggregateType.Multiplicate);
            powerUp.BonusPerLevelTable = damageIncreasePerLevel;
            wing.BasePowerUpAttributes.Add(powerUp);
        }

        var optionDefinition = this.Context.CreateNew<ItemOptionDefinition>();
        optionDefinition.SetGuid(wing.GetItemId());
        this.GameConfiguration.ItemOptions.Add(optionDefinition);

        optionDefinition.Name = $"{name} Options";
        optionDefinition.AddChance = 0.25f;
        optionDefinition.AddsRandomly = true;
        optionDefinition.MaximumOptionsPerItem = 1;
        wing.PossibleItemOptions.Add(optionDefinition);
        byte i = 0;
        foreach (var option in possibleOptions)
        {
            i++;
            option.SetGuid(ItemOptionDefinitionNumbers.WingDefense, wing.GetItemId(), i);
            optionDefinition.PossibleOptions.Add(option);
        }

        wing.PossibleItemOptions.Add(this.GameConfiguration.ItemOptions.First(iod => iod.PossibleOptions.Any(o => o?.OptionType == ItemOptionTypes.Luck)));
        return wing;
    }

    private ItemDefinition CreateWing(byte group, short number, byte width, byte height, string name, short dropLevel, int defense, byte durability, int levelRequirement, int wizardClass, int knightClass, int elfClass, int magicGladiatorClass, int darkLordClass, int summonerClass, int ragefighterClass, int growLancerClass, int runeWizardClass, int slayerClass, int gunCrusherClass, int whiteWizardClass, int lemuriaClass, int illusionKnightClass, int alchemistClass, int crusaderClass)
    {
        var wing = this.Context.CreateNew<ItemDefinition>();
        this.GameConfiguration.Items.Add(wing);
        wing.Group = group;
        wing.Number = number;
        wing.Width = width;
        wing.Height = height;
        wing.Name = name;
        wing.DropLevel = dropLevel;
        wing.MaximumItemLevel = 15;
        wing.DropsFromMonsters = false;
        wing.Durability = durability;
        wing.ItemSlot = this.GameConfiguration.ItemSlotTypes.First(st => st.ItemSlots.Contains(7));
        wing.SetGuid(wing.Group, wing.Number);

        //// TODO: each level increases the requirement by 5 Levels
        this.CreateItemRequirementIfNeeded(wing, Stats.TotalLevel, levelRequirement);

        if (defense > 0)
        {
            var powerUp = this.CreateItemBasePowerUpDefinition(Stats.DefenseBase, defense, AggregateType.AddRaw);
            wing.BasePowerUpAttributes.Add(powerUp);
            powerUp.BonusPerLevelTable = levelRequirement == 400 ? this._defenseBonusByLevelTableThird : this._defenseBonusByLevelTable;
        }

        var classes = this.GameConfiguration.DetermineCharacterClasses(false, wizardClass, knightClass, elfClass, magicGladiatorClass, darkLordClass, summonerClass, ragefighterClass, growLancerClass, runeWizardClass, slayerClass, gunCrusherClass, whiteWizardClass, lemuriaClass, illusionKnightClass, alchemistClass, crusaderClass);
        foreach (var characterClass in classes)
        {
            wing.QualifiedCharacters.Add(characterClass);
        }

        // add CanFly Attribute to all wings
        var canFlyPowerUp = this.Context.CreateNew<ItemBasePowerUpDefinition>();
        canFlyPowerUp.TargetAttribute = Stats.CanFly.GetPersistent(this.GameConfiguration);
        canFlyPowerUp.BaseValue = 1;
        wing.BasePowerUpAttributes.Add(canFlyPowerUp);

        return wing;
    }

    private ItemOptionDefinition CreateCapeOptions()
    {
        var definition = this.CreateSecondClassWingOptions();
        definition.SetGuid(ItemOptionDefinitionNumbers.Cape);
        definition.Name = "Cape of Lord Options";
        definition.PossibleOptions.Add(this.CreateWingOption(4, Stats.TotalLeadership, 10f, AggregateType.AddRaw, 5f)); // Increase Command +10~85. Increases your Command by 10 plus 5 for each level. Only Cape of Lord can have it (PvM, PvP)
        this.GameConfiguration.ItemOptions.Add(definition);
        foreach (var option in definition.PossibleOptions)
        {
            option.SetGuid(ItemOptionDefinitionNumbers.Cape, option.PowerUpDefinition!.TargetAttribute!.Id.ExtractFirstTwoBytes());
        }

        return definition;
    }

    private ItemOptionDefinition CreateSecondClassWingOptions()
    {
        var definition = this.Context.CreateNew<ItemOptionDefinition>();
        definition.SetGuid(ItemOptionDefinitionNumbers.Wing2nd);
        this.GameConfiguration.ItemOptions.Add(definition);
        definition.Name = "2nd Wing Options";
        definition.AddChance = 0.1f;
        definition.AddsRandomly = true;
        definition.MaximumOptionsPerItem = 1;

        // "Excellent" 2nd wing options:
        definition.PossibleOptions.Add(this.CreateWingOption(1, Stats.MaximumHealth, 50f, AggregateType.AddRaw, 5f)); // Increase max HP +50~125. Increases your maximum amount of life by 50 plus 5 for each level (PvM, PvP)
        definition.PossibleOptions.Add(this.CreateWingOption(2, Stats.MaximumMana, 50f, AggregateType.AddRaw, 5f)); // Increase max mana +50~125. Increases your maximum amount of mana by 50 plus 5 for each level (PvM, PvP)
        definition.PossibleOptions.Add(this.CreateWingOption(3, Stats.DefenseIgnoreChance, 0.03f, AggregateType.AddRaw)); // Ignore opponent's defensive power by 3%. Gives you 3% chance to bypass your opponent's defense for a strike. This strike is shown with cyan colour (PvM, PvP)

        return definition;
    }

    private ItemOptionDefinition CreateThirdClassWingOptions()
    {
        var definition = this.Context.CreateNew<ItemOptionDefinition>();
        definition.SetGuid(ItemOptionDefinitionNumbers.Wing3rd);
        this.GameConfiguration.ItemOptions.Add(definition);
        definition.Name = "3rd Wing Options";
        definition.AddChance = 0.1f;
        definition.AddsRandomly = true;
        definition.MaximumOptionsPerItem = 1;

        definition.PossibleOptions.Add(this.CreateWingOption(1, Stats.DefenseIgnoreChance, 0.05f, AggregateType.AddRaw)); // Ignore opponent's defensive power by 5%
        definition.PossibleOptions.Add(this.CreateWingOption(2, Stats.DamageReflection, 0.05f, AggregateType.AddRaw)); // Ignore opponent's defensive power by 5%
        definition.PossibleOptions.Add(this.CreateWingOption(3, Stats.FullyRecoverHealthAfterHitChance, 0.05f, AggregateType.AddRaw)); // Fully restore health when hit by 5%
        definition.PossibleOptions.Add(this.CreateWingOption(4, Stats.FullyRecoverManaAfterHitChance, 0.05f, AggregateType.AddRaw)); // Fully recover mana when hit by 5%

        return definition;
    }

    private IncreasableItemOption CreateWingOption(byte number, AttributeDefinition attributeDefinition, float value, AggregateType aggregateType, float? valueIncrementPerLevel = null)
    {
        var itemOption = this.Context.CreateNew<IncreasableItemOption>();
        itemOption.SetGuid(ItemOptionDefinitionNumbers.Wing2nd, attributeDefinition.Id.ExtractFirstTwoBytes(), number);
        itemOption.OptionType = this.GameConfiguration.ItemOptionTypes.First(t => t == ItemOptionTypes.Wing);
        itemOption.Number = number;
        var targetAttribute = attributeDefinition.GetPersistent(this.GameConfiguration);
        itemOption.PowerUpDefinition = this.CreatePowerUpDefinition(targetAttribute, value, aggregateType);

        if (valueIncrementPerLevel.HasValue)
        {
            itemOption.LevelType = LevelType.ItemLevel;
            for (int level = 1; level <= this.MaximumItemLevel; level++)
            {
                var optionOfLevel = this.Context.CreateNew<ItemOptionOfLevel>();
                optionOfLevel.Level = level;
                optionOfLevel.PowerUpDefinition = this.CreatePowerUpDefinition(targetAttribute, value + (level * valueIncrementPerLevel.Value), aggregateType);
                itemOption.LevelDependentOptions.Add(optionOfLevel);
            }
        }

        return itemOption;
    }
}