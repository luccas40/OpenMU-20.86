// <copyright file="ShowCharacterListPluginS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Character;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MUnique.OpenMU.DataModel.Entities;
using MUnique.OpenMU.GameLogic.Attributes;
using MUnique.OpenMU.GameLogic.Views.Character;
using MUnique.OpenMU.GameServer.RemoteView.Guild;
using MUnique.OpenMU.Interfaces;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The S21 implementation of the <see cref="IShowCharacterListPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ShowCharacterListPluginS21"/> class.
/// </remarks>
/// <param name="player">The player.</param>
[PlugIn("ShowCharacterListPlugInS21", "The S21 implementation of the IShowCharacterListPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("ddb6b43a-3657-4dc2-9b1f-f59f7cf39a3a")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
public class ShowCharacterListPluginS21(RemotePlayer player) : IShowCharacterListPlugIn
{
    private readonly RemotePlayer _player = player;

    /// <inheritdoc/>
    public async ValueTask ShowCharacterListAsync()
    {
        var connection = this._player.Connection;
        if (connection is null || this._player.Account is not { } account)
        {
            return;
        }

        var unlockFlags = CreateUnlockFlags(account);
        var guildPositions = new GuildPosition?[account.Characters.Count];
        int i = 0;
        foreach (var character in account.Characters)
        {
            guildPositions[i] = await this._player.GameServerContext.GuildServer.GetGuildPositionAsync(character.Id).ConfigureAwait(false);
            i++;
        }

        var appearanceSerializer = this._player.AppearanceSerializer;
        int Write()
        {
            var size = CharacterListS21Ref.GetRequiredSize(account.Characters.Count);
            var span = connection.Output.GetSpan(size)[..size];
            var packet = new CharacterListS21Ref(span)
            {
                SpecialCharacter = 5, // unlock all but tbh only 4 classes is locked
                CharacterCount = (byte)account.Characters.Count,
                CharacterSlotCount = (byte)account.Characters.Count,
                DailyCharCreation = 10,
                IsVaultExtended = account.IsVaultExtended,
            };

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
            }

            return size;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);
    }

    private static CharacterCreationUnlockFlags CreateUnlockFlags(Account account)
    {
        byte aggregatedFlags = 0;
        var result = account.UnlockedCharacterClasses?
            .Select(c => c.CreationAllowedFlag)
            .Aggregate(aggregatedFlags, (current, flag) => (byte)(current | flag)) ?? 0;
        return (CharacterCreationUnlockFlags)result;
    }
}
