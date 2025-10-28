// <copyright file="UpdateMasterStatsPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Character;

using System.Runtime.InteropServices;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.Attributes;
using MUnique.OpenMU.GameLogic.Views.Character;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IUpdateMasterStatsPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn("UpdateMasterStatsPlugInS21", "The S21 implementation of the IUpdateMasterStatsPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("BDE76C3C-0F60-4C54-9CD7-906076781698")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class UpdateMasterStatsPlugInS21 : IUpdateMasterStatsPlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMasterStatsPlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public UpdateMasterStatsPlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc />
    public async ValueTask SendMasterStatsAsync()
    {
        var character = this._player.SelectedCharacter;
        var connection = this._player.Connection;
        if (character is null || this._player.Attributes is null || connection is null)
        {
            return;
        }

        var combinedLevel = this._player.Attributes[Stats.MasterLevel] + this._player.Attributes[Stats.MajesticLevel];
        ulong exp = 0;
        long nextLevel = 0;
        if (character.CharacterClass!.IsMajesticClass)
        {
            exp = (ulong)character.MajesticExperience;
            nextLevel = this._player.GameContext.MajesticExperienceTable[(int)this._player.Attributes[Stats.MajesticLevel] + 1];
        }
        else
        {
            exp = (ulong)character.MasterExperience;
            nextLevel = this._player.GameContext.MasterExperienceTable[(int)this._player.Attributes[Stats.MasterLevel] + 1];
        }

        await connection.SendMasterStatsUpdateAsync(
                (ushort)combinedLevel,
                exp,
                (ulong)nextLevel,
                (ushort)character.MasterLevelUpPoints,
                (ushort)this._player.Attributes[Stats.MaximumHealth],
                (ushort)this._player.Attributes[Stats.MaximumMana],
                (ushort)this._player.Attributes[Stats.MaximumShield],
                (ushort)this._player.Attributes[Stats.MaximumAbility]).ConfigureAwait(false);

        await this._player.InvokeViewPlugInAsync<IUpdateMasterSkillsPlugIn>(p => p.UpdateMasterSkillsAsync()).ConfigureAwait(false);
    }
}