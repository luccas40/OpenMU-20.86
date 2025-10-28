// <copyright file="UpdateMoneyPlugInS21.cs" company="MUnique">
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
/// The default implementation of the <see cref="IUpdateMoneyPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn("UpdateMoneyPlugIn", "The S21 implementation of the IUpdateMoneyPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("4DF3CBF5-F641-44A7-A5FC-E3BD1DFC0D03")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class UpdateMoneyPlugInS21 : IUpdateMoneyPlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMoneyPlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public UpdateMoneyPlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc/>
    public async ValueTask UpdateMoneyAsync()
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
            packet.InventorySlot = 0xFE;
            packet.DropId = ushort.MaxValue;
            var money = this._player.Money;
            BinaryPrimitives.WriteUInt32BigEndian(packet.ItemData, (uint)money);
            return packet.Header.Length;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);
    }
}