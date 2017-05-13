namespace ApkReverse.Core.ApkUtils.ResourcesModels
{
    using System;
    using System.IO;

    /// <summary>
    /// The base chunk header
    /// </summary>
    [Serializable]
    public struct BaseHeader
    {
        /// <summary>
        /// Enum of all supported chunk types
        /// </summary>
        public enum ChunkType : ushort
        {
            Null = 0x0000,
            StringPool = 0x0001,
            ResTable = 0x0002,
            Xml = 0x0003,

            // Chunk types in RES_XML_TYPE
            ResXmlFirstChunkType = 0x0100,
            ResXmlStartNamespaceType = 0x0100,
            ResXmlEndNamespaceType = 0x0101,
            ResXmlStartElementType = 0x0102,
            ResXmlEndElementType = 0x0103,
            ResXmlCdataType = 0x0104,
            ResXmlLastChunkType = 0x017f,
            // This contains a uint32_t array mapping strings in the string
            // pool back to resource identifiers.  It is optional.
            ResXmlResourceMapType = 0x0180,

            // Chunk types in ResTable
            ResTablePackageType = 0x0200,
            ResTableTypeType = 0x0201,
            ResTableTypeSpecType = 0x0202,
            ResTableLibraryType = 0x0203
        };
        /// <summary>
        /// Chunk type
        /// </summary>
        public readonly ChunkType Type;

        /// <summary>
        /// Chunk header size
        /// </summary>
        public ushort HeaderSize;

        /// <summary>
        /// Chunk size (Header + all content)
        /// </summary>
        public uint Size;

        /// <summary>
        /// BaseHeader constructor
        /// </summary>
        /// <param name="reader"></param>
        public BaseHeader(BinaryReader reader)
        {
            var type = reader.ReadUInt16();
            if (!Enum.IsDefined(typeof(ChunkType), type))
                throw new ResourceParsingException($"Unknown chunk type {type} at offset {reader.BaseStream.Position - 1}");

            this.Type = (ChunkType)type;
            this.HeaderSize = reader.ReadUInt16();
            this.Size = reader.ReadUInt32();
        }

        /// <summary>
        /// Return all content of BaseHeader as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Base header: Type {this.Type}, HeaderSize {this.HeaderSize}, Size {this.Size}";
        }
    }
}
