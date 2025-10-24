// <copyright file="ItemUseAction.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameLogic.PlayerActions.Items;

using System.Threading.Tasks;
using MUnique.OpenMU.GameLogic.Views.Inventory;

public class ItemUseAction
{

    public async ValueTask UseItemAsync(Player player, byte slot, byte useType)
    {
        if (useType != 0xFF && useType != 0xFE)
        {
            await player.InvokeViewPlugInAsync<IItemUsedPlugIn>(i => i.ItemUsedAsync(slot, 0xFF)).ConfigureAwait(false);
            return;
        }

        var result = await player.Inventory!.SetItemActiveAsync(slot, useType == 0xFE).ConfigureAwait(false);

        await player.InvokeViewPlugInAsync<IItemUsedPlugIn>(i => i.ItemUsedAsync(slot, (byte)(result ? useType : 0xFF))).ConfigureAwait(false);
    }
}
