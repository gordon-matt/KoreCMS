using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    public class FilterLexer
    {
        private static readonly string[] comparisonOperators =
        {
            "eq",
            "neq",
            "lt",
            "lte",
            "gt",
            "gte"
        };

        private static readonly string[] logicalOperators =
        {
            "and",
            "or",
            "not"
        };

        private static readonly string[] booleans =
        {
            "true",
            "false"
        };

        private static readonly string[] functions =
        {
            "contains",
            "endswith",
            "startswith",
            "notsubstringof",
            "doesnotcontain"
        };

        private readonly string input;
        private int currentCharacterIndex;

        static FilterLexer()
        {
        }

        public FilterLexer(string input)
        {
            input = input ?? string.Empty;
            this.input = input.Trim(new[]
            {
                '~'
            });
        }

        public IList<FilterToken> Tokenize()
        {
            var list = new List<FilterToken>();
            while (currentCharacterIndex < input.Length)
            {
                string result;
                if (TryParseIdentifier(out result))
                    list.Add(Identifier(result));
                else if (TryParseNumber(out result))
                    list.Add(Number(result));
                else if (TryParseString(out result))
                    list.Add(String(result));
                else if (TryParseCharacter(out result, '('))
                    list.Add(LeftParenthesis(result));
                else if (TryParseCharacter(out result, ')'))
                {
                    list.Add(RightParenthesis(result));
                }
                else
                {
                    if (!TryParseCharacter(out result, ','))
                        throw new FilterParserException("Expected token");
                    list.Add(Comma(result));
                }
            }
            return list;
        }

        private static bool IsComparisonOperator(string value)
        {
            return Array.IndexOf(comparisonOperators, value) > -1;
        }

        private static bool IsLogicalOperator(string value)
        {
            return Array.IndexOf(logicalOperators, value) > -1;
        }

        private static bool IsBoolean(string value)
        {
            return Array.IndexOf(booleans, value) > -1;
        }

        private static bool IsFunction(string value)
        {
            return Array.IndexOf(functions, value) > -1;
        }

        private static FilterToken Comma(string result)
        {
            return new FilterToken
            {
                TokenType = FilterTokenType.Comma,
                Value = result
            };
        }

        private static FilterToken Boolean(string result)
        {
            return new FilterToken
            {
                TokenType = FilterTokenType.Boolean,
                Value = result
            };
        }

        private static FilterToken RightParenthesis(string result)
        {
            return new FilterToken
            {
                TokenType = FilterTokenType.RightParenthesis,
                Value = result
            };
        }

        private static FilterToken LeftParenthesis(string result)
        {
            return new FilterToken
            {
                TokenType = FilterTokenType.LeftParenthesis,
                Value = result
            };
        }

        private static FilterToken String(string result)
        {
            return new FilterToken
            {
                TokenType = FilterTokenType.String,
                Value = result
            };
        }

        private static FilterToken Number(string result)
        {
            return new FilterToken
            {
                TokenType = FilterTokenType.Number,
                Value = result
            };
        }

        private FilterToken Date(string dtValue)
        {
            TryParseString(out dtValue);
            return new FilterToken
            {
                TokenType = FilterTokenType.DateTime,
                Value = dtValue
            };
        }

        private static FilterToken ComparisonOperator(string result)
        {
            return new FilterToken
            {
                TokenType = FilterTokenType.ComparisonOperator,
                Value = result
            };
        }

        private static FilterToken LogicalOperator(string result)
        {
            if (result == "or")
                return new FilterToken
                {
                    TokenType = FilterTokenType.Or,
                    Value = result
                };
            if (result == "and")
                return new FilterToken
                {
                    TokenType = FilterTokenType.And,
                    Value = result
                };
            return new FilterToken
            {
                TokenType = FilterTokenType.Not,
                Value = result
            };
        }

        private static FilterToken Function(string result)
        {
            return new FilterToken
            {
                TokenType = FilterTokenType.Function,
                Value = result
            };
        }

        private static FilterToken Property(string result)
        {
            return new FilterToken
            {
                TokenType = FilterTokenType.Property,
                Value = result
            };
        }

        private FilterToken Identifier(string result)
        {
            if (result == "datetime")
                return Date(result);
            if (IsComparisonOperator(result))
                return ComparisonOperator(result);
            if (IsLogicalOperator(result))
                return LogicalOperator(result);
            if (IsBoolean(result))
                return Boolean(result);
            if (IsFunction(result))
                return Function(result);
            return Property(result);
        }

        private void SkipSeparators()
        {
            char ch = Peek();
            while (ch == 126)
                ch = Next();
        }

        private bool TryParseCharacter(out string character, char whatCharacter)
        {
            SkipSeparators();
            char ch = Peek();
            if (ch != whatCharacter)
            {
                character = null;
                return false;
            }
            Next();
            character = ch.ToString(CultureInfo.InvariantCulture);
            return true;
        }

        private bool TryParseString(out string @string)
        {
            SkipSeparators();
            if (Peek() != 39)
            {
                @string = null;
                return false;
            }
            Next();
            var result = new StringBuilder();
            @string = Read(character =>
            {
                if (character == ushort.MaxValue)
                    throw new FilterParserException("Unterminated string");
                if (character != 39 || Peek(1) != 39)
                    return (int)character != 39;
                Next();
                return true;
            }, result);
            Next();
            return true;
        }

        private bool TryParseNumber(out string number)
        {
            SkipSeparators();
            char c = Peek();
            var result = new StringBuilder();
            int decimalSymbols = 0;
            if (c == 45 || c == 43)
            {
                result.Append(c);
                c = Next();
            }
            if (c == 46)
            {
                ++decimalSymbols;
                result.Append(c);
                c = Next();
            }
            if (!char.IsDigit(c))
            {
                number = null;
                return false;
            }
            number = Read(character =>
            {
                if (character != 46)
                    return char.IsDigit(character);
                if (decimalSymbols >= 1)
                    throw new FilterParserException("A number cannot have more than one decimal symbol");
                ++decimalSymbols;
                return true;
            }, result);
            return true;
        }

        private bool TryParseIdentifier(out string identifier)
        {
            SkipSeparators();
            char character = Peek();
            var result = new StringBuilder();
            if (!IsIdentifierStart(character))
            {
                identifier = null;
                return false;
            }
            result.Append(character);
            Next();
            identifier = Read(c =>
            {
                if (!IsIdentifierPart(c))
                    return (int)c == 46;
                return true;
            }, result);
            return true;
        }

        private static bool IsIdentifierPart(char character)
        {
            if (!char.IsLetter(character) && !char.IsDigit(character) && character != 95)
                return character == 36;
            return true;
        }

        private static bool IsIdentifierStart(char character)
        {
            if (!char.IsLetter(character) && character != 95 && character != 36)
                return character == 64;
            return true;
        }

        private string Read(Func<char, bool> predicate, StringBuilder result)
        {
            for (char ch = Peek(); predicate(ch); ch = Next())
                result.Append(ch);
            return result.ToString();
        }

        private char Peek(int chars = 0)
        {
            if (currentCharacterIndex + chars < input.Length)
                return input[currentCharacterIndex + chars];
            return char.MaxValue;
        }

        private char Next()
        {
            ++currentCharacterIndex;
            return Peek();
        }
    }
}