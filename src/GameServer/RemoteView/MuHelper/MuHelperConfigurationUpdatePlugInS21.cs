// <copyright file="MuHelperConfigurationUpdatePlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.MuHelper;

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MUnique.OpenMU.GameLogic.Views.MuHelper;
using MUnique.OpenMU.GameServer.RemoteView;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IMuHelperConfigurationUpdatePlugIn"/>
/// which sends the new MU Helper status to the client.
/// </summary>
[PlugIn(nameof(MuHelperConfigurationUpdatePlugInS21), "Sends the new S21 MU Helper status to the client.")]
[Guid("418C42BD-9224-43E9-A520-792DC4F58A1C")]
[MinimumClient(21, 0 ,Network.PlugIns.ClientLanguage.Korean)]
public class MuHelperConfigurationUpdatePlugInS21 : IMuHelperConfigurationUpdatePlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="MuHelperConfigurationUpdatePlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public MuHelperConfigurationUpdatePlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc />
    public async ValueTask UpdateMuHelperConfigurationAsync(Memory<byte> data)
    {
        if (this._player.Connection is not { Connected: true } connection)
        {
            return;
        }

        await connection.SendMuHelperConfigurationDataS21Async(data).ConfigureAwait(false);
    }
}