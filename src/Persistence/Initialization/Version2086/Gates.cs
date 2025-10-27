// <copyright file="Gates.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086;

using MUnique.OpenMU.DataModel.Configuration;

/// <summary>
/// Gates initialization.
/// </summary>
public class Gates : InitializerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Gates" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    public Gates(IContext context, GameConfiguration gameConfiguration)
        : base(context, gameConfiguration)
    {
    }

    /// <summary>
    /// Initializes the gates.
    /// </summary>
    public override void Initialize()
    {
        var maps = this.GameConfiguration.Maps.ToDictionary(map => new MapIdentity(map.Number, map.Discriminator), map => map);
        var targetGates = this.CreateTargetGates(maps);
        this.CreateEnterGates(maps, targetGates);
        this.CreateWarpEntries(targetGates);
        this.GameConfiguration.DuelConfiguration = this.CreateDuelConfiguration(targetGates);
    }

    /// <summary>
    /// Creates the warp entries.
    /// </summary>
    /// <param name="gates">The gates.</param>
    /// <remarks>
    /// MoveReq.txt
    /// Search Regex: (?m)^\s*(\d+)\s+\"(\S+)\"\s+\"(\S+)\"\s+(\d+)\s+(\d+)\s+(\d+)\s*?$
    /// Replace by: GameConfiguration.WarpList.Add(this.CreateWarpInfo($1, "$2", $4, $5, gates[$6]));.
    /// </remarks>
    private void CreateWarpEntries(IDictionary<short, ExitGate> gates)
    {
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(1, "PvP Square", 2000, 1, gates[50]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(48, "Event Square", 20000, 1, gates[503]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(44, "Loren Market", 10000, 1, gates[333]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(2, "Lorencia", 2000, 10, gates[17]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(3, "Noria", 2000, 10, gates[27]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(31, "Elbeland", 2000, 10, gates[267]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(32, "Elbeland 2", 2500, 10, gates[268]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(43, "Elbeland 3", 3000, 10, gates[269]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(4, "Devias", 2000, 10, gates[22]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(5, "Devias 2", 2500, 10, gates[72]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(6, "Devias 3", 3000, 10, gates[73]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(7, "Devias 4", 3500, 10, gates[74]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(8, "Dungeon", 3000, 20, gates[2]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(9, "Dungeon 2", 3500, 25, gates[6]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(10, "Dungeon 3", 4000, 30, gates[10]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(11, "Atlans", 4000, 50, gates[49]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(12, "Atlans 2", 4500, 80, gates[75]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(13, "Atlans 3", 5000, 90, gates[76]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(14, "LostTower", 5000, 40, gates[42]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(15, "LostTower 2", 5500, 40, gates[31]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(16, "LostTower 3", 6000, 40, gates[33]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(17, "LostTower 4", 6500, 50, gates[35]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(18, "LostTower 5", 7000, 50, gates[37]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(19, "LostTower 6", 7500, 70, gates[39]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(20, "LostTower 7", 8000, 70, gates[41]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(21, "Tarkan", 8000, 130, gates[57]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(22, "Tarkan 2", 8500, 160, gates[77]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(50, "Ferea", 50000, 400, gates[509]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(25, "Aida 1", 8500, 150, gates[119]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(27, "Aida 2", 8500, 380, gates[140]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(23, "Icarus", 10000, 160, gates[63]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(28, "Kanturu Ruins", 9000, 160, gates[138]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(29, "Kanturu Ruins 2", 9000, 160, gates[141]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(45, "Kanturu Ruins Island", 15000, 380, gates[334]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(30, "Kanturu Remain", 12000, 300, gates[139]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(46, "Karutan 1", 13000, 280, gates[335]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(47, "Karutan 2", 14000, 380, gates[344]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(34, "Raklion", 15000, 380, gates[287]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(51, "Nixies Lake", 50000, 630, gates[522]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(66, "Old Kethotum", 50000, 700, gates[648]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(33, "Swamp of Calmness", 15000, 380, gates[273]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(49, "Acheron", 50000, 300, gates[417]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(42, "Vulcanus", 15000, 300, gates[294]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(24, "Loren Deep", 0, 10, gates[104]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(52, "Deep Dungeon 1", 50000, 730, gates[561]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(53, "Deep Dungeon 2", 50000, 750, gates[562]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(54, "Deep Dungeon 3", 50000, 770, gates[563]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(55, "Deep Dungeon 4", 50000, 780, gates[564]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(56, "Deep Dungeon 5", 50000, 780, gates[565]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(57, "Swamp of Darkness", 50000, 800, gates[567]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(58, "Kubera Mine", 50000, 850, gates[591]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(59, "Abyss of Atlans 1", 50000, 900, gates[617]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(60, "Abyss of Atlans 2", 50000, 925, gates[618]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(61, "Abyss of Atlans 3", 50000, 950, gates[619]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(62, "Scorched Canyon", 50000, 1000, gates[620]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(63, "Red Smoke Icarus", 50000, 1050, gates[631]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(64, "Arenil Temple", 50000, 1100, gates[634]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(65, "Ashen Aida", 50000, 1150, gates[643]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(67, "Blaze Kethotum", 50000, 1200, gates[659]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(68, "Kanturu Undergrounds", 50000, 1250, gates[670]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(69, "Ignis Volcano", 50000, 1300, gates[675]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(70, "Blood Tarkan", 50000, 1350, gates[682]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(71, "Tormenta Island", 50000, 1400, gates[694]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(72, "Distorted Karutan", 50000, 1450, gates[699]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(73, "Kardamahal UnderGround Temple", 50000, 1500, gates[704]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(74, "Swamp of Doom", 50000, 1550, gates[713]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(35, "Kalima1", 0, 40, gates[88]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(36, "Kalima2", 0, 131, gates[89]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(37, "Kalima3", 0, 181, gates[90]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(38, "Kalima4", 0, 231, gates[91]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(39, "Kalima5", 0, 281, gates[92]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(40, "Kalima6", 0, 331, gates[93]));
        this.GameConfiguration.WarpList.Add(this.CreateWarpInfo(41, "Kalima7", 0, 350, gates[116]));
    }

    private WarpInfo CreateWarpInfo(ushort index, string name, int costs, int levelRequirement, ExitGate gate)
    {
        var warpInfo = this.Context.CreateNew<WarpInfo>();
        warpInfo.Index = index;
        warpInfo.Name = name;
        warpInfo.Costs = costs;
        warpInfo.LevelRequirement = levelRequirement;
        warpInfo.Gate = gate;
        return warpInfo;
    }

    private ExitGate CreateExitGate(GameMapDefinition map, byte x1, byte y1, byte x2, byte y2, byte direction, bool isSpawnGate = false)
    {
        if (x1 > x2)
        {
            throw new ArgumentException("x1 > x2");
        }

        if (y1 > y2)
        {
            throw new ArgumentException("y1 > y2");
        }

        var gate = this.Context.CreateNew<ExitGate>();
        gate.Map = map;
        gate.X1 = x1;
        gate.Y1 = y1;
        gate.X2 = x2;
        gate.Y2 = y2;

        // different to all other configurations, 0 means 'Undefined', so we just assume that we can cast it to Direction without adding 1.
        gate.Direction = (Direction)direction;
        gate.IsSpawnGate = isSpawnGate;
        map.ExitGates.Add(gate);
        return gate;
    }

    /// <summary>
    /// Creates the target gates and adds them to the <see cref="GameMapDefinition.ExitGates" /> if the gate is specified as spawn gate (flag == 0).
    /// </summary>
    /// <param name="maps">The previously created game maps.</param>
    /// <returns>A dictionary of all created exit gates. The key is the number.</returns>
    /// <remarks>
    /// Regex #1: (?m)^(\d+)\s+?(0)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(\d+).*?$
    /// Rplace by #1: targetGates.Add($1, this.CreateExitGate(maps[$3], $4, $5, $6, $7, $9, true));
    /// Regex #2: (?m)^(\d+)\s+?(2)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(\d+).*?$
    /// Rplace by #2: targetGates.Add($1, this.CreateExitGate(maps[$3], $4, $5, $6, $7, $9, false));.
    /// </remarks>
    private IDictionary<short, ExitGate> CreateTargetGates(IDictionary<MapIdentity, GameMapDefinition> maps)
    {
        var targetGates = new Dictionary<short, ExitGate>();

        // targetGates.Add(0, this.CreateExitGate(maps[0], 0, 0, 0, 0, 0, true));
        targetGates.Add(2, this.CreateExitGate(maps[1], 107, 247, 110, 247, 1, false));
        targetGates.Add(4, this.CreateExitGate(maps[0], 121, 231, 123, 231, 1, false));
        targetGates.Add(6, this.CreateExitGate(maps[1], 231, 126, 234, 127, 1, false));
        targetGates.Add(8, this.CreateExitGate(maps[1], 240, 148, 241, 151, 3, false));
        targetGates.Add(10, this.CreateExitGate(maps[1], 3, 83, 4, 86, 3, false));
        targetGates.Add(12, this.CreateExitGate(maps[1], 3, 16, 6, 17, 3, false));
        targetGates.Add(14, this.CreateExitGate(maps[1], 29, 125, 30, 126, 1, false));
        targetGates.Add(16, this.CreateExitGate(maps[1], 5, 32, 7, 33, 1, false));
        targetGates.Add(17, this.CreateExitGate(maps[0], 133, 118, 151, 135, 0, true));
        targetGates.Add(19, this.CreateExitGate(maps[2], 242, 34, 243, 37, 7, false));
        targetGates.Add(21, this.CreateExitGate(maps[0], 7, 38, 8, 41, 3, false));
        targetGates.Add(22, this.CreateExitGate(maps[2], 197, 35, 218, 50, 0, true));
        targetGates.Add(24, this.CreateExitGate(maps[3], 150, 9, 153, 10, 5, false));
        targetGates.Add(26, this.CreateExitGate(maps[0], 213, 244, 217, 245, 1, false));
        targetGates.Add(27, this.CreateExitGate(maps[3], 171, 108, 177, 117, 0, true));
        targetGates.Add(29, this.CreateExitGate(maps[4], 162, 2, 166, 3, 5, false));
        targetGates.Add(31, this.CreateExitGate(maps[4], 241, 237, 244, 238, 1, false));
        targetGates.Add(33, this.CreateExitGate(maps[4], 86, 166, 87, 168, 3, false));
        targetGates.Add(35, this.CreateExitGate(maps[4], 87, 86, 88, 89, 3, false));
        targetGates.Add(37, this.CreateExitGate(maps[4], 128, 53, 131, 54, 1, false));
        targetGates.Add(39, this.CreateExitGate(maps[4], 52, 53, 55, 54, 1, false));
        targetGates.Add(41, this.CreateExitGate(maps[4], 8, 85, 9, 87, 1, false));
        targetGates.Add(42, this.CreateExitGate(maps[4], 203, 70, 213, 81, 0, true));
        targetGates.Add(44, this.CreateExitGate(maps[2], 5, 244, 7, 246, 2, false));
        targetGates.Add(46, this.CreateExitGate(maps[7], 14, 12, 15, 13, 3, false));
        targetGates.Add(48, this.CreateExitGate(maps[3], 240, 240, 241, 243, 7, false));
        targetGates.Add(49, this.CreateExitGate(maps[7], 15, 11, 27, 23, 0, true));
        targetGates.Add(50, this.CreateExitGate(maps[6], 72, 140, 73, 181, 0, true));
        targetGates.Add(51, this.CreateExitGate(maps[6], 59, 153, 59, 153, 0, true));
        targetGates.Add(52, this.CreateExitGate(maps[6], 59, 164, 59, 165, 0, true));
        targetGates.Add(54, this.CreateExitGate(maps[8], 248, 40, 251, 44, 7, false));
        targetGates.Add(56, this.CreateExitGate(maps[7], 16, 225, 17, 230, 3, false));
        targetGates.Add(57, this.CreateExitGate(maps[8], 189, 63, 205, 72, 0, true));
        targetGates.Add(58, this.CreateExitGate(maps[new(9,1)], 133, 91, 141, 99, 0, true));
        targetGates.Add(59, this.CreateExitGate(maps[new(9, 2)], 135, 162, 142, 170, 0, true));
        targetGates.Add(60, this.CreateExitGate(maps[new(9, 3)], 62, 150, 70, 158, 0, true));
        targetGates.Add(61, this.CreateExitGate(maps[new(9, 4)], 66, 84, 74, 92, 0, true));
        targetGates.Add(63, this.CreateExitGate(maps[10], 14, 13, 16, 13, 5, false));
        targetGates.Add(65, this.CreateExitGate(maps[4], 17, 249, 19, 249, 1, false));
        targetGates.Add(66, this.CreateExitGate(maps[11], 12, 5, 14, 10, 0, true));
        targetGates.Add(67, this.CreateExitGate(maps[12], 12, 5, 14, 10, 0, true));
        targetGates.Add(68, this.CreateExitGate(maps[13], 12, 5, 14, 10, 0, true));
        targetGates.Add(69, this.CreateExitGate(maps[14], 12, 5, 14, 10, 0, true));
        targetGates.Add(70, this.CreateExitGate(maps[15], 12, 5, 14, 10, 0, true));
        targetGates.Add(71, this.CreateExitGate(maps[16], 12, 5, 14, 10, 0, true));
        targetGates.Add(80, this.CreateExitGate(maps[17], 12, 5, 14, 10, 0, true));
        targetGates.Add(82, this.CreateExitGate(maps[18], 31, 88, 36, 95, 0, true));
        targetGates.Add(83, this.CreateExitGate(maps[19], 31, 88, 36, 95, 0, true));
        targetGates.Add(84, this.CreateExitGate(maps[20], 31, 88, 36, 95, 0, true));
        targetGates.Add(85, this.CreateExitGate(maps[21], 31, 88, 36, 95, 0, true));
        targetGates.Add(86, this.CreateExitGate(maps[22], 31, 88, 36, 95, 0, true));
        targetGates.Add(87, this.CreateExitGate(maps[23], 31, 88, 36, 95, 0, true));
        targetGates.Add(72, this.CreateExitGate(maps[2], 23, 24, 27, 27, 0, true));
        targetGates.Add(73, this.CreateExitGate(maps[2], 224, 227, 227, 231, 0, true));
        targetGates.Add(74, this.CreateExitGate(maps[2], 69, 178, 72, 181, 0, true));
        targetGates.Add(75, this.CreateExitGate(maps[7], 225, 50, 228, 53, 0, true));
        targetGates.Add(76, this.CreateExitGate(maps[7], 62, 157, 68, 163, 0, true));
        targetGates.Add(77, this.CreateExitGate(maps[8], 96, 143, 100, 146, 0, true));
        targetGates.Add(88, this.CreateExitGate(maps[24], 10, 16, 17, 22, 0, true));
        targetGates.Add(89, this.CreateExitGate(maps[25], 10, 16, 17, 22, 0, true));
        targetGates.Add(90, this.CreateExitGate(maps[26], 10, 16, 17, 22, 0, true));
        targetGates.Add(91, this.CreateExitGate(maps[27], 10, 16, 17, 22, 0, true));
        targetGates.Add(92, this.CreateExitGate(maps[28], 10, 16, 17, 22, 0, true));
        targetGates.Add(93, this.CreateExitGate(maps[29], 10, 16, 17, 22, 0, true));
        targetGates.Add(116, this.CreateExitGate(maps[36], 10, 16, 17, 22, 0, true));
        targetGates.Add(94, this.CreateExitGate(maps[30], 88, 31, 102, 46, 0, true));
        targetGates.Add(97, this.CreateExitGate(maps[30], 164, 198, 187, 209, 0, false));
        targetGates.Add(99, this.CreateExitGate(maps[30], 90, 236, 99, 239, 0, false));
        targetGates.Add(100, this.CreateExitGate(maps[30], 39, 14, 142, 50, 0, true));
        targetGates.Add(105, this.CreateExitGate(maps[30], 72, 10, 104, 199, 0, true));
        targetGates.Add(101, this.CreateExitGate(maps[30], 84, 180, 100, 222, 0, true));
        targetGates.Add(104, this.CreateExitGate(maps[30], 87, 209, 100, 232, 0, true));
        targetGates.Add(106, this.CreateExitGate(maps[30], 131, 92, 138, 94, 0, true));
        targetGates.Add(110, this.CreateExitGate(maps[30], 131, 92, 138, 94, 0, false));
        targetGates.Add(103, this.CreateExitGate(maps[30], 29, 37, 30, 42, 0, false));
        targetGates.Add(108, this.CreateExitGate(maps[0], 235, 13, 239, 13, 0, false));

        targetGates.Add(111, this.CreateExitGate(maps[new(32, 5)], 133, 91, 141, 99, 0, true));
        targetGates.Add(112, this.CreateExitGate(maps[new(32, 6)], 135, 162, 142, 170, 0, true));
        targetGates.Add(95, this.CreateExitGate(maps[31], 60, 10, 69, 19, 0, true));
        targetGates.Add(113, this.CreateExitGate(maps[33], 76, 9, 78, 16, 0, false));
        targetGates.Add(122, this.CreateExitGate(maps[3], 220, 31, 226, 34, 0, false));
        targetGates.Add(119, this.CreateExitGate(maps[33], 82, 8, 87, 14, 0, true));
        targetGates.Add(140, this.CreateExitGate(maps[33], 186, 173, 190, 177, 0, true));
        targetGates.Add(114, this.CreateExitGate(maps[34], 231, 37, 234, 45, 0, false));
        targetGates.Add(124, this.CreateExitGate(maps[30], 155, 37, 158, 43, 0, false));
        targetGates.Add(118, this.CreateExitGate(maps[34], 229, 37, 239, 46, 0, true));
        targetGates.Add(126, this.CreateExitGate(maps[37], 17, 219, 21, 220, 0, false));
        targetGates.Add(128, this.CreateExitGate(maps[8], 7, 199, 7, 201, 0, false));
        targetGates.Add(130, this.CreateExitGate(maps[38], 70, 104, 70, 107, 0, false));
        targetGates.Add(132, this.CreateExitGate(maps[37], 85, 89, 86, 92, 0, false));
        targetGates.Add(133, this.CreateExitGate(maps[39], 196, 56, 201, 57, 0, true));
        targetGates.Add(134, this.CreateExitGate(maps[39], 78, 93, 82, 95, 0, true));
        targetGates.Add(135, this.CreateExitGate(maps[39], 78, 93, 82, 95, 0, true));
        targetGates.Add(136, this.CreateExitGate(maps[38], 137, 162, 143, 163, 0, true));
        targetGates.Add(137, this.CreateExitGate(maps[38], 71, 104, 72, 107, 0, true));
        targetGates.Add(138, this.CreateExitGate(maps[37], 19, 217, 21, 219, 0, true));
        targetGates.Add(139, this.CreateExitGate(maps[38], 71, 104, 72, 107, 0, true));
        targetGates.Add(141, this.CreateExitGate(maps[37], 205, 36, 208, 41, 0, true));
        targetGates.Add(142, this.CreateExitGate(maps[45], 98, 128, 108, 137, 0, true));
        targetGates.Add(143, this.CreateExitGate(maps[46], 98, 128, 108, 137, 0, true));
        targetGates.Add(144, this.CreateExitGate(maps[47], 98, 128, 108, 137, 0, true));
        targetGates.Add(145, this.CreateExitGate(maps[48], 98, 128, 108, 137, 0, true));
        targetGates.Add(146, this.CreateExitGate(maps[49], 98, 128, 108, 137, 0, true));
        targetGates.Add(147, this.CreateExitGate(maps[50], 98, 128, 108, 137, 0, true));
        targetGates.Add(148, this.CreateExitGate(maps[45], 141, 41, 146, 45, 0, true));
        targetGates.Add(149, this.CreateExitGate(maps[46], 141, 41, 146, 45, 0, true));
        targetGates.Add(150, this.CreateExitGate(maps[47], 141, 41, 146, 45, 0, true));
        targetGates.Add(151, this.CreateExitGate(maps[48], 141, 41, 146, 45, 0, true));
        targetGates.Add(152, this.CreateExitGate(maps[49], 141, 41, 146, 45, 0, true));
        targetGates.Add(153, this.CreateExitGate(maps[50], 141, 41, 146, 45, 0, true));
        targetGates.Add(154, this.CreateExitGate(maps[45], 194, 124, 198, 127, 0, true));
        targetGates.Add(155, this.CreateExitGate(maps[46], 194, 124, 198, 127, 0, true));
        targetGates.Add(156, this.CreateExitGate(maps[47], 194, 124, 198, 127, 0, true));
        targetGates.Add(157, this.CreateExitGate(maps[48], 194, 124, 198, 127, 0, true));
        targetGates.Add(158, this.CreateExitGate(maps[49], 194, 124, 198, 127, 0, true));
        targetGates.Add(159, this.CreateExitGate(maps[50], 194, 124, 198, 127, 0, true));
        targetGates.Add(256, this.CreateExitGate(maps[41], 29, 79, 31, 82, 0, true));
        targetGates.Add(257, this.CreateExitGate(maps[42], 104, 178, 107, 181, 0, true));
        targetGates.Add(258, this.CreateExitGate(maps[34], 227, 41, 229, 43, 0, true));
        targetGates.Add(260, this.CreateExitGate(maps[51], 26, 29, 27, 30, 0, false));
        targetGates.Add(262, this.CreateExitGate(maps[2], 161, 241, 163, 242, 0, false));
        targetGates.Add(264, this.CreateExitGate(maps[51], 243, 149, 244, 150, 0, false));
        targetGates.Add(266, this.CreateExitGate(maps[7], 16, 19, 17, 20, 0, false));
        targetGates.Add(267, this.CreateExitGate(maps[51], 51, 224, 54, 227, 0, true));
        targetGates.Add(268, this.CreateExitGate(maps[51], 99, 55, 100, 57, 0, true));
        targetGates.Add(269, this.CreateExitGate(maps[51], 191, 148, 193, 150, 0, true));
        targetGates.Add(270, this.CreateExitGate(maps[new(32, 7)], 62, 150, 70, 158, 0, true));
        targetGates.Add(271, this.CreateExitGate(maps[52], 12, 5, 14, 10, 0, true));
        targetGates.Add(272, this.CreateExitGate(maps[53], 31, 88, 36, 95, 0, true));
        targetGates.Add(273, this.CreateExitGate(maps[56], 135, 105, 142, 111, 0, true));
        targetGates.Add(275, this.CreateExitGate(maps[56], 189, 190, 191, 193, 0, false));
        targetGates.Add(278, this.CreateExitGate(maps[56], 204, 10, 206, 14, 0, false));
        targetGates.Add(281, this.CreateExitGate(maps[56], 65, 47, 67, 48, 0, false));
        targetGates.Add(284, this.CreateExitGate(maps[56], 62, 174, 63, 179, 0, false));
        targetGates.Add(287, this.CreateExitGate(maps[57], 222, 211, 225, 212, 0, true));
        targetGates.Add(289, this.CreateExitGate(maps[2], 51, 85, 55, 86, 0, true));
        targetGates.Add(291, this.CreateExitGate(maps[58], 160, 24, 161, 27, 0, true));
        targetGates.Add(293, this.CreateExitGate(maps[57], 174, 23, 175, 25, 0, true));
        targetGates.Add(294, this.CreateExitGate(maps[63], 120, 129, 126, 134, 0, true));
        targetGates.Add(295, this.CreateExitGate(maps[64], 101, 64, 101, 64, 0, true));
        targetGates.Add(296, this.CreateExitGate(maps[64], 101, 75, 101, 75, 0, true));
        targetGates.Add(297, this.CreateExitGate(maps[64], 101, 113, 101, 113, 0, true));
        targetGates.Add(298, this.CreateExitGate(maps[64], 101, 124, 101, 124, 0, true));
        targetGates.Add(299, this.CreateExitGate(maps[64], 154, 64, 154, 64, 0, true));
        targetGates.Add(300, this.CreateExitGate(maps[64], 154, 75, 154, 75, 0, true));
        targetGates.Add(301, this.CreateExitGate(maps[64], 154, 113, 154, 113, 0, true));
        targetGates.Add(302, this.CreateExitGate(maps[64], 154, 124, 154, 124, 0, true));
        targetGates.Add(303, this.CreateExitGate(maps[64], 100, 70, 100, 70, 0, true));
        targetGates.Add(304, this.CreateExitGate(maps[64], 100, 120, 100, 120, 0, true));
        targetGates.Add(305, this.CreateExitGate(maps[64], 150, 70, 150, 70, 0, true));
        targetGates.Add(306, this.CreateExitGate(maps[64], 150, 120, 150, 120, 0, true));
        targetGates.Add(307, this.CreateExitGate(maps[69], 231, 15, 233, 17, 0, true));
        targetGates.Add(309, this.CreateExitGate(maps[69], 202, 24, 203, 27, 0, true));
        targetGates.Add(311, this.CreateExitGate(maps[69], 179, 65, 181, 67, 0, true));
        targetGates.Add(312, this.CreateExitGate(maps[70], 86, 63, 87, 66, 0, true));
        targetGates.Add(314, this.CreateExitGate(maps[70], 35, 84, 38, 85, 0, true));
        targetGates.Add(316, this.CreateExitGate(maps[70], 121, 110, 123, 112, 0, true));
        targetGates.Add(317, this.CreateExitGate(maps[71], 154, 187, 155, 189, 0, true));
        targetGates.Add(319, this.CreateExitGate(maps[71], 222, 121, 224, 123, 0, true));
        targetGates.Add(321, this.CreateExitGate(maps[71], 165, 206, 168, 207, 0, true));
        targetGates.Add(322, this.CreateExitGate(maps[72], 93, 66, 94, 69, 0, true));
        targetGates.Add(324, this.CreateExitGate(maps[72], 32, 162, 34, 164, 0, true));
        targetGates.Add(326, this.CreateExitGate(maps[72], 145, 155, 147, 157, 0, true));
        targetGates.Add(328, this.CreateExitGate(maps[72], 241, 23, 243, 25, 0, true));
        targetGates.Add(329, this.CreateExitGate(maps[65], 193, 26, 200, 32, 0, true));
        targetGates.Add(330, this.CreateExitGate(maps[66], 133, 68, 139, 74, 0, true));
        targetGates.Add(331, this.CreateExitGate(maps[67], 106, 58, 111, 62, 0, true));
        targetGates.Add(332, this.CreateExitGate(maps[68], 90, 10, 97, 17, 0, true));
        targetGates.Add(333, this.CreateExitGate(maps[79], 56, 74, 58, 77, 0, true));
        targetGates.Add(334, this.CreateExitGate(maps[37], 66, 183, 74, 191, 0, true));
        targetGates.Add(335, this.CreateExitGate(maps[80], 124, 123, 127, 125, 0, true));
        targetGates.Add(337, this.CreateExitGate(maps[80], 118, 44, 119, 46, 3, false));
        targetGates.Add(339, this.CreateExitGate(maps[33], 237, 166, 240, 166, 1, false));
        targetGates.Add(341, this.CreateExitGate(maps[81], 162, 12, 164, 14, 5, false));
        targetGates.Add(343, this.CreateExitGate(maps[80], 188, 207, 189, 208, 1, false));
        targetGates.Add(344, this.CreateExitGate(maps[81], 162, 16, 163, 17, 5, true));
        targetGates.Add(345, this.CreateExitGate(maps[82], 60, 66, 63, 69, 0, true));
        targetGates.Add(346, this.CreateExitGate(maps[82], 69, 195, 72, 198, 0, true));
        targetGates.Add(347, this.CreateExitGate(maps[82], 196, 187, 199, 190, 0, true));
        targetGates.Add(348, this.CreateExitGate(maps[82], 187, 61, 190, 64, 0, true));
        targetGates.Add(349, this.CreateExitGate(maps[83], 60, 66, 63, 69, 0, true));
        targetGates.Add(350, this.CreateExitGate(maps[83], 69, 195, 72, 198, 0, true));
        targetGates.Add(351, this.CreateExitGate(maps[83], 196, 187, 199, 190, 0, true));
        targetGates.Add(352, this.CreateExitGate(maps[83], 187, 61, 190, 64, 0, true));
        targetGates.Add(353, this.CreateExitGate(maps[84], 60, 66, 63, 69, 0, true));
        targetGates.Add(354, this.CreateExitGate(maps[84], 69, 195, 72, 198, 0, true));
        targetGates.Add(355, this.CreateExitGate(maps[84], 196, 187, 199, 190, 0, true));
        targetGates.Add(356, this.CreateExitGate(maps[84], 187, 61, 190, 64, 0, true));
        targetGates.Add(357, this.CreateExitGate(maps[85], 60, 66, 63, 69, 0, true));
        targetGates.Add(358, this.CreateExitGate(maps[85], 69, 195, 72, 198, 0, true));
        targetGates.Add(359, this.CreateExitGate(maps[85], 196, 187, 199, 190, 0, true));
        targetGates.Add(360, this.CreateExitGate(maps[85], 187, 61, 190, 64, 0, true));
        targetGates.Add(361, this.CreateExitGate(maps[86], 60, 66, 63, 69, 0, true));
        targetGates.Add(362, this.CreateExitGate(maps[86], 69, 195, 72, 198, 0, true));
        targetGates.Add(363, this.CreateExitGate(maps[86], 196, 187, 199, 190, 0, true));
        targetGates.Add(364, this.CreateExitGate(maps[86], 187, 61, 190, 64, 0, true));
        targetGates.Add(365, this.CreateExitGate(maps[87], 60, 66, 63, 69, 0, true));
        targetGates.Add(366, this.CreateExitGate(maps[87], 69, 195, 72, 198, 0, true));
        targetGates.Add(367, this.CreateExitGate(maps[87], 196, 187, 199, 190, 0, true));
        targetGates.Add(368, this.CreateExitGate(maps[87], 187, 61, 190, 64, 0, true));
        targetGates.Add(369, this.CreateExitGate(maps[88], 60, 66, 63, 69, 0, true));
        targetGates.Add(370, this.CreateExitGate(maps[88], 69, 195, 72, 198, 0, true));
        targetGates.Add(371, this.CreateExitGate(maps[88], 196, 187, 199, 190, 0, true));
        targetGates.Add(372, this.CreateExitGate(maps[88], 187, 61, 190, 64, 0, true));
        targetGates.Add(373, this.CreateExitGate(maps[89], 60, 66, 63, 69, 0, true));
        targetGates.Add(374, this.CreateExitGate(maps[89], 69, 195, 72, 198, 0, true));
        targetGates.Add(375, this.CreateExitGate(maps[89], 196, 187, 199, 190, 0, true));
        targetGates.Add(376, this.CreateExitGate(maps[89], 187, 61, 190, 64, 0, true));
        targetGates.Add(377, this.CreateExitGate(maps[90], 60, 66, 63, 69, 0, true));
        targetGates.Add(378, this.CreateExitGate(maps[90], 69, 195, 72, 198, 0, true));
        targetGates.Add(379, this.CreateExitGate(maps[90], 196, 187, 199, 190, 0, true));
        targetGates.Add(380, this.CreateExitGate(maps[90], 187, 61, 190, 64, 0, true));
        targetGates.Add(381, this.CreateExitGate(maps[82], 123, 120, 129, 134, 0, true));
        targetGates.Add(382, this.CreateExitGate(maps[82], 123, 126, 134, 132, 0, true));
        targetGates.Add(383, this.CreateExitGate(maps[82], 127, 121, 133, 133, 0, true));
        targetGates.Add(384, this.CreateExitGate(maps[82], 123, 121, 133, 128, 0, true));
        targetGates.Add(385, this.CreateExitGate(maps[83], 123, 120, 129, 134, 0, true));
        targetGates.Add(386, this.CreateExitGate(maps[83], 123, 126, 134, 132, 0, true));
        targetGates.Add(387, this.CreateExitGate(maps[83], 127, 121, 133, 133, 0, true));
        targetGates.Add(388, this.CreateExitGate(maps[83], 123, 121, 133, 128, 0, true));
        targetGates.Add(389, this.CreateExitGate(maps[84], 123, 120, 129, 134, 0, true));
        targetGates.Add(390, this.CreateExitGate(maps[84], 123, 126, 134, 132, 0, true));
        targetGates.Add(391, this.CreateExitGate(maps[84], 127, 121, 133, 133, 0, true));
        targetGates.Add(392, this.CreateExitGate(maps[84], 123, 121, 133, 128, 0, true));
        targetGates.Add(393, this.CreateExitGate(maps[85], 123, 120, 129, 134, 0, true));
        targetGates.Add(394, this.CreateExitGate(maps[85], 123, 126, 134, 132, 0, true));
        targetGates.Add(395, this.CreateExitGate(maps[85], 127, 121, 133, 133, 0, true));
        targetGates.Add(396, this.CreateExitGate(maps[85], 123, 121, 133, 128, 0, true));
        targetGates.Add(397, this.CreateExitGate(maps[86], 123, 120, 129, 134, 0, true));
        targetGates.Add(398, this.CreateExitGate(maps[86], 123, 126, 134, 132, 0, true));
        targetGates.Add(399, this.CreateExitGate(maps[86], 127, 121, 133, 133, 0, true));
        targetGates.Add(400, this.CreateExitGate(maps[86], 123, 121, 133, 128, 0, true));
        targetGates.Add(401, this.CreateExitGate(maps[87], 123, 120, 129, 134, 0, true));
        targetGates.Add(402, this.CreateExitGate(maps[87], 123, 126, 134, 132, 0, true));
        targetGates.Add(403, this.CreateExitGate(maps[87], 127, 121, 133, 133, 0, true));
        targetGates.Add(404, this.CreateExitGate(maps[87], 123, 121, 133, 128, 0, true));
        targetGates.Add(405, this.CreateExitGate(maps[88], 123, 120, 129, 134, 0, true));
        targetGates.Add(406, this.CreateExitGate(maps[88], 123, 126, 134, 132, 0, true));
        targetGates.Add(407, this.CreateExitGate(maps[88], 127, 121, 133, 133, 0, true));
        targetGates.Add(408, this.CreateExitGate(maps[88], 123, 121, 133, 128, 0, true));
        targetGates.Add(409, this.CreateExitGate(maps[89], 123, 120, 129, 134, 0, true));
        targetGates.Add(410, this.CreateExitGate(maps[89], 123, 126, 134, 132, 0, true));
        targetGates.Add(411, this.CreateExitGate(maps[89], 127, 121, 133, 133, 0, true));
        targetGates.Add(412, this.CreateExitGate(maps[89], 123, 121, 133, 128, 0, true));
        targetGates.Add(413, this.CreateExitGate(maps[90], 123, 120, 129, 134, 0, true));
        targetGates.Add(414, this.CreateExitGate(maps[90], 123, 126, 134, 132, 0, true));
        targetGates.Add(415, this.CreateExitGate(maps[90], 127, 121, 133, 133, 0, true));
        targetGates.Add(416, this.CreateExitGate(maps[90], 123, 121, 133, 128, 0, true));
        targetGates.Add(417, this.CreateExitGate(maps[91], 51, 190, 60, 197, 0, true));
        targetGates.Add(419, this.CreateExitGate(maps[91], 112, 161, 114, 166, 0, false));
        targetGates.Add(421, this.CreateExitGate(maps[91], 33, 167, 37, 170, 0, false));
        targetGates.Add(423, this.CreateExitGate(maps[91], 151, 198, 156, 200, 0, false));
        targetGates.Add(425, this.CreateExitGate(maps[91], 81, 197, 83, 199, 0, false));
        targetGates.Add(426, this.CreateExitGate(maps[92], 51, 190, 60, 197, 0, true));
        targetGates.Add(428, this.CreateExitGate(maps[92], 112, 161, 114, 166, 0, false));
        targetGates.Add(430, this.CreateExitGate(maps[92], 33, 167, 37, 170, 0, false));
        targetGates.Add(432, this.CreateExitGate(maps[92], 151, 198, 156, 200, 0, false));
        targetGates.Add(434, this.CreateExitGate(maps[92], 81, 197, 83, 199, 0, false));
        targetGates.Add(436, this.CreateExitGate(maps[95], 19, 104, 20, 105, 1, false));
        targetGates.Add(438, this.CreateExitGate(maps[91], 54, 163, 55, 164, 5, false));
        targetGates.Add(440, this.CreateExitGate(maps[96], 19, 104, 20, 105, 1, false));
        targetGates.Add(442, this.CreateExitGate(maps[92], 54, 163, 55, 164, 5, false));
        targetGates.Add(443, this.CreateExitGate(maps[97], 31, 88, 36, 95, 0, true));
        targetGates.Add(444, this.CreateExitGate(maps[98], 98, 128, 108, 137, 0, true));
        targetGates.Add(445, this.CreateExitGate(maps[99], 98, 128, 108, 137, 0, true));
        targetGates.Add(446, this.CreateExitGate(maps[98], 141, 41, 146, 45, 0, true));
        targetGates.Add(447, this.CreateExitGate(maps[99], 141, 41, 146, 45, 0, true));
        targetGates.Add(448, this.CreateExitGate(maps[98], 194, 124, 198, 127, 0, true));
        targetGates.Add(449, this.CreateExitGate(maps[99], 194, 124, 198, 127, 0, true));
        targetGates.Add(451, this.CreateExitGate(maps[100], 24, 86, 25, 90, 5, false));
        targetGates.Add(453, this.CreateExitGate(maps[91], 62, 213, 64, 214, 3, false));
        targetGates.Add(455, this.CreateExitGate(maps[100], 212, 31, 213, 32, 0, false));
        targetGates.Add(457, this.CreateExitGate(maps[100], 40, 82, 41, 84, 0, false));
        targetGates.Add(459, this.CreateExitGate(maps[100], 107, 225, 109, 226, 0, false));
        targetGates.Add(461, this.CreateExitGate(maps[100], 39, 94, 40, 95, 0, false));
        targetGates.Add(474, this.CreateExitGate(maps[102], 125, 126, 130, 130, 0, true));
        targetGates.Add(476, this.CreateExitGate(maps[1], 45, 152, 48, 155, 0, false));
        targetGates.Add(478, this.CreateExitGate(maps[1], 112, 232, 113, 234, 0, false));
        targetGates.Add(480, this.CreateExitGate(maps[1], 241, 85, 242, 89, 0, false));
        targetGates.Add(482, this.CreateExitGate(maps[1], 229, 108, 231, 110, 0, false));
        targetGates.Add(483, this.CreateExitGate(maps[103], 125, 126, 130, 130, 0, true));
        targetGates.Add(484, this.CreateExitGate(maps[104], 125, 126, 130, 130, 0, true));
        targetGates.Add(485, this.CreateExitGate(maps[105], 125, 126, 130, 130, 0, true));
        targetGates.Add(486, this.CreateExitGate(maps[106], 125, 126, 130, 130, 0, true));
        targetGates.Add(488, this.CreateExitGate(maps[110], 213, 193, 214, 196, 5, false));
        targetGates.Add(490, this.CreateExitGate(maps[91], 94, 184, 95, 187, 3, false));
        targetGates.Add(492, this.CreateExitGate(maps[110], 133, 166, 136, 167, 0, false));
        targetGates.Add(494, this.CreateExitGate(maps[110], 213, 193, 214, 196, 0, false));
        targetGates.Add(496, this.CreateExitGate(maps[110], 127, 29, 128, 30, 0, false));
        targetGates.Add(498, this.CreateExitGate(maps[110], 181, 188, 183, 188, 0, false));
        targetGates.Add(500, this.CreateExitGate(maps[110], 34, 198, 35, 199, 0, false));
        targetGates.Add(502, this.CreateExitGate(maps[110], 181, 208, 183, 209, 0, false));
        targetGates.Add(503, this.CreateExitGate(maps[79], 203, 56, 207, 59, 0, true));
        targetGates.Add(504, this.CreateExitGate(maps[79], 188, 90, 192, 91, 0, true));
        targetGates.Add(505, this.CreateExitGate(maps[79], 221, 91, 227, 92, 0, true));
        targetGates.Add(506, this.CreateExitGate(maps[79], 191, 20, 194, 23, 0, true));
        targetGates.Add(507, this.CreateExitGate(maps[79], 170, 55, 173, 59, 0, true));
        targetGates.Add(508, this.CreateExitGate(maps[79], 230, 28, 236, 29, 0, true));
        targetGates.Add(509, this.CreateExitGate(maps[112], 236, 152, 237, 153, 0, true));
        targetGates.Add(511, this.CreateExitGate(maps[112], 236, 152, 237, 153, 0, false));
        targetGates.Add(513, this.CreateExitGate(maps[112], 107, 67, 108, 68, 0, false));
        targetGates.Add(515, this.CreateExitGate(maps[112], 201, 155, 202, 156, 0, false));
        targetGates.Add(517, this.CreateExitGate(maps[112], 82, 226, 83, 227, 0, false));
        targetGates.Add(519, this.CreateExitGate(maps[112], 201, 155, 202, 156, 0, false));
        targetGates.Add(521, this.CreateExitGate(maps[8], 187, 54, 189, 55, 0, false));
        targetGates.Add(522, this.CreateExitGate(maps[113], 24, 118, 27, 121, 0, true));
        targetGates.Add(524, this.CreateExitGate(maps[113], 24, 118, 27, 121, 1, false));
        targetGates.Add(526, this.CreateExitGate(maps[57], 214, 203, 218, 206, 3, false));
        targetGates.Add(528, this.CreateExitGate(maps[113], 149, 85, 152, 88, 0, false));
        targetGates.Add(530, this.CreateExitGate(maps[113], 60, 108, 62, 112, 0, false));
        targetGates.Add(531, this.CreateExitGate(maps[113], 22, 228, 27, 229, 0, true));
        targetGates.Add(533, this.CreateExitGate(maps[114], 232, 56, 234, 59, 0, false));
        targetGates.Add(535, this.CreateExitGate(maps[79], 223, 56, 224, 59, 0, false));
        targetGates.Add(537, this.CreateExitGate(maps[114], 238, 237, 239, 238, 0, true));
        targetGates.Add(540, this.CreateExitGate(maps[121], 147, 27, 148, 28, 0, true));
        targetGates.Add(542, this.CreateExitGate(maps[116], 120, 125, 121, 128, 0, false));
        targetGates.Add(544, this.CreateExitGate(maps[0], 179, 30, 181, 31, 0, false));
        targetGates.Add(546, this.CreateExitGate(maps[117], 131, 128, 132, 129, 0, false));
        targetGates.Add(548, this.CreateExitGate(maps[116], 128, 127, 129, 130, 0, false));
        targetGates.Add(550, this.CreateExitGate(maps[118], 123, 123, 124, 124, 0, false));
        targetGates.Add(552, this.CreateExitGate(maps[117], 131, 127, 132, 128, 0, false));
        targetGates.Add(554, this.CreateExitGate(maps[119], 125, 119, 126, 120, 0, false));
        targetGates.Add(556, this.CreateExitGate(maps[118], 135, 127, 136, 128, 0, false));
        targetGates.Add(558, this.CreateExitGate(maps[120], 126, 119, 127, 120, 0, false));
        targetGates.Add(560, this.CreateExitGate(maps[119], 136, 127, 137, 128, 0, false));
        targetGates.Add(561, this.CreateExitGate(maps[116], 120, 125, 121, 128, 0, true));
        targetGates.Add(562, this.CreateExitGate(maps[117], 131, 128, 132, 129, 0, true));
        targetGates.Add(563, this.CreateExitGate(maps[118], 123, 123, 124, 124, 0, true));
        targetGates.Add(564, this.CreateExitGate(maps[119], 125, 119, 126, 120, 0, true));
        targetGates.Add(565, this.CreateExitGate(maps[120], 126, 119, 127, 120, 0, true));
        targetGates.Add(566, this.CreateExitGate(maps[0], 138, 120, 139, 121, 0, true));
        targetGates.Add(567, this.CreateExitGate(maps[122], 128, 110, 140, 125, 0, true));
        targetGates.Add(569, this.CreateExitGate(maps[122], 103, 159, 104, 160, 0, false));
        targetGates.Add(571, this.CreateExitGate(maps[122], 128, 125, 129, 126, 0, false));
        targetGates.Add(573, this.CreateExitGate(maps[122], 182, 102, 183, 103, 0, false));
        targetGates.Add(575, this.CreateExitGate(maps[122], 142, 112, 143, 113, 0, false));
        targetGates.Add(577, this.CreateExitGate(maps[122], 132, 68, 133, 69, 0, false));
        targetGates.Add(579, this.CreateExitGate(maps[122], 133, 107, 134, 108, 0, false));
        targetGates.Add(581, this.CreateExitGate(maps[122], 88, 104, 89, 105, 0, false));
        targetGates.Add(583, this.CreateExitGate(maps[122], 125, 114, 126, 115, 0, false));
        targetGates.Add(585, this.CreateExitGate(maps[122], 160, 152, 161, 153, 0, false));
        targetGates.Add(587, this.CreateExitGate(maps[122], 139, 125, 140, 126, 0, false));
        targetGates.Add(590, this.CreateExitGate(maps[122], 192, 9, 193, 11, 0, true));
        targetGates.Add(591, this.CreateExitGate(maps[123], 236, 87, 239, 91, 0, true));
        targetGates.Add(593, this.CreateExitGate(maps[123], 204, 64, 208, 67, 0, false));
        targetGates.Add(595, this.CreateExitGate(maps[123], 220, 107, 221, 111, 0, false));
        targetGates.Add(597, this.CreateExitGate(maps[126], 204, 64, 208, 67, 0, false));
        targetGates.Add(599, this.CreateExitGate(maps[123], 222, 111, 225, 113, 0, false));
        targetGates.Add(601, this.CreateExitGate(maps[124], 204, 64, 208, 67, 0, false));
        targetGates.Add(603, this.CreateExitGate(maps[123], 226, 117, 230, 119, 0, false));
        targetGates.Add(605, this.CreateExitGate(maps[127], 204, 64, 208, 67, 0, false));
        targetGates.Add(607, this.CreateExitGate(maps[123], 233, 112, 236, 124, 0, false));
        targetGates.Add(609, this.CreateExitGate(maps[125], 204, 64, 208, 67, 0, false));
        targetGates.Add(611, this.CreateExitGate(maps[123], 236, 124, 239, 125, 0, false));
        targetGates.Add(613, this.CreateExitGate(maps[123], 236, 87, 239, 91, 3, false));
        targetGates.Add(615, this.CreateExitGate(maps[2], 163, 54, 165, 59, 3, false));
        targetGates.Add(616, this.CreateExitGate(maps[123], 224, 111, 231, 115, 0, true));
        targetGates.Add(617, this.CreateExitGate(maps[128], 129, 126, 132, 129, 0, true));
        targetGates.Add(618, this.CreateExitGate(maps[129], 128, 124, 131, 127, 0, true));
        targetGates.Add(619, this.CreateExitGate(maps[130], 128, 122, 131, 126, 0, true));
        targetGates.Add(620, this.CreateExitGate(maps[131], 236, 13, 242, 16, 0, true));
        targetGates.Add(622, this.CreateExitGate(maps[131], 80, 74, 82, 77, 0, false));
        targetGates.Add(624, this.CreateExitGate(maps[131], 197, 29, 200, 30, 0, false));
        targetGates.Add(626, this.CreateExitGate(maps[131], 141, 203, 145, 204, 0, false));
        targetGates.Add(628, this.CreateExitGate(maps[131], 207, 38, 210, 38, 0, false));
        targetGates.Add(630, this.CreateExitGate(maps[131], 240, 19, 244, 21, 0, false));
        targetGates.Add(631, this.CreateExitGate(maps[132], 123, 114, 132, 128, 0, true));
        targetGates.Add(633, this.CreateExitGate(maps[132], 123, 114, 132, 128, 0, false));
        targetGates.Add(634, this.CreateExitGate(maps[133], 125, 57, 132, 59, 0, true));
        targetGates.Add(636, this.CreateExitGate(maps[133], 125, 57, 132, 59, 0, false));
        targetGates.Add(638, this.CreateExitGate(maps[51], 218, 118, 223, 119, 0, false));
        targetGates.Add(640, this.CreateExitGate(maps[133], 120, 185, 131, 186, 0, false));
        targetGates.Add(642, this.CreateExitGate(maps[133], 127, 26, 132, 28, 0, false));
        targetGates.Add(643, this.CreateExitGate(maps[134], 122, 29, 128, 31, 0, true));
        targetGates.Add(645, this.CreateExitGate(maps[134], 122, 29, 128, 31, 0, false));
        targetGates.Add(647, this.CreateExitGate(maps[33], 115, 108, 120, 109, 0, false));
        targetGates.Add(648, this.CreateExitGate(maps[135], 124, 17, 129, 18, 0, true));
        targetGates.Add(650, this.CreateExitGate(maps[135], 124, 17, 129, 18, 0, false));
        targetGates.Add(652, this.CreateExitGate(maps[135], 103, 166, 106, 167, 0, false));
        targetGates.Add(654, this.CreateExitGate(maps[135], 112, 56, 118, 58, 0, false));
        targetGates.Add(656, this.CreateExitGate(maps[135], 210, 132, 211, 136, 0, false));
        targetGates.Add(658, this.CreateExitGate(maps[135], 136, 56, 142, 59, 0, false));
        targetGates.Add(659, this.CreateExitGate(maps[136], 124, 17, 129, 19, 0, true));
        targetGates.Add(661, this.CreateExitGate(maps[136], 124, 17, 129, 19, 0, false));
        targetGates.Add(663, this.CreateExitGate(maps[136], 103, 166, 106, 167, 0, false));
        targetGates.Add(665, this.CreateExitGate(maps[136], 112, 56, 118, 58, 0, false));
        targetGates.Add(667, this.CreateExitGate(maps[136], 210, 132, 211, 136, 0, false));
        targetGates.Add(669, this.CreateExitGate(maps[136], 136, 58, 140, 59, 0, false));
        targetGates.Add(670, this.CreateExitGate(maps[137], 129, 120, 130, 130, 0, true));
        targetGates.Add(672, this.CreateExitGate(maps[137], 129, 120, 130, 130, 0, false));
        targetGates.Add(674, this.CreateExitGate(maps[38], 106, 157, 110, 159, 0, false));
        targetGates.Add(675, this.CreateExitGate(maps[138], 122, 119, 129, 127, 0, true));
        targetGates.Add(677, this.CreateExitGate(maps[138], 122, 119, 129, 127, 0, false));
        targetGates.Add(679, this.CreateExitGate(maps[63], 40, 14, 43, 20, 0, false));
        targetGates.Add(680, this.CreateExitGate(maps[114], 169, 236, 174, 237, 0, true));
        targetGates.Add(681, this.CreateExitGate(maps[139], 122, 111, 134, 111, 0, true));
        targetGates.Add(682, this.CreateExitGate(maps[140], 127, 28, 145, 47, 0, true));
        targetGates.Add(684, this.CreateExitGate(maps[140], 133, 24, 141, 31, 0, false));
        targetGates.Add(686, this.CreateExitGate(maps[8], 101, 231, 117, 236, 0, false));
        targetGates.Add(688, this.CreateExitGate(maps[140], 160, 159, 167, 162, 0, false));
        targetGates.Add(690, this.CreateExitGate(maps[140], 139, 54, 140, 62, 0, false));
        targetGates.Add(691, this.CreateExitGate(maps[114], 116, 236, 120, 238, 0, true));
        targetGates.Add(692, this.CreateExitGate(maps[121], 101, 27, 105, 28, 0, true));
        targetGates.Add(693, this.CreateExitGate(maps[2], 192, 16, 194, 20, 0, true));
        targetGates.Add(694, this.CreateExitGate(maps[141], 127, 119, 132, 124, 0, true));
        targetGates.Add(696, this.CreateExitGate(maps[141], 127, 119, 132, 124, 0, false));
        targetGates.Add(697, this.CreateExitGate(maps[79], 59, 232, 65, 234, 0, true));
        targetGates.Add(698, this.CreateExitGate(maps[79], 23, 233, 27, 235, 0, true));
        targetGates.Add(699, this.CreateExitGate(maps[142], 119, 120, 128, 121, 0, true));
        targetGates.Add(701, this.CreateExitGate(maps[142], 113, 117, 116, 117, 0, false));
        targetGates.Add(703, this.CreateExitGate(maps[81], 74, 63, 79, 63, 0, false));
        targetGates.Add(704, this.CreateExitGate(maps[143], 119, 16, 127, 24, 0, true));
        targetGates.Add(706, this.CreateExitGate(maps[143], 107, 9, 108, 13, 0, false));
        targetGates.Add(708, this.CreateExitGate(maps[143], 110, 147, 113, 150, 0, false));
        targetGates.Add(710, this.CreateExitGate(maps[143], 122, 40, 126, 42, 0, false));
        targetGates.Add(712, this.CreateExitGate(maps[80], 214, 18, 216, 20, 0, false));
        targetGates.Add(713, this.CreateExitGate(maps[144], 107, 121, 116, 134, 0, true));
        targetGates.Add(715, this.CreateExitGate(maps[144], 107, 121, 116, 134, 0, false));
        targetGates.Add(717, this.CreateExitGate(maps[56], 135, 105, 142, 111, 0, false));
        return targetGates;
    }

    private DuelConfiguration CreateDuelConfiguration(IDictionary<short, ExitGate> targetGates)
    {
        var duelConfig = this.Context.CreateNew<DuelConfiguration>();
        duelConfig.MaximumScore = 10;
        duelConfig.MinimumCharacterLevel = 30;
        duelConfig.EntranceFee = 30000;
        duelConfig.Exit = targetGates[294]; // Vulcanus, see above

        List<(short FirstPlayerGate, short SecondPlayerGate, short SpectatorGate)> duelGateNumbers =
        [
            (295, 296, 303),
            (297, 298, 304),
            (299, 300, 305),
            (301, 302, 306),
        ];

        for (short i = 0; i < duelGateNumbers.Count; i++)
        {
            var indices = duelGateNumbers[i];
            var duelArea = this.Context.CreateNew<DuelArea>();
            duelArea.Index = i;
            duelArea.FirstPlayerGate = targetGates[indices.FirstPlayerGate];
            duelArea.SecondPlayerGate = targetGates[indices.SecondPlayerGate];
            duelArea.SpectatorsGate = targetGates[indices.SpectatorGate];
            duelConfig.DuelAreas.Add(duelArea);
        }

        return duelConfig;
    }

    private EnterGate CreateEnterGate(short number, ExitGate targetGate, byte x1, byte y1, byte x2, byte y2, short levelRequirement)
    {
        var enterGate = this.Context.CreateNew<EnterGate>();
        enterGate.Number = number;
        enterGate.LevelRequirement = levelRequirement;
        enterGate.TargetGate = targetGate;
        enterGate.X1 = x1;
        enterGate.Y1 = y1;
        enterGate.X2 = x2;
        enterGate.Y2 = y2;
        enterGate.LevelRequirement = levelRequirement;
        return enterGate;
    }

    /// <summary>
    /// Creates the enter gates for all maps.
    /// </summary>
    /// <remarks>
    /// vscode Regex: ^(\d+)\s+?(1)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(\d+)\s+?(-?\d+)\s+(-?\d+)
    /// Replace by #2: maps[$3].EnterGates.Add(this.CreateEnterGate($1, targetGates[$8], $4, $5, $6, $7, $10));
    /// Remove other lines with empty replacement and regex: (?m)^\d+\s+.*$.
    /// </remarks>
    /// <param name="maps">The maps.</param>
    /// <param name="targetGates">The target gates by number.</param>
    private void CreateEnterGates(IDictionary<MapIdentity, GameMapDefinition> maps, IDictionary<short, ExitGate> targetGates)
    {
        maps[0].EnterGates.Add(this.CreateEnterGate(1, targetGates[2], 121, 232, 123, 233, 20));
        maps[1].EnterGates.Add(this.CreateEnterGate(3, targetGates[4], 108, 248, 109, 248, 0));
        maps[1].EnterGates.Add(this.CreateEnterGate(5, targetGates[6], 239, 149, 239, 150, 25));
        maps[1].EnterGates.Add(this.CreateEnterGate(7, targetGates[8], 232, 127, 233, 128, 20));
        maps[1].EnterGates.Add(this.CreateEnterGate(9, targetGates[10], 2, 17, 2, 18, 30));
        maps[1].EnterGates.Add(this.CreateEnterGate(11, targetGates[12], 2, 84, 2, 85, 25));
        maps[1].EnterGates.Add(this.CreateEnterGate(13, targetGates[14], 5, 34, 6, 34, 30));
        maps[1].EnterGates.Add(this.CreateEnterGate(15, targetGates[16], 29, 127, 30, 127, 25));
        maps[0].EnterGates.Add(this.CreateEnterGate(18, targetGates[19], 5, 38, 6, 41, 10));
        maps[2].EnterGates.Add(this.CreateEnterGate(20, targetGates[21], 244, 34, 245, 37, 10));
        maps[0].EnterGates.Add(this.CreateEnterGate(23, targetGates[24], 213, 246, 217, 247, 10));
        maps[3].EnterGates.Add(this.CreateEnterGate(25, targetGates[26], 150, 6, 153, 7, 10));
        maps[2].EnterGates.Add(this.CreateEnterGate(28, targetGates[29], 4, 247, 5, 248, 40));
        maps[4].EnterGates.Add(this.CreateEnterGate(30, targetGates[31], 190, 6, 191, 8, 40));
        maps[4].EnterGates.Add(this.CreateEnterGate(32, targetGates[33], 166, 163, 167, 166, 40));
        maps[4].EnterGates.Add(this.CreateEnterGate(34, targetGates[35], 132, 245, 135, 246, 50));
        maps[4].EnterGates.Add(this.CreateEnterGate(36, targetGates[37], 132, 135, 135, 136, 50));
        maps[4].EnterGates.Add(this.CreateEnterGate(38, targetGates[39], 131, 15, 132, 18, 70));
        maps[4].EnterGates.Add(this.CreateEnterGate(40, targetGates[41], 6, 5, 7, 8, 70));
        maps[4].EnterGates.Add(this.CreateEnterGate(43, targetGates[44], 162, 0, 166, 1, 10));
        maps[3].EnterGates.Add(this.CreateEnterGate(45, targetGates[46], 242, 240, 245, 243, 50));
        maps[7].EnterGates.Add(this.CreateEnterGate(47, targetGates[48], 9, 9, 11, 12, 50));
        maps[7].EnterGates.Add(this.CreateEnterGate(53, targetGates[54], 14, 225, 15, 230, 130));
        maps[8].EnterGates.Add(this.CreateEnterGate(55, targetGates[56], 246, 40, 247, 44, 130));
        maps[4].EnterGates.Add(this.CreateEnterGate(62, targetGates[63], 17, 250, 19, 250, 160));
        maps[10].EnterGates.Add(this.CreateEnterGate(64, targetGates[65], 14, 12, 16, 12, 70));
        maps[30].EnterGates.Add(this.CreateEnterGate(96, targetGates[97], 93, 242, 95, 243, 0));
        maps[30].EnterGates.Add(this.CreateEnterGate(98, targetGates[99], 160, 203, 161, 205, 0));
        maps[31].EnterGates.Add(this.CreateEnterGate(109, targetGates[110], 59, 7, 63, 8, 0));
        maps[0].EnterGates.Add(this.CreateEnterGate(102, targetGates[103], 239, 14, 240, 15, 10));
        maps[30].EnterGates.Add(this.CreateEnterGate(107, targetGates[108], 28, 40, 28, 41, 0));
        maps[3].EnterGates.Add(this.CreateEnterGate(120, targetGates[113], 220, 30, 226, 30, 150));
        maps[33].EnterGates.Add(this.CreateEnterGate(121, targetGates[122], 74, 9, 74, 13, 10));
        maps[30].EnterGates.Add(this.CreateEnterGate(117, targetGates[114], 161, 37, 165, 45, 10));
        maps[34].EnterGates.Add(this.CreateEnterGate(123, targetGates[124], 239, 40, 240, 44, 10));
        maps[8].EnterGates.Add(this.CreateEnterGate(125, targetGates[126], 6, 199, 6, 201, 160));
        maps[37].EnterGates.Add(this.CreateEnterGate(127, targetGates[128], 17, 220, 19, 222, 130));
        maps[37].EnterGates.Add(this.CreateEnterGate(129, targetGates[130], 89, 89, 89, 92, 300));
        maps[38].EnterGates.Add(this.CreateEnterGate(131, targetGates[132], 69, 104, 69, 107, 160));
        maps[2].EnterGates.Add(this.CreateEnterGate(259, targetGates[260], 161, 245, 166, 246, 10));
        maps[51].EnterGates.Add(this.CreateEnterGate(261, targetGates[262], 24, 29, 25, 30, 10));
        maps[7].EnterGates.Add(this.CreateEnterGate(263, targetGates[264], 13, 19, 14, 20, 10));
        maps[51].EnterGates.Add(this.CreateEnterGate(265, targetGates[266], 247, 149, 248, 150, 50));
        maps[56].EnterGates.Add(this.CreateEnterGate(274, targetGates[275], 139, 125, 139, 126, 380));
        maps[56].EnterGates.Add(this.CreateEnterGate(276, targetGates[273], 185, 187, 186, 188, 380));
        maps[56].EnterGates.Add(this.CreateEnterGate(277, targetGates[278], 149, 109, 150, 109, 380));
        maps[56].EnterGates.Add(this.CreateEnterGate(279, targetGates[273], 197, 12, 197, 14, 380));
        maps[56].EnterGates.Add(this.CreateEnterGate(280, targetGates[281], 139, 95, 140, 95, 380));
        maps[56].EnterGates.Add(this.CreateEnterGate(282, targetGates[273], 68, 52, 69, 53, 380));
        maps[56].EnterGates.Add(this.CreateEnterGate(283, targetGates[284], 124, 109, 124, 110, 380));
        maps[56].EnterGates.Add(this.CreateEnterGate(285, targetGates[273], 57, 176, 57, 177, 380));
        maps[2].EnterGates.Add(this.CreateEnterGate(286, targetGates[287], 52, 90, 54, 91, 380));
        maps[57].EnterGates.Add(this.CreateEnterGate(288, targetGates[289], 223, 215, 225, 215, 380));
        maps[57].EnterGates.Add(this.CreateEnterGate(290, targetGates[291], 171, 23, 171, 25, 380));
        maps[58].EnterGates.Add(this.CreateEnterGate(292, targetGates[293], 167, 24, 167, 25, 380));
        maps[69].EnterGates.Add(this.CreateEnterGate(308, targetGates[309], 209, 80, 211, 82, 0));
        maps[69].EnterGates.Add(this.CreateEnterGate(310, targetGates[311], 153, 60, 155, 62, 0));
        maps[70].EnterGates.Add(this.CreateEnterGate(313, targetGates[314], 10, 64, 12, 66, 0));
        maps[70].EnterGates.Add(this.CreateEnterGate(315, targetGates[316], 54, 161, 56, 163, 0));
        maps[71].EnterGates.Add(this.CreateEnterGate(318, targetGates[319], 82, 194, 84, 196, 0));
        maps[71].EnterGates.Add(this.CreateEnterGate(320, targetGates[321], 222, 201, 224, 203, 0));
        maps[72].EnterGates.Add(this.CreateEnterGate(323, targetGates[324], 30, 95, 32, 97, 0));
        maps[72].EnterGates.Add(this.CreateEnterGate(325, targetGates[326], 68, 160, 70, 162, 0));
        maps[72].EnterGates.Add(this.CreateEnterGate(327, targetGates[328], 223, 165, 225, 167, 0));
        maps[33].EnterGates.Add(this.CreateEnterGate(336, targetGates[337], 237, 167, 240, 168, 280));
        maps[80].EnterGates.Add(this.CreateEnterGate(338, targetGates[339], 116, 44, 117, 47, 150));
        maps[80].EnterGates.Add(this.CreateEnterGate(340, targetGates[341], 186, 210, 190, 212, 380));
        maps[81].EnterGates.Add(this.CreateEnterGate(342, targetGates[343], 161, 8, 165, 9, 280));
        maps[91].EnterGates.Add(this.CreateEnterGate(418, targetGates[419], 26, 168, 28, 172, 300));
        maps[91].EnterGates.Add(this.CreateEnterGate(420, targetGates[421], 115, 168, 118, 170, 300));
        maps[91].EnterGates.Add(this.CreateEnterGate(422, targetGates[423], 82, 204, 86, 205, 300));
        maps[91].EnterGates.Add(this.CreateEnterGate(424, targetGates[425], 147, 202, 148, 206, 300));
        maps[92].EnterGates.Add(this.CreateEnterGate(427, targetGates[428], 26, 168, 28, 172, 0));
        maps[92].EnterGates.Add(this.CreateEnterGate(429, targetGates[430], 115, 168, 118, 170, 0));
        maps[92].EnterGates.Add(this.CreateEnterGate(431, targetGates[432], 82, 204, 86, 205, 0));
        maps[92].EnterGates.Add(this.CreateEnterGate(433, targetGates[434], 147, 202, 148, 206, 0));
        maps[91].EnterGates.Add(this.CreateEnterGate(435, targetGates[436], 52, 161, 53, 163, 300));
        maps[95].EnterGates.Add(this.CreateEnterGate(437, targetGates[438], 17, 106, 18, 107, 300));
        maps[92].EnterGates.Add(this.CreateEnterGate(439, targetGates[440], 52, 161, 53, 163, 300));
        maps[96].EnterGates.Add(this.CreateEnterGate(441, targetGates[442], 17, 106, 18, 107, 300));
        maps[91].EnterGates.Add(this.CreateEnterGate(450, targetGates[451], 65, 218, 67, 219, 300));
        maps[100].EnterGates.Add(this.CreateEnterGate(452, targetGates[453], 19, 87, 20, 88, 300));
        maps[100].EnterGates.Add(this.CreateEnterGate(454, targetGates[455], 42, 80, 44, 81, 300));
        maps[100].EnterGates.Add(this.CreateEnterGate(456, targetGates[457], 215, 33, 217, 34, 300));
        maps[100].EnterGates.Add(this.CreateEnterGate(458, targetGates[459], 40, 97, 42, 98, 300));
        maps[100].EnterGates.Add(this.CreateEnterGate(460, targetGates[461], 108, 229, 110, 230, 300));
        maps[1].EnterGates.Add(this.CreateEnterGate(475, targetGates[476], 115, 231, 117, 233, 0));
        maps[1].EnterGates.Add(this.CreateEnterGate(477, targetGates[478], 45, 148, 47, 150, 0));
        maps[1].EnterGates.Add(this.CreateEnterGate(479, targetGates[480], 233, 106, 237, 108, 0));
        maps[1].EnterGates.Add(this.CreateEnterGate(481, targetGates[482], 244, 86, 245, 89, 0));
        maps[91].EnterGates.Add(this.CreateEnterGate(487, targetGates[488], 90, 181, 91, 184, 300));
        maps[110].EnterGates.Add(this.CreateEnterGate(489, targetGates[490], 211, 190, 212, 191, 300));
        maps[110].EnterGates.Add(this.CreateEnterGate(491, targetGates[492], 179, 148, 180, 149, 300));
        maps[110].EnterGates.Add(this.CreateEnterGate(493, targetGates[494], 131, 170, 133, 171, 300));
        maps[110].EnterGates.Add(this.CreateEnterGate(495, targetGates[496], 179, 185, 180, 187, 300));
        maps[110].EnterGates.Add(this.CreateEnterGate(497, targetGates[498], 126, 33, 127, 34, 300));
        maps[110].EnterGates.Add(this.CreateEnterGate(499, targetGates[500], 179, 210, 180, 211, 300));
        maps[110].EnterGates.Add(this.CreateEnterGate(501, targetGates[502], 36, 195, 37, 196, 300));
        maps[8].EnterGates.Add(this.CreateEnterGate(510, targetGates[511], 176, 38, 178, 40, 400));
        maps[112].EnterGates.Add(this.CreateEnterGate(512, targetGates[513], 198, 173, 199, 175, 400));
        maps[112].EnterGates.Add(this.CreateEnterGate(514, targetGates[515], 80, 233, 81, 235, 400));
        maps[112].EnterGates.Add(this.CreateEnterGate(516, targetGates[517], 199, 137, 200, 138, 400));
        maps[112].EnterGates.Add(this.CreateEnterGate(518, targetGates[519], 100, 66, 101, 68, 400));
        maps[112].EnterGates.Add(this.CreateEnterGate(520, targetGates[521], 236, 175, 239, 176, 400));
        maps[57].EnterGates.Add(this.CreateEnterGate(523, targetGates[524], 208, 219, 211, 221, 630));
        maps[113].EnterGates.Add(this.CreateEnterGate(525, targetGates[526], 17, 118, 19, 121, 630));
        maps[113].EnterGates.Add(this.CreateEnterGate(527, targetGates[528], 62, 108, 67, 112, 630));
        maps[113].EnterGates.Add(this.CreateEnterGate(529, targetGates[530], 143, 89, 149, 91, 630));
        maps[79].EnterGates.Add(this.CreateEnterGate(532, targetGates[533], 227, 55, 227, 60, 0));
        maps[114].EnterGates.Add(this.CreateEnterGate(534, targetGates[535], 228, 56, 229, 59, 0));
        maps[0].EnterGates.Add(this.CreateEnterGate(541, targetGates[542], 176, 29, 178, 32, 730));
        maps[116].EnterGates.Add(this.CreateEnterGate(543, targetGates[544], 117, 124, 118, 125, 730));
        maps[116].EnterGates.Add(this.CreateEnterGate(545, targetGates[546], 130, 130, 131, 131, 750));
        maps[117].EnterGates.Add(this.CreateEnterGate(547, targetGates[548], 129, 131, 132, 132, 750));
        maps[117].EnterGates.Add(this.CreateEnterGate(549, targetGates[550], 116, 124, 117, 127, 770));
        maps[118].EnterGates.Add(this.CreateEnterGate(551, targetGates[552], 121, 118, 122, 120, 770));
        maps[118].EnterGates.Add(this.CreateEnterGate(553, targetGates[554], 135, 130, 136, 131, 780));
        maps[119].EnterGates.Add(this.CreateEnterGate(555, targetGates[556], 122, 117, 123, 119, 780));
        maps[119].EnterGates.Add(this.CreateEnterGate(557, targetGates[558], 136, 131, 137, 132, 780));
        maps[120].EnterGates.Add(this.CreateEnterGate(559, targetGates[560], 121, 118, 122, 120, 780));
        maps[122].EnterGates.Add(this.CreateEnterGate(568, targetGates[569], 126, 128, 127, 129, 800));
        maps[122].EnterGates.Add(this.CreateEnterGate(570, targetGates[571], 107, 154, 108, 155, 800));
        maps[122].EnterGates.Add(this.CreateEnterGate(572, targetGates[573], 148, 111, 148, 113, 800));
        maps[122].EnterGates.Add(this.CreateEnterGate(574, targetGates[575], 177, 102, 177, 104, 800));
        maps[122].EnterGates.Add(this.CreateEnterGate(576, targetGates[577], 132, 102, 133, 103, 800));
        maps[122].EnterGates.Add(this.CreateEnterGate(578, targetGates[579], 133, 73, 135, 73, 800));
        maps[122].EnterGates.Add(this.CreateEnterGate(580, targetGates[581], 120, 113, 121, 114, 800));
        maps[122].EnterGates.Add(this.CreateEnterGate(582, targetGates[583], 92, 105, 92, 107, 800));
        maps[122].EnterGates.Add(this.CreateEnterGate(584, targetGates[585], 142, 128, 144, 129, 800));
        maps[122].EnterGates.Add(this.CreateEnterGate(586, targetGates[587], 158, 150, 160, 151, 800));
        maps[122].EnterGates.Add(this.CreateEnterGate(588, targetGates[590], 241, 91, 242, 92, 800));
        maps[122].EnterGates.Add(this.CreateEnterGate(589, targetGates[590], 12, 158, 13, 158, 800));
        maps[123].EnterGates.Add(this.CreateEnterGate(592, targetGates[593], 215, 109, 216, 111, 850));
        maps[123].EnterGates.Add(this.CreateEnterGate(594, targetGates[595], 207, 67, 210, 68, 850));
        maps[123].EnterGates.Add(this.CreateEnterGate(596, targetGates[597], 217, 116, 220, 117, 850));
        maps[126].EnterGates.Add(this.CreateEnterGate(598, targetGates[599], 207, 67, 210, 68, 850));
        maps[123].EnterGates.Add(this.CreateEnterGate(600, targetGates[601], 223, 122, 226, 122, 850));
        maps[124].EnterGates.Add(this.CreateEnterGate(602, targetGates[603], 207, 67, 210, 68, 850));
        maps[123].EnterGates.Add(this.CreateEnterGate(604, targetGates[605], 228, 127, 231, 128, 850));
        maps[127].EnterGates.Add(this.CreateEnterGate(606, targetGates[607], 207, 67, 210, 68, 850));
        maps[123].EnterGates.Add(this.CreateEnterGate(608, targetGates[609], 235, 129, 237, 130, 850));
        maps[125].EnterGates.Add(this.CreateEnterGate(610, targetGates[611], 207, 67, 210, 68, 850));
        maps[2].EnterGates.Add(this.CreateEnterGate(612, targetGates[613], 159, 55, 160, 59, 850));
        maps[123].EnterGates.Add(this.CreateEnterGate(614, targetGates[615], 232, 86, 234, 89, 850));
        maps[131].EnterGates.Add(this.CreateEnterGate(621, targetGates[622], 196, 25, 198, 27, 1000));
        maps[131].EnterGates.Add(this.CreateEnterGate(623, targetGates[624], 75, 74, 77, 76, 1000));
        maps[131].EnterGates.Add(this.CreateEnterGate(625, targetGates[626], 206, 42, 211, 43, 1000));
        maps[131].EnterGates.Add(this.CreateEnterGate(627, targetGates[628], 143, 208, 144, 210, 1000));
        maps[8].EnterGates.Add(this.CreateEnterGate(629, targetGates[630], 188, 74, 190, 76, 1000));
        maps[4].EnterGates.Add(this.CreateEnterGate(632, targetGates[633], 3, 238, 4, 242, 1050));
        maps[51].EnterGates.Add(this.CreateEnterGate(635, targetGates[636], 219, 123, 222, 125, 1100));
        maps[133].EnterGates.Add(this.CreateEnterGate(637, targetGates[638], 125, 61, 130, 61, 1100));
        maps[133].EnterGates.Add(this.CreateEnterGate(639, targetGates[640], 123, 13, 133, 21, 1100));
        maps[133].EnterGates.Add(this.CreateEnterGate(641, targetGates[642], 123, 190, 128, 195, 1100));
        maps[33].EnterGates.Add(this.CreateEnterGate(644, targetGates[645], 115, 112, 119, 113, 1150));
        maps[134].EnterGates.Add(this.CreateEnterGate(646, targetGates[647], 124, 25, 127, 26, 1150));
        maps[33].EnterGates.Add(this.CreateEnterGate(649, targetGates[650], 81, 17, 83, 17, 700));
        maps[135].EnterGates.Add(this.CreateEnterGate(651, targetGates[652], 113, 63, 116, 63, 700));
        maps[135].EnterGates.Add(this.CreateEnterGate(653, targetGates[654], 98, 157, 98, 159, 700));
        maps[135].EnterGates.Add(this.CreateEnterGate(655, targetGates[656], 137, 63, 139, 63, 700));
        maps[135].EnterGates.Add(this.CreateEnterGate(657, targetGates[658], 200, 139, 202, 140, 700));
        maps[33].EnterGates.Add(this.CreateEnterGate(660, targetGates[661], 211, 237, 213, 241, 1200));
        maps[136].EnterGates.Add(this.CreateEnterGate(662, targetGates[663], 113, 63, 116, 63, 1200));
        maps[136].EnterGates.Add(this.CreateEnterGate(664, targetGates[665], 98, 157, 98, 159, 1200));
        maps[136].EnterGates.Add(this.CreateEnterGate(666, targetGates[667], 137, 63, 139, 63, 1200));
        maps[136].EnterGates.Add(this.CreateEnterGate(668, targetGates[669], 200, 139, 202, 140, 1200));
        maps[38].EnterGates.Add(this.CreateEnterGate(671, targetGates[672], 106, 159, 109, 162, 1250));
        maps[137].EnterGates.Add(this.CreateEnterGate(673, targetGates[674], 117, 125, 118, 127, 1250));
        maps[63].EnterGates.Add(this.CreateEnterGate(676, targetGates[677], 35, 12, 36, 15, 1300));
        maps[138].EnterGates.Add(this.CreateEnterGate(678, targetGates[679], 116, 115, 117, 119, 1300));
        maps[8].EnterGates.Add(this.CreateEnterGate(683, targetGates[684], 107, 238, 112, 242, 1350));
        maps[140].EnterGates.Add(this.CreateEnterGate(685, targetGates[686], 131, 18, 133, 25, 1350));
        maps[140].EnterGates.Add(this.CreateEnterGate(687, targetGates[688], 132, 55, 135, 62, 1350));
        maps[140].EnterGates.Add(this.CreateEnterGate(689, targetGates[690], 160, 166, 166, 169, 1350));
        maps[51].EnterGates.Add(this.CreateEnterGate(695, targetGates[696], 178, 201, 183, 201, 1400));
        maps[81].EnterGates.Add(this.CreateEnterGate(700, targetGates[701], 73, 65, 78, 65, 1450));
        maps[142].EnterGates.Add(this.CreateEnterGate(702, targetGates[703], 109, 114, 109, 117, 1450));
        maps[80].EnterGates.Add(this.CreateEnterGate(705, targetGates[706], 210, 16, 213, 20, 1500));
        maps[143].EnterGates.Add(this.CreateEnterGate(707, targetGates[708], 122, 44, 125, 46, 1500));
        maps[143].EnterGates.Add(this.CreateEnterGate(709, targetGates[710], 114, 146, 118, 149, 1500));
        maps[143].EnterGates.Add(this.CreateEnterGate(711, targetGates[712], 104, 10, 105, 13, 1500));
        maps[56].EnterGates.Add(this.CreateEnterGate(714, targetGates[715], 131, 116, 133, 119, 1550));
        maps[144].EnterGates.Add(this.CreateEnterGate(716, targetGates[717], 101, 124, 102, 128, 1550));
    }
}