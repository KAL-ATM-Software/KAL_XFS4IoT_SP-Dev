/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTServer
{
    public class XFSFormReader
    {
        public enum FormKeyword
        {
            //XFSFORM
            UNIT, SIZE, ALIGNMENT, ORIENTATION, SKEW, VERSION, CPI, LPI, POINTSIZE, COPYRIGHT, TITLE, COMMENT, USERPROMPT, LANGUAGE,

            //XFSFIELD
            POSITION, FOLLOWS, HEADER, FOOTER, SIDE, /*SIZE,*/ INDEX, TYPE, SCALING, BARCODE, COERCIVITY, CLASS, ACCESS, OVERFLOW, STYLE, CASE,
            HORIZONTAL, VERTICAL, COLOR, RGBCOLOR, FONT, /*POINTSIZE, CPI, LPI,*/ FORMAT, INITIALVALUE,

            //XFSMEDIA
            SOURCE, PRINTAREA, RESTRICTED, FOLD, STAGGERING, PAGE, LINES
        }

        public enum TokenType
        {
            XFSFORM, XFSFIELD, XFSMEDIA, BEGIN, END, KEYWORD, INTEGER, STRING, ENUM, COMMA, OR, UNKNOWN, EOF
        }

        /// <summary>
        /// Struct to hold information on the current token
        /// </summary>
        private struct CurrentTokenInfo
        {
            public TokenType Type;
            public FormKeyword? Keyword;
            public int? IntVal;
            public int Line;
            public int Position;
            public int Length;

            internal TokenType SetEOF()
            {
                Type = TokenType.EOF;
                Keyword = null;
                IntVal = null;
                Length = 0;
                return Type;
            }

            public override string ToString() => $"{{ Type: {Type},Line: {Line},Position: {Position},Length: {Length}{(Keyword != null ? ",Keyword:" + Keyword : "")}{(IntVal != null ? ",IntVal: " + IntVal : "")}}}";
        }

        private readonly string[] form;

        private int lineNum = 0;
        private int linePos = 0;
        private CurrentTokenInfo CurrentToken = new();
        private CurrentTokenInfo PeekToken = new();
        public TokenType CurrentTokenType { get => CurrentToken.Type; }

        /// <summary>
        /// Create a new XFSFormReader instance for the specified definition
        /// </summary>
        /// <param name="definition">The form or media definition</param>
        public XFSFormReader(string definition)
        {
            this.form = definition.Split(new string[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
        }

        /// <summary>
        /// Read the current Token as the Enum Type specified
        /// </summary>
        private T CurrentEnum<T>() where T : System.Enum
        {
            FormReaderAssertToken(TokenType.ENUM);
            return (T)Enum.Parse(typeof(T), form[CurrentToken.Line].Substring(CurrentToken.Position, CurrentToken.Length));
        }

        /// <summary>
        /// Read the next token as the Enum Type specified
        /// </summary>
        public T ReadEnum<T>() where T : System.Enum
        {
            FormReaderAssertToken(TokenType.ENUM, true);
            return CurrentEnum<T>();
        }

        /// <summary>
        /// Read the next token as an integer
        /// </summary>
        public int ReadInt()
        {
            FormReaderAssertToken(TokenType.INTEGER, true);
            return CurrentToken.IntVal.Value;
        }

        /// <summary>
        /// Read the next token as a string
        /// </summary>
        public string ReadString()
        {
            FormReaderAssertToken(TokenType.STRING, true);
            return form[CurrentToken.Line].Substring(CurrentToken.Position, CurrentToken.Length);
        }

        /// <summary>
        /// Retrieve the current Keyword token value
        /// </summary>
        public FormKeyword CurrentKeyword()
        {
            FormReaderAssertToken(TokenType.KEYWORD);
            return CurrentToken.Keyword.Value;
        }

        /// <summary>
        /// Assert the current token is as expected.
        /// Will throw a FormParseException with details otherwise.
        /// </summary>
        /// <exception cref="FormParseException"></exception>
        public void FormReaderAssertToken(TokenType Expected, bool readOne = false)
        {
            if (readOne)
                ReadNextToken();
            if (CurrentTokenType == Expected)
                return;
            // accept BARCODE as both Keyword and Enum
            if (CurrentToken.Keyword == FormKeyword.BARCODE && Expected == TokenType.ENUM)
                return;

            throw BuildParseException($"Expected token type {Expected} but got {CurrentTokenType}");
        }
        /// <summary>
        /// Assert the bool condition is true
        /// Will throw a FormParseException with details otherwise.
        /// </summary>
        /// <exception cref="FormParseException"></exception>
        public void FormReaderAssert(bool cond, string msg)
        {
            if (cond)
                return;

            throw BuildParseException(msg);
        }

        /// <summary>
        /// Build a FormParseException with details on the readers current state.
        /// </summary>
        private FormParseException BuildParseException(string msg)
        {
            if(lineNum < form.Length)
                return new FormParseException(msg + $" At line:{lineNum},position:{linePos}. Line=\"{form[lineNum][..linePos] + ">>" + form[lineNum][linePos..]}\". CurrentTokenInfo={CurrentToken}", form[lineNum], lineNum, linePos);
            else
                return new FormParseException(msg + $" At line:{form.Length-1},position:{linePos}. Line=\"{form[^1]}\". CurrentTokenInfo={CurrentToken}", form[^1], form.Length-1, linePos);
        }

        //Constants
        private static ReadOnlySpan<char> CONST_WHITESPACE_CHARS => " \t\r\n".AsSpan();
        private static ReadOnlySpan<char> CONST_COMMENT => "//".AsSpan();
        private static ReadOnlySpan<char> CONST_QUOTE => "\"".AsSpan();
        private static ReadOnlySpan<char> CONST_COMMA => ",".AsSpan();
        private static ReadOnlySpan<char> CONST_OR => "|".AsSpan();
        private static ReadOnlySpan<char> CONST_TOKENSEP => ", \t\r\n|".AsSpan();
        private static ReadOnlySpan<char> CONST_XFSFORM => "XFSFORM".AsSpan();
        private static ReadOnlySpan<char> CONST_BEGIN => "BEGIN".AsSpan();
        private static ReadOnlySpan<char> CONST_END => "END".AsSpan();
        private static ReadOnlySpan<char> CONST_XFSFIELD => "XFSFIELD".AsSpan();
        private static ReadOnlySpan<char> CONST_XFSMEDIA => "XFSMEDIA".AsSpan();

        /// <summary>
        /// Read the next token
        /// </summary>
        public TokenType ReadNextToken()
        {
            return InternalGetNextToken(ref lineNum, ref linePos, ref CurrentToken);
        }

        /// <summary>
        /// Peek the next token without advancing the reader
        /// </summary>
        /// <returns></returns>
        public TokenType PeekNextToken()
        {
            int peekLineNum = lineNum;
            int peekLinePos = linePos;
            return InternalGetNextToken(ref peekLineNum, ref peekLinePos, ref PeekToken);
        }

        private TokenType InternalGetNextToken(ref int peekLineNum, ref int peekLinePos, ref CurrentTokenInfo peekNode)
        {
            TokenType type;
            int intVal = -1;
            FormKeyword keyword = default;
            int tokenLength = 0;

            //Check if we have reached EOF
            if (lineNum >= form.Length)
                return peekNode.SetEOF();

            //Ignore empty lines or comment lines
            while (linePos == 0 && (form[lineNum].StartsWith("//") || string.IsNullOrWhiteSpace(form[lineNum])))
            {
                peekLineNum++;
                if (peekLineNum >= form.Length)
                    return peekNode.SetEOF();
            }

            //Find the start of a new token
            for (int i = peekLinePos; i < form[lineNum].Length; i++)
            {
                if (CONST_WHITESPACE_CHARS.Contains(form[lineNum][i]))
                    peekLinePos++;
                else // Token starts here
                {
                    break;
                }
            }

            //Check we found a new token, if not move on to the next line.
            if (peekLinePos == -1 || peekLinePos >= form[lineNum].Length)
            {
                peekLineNum++; peekLinePos = 0;
                return InternalGetNextToken(ref peekLineNum, ref peekLinePos, ref peekNode);
            }

            //Get the rest of the line as a span
            ReadOnlySpan<char> lineSpan = form[lineNum].AsSpan()[peekLinePos..];

            // text between comment start and the end of the line is ignored
            if (lineSpan.StartsWith("//"))
            {
                peekLineNum++; peekLinePos = 0;
                return InternalGetNextToken(ref peekLineNum, ref peekLinePos, ref peekNode);
            }


            //If the token is a string surrounded by quotes
            if (lineSpan.StartsWith(CONST_QUOTE))
            {
                //Find end quote
                tokenLength = lineSpan[1..].IndexOf(CONST_QUOTE) + 2;
                //Check the found quote isn't escaped
                while (lineSpan[tokenLength - 2] == '\\')
                {
                    //Keep searching till we find an un-escaped double quote
                    int index = lineSpan[tokenLength..].IndexOf(CONST_QUOTE);
                    FormReaderAssert(index != -1, "Unable to find end quote for String.");
                    tokenLength += index + 1;
                }
            }
            //Check if the token is a , or |
            else if (lineSpan.StartsWith(CONST_COMMA) || lineSpan.StartsWith(CONST_OR))
                tokenLength = 1;
            else
            {
                //Get the end of the token
                tokenLength = lineSpan.IndexOfAny(CONST_TOKENSEP);
            }

            //If we found no end, take the whole lineSpan
            if (tokenLength == -1)
                tokenLength = lineSpan.Length;

            //Check we have some value
            FormReaderAssert(tokenLength > 0 && !lineSpan.IsWhiteSpace(), "Could not find any token in the current line.");

            //Get the token now
            ReadOnlySpan<char> theToken = lineSpan[..tokenLength];

            //Check possible types for the token
            if (theToken.Equals(CONST_XFSFORM, StringComparison.Ordinal))
                type = TokenType.XFSFORM;

            else if (theToken.Equals(CONST_XFSFIELD, StringComparison.Ordinal))
                type = TokenType.XFSFIELD;

            else if (theToken.Equals(CONST_XFSMEDIA, StringComparison.Ordinal))
                type = TokenType.XFSMEDIA;

            else if (theToken.Equals(CONST_BEGIN, StringComparison.Ordinal))
                type = TokenType.BEGIN;

            else if (theToken.Equals(CONST_END, StringComparison.Ordinal))
                type = TokenType.END;

            else if (theToken.Equals(CONST_COMMA, StringComparison.Ordinal))
                type = TokenType.COMMA;

            else if (theToken.Equals(CONST_OR, StringComparison.Ordinal))
                type = TokenType.OR;

            else if (theToken.StartsWith(CONST_QUOTE) && theToken.EndsWith(CONST_QUOTE))
                type = TokenType.STRING;

            else if (int.TryParse(theToken, out intVal))
                type = TokenType.INTEGER;

            else if (Enum.TryParse(theToken.ToString(), false, out keyword))
                type = TokenType.KEYWORD;

            else if (theToken.Length < 100)
                type = TokenType.ENUM;

            else
                type = TokenType.UNKNOWN;

            FormReaderAssert(type != TokenType.UNKNOWN, "Unknown token found.");

            //Update the node with the token details.
            peekNode.Type = type;
            peekNode.Line = peekLineNum;
            peekNode.Position = peekLinePos;
            peekNode.Length = tokenLength;
            if (type is TokenType.STRING)
            {
                //Strip surrounding quotes.
                peekNode.Position += 1;
                peekNode.Length -= 2;
            }
            //Store the values if we have them.
            peekNode.Keyword = (type is TokenType.KEYWORD) ? keyword : null;
            peekNode.IntVal = (type is TokenType.INTEGER) ? intVal : null;

            //Advance to next token
            peekLinePos += tokenLength;
            if (peekLinePos >= form[lineNum].Length)
            {
                peekLineNum++;
                peekLinePos = 0;
            }
            return type;
        }

        /// <summary>
        /// Exception thrown when an issue is found within a XFSForm.
        /// </summary>
        public class FormParseException : Exception
        {
            /// <summary>
            /// Exception thrown when an issue is found within a XFSForm.
            /// </summary>
            /// <param name="message">Message describing the issue.</param>
            /// <param name="line">Line the reader was on when the exception occured.</param>
            /// <param name="lineNum">Line number the reader was at when the exception occured.</param>
            /// <param name="linePos">Line position the reader was at when the exception occured.</param>
            public FormParseException(string message, string line, int lineNum, int linePos) : base(message)
            {
                this.Line = line;
                this.LineNum = lineNum;
                this.LinePos = linePos;
            }

            /// <summary>
            /// Line the reader was on when the exception occured.
            /// </summary>
            public string Line { get; init; }

            /// <summary>
            /// Line number the reader was at when the exception occured.
            /// </summary>
            public int LineNum { get; init; }

            /// <summary>
            /// Line position the reader was at when the exception occured.
            /// </summary>
            public int LinePos { get; init; }
        }
    }
}
