// <copyright file="UpdateCharacterStatsPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Character;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MUnique.OpenMU.GameLogic;
using MUnique.OpenMU.GameLogic.Attributes;
using MUnique.OpenMU.GameLogic.Views.Character;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The S21 implementation of the <see cref="IUpdateCharacterStatsPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="UpdateCharacterStatsPlugInS21"/> class.
/// </remarks>
/// <param name="player">The player.</param>
[PlugIn("UpdateCharacterStatsPlugInS21", "The S21 implementation of the IUpdateCharacterStatsPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("ae0e4067-7eb6-4433-aec1-b1491b2e2067")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
public class UpdateCharacterStatsPlugInS21(RemotePlayer player) : IUpdateCharacterStatsPlugIn
{
    private readonly RemotePlayer _player = player;

    /// <inheritdoc/>
    public async ValueTask UpdateCharacterStatsAsync()
    {
        var connection = this._player.Connection;
        if (connection is null || this._player.Account is null)
        {
            return;
        }

        int Write()
        {
            var size = CharacterInformationS21.Length;
            var span = connection.Output.GetSpan(size)[..size];
            var packet = new CharacterInformationS21Ref(span)
            {
                X = this._player.Position.X,
                Y = this._player.Position.Y,
                MapId = unchecked((ushort)this._player.SelectedCharacter!.CurrentMap!.Number),
                Direction = 0,
                CurrentExperience = (ulong)this._player.SelectedCharacter.Experience,
                ExperienceForNextLevel = (ulong)this._player.GameServerContext.ExperienceTable[(int)this._player.Attributes![Stats.Level] + 1],
                LevelUpPoints = (ushort)Math.Max(0, this._player.SelectedCharacter.LevelUpPoints),
                Strength = (ushort)this._player.Attributes[Stats.BaseStrength],
                Agility = (ushort)this._player.Attributes[Stats.BaseAgility],
                Vitality = (ushort)this._player.Attributes[Stats.BaseVitality],
                Energy = (ushort)this._player.Attributes[Stats.BaseEnergy],
                CurrentHealth = (ushort)this._player.Attributes[Stats.CurrentHealth],
                MaximumHealth = (ushort)this._player.Attributes[Stats.MaximumHealth],
                CurrentMana = (ushort)this._player.Attributes[Stats.CurrentMana],
                MaximumMana = (ushort)this._player.Attributes[Stats.MaximumMana],
                CurrentShield = (ushort)this._player.Attributes[Stats.CurrentShield],
                MaximumShield = (ushort)this._player.Attributes[Stats.MaximumShield],
                CurrentAbility = (ushort)this._player.Attributes[Stats.CurrentAbility],
                MaximumAbility = (ushort)this._player.Attributes[Stats.MaximumAbility],
                Money = (uint)this._player.Money,
                HeroState = this._player.SelectedCharacter.State.Convert(),
                Status = this._player.SelectedCharacter.CharacterStatus.Convert(),
                UsedFruitPoints = (ushort)this._player.SelectedCharacter.UsedFruitPoints,
                MaxFruitPoints = this._player.SelectedCharacter.GetMaximumFruitPoints(),
                Leadership = (ushort)this._player.Attributes[Stats.BaseLeadership],
                UsedNegativeFruitPoints = (ushort)this._player.SelectedCharacter.UsedNegFruitPoints,
                MaxNegativeFruitPoints = this._player.SelectedCharacter.GetMaximumFruitPoints(),
                InventoryExtensions = (byte)this._player.SelectedCharacter.InventoryExtensions,
                Ruud = 100,
            };
            return size;
        }

        await connection.SendAsync(Write).ConfigureAwait(false);

        if (this._player.SelectedCharacter!.CharacterClass!.IsMasterClass)
        {
            await this._player.InvokeViewPlugInAsync<IUpdateMasterStatsPlugIn>(p => p.SendMasterStatsAsync()).ConfigureAwait(false);
        }
    }
}
