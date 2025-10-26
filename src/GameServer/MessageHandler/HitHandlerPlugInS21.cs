// <copyright file="HitHandlerPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.MessageHandler;

using System.Runtime.InteropServices;
using MUnique.OpenMU.Network.Packets.ClientToServer;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// Handler for hit packets.
/// </summary>
[PlugIn("HitHandlerPlugInS21", "Handler for hit packets.")]
[Guid("EA916ACC-F093-4988-9BAA-338A88406B23")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
internal class HitHandlerPlugInS21 : HitHandlerPlugInBase
{
    /// <inheritdoc/>
    public override byte Key => HitRequest.Code;
}