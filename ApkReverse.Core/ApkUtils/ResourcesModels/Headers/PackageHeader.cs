namespace ApkReverse.Core.ApkUtils.ResourcesModels.Headers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public struct PackageHeader
    {
        public BaseHeader Header;

        public uint Id;

        public string Name;//128 with null terminated

        public uint TypeStrings;

        public uint LastPublicType;

        public uint KeyStrings;

        public uint LastPublicKey;

        public uint TypeIdOffset;

        public PackageHeader(BinaryReader reader)
        {
            this.Header = new BaseHeader(reader);

            if(this.Header.Type != BaseHeader.ChunkType.ResTablePackageType)
                throw new ResourceParsingException(
                    string.Format(
                        CoreResources.ChunkTypeError,
                        this.Header.Type,
                        BaseHeader.ChunkType.ResTablePackageType,
                        reader.BaseStream.Position - 1));

            if (this.Header.HeaderSize != 0x0120)
                throw new ResourceParsingException(
                    string.Format(
                        CoreResources.ChunkHeaderSizeError,
                        this.Header.HeaderSize,
                        0x011c));

            this.Id = reader.ReadUInt32();

            var name = new List<ushort>();
            for (var i = 0; i < 128; i++)
            {
                var tmp = reader.ReadUInt16();
                if (tmp == 0u)
                    break;

                name.Add(tmp);
            }
            this.Name = new string(name.ConvertAll(Convert.ToChar).ToArray());
            if(name.Count != 128)
                reader.BaseStream.Seek((128 - name.Count - 1) * sizeof(ushort), SeekOrigin.Current);//Skip other name bytes

            this.TypeStrings = reader.ReadUInt32();
            this.LastPublicType = reader.ReadUInt32();
            this.KeyStrings = reader.ReadUInt32();
            this.LastPublicKey = reader.ReadUInt32();
            this.TypeIdOffset = reader.ReadUInt32();
        }
    }
}