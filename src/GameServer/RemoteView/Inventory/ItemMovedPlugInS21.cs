// <copyright file="ItemMovedPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Inventory;

using System.Runtime.InteropServices;
using MUnique.OpenMU.DataModel.Entities;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.Views.Inventory;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IItemMovedPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn("ItemMovedPlugInS21", "The S21 implementation of the IItemMovedPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("6A899552-09AD-4B83-A4E8-B2EBA0D8E178")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
public class ItemMovedPlugInS21 : IItemMovedPlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemMovedPlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public ItemMovedPlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc/>
    public async ValueTask ItemMovedAsync(Item item, byte toSlot, Storages storage)
    {
        var connection = this._player.Connection;
        if (connection is null)
        {
            return;
        }

        var itemSerializer = this._player.ItemSerializer;
        var targetStorage = storage.Convert();
        if (targetStorage == ItemStorageKind.PlayerShop)
        {
            targetStorage = ItemStorageKind.Inventory;
        }

        int Write()
        {
            var size = ItemMovedS21Ref.GetRequiredSize(itemSerializer.NeededSpace);
            var span = connection.Output.GetSpan(size)[..size];
            var message = new ItemMovedS21Ref(span)
            {
                TargetStorageType = (byte)targetStorage,
                TargetSlot = toSlot,
            };
            var itemSize = itemSerializer.SerializeItem(message.ItemData, item);

            var actualSize = ItemMovedRef.GetRequiredSize(itemSize);
            span.Slice(0, actualSize).SetPacketSize();
            return actualSize;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);
    }
}