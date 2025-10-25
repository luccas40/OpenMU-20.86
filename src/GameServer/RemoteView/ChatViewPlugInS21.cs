// <copyright file="ChatViewPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView;

using System.Runtime.InteropServices;
using MUnique.OpenMU.GameLogic.Views;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The S21 implementation of the chat view which is forwarding everything to the game client which specific data packets.
/// </summary>
[PlugIn("Chat View PlugIn S21", "View Plugin to send chat messages to the player")]
[Guid("7580901A-B3B3-4D6F-971F-9951DDB9A2B8")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class ChatViewPlugInS21 : IChatViewPlugIn
{
    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatViewPlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public ChatViewPlugInS21(RemotePlayer player)
    {
        this._player = player;
    }

    /// <inheritdoc/>
    public async ValueTask ChatMessageAsync(string message, string sender, ChatMessageType type)
    {
        await this._player.Connection.SendChatMessageS21Async(ConvertChatMessageType(type), sender, message).ConfigureAwait(false);
    }

    private static ChatMessageS21.ChatMessageType ConvertChatMessageType(ChatMessageType type)
    {
        if (type == ChatMessageType.Whisper)
        {
            return Network.Packets.ServerToClient.ChatMessageS21.ChatMessageType.Whisper;
        }

        return Network.Packets.ServerToClient.ChatMessageS21.ChatMessageType.Normal;
    }
}