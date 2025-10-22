// <copyright file="CharacterClassNumber.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086.CharacterClasses;

/// <summary>
/// The default character class numbers.
/// </summary>
public enum CharacterClassNumber : byte
{
    /// <summary>
    /// The dark wizard.
    /// </summary>
    DarkWizard = 0,

    /// <summary>
    /// The soul master.
    /// </summary>
    SoulMaster = 0b0000_0000_1000,

    /// <summary>
    /// The grand master.
    /// </summary>
    GrandMaster = 0b0000_0000_1100,
    SoulWizard = 0b0000_0000_1110,
    DarknessWizard = 0b0000_0000_1111,

    /// <summary>
    /// The dark knight.
    /// </summary>
    DarkKnight = 0b0000_0001_0000,

    /// <summary>
    /// The blade knight.
    /// </summary>
    BladeKnight = 0b0000_0001_1000,

    /// <summary>
    /// The blade master.
    /// </summary>
    BladeMaster = 0b0000_0001_1100,
    DragonKnight = 0b0000_0001_1110,
    IgnitionKnight = 0b0000_0001_1111,

    /// <summary>
    /// The fairy elf.
    /// </summary>
    FairyElf = 0b0000_0010_0000,

    /// <summary>
    /// The muse elf.
    /// </summary>
    MuseElf = 0b0000_0010_1000,

    /// <summary>
    /// The high elf.
    /// </summary>
    HighElf = 0b0000_0010_1100,
    NobleElf = 0b0000_0010_1110,
    RoyalElf = 0b0000_0010_1111,

    /// <summary>
    /// The magic gladiator.
    /// </summary>
    MagicGladiator = 0b0000_0011_0000,

    /// <summary>
    /// The duel master.
    /// </summary>
    DuelMaster = 0b0000_0011_1100,
    MagicKnight = 0b0000_0011_1110,
    DupleKnight = 0b0000_0011_1111,

    /// <summary>
    /// The dark lord.
    /// </summary>
    DarkLord = 0b0000_0100_0000,

    /// <summary>
    /// The lord emperor.
    /// </summary>
    LordEmperor = 0b0000_0100_1100,
    EmpireLord = 0b0000_0100_1110,
    ForceEmpire = 0b0000_0100_1111,

    /// <summary>
    /// The summoner.
    /// </summary>
    Summoner = 0b0000_0101_0000,

    /// <summary>
    /// The bloody summoner.
    /// </summary>
    BloodySummoner = 0b0000_0101_1000,

    /// <summary>
    /// The dimension master.
    /// </summary>
    DimensionMaster = 0b0000_0101_1100,
    DimensionSummoner = 0b0000_0101_1110,
    EndlessSummoner = 0b0000_0101_1111,

    /// <summary>
    /// The rage fighter.
    /// </summary>
    RageFighter = 0b0000_0110_0000,

    /// <summary>
    /// The fist master.
    /// </summary>
    FistMaster = 0b0000_0110_1100,
    FistBlazer = 0b0000_0110_1110,
    BloodyFighter = 0b0000_0110_1111,

    GrowLancer = 0b0000_0111_0000,
    MirageLancer = 0b0000_0111_1100,
    ShiningLancer = 0b0000_0111_1110,
    ArcaneLancer = 0b0000_0111_1111,

    RuneWizard = 0b0000_1000_0000,
    RuneSpellMaster = 0b0000_1000_1000,
    GrandRuneMaster = 0b0000_1000_1100,
    MajesticRuneWizard = 0b0000_1000_1110,
    InfinityRuneWizard = 0b0000_1000_1111,

    Slayer = 0b0000_1001_0000,
    RoyalSlayer = 0b0000_1001_1000,
    MasterSlayer = 0b0000_1001_1100,
    Slaughterer = 0b0000_1001_1110,
    RogueSlayer = 0b0000_1001_1111,

    GunCrusher = 0b0000_1010_0000,
    GunBreaker = 0b0000_1010_1000,
    MasterGunBreaker = 0b0000_1010_1100,
    HeistGunCrusher = 0b0000_1010_1110,
    MagnusGunCrusher = 0b0000_1010_1111,

    WhiteWizard = 0b0000_1011_0000,
    LightMaster = 0b0000_1011_1000,
    ShineWizard = 0b0000_1011_1100,
    LuminousWizard = 0b0000_1011_1110,
    GloryWizard = 0b0000_1011_1111,

    Lemuria = 0b0000_1100_0000,
    WoMage = 0b0000_1100_1000,
    ArchMage = 0b0000_1100_1100,
    MysticMage = 0b0000_1100_1110,
    BattleMage = 0b0000_1100_1111,

    IllusionKnight = 0b0000_1101_0000,
    MirageKnight = 0b0000_1101_1000,
    IllusionMaster = 0b0000_1101_1100,
    MysticKnight = 0b0000_1101_1110,
    PhantomPainKnight = 0b0000_1101_1111,

    Alchemist = 0b0000_1110_0000,
    AlchemicMagician = 0b0000_1110_1000,
    AlchemicMaster = 0b0000_1110_1100,
    AlchemicForce = 0b0000_1110_1110,
    Creator = 0b0000_1110_1111,

    Crusader = 0b0000_1111_0000,
    ImpactCrusader = 0b0000_1111_1000,
    MasterPaladin = 0b0000_1111_1100,
    SacredPaladin = 0b0000_1111_1110,
    TemplarCommander = 0b0000_1111_1111,
}

public static class ClassConsts
{
    public static readonly List<CharacterClassNumber> ALL_WIZARDS = [
        CharacterClassNumber.DarkWizard,
        CharacterClassNumber.SoulMaster,
        CharacterClassNumber.GrandMaster,
        CharacterClassNumber.SoulWizard,
        CharacterClassNumber.DarknessWizard,
    ];

    public static readonly List<CharacterClassNumber> WIZARD_SECOND_PLUS = [
        CharacterClassNumber.SoulMaster,
        CharacterClassNumber.GrandMaster,
        CharacterClassNumber.SoulWizard,
        CharacterClassNumber.DarknessWizard,
    ];

    public static readonly List<CharacterClassNumber> ALL_KNIGHTS = [
        CharacterClassNumber.DarkKnight,
        CharacterClassNumber.BladeKnight,
        CharacterClassNumber.BladeMaster,
        CharacterClassNumber.DragonKnight,
        CharacterClassNumber.IgnitionKnight,
    ];

    public static readonly List<CharacterClassNumber> KNIGHT_SECOND_PLUS = [
        CharacterClassNumber.BladeKnight,
        CharacterClassNumber.BladeMaster,
        CharacterClassNumber.DragonKnight,
        CharacterClassNumber.IgnitionKnight,
    ];

    public static readonly List<CharacterClassNumber> ALL_ELFS = [
        CharacterClassNumber.FairyElf,
        CharacterClassNumber.MuseElf,
        CharacterClassNumber.HighElf,
        CharacterClassNumber.NobleElf,
        CharacterClassNumber.RoyalElf,
    ];

    public static readonly List<CharacterClassNumber> ELF_SECOND_PLUS = [
        CharacterClassNumber.MuseElf,
        CharacterClassNumber.HighElf,
        CharacterClassNumber.NobleElf,
        CharacterClassNumber.RoyalElf,
    ];

    public static readonly List<CharacterClassNumber> ALL_MGS = [
        CharacterClassNumber.MagicGladiator,
        CharacterClassNumber.DuelMaster,
        CharacterClassNumber.MagicKnight,
        CharacterClassNumber.DupleKnight,
    ];

    public static readonly List<CharacterClassNumber> ALL_DLS = [
        CharacterClassNumber.DarkLord,
        CharacterClassNumber.LordEmperor,
        CharacterClassNumber.EmpireLord,
        CharacterClassNumber.ForceEmpire,
    ];

    public static readonly List<CharacterClassNumber> ALL_SUMMONERS = [
        CharacterClassNumber.Summoner,
        CharacterClassNumber.BloodySummoner,
        CharacterClassNumber.DimensionMaster,
        CharacterClassNumber.DimensionSummoner,
        CharacterClassNumber.EndlessSummoner,
    ];

    public static readonly List<CharacterClassNumber> ALL_FIGHTERS = [
        CharacterClassNumber.RageFighter,
        CharacterClassNumber.FistMaster,
        CharacterClassNumber.FistBlazer,
        CharacterClassNumber.BloodyFighter,
    ];

    public static readonly List<CharacterClassNumber> ALL_MAGICIANS = [
        ..ALL_WIZARDS,
        ..ALL_MGS,
    ];

    public static readonly List<CharacterClassNumber> ALL_SECOND_PLUS = [
        CharacterClassNumber.SoulMaster,
        CharacterClassNumber.GrandMaster,
        CharacterClassNumber.SoulWizard,
        CharacterClassNumber.DarknessWizard,
        CharacterClassNumber.BladeKnight,
        CharacterClassNumber.BladeMaster,
        CharacterClassNumber.DragonKnight,
        CharacterClassNumber.IgnitionKnight,
        CharacterClassNumber.MuseElf,
        CharacterClassNumber.HighElf,
        CharacterClassNumber.NobleElf,
        CharacterClassNumber.RoyalElf,
        ..ALL_DLS,
        ..ALL_MGS,
        CharacterClassNumber.BloodySummoner,
        CharacterClassNumber.DimensionMaster,
        CharacterClassNumber.DimensionSummoner,
        CharacterClassNumber.EndlessSummoner,
        ..ALL_FIGHTERS
    ];

    public static readonly List<CharacterClassNumber> ALL_MASTERS = [
        CharacterClassNumber.GrandMaster,
        CharacterClassNumber.SoulWizard,
        CharacterClassNumber.DarknessWizard,
        CharacterClassNumber.BladeMaster,
        CharacterClassNumber.DragonKnight,
        CharacterClassNumber.IgnitionKnight,
        CharacterClassNumber.HighElf,
        CharacterClassNumber.NobleElf,
        CharacterClassNumber.RoyalElf,
        CharacterClassNumber.DuelMaster,
        CharacterClassNumber.MagicKnight,
        CharacterClassNumber.DupleKnight,
        CharacterClassNumber.LordEmperor,
        CharacterClassNumber.EmpireLord,
        CharacterClassNumber.ForceEmpire,
        CharacterClassNumber.DimensionMaster,
        CharacterClassNumber.DimensionSummoner,
        CharacterClassNumber.EndlessSummoner,
        CharacterClassNumber.FistMaster,
        CharacterClassNumber.FistBlazer,
        CharacterClassNumber.BloodyFighter,
    ];

    public static readonly List<CharacterClassNumber> ALL_MASTERS_EXCEPT_RF = [
    CharacterClassNumber.GrandMaster,
        CharacterClassNumber.SoulWizard,
        CharacterClassNumber.DarknessWizard,
        CharacterClassNumber.BladeMaster,
        CharacterClassNumber.DragonKnight,
        CharacterClassNumber.IgnitionKnight,
        CharacterClassNumber.HighElf,
        CharacterClassNumber.NobleElf,
        CharacterClassNumber.RoyalElf,
        CharacterClassNumber.DuelMaster,
        CharacterClassNumber.MagicKnight,
        CharacterClassNumber.DupleKnight,
        CharacterClassNumber.LordEmperor,
        CharacterClassNumber.EmpireLord,
        CharacterClassNumber.ForceEmpire,
        CharacterClassNumber.DimensionMaster,
        CharacterClassNumber.DimensionSummoner,
        CharacterClassNumber.EndlessSummoner,
    ];

    public static readonly List<CharacterClassNumber> ALL = [
        ..ALL_WIZARDS,
        ..ALL_KNIGHTS,
        ..ALL_ELFS,
        ..ALL_MGS,
        ..ALL_DLS,
        ..ALL_SUMMONERS,
        ..ALL_FIGHTERS,
    ];
}