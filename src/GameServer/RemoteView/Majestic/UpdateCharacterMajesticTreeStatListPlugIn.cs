// <copyright file="UpdateCharacterMajesticTreeStatListPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Majestic;

using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.Views.Character;
using MUnique.OpenMU.GameLogic.Views.Minimap;
using MUnique.OpenMU.GameServer.RemoteView.Minimap;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;
using System.Runtime.InteropServices;

/// <summary>
/// The default implementation of the <see cref="IUpdateCharacterMajesticTreeStatListPlugIn"/>
/// which sends the majestic data to the client.
/// </summary>
[PlugIn(nameof(UpdateCharacterMajesticTreeStatListPlugIn), "Sends the majestic data to the client.")]
[Guid("491B778F-E0B8-4955-A3E1-689776E677E1")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class UpdateCharacterMajesticTreeStatListPlugIn(RemotePlayer player) : IUpdateCharacterMajesticTreeStatListPlugIn
{
    private readonly RemotePlayer _player = player;

    /// <summary>
    /// Updates the character stats.
    /// </summary>
    public async ValueTask UpdateCharacterMajesticTreeStatListAsync()
    {
        if (this._player.Connection is not { Connected: true } connection)
        {
            return;
        }

        int Write()
        {
            var size = MajesticTreeStatListRef.GetRequiredSize(8);
            var span = connection.Output.GetSpan(size)[..size];
            var packet = new MajesticTreeStatListRef(span)
            {
                Count = 8,
            };

            var skillData = packet[0];
            skillData.Section = 0;
            skillData.Id = 0;
            skillData.CurrentLevel = 2;
            skillData.NextLevel = 8;
            skillData.Level = 10;

            skillData = packet[1];
            skillData.Section = 0;
            skillData.Id = 1;
            skillData.CurrentLevel = 2;
            skillData.NextLevel = 8;
            skillData.Level = 10;

            skillData = packet[2];
            skillData.Section = 0;
            skillData.Id = 2;
            skillData.CurrentLevel = 2;
            skillData.NextLevel = 8;
            skillData.Level = 10;

            skillData = packet[3];
            skillData.Section = 0;
            skillData.Id = 3;
            skillData.CurrentLevel = 2;
            skillData.NextLevel = 8;
            skillData.Level = 10;

            skillData = packet[4];
            skillData.Section = 1;
            skillData.Id = 0;
            skillData.CurrentLevel = 2;
            skillData.NextLevel = 8;
            skillData.Level = 10;

            skillData = packet[5];
            skillData.Section = 1;
            skillData.Id = 1;
            skillData.CurrentLevel = 2;
            skillData.NextLevel = 8;
            skillData.Level = 10;

            skillData = packet[6];
            skillData.Section = 1;
            skillData.Id = 2;
            skillData.CurrentLevel = 2;
            skillData.NextLevel = 8;
            skillData.Level = 10;

            skillData = packet[7];
            skillData.Section = 1;
            skillData.Id = 3;
            skillData.CurrentLevel = 2;
            skillData.NextLevel = 8;
            skillData.Level = 10;
            return size;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);
    }

}