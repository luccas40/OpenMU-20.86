// <copyright file="ItemUsedPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Inventory;

using System.Runtime.InteropServices;
using MUnique.OpenMU.DataModel.Entities;
using MUnique.OpenMU.GameLogic.Views.Inventory;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.Persistence.BasicModel;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IItemUsedPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ItemUsedPlugIn"/> class.
/// </remarks>
/// <param name="player">The player.</param>
[PlugIn("ItemUsedPlugIn", "The default implementation of the IItemUsedPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("2EB0DEE8-FE9C-4B2E-AD7E-6D4963F59A45")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
public class ItemUsedPlugIn(RemotePlayer player) : IItemUsedPlugIn
{
    private readonly RemotePlayer _player = player;

    /// <inheritdoc/>
    public async ValueTask ItemUsedAsync(byte slot, bool active)
    {
        var connection = this._player.Connection;
        if (connection is null)
        {
            return;
        }

        int Write()
        {
            var size = InventoryItemUseRef.Length;
            var span = connection.Output.GetSpan(size)[..size];
            var packet = new InventoryItemUseRef(span)
            {
                Slot = slot,
                UseType = (byte)(active ? 0xFE : 0xFF),
            };

            return size;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);
    }
}