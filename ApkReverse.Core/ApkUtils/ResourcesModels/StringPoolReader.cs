namespace ApkReverse.Core.ApkUtils.ResourcesModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Headers;

    /// <summary>
    /// String pool structure. Used in other structures for store service information
    /// </summary>
    /// <remarks>
    /// Structure:
    ///     StringPoolHeader structure
    ///     Strings offsets - array of uint32
    ///     Styles offsets - array of uint32
    ///     Strings
    ///     Styles
    /// </remarks>
    internal class StringPoolReader
    {
        private readonly BinaryReader _reader;

        /// <summary>
        /// List of all strings from string pool, used from packages
        /// </summary>
        private long[] _stringsOffsets;

        /// <summary>
        /// List of all strings from string pool, used from packages
        /// </summary>
        private long[] _stylesOffsets;

        private StringPoolHeader _header;

        private readonly long _baseOffset;

        public uint Size => this._header.Header.Size;

        public uint StringsCount => this._header.StringsCount;

        public uint StylesCount => this._header.StylesCount;

        /// <summary>
        /// Create String pool from current position
        /// </summary>
        public StringPoolReader(BinaryReader reader) : this(reader, reader.BaseStream.Position)
        {
        }

        /// <summary>
        /// Create StringPool at offset
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="offset"></param>
        public StringPoolReader(BinaryReader reader, long offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            this._reader = reader;
            this._baseOffset = reader.BaseStream.Position;
            this._header = new StringPoolHeader(reader);
        }

        /// <summary>
        /// String pool strings getter
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GeString(int index)
        {
            var lastOffset = this._reader.BaseStream.Position; //Save last position

            if (this._stringsOffsets == null)
            {
                this.InitStrings();
            }
            if (index > this.StringsCount)
                throw new IndexOutOfRangeException(string.Format(CoreResources.StringPoolIndexError, index));

            this._reader.BaseStream.Seek(this._stringsOffsets[index], SeekOrigin.Begin);
            var result = this._header.Flags.HasFlag(StringPoolHeader.FlagType.Utf8Flag)
                             ? this.ReadUtf8String()
                             : this.ReadUtf16String();

            this._reader.BaseStream.Seek(lastOffset, SeekOrigin.Begin); //Restore position

            return result;
        }

        /// <summary>
        /// Fill string pool strings offsets array from binary resource file
        /// </summary>
        /// <returns></returns>
        private void InitStrings()
        {
            if (this._header.StringsCount > 0)
            {
                var stringsOffset = this._baseOffset + this._header.StringsStart;
                this._reader.BaseStream.Seek(this._baseOffset + this._header.Header.HeaderSize, SeekOrigin.Begin);//Skip header and previous data
                this._stringsOffsets = new long[this._header.StringsCount];
                for (var i = 0; i < this._header.StringsCount; i++)
                {
                    this._stringsOffsets[i] = this._reader.ReadUInt32() + stringsOffset;
                }
            }
        }

        /// <summary>
        /// Fill string pool styles offsets array from binary resource file
        /// </summary>
        /// <returns></returns>
        private void InitStyles()
        {
            if (this._header.StylesCount > 0)
            {
                var stylesOffset = this._baseOffset + this._header.StylesStart;
                this._reader.BaseStream.Seek(this._baseOffset + this._header.Header.HeaderSize + this._header.StringsCount * sizeof(uint), SeekOrigin.Begin);//Skip previous data, header and strings offsets
                this._stylesOffsets = new long[this._header.StylesCount];
                for (var i = 0; i < this._header.StylesCount; i++)
                {
                    this._stylesOffsets[i] = this._reader.ReadUInt32() + stylesOffset;
                }
            }
        }

        /// <summary>
        /// Read utf-8 string from binary resources file
        /// </summary>
        /// <remarks>
        /// String structure in file
        ///     One or two bytes of size, null or two bytes of align, null or more bytes of string content, 0x00 (null terminal) 
        /// </remarks>
        /// <returns></returns>
        private string ReadUtf8String()
        {
            var lengths = this._reader.ReadBytes(2);
            uint len;
            if ((lengths[0] & 0x80u) == 0u)
                len = lengths[0];
            else
            {
                //drop the high bit of the first character and add it together with the next character
                len = ((lengths[0] & 0x7Fu) << 8) | lengths[1];
                this._reader.BaseStream.Seek(2, SeekOrigin.Current);//skip two bytes (it used in utf-16 only)
            }

            var byteArray = this._reader.ReadBytes((int)len);
            var endChr = this._reader.ReadByte();

            if (endChr != 0)
                throw new ResourceParsingException(
                    string.Format(
                        CoreResources.StringTerminatorError,
                        endChr,
                        this._reader.BaseStream.Position - sizeof(byte)));

            return System.Text.Encoding.UTF8.GetString(byteArray);
        }

        [Obsolete("Used not tested method: ReadUtf16String()")]
        private string ReadUtf16String()
        {
            var len1 = this._reader.ReadUInt16();
            var len2 = this._reader.ReadUInt16();
            int len;
            if ((len1 & 0x8000) != 0)
            {
                len = ((len1 & 0x7FFF) << 16) | len2;
            }
            else len = len1;

            var charArray = this._reader.ReadChars(len);
            var endChr = this._reader.ReadUInt16();

            if (endChr != 0)
                throw new ResourceParsingException(
                    string.Format(
                        CoreResources.StringTerminatorError,
                        endChr,
                        this._reader.BaseStream.Position - sizeof(ushort)));

            return new string(charArray);
        }

        public override string ToString()
        {
            return
                string.Format(CoreResources.StringPoolReaderDebug, this._baseOffset, this.Size, this.StringsCount, this.StylesCount, this._header.Flags);
        }
    }
}