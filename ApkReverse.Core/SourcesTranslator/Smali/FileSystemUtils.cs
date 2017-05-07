namespace ApkReverse.Core.SourcesTranslator.Smali
{
    using System.IO;

    using ApkReverse.Core.Sevices;

    class FileSystemUtils : ISmaliParserUtils
    {
        private readonly string _projectRoot;

        private char _namespaceSeparator;

        private FileMode _streamMode;

        private FileAccess _streamAccess;

        public FileSystemUtils(string projectRoot, bool isSourceProject = true, char namespaceSeparator = '.')
        {
            this._projectRoot = projectRoot;
            this._streamAccess = isSourceProject ? FileAccess.Read : FileAccess.Write;
            this._streamMode = isSourceProject ? FileMode.Open : FileMode.OpenOrCreate;
            this._namespaceSeparator = namespaceSeparator;
        }

        /// <summary>
        /// Проверяет, содержится ли в путях проекта указанный неймспейс
        /// </summary>
        /// <param name="namespace"></param>
        /// <returns></returns>
        public bool ContainsNamespace(string @namespace)
        {
            return Directory.Exists(this.NamespaceToPath(@namespace));
        }

        /// <summary>
        /// ПРоверяет, содержится ли в путях проекта указанный неймспейс и указанный файл в нем
        /// </summary>
        /// <param name="namespace">Неймспейс. Имя файла может быть задано либо здесь, либо в следуюющем параметре</param>
        /// <param name="fileName">имя файла в указнном неймспейсе</param>
        /// <param name="fileWthExtension">Признак того, что имя файла уже содержит расшиирение, если false то к пути добавляется расширение по умолчанию</param>
        /// <returns></returns>
        public bool NamespaceContainsFile(string @namespace, string fileName, bool fileWthExtension = true)
        {
            return File.Exists(this.NamespaceToPath(@namespace, fileName, fileWthExtension));
        }

        /// <summary>
        /// Открывает указанный файл только для чтения. Если файл не существует, 
        /// к нему отсутствует доступ илии попытаться записать в проект источник то бросает исключение.
        /// </summary>
        /// <param name="namespace"></param>
        /// <param name="file"></param>
        /// <param name="fileWthExtension"></param>
        /// <returns></returns>
        public Stream GetProjectFileStream(string @namespace, string file, bool fileWthExtension = true)
        {
            var path = this.NamespaceToPath(@namespace, file, fileWthExtension);
            return new FileStream(path, this._streamMode, this._streamAccess);
        }

        /// <summary>
        /// Преобразует неймспейс и имя файла в путь в файловой системе
        /// </summary>
        /// <param name="namespace"></param>
        /// <param name="file"></param>
        /// <param name="fileWthExtension"></param>
        /// <returns></returns>
        public string NamespaceToPath(string @namespace, string file = null, bool fileWthExtension = true)
        {
            if (string.IsNullOrEmpty(@namespace)) return string.Empty;

            var path = this._projectRoot + Path.DirectorySeparatorChar + @namespace.Replace(this._namespaceSeparator, Path.DirectorySeparatorChar);
            if (file != null)
            {
                path += Path.DirectorySeparatorChar + file;
                if (!fileWthExtension) path += ".smali";
            }
            return path;
        }
    }
}
