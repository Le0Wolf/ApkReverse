namespace ApkReverse.Core.Sevices
{
    using System.Collections.Generic;

    public interface IProject
    {
        string ResourceFilePath { get; }

        string SourcesFilePath { get; }

        string ManifestFilePath { get; }

        Dictionary<string, string> ResourcesTable { get; }

        string ResourcesSubDir { get; set; }

        string SmaliFilesSubDir { get; set; }

        string OriginalFilesSubDir { get; set; }

        string ResourceFileExt { get; set; }

        string SourceFileExt { get; set; }

        string ResourceFileName { get; set; }

        /// <summary>
        /// Распаковывает ak файл как архив
        /// </summary>
        void Extract();

        void ExtractAll();

        /// <summary>
        /// извлекает ресурсы из arsc файла, десериализует файлы в папке с ресурсами
        /// </summary>
        /// <param name="resFilePath"></param>
        void ExtractResources(string resFilePath);

        /// <summary>
        /// Извлекает smali файлы из dex файла
        /// </summary>
        /// <param name="sourcesFile"></param>
        void ExtractSources(string sourcesFile);

        /// <summary>
        /// Конвертирует smali файлы в java, испольует отладочную информицию, чтобы дать корректные имена файлам
        /// </summary>
        /// <param name="smaliSourcesPath"></param>
        /// <param name="resources"></param>
        void ConvertSources(string smaliSourcesPath, Dictionary<string, string> resources);

        void DecompileManifest(string manifestFile);

        void GenerateProject();
    }
}