namespace MUnique.OpenMU.GameServer.MessageHandler.Majestic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameServer.MessageHandler.MuHelper;
using MUnique.OpenMU.Network.Packets.ClientToServer;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// Handler for MU Majestic Skill Tree Add requests.
/// </summary>
[PlugIn(nameof(MajesticSkillTreeAddPlugIn), "Handler for MU Helper status change requests.")]
[Guid("39E20BF4-D667-4BBA-872C-40F1188DFB19")]

[BelongsToGroup(MajesticSkillTreeGroupHandler.GroupKey)]
public class MajesticSkillTreeAddPlugIn : ISubPacketHandlerPlugIn
{
    /// <inheritdoc/>
    public bool IsEncryptionExpected => false;

    /// <inheritdoc/>
    public byte Key => 01;

    /// <inheritdoc/>
    public async ValueTask HandlePacketAsync(Player player, Memory<byte> packet)
    {
        MajesticSkillTreeAddRequest request = packet;
    }
}
