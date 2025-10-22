namespace MUnique.OpenMU.Persistence.Initialization.Version2086;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MUnique.OpenMU.DataModel.Configuration;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.Persistence.Initialization.Version2086.TestAccounts;
using MUnique.OpenMU.Persistence.Initialization.VersionSeasonSix;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// Provides initial data for Season 20 Episode 0.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DataInitialization" /> class.
/// </remarks>
/// <param name="persistenceContextProvider">The persistence context provider.</param>
/// <param name="loggerFactory">The logger factory.</param>
[Guid("865d4800-dfed-429f-8677-1abe95a76627")]
[PlugIn("Season 21 Episode 0 Initialization", "Provides initial data for Season 21 Episode 0.")]
public class DataInitialization(IPersistenceContextProvider persistenceContextProvider, ILoggerFactory loggerFactory) : DataInitializationBase(persistenceContextProvider, loggerFactory)
{
    /// <summary>
    /// Gets the identifier, by which the initialization is selected.
    /// </summary>
    public static string Id => "season21";

    /// <inheritdoc />
    public override string Key => Id;

    public override string Caption => "1.20.86 - Season 21";

    /// <inheritdoc/>
    protected override IInitializer TestAccountsInitializer => new TestAccountsInitialization(this.Context, this.GameConfiguration);

    /// <inheritdoc/>
    protected override IInitializer GameConfigurationInitializer => new GameConfigurationInitializer(this.Context, this.GameConfiguration);

    /// <inheritdoc/>
    protected override IGameMapsInitializer GameMapsInitializer => new GameMapsInitializer(this.Context, this.GameConfiguration);

    /// <inheritdoc />
    protected override void CreateGameClientDefinition()
    {
        var season21KmoClient = this.Context.CreateNew<GameClientDefinition>();
        season21KmoClient.SetGuid(0x2086);
        season21KmoClient.Season = 21;
        season21KmoClient.Episode = 0;
        season21KmoClient.Language = ClientLanguage.Korean;
        season21KmoClient.Version = "12086"u8.ToArray();
        season21KmoClient.Serial = "k1Pk2jcET48mxL3b"u8.ToArray();
        season21KmoClient.Description = "Season 21 Episode 0 KMO Client";
    }
}
