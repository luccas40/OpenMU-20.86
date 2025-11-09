// <copyright file="UpdateCharacterMajesticTreeDataPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Majestic;

using System.Runtime.InteropServices;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.Views.Character;
using MUnique.OpenMU.GameLogic.Views.Minimap;
using MUnique.OpenMU.GameServer.RemoteView.Minimap;
using MUnique.OpenMU.Network.Packets.ClientToServer;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IUpdateCharacterMajesticTreeStatListPlugIn"/>
/// which sends the majestic data to the client.
/// </summary>
[PlugIn(nameof(UpdateCharacterMajesticTreeDataPlugIn), "Sends the majestic data to the client.")]
[Guid("A9F67CED-0860-4E3E-A3D1-A526C1C9604C")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class UpdateCharacterMajesticTreeDataPlugIn(RemotePlayer player) : IUpdateCharacterMajesticTreeDataPlugIn
{
    private readonly RemotePlayer _player = player;

    /// <summary>
    /// Updates the character stats.
    /// </summary>
    public async ValueTask UpdateCharacterMajesticTreeDataAsync()
    {
        if (this._player.Connection is not { Connected: true } connection)
        {
            return;
        }

        await this._player.Connection.SendMajesticTreeDataAsync(20).ConfigureAwait(false);
    }

}