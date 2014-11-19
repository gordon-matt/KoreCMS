using System;
using System.Collections.Generic;
using System.Globalization;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    public class FilterParser
    {
        private readonly IList<FilterToken> tokens;
        private int currentTokenIndex;

        public FilterParser(string input)
        {
            tokens = new FilterLexer(input).Tokenize();
        }

        public IFilterNode Parse()
        {
            if (tokens.Count > 0)
                return Expression();
            return null;
        }

        private IFilterNode Expression()
        {
            return OrExpression();
        }

        private IFilterNode OrExpression()
        {
            IFilterNode firstArgument = AndExpression();
            if (Is(FilterTokenType.Or))
                return ParseOrExpression(firstArgument);
            if (!Is(FilterTokenType.And))
                return firstArgument;
            Expect(FilterTokenType.And);
            return new AndNode
            {
                First = firstArgument,
                Second = OrExpression()
            };
        }

        private IFilterNode AndExpression()
        {
            IFilterNode firstArgument = ComparisonExpression();
            if (Is(FilterTokenType.And))
                return ParseAndExpression(firstArgument);
            return firstArgument;
        }

        private IFilterNode ComparisonExpression()
        {
            IFilterNode firstArgument = PrimaryExpression();
            if (Is(FilterTokenType.ComparisonOperator) || Is(FilterTokenType.Function))
                return ParseComparisonExpression(firstArgument);
            return firstArgument;
        }

        private IFilterNode PrimaryExpression()
        {
            if (Is(FilterTokenType.LeftParenthesis))
                return ParseNestedExpression();
            if (Is(FilterTokenType.Function))
                return ParseFunctionExpression();
            if (Is(FilterTokenType.Boolean))
                return ParseBoolean();
            if (Is(FilterTokenType.DateTime))
                return ParseDateTimeExpression();
            if (Is(FilterTokenType.Property))
                return ParsePropertyExpression();
            if (Is(FilterTokenType.Number))
                return ParseNumberExpression();
            if (Is(FilterTokenType.String))
                return ParseStringExpression();
            throw new FilterParserException("Expected primaryExpression");
        }

        private IFilterNode ParseOrExpression(IFilterNode firstArgument)
        {
            Expect(FilterTokenType.Or);
            IFilterNode filterNode = OrExpression();
            return new OrNode
            {
                First = firstArgument,
                Second = filterNode
            };
        }

        private IFilterNode ParseComparisonExpression(IFilterNode firstArgument)
        {
            if (Is(FilterTokenType.ComparisonOperator))
            {
                FilterToken token = Expect(FilterTokenType.ComparisonOperator);
                IFilterNode filterNode = PrimaryExpression();
                return new ComparisonNode
                {
                    First = firstArgument,
                    FilterOperator = token.ToFilterOperator(),
                    Second = filterNode
                };
            }
            else
            {
                FilterToken token = Expect(FilterTokenType.Function);
                var functionNode = new FunctionNode
                {
                    FilterOperator = token.ToFilterOperator()
                };
                functionNode.Arguments.Add(firstArgument);
                functionNode.Arguments.Add(PrimaryExpression());
                return functionNode;
            }
        }

        private IFilterNode ParseAndExpression(IFilterNode firstArgument)
        {
            Expect(FilterTokenType.And);
            IFilterNode filterNode = ComparisonExpression();
            return new AndNode
            {
                First = firstArgument,
                Second = filterNode
            };
        }

        private IFilterNode ParseStringExpression()
        {
            FilterToken filterToken = Expect(FilterTokenType.String);
            return new StringNode
            {
                Value = filterToken.Value
            };
        }

        private IFilterNode ParseBoolean()
        {
            FilterToken filterToken = Expect(FilterTokenType.Boolean);
            return new BooleanNode
            {
                Value = Convert.ToBoolean(filterToken.Value)
            };
        }

        private IFilterNode ParseNumberExpression()
        {
            FilterToken filterToken = Expect(FilterTokenType.Number);
            return new NumberNode
            {
                Value = Convert.ToDouble(filterToken.Value, CultureInfo.InvariantCulture)
            };
        }

        private IFilterNode ParsePropertyExpression()
        {
            FilterToken filterToken = Expect(FilterTokenType.Property);
            return new PropertyNode
            {
                Name = filterToken.Value
            };
        }

        private IFilterNode ParseDateTimeExpression()
        {
            FilterToken filterToken = Expect(FilterTokenType.DateTime);
            return new DateTimeNode
            {
                Value = DateTime.ParseExact(filterToken.Value, "yyyy-MM-ddTHH-mm-ss", null)
            };
        }

        private IFilterNode ParseNestedExpression()
        {
            Expect(FilterTokenType.LeftParenthesis);
            IFilterNode filterNode = Expression();
            Expect(FilterTokenType.RightParenthesis);
            return filterNode;
        }

        private IFilterNode ParseFunctionExpression()
        {
            FilterToken token = Expect(FilterTokenType.Function);
            var functionNode = new FunctionNode
            {
                FilterOperator = token.ToFilterOperator()
            };
            Expect(FilterTokenType.LeftParenthesis);
            functionNode.Arguments.Add(Expression());
            while (Is(FilterTokenType.Comma))
            {
                Expect(FilterTokenType.Comma);
                functionNode.Arguments.Add(Expression());
            }
            Expect(FilterTokenType.RightParenthesis);
            return functionNode;
        }

        private FilterToken Expect(FilterTokenType tokenType)
        {
            if (!Is(tokenType))
                throw new FilterParserException("Expected " + tokenType);
            FilterToken filterToken = Peek();
            ++currentTokenIndex;
            return filterToken;
        }

        private bool Is(FilterTokenType tokenType)
        {
            FilterToken filterToken = Peek();
            if (filterToken != null)
                return filterToken.TokenType == tokenType;
            return false;
        }

        private FilterToken Peek()
        {
            if (currentTokenIndex < tokens.Count)
                return tokens[currentTokenIndex];
            return null;
        }
    }
}