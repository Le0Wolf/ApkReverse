namespace ApkReverse.Core.ApkUtils.ResourcesModels
{
    using System;
    using System.IO;

    /// <summary>
    /// The resources table header, top item in Resources.arsc file, only one for the whole file
    /// <remarks>
    /// Resources tale structure:
    ///     TableHeader
    ///     StringPool
    ///     Resource package 1
    ///     ...
    ///     Resource package n
    /// </remarks>
    /// </summary>
    [Serializable]
    public struct TableHeader
    {
        /// <summary>
        /// ref to BaseHeader 
        /// </summary>
        public BaseHeader Header;

        /// <summary>
        /// Count of resources packages
        /// </summary>
        public uint PackageCount;

        /// <summary>
        /// TableHeader constructor with create base header
        /// </summary>
        /// <param name="reader"></param>
        public TableHeader(BinaryReader reader) : this(reader, new BaseHeader(reader))
        {}

        /// <summary>
        /// TableHeader constructor with using base header
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="header"></param>
        public TableHeader(BinaryReader reader, BaseHeader header)
        {
            if (header.Type != BaseHeader.ChunkType.ResTable)
                throw new ResourceParsingException($"Resources parse error: unexpected chunk type '{header.Type}', expecting '{BaseHeader.ChunkType.ResTable}' at offset { reader.BaseStream.Position - 1 }");

            if(header.HeaderSize != 12)
                throw new ResourceParsingException($"Resources parse error: unexpected chunk header size '{header.HeaderSize}, expecting 12");

            this.Header = header;
            this.PackageCount = reader.ReadUInt32();
        }

        /// <summary>
        /// Return all content of TableHeader as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"TableHeader: {this.Header}; PackagesCount {this.PackageCount}";
        }
    }
}