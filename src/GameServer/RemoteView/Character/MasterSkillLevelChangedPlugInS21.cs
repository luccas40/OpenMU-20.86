// <copyright file="MasterSkillLevelChangedPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Character;

using System.Runtime.InteropServices;
using MUnique.OpenMU.DataModel.Entities;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.Views.Character;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IMasterSkillLevelChangedPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn("MasterSkillLevelChangedPlugInS21", "The S21 implementation of the IMasterSkillLevelChangedPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("5417C2D0-AABE-44B2-B757-D68CACEFB644")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class MasterSkillLevelChangedPlugInS21 : IMasterSkillLevelChangedPlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="MasterSkillLevelChangedPlugIn"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public MasterSkillLevelChangedPlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc />
    public async ValueTask MasterSkillLevelChangedAsync(SkillEntry skillEntry)
    {
        var character = this._player.SelectedCharacter;
        if (character?.CharacterClass is null || skillEntry.Skill is null)
        {
            return;
        }

        await this._player.Connection.SendMasterSkillLevelUpdateS21Async(
            true,
            (ushort)character.MasterLevelUpPoints,
            skillEntry.Skill.GetMasterSkillIndexS21(character.CharacterClass),
            (ushort)skillEntry.Skill.Number,
            (byte)skillEntry.Level,
            skillEntry.CalculateDisplayValue(),
            skillEntry.CalculateNextDisplayValue(),
            0).ConfigureAwait(false);
    }
}