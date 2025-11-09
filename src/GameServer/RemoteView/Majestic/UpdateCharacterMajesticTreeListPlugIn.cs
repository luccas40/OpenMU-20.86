// <copyright file="UpdateCharacterMajesticTreeListPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Majestic;

using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.Attributes;
using MUnique.OpenMU.GameLogic.Views.Character;
using MUnique.OpenMU.GameLogic.Views.Minimap;
using MUnique.OpenMU.GameServer.RemoteView.Character;
using MUnique.OpenMU.GameServer.RemoteView.Minimap;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;
using System.Runtime.InteropServices;

/// <summary>
/// The default implementation of the <see cref="IUpdateCharacterMajesticTreeStatListPlugIn"/>
/// which sends the majestic data to the client.
/// </summary>
[PlugIn(nameof(UpdateCharacterMajesticTreeListPlugIn), "Sends the majestic data to the client.")]
[Guid("3E0F0140-8BD4-4487-A52F-EF4702EC69FB")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class UpdateCharacterMajesticTreeListPlugIn(RemotePlayer player) : IUpdateCharacterMajesticTreeListPlugIn
{
    private readonly RemotePlayer _player = player;

    /// <summary>
    /// Updates the character stats.
    /// </summary>
    public async ValueTask UpdateCharacterMajesticTreeListAsync()
    {
        if (this._player.Connection is not { Connected: true } connection)
        {
            return;
        }

        int Write()
        {
            var size = MajesticTreeListRef.GetRequiredSize(1);
            var span = connection.Output.GetSpan(size)[..size];
            var packet = new MajesticTreeListRef(span)
            {
                Points = 420,
                Count = 1,
            };

            var skillData = packet[0];
            skillData.Section = 3;
            skillData.Id = 0;
            skillData.CurrentLevel = 2;
            skillData.NextLevel = 8;
            skillData.Level = 10;

            /*
            var j = 0;
            foreach (var character in account.Characters.OrderBy(c => c.CharacterSlot))
            {
                var characterData = packet[j];
                characterData.SlotIndex = character.CharacterSlot;
                characterData.Name = character.Name;

                // Hack to show the correct Level since we only calculate Total Level when selecting the character
                var level = (ushort)(character.Attributes.FirstOrDefault(s => s.Definition == Stats.Level)?.Value ?? 1);
                level += (ushort)(character.Attributes.FirstOrDefault(s => s.Definition == Stats.MasterLevel)?.Value ?? 0);
                level += (ushort)(character.Attributes.FirstOrDefault(s => s.Definition == Stats.MajesticLevel)?.Value ?? 0);
                characterData.Level = level;
                characterData.Status = character.CharacterStatus.Convert();
                characterData.GuildPosition = guildPositions[j].Convert();

                appearanceSerializer.WriteAppearanceData(characterData.Appearance, new CharacterAppearanceDataAdapter(character), false);
                j++;
            }*/

            return size;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);
    }
}