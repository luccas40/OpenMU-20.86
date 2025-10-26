// <copyright file="MapChangePlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.World;

using System.Runtime.InteropServices;
using MUnique.OpenMU.GameLogic.Views.World;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IMapChangePlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn("MapChangePlugInS21", "The S21 implementation of the IMapChangePlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("67706210-6A88-49A4-9791-9860F20512A8")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
public class MapChangePlugInS21 : IMapChangePlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="MapChangePlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public MapChangePlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc/>
    public ValueTask MapChangeAsync()
    {
        return this.SendMessageAsync(true);
    }

    /// <inheritdoc/>
    public ValueTask MapChangeFailedAsync()
    {
        return this.SendMessageAsync(false);
    }

    private async ValueTask SendMessageAsync(bool success)
    {
        if (this._player.SelectedCharacter?.CurrentMap is null)
        {
            return;
        }

        var mapNumber = this._player.SelectedCharacter.CurrentMap.Number.ToUnsigned();
        var position = this._player.IsWalking ? this._player.WalkTarget : this._player.Position;
        //await this._player.Connection.SendWarpResultAsync(0, 0).ConfigureAwait(false);
        await this._player.Connection.SendMapChangedS21Async(1, mapNumber, position.X, position.Y, this._player.Rotation.ToPacketByte()).ConfigureAwait(false);
    }
}