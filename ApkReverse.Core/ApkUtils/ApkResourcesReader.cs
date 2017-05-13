namespace ApkReverse.Core.ApkUtils
{
    using System.IO;

    using ResourcesModels;
    using Sevices;

    /// <summary>
    /// Class for decode all apk resources (Resources.arsc and all encoded files in res folder)
    /// </summary>
    public class ApkResourcesReader/* : IApkResourcesService*/
    {
        /// <summary>
        /// ref to Display printer object. Optional initialized from Castle Windsor
        /// </summary>
        public IDisplayService DisplayService;

        private readonly BinaryReader _reader;

        /// <summary>
        /// List of all strings from string pool, used from packages
        /// </summary>
        private string[] _stringsPool;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resFileStream"></param>
        /// <param name="resDir"></param>
        public ApkResourcesReader(Stream resFileStream, string resDir)
        {
            this._reader = new BinaryReader(resFileStream);
        }

        /// <summary>
        /// Read all content of binary resource file (Resources.arsc)
        /// </summary>
        public void ReadResourceTable()
        {
            var header = new TableHeader(this._reader);

            this.DisplayService.WriteMsg(header.ToString());

            var baseOffset = this._reader.BaseStream.Position;
            baseOffset += this.InitStringPool();

            for (var i = 0; i < header.PackageCount; i++)
            {
                this._reader.BaseStream.Seek(baseOffset, SeekOrigin.Begin);
                baseOffset += this.ReadPackage();
            }
        }

        /// <summary>
        /// Fill string pool array from binary resource file
        /// </summary>
        /// <returns></returns>
        private uint InitStringPool()
        {
            uint baseOffset = 12/*this._reader.BaseStream.Position*/; //12 - is table header size

            var header = new StringPoolHeader(this._reader);

            baseOffset += header.StringsStart;

            if (header.StringCount > 0)
            {
                this._stringsPool = new string[header.StringCount];
                for (var i = 0; i < header.StringCount; i++)
                {
                    var relativeOffset = this._reader.ReadUInt32();

                    var currentOffset = this._reader.BaseStream.Position;//Save offset before read string

                    this._reader.BaseStream.Seek(relativeOffset + baseOffset, SeekOrigin.Begin);
                    this._stringsPool[i] = header.Flags.HasFlag(StringPoolHeader.FlagType.Utf8Flag)
                                               ? this.ReadUtf8String()
                                               : this.ReadUtf16String();

                    this._reader.BaseStream.Seek(currentOffset, SeekOrigin.Begin);//Restore offset after read string
                }
            }
            //if (header.StyleCount > 0)
            //{
            //    for (var i = 0; i < header.StyleCount; i++)
            //    {
            //        var offset = this._reader.ReadUInt32();
            //    }
            //}
            return header.Header.Size;
        }

        private uint ReadPackage()
        {
            return 0;
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
            if ((lengths[0] & 0x80) == 0)
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
                throw new ResourceParsingException($"Resources parse error: unexpected {endChr}, expecting 'NULL' at offset {this._reader.BaseStream.Position - 1}");
            return System.Text.Encoding.UTF8.GetString(byteArray);
        }

        private string ReadUtf16String()
        {
            throw new ResourceParsingException("Resources parse error: Currently utf-16 strings is not supported");

            var len = this._reader.ReadUInt16();
            var charArray = this._reader.ReadChars(len);
            var endChr = this._reader.ReadUInt16();
            if (len < short.MaxValue && endChr != 0)
                throw new ResourceParsingException($"Строка завершилась символом {endChr} Ожидалось 0");
            //var shortArray = new ushort[len];
            //for (var i = 0; i < len; i++)
            //{
            //    shortArray[i] = this._reader.ReadUInt16();
            //}
            //return new string(Array.ConvertAll(shortArray, Convert.ToChar));// System.Text.Encoding.Unicode.GetString(byteArray);
            return new string(charArray);
        }
    }
}
