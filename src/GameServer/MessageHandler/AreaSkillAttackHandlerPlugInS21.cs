// <copyright file="AreaSkillAttackHandlerPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.MessageHandler;

using System.Runtime.InteropServices;
using MUnique.OpenMU.DataModel.Configuration;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.PlayerActions.Skills;
using MUnique.OpenMU.Network.Packets.ClientToServer;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.Pathfinding;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// Handler for area skill attack packets.
/// </summary>
[PlugIn(nameof(AreaSkillAttackHandlerPlugInS21), "Handler for area skill attack packets.")]
[Guid("CD590FBE-6145-416B-84FE-99C411FAE5B9")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
internal class AreaSkillAttackHandlerPlugInS21 : IPacketHandlerPlugIn
{
    private readonly AreaSkillAttackAction _attackAction = new();

    /// <inheritdoc/>
    public bool IsEncryptionExpected => false;

    /// <inheritdoc/>
    public byte Key => AreaSkillS21.Code;

    /// <inheritdoc/>
    public async ValueTask HandlePacketAsync(Player player, Memory<byte> packet)
    {
        AreaSkillS21 message = packet;
        if (player.SkillList is null || !player.SkillList.ContainsSkill(message.SkillId))
        {
            return;
        }

        if (player.SkillList.GetSkill(message.SkillId) is { Skill.SkillType: SkillType.AreaSkillExplicitHits })
        {
            // we don't need to return if it fails - it doesn't cause any damage, and the player
            // still "pays" the mana and ag.
            player.SkillHitValidator.TryRegisterAnimation(message.SkillId, message.AnimationCounter);
        }

        await this._attackAction.AttackAsync(player, message.ExtraTargetId, message.SkillId, new Point((byte)message.TargetX, (byte)message.TargetY), message.Rotation).ConfigureAwait(false);
    }
}