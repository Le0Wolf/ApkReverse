namespace ApkReverse.Core.ApkUtils.ResourcesModels
{
    using System;
    using System.IO;

    /// <summary>
    /// The string pool header
    /// <remarks>
    /// String pool structure:
    ///     StringPoolHeader;
    ///     List of uint offsets of strings;
    ///     List of uint offsets of styles;
    ///     List of String length and null terminated string;
    ///     Styles
    /// </remarks>
    /// </summary>
    [Serializable]
    public struct StringPoolHeader
    {
        /// <summary>
        /// ref to BaseHeader
        /// </summary>
        public BaseHeader Header;

        /// <summary>
        /// Count of uint values of strings offset
        /// </summary>
        public uint StringCount;

        /// <summary>
        /// Count of uint values of styles offset
        /// </summary>
        public uint StyleCount;

        /// <summary>
        /// Flags types
        /// </summary>
        [Flags]
        public enum FlagType : uint {
            /// <summary>
            /// if string list is sorted
            /// </summary>
            SortedFlag = 1 << 0,

            /// <summary>
            /// if string list encoded as utf-8, else utf-16
            /// </summary>
            Utf8Flag = 1 << 8
        };

        /// <summary>
        /// Flags
        /// </summary>
        public FlagType Flags;

        /// <summary>
        /// Offset of start of this header to start of strings. 
        /// Absolute string offset = 12 (This header offset) + StringsStart + string offset
        /// </summary>
        public uint StringsStart;

        /// <summary>
        /// Offset of start of this header to start of styles. 
        /// Absolute style offset = 12 (This header offset) + StylesStart + style offset
        /// </summary>
        public uint StylesStart;

        /// <summary>
        /// StringPoolHeader constructor
        /// </summary>
        /// <param name="reader"></param>
        public StringPoolHeader(BinaryReader reader)
        {
            var header = new BaseHeader(reader);

            if (header.Type != BaseHeader.ChunkType.StringPool)
                throw new ResourceParsingException($"Resources parse error: unexpected chunk type '{header.Type}', expecting '{BaseHeader.ChunkType.StringPool}' at offset {reader.BaseStream.Position - 1}");

            if (header.HeaderSize != 0x001c)
                throw new ResourceParsingException($"Resources parse error: unexpected chunk header size '{header.HeaderSize}, expecting {0x001c}");

            this.Header = header;
            this.StringCount = reader.ReadUInt32();
            this.StyleCount = reader.ReadUInt32();

            var flags = (FlagType)reader.ReadUInt32();

            if (flags != 0 && flags != FlagType.SortedFlag
                && flags != FlagType.Utf8Flag
                && flags != (FlagType.SortedFlag | FlagType.Utf8Flag))
                throw new ResourceParsingException($"Resources parse error: unexpected flag value: {flags}, expecting '{FlagType.Utf8Flag}', '{FlagType.SortedFlag}'");

            this.Flags = flags;
            this.StringsStart = reader.ReadUInt32();
            this.StylesStart = reader.ReadUInt32();
        }

        /// <summary>
        /// Return all content of StringPoolHeader as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                $"String pool header: {this.Header}; Flags {this.Flags}, StringsCount {this.StringCount}, StringsStart {this.StringsStart}, StyleCount {this.StyleCount}, StyleStart {this.StylesStart}";
        }
    };
}
