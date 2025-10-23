// <copyright file="ItemMoveHandlerPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.MessageHandler.Items;

using System.Runtime.InteropServices;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.PlayerActions.Items;
using MUnique.OpenMU.GameServer.RemoteView.Inventory;
using MUnique.OpenMU.Network.Packets.ClientToServer;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// Packet handler for S21 item move packets.
/// </summary>
[PlugIn(nameof(ItemMoveHandlerPlugInS21), "Packet handler for extended item move packets.")]
[Guid("DC02EB39-241C-46FB-B552-5CB8F1F0FC6E")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
internal class ItemMoveHandlerPlugInS21 : IPacketHandlerPlugIn
{
    private readonly MoveItemAction _moveAction = new();

    /// <inheritdoc/>
    public bool IsEncryptionExpected => false;

    /// <inheritdoc/>
    public byte Key => ItemMoveRequest.Code;

    /// <inheritdoc/>
    public async ValueTask HandlePacketAsync(Player player, Memory<byte> packet)
    {
        ItemMoveRequestS21 message = packet;

        // we don't transmit the item binary data anymore in this extended message.
        await this._moveAction.MoveItemAsync(player, message.FromSlot, message.FromStorage.Convert(), message.ToSlot, message.ToStorage.Convert()).ConfigureAwait(false);
    }
}