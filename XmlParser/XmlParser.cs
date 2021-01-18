﻿#nullable enable
//#define CONSOLE_DEBUG
//#define CONSOLE_DEBUG_UNSUPPORTED
//#define CONSOLE_DEBUG_ATTRIBUTES
//#define CREATE_ELEMENTS
using System;
using System.Collections.Generic;

namespace XmlParser
{
    public static class XmlParser
    {
        public const char LessThanSign = '<';
        public const char GreaterThanSign = '>';
        public const char Apostrophe = '\'';
        public const char QuotationMark = '\"';
        public const char Solidus = '/';
        public const char QuestionMark = '?';
        public const char ExclamationMark = '!';
        public const char EqualsSign = '=';
        public readonly static char[] CommentStart = new[] {'<', '!', '-', '-'};
        public readonly static char[] CommentEnd = new[] {'-', '-', '>'};
        public readonly static char[] WhitespaceChars = new[] {' ', '\t', '\n', '\r'};
        public const string UriString = "http://www.w3.org/2000/svg";

        public static void Parse(ReadOnlySpan<char> span)
        {
#if CONSOLE_DEBUG
            int indent = 0;
            int indentSize = 4;
#endif
#if CREATE_ELEMENTS
            var elements = new Stack<Element>();
            var root = default(Element);
            var element = default(Element);
#endif
            var whitespaceChars = WhitespaceChars.AsSpan();

            while (true)
            {
                // Next Tag

                var start = span.IndexOf(LessThanSign);
                if (start < 0)
                {
                    break;
                }

                var spanLength = span.Length;
                if (spanLength < 1)
                {
                    break;
                }
                var firstChar = span[start + 1];

#if true
                if (spanLength >= start + 1 && (firstChar == QuestionMark || firstChar == ExclamationMark))
                {
                    var tagEndIndex = span.IndexOf(GreaterThanSign);
                    if (tagEndIndex < 0)
                    {
                        // ERROR
                        break;
                    }
#if CONSOLE_DEBUG_UNSUPPORTED
                    var content = span.Slice(start + 2, tagEndIndex - 3 - start);
                    Console.WriteLine($"{new string(' ', indent)}<>");
                    Console.WriteLine($"{new string(' ', indent)}{content.ToString()}");
#endif
                    span = span.Slice(tagEndIndex + 1);
                    if (span.Length <= 0)
                    {
                        break;
                    }
                    continue;
                }
#endif

                // Comment
#if false
                var isComment = spanLength >= start + 4 && firstChar == CommentStart[1] && span[start + 2] == CommentStart[2] && span[start + 3] == CommentStart[3];
                if (isComment)
                {
                    var commentEnd = span.IndexOf(CommentEnd.AsSpan()) + 3;
#if CONSOLE_DEBUG
                    var commentStart = start + 4;
                    var comment = span.Slice(commentStart, commentEnd - commentStart - 3);
                    Console.WriteLine($"{new string(' ', indent)}<Comment>");
                    Console.WriteLine($"{new string(' ', indent)}{comment.ToString()}");
#endif
                    span = span.Slice(commentEnd);
                    continue;
                }
#endif
                // Attributes

                span = span.Slice(start + 1);
                var splitIndex = span.IndexOfAny(whitespaceChars);
                if (splitIndex <= 0)
                {
                    break;
                }

                var endIndex = span.IndexOf(GreaterThanSign);
                if (endIndex < 0)
                {
                    break;
                }
                bool isSelfEnd = (endIndex > 0 && span[endIndex - 1] == Solidus);
                bool isEnd = span[0] == Solidus;
                bool hasAttributes = span[splitIndex - 1] != GreaterThanSign;

                var endOffset = isEnd && !isSelfEnd ? 1 : 0;
                var elementEnd = hasAttributes ? 
                                 splitIndex - endOffset 
                                 : (splitIndex > endIndex ? endIndex - endOffset : splitIndex - endOffset - 1);

                var elementName = span.Slice(endOffset, elementEnd);

                if (!isEnd)
                {
#if CONSOLE_DEBUG
                    Console.WriteLine($"{new string(' ', indent)}<Element> '{elementName.ToString()}' Attributes={hasAttributes}, {endOffset}:{elementEnd}");
                    indent += indentSize;
#endif
#if CREATE_ELEMENTS
                    element = new Element()
                    {
                        ElementName = elementName.ToString()
                    };
#endif
                    var nextIndex = span.IndexOf(LessThanSign);
                    if (nextIndex > 0)
                    {
                        // Content
                        var contentStart = endIndex + 1;
                        var contentLength = nextIndex - endIndex - 1;
                        var content = span.Slice(contentStart, contentLength).Trim();
#if CONSOLE_DEBUG
                        if (content.Length > 0)
                        {
                            Console.WriteLine($"{new string(' ', indent)}<Content>");
                            Console.WriteLine($"{new string(' ', indent)}'{content.ToString()}'");
                        }
#endif
#if CREATE_ELEMENTS
                        if (content.Length > 0)
                        {
                            element.Content = content.ToString();
                        }
#endif
                    }
#if CREATE_ELEMENTS
                    if (elements.Count == 0)
                    {
                        root = element;
                    }

                    if (elements.Count > 0)
                    {
                        var parent = elements.Peek();
                        parent.AddChild(element);
                    }

                    elements.Push(element);
#endif
                }
#if CONSOLE_DEBUG
                if (isEnd || isSelfEnd)
                {
                    indent -= indentSize;
                }
#endif
#if CREATE_ELEMENTS
                if (isEnd || isSelfEnd)
                {
                    element = elements.Pop();
                }
#endif
                if (hasAttributes)
                {
                    var attributes = span.Slice(splitIndex, endIndex - splitIndex);
#if CONSOLE_DEBUG_ATTRIBUTES
                    Console.WriteLine($"{new string(' ', indent)}<Attributes>");
#endif
                    while (true)
                    {
                        var attributeSplitIndex = attributes.IndexOf(EqualsSign);
                        if (attributeSplitIndex < 0)
                        {
                            // ERROR
                            break;
                        }

                        var attributeKey = attributes.Slice(0, attributeSplitIndex);

                        attributes = attributes.Slice(attributeSplitIndex + 1);
                        var attributeStartValueIndex1 = attributes.IndexOf(QuotationMark);
                        var attributeStartValueIndex2 = attributes.IndexOf(Apostrophe);
                        if (attributeStartValueIndex1 < 0 && attributeStartValueIndex2 < 0)
                        {
                            // ERROR
                            break;
                        }
                        attributes = attributes.Slice(attributeStartValueIndex1 >= 0 ? attributeStartValueIndex1 + 1 : attributeStartValueIndex2 + 1);
                        var attributeEndValueIndex1 = attributes.IndexOf(QuotationMark);
                        var attributeEndValueIndex2 = attributes.IndexOf(Apostrophe);
                        if (attributeEndValueIndex1 < 0 && attributeEndValueIndex2 < 0)
                        {
                            // ERROR
                            break;
                        }
                        var attributeValue = attributes.Slice(0, attributeEndValueIndex1 >= 0 ? attributeEndValueIndex1 : attributeEndValueIndex2);

// Filter Attributes
/*
if (elementName.SequenceEqual("path".AsSpan()))
{
    if (attributeKey.Trim().SequenceEqual("d".AsSpan()))
    {
        Console.WriteLine($"d={attributeValue.ToString()}");
    }
}
*/

#if CONSOLE_DEBUG_ATTRIBUTES
                        Console.WriteLine($"{new string(' ', indent)}[\"{attributeKey.Trim().ToString()}\"] = \"{attributeValue.ToString()}\"");
#endif
#if CREATE_ELEMENTS
                        element?.AddAttribute(attributeKey.Trim().ToString(), attributeValue.ToString());
#endif
                        attributes = attributes.Slice(attributeEndValueIndex1 >= 0 ? attributeEndValueIndex1 + 1 : attributeEndValueIndex2 + 1);
                    }

                    span = span.Slice(endIndex + 1);
                    if (span.Length == 0)
                    {
                        break;
                    }
                    continue;
                }
                else
                {
#if CONSOLE_DEBUG
                    if (isEnd)
                    {
                        Console.WriteLine($"{new string(' ', indent)}<EndElement> '{elementName.ToString()}'");
                    }
#endif
                    var tagEndIndex = span.IndexOf(GreaterThanSign);
                    span = span.Slice(tagEndIndex + 1);
                    if (span.Length == 0)
                    {
                        break;
                    }
                }
            }
        }   
    }
}