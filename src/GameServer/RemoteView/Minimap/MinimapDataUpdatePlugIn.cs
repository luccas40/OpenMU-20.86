// <copyright file="MinimapDataUpdatePlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Minimap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.PlugIns;
using MUnique.OpenMU.GameLogic.Views.Minimap;
using MUnique.OpenMU.GameServer.RemoteView.MuHelper;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IMinimapDataUpdatePlugIn"/>
/// which sends the minimap data to the client.
/// </summary>
[PlugIn(nameof(MinimapDataUpdatePlugIn), "Sends the minimap data to the client.")]
[Guid("CF4B9FC7-CC2E-44F3-A645-9B279570C7AB")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class MinimapDataUpdatePlugIn(RemotePlayer player) : IMinimapDataUpdatePlugIn
{
    private readonly RemotePlayer _player = player;

    /// <inheritdoc/>
    public async ValueTask UpdateMinimapAsync()
    {
        if (this._player.Connection is not { Connected: true } connection)
        {
            return;
        }

        await connection.SendMiniMapInfoS21Async(10, 1, 4, 0, 123, 135, "Lumen the Barmaid").ConfigureAwait(false);
    }
}
