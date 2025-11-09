// <copyright file="IMinimapDataUpdatePlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameLogic.Views.Minimap;

using System.Runtime.InteropServices;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// A plugin interface which is called when the minimap is oppened and needs to be updated.
/// </summary>
public interface IMinimapDataUpdatePlugIn : IViewPlugIn
{
    /// <summary>
    /// Is called when a the minimap is oppened and needs to be updated.
    /// </summary>
    ValueTask UpdateMinimapAsync();
}