// <copyright file="OpenNpcWindowPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.NPC;

using System.Runtime.InteropServices;
using MUnique.OpenMU.DataModel.Configuration;
using MUnique.OpenMU.GameLogic.Views.NPC;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IOpenNpcWindowPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn("OpenNpcWindowPlugInS21", "The S21 implementation of the IOpenNpcWindowPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("3d77d6ef-479c-45f6-8ec9-a7bb046d306a")]
[MinimumClient(21, 0,  Network.PlugIns.ClientLanguage.Korean)]
public class OpenNpcWindowPlugInS21 : IOpenNpcWindowPlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenNpcWindowPlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public OpenNpcWindowPlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc/>
    public async ValueTask OpenNpcWindowAsync(NpcWindow window, int tabCount = 0)
    {
        if (window == NpcWindow.NpcDialog)
        {
            if (this._player.OpenedNpc is not null)
            {
                await this._player.Connection.SendOpenNpcDialogAsync(this._player.OpenedNpc.Definition.Number.ToUnsigned(), 0).ConfigureAwait(false);
            }
        }
        else
        {
            await this._player.Connection.SendNpcWindowResponseS21Async(Convert(window), (byte)tabCount).ConfigureAwait(false);
        }
    }

    private static NpcWindowResponseS21.NpcWindow Convert(NpcWindow window)
    {
        return window switch
        {
            NpcWindow.Merchant => NpcWindowResponseS21.NpcWindow.Merchant,
            NpcWindow.Merchant1 => NpcWindowResponseS21.NpcWindow.Merchant1,
            NpcWindow.VaultStorage => NpcWindowResponseS21.NpcWindow.VaultStorage,
            NpcWindow.ChaosMachine => NpcWindowResponseS21.NpcWindow.ChaosMachine,
            NpcWindow.DevilSquare => NpcWindowResponseS21.NpcWindow.DevilSquare,
            NpcWindow.BloodCastle => NpcWindowResponseS21.NpcWindow.BloodCastle,
            NpcWindow.PetTrainer => NpcWindowResponseS21.NpcWindow.PetTrainer,
            NpcWindow.Lahap => NpcWindowResponseS21.NpcWindow.Lahap,
            NpcWindow.CastleSeniorNPC => NpcWindowResponseS21.NpcWindow.CastleSeniorNPC,
            NpcWindow.ElphisRefinery => NpcWindowResponseS21.NpcWindow.ElphisRefinery,
            NpcWindow.RefineStoneMaking => NpcWindowResponseS21.NpcWindow.RefineStoneMaking,
            NpcWindow.RemoveJohOption => NpcWindowResponseS21.NpcWindow.RemoveJohOption,
            NpcWindow.IllusionTemple => NpcWindowResponseS21.NpcWindow.IllusionTemple,
            NpcWindow.ChaosCardCombination => NpcWindowResponseS21.NpcWindow.ChaosCardCombination,
            NpcWindow.CherryBlossomBranchesAssembly => NpcWindowResponseS21.NpcWindow.CherryBlossomBranchesAssembly,
            NpcWindow.SeedMaster => NpcWindowResponseS21.NpcWindow.SeedMaster,
            NpcWindow.SeedResearcher => NpcWindowResponseS21.NpcWindow.SeedResearcher,
            NpcWindow.StatReInitializer => NpcWindowResponseS21.NpcWindow.StatReInitializer,
            NpcWindow.DelgadoLuckyCoinRegistration => NpcWindowResponseS21.NpcWindow.DelgadoLuckyCoinRegistration,
            NpcWindow.DoorkeeperTitusDuelWatch => NpcWindowResponseS21.NpcWindow.DoorkeeperTitusDuelWatch,
            NpcWindow.LugardDoppelgangerEntry => NpcWindowResponseS21.NpcWindow.LugardDoppelgangerEntry,
            NpcWindow.JerintGaionEvententry => NpcWindowResponseS21.NpcWindow.JerintGaionEvententry,
            NpcWindow.JuliaWarpMarketServer => NpcWindowResponseS21.NpcWindow.JuliaWarpMarketServer,
            NpcWindow.CombineLuckyItem => NpcWindowResponseS21.NpcWindow.CombineLuckyItem,
            NpcWindow.GuildMaster => throw new ArgumentException("guild master dialog is opened by another action."),
            NpcWindow.NpcDialog => throw new ArgumentException("The quest dialog is opened by another action"),
            NpcWindow.LegacyQuest => throw new ArgumentException("The legacy quest dialog is opened by another action"),
            _ => throw new ArgumentException($"Unhandled case {window}."),
        };
    }
}