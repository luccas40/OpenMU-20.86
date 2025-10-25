// <copyright file="IItemMovedPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameLogic.Views.Inventory;

/// <summary>
/// Interface of a view whose implementation informs about an item which has moved in the inventory.
/// </summary>
public interface IItemUsedPlugIn : IViewPlugIn
{
    /// <summary>
    /// An item got moved.
    /// </summary>
    /// <param name="slot">The slot.</param>
    /// <param name="useType">The Use Type.</param>
    ValueTask ItemUsedAsync(byte slot, bool active);
}