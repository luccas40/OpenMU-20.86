// <copyright file="MuHelperSaveDataRequestHandlerPluginS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.MessageHandler.MuHelper;

using System.Buffers;
using System.Runtime.InteropServices;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.PlayerActions.MuHelper;
using MUnique.OpenMU.Network.Packets.ClientToServer;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// Packet handler for mu helper data save packets (0xAE identifier).
/// </summary>
[PlugIn(nameof(MuHelperSaveDataRequestHandlerPluginS21), "Handler for S21 mu bot data save request.")]
[Guid("B8914D7E-0AD3-460B-8D02-2E9955551F49")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class MuHelperSaveDataRequestHandlerPluginS21 : IPacketHandlerPlugIn
{
    private readonly UpdateMuHelperConfigurationAction _updateMuBotConfigurationAction = new();

    /// <inheritdoc />
    public byte Key => MuHelperSaveDataRequest.Code;

    /// <inheritdoc />
    public bool IsEncryptionExpected => false;

    /// <inheritdoc />
    public async ValueTask HandlePacketAsync(Player player, Memory<byte> packet)
    {
        MuHelperSaveDataRequestS21 message = packet;
        var dataSize = message.HelperData.Length;
        using var memoryOwner = MemoryPool<byte>.Shared.Rent(dataSize);
        var memory = memoryOwner.Memory[..dataSize];
        message.HelperData.CopyTo(memory.Span);
        await this._updateMuBotConfigurationAction.SaveDataAsync(player, memory).ConfigureAwait(false);
    }
}