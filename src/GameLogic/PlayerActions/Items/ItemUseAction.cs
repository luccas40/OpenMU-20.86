// <copyright file="ItemUseAction.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameLogic.PlayerActions.Items;

using System.Threading.Tasks;
using MUnique.OpenMU.GameLogic.Views.Inventory;

/// <summary>
/// Action to Activate items in inventory ex: Guardian Mounts, Raven, Horse, Exp Talisman etc.
/// </summary>
public class ItemUseAction
{
    /// <summary>
    /// Stacks several items to one stacked item.
    /// </summary>
    /// <param name="player">The player which is activating.</param>
    /// <param name="slot">The inventory slot number.</param>
    /// <param name="active">Item active status will be set to the arg.</param>
    public async ValueTask UseItemAsync(Player player, byte slot, bool active)
    {
        var result = await player.Inventory!.SetItemActiveAsync(slot, active).ConfigureAwait(false);

        await player.InvokeViewPlugInAsync<IItemUsedPlugIn>(i => i.ItemUsedAsync(slot, result ? active : false)).ConfigureAwait(false);
    }
}
