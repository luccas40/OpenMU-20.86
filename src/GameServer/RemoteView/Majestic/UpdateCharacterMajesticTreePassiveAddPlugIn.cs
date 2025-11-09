// <copyright file="UpdateCharacterMajesticTreePassiveAddPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Majestic;

using System.Runtime.InteropServices;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.Views.Character;
using MUnique.OpenMU.GameLogic.Views.Minimap;
using MUnique.OpenMU.GameServer.RemoteView.Minimap;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IUpdateCharacterMajesticTreeStatListPlugIn"/>
/// which sends the majestic data to the client.
/// </summary>
[PlugIn(nameof(UpdateCharacterMajesticTreePassiveAddPlugIn), "Sends the majestic data to the client.")]
[Guid("930A245A-7C01-48FC-9DD7-5511AAC7B280")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class UpdateCharacterMajesticTreePassiveAddPlugIn(RemotePlayer player) : IUpdateCharacterMajesticTreePassiveAddPlugIn
{
    private readonly RemotePlayer _player = player;

    /// <summary>
    /// Updates the character stats.
    /// </summary>
    public async ValueTask UpdateCharacterMajesticTreePassiveAddAsync()
    {
        if (this._player.Connection is not { Connected: true } connection)
        {
            return;
        }

    }

}