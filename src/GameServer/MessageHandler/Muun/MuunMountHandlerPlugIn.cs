// <copyright file="MuunMountHandlerPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.MessageHandler.Muun;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.Views.World;
using MUnique.OpenMU.Network.Packets.ClientToServer;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// Handler for the request for mount to be visible.
/// </summary>
[PlugIn(nameof(MuunMountHandlerPlugIn), "Handler for the request for mount to be visible.")]
[Guid("C42B636A-7E8D-424B-B6D2-4F1CE807A03B")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
internal class MuunMountHandlerPlugIn : IPacketHandlerPlugIn
{
    /// <inheritdoc/>
    public bool IsEncryptionExpected => false;

    /// <inheritdoc/>
    public byte Key => MuunMountRequest.Code;

    /// <reminder>
    /// Verify if mount hes requesting is the same that is active in the inventory.
    /// if not maybe deactivate mount.
    /// </reminder>
    /// <inheritdoc/>
    public async ValueTask HandlePacketAsync(Player player, Memory<byte> packet)
    {
        MuunMountRequest request = packet;
        await player.ForEachWorldObserverAsync<IUpdatePlayerMountPlugIn>(p => p.UpdatePlayerMountAsync(player, request.MountId), true).ConfigureAwait(false);
    }
}
