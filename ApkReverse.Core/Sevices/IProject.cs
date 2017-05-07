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
        /// ������������� ak ���� ��� �����
        /// </summary>
        void Extract();

        void ExtractAll();

        /// <summary>
        /// ��������� ������� �� arsc �����, ������������� ����� � ����� � ���������
        /// </summary>
        /// <param name="resFilePath"></param>
        void ExtractResources(string resFilePath);

        /// <summary>
        /// ��������� smali ����� �� dex �����
        /// </summary>
        /// <param name="sourcesFile"></param>
        void ExtractSources(string sourcesFile);

        /// <summary>
        /// ������������ smali ����� � java, ��������� ���������� ����������, ����� ���� ���������� ����� ������
        /// </summary>
        /// <param name="smaliSourcesPath"></param>
        /// <param name="resources"></param>
        void ConvertSources(string smaliSourcesPath, Dictionary<string, string> resources);

        void DecompileManifest(string manifestFile);

        void GenerateProject();
    }
}