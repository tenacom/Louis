// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

// The following code has been adapted from the Roslyn project.
// https://github.com/dotnet/roslyn/blob/v4.2.0/src/Compilers/Core/Portable/InternalUtilities/UnicodeCharacterUtilities.cs
namespace Louis.Text;

/// <summary>
/// Provides utility methods to classify Unicode characters.
/// </summary>
public static class UnicodeCharacterUtility
{
    /// <summary>
    /// Checks whether a Unicode character can be the first character
    /// of a C# or Visual Basic identifier.
    /// </summary>
    /// <param name="ch">The character to check.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="ch"/> can be the first character of an identifier;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsIdentifierStartCharacter(char ch)
        => ch switch {
            < 'A' => false,
            <= 'Z' => true,
            '_' => true,
            < 'a' => false,
            <= 'z' => true,
            <= '\u007F' => false,
            _ => CharUnicodeInfo.GetUnicodeCategory(ch)
                is UnicodeCategory.UppercaseLetter
                or UnicodeCategory.LowercaseLetter
                or UnicodeCategory.TitlecaseLetter
                or UnicodeCategory.ModifierLetter
                or UnicodeCategory.OtherLetter,
        };

    /// <summary>
    /// Checks whether a Unicode character can be a part
    /// of a C# or Visual Basic identifier.
    /// </summary>
    /// <param name="ch">The character to check.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="ch"/> can be a part of an identifier;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsIdentifierPartCharacter(char ch)
        => ch switch {
            < '0' => false,
            <= '9' => true,
            < 'A' => false,
            <= 'Z' => true,
            '_' => true,
            < 'a' => false,
            <= 'z' => true,
            <= '\u007F' => false,
            _ => CharUnicodeInfo.GetUnicodeCategory(ch)
                is UnicodeCategory.UppercaseLetter
                or UnicodeCategory.LowercaseLetter
                or UnicodeCategory.TitlecaseLetter
                or UnicodeCategory.ModifierLetter
                or UnicodeCategory.OtherLetter
                or UnicodeCategory.LetterNumber
                or UnicodeCategory.DecimalDigitNumber
                or UnicodeCategory.ConnectorPunctuation
                or UnicodeCategory.NonSpacingMark
                or UnicodeCategory.SpacingCombiningMark
                or UnicodeCategory.Format,
        };

    /// <summary>
    /// Checks whether a Unicode string is a valid C# or Visual Basic identifier.
    /// </summary>
    /// <param name="name">The string to check.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="name"/> is a valid identifier;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsValidIdentifier([NotNullWhen(true)] string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        if (!IsIdentifierStartCharacter(name![0]))
        {
            return false;
        }

        var length = name.Length;
        for (var i = 1; i < length; i++)
        {
            if (!IsIdentifierPartCharacter(name[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Checks whether a span of Unicode characters is a valid C# or Visual Basic identifier.
    /// </summary>
    /// <param name="name">The span to check.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="name"/> is a valid identifier;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsValidIdentifier(ReadOnlySpan<char> name)
    {
        if (name.Length == 0)
        {
            return false;
        }

        if (!IsIdentifierStartCharacter(name[0]))
        {
            return false;
        }

        var length = name.Length;
        for (var i = 1; i < length; i++)
        {
            if (!IsIdentifierPartCharacter(name[i]))
            {
                return false;
            }
        }

        return true;
    }
}
