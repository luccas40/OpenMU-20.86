// <copyright file="CharacterCreatePacketHandlerPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.MessageHandler.Character;

using System.Runtime.InteropServices;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.PlayerActions.Character;
using MUnique.OpenMU.Network.Packets.ClientToServer;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// S21 Packet handler for character creation packets (0xF3, 0x01 identifier).
/// </summary>
[PlugIn("Character - Create S21", "S21 Packet handler for character creation packets (0xF3, 0x01 identifier).")]
[Guid("05DC7153-FA0A-4BE8-97A9-BE8C4D85261F")]
[MinimumClient(20, 0, Network.PlugIns.ClientLanguage.Korean)]
[BelongsToGroup(CharacterGroupHandlerPlugIn.GroupKey)]
internal class CharacterCreatePacketHandlerPlugInS21 : ISubPacketHandlerPlugIn
{
    private readonly CreateCharacterAction _createCharacterAction = new();

    /// <inheritdoc/>
    public bool IsEncryptionExpected => false;

    /// <inheritdoc/>
    public byte Key => 1;

    /// <inheritdoc />
    public async ValueTask HandlePacketAsync(Player player, Memory<byte> packet)
    {
        CreateCharacterS21 message = packet;
        await this._createCharacterAction.CreateCharacterAsync(player, message.Name, message.Class).ConfigureAwait(false);
    }
}