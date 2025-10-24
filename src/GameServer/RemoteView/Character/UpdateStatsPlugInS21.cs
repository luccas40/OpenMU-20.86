// <copyright file="UpdateStatsPlugInS21.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Character;

using MUnique.OpenMU.AttributeSystem;
using MUnique.OpenMU.GameLogic.Attributes;
using MUnique.OpenMU.GameLogic.Views.Character;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Packets.ServerToClient;
using MUnique.OpenMU.Network.PlugIns;
using MUnique.OpenMU.PlugIns;
using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Threading;

/// <summary>
/// The default implementation of the <see cref="IUpdateStatsPlugIn"/> which is forwarding everything to the game client with specific data packets.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="UpdateStatsPlugInS21"/> class.
/// </remarks>
/// <param name="player">The player.</param>
[PlugIn(nameof(UpdateStatsPlugInS21), "The default implementation of the IUpdateStatsPlugIn which is forwarding everything to the game client with specific data packets.")]
[Guid("5626A661-2F03-4159-BB21-959C661EB69F")]
[MinimumClient(21, 0, ClientLanguage.Korean)]
public class UpdateStatsPlugInS21(RemotePlayer player) : UpdateStatsBasePlugIn(player, AttributeChangeActions)
{
    private static readonly FrozenDictionary<AttributeDefinition, Func<RemotePlayer, ValueTask>> AttributeChangeActions = new Dictionary<AttributeDefinition, Func<RemotePlayer, ValueTask>>
    {
        { Stats.CurrentHealth, OnCurrentHealthOrShieldChangedAsync },
        { Stats.CurrentShield, OnCurrentHealthOrShieldChangedAsync },
        { Stats.MaximumHealth, OnMaximumHealthOrShieldChangedAsync },
        { Stats.MaximumShield, OnMaximumHealthOrShieldChangedAsync },

        { Stats.CurrentMana, OnCurrentManaOrAbilityChangedAsync },
        { Stats.CurrentAbility, OnCurrentManaOrAbilityChangedAsync },
        { Stats.MaximumMana, OnMaximumManaOrAbilityChangedAsync },
        { Stats.MaximumAbility, OnMaximumManaOrAbilityChangedAsync },

        { Stats.AttackSpeedAny, OnAttackSpeedAnyChangedAsync },

        { Stats.MinimumPhysicalDmg, OnPhysicalDamageChangedAsync },
        { Stats.MaximumPhysicalDmg, OnPhysicalDamageChangedAsync },
        { Stats.CombatPower, OnPhysicalDamageChangedAsync },

        { Stats.MinimumSpecialAttackPower, OnSpecializationChanged },
        { Stats.MaximumSpecialAttackPower, OnSpecializationChanged },
        { Stats.SpecialDefense, OnSpecializationChanged },

        { Stats.CriticalDamageBonus, OnStatsChangedAsync },
        { Stats.ExcellentDamageBonus, OnStatsChangedAsync },
        { Stats.ShieldBypassChance, OnStatsChangedAsync },
        { Stats.DefenseIgnoreChance, OnStatsChangedAsync },
        { Stats.HealthRecoveryMultiplier, OnStatsChangedAsync },
        { Stats.ManaRecoveryMultiplier, OnStatsChangedAsync },
        { Stats.ShieldRecoveryAbsolute, OnStatsChangedAsync },
        { Stats.ShieldRecoveryMultiplier, OnStatsChangedAsync },
        { Stats.DamageReflection, OnStatsChangedAsync },
        { Stats.AttackDamageIncrease, OnStatsChangedAsync },
        { Stats.ManaUsageReduction, OnStatsChangedAsync },
        { Stats.CriticalDamageChance, OnStatsChangedAsync },
        { Stats.ExcellentDamageChance, OnStatsChangedAsync },
        { Stats.DoubleDamageChance, OnStatsChangedAsync },
        { Stats.DamageReceiveDecrement, OnStatsChangedAsync },
        { Stats.AbilityUsageReduction, OnStatsChangedAsync },
    }.ToFrozenDictionary();

    private static async ValueTask OnSpecializationChanged(RemotePlayer player)
    {
        // I'm doing this because I'm not sure but I think we need to send all character specialization every time any of them updates
        List<(IAttribute, byte)> attributes = [];
        var minSplAtk = player.Attributes!.FirstOrDefault(a => a.Definition == Stats.MinimumSpecialAttackPower);
        if (minSplAtk != null && minSplAtk.Value > 0)
        {
            attributes.Add((minSplAtk, 0));
        }

        var maxSplAtk = player.Attributes!.FirstOrDefault(a => a.Definition == Stats.MaximumSpecialAttackPower);
        if (maxSplAtk != null && maxSplAtk.Value > 0)
        {
            attributes.Add((maxSplAtk, 1));
        }

        var splDefense = player.Attributes!.FirstOrDefault(a => a.Definition == Stats.SpecialDefense);
        if (splDefense != null && splDefense.Value > 0)
        {
            attributes.Add((splDefense, 4));
        }

        int Write()
        {
            var length = CharacterSpecializationRef.Length;
            var packet = new CharacterSpecializationRef(player.Connection!.Output.GetSpan(length)[..length]);
            for (int i = 0; i < 5; i++)
            {
                if (attributes.Count <= i + 1)
                {
                    break;
                }

                var splBlock = packet[i];
                splBlock.Type = attributes[i].Item2;
                splBlock.Value1 = (ushort)attributes[i].Item1.Value;
            }

            return packet.Header.Length;
        }

        await player.Connection!.SendAsync(Write).ConfigureAwait(false);
    }

    private static async ValueTask OnMaximumHealthOrShieldChangedAsync(RemotePlayer player)
    {
        await player.Connection.SendMaximumHealthAndShieldS21Async(
            (uint)Math.Max(player.Attributes![Stats.MaximumHealth], 0f),
            (uint)Math.Max(player.Attributes[Stats.MaximumShield], 0f)).ConfigureAwait(false);
    }

    private static async ValueTask OnMaximumManaOrAbilityChangedAsync(RemotePlayer player)
    {
        await player.Connection.SendMaximumManaAndAbilityAsync(
            (ushort)Math.Max(player.Attributes![Stats.MaximumMana], 0f),
            (ushort)Math.Max(player.Attributes[Stats.MaximumAbility], 0f)).ConfigureAwait(false);
    }

    private static async ValueTask OnCurrentHealthOrShieldChangedAsync(RemotePlayer player)
    {
        await player.Connection.SendCurrentHealthAndShieldS21Async(
            (ushort)Math.Max(player.Attributes![Stats.CurrentHealth], 0f),
            (ushort)Math.Max(player.Attributes[Stats.CurrentShield], 0f)).ConfigureAwait(false);
    }

    private static async ValueTask OnCurrentManaOrAbilityChangedAsync(RemotePlayer player)
    {
        await player.Connection.SendCurrentManaAndAbilityAsync(
            (ushort)Math.Max(player.Attributes![Stats.CurrentMana], 0f),
            (ushort)Math.Max(player.Attributes[Stats.CurrentAbility], 0f)).ConfigureAwait(false);
    }

    private static async ValueTask OnAttackSpeedAnyChangedAsync(RemotePlayer player)
    {
        await player.Connection.SendUpdateAttackSpeedS21Async(
            (uint)Math.Max(player.Attributes![Stats.AttackSpeed], 0f),
            (uint)Math.Max(player.Attributes![Stats.MagicSpeed], 0f)).ConfigureAwait(false);
    }

    private static async ValueTask OnPhysicalDamageChangedAsync(RemotePlayer player)
    {
        await player.Connection.SendUpdateWeaponCombatPowerAsync(
            (ushort)Math.Max(player.Attributes![Stats.MinimumPhysicalDmg], 0f),
            (ushort)Math.Max(player.Attributes![Stats.MaximumPhysicalDmg], 0f),
            (ushort)Math.Max(player.Attributes![Stats.CombatPowerDamage], 0f),
            (ushort)Math.Max(player.Attributes![Stats.CombatPower], 0f)).ConfigureAwait(false);
    }

    private static async ValueTask OnStatsChangedAsync(RemotePlayer player)
    {
        await player.Connection.SendUpdateCharacterSheetS21Async(
            (ushort)player.Attributes![Stats.CriticalDamageBonus],
            (ushort)player.Attributes[Stats.ExcellentDamageBonus],
            0,
            0,
            (ushort)player.Attributes[Stats.BaseStrength],
            (ushort)(player.Attributes[Stats.TotalStrength] - player.Attributes[Stats.BaseStrength]),
            (ushort)player.Attributes[Stats.BaseAgility],
            (ushort)(player.Attributes[Stats.TotalAgility] - player.Attributes[Stats.BaseAgility]),
            (ushort)player.Attributes[Stats.BaseVitality],
            (ushort)(player.Attributes[Stats.TotalVitality] - player.Attributes[Stats.BaseVitality]),
            (ushort)player.Attributes[Stats.BaseEnergy],
            (ushort)(player.Attributes[Stats.TotalEnergy] - player.Attributes[Stats.BaseEnergy]),
            (ushort)player.Attributes[Stats.BaseLeadership],
            (ushort)(player.Attributes[Stats.TotalLeadership] - player.Attributes[Stats.BaseLeadership]),
            0,
            (ushort)player.Attributes[Stats.ShieldBypassChance],
            0,
            (ushort)player.Attributes[Stats.MoneyAmountRate],
            player.Attributes[Stats.DefenseIgnoreChance],
            player.Attributes[Stats.HealthRecoveryMultiplier],
            player.Attributes[Stats.ManaRecoveryMultiplier],
            0,
            0,
            0,
            player.Attributes[Stats.ShieldRecoveryAbsolute],
            0,
            0,
            0,
            player.Attributes[Stats.ShieldRecoveryMultiplier],
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            player.Attributes[Stats.DamageReflection] * 100,
            (player.Attributes[Stats.AttackDamageIncrease] * 100) - 100,
            player.Attributes[Stats.ManaUsageReduction],
            player.Attributes[Stats.CriticalDamageChance] * 100,
            player.Attributes[Stats.ExcellentDamageChance] * 100,
            player.Attributes[Stats.DoubleDamageChance] * 100,
            (byte)(int)(player.Attributes[Stats.DamageReceiveDecrement] * 100),
            0,
            (byte)(int)player.Attributes[Stats.AbilityUsageReduction],
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
            ).ConfigureAwait(false);

        int Write()
        {
            var length = CharacterSpecializationRef.Length;
            var packet = new CharacterSpecializationRef(player.Connection!.Output.GetSpan(length)[..length]);
            
            var splBlock = packet[0];

            splBlock = packet[1];
            splBlock.Type = 1;
            splBlock.Value1 = 3;
            splBlock.Value2 = 4;

            splBlock = packet[2];
            splBlock.Type = 4;
            splBlock.Value1 = 5;

            return packet.Header.Length;
        }

        await player.Connection!.SendAsync(Write).ConfigureAwait(false);
    }
}