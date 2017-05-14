namespace ApkReverse.Core.ApkUtils
{
    using System;
    using System.IO;
    using System.Text;

    using ApkReverse.Core.ApkUtils.ResourcesModels.Headers;

    using JetBrains.Annotations;

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
        [CanBeNull]
        // ReSharper disable once NotAccessedField.Global
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

            var baseOffset = this._reader.BaseStream.Position;

            var tableStringPool = new StringPoolReader(this._reader);

            baseOffset += tableStringPool.Size;

            for (var i = 0; i < header.PackageCount; i++)
            {
                this._reader.BaseStream.Seek(baseOffset, SeekOrigin.Begin);
                baseOffset += this.ReadPackage();
            }
        }

        

        private uint ReadPackage()
        {
            var baseOffset = this._reader.BaseStream.Position;
            var header = new PackageHeader(this._reader);

            var keysStringPool = new StringPoolReader(this._reader, header.KeyStrings + baseOffset);
            var typesStringPool = new StringPoolReader(this._reader, header.TypeStrings + baseOffset);
            return 0;
        }
    }
}
