// <copyright file="ItemUpgradedPlugInS21.cs" company="MUnique">
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
/// The default implementation of the <see cref="IItemUpgradedPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn("ItemUpgradedPlugInS21", "The S21 implementation of the IItemUpgradedPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("BD9940D6-61B4-4352-B1BC-6994207EE7C5")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class ItemUpgradedPlugInS21 : IItemUpgradedPlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemUpgradedPlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public ItemUpgradedPlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc/>
    public async ValueTask ItemUpgradedAsync(Item item)
    {
        var connection = this._player.Connection;
        if (connection is null)
        {
            return;
        }

        int Write()
        {
            var itemSerializer = this._player.ItemSerializer;
            var size = InventoryItemUpgradedS21Ref.GetRequiredSize(itemSerializer.NeededSpace);
            var span = connection.Output.GetSpan(size)[..size];
            var packet = new InventoryItemUpgradedS21Ref(span)
            {
                InventorySlot = item.ItemSlot,
            };
            var itemSize = itemSerializer.SerializeItem(packet.ItemData, item);
            var actualSize = InventoryItemUpgradedS21Ref.GetRequiredSize(itemSize);
            span.Slice(0, actualSize).SetPacketSize();
            return actualSize;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);
    }
}