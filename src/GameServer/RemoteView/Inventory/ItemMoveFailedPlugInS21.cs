﻿// <copyright file="ItemMoveFailedPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Inventory;

using System.Runtime.InteropServices;
using MUnique.OpenMU.DataModel.Entities;
using MUnique.OpenMU.GameLogic.Views.Inventory;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IItemMoveFailedPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn("ItemMoveFailedPlugInS21", "The default implementation of the IItemMoveFailedPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("DB7CBBC5-AC16-4648-B845-A793F81787C8")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
public class ItemMoveFailedPlugInS21 : IItemMoveFailedPlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemMoveFailedPlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public ItemMoveFailedPlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc/>
    public async ValueTask ItemMoveFailedAsync(Item? item)
    {
        var connection = this._player.Connection;
        if (connection is null)
        {
            return;
        }

        int Write()
        {
            var itemSerializer = this._player.ItemSerializer;
            var size = ItemMovedS21Ref.GetRequiredSize(itemSerializer.NeededSpace);
            var span = connection.Output.GetSpan(size)[..size];
            var packet = new ItemMovedS21Ref(span)
            {
                TargetStorageType = 0xFF
            };

            if (item != null)
            {
                var itemSize = itemSerializer.SerializeItem(packet.ItemData, item);
                var actualSize = ItemMovedS21Ref.GetRequiredSize(itemSize);
                span.Slice(0, actualSize).SetPacketSize();
                return actualSize;
            }

            return size;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);
    }
}