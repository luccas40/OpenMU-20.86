// <copyright file="KalimaBase.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086.Maps;

using MUnique.OpenMU.DataModel.Configuration;

/// <summary>
/// The base class for the initialization of the Kalima maps.
/// </summary>
internal abstract class KalimaBase : BaseMapInitializer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KalimaBase"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    protected KalimaBase(IContext context, GameConfiguration gameConfiguration)
        : base(context, gameConfiguration)
    {
    }

    /// <inheritdoc/>
    protected override string TerrainVersionPrefix => "S21_";

    /// <inheritdoc/>
    protected override IEnumerable<MonsterSpawnArea> CreateNpcSpawns()
    {
        yield return this.CreateMonsterSpawn(1, this.NpcDictionary[259], 007, 019, Direction.South); // Oracle Layla
    }
}