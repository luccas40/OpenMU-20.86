// <copyright file="GameMapsInitializer.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086;

using MUnique.OpenMU.DataModel.Configuration;
using MUnique.OpenMU.Persistence.Initialization.Version2086.Maps;

/// <summary>
/// Initializes the <see cref="GameMapDefinition"/>s.
/// </summary>
public class GameMapsInitializer : GameMapsInitializerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameMapsInitializer"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    public GameMapsInitializer(IContext context, GameConfiguration gameConfiguration)
        : base(context, gameConfiguration)
    {
    }

    /// <inheritdoc />
    protected override IEnumerable<Type> MapInitializerTypes
    {
        get
        {
            yield return typeof(Lorencia);
            yield return typeof(Dungeon);
            yield return typeof(Devias);
            yield return typeof(Noria);
            yield return typeof(LostTower);
            // yield return typeof(Exile);
            yield return typeof(Arena);
            yield return typeof(Atlans);
            yield return typeof(Tarkan);
            yield return typeof(Icarus);
            yield return typeof(Elvenland);
            yield return typeof(Karutan1);
            yield return typeof(Karutan2);
            yield return typeof(Aida);
            yield return typeof(Vulcanus);
            yield return typeof(CrywolfFortress);
            yield return typeof(LandOfTrials);
            yield return typeof(LorenMarket);
            yield return typeof(SantaVillage);
            yield return typeof(SilentMap);
            yield return typeof(ValleyOfLoren);
            yield return typeof(BarracksOfBalgass);
            yield return typeof(BalgassRefuge);
            yield return typeof(Kalima1);
            yield return typeof(Kalima2);
            yield return typeof(Kalima3);
            yield return typeof(Kalima4);
            yield return typeof(Kalima5);
            yield return typeof(Kalima6);
            yield return typeof(Kalima7);
            yield return typeof(KanturuRelics);
            yield return typeof(KanturuRuins);
            yield return typeof(KanturuEvent);
            yield return typeof(Raklion);
            yield return typeof(RaklionBoss);
            yield return typeof(SwampOfCalmness);
            yield return typeof(DuelArena);
            yield return typeof(BloodCastle1);
            yield return typeof(BloodCastle2);
            yield return typeof(BloodCastle3);
            yield return typeof(BloodCastle4);
            yield return typeof(BloodCastle5);
            yield return typeof(BloodCastle6);
            yield return typeof(BloodCastle7);
            yield return typeof(BloodCastle8);
            yield return typeof(ChaosCastle1);
            yield return typeof(ChaosCastle2);
            yield return typeof(ChaosCastle3);
            yield return typeof(ChaosCastle4);
            yield return typeof(ChaosCastle5);
            yield return typeof(ChaosCastle6);
            yield return typeof(ChaosCastle7);
            yield return typeof(IllusionTemple1);
            yield return typeof(IllusionTemple2);
            yield return typeof(IllusionTemple3);
            yield return typeof(IllusionTemple4);
            yield return typeof(IllusionTemple5);
            yield return typeof(IllusionTemple6);
            yield return typeof(DevilSquare1);
            yield return typeof(DevilSquare2);
            yield return typeof(DevilSquare3);
            yield return typeof(DevilSquare4);
            yield return typeof(DevilSquare5);
            yield return typeof(DevilSquare6);
            yield return typeof(DevilSquare7);
            yield return typeof(Doppelgaenger1);
            yield return typeof(Doppelgaenger2);
            yield return typeof(Doppelgaenger3);
            yield return typeof(Doppelgaenger4);
            yield return typeof(FortressOfImperialGuardian1);
            yield return typeof(FortressOfImperialGuardian2);
            yield return typeof(FortressOfImperialGuardian3);
            yield return typeof(FortressOfImperialGuardian4);
            yield return typeof(DoubleGoer1);
            yield return typeof(DoubleGoer2);
            yield return typeof(DoubleGoer3);
            yield return typeof(DoubleGoer4);
            yield return typeof(DoubleGoer5);
            yield return typeof(DoubleGoer6);
            yield return typeof(DoubleGoer7);
            yield return typeof(DoubleGoer8);
            yield return typeof(DoubleGoer9);
            yield return typeof(Acheron1);
            yield return typeof(Acheron2);
            yield return typeof(Debenter1);
            yield return typeof(Debenter2);
            yield return typeof(ChaosCastle8);
            yield return typeof(IllusionTemple7);
            yield return typeof(IllusionTemple8);
            yield return typeof(UrkMontain1);
            yield return typeof(UrkMontain2);
            yield return typeof(TormentedSquare1);
            yield return typeof(TormentedSquare2);
            yield return typeof(TormentedSquare3);
            yield return typeof(TormentedSquare4);
            yield return typeof(TormentedSquare5);
            yield return typeof(Nars);
            yield return typeof(Ferea);
            yield return typeof(NixieLake);
            yield return typeof(QuestZoneEntrance);
            yield return typeof(Labyrinth);
            yield return typeof(DeepDungeon1);
            yield return typeof(DeepDungeon2);
            yield return typeof(DeepDungeon3);
            yield return typeof(DeepDungeon4);
            yield return typeof(DeepDungeon5);
            yield return typeof(Place4thQuest);
            yield return typeof(SwampOfDarkness);
            yield return typeof(KuberaMine);
            yield return typeof(KuberaMine2);
            yield return typeof(KuberaMine3);
            yield return typeof(KuberaMine4);
            yield return typeof(KuberaMine5);
            yield return typeof(AbyssOfAtlans);
            yield return typeof(AbyssOfAtlans2);
            yield return typeof(AbyssOfAtlans3);
            yield return typeof(ScorchedCanyon);
            yield return typeof(RedSmokeIcarus);
            yield return typeof(ArenilTemple);
            yield return typeof(AshyAida);
            yield return typeof(OldKethotum);
            yield return typeof(BlazeKethotum);
            yield return typeof(KanturuUndergrounds);
            yield return typeof(VolcanoIgnis);
            yield return typeof(BossBattle);
            yield return typeof(BloodyTarkan);
            yield return typeof(TormentaIsland);
            yield return typeof(TwistedKarutan);
            yield return typeof(KardamahalUnderGroundTemple);
            yield return typeof(SwampOfDoom);
        }
    }
}