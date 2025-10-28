// <copyright file="ItemPickUpResponse.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Network.Packets.ServerToClient;

/// <summary>
/// This part contains the custom indexer.
/// </summary>
public readonly ref partial struct ItemPickUpResponseRef
{
    /// <summary>
    /// Gets or sets the Drop ID.
    /// </summary>
    public ushort DropId
    {
        get => (ushort)(this._data[4] << 8 | this._data[6]);
        set
        {
            this._data[4] = (byte)(value >> 8);
            this._data[6] = (byte)(value & 0xFF);
        }
    }

}
