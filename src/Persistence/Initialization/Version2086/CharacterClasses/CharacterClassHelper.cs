// <copyright file="CharacterClassHelper.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086.CharacterClasses;

using MUnique.OpenMU.AttributeSystem;
using MUnique.OpenMU.DataModel.Configuration;

/// <summary>
/// Helper class related to <see cref="CharacterClass"/>.
/// </summary>
public static class CharacterClassHelper
{
    /// <summary>
    /// Determines the <see cref="CharacterClass" />es, depending on the given class ranks which are provided by original configuration files.
    /// </summary>
    /// <param name="gameConfiguration">The game configuration which contains the character classes.</param>
    /// <param name="classes">The flags enum of character classes.</param>
    /// <param name="ignoreMissing">If set to <c>true</c>, missing classes are ignored and throw no exception.</param>
    /// <returns>
    /// The corresponding <see cref="CharacterClass" />es of the provided class ranks.
    /// </returns>
    public static IEnumerable<CharacterClass> DetermineCharacterClasses(this GameConfiguration gameConfiguration, IEnumerable<CharacterClassNumber> classes, bool ignoreMissing = false)
    {
        var characterClasses = gameConfiguration.CharacterClasses;

        foreach (var characterClassNumber in classes)
        {
            yield return characterClasses.First(c => c.Number == (int)characterClassNumber);
        }
    }

    public static IEnumerable<CharacterClass> DetermineCharacterClasses(this GameConfiguration gameConfiguration, bool ignoreMissing = false, params int[] classes)
    {
        var characterClasses = gameConfiguration.CharacterClasses;

        for (int i = 0; i < classes.Length; i++)
        {
            if (classes[i] == 0) continue;
            int classId = i * 16;
            if (classes[i] == 5) classId += 15;
            else if (classes[i] == 4) classId += 14;
            else if (classes[i] == 3) classId += 12;
            else if (classes[i] == 2) classId += 8;

            yield return characterClasses.First(c => c.Number == classId);

        }
    }

    /// <summary>
    /// Determines the <see cref="CharacterClass"/>es, depending on the given classes which are provided by original configuration files.
    /// </summary>
    /// <param name="gameConfiguration">The game configuration which contains the character classes.</param>
    /// <param name="wizardClass">Class of the wizard class.</param>
    /// <param name="knightClass">Class of the knight class.</param>
    /// <param name="elfClass">Class of the elf class.</param>
    /// <returns>The corresponding <see cref="CharacterClass"/>es of the provided class ranks.</returns>
    public static IEnumerable<CharacterClass> DetermineCharacterClasses(this GameConfiguration gameConfiguration, bool wizardClass, bool knightClass, bool elfClass)
    {
        var characterClasses = gameConfiguration.CharacterClasses;
        if (wizardClass)
        {
            yield return characterClasses.First(c => c.Number == (int)CharacterClassNumber.DarkWizard);
        }

        if (knightClass)
        {
            yield return characterClasses.First(c => c.Number == (int)CharacterClassNumber.DarkKnight);
        }

        if (elfClass)
        {
            yield return characterClasses.First(c => c.Number == (int)CharacterClassNumber.FairyElf);
        }
    }

    /// <summary>
    /// Creates the attribute relationship.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    /// <param name="targetAttribute">The target attribute.</param>
    /// <param name="multiplier">The multiplier.</param>
    /// <param name="sourceAttribute">The source attribute.</param>
    /// <param name="inputOperator">The input operator.</param>
    /// <param name="aggregateType">The aggregate type with which the relationship will effect the <paramref name="targetAttribute"/>.</param>
    /// <returns>The attribute relationship.</returns>
    public static AttributeRelationship CreateAttributeRelationship(IContext context, GameConfiguration gameConfiguration, AttributeDefinition targetAttribute, float multiplier, AttributeDefinition sourceAttribute, InputOperator inputOperator = InputOperator.Multiply, AggregateType aggregateType = AggregateType.AddRaw)
    {
        return context.CreateNew<AttributeRelationship>(
            targetAttribute.GetPersistent(gameConfiguration) ?? targetAttribute,
            multiplier,
            sourceAttribute.GetPersistent(gameConfiguration) ?? sourceAttribute,
            inputOperator,
            default(AttributeDefinition?),
            aggregateType);
    }

    /// <summary>
    /// Creates the attribute relationship.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    /// <param name="targetAttribute">The target attribute.</param>
    /// <param name="multiplierAttribute">The multiplier attribute.</param>
    /// <param name="sourceAttribute">The source attribute.</param>
    /// <param name="inputOperator">The input operator.</param>
    /// <param name="aggregateType">The aggregate type with which the relationship will effect the <paramref name="targetAttribute"/>.</param>
    /// <returns>The attribute relationship.</returns>
    public static AttributeRelationship CreateAttributeRelationship(IContext context, GameConfiguration gameConfiguration, AttributeDefinition targetAttribute, AttributeDefinition multiplierAttribute, AttributeDefinition sourceAttribute, InputOperator inputOperator = InputOperator.Multiply, AggregateType aggregateType = AggregateType.AddRaw)
    {
        return context.CreateNew<AttributeRelationship>(
            targetAttribute.GetPersistent(gameConfiguration) ?? targetAttribute,
            0f,
            sourceAttribute.GetPersistent(gameConfiguration) ?? sourceAttribute,
            inputOperator,
            multiplierAttribute.GetPersistent(gameConfiguration) ?? multiplierAttribute,
            aggregateType);
    }

    /// <summary>
    /// Creates the conditional relationship between attributes.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    /// <param name="targetAttribute">The target attribute.</param>
    /// <param name="conditionalAttribute">The conditional attribute.</param>
    /// <param name="sourceAttribute">The source attribute.</param>
    /// <param name="aggregateType">The aggregate type with which the relationship will effect the <paramref name="targetAttribute"/>.</param>
    /// <returns>The attribute relationship.</returns>
    public static AttributeRelationship CreateConditionalRelationship(IContext context, GameConfiguration gameConfiguration, AttributeDefinition targetAttribute, AttributeDefinition conditionalAttribute, AttributeDefinition sourceAttribute, AggregateType aggregateType = AggregateType.AddRaw)
    {
        return context.CreateNew<AttributeRelationship>(
            targetAttribute.GetPersistent(gameConfiguration) ?? targetAttribute,
            conditionalAttribute.GetPersistent(gameConfiguration) ?? conditionalAttribute,
            sourceAttribute.GetPersistent(gameConfiguration) ?? sourceAttribute,
            aggregateType);
    }

    /// <summary>
    /// Creates the constant value attribute.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    /// <param name="value">The value.</param>
    /// <param name="attribute">The attribute.</param>
    /// <returns>The constant value attribute.</returns>
    public static ConstValueAttribute CreateConstValueAttribute(IContext context, GameConfiguration gameConfiguration, float value, AttributeDefinition attribute)
    {
        return context.CreateNew<ConstValueAttribute>(value, attribute.GetPersistent(gameConfiguration));
    }
}