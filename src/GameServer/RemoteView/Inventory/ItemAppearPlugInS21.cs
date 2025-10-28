// <copyright file="ItemAppearPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Inventory;

using System.Runtime.InteropServices;
using MUnique.OpenMU.DataModel.Entities;
using MUnique.OpenMU.GameLogic.Views.Inventory;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IItemAppearPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn("ItemAppearPlugInS21", "The S21 implementation of the IItemAppearPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("F2378D1A-93A8-4380-9B12-FC7038D2332E")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class ItemAppearPlugInS21 : IItemAppearPlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemAppearPlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public ItemAppearPlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc/>
    public async ValueTask ItemAppearAsync(Item newItem, ushort dropId)
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
            packet.InventorySlot = newItem.ItemSlot;
            packet.DropId = @dropId;
            itemSerializer.SerializeItem(packet.ItemData, newItem);
            return packet.Header.Length;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);
    }
}