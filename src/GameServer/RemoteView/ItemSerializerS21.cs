// <copyright file="ItemSerializerExtended.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView;

using System.Runtime.InteropServices;
using MUnique.OpenMU.DataModel;
using MUnique.OpenMU.DataModel.Configuration;
using MUnique.OpenMU.DataModel.Configuration.Items;
using MUnique.OpenMU.DataModel.Entities;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.Persistence;
using MUnique.OpenMU.PlugIns;

using static ItemSerializerHelper;

/// <summary>
/// This item serializer is used to serialize the item data to the data packets.
/// At the moment, each item is serialized into a dynamical length 5 to 15-byte
/// long part of an array.
/// </summary>
[Guid("A1C9BEBF-FD23-4F89-A2CD-AE9F4CF7D652")]
[PlugIn("Item Serializer S21", "The S21 item serializer. It's most likely only correct for season 21.")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
public class ItemSerializerS21 : IItemSerializer
{
    [Flags]
    private enum OptionFlags : byte
    {
        None = 0x00,
        HasOption = 0x01,
        HasLuck = 0x02,
        HasSkill = 0x04,
        HasExcellent = 0x08,
        HasAncient = 0x10,
        HasHarmony = 0x20,
        HasGuardian = 0x40,
        HasSockets = 0x80,
    }

    /// <inheritdoc/>
    public int NeededSpace => 15;

    /// <inheritdoc/>
    public int SerializeItem(Span<byte> target, Item item)
    {
        item.ThrowNotInitializedProperty(item.Definition is null, nameof(item.Definition));
        var targetStruct = new ItemStruct(target);
        targetStruct.ItemID = (ushort)((item.Definition.Group * 512) + item.Definition.Number);
        targetStruct.Level = item.IsTrainablePet() ? (byte)0 : (byte)item.Level;
        targetStruct.Durability = item.Durability();
        targetStruct.Luck = item.ItemOptions.Any(o => o.ItemOption?.OptionType == ItemOptionTypes.Luck);
        targetStruct.Skill = item.HasSkill;

        if (item.ItemOptions.FirstOrDefault(o => o.ItemOption?.OptionType == ItemOptionTypes.Option) is { } itemOption)
        {
            targetStruct.Option = (byte)(itemOption.Level & 0xF);
        }

        targetStruct.Unk = 0xFF;
        targetStruct.Unk2 = 0xFF;
        targetStruct.Unk3 = 0xFF;
        targetStruct.Unk4 = 0xFF;
        targetStruct.Unk5 = 0xFF;
        targetStruct.Unk6 = 0xFF;
        targetStruct.Unk7 = 0xFF;
        targetStruct.Unk8 = 0xFF;
        targetStruct.Excellent = GetExcellentByte(item);
        targetStruct.Excellent |= GetFenrirByte(item);

        /*
        if (targetStruct.Options.HasFlag(OptionFlags.HasAncient)
            && item.ItemSetGroups.FirstOrDefault(set => set.AncientSetDiscriminator != 0) is { } ancientSet)
        {
            targetStruct.AncientDiscriminator = (byte)ancientSet.AncientSetDiscriminator;

            // An ancient item may or may not have an ancient bonus option. Example without bonus: Gywen Pendant.
            if (item.ItemOptions.FirstOrDefault(o => o.ItemOption?.OptionType == ItemOptionTypes.AncientBonus) is { } ancientBonus)
            {
                targetStruct.AncientBonusLevel = (byte)ancientBonus.Level;
            }
        }

        if (targetStruct.Options.HasFlag(OptionFlags.HasHarmony))
        {
            targetStruct.Harmony = GetHarmonyByte(item);
        }

        if (targetStruct.Options.HasFlag(OptionFlags.HasSockets))
        {
            targetStruct.SocketCount = (byte)item.SocketCount;
            targetStruct.SocketBonus = GetSocketBonusByte(item);
            SetSocketBytes(targetStruct.Sockets, item);
        }
        */
        return 15;
    }

    /// <inheritdoc />
    public Item DeserializeItem(Span<byte> array, GameConfiguration gameConfiguration, IContext persistenceContext)
    {
        /*
        var itemStruct = new ItemStruct(array);
        var itemGroup = itemStruct.Group;
        var itemNumber = itemStruct.Number;
        var definition = gameConfiguration.Items.FirstOrDefault(def => def.Number == itemNumber && def.Group == itemGroup)
                         ?? throw new ArgumentException($"Couldn't find the item definition for the given byte array. Extracted item number and group: {itemNumber}, {itemGroup}");

        var item = persistenceContext.CreateNew<Item>();
        item.Definition = definition;
        item.Level = itemStruct.Level;
        item.Durability = itemStruct.Durability;
        item.HasSkill = itemStruct.Options.HasFlag(OptionFlags.HasSkill) && item.Definition?.Skill is not null;

        if (itemStruct.Options.HasFlag(OptionFlags.HasOption))
        {
            AddNormalOption(itemStruct.OptionType, itemStruct.OptionLevel, persistenceContext, item);
        }

        if (itemStruct.Options.HasFlag(OptionFlags.HasLuck))
        {
            AddLuckOption(persistenceContext, item);
        }

        if (itemStruct.Options.HasFlag(OptionFlags.HasExcellent))
        {
            if (item.IsWing())
            {
                ReadWingOptionBits(itemStruct.Excellent, persistenceContext, item);
            }
            else
            {
                ReadExcellentOptionBits(itemStruct.Excellent, persistenceContext, item);
            }
        }

        if (itemStruct.Options.HasFlag(OptionFlags.HasAncient))
        {
            AddAncientOption(itemStruct.AncientDiscriminator, itemStruct.AncientBonusLevel, persistenceContext, item);
        }

        if (itemStruct.Options.HasFlag(OptionFlags.HasHarmony))
        {
            AddHarmonyOption(itemStruct.Harmony, persistenceContext, item);
        }

        if (itemStruct.Options.HasFlag(OptionFlags.HasGuardian))
        {
            AddLevel380Option(persistenceContext, item);
        }

        if (itemStruct.Options.HasFlag(OptionFlags.HasSockets))
        {
            ReadSocketBonus(itemStruct.SocketBonus, persistenceContext, item);
            ReadSockets(itemStruct.Sockets, persistenceContext, item);
        }

        return item;
        */
        throw new NotImplementedException();
    }

    private OptionFlags GetOptionFlags(Item item)
    {
        OptionFlags result = default;
        if (item.ItemOptions.Any(o => o.ItemOption?.OptionType == ItemOptionTypes.Luck))
        {
            result |= OptionFlags.HasLuck;
        }

        if (item.HasSkill)
        {
            result |= OptionFlags.HasSkill;
        }

        if (item.ItemOptions.Any(o => o.ItemOption?.OptionType == ItemOptionTypes.Option))
        {
            result |= OptionFlags.HasOption;
        }

        if (item.ItemOptions.Any(o => o.ItemOption?.OptionType == ItemOptionTypes.Excellent
                                      || o.ItemOption?.OptionType == ItemOptionTypes.Wing
                                      || o.ItemOption?.OptionType == ItemOptionTypes.BlackFenrir
                                      || o.ItemOption?.OptionType == ItemOptionTypes.BlueFenrir
                                      || o.ItemOption?.OptionType == ItemOptionTypes.GoldFenrir))
        {
            result |= OptionFlags.HasExcellent;
        }

        if (item.ItemOptions.Any(o => o.ItemOption?.OptionType == ItemOptionTypes.HarmonyOption))
        {
            result |= OptionFlags.HasHarmony;
        }

        if (item.ItemSetGroups.Any(set => set.AncientSetDiscriminator != 0))
        {
            result |= OptionFlags.HasAncient;
        }

        if (item.ItemOptions.Any(o => o.ItemOption?.OptionType == ItemOptionTypes.GuardianOption))
        {
            result |= OptionFlags.HasGuardian;
        }

        if (item.SocketCount > 0)
        {
            result |= OptionFlags.HasSockets;
        }

        return result;
    }

    /// <summary>
    /// Layout:
    ///   Group:  4 bit
    ///   Number: 12 bit
    ///   Level:  4 bit
    ///   Dura:   8 bit
    ///   OptFlags: 8 bit
    ///     HasOpt
    ///     HasLuck
    ///     HasSkill
    ///     HasExc
    ///     HasAnc
    ///     HasGuardian
    ///     HasHarmony
    ///     HasSockets
    ///   Optional, depending on Flags:
    ///     Opt_Lvl 4 bit
    ///     Opt_Typ 4 bit
    ///     Exc:    8 bit
    ///     Anc_Dis 4 bit
    ///     Anc_Bon 4 bit
    ///     Harmony 8 bit
    ///     Soc_Bon 4 bit
    ///     Soc_Cnt 4 bit
    ///     Sockets n * 8 bit
    ///
    ///  Total: 5 ~ 15 bytes.
    ///  1st byte  = left to right
    ///  Skill 1 bit
    ///  Level 4 bit = max lvl 15 or 0xF
    ///  Luck 1 bit
    ///  Option 2 bit and continue
    ///  if option value is greater than 3
    ///  3rt byte 
    /// </summary>
    private readonly ref struct ItemStruct(Span<byte> data)
    {
        private readonly Span<byte> _data = data;

        public ushort ItemID
        {
            get => (ushort)(this._data[0] | (this._data[3] & 0x80) << 1 | this._data[5] & 0xf0 << 5 | (this._data[5] & 1) << 0xd);
            set
            {
                this._data[0] |= (byte)value;
                this._data[3] |= (byte)((value & 0x100) >> 1);
                this._data[5] |= (byte)((value & 0x1E00) >> 5);
                this._data[5] |= (byte)((value & 0x2000) >> 13);
            }
        }

        public byte Level
        {
            get => (byte)(this._data[1] >> 3 & 0xF);
            set => this._data[1] |= (byte)(value << 3);
        }

        public bool Skill
        {
            get => ((this._data[1] & 128) >> 7) == 1;
            set => this._data[1] |= (byte)(value ? 0x80 : 0);
        }

        public bool Luck
        {
            get => (this._data[1] & 4) == 4;
            set => this._data[1] |= (byte)(value ? 4 : 0);
        }

        public int Option
        {
            get => (this._data[1] & 3) + (((this._data[3] & 0x40) >> 6) * 4);
            set
            {
                value &= 7;

                // Set low 2 bits
                this._data[1] = (byte)((this._data[1] & ~3) | (value & 3));

                // Bit 6 -> _data[3]  (0x40)
                if ((value & 4) != 0)
                {
                    this._data[3] |= 0x40;   // set bit 6
                }
                else
                {
                    this._data[3] &= 0xBF;   // clear bit 6
                }
            }
        }

        public byte Durability
        {
            get => this._data[2];
            set => this._data[2] = value;
        }

        public byte Excellent
        {
            get => (byte)(this._data[3] & 0x3f);
            set => this._data[3] = (byte)((this._data[3] & 0xC0) | (value & 0x3F));
        }

        public int Unk8
        {
            get => this._data[7];
            set => this._data[7] = Convert.ToByte(value);
        }

        public int Unk
        {
            get => this._data[8];
            set => this._data[8] = Convert.ToByte(value);
        }

        public int Unk2
        {
            get => this._data[9];
            set => this._data[9] = Convert.ToByte(value);
        }

        public int Unk3
        {
            get => this._data[10];
            set => this._data[10] = Convert.ToByte(value);
        }

        public int Unk4
        {
            get => this._data[11];
            set => this._data[11] = Convert.ToByte(value);
        }

        public int Unk5
        {
            get => this._data[12];
            set => this._data[12] = Convert.ToByte(value);
        }

        public int Unk6
        {
            get => this._data[13];
            set => this._data[13] = Convert.ToByte(value);
        }

        public int Unk7
        {
            get => this._data[14];
            set => this._data[14] = Convert.ToByte(value);
        }
    }
}