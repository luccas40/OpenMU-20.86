// <copyright file="PetInfoViewPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Pet;

using System.Runtime.InteropServices;
using MUnique.OpenMU.DataModel;
using MUnique.OpenMU.DataModel.Entities;
using MUnique.OpenMU.GameLogic.PlayerActions.Items;
using MUnique.OpenMU.GameLogic.Views.Pet;
using MUnique.OpenMU.GameServer.RemoteView;
using MUnique.OpenMU.Network.Packets.ClientToServer;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The default implementation of the <see cref="IPetInfoViewPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
[PlugIn(nameof(PetInfoViewPlugInS21), "The S21 implementation of the IPetInfoViewPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("2FD02467-108D-4023-A9B7-3793D03497F4")]
[MinimumClient(21, 0, Network.PlugIns.ClientLanguage.Korean)]
public class PetInfoViewPlugInS21 : IPetInfoViewPlugIn
{
    private const byte DarkHorseNumber = 4;
    private const byte DarkRavenNumber = 5;

    private readonly RemotePlayer _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="PetInfoViewPlugInS21"/> class.
    /// </summary>
    /// <param name="player">The player.</param>
    public PetInfoViewPlugInS21(RemotePlayer player) => this._player = player;

    /// <inheritdoc />
    public async ValueTask ShowPetInfoAsync(Item petItem, byte slot, PetStorageLocation petStorageLocation)
    {
        await this._player.Connection.SendPetInfoResponseAsync(
                DeterminePetType(petItem),
                ConvertStorageLocation(petStorageLocation),
                slot,
                petItem.Level,
                (uint)petItem.PetExperience,
                petItem.Durability())
            .ConfigureAwait(false);
    }

    private static PetType DeterminePetType(Item item)
    {
        var itemDefinition = item.Definition;
        if (!item.IsDlPet())
        {
            throw new ArgumentException($"Item {item} is not a known pet");
        }

        return itemDefinition!.Number is (4 or 247) ? PetType.DarkHorse : PetType.DarkRaven;
    }

    private static StorageType ConvertStorageLocation(PetStorageLocation location)
    {
        return location switch
        {
            PetStorageLocation.Crafting => StorageType.Crafting,
            PetStorageLocation.Inventory => StorageType.Inventory,
            PetStorageLocation.InventoryPetSlot => StorageType.InventoryPetSlot,
            PetStorageLocation.PersonalShop => StorageType.PersonalShop,
            PetStorageLocation.TradeOther => StorageType.TradeOther,
            PetStorageLocation.TradeOwn => StorageType.TradeOwn,
            PetStorageLocation.Vault => StorageType.Vault,
            _ => throw new ArgumentOutOfRangeException(nameof(location)),
        };
    }
}