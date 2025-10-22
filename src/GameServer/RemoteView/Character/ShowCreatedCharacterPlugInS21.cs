// <copyright file="ShowCreatedCharacterPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Character;

using System.Runtime.InteropServices;
using MUnique.OpenMU.DataModel.Entities;
using MUnique.OpenMU.GameLogic.Attributes;
using MUnique.OpenMU.GameLogic.Views.Character;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IShowCreatedCharacterPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn("ShowCreatedCharacterPlugInS21", "The S21 implementation of the IShowCreatedCharacterPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("C7573220-280A-4682-A99B-AEDC67123910")]
[MinimumClient(20, 0, Network.PlugIns.ClientLanguage.Korean)]
public class ShowCreatedCharacterPlugInS21 : IShowCreatedCharacterPlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShowCreatedCharacterPlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public ShowCreatedCharacterPlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc/>
    public async ValueTask ShowCreatedCharacterAsync(Character character)
    {
        var connection = this._player.Connection;
        if (connection is null)
        {
            return;
        }

        int Write()
        {
            var size = CharacterCreationSuccessfulS21Ref.Length;
            var span = connection.Output.GetSpan(size)[..size];
            var packet = new CharacterCreationSuccessfulS21Ref(span)
            {
                CharacterName = character.Name,
                CharacterSlot = character.CharacterSlot,
                Level = (ushort)(character.Attributes.FirstOrDefault(a => a.Definition == Stats.Level)?.Value ?? 0),
                Class = character.CharacterClass!.Number,
                CharacterStatus = (byte)character.CharacterStatus,
            };


            packet.PreviewData.Fill(0xFF);
            return size;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);
    }
}