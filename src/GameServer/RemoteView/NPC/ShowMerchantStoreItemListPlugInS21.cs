// <copyright file="ShowMerchantStoreItemListPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.NPC;

using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using MUnique.OpenMU.DataModel.Entities;
using MUnique.OpenMU.GameLogic.Views.NPC;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IShowMerchantStoreItemListPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn("ShowMerchantStoreItemListPlugInS21", "The S21 implementation of the IShowMerchantStoreItemListPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("93FD6052-D905-4163-9231-D602F2D1F8A8")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class ShowMerchantStoreItemListPlugInS21 : IShowMerchantStoreItemListPlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShowMerchantStoreItemListPlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public ShowMerchantStoreItemListPlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc/>
    public async ValueTask ShowMerchantStoreItemListAsync(ICollection<Item> storeItems, StoreKind storeKind)
    {
        var connection = this._player.Connection;
        if (connection is null)
        {
            return;
        }

        int Write()
        {
            var itemSerializer = this._player.ItemSerializer;
            int sizePerItem = StoredItemRef.GetRequiredSize(itemSerializer.NeededSpace);
            var size = StoreItemListS21Ref.GetRequiredSize(storeItems.Count, sizePerItem);
            var span = connection.Output.GetSpan(size)[..size];
            var packet = new StoreItemListS21Ref(span)
            {
                ItemCount = (byte)storeItems.Count,
                Type = Convert(storeKind),
                TabIndex = 0,
            };

            int headerSize = StoreItemListS21Ref.GetRequiredSize(0, 0);
            int itemListOffset = headerSize;
            int i = 0;
            foreach (var item in storeItems)
            {
                if (item.Definition is null)
                {
                    this._player.Logger.LogWarning("Item {0} has no definition.", item);
                    packet.ItemCount--;
                    continue;
                }

                var storedItem = new StoredItemRef(span[itemListOffset..]);
                storedItem.ItemSlot = item.ItemSlot;
                itemSerializer.SerializeItem(storedItem.ItemData, item);
                itemListOffset += sizePerItem;
                i++;
            }

            span.Slice(0, itemListOffset).SetPacketSize();
            return itemListOffset;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);
    }

    private static StoreItemListS21.ItemWindow Convert(StoreKind storeKind)
    {
        return storeKind switch
        {
            StoreKind.Normal => StoreItemListS21.ItemWindow.Normal,
            StoreKind.ChaosMachine => StoreItemListS21.ItemWindow.ChaosMachine,
            StoreKind.ResurrectionFailed => StoreItemListS21.ItemWindow.ResurrectionFailed,
            _ => throw new ArgumentException($"Unknown value {storeKind}", nameof(storeKind)),
        };
    }
}