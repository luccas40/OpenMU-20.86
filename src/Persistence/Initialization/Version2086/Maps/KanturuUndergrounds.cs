// <copyright file="KanturuUndergrounds.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086.Maps;

using MUnique.OpenMU.DataModel.Configuration;

/// <summary>
/// Map initialization for the Doppelgaenger 1 (a.k.a. "Double Gear", "Double Goer", etc.) event map.
/// </summary>
internal class KanturuUndergrounds : BaseMapInitializer
{
    /// <summary>
    /// The Number of the Map.
    /// </summary>
    internal const byte Number = 137;

    /// <summary>
    /// The Name of the Map.
    /// </summary>
    internal const string Name = "Kanturu Undergrounds";

    /// <summary>
    /// Initializes a new instance of the <see cref="KanturuUndergrounds"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    public KanturuUndergrounds(IContext context, GameConfiguration gameConfiguration)
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