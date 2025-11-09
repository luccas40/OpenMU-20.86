// <copyright file="MinimapOpenHandlerPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.MessageHandler.Minimap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.Views.Minimap;
using MUnique.OpenMU.GameServer.MessageHandler.MuHelper;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// Handler for MU Helper use requests.
/// </summary>
[PlugIn(nameof(MinimapOpenHandlerPlugIn), "Handler for minimap stuff.")]
[Guid("6B7BFFF3-E7F5-44EA-B439-C67E84A1229D")]
[BelongsToGroup(MinimapGroupHandler.GroupKey)]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class MinimapOpenHandlerPlugIn : ISubPacketHandlerPlugIn
{
    /// <inheritdoc/>
    public bool IsEncryptionExpected => false;

    /// <inheritdoc/>
    public byte Key => 1;

    /// <inheritdoc/>
    public async ValueTask HandlePacketAsync(Player player, Memory<byte> packet)
    {
        await player.InvokeViewPlugInAsync<IMinimapDataUpdatePlugIn>(p => p.UpdateMinimapAsync()).ConfigureAwait(false);
    }
}
