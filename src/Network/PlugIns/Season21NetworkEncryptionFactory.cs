// <copyright file="Season21NetworkEncryptionFactory.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Network.PlugIns;

using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MUnique.OpenMU.Network.PacketOpcodeTranslator;
using MUnique.OpenMU.Network.SimpleModulus;
using MUnique.OpenMU.Network.Xor;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// A plugin which provides network encryptors and decryptors for english game clients of season 21 episode 0, version 1.20.86.
/// </summary>
[PlugIn("Network Encryption - Season 21 Episode 0?, 1.20.86, KR", "A plugin which provides network encryptors and decryptors for Korean game clients of season 21 episode 0?, version 1.20.86")]
[Guid("0e5370fd-d721-406b-998f-3cacdbb88aac")]
public class Season21NetworkEncryptionFactory : INetworkEncryptionFactoryPlugIn
{
    /// <inheritdoc/>
    public ClientVersion Key { get; } = new(21, 0, ClientLanguage.Korean);

    /// <inheritdoc/>
    public IPipelinedDecryptor? CreateDecryptor(PipeReader source, DataDirection direction)
    {
        return new S21PacketOpcodeDecryptor(source);
    }

    /// <inheritdoc/>
    public IPipelinedEncryptor? CreateEncryptor(PipeWriter target, DataDirection direction)
    {
        return new S21PacketOpcodeEncryptor(target);
    }
}
