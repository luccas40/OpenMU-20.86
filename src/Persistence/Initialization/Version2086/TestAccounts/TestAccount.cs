// <copyright file="TestAccount.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086.TestAccounts;

using MUnique.OpenMU.DataModel;
using MUnique.OpenMU.DataModel.Configuration;
using MUnique.OpenMU.DataModel.Entities;
using MUnique.OpenMU.GameLogic.Attributes;
using MUnique.OpenMU.Persistence.Initialization.Version2086.CharacterClasses;

/// <summary>
/// Initializer for an account with level 300 characters.
/// </summary>
internal class TestAccount : AccountInitializerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestAccount"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    public TestAccount(IContext context, GameConfiguration gameConfiguration, string name)
        : base(context, gameConfiguration, name, 400, 400, 800)
    {
    }

    /// <inheritdoc/>
    protected override Character CreateDarkLord()
    {
        var character = this.CreateDarkLord(CharacterClassNumber.ForceEmpire, 0);

        character.Attributes.First(a => a.Definition == Stats.BaseStrength).Value = 841;
        character.Attributes.First(a => a.Definition == Stats.BaseAgility).Value = 1010;
        character.Attributes.First(a => a.Definition == Stats.BaseEnergy).Value = 401;
        character.Attributes.First(a => a.Definition == Stats.BaseLeadership).Value = 1101;
        character.LevelUpPoints = 1500; // for the added strength and agility

        //character.Inventory!.Items.Add(this.CreateWeapon(InventoryConstants.LeftHandSlot, 2, 12, 13, 4, true, true, Stats.ExcellentDamageChance)); // Exc Great Lord Scepter+13+16+L+ExcDmg
        //character.Inventory.Items.Add(this.CreateArmorItem(InventoryConstants.HelmSlot, 26, 7, Stats.MaximumHealth, 13, 4, true)); // Exc Ada Helm+13+16+L
        //character.Inventory.Items.Add(this.CreateArmorItem(InventoryConstants.ArmorSlot, 26, 8, Stats.ArmorDamageDecrease, 13, 4, true)); // Exc Ada Armor+13+16+L
        //character.Inventory.Items.Add(this.CreateArmorItem(InventoryConstants.PantsSlot, 26, 9, Stats.MoneyAmountRate, 13, 4, true)); // Exc Ada Pants+13+16+L
        //character.Inventory.Items.Add(this.CreateArmorItem(InventoryConstants.GlovesSlot, 26, 10, Stats.MaximumMana, 13, 4, true)); // Exc Ada Gloves+13+16+L
        //character.Inventory.Items.Add(this.CreateArmorItem(InventoryConstants.BootsSlot, 26, 11, Stats.DamageReflection, 13, 4, true)); // Exc Ada Boots+13+16+L
        //character.Inventory.Items.Add(this.CreateWings(InventoryConstants.WingsSlot, 30, 13, 13)); // Cape +13

        this.AddDarkLordItems(character.Inventory!);
        this.AddTestJewelsAndPotions(character.Inventory!);

        return character;
    }

}
