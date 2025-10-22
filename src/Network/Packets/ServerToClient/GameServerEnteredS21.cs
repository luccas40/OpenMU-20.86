// <copyright file="GameEnteredS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Network.Packets.ServerToClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// This part contains the custom indexer.
/// </summary>
public readonly ref partial struct GameServerEnteredS21Ref
{
    /// <summary>
    /// Gets or sets the PlayerId when connected.
    /// </summary>
    public ushort PlayerId
    {
        get
        {
            return (ushort)(this._data[6] << 8 | this._data[11]);
        }
        set
        {
            this._data[6] = (byte)(value >> 8);
            this._data[11] = (byte)(value & 0xFF);
        }
    }
}
