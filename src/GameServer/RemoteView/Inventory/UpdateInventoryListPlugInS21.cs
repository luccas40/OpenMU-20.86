// <copyright file="UpdateInventoryListPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Inventory;

using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using MUnique.OpenMU.GameLogic.Views.Inventory;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IUpdateInventoryListPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="UpdateInventoryListPlugInS21"/> class.
/// </remarks>
/// <param name="player">The player.</param>
[PlugIn("UpdateInventoryListPlugInS21", "The S21 implementation of the IUpdateInventoryListPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("c46bc81f-70be-41a2-b817-c5d24410a6a5")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
public class UpdateInventoryListPlugInS21(RemotePlayer player) : IUpdateInventoryListPlugIn
{
    private readonly RemotePlayer _player = player;

    /// <inheritdoc/>
    public async ValueTask UpdateInventoryListAsync()
    {
        var connection = this._player.Connection;
        if (connection is null || this._player.SelectedCharacter?.Inventory is null)
        {
            return;
        }

        var items = this._player.SelectedCharacter.Inventory.Items.OrderBy(item => item.ItemSlot).ToList();
        int Write()
        {
            var itemSerializer = this._player.ItemSerializer;
            var lengthPerItem = StoredItemRef.GetRequiredSize(itemSerializer.NeededSpace);
            var size = CharacterInventoryS21Ref.GetRequiredSize(items.Count, lengthPerItem);
            var span = connection.Output.GetSpan(size)[..size];
            var packet = new CharacterInventoryS21Ref(span)
            {
                ItemCount = (byte)items.Count,
            };

            int headerSize = CharacterInventoryS21Ref.GetRequiredSize(0, 0);
            int actualSize = headerSize;
            int i = 0;
            foreach (var item in items)
            {
                if (item.Definition is null)
                {
                    this._player.Logger.LogWarning("Item {0} has no definition.", item);
                    packet.ItemCount--;
                    continue;
                }

                var storedItem = new StoredItemRef(span[actualSize..]);
                storedItem.ItemSlot = item.ItemSlot;
                var itemSize = itemSerializer.SerializeItem(storedItem.ItemData, item);
                actualSize += StoredItemRef.GetRequiredSize(itemSize);
                i++;
            }

            span.Slice(0, actualSize).SetPacketSize();
            return actualSize;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);
    }
}