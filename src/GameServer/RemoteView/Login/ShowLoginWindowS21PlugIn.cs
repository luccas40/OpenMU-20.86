// <copyright file="ShowLoginWindowS21PlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Login;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MUnique.OpenMU.GameLogic.Views;
using MUnique.OpenMU.GameLogic.Views.Login;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IShowLoginWindowPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn(nameof(ShowLoginWindowS21PlugIn), "The S21 implementation of the IShowLoginWindowPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("8a51db45-a0ff-4001-a06e-b2fbf0417be4")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class ShowLoginWindowS21PlugIn : IShowLoginWindowPlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShowLoginWindowS21PlugIn"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public ShowLoginWindowS21PlugIn(RemotePlayer player)
    {
        this._player = player;
    }

    /// <inheritdoc/>
    public async ValueTask ShowLoginWindowAsync()
    {
        var connection = this._player.Connection;
        if (connection is null)
        {
            return;
        }

        int Write()
        {
            var size = GameServerEnteredS21Ref.Length;
            var span = connection.Output.GetSpan(size)[..size];
            var packet = new GameServerEnteredS21Ref(span)
            {
                PlayerId = ViewExtensions.ConstantPlayerId,
            };

            ClientVersionResolver.Resolve(this._player.ClientVersion).CopyTo(packet.Version);

            return size;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);
    }
}
