// <copyright file="AppearanceSerializerS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MUnique.OpenMU.DataModel;
using MUnique.OpenMU.DataModel.Configuration.Items;
using MUnique.OpenMU.DataModel.Entities;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// S21 serializer for the appearance of a player.
/// </summary>
[Guid("9e4e6c3c-407d-46b7-9bec-69d624bfd2ec")]
[PlugIn("S21 appearance serializer", "S21 serializer for the appearance of a player. It will most likely only work correctly in season 21.")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
public class AppearanceSerializerS21 : IAppearanceSerializer
{
    /// <summary>
    /// A cache which holds the results of the serializer.
    /// </summary>
    private static readonly ConcurrentDictionary<IAppearanceData, byte[]> Cache = new();

    private enum PetIndex
    {
        Angel = 0,
        Imp = 1,
        Unicorn = 2,
        Dinorant = 3,
        DarkHorse = 4,
        DarkRaven = 5,
        Fenrir = 37,
        Demon = 64,
        SpiritOfGuardian = 65,
        Rudolph = 66,
        Panda = 80,
        PetUnicorn = 106,
        Skeleton = 123,
    }

    /// <inheritdoc/>
    public int NeededSpace => 20;

    /// <inheritdoc/>
    public void InvalidateCache(IAppearanceData appearance)
    {
        Cache.TryRemove(appearance, out _);
        appearance.AppearanceChanged -= this.OnAppearanceOfAppearanceChanged;
    }

    /// <inheritdoc/>
    public void WriteAppearanceData(Span<byte> target, IAppearanceData appearance, bool useCache)
    {
        if (target.Length < this.NeededSpace)
        {
            throw new ArgumentException($"Target span too small. Actual size: {target.Length}; Required: {this.NeededSpace}.", nameof(target));
        }

        if (useCache && Cache.TryGetValue(appearance, out var cached))
        {
            cached.CopyTo(target);
        }
        else
        {
            this.WritePreviewCharSet(target, appearance);
            if (useCache)
            {
                var cacheEntry = target.Slice(0, this.NeededSpace).ToArray();
                if (Cache.TryAdd(appearance, cacheEntry))
                {
                    appearance.AppearanceChanged += this.OnAppearanceOfAppearanceChanged;
                }
            }
        }
    }

    private void OnAppearanceOfAppearanceChanged(object? sender, EventArgs args) => this.InvalidateCache(sender as IAppearanceData ?? throw new ArgumentException($"sender must be of type {nameof(IAppearanceData)}"));

    private void WritePreviewCharSet(Span<byte> target, IAppearanceData appearanceData)
    {
        ItemAppearance?[] itemArray = new ItemAppearance[InventoryConstants.EquippableSlotsCount];
        for (byte i = 0; i < itemArray.Length; i++)
        {
            itemArray[i] = appearanceData.EquippedItems.FirstOrDefault(item => item.ItemSlot == i);
        }

        if (appearanceData.CharacterClass is not null)
        {
            target[0] = appearanceData.CharacterClass.Number;
        }

        //target[0] |= (byte)appearanceData.Pose;
        this.SetHand(target, itemArray[InventoryConstants.LeftHandSlot], 1, 12);

        this.SetHand(target, itemArray[InventoryConstants.RightHandSlot], 2, 13);

        this.SetArmorPiece(target, itemArray[InventoryConstants.HelmSlot], 3, true, 0x80, 13, false);

        this.SetArmorPiece(target, itemArray[InventoryConstants.ArmorSlot], 3, false, 0x40, 14, true);

        this.SetArmorPiece(target, itemArray[InventoryConstants.PantsSlot], 4, true, 0x20, 14, false);

        this.SetArmorPiece(target, itemArray[InventoryConstants.GlovesSlot], 4, false, 0x10, 15, true);

        this.SetArmorPiece(target, itemArray[InventoryConstants.BootsSlot], 5, true, 0x08, 15, false);

        this.SetItemLevels(target, itemArray);

        if (appearanceData.FullAncientSetEquipped)
        {
            target[11] |= 0x01;
        }

        this.AddWing(target, itemArray[InventoryConstants.WingsSlot]);

        this.AddPet(target, itemArray[InventoryConstants.PetSlot]);
    }

    private void SetHand(Span<byte> preview, ItemAppearance? item, int indexIndex, int groupIndex)
    {
        if (item?.Definition is null)
        {
            preview[indexIndex] = 0xFF;
            preview[groupIndex] |= 0xF0;
        }
        else
        {
            preview[indexIndex] = (byte)item.Definition.Number;
            preview[groupIndex] |= (byte)(item.Definition.Group << 5);
        }
    }

    private byte GetOrMaskForHighNibble(int value)
    {
        return (byte)((value << 4) & 0xF0);
    }

    private byte GetOrMaskForLowNibble(int value)
    {
        return (byte)(value & 0x0F);
    }

    private void SetEmptyArmor(Span<byte> preview, int firstIndex, bool firstIndexHigh, byte secondIndexMask, int thirdIndex, bool thirdIndexHigh)
    {
        // if the item is not equipped every index bit is set to 1
        preview[firstIndex] |= firstIndexHigh ? this.GetOrMaskForHighNibble(0x0F) : this.GetOrMaskForLowNibble(0x0F);
        preview[9] |= secondIndexMask;
        preview[thirdIndex] |= thirdIndexHigh ? this.GetOrMaskForHighNibble(0x0F) : this.GetOrMaskForLowNibble(0x0F);
    }

    private void SetArmorItemIndex(Span<byte> preview, ItemAppearance item, int firstIndex, bool firstIndexHigh, byte secondIndexMask, int thirdIndex, bool thirdIndexHigh)
    {
        preview[firstIndex] |= firstIndexHigh ? this.GetOrMaskForHighNibble(item.Definition!.Number) : this.GetOrMaskForLowNibble(item.Definition!.Number);
        byte multi = (byte)(item.Definition.Number / 16);
        if (multi > 0)
        {
            byte bit1 = (byte)(multi % 2);
            byte byte2 = (byte)(multi / 2);
            if (bit1 == 1)
            {
                preview[9] |= secondIndexMask;
            }

            if (byte2 > 0)
            {
                preview[thirdIndex] |= thirdIndexHigh ? this.GetOrMaskForHighNibble(byte2) : this.GetOrMaskForLowNibble(byte2);
            }
        }
    }

    private void SetArmorPiece(Span<byte> preview, ItemAppearance? item, int firstIndex, bool firstIndexHigh, byte secondIndexMask, int thirdIndex, bool thirdIndexHigh)
    {
        if (item?.Definition is null)
        {
            this.SetEmptyArmor(preview, firstIndex, firstIndexHigh, secondIndexMask, thirdIndex, thirdIndexHigh);
        }
        else
        {
            // item id
            this.SetArmorItemIndex(preview, item, firstIndex, firstIndexHigh, secondIndexMask, thirdIndex, thirdIndexHigh);

            // exc bit
            if (this.IsExcellent(item))
            {
                preview[10] |= secondIndexMask;
            }

            // ancient bit
            if (this.IsAncient(item))
            {
                preview[11] |= secondIndexMask;
            }
        }
    }

    private void SetItemLevels(Span<byte> preview, ItemAppearance?[] itemArray)
    {
        int levelIndex = 0;
        for (int i = 0; i < 7; i++)
        {
            if (itemArray[i] is not null)
            {
                levelIndex |= itemArray[i]!.GetGlowLevel() << (i * 3);
            }
        }

        preview[6] = (byte)((levelIndex >> 16) & 255);
        preview[7] = (byte)((levelIndex >> 8) & 255);
        preview[8] = (byte)(levelIndex & 255);
    }

    private void AddWing(Span<byte> preview, ItemAppearance? wing)
    {
        if (wing?.Definition is null)
        {
            return;
        }

        var values = this.GetWingsBytes(wing.Definition.Group, wing.Definition.Number);
        preview[5] |= values.b5;
        preview[9] |= values.b9;
        preview[16] |= values.b16;
    }

    private void AddPet(Span<byte> preview, ItemAppearance? pet)
    {
        if (pet?.Definition is null)
        {
            preview[5] |= 0b0000_0011;
            return;
        }

        switch ((PetIndex)pet.Definition.Number)
        {
            case PetIndex.Angel:
            case PetIndex.Imp:
            case PetIndex.Unicorn:
                preview[5] |= (byte)pet.Definition.Number;
                break;
            case PetIndex.Dinorant:
                preview[5] |= 0x03;
                preview[10] |= 0x01;
                break;
            case PetIndex.DarkHorse:
                preview[5] |= 0x03;
                preview[12] |= 0x01;
                break;
            case PetIndex.Fenrir:
                preview[5] |= 0x03;
                preview[10] &= 0xFE;
                preview[12] &= 0xFE;
                preview[12] |= 0x04;
                preview[16] = 0x00;

                if (pet.VisibleOptions.Contains(ItemOptionTypes.BlackFenrir))
                {
                    preview[16] |= 0x01;
                }

                if (pet.VisibleOptions.Contains(ItemOptionTypes.BlueFenrir))
                {
                    preview[16] |= 0x02;
                }

                if (pet.VisibleOptions.Contains(ItemOptionTypes.GoldFenrir))
                {
                    preview[17] |= 0x01;
                }

                break;
            default:
                preview[5] |= 0x03;
                break;
        }

        switch ((PetIndex)pet.Definition.Number)
        {
            case PetIndex.Panda:
                preview[16] |= 0xE0;
                break;
            case PetIndex.PetUnicorn:
                preview[16] |= 0xA0;
                break;
            case PetIndex.Skeleton:
                preview[16] |= 0x60;
                break;
            case PetIndex.Rudolph:
                preview[16] |= 0x80;
                break;
            case PetIndex.SpiritOfGuardian:
                preview[16] |= 0x40;
                break;
            case PetIndex.Demon:
                preview[16] |= 0x20;
                break;
            default:
                // no further flag required.
                break;
        }
    }

    private bool IsExcellent(ItemAppearance item)
    {
        return item.VisibleOptions.Contains(ItemOptionTypes.Excellent);
    }

    private bool IsAncient(ItemAppearance item)
    {
        return item.VisibleOptions.Contains(ItemOptionTypes.AncientOption);
    }

    public int GetItemID(int x, int y) => (x * 512) + y;

    private (byte b5, byte b9, byte b16) GetWingsBytes(int cat, int subId) => (cat, subId) switch
    {
        //Wings of Elf
        //Wings of Heaven
        //Wings of Satan
        (12, 0) or (12, 1) or (12, 2) => (0, 0, Convert.ToByte(4 * (GetItemID(cat, subId) - GetItemID(12, 0) + 1))),
        //Wings of Spirits
        //[Bound] Wings of Spirit
        (12, 3) or (12, 424) => (0, 0, 16),
        //Wings of Soul
        //[Bound] Wings of Soul
        (12, 4) or (12, 422) => (0, 0, 20),
        //Wings of Dragon
        //[Bound] Wings of Dragon
        (12, 5) or (12, 423) => (0, 0, 24),
        //Darkness Wings
        //[Bound] Darkness Wings
        (12, 6) or (12, 425) => (0, 0, 28),
        //Wings of Storm
        //[Bound] Wings of Storm
        (12, 36) or (12, 431) => (0, 1, 0),
        //Wings of Eternal
        //[Bound] Wings of Eternal
        (12, 37) or (12, 430) => (0, 1, 4),
        //Wings of Illusion
        //[Bound] Wings of Illusion
        (12, 38) or (12, 432) => (0, 1, 8),
        //Wings of Ruin
        //[Bound] Wings of Ruin
        (12, 39) or (12, 433) => (0, 1, 12),
        //Cape of Emperor
        //[Bound] Cape of Emperor
        (12, 40) or (12, 434) => (0, 1, 16),
        //Wings of Curse
        //[Bound] Wings of Curse
        (12, 41) or (12, 435) => (0, 1, 20),
        //Wings of Despair
        //[Bound] Wings of Despair
        (12, 42) or (12, 427) => (0, 1, 24),
        //Cape of Fighter
        //[Bound] Cape of Fighter
        (12, 49) or (12, 428) => (0, 2, 0),
        //Reigning Cloak
        //[Bound] Cape of Overrule
        (12, 50) or (12, 436) => (0, 2, 4),
        //Little Monarch's Cloak
        //Small Cursed Wings
        //Small Elven Wings
        //Small Wings of Heaven
        //Small Wings of Satan
        //Small Warrior's Cloak
        (12, 130) or (12, 131) or (12, 132) or (12, 133) or (12, 134) or (12, 135) => (0, 2, Convert.ToByte(4 * (GetItemID(cat, subId) - GetItemID(12, 130) + 2))),
        //Cloak of Death
        //Wings of Chaos
        //Wings of Magic
        //Wings of Life
        //Wings of Angel and Devil
        (12, 262) or (12, 263) or (12, 264) or (12, 265) or (12, 267) => (0, 3, Convert.ToByte(4 * (GetItemID(cat, subId) - GetItemID(12, 262)))),
        //Cape of the Lord
        //[Bound] Cape of Lord
        (13, 30) or (12, 426) => (0, 3, 24),
        //Cloak of Limit
        //[Bound] Cloak of Limit
        (12, 269) or (12, 429) => (0, 3, 28),
        //Cloak of Transcendence
        //[Bound] Cloak of Transcendence
        (12, 270) or (12, 437) => (0, 4, 0),//0,4,8 if level 15
        //Small Cloak of Limit
        (12, 278) => (0, 4, 4),
        //Wings of Conqueror
        //Wings of Conqueror
        (12, 268) or (12, 266) => (0, 4, 12),
        //[Bound] Cloak of Death
        //[PC] Cloak of Death
        (12, 279) or (12, 284) => (0, 4, 16),
        //[Bound] Wings of Chaos
        //[PC] Wings of Chaos
        (12, 280) or (12, 285) => (0, 4, 20),
        //[Bound] Wings of Magic
        //[PC] Wings of Magic
        (12, 281) or (12, 286) => (0, 4, 24),
        //[Bound] Wings of Life
        //[PC] Wings of Life
        (12, 282) or (12, 287) => (0, 4, 28),
        //Angel Wings
        (12, 414) => (0, 5, 0),
        //Devil Wings
        (12, 415) => (0, 5, 4),
        //Genius Wings
        (12, 416) => (0, 5, 8),
        //Destruction Wings
        (12, 417) => (0, 5, 12),
        //Control Wings
        (12, 418) => (0, 5, 16),
        //Eternal Wings
        (12, 419) => (0, 5, 20),
        //Judgement Cloak
        (12, 420) => (0, 5, 24),
        //Eternity Cloak
        (12, 421) => (0, 5, 28),
        //[Bound] Angel Wings
        (12, 438) => (0, 6, 0),
        //[Bound] Devil Wings
        (12, 439) => (0, 6, 4),
        //[Bound] Genius Wings
        (12, 440) => (0, 6, 8),
        //[Bound] Destruction Wings
        (12, 441) => (0, 6, 12),
        //[Bound] Control Wings
        (12, 442) => (0, 6, 16),
        //[Bound] Eternal Wings
        (12, 443) => (0, 6, 20),
        //[Bound] Judgment Cloak
        (12, 444) => (0, 6, 24),
        //[Bound] Eternity Cloak
        (12, 445) => (0, 6, 28),
        //Wings of Disillusionment
        //[Bound] Wings of Disillusionment
        (12, 467) or (12, 468) => (0, 7, 0),
        //Wings of Fate 57 - 58
        (12, 469) => (0, 7, 4),
        //Wings of Power 59
        (12, 480) => (0, 7, 12),
        //Wings of Silence 60
        //[Bound] Wings of Silence
        (12, 472) or (12, 473) => (0, 7, 16),
        //Wings of Condemnation 61 62
        (12, 474) => (0, 7, 20),
        //Wings of Hit 63
        (12, 489) => (0, 7, 28),
        //Blood Wings 64 65
        (12, 490) => (4, 0, 0),
        //Small White Cloak 66 8318
        (12, 154) => (4, 0, 8),
        //Pure White Clock 67 8319
        (12, 155) => (4, 0, 12),
        //[Bound] Cloak of Innocence 68 8320
        (12, 156) or (12, 157) => (4, 0, 16),
        //Cloak of Brilliance 69 8322
        //[Bound] Cloak of Brilliance 69 8322 checkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk
        (12, 158) or (12, 159) => (4, 0, 20),
        //Cloak of Brilliance 70 71 8324
        (12, 160) => (4, 0, 24),
        //Wings of Eternity 72 8191
        (12, 27) => (4, 1, 0),
        //Storm's Wings 73 74 8316
        (12, 152) => (4, 1, 4),
        //Small Iron Cloak 75 8336
        (12, 172) => (4, 1, 12),
        //Iron Cloack 76 8337
        (12, 173) => (4, 1, 16),
        //Black Cloak 77 8338
        //[Bound] Black Cloak 77 8338
        (12, 174) or (12, 175) => (4, 1, 20),
        //Cloak of Death 78 8340
        //[Bound] Cloak of Death 78 8340
        (12, 176) or (12, 177) => (4, 1, 24),
        //Cloak of Hatred 79 80 8342
        (12, 178) => (4, 1, 28),
        //Wings of Virtue 81 8344
        (12, 180) => (4, 2, 4),
        //Wings of Destruction 82 8345
        (12, 181) => (4, 2, 8),
        //Wings of Fantasy 83 8346
        (12, 182) => (4, 2, 12),
        //Wings of Punishment 84 8347 /////////////////////
        (12, 183) => (4, 2, 16),
        //Cloak of Youngdo 86 8349
        (12, 184) => (4, 2, 20),
        //Wings of Barrier 87 8350
        (12, 185) => (4, 2, 24),
        //Cloak of Oath 88 8351
        (12, 186) => (4, 2, 28),
        //Cloak of Discipline 89 8352
        (12, 187) => (4, 3, 0),
        //Wings of Inevitability 90 8353
        (12, 188) => (4, 3, 4),
        //Wings of Jaan 91 8354
        (12, 189) => (4, 3, 8),
        //Crimson Wings
        (12, 190) => (4, 3, 12),
        //Cloak of Movie
        (12, 191) => (4, 3, 16),
        //Wings of Eternity
        (12, 192) => (4, 3, 20),
        //Cloak of Unsullied
        (12, 193) => (4, 3, 24),
        //Wings of Guardian 95 8358
        //Wings of Guardian 95 8358
        (12, 194) or (12, 195) => (4, 3, 28),
        //Wings of Purity 96 97 8360
        (12, 196) => (4, 4, 0),
        //Wings of Wisdom 98 8362
        (12, 198) => (4, 3, 8),
    };
}
