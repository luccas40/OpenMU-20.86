// <copyright file="TalkNpcHandlerPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.MessageHandler;

using System.Runtime.InteropServices;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.NPC;
using MUnique.OpenMU.GameLogic.PlayerActions;
using MUnique.OpenMU.Network.Packets.ClientToServer;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// Handler for talk npc request packets.
/// </summary>
[PlugIn(nameof(TalkNpcHandlerPlugInS21), "Handler for talk npc request packets.")]
[Guid("18FFB144-E8D3-470A-A101-CF53E2B58C32")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
internal class TalkNpcHandlerPlugInS21 : IPacketHandlerPlugIn
{
    /// <inheritdoc/>
    public virtual bool IsEncryptionExpected => false;

    /// <inheritdoc/>
    public byte Key => TalkToNpcRequest.Code;

    /// <summary>
    /// Gets the talk NPC action.
    /// </summary>
    protected TalkNpcActionS21 TalkNpcAction { get; } = new();

    /// <inheritdoc/>
    public async ValueTask HandlePacketAsync(Player player, Memory<byte> packet)
    {
        TalkToNpcRequest message = packet;
        if (player.CurrentMap?.GetObject(message.NpcId) is NonPlayerCharacter npc)
        {
            await this.TalkNpcAction.TalkToNpcAsync(player, npc).ConfigureAwait(false);
        }
    }
}