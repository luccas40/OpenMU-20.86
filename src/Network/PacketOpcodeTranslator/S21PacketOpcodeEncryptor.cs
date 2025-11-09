// <copyright file="S21PacketOpcodeEncryptor.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Network.PacketOpcodeTranslator;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Pipelined encryptor which will translate headcodes.
/// It's typically used to tranlate headcodes from server to client.
/// S17+ Kr Clients are shuffling the packet headcodes every update so this "encryptor" is used to convert
/// the converntional headcodes into the ones that the client knows.
/// </summary>
public class S21PacketOpcodeEncryptor : PacketPipeReaderBase, IPipelinedEncryptor
{
    private readonly PipeWriter _target;
    private readonly Pipe _pipe = new();
    private readonly Dictionary<ushort, ushort> _translator = new()
    {
        // Original, Transformed
        { 0xF100, 0x3700 }, // Show Login Box
        { 0xF101, 0x37FE }, // Login Response
        { 0xF102, 0x3702 }, // LogOut
        { 0xF300, 0x8130 }, // Character List Response
        { 0xF301, 0x8153 }, // Show Character Create
        { 0xF302, 0x8105 }, // Delete Character Response
        { 0xF303, 0x8122 }, // CharacterJoin
        { 0xF310, 0x8101 }, // CharacterInventory
        { 0xF311, 0x8113 }, // Character Skill List Update
        { 0xF314, 0x8115 }, // Inventory Item Upgraded
        { 0xF315, 0x8123 }, // Character Select Confirm
        { 0xF330, 0x8106 }, // Apply Key Configuration
        { 0xF350, 0x8114 }, // Master Infos
        { 0xF352, 0x8140 }, // Master Skill Level Update
        { 0xF353, 0x8111 }, // Master Skill List
        { 0x12FF, 0x12FF }, // Add Player to Scope
        { 0x13FF, 0x13FF }, // Add Creature to Scope
        { 0x14FF, 0x14FF }, // Remove Object from Scope
        { 0x24FF, 0xA7FF }, // Inventory Item Move Result
        { 0x25FF, 0x25FF }, // Appearance Changed
        { 0x26FF, 0xCAFF }, // Update Character Health and Shield, current and max
        { 0x27FF, 0x27FF }, // Update Character Mana and Ability, current and max
        { 0x28FF, 0x28FF }, // Inventory Item Removed
        { 0x2AFF, 0x2AFF }, // Item Durability Changed
        { 0xEC29, 0xC710 }, // Update Character Sheet
        { 0xEC30, 0xC755 }, // Update Character Attack and Magic Speed
        { 0xEC63, 0xC726 }, // Update Character Damage and Combat Power / subcode 63 is made of, there wasnt a subcode for s16
        { 0xD4FF, 0x5AFF }, // Object in Scope Moved
        { 0x15FF, 0xC1FF }, // Object in Scope Moved Instant
        { 0xA9FF, 0x32FF }, // Pet Info Response
        { 0xBF20, 0xBF20 }, // Item Use Response
        { 0x4E14, 0x4E14 }, // Muun Single Mount
        { 0x5900, 0x5900 }, // Character Specialization
        { 0x19FF, 0x19FF }, // Skill Animation
        { 0x11FF, 0x52FF }, // Object Hit
        { 0x0DFF, 0x0DFF }, // Server Message
        { 0x00FF, 0x00FF }, // Public Chat Message
        { 0x18FF, 0x82FF }, // Animation Result
        { 0x17FF, 0x17FF }, // Object got Killed
        { 0xF304, 0x8107 }, // Respawn
        { 0x9CFF, 0x9CFF }, // Exp Gained { 0x16FF, 0x16FF }
        { 0xF305, 0x8103 }, // Normal Level Up
        { 0xF306, 0x8110 }, // Add Stat Result
        { 0x1CFF, 0x1CFF }, // Teleport Result / Contains Warp between maps
        { 0x20FF, 0x20FF }, // Item Dropped
        { 0x22FF, 0xC2FF }, // Item PickUp Answer
        { 0x21FF, 0x21FF }, // Ground Item Remove
        { 0x23FF, 0x83FF }, // Drop Bag Item Response
        { 0x31FF, 0x31FF }, // Store Item List
        { 0x30FF, 0x4DFF }, // Npc Window Response
        { 0x0FFF, 0x0FFF }, // Weather Update
        { 0x81FF, 0xA2FF }, // Vault Update Money
        { 0x83FF, 0x22FF }, // Vault Protection Information
        { 0x1EFF, 0x1EFF }, // Area Skill Animation
        { 0xAEFF, 0xAEFF }, // Mu Helper Configuration Data
        { 0xBF51, 0xBF51 }, // MuHelper Toggle Status
        { 0xE703, 0xE703 }, // Minimap Data Send
        { 0x7EFF, 0x7EFF }, // Majestic Tree, not needed to translate

        // { 0x8E03, 0x8E03 }, // Warp result (testing)
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="S21PacketOpcodeEncryptor"/> class.
    /// </summary>
    /// <param name="target">The target.</param>
    public S21PacketOpcodeEncryptor(PipeWriter target)
    {
        this.Source = this._pipe.Reader;
        this._target = target;
        _ = this.ReadSourceAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public PipeWriter Writer => this._pipe.Writer;

    /// <inheritdoc/>
    protected override ValueTask OnCompleteAsync(Exception? exception)
    {
        return this._target.CompleteAsync(exception);
    }

    /// <inheritdoc/>
    protected override async ValueTask<bool> ReadPacketAsync(ReadOnlySequence<byte> packet)
    {
        this.EncryptAndWrite(packet);
        return await this.TryFlushWriterAsync(this._target).ConfigureAwait(false);
    }

    private void EncryptAndWrite(ReadOnlySequence<byte> packet)
    {
        var span = this._target.GetSpan((int)packet.Length);
        var result = span.Slice(0, (int)packet.Length);
        packet.CopyTo(result);

        var headerSize = result.GetPacketHeaderSize();
        var code = result.GetPacketCode();
        var translated = this._translator.SingleOrDefault(h => h.Key == code || h.Key == (code | 0xFF)).Value;
        if (translated != 0)
        {
            result.SetPacketCode(translated);
            if (result[0] == 0xC3) result[0] = 0xC1;
            if (result[0] == 0xC4) result[0] = 0xC2;
            this._target.Advance(result.Length);
        }
        else
        {
            Debug.WriteLine($"Packet not Translated S-C>: {result.GetHeadCode():X2} - {result.GetSubcode():X2}  len {result.Length}");
        }
    }
}

public static class Exntensions
{
    public static void SetPacketCode(this Span<byte> packet, ushort code)
    {
        int headerSize = packet.GetPacketHeaderSize();
        packet[headerSize] = (byte)(code >> 8);
        byte subcode = (byte)(code & 0xFF);
        if (subcode != 0xFF)
        {
            packet[headerSize + 1] = subcode;
        }
    }

    public static byte GetHeadCode(this Span<byte> packet) => packet[packet.GetPacketHeaderSize()];

    public static byte GetSubcode(this Span<byte> packet)
    {
        var headsize = packet.GetPacketHeaderSize() + 1;
        if (packet.Length > headsize)
        {
            return packet[headsize];
        }
        else
        {
            return 0xFF;
        }
    }

    public static ushort GetPacketCode(this Span<byte> packet) => (ushort)(packet.GetHeadCode() << 8 | packet.GetSubcode());
}
