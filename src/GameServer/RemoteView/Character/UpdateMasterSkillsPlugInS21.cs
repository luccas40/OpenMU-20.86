// <copyright file="UpdateMasterSkillsPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Character;

using System.Runtime.InteropServices;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.Views.Character;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IUpdateMasterSkillsPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn("UpdateMasterSkillsPlugInS21", "The default implementation of the IUpdateMasterSkillsPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("57BCA385-091F-4C4F-834B-A24C7283F088")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class UpdateMasterSkillsPlugInS21 : IUpdateMasterSkillsPlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMasterSkillsPlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public UpdateMasterSkillsPlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc />
    public async ValueTask UpdateMasterSkillsAsync()
    {
        var connection = this._player.Connection;
        if (connection is null)
        {
            return;
        }

        var masterSkills = this._player.SkillList?.Skills.Where(s => s.Skill?.MasterDefinition != null).ToList();
        if (masterSkills is null || this._player.SelectedCharacter?.CharacterClass is null)
        {
            return;
        }

        int Write()
        {
            var size = MasterSkillListS21Ref.GetRequiredSize(masterSkills.Count);
            var span = connection.Output.GetSpan(size)[..size];
            var packet = new MasterSkillListS21Ref(span)
            {
                MasterSkillCount = (uint)masterSkills.Count,
            };

            int i = 0;
            foreach (var masterSkill in masterSkills)
            {
                var skillsBlock = packet[i];
                skillsBlock.MasterSkillIndex = masterSkill.Skill!.GetMasterSkillIndexS21(this._player.SelectedCharacter.CharacterClass);
                skillsBlock.Level = (byte)masterSkill.Level;
                skillsBlock.DisplayValue = masterSkill.CalculateDisplayValue();
                skillsBlock.DisplayValueOfNextLevel = masterSkill.CalculateNextDisplayValue();
                skillsBlock.Type = 0; // ??
                i++;
            }

            return size;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);
    }
}