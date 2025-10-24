// <copyright file="ItemDurabilityChangedPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Inventory;

using System.Runtime.InteropServices;
using MUnique.OpenMU.DataModel;
using MUnique.OpenMU.DataModel.Entities;
using MUnique.OpenMU.GameLogic.Views.Inventory;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IItemDurabilityChangedPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn(nameof(ItemDurabilityChangedPlugInS21), "The S21 implementation of the IItemDurabilityChangedPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("2D5EA939-3CD3-4EF9-88AB-E6F78414D141")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
public class ItemDurabilityChangedPlugInS21 : IItemDurabilityChangedPlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemDurabilityChangedPlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public ItemDurabilityChangedPlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc/>
    public async ValueTask ItemDurabilityChangedAsync(Item item, bool afterConsumption)
    {
        await this._player.Connection.SendItemDurabilityChangedS21Async(item.ItemSlot, item.Durability(), afterConsumption, 0).ConfigureAwait(false);
    }
}