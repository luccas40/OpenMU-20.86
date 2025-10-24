// <copyright file="ShowMountPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.World;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MUnique.OpenMU.DataModel.Entities;
using MUnique.OpenMU.GameLogic.Views;
using MUnique.OpenMU.GameLogic.Views.World;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// Initializes a new instance of the <see cref="ShowVisibleMountPlugIn"/> class.
/// </summary>
/// <param name="player">The player.</param>
[PlugIn("ShowVisibleMountPlugIn", "The default implementation of the IShowMountPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("1752AC0C-D439-41CE-83FB-FAD42A70E8E5")]
[MaximumClient(21, 0, ClientLanguage.Korean)]
public class ShowVisibleMountPlugIn(RemotePlayer player) : IShowVisibleMountPlugIn
{
    private readonly RemotePlayer _player = player;

    /// <inheritdoc/>
    public ValueTask ShowVisibleMountAsync(ushort Mount)
    {
        return this._player.Connection.SendShowVisibleMountAsync(1, this._player.GetId(this._player), Mount, 0, 0);
    }
}
