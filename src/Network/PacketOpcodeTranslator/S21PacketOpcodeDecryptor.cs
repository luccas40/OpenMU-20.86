// <copyright file="S21PacketOpcodeDecryptor.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Network.PacketOpcodeTranslator;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Pipelined Decryptor which will translate headcodes.
/// It's typically used to tranlate headcodes from client to server.
/// S17+ Kr Clients are shuffling the packet headcodes every update so this "decryptor" is used to convert
/// the converntional headcodes into the ones that the server knows.
/// </summary>
public class S21PacketOpcodeDecryptor : PacketPipeReaderBase, IPipelinedDecryptor
{
    private readonly Pipe _pipe = new();
    private readonly Dictionary<ushort, ushort> _translator = new()
    {
        // Transformed, Original
        { 0x4d02, 0xF101 }, // Login Attempt
        { 0x5701, 0xF301 }, // Create Character Request
        { 0x5706, 0xF302 }, // Delete Character Request
        { 0x5726, 0xF300 }, // Request Character List
        { 0x5721, 0xF315 }, // Clicked Connect (Selected Character)
        { 0x5715, 0xF303 }, // Select Character Confirmation
        { 0x96FF, 0xD4FF }, // Select Character Confirmation
        { 0xA000, 0x24FF }, // Inventory Item move
        { 0xC3FF, 0x26FF }, // Inventory Consume
        { 0x83FF, 0xA9FF }, // Inventory Pet Info Request
        { 0xBF20, 0xBF20 }, // User Tries to Use Pet
        { 0x0EFF, 0x19FF }, // Target Skill Cast (tested with Force Wave Dl)
        { 0x23FF, 0x00FF }, // Public Chat Message
        { 0xC9FF, 0x18FF }, // Animation Request
        { 0x4E11, 0x4E11 }, // Muun Mount Request (Dl horse + guardian mounts are now muun somehow)
        { 0xE2FF, 0x11FF }, // Auto Attack Request
        { 0x4300, 0x0E00 }, // Ping
        { 0x4D01, 0xF102 }, // LogOut By Exiting / Character Selection / Server Selection
        { 0x5712, 0xF306 }, // LogOut By Exiting / Character Selection / Server Selection
    };
    /*

     *  Packet not Translated C-S>: E7 - 02 len 4
     *  Packet not Translated C-S>: C7 - 01 len 14
Packet not Translated C-S>: ED - 00 len 8
Packet not Translated C-S>: 22 - 31 len 4
    Packet not Translated C-S>: 22 - 31 len 4
    Packet not Translated C-S>: E7 - 02 len 4
Packet not Translated C-S>: E7 - 02 len 4
Packet not Translated C-S>: 57 - 60 len 4
Packet not Translated C-S>: 22 - 27 len 5
Packet not Translated C-S>: 90 - 3D len 5
    after trying to go back to character selection
    Packet not Translated C-S>: 57 - 30 len 41
Packet not Translated C-S>: 59 - 04 len 28
Packet not Translated C-S>: E7 - 02 len 4
Packet not Translated C-S>: 4D - 01 len 5
Packet not Translated C-S>: 43 - 00 len 12
     * */
    /// <summary>
    /// Initializes a new instance of the <see cref="S21PacketOpcodeDecryptor"/> class.
    /// </summary>
    /// <param name="source">The Source.</param>
    public S21PacketOpcodeDecryptor(PipeReader source)
    {
        this.Source = source;
        _ = this.ReadSourceAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public PipeReader Reader => this._pipe.Reader;

    /// <inheritdoc />
    protected override ValueTask OnCompleteAsync(Exception? exception)
    {
        return this._pipe.Writer.CompleteAsync(exception);
    }

    /// <summary>
    /// Reads the mu online packet.
    /// Decrypts the packet and writes it into our pipe.
    /// </summary>
    /// <param name="packet">The mu online packet.</param>
    /// <returns><see langword="true" />, if the flush was successful or not required.<see langword="false" />, if the pipe reader is completed and no longer reading data.</returns>
    protected override async ValueTask<bool> ReadPacketAsync(ReadOnlySequence<byte> packet)
    {
        this.DecryptAndWrite(packet);
        return await this.TryFlushWriterAsync(this._pipe.Writer).ConfigureAwait(false);
    }

    private void DecryptAndWrite(ReadOnlySequence<byte> packet)
    {
        // The next line is getting a span from the writer which is at least as big as the packet.
        // As I found out, it's initially about 2 kb in size and gets smaller within further
        // usage. If the previous span was used up, a new piece of memory is getting provided for us.
        var span = this._pipe.Writer.GetSpan((int)packet.Length);

        // we just want to work on a span with the exact size of the packet.
        var target = span.Slice(0, (int)packet.Length);
        packet.CopyTo(target);

        var headerSize = target.GetPacketHeaderSize();
        var code = target.GetPacketCode();
        var translated = this._translator.SingleOrDefault(h => h.Key == code || h.Key == (code | 0xFF)).Value;
        if (translated != 0)
        {
            target.SetPacketCode(translated);
            this._pipe.Writer.Advance(target.Length);
        }
        else
        {
            Debug.WriteLine($"Packet not Translated C-S>: {target.GetHeadCode():X2} - {target.GetSubcode():X2} len {target.Length}");
        }
    }
}
