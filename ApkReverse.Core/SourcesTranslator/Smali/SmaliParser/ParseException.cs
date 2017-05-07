namespace ApkReverse.Core.SourcesTranslator.Smali.SmaliParser
{
    using System;
    using System.Collections;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// A parse exception.
    /// </summary>
    [Serializable]
    public class ParseException : Exception
    {

        /// <summary>
        /// The error type enumeration.
        /// </summary>
        public enum ErrorType
        {

            /// <summary>
            /// The internal error type is only used to signal an error
            /// that is a result of a bug in the parser or tokenizer
            /// code.
            /// </summary>
            Internal,

            /// <summary>
            /// The I/O error type is used for stream I/O errors.
            /// </summary>
            Io,

            /// <summary>
            /// The unexpected end of file error type is used when end
            /// of file is encountered instead of a valid token.
            /// </summary>
            UnexpectedEof,

            /// <summary>
            /// The unexpected character error type is used when a
            /// character is read that isn't handled by one of the
            /// token patterns.
            /// </summary>
            UnexpectedChar,

            /// <summary>
            /// The unexpected token error type is used when another
            /// token than the expected one is encountered.
            /// </summary>
            UnexpectedToken,

            /// <summary>
            /// The invalid token error type is used when a token
            /// pattern with an error message is matched. The
            /// additional information provided should contain the
            /// error message.
            /// </summary>
            InvalidToken,

            /// <summary>
            /// The analysis error type is used when an error is
            /// encountered in the analysis. The additional information
            /// provided should contain the error message.
            /// </summary>
            Analysis
        }

        /**
         * The additional details information. This variable is only
         * used for unexpected token errors.
         */
        private readonly ArrayList _details;

        /// <summary>
        /// Creates a new parse exception.
        /// </summary>
        /// <param name="type">the parse error type</param>
        /// <param name="info">the additional information</param>
        /// <param name="line">the line number, or -1 for unknown</param>
        /// <param name="column">the column number, or -1 for unknown</param>
        public ParseException(ErrorType type,
                              string info,
                              int line,
                              int column)
            : this(type, info, null, line, column)
        {
        }

        /// <summary>
        /// Creates a new parse exception. This constructor is only
        /// used to supply the detailed information array, which is
        /// only used for expected token errors.The list then contains
        /// descriptions of the expected tokens.
        /// </summary>
        /// <param name="type">the parse error type</param>
        /// <param name="info">the additional information</param>
        /// <param name="details">the additional detailed information</param>
        /// <param name="line">the line number, or -1 for unknown</param>
        /// <param name="column">the column number, or -1 for unknown</param>
        public ParseException(ErrorType type,
                              string info,
                              ArrayList details,
                              int line,
                              int column)
        {

            this.Type = type;
            this.Info = info;
            this._details = details;
            this.Line = line;
            this.Column = column;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ParseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// The error type property (read-only).
        /// </summary>
        public ErrorType Type { get; }

        /// <summary>
        /// The additional error information property (read-only).
        /// </summary>
        public string Info { get; }

        /// <summary>
        /// The additional detailed error information property
        /// (read-only).
        /// </summary>
        public ArrayList Details => new ArrayList(this._details);

        /// <summary>
        /// The line number property (read-only). This is the line
        /// number where the error occured, or -1 if unknown.
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// The column number property (read-only). This is the column
        /// number where the error occured, or -1 if unknown.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// The message property (read-only). This property contains
        /// the detailed exception error message, including line and
        /// column numbers when available.
        /// </summary>
        /// 
        public override string Message
        {
            get
            {
                var buffer = new StringBuilder();

                // Add error description
                buffer.Append(this.ErrorMessage);

                // Add line and column
                if (this.Line > 0 && this.Column > 0)
                {
                    buffer.Append(", on line: ");
                    buffer.Append(this.Line);
                    buffer.Append(" column: ");
                    buffer.Append(this.Column);
                }

                return buffer.ToString();
            }
        }

        /// <summary>
        /// The error message property (read-only). This property
        /// contains all the information available, except for the line
        /// and column number information.
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                var buffer = new StringBuilder();

                // Add type and info
                switch (this.Type)
                {
                    case ErrorType.Io:
                        buffer.Append("I/O error: ");
                        buffer.Append(this.Info);
                        break;
                    case ErrorType.UnexpectedEof:
                        buffer.Append("unexpected end of file");
                        break;
                    case ErrorType.UnexpectedChar:
                        buffer.Append("unexpected character '");
                        buffer.Append(this.Info);
                        buffer.Append("'");
                        break;
                    case ErrorType.UnexpectedToken:
                        buffer.Append("unexpected token ");
                        buffer.Append(this.Info);
                        if (this._details != null)
                        {
                            buffer.Append(", expected ");
                            if (this._details.Count > 1)
                            {
                                buffer.Append("one of ");
                            }

                            buffer.Append(this.GetMessageDetails());
                        }

                        break;
                    case ErrorType.InvalidToken:
                        buffer.Append(this.Info);
                        break;
                    case ErrorType.Analysis:
                        buffer.Append(this.Info);
                        break;
                    default:
                        buffer.Append("internal error");
                        if (this.Info != null)
                        {
                            buffer.Append(": ");
                            buffer.Append(this.Info);
                        }

                        break;

                }

                return buffer.ToString();
            }
        }

        /// <summary>
        /// Returns a string containing all the detailed information in
        /// a list.The elements are separated with a comma.
        /// </summary>
        /// 
        /// <returns>the detailed information string</returns>
        private string GetMessageDetails()
        {
            var buffer = new StringBuilder();

            for (var i = 0; i < this._details.Count; i++)
            {
                if (i > 0)
                {
                    buffer.Append(", ");
                    if (i + 1 == this._details.Count)
                    {
                        buffer.Append("or ");
                    }
                }

                buffer.Append(this._details[i]);
            }

            return buffer.ToString();
        }
    }
}
