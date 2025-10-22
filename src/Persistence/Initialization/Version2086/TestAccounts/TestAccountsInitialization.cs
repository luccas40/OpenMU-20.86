// <copyright file="TestAccountsInitialization.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086.TestAccounts;

using MUnique.OpenMU.DataModel.Configuration;

/// <summary>
/// Initializes the test accounts.
/// </summary>
public class TestAccountsInitialization : InitializerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestAccountsInitialization"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    public TestAccountsInitialization(IContext context, GameConfiguration gameConfiguration)
        : base(context, gameConfiguration)
    {
    }

    /// <inheritdoc/>
    public override void Initialize()
    {
        new TestAccount(this.Context, this.GameConfiguration, "test").Initialize();
        new TestAccount(this.Context, this.GameConfiguration, "asd").Initialize();
    }
}
