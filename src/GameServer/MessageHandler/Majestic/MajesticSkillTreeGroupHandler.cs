// <copyright file="MajesticSkillTreeGroupHandler.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.MessageHandler.MuHelper;

using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// Packet handler for mu majestic skill tree packets (0x7E identifier).
/// </summary>
[PlugIn(nameof(MajesticSkillTreeGroupHandler), "Group packet handler for mu majestic skill tree (0x7E identifier).")]
[Guid("AEFEF5D9-C12D-4041-AC78-B9343FD3C1B9")]
internal class MajesticSkillTreeGroupHandler : GroupPacketHandlerPlugIn
{
    /// <summary>
    /// The group key.
    /// </summary>
    internal const byte GroupKey = 0x7E;

    /// <summary>
    /// Initializes a new instance of the <see cref="MajesticSkillTreeGroupHandler" /> class.
    /// </summary>
    /// <param name="clientVersionProvider">The client version provider.</param>
    /// <param name="manager">The manager.</param>
    /// <param name="loggerFactory">The logger.</param>
    public MajesticSkillTreeGroupHandler(
        IClientVersionProvider clientVersionProvider,
        PlugInManager manager,
        ILoggerFactory loggerFactory)
        : base(clientVersionProvider, manager, loggerFactory)
    {
    }

    /// <inheritdoc/>
    public override bool IsEncryptionExpected => false;

    /// <inheritdoc/>
    public override byte Key => GroupKey;
}