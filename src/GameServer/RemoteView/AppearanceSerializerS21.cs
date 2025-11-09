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

        var wingNumber = this.GetWingNum(wing.Definition.Group, wing.Definition.Number);

        // var wingNumber = 99;
        int bits_bVar1 = (wingNumber >> 6) & 0x03;  // top 2 bits
        int bits_bVar2 = (wingNumber >> 3) & 0x07;  // middle 3 bits
        int bits_bVar3 = wingNumber & 0x07;         // bottom 3 bits

        preview[5] |= (byte)(bits_bVar1 << 2);
        preview[9] |= (byte)bits_bVar2;
        preview[16] |= (byte)(bits_bVar3 << 2);
        preview[18] |= wing.Level;
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

    private byte GetWingNum(int category, int index) => (category, index) switch
    {
        (12, 0) => 1, // Wings of Elf
        (12, 1) => 2, // Wings of Heaven
        (12, 2) => 3, // Wings of Satan
        (12, 3) or (12, 424) => 4, // Wings of Spirits and Bound
        (12, 4) or (12, 422) => 5, // Wings of Soul and Bound
        (12, 5) or (12, 423) => 6, // Wings of Dragon and Bound
        (12, 6) or (12, 425) => 7, // Darkness Wings and Bound
        (12, 36) or (12, 431) => 8, // Wings of Storm and Bound
        (12, 37) or (12, 430) => 9, // Wings of Eternal and Bound
        (12, 38) or (12, 432) => 10, // Wings of Illusion and Bound
        (12, 39) or (12, 433) => 11, // Wings of Ruin and Bound
        (12, 40) or (12, 434) => 12, // Cape of Emperor and Bound
        (12, 41) or (12, 435) => 13, // Wings of Curse and Bound
        (12, 42) or (12, 427) => 14, // Wings of Despair and Bound
        (12, 43) or (12, 435) => 15, // Wings of Dimension and Bound
        (12, 49) or (12, 428) => 16, // Cape of Fighter and Bound
        (12, 50) or (12, 436) => 17, // Cape of Overrule and Bound
        (12, 130) => 18, // Small Cape of Lord
        (12, 131) => 19, // Small Wing of Curse
        (12, 132) => 20, // Small Wings of Elf
        (12, 133) => 21, // Small Wings of Heaven
        (12, 134) => 22, // Small Wings of Satan
        (12, 135) => 23, // Little Warrior's Cloak
        (12, 262) => 24, // Cloak of Death
        (12, 263) => 25, // Wings of Chaos
        (12, 264) => 26, // Wings of Magic
        (12, 265) => 27, // Wings of Life

        // ?? => 28, shows Wings of Conqueror as well
        (12, 267) => 29, // Wings of Angel and Devil
        (13, 30) or (12, 426) => 30, // Cape of the Lord and Bound
        (12, 269) or (12, 429) => 31, // Cloak of Limit and Bound
        (12, 270) or (12, 437) => 32, // Cloak of Transcendence and Bound or 34 if level 15
        (12, 278) => 33, // Small Cloak of Limit
        (12, 266) or (12, 268) => 35, // Wings of Conqueror (I) and Wings of Conqueror (II)
        (12, 279) or (12, 284) => 36, // Cloak of Death Bound and PC
        (12, 280) or (12, 285) => 37, // Wings of Chaos Bound and PC
        (12, 281) or (12, 286) => 38, // Wings of Magic Bound and PC
        (12, 282) or (12, 287) => 39, // Wings of Life Bound and PC
        (12, 414) => 40, // Angel Wings
        (12, 415) => 41, // Devil Wings
        (12, 416) => 42, // Genius Wings
        (12, 417) => 43, // Destruction Wings
        (12, 418) => 44, // Control Wings
        (12, 419) => 45, // Eternal Wings
        (12, 420) => 46, // Judgement Cloak
        (12, 421) => 47, // Eternity Cloak
        (12, 438) => 48, // Angel Wings Bound
        (12, 439) => 49, // Devil Wings Bound
        (12, 440) => 50, // Genius Wings Bound
        (12, 441) => 51, // Destruction Wings Bound
        (12, 442) => 52, // Control Wings Bound
        (12, 443) => 53, // Eternal Wings Bound
        (12, 444) => 54, // Judgment Wings Bound
        (12, 445) => 55, // Eternity Wings Bound
        (12, 467) or (12, 268) => 56, // Wings of Disillusion and Bound
        (12, 469) => 57, // Wings of Fate and 58
        (12, 480) => 59, // Wings of Power
        (12, 472) or (12, 473) => 60, // Wings of Silence and Bound
        (12, 474) => 61, // Wings of Condemnation and 62
        (12, 489) or (12, 496) => 63, // Wings of Hit and Bound
        (12, 490) => 64, // Blood Wings and 65
        (12, 154) => 66, // Small White Cloak
        (12, 155) => 67, // Pure White
        (12, 156) or (12, 157) => 68, // Cloak of Innocence and Bound
        (12, 158) or (12, 159) => 69, // Cloak of Splendor and Bound
        (12, 160) => 70, // Cloak of Radiance and 71
        (12, 27) => 72, // Wings of Eternity
        (12, 152) => 73, // Storm's Wings and 74
        (12, 172) => 75, // Small Steel Cloak
        (12, 173) => 76, // Steel Cloak
        (12, 174) or (12, 175) => 77, // Pitch Black Cloak and Bound
        (12, 176) or (12, 177) => 78, // Cloak of Sacrifice and Bound
        (12, 178) => 79, // Cloak of Hatred and 80
        (12, 180) => 81, // Wings of Virtue
        (12, 181) => 82, // Wings of Destruction
        (12, 182) => 83, // Wings of Fantasy
        (12, 183) => 84, // Wings of Punishment
        (12, 184) => 85, // Wings of Youngdo
        (12, 185) => 86, // Wings of Barrier
        (12, 186) => 87, // Cloak of Oath
        (12, 187) => 88, // Cloak of Discipline
        (12, 188) => 89, // Wings of Inevitability
        (12, 189) => 90, // Wings of Jaan
        (12, 190) => 91, // Crimson Wings
        (12, 191) => 92, // Cloak of Movie
        (12, 192) => 93, // Wings of Eternity
        (12, 193) => 94, // Cloak of Unsullied
        (12, 194) or (12, 195) => 95, // Wings of Guardian and Bound
        (12, 196) => 96, // Wings of Purity and 97
        (12, 198) => 98, // Wings of Wisdom
        //?? => 99, // Wings of Wisdom
        _ => 0,
    };
}
