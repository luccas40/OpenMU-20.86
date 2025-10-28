// <copyright file="ItemPickUpFailedPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Inventory;

using MUnique.OpenMU.GameLogic.Views.Inventory;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

/// <summary>
/// The default implementation of the <see cref="IItemPickUpFailedPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn("ItemPickUpFailedPlugInS21", "The S21 implementation of the IItemPickUpFailedPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("624379DA-1655-4579-8D52-DD019BCB27C8")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class ItemPickUpFailedPlugInS21 : IItemPickUpFailedPlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemPickUpFailedPlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public ItemPickUpFailedPlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc />
    public async ValueTask ItemPickUpFailedAsync(ItemPickFailReason reason)
    {
        var connection = this._player.Connection;
        if (connection is null)
        {
            return;
        }

        int Write()
        {
            var itemSerializer = this._player.ItemSerializer;
            var length = ItemPickUpResponseRef.GetRequiredSize(itemSerializer.NeededSpace);
            var packet = new ItemPickUpResponseRef(connection.Output.GetSpan(length)[..length]);
            packet.InventorySlot = 0xFF;
            packet.DropId = ushort.MaxValue;
            return packet.Header.Length;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);
    }
}