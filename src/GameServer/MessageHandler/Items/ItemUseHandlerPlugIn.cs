// <copyright file="ItemUseHandlerPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.MessageHandler.Items;

using System.Runtime.InteropServices;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.PlayerActions.Items;
using MUnique.OpenMU.GameServer.MessageHandler.MuHelper;
using MUnique.OpenMU.Network.Packets.ClientToServer;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// Packet handler for Inventory Item Use packets.
/// </summary>
[PlugIn(nameof(ItemUseHandlerPlugIn), "Packet handler for inventory item use packets.")]
[Guid("32B23A20-A977-4674-A762-C1885693F296")]
[BelongsToGroup(MuHelperGroupHandler.GroupKey)]
[MinimumClient(21, 0, ClientLanguage.Korean)]
internal class ItemUseHandlerPlugIn : ISubPacketHandlerPlugIn
{
    private readonly ItemUseAction _action = new();

    /// <inheritdoc/>
    public bool IsEncryptionExpected => false;

    /// <inheritdoc/>
    public byte Key => InventoryItemUse.SubCode;

    /// <inheritdoc/>
    public async ValueTask HandlePacketAsync(Player player, Memory<byte> packet)
    {
        InventoryItemUse message = packet;
        await this._action.UseItemAsync(player, message.Slot, message.UseType == 0xFE).ConfigureAwait(false);
    }
}
