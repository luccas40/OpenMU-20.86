// <copyright file="Debenter2.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086.Maps;

using MUnique.OpenMU.DataModel.Configuration;

/// <summary>
/// Map initialization for the Doppelgaenger 1 (a.k.a. "Double Gear", "Double Goer", etc.) event map.
/// </summary>
internal class Debenter2 : BaseMapInitializer
{
    /// <summary>
    /// The Number of the Map.
    /// </summary>
    internal const byte Number = 96;

    /// <summary>
    /// The Name of the Map.
    /// </summary>
    internal const string Name = "Debenter 2";

    /// <summary>
    /// Initializes a new instance of the <see cref="Debenter2"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    public Debenter2(IContext context, GameConfiguration gameConfiguration)
        : base(context, gameConfiguration)
    {
    }

    /// <inheritdoc />
    protected override byte MapNumber => Number;

    /// <inheritdoc />
    protected override string MapName => Name;

    /// <inheritdoc/>
    protected override string TerrainVersionPrefix => "S21_";
}