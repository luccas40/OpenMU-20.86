// <copyright file="UpdateCharacterMajesticTreeAddPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Majestic;

using System.Runtime.InteropServices;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.Views.Character;
using MUnique.OpenMU.GameLogic.Views.Minimap;
using MUnique.OpenMU.GameServer.RemoteView.Minimap;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IUpdateCharacterMajesticTreeStatListPlugIn"/>
/// which sends the majestic data to the client.
/// </summary>
[PlugIn(nameof(UpdateCharacterMajesticTreeAddPlugIn), "Sends the majestic data to the client.")]
[Guid("B42BBF81-0471-474C-B614-B109700716BE")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class UpdateCharacterMajesticTreeAddPlugIn(RemotePlayer player) : IUpdateCharacterMajesticTreeAddPlugIn
{
    private readonly RemotePlayer _player = player;

    /// <summary>
    /// Updates the character stats.
    /// </summary>
    public async ValueTask UpdateCharacterMajesticTreeAddAsync()
    {
        if (this._player.Connection is not { Connected: true } connection)
        {
            return;
        }

    }

}