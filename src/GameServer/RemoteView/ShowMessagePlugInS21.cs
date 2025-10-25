// <copyright file="ShowMessagePlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView;

using System.Runtime.InteropServices;
using MUnique.OpenMU.GameLogic.Views;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IShowMessagePlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn("ShowMessagePlugInS21", "The S21 implementation of the IShowMessagePlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("3550B66F-CFA8-488A-A7A5-52FA40A847DC")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
public class ShowMessagePlugInS21 : IShowMessagePlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShowMessagePlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public ShowMessagePlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc/>
    public async ValueTask ShowMessageAsync(string message, OpenMU.Interfaces.MessageType messageType)
    {
        const int maxMessageLength = 241;

        if (Encoding.UTF8.GetByteCount(message) > maxMessageLength)
        {
            var rest = message;
            while (rest.Length > 0)
            {
                var partSize = Encoding.UTF8.GetCharacterCountOfMaxByteCount(rest, maxMessageLength);
                await this.ShowMessageAsync(rest.Substring(0, partSize), messageType).ConfigureAwait(false);
                rest = rest.Length > partSize ? rest.Substring(startIndex: partSize) : string.Empty;
            }

            return;
        }

        await this._player.Connection.SendServerMessageS21Async(ConvertMessageType(messageType), (byte)message.Length, 0, 0xFFFFFFFF, 0, message).ConfigureAwait(false);
    }

    private static ServerMessageS21.MessageType ConvertMessageType(OpenMU.Interfaces.MessageType messageType)
    {
        return messageType switch
        {
            Interfaces.MessageType.BlueNormal => ServerMessageS21.MessageType.BlueNormal,
            Interfaces.MessageType.GoldenCenter => ServerMessageS21.MessageType.GoldenCenter,
            Interfaces.MessageType.GuildNotice => ServerMessageS21.MessageType.GuildNotice,
            _ => throw new NotImplementedException($"Case for {messageType} is not implemented."),
        };
    }
}