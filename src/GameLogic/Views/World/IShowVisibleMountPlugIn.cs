// <copyright file="IShowMountPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameLogic.Views.World;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Interface of a view whose implementation informs about a mount change.
/// </summary>
public interface IShowVisibleMountPlugIn : IViewPlugIn
{
    /// <summary>
    /// Will be called when the player mounted up.
    /// </summary>
    ValueTask ShowVisibleMountAsync(ushort Mount);
}
