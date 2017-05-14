namespace ApkReverse.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;

    using ApkUtils;

    using JetBrains.Annotations;

    using Sevices;

    internal class Project : IProject
    {
        private readonly string _apkPath;

        private readonly string _projectPath;

        private readonly IAndroidManifestManager _androidManifestManager;

        [CanBeNull]
        public IDisplayService DisplayService;//Castle Windsor initialization

        public string ResourceFilePath { get; private set; }

        public string SourcesFilePath { get; private set; }

        public string ManifestFilePath { get; private set; }

        public Dictionary<string, string> ResourcesTable { get; private set; }

        public string ResourcesSubDir { get; set; } = "res";

        public string SmaliFilesSubDir { get; set; } = "smali";

        public string OriginalFilesSubDir { get; set; } = "original";

        public string ResourceFileExt { get; set; } = "arsc";

        public string SourceFileExt { get; set; } = "dex";

        public string ResourceFileName { get; set; } = "AndroidManifest.xml";

        public Project(IAndroidManifestManager androidManifestManager, string apkPath, string projectPath)
        {
            this._apkPath = apkPath;
            this._projectPath = projectPath;
            this._androidManifestManager = androidManifestManager;
        }

        /// <summary>
        /// Распаковывает ak файл как архив
        /// </summary>
        public void Extract()
        {
            if (Directory.Exists(this._projectPath))
            {
                this.DisplayService?.WriteMsg($"Destantion folder exists, removing: {this._projectPath}");
                Directory.Delete(this._projectPath, true);
            }

            this.DisplayService?.WriteMsg($"Unpack apk: {this._apkPath}");

            ZipFile.ExtractToDirectory(this._apkPath, this._projectPath);

            var dirInfo =new DirectoryInfo(this._projectPath);
            foreach (var fileSystemInfo in dirInfo.EnumerateFileSystemInfos())
            {
                if (fileSystemInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    if (fileSystemInfo.Name == this.ResourcesSubDir) continue;

                    this.DisplayService?.WriteMsg(
                        $"Move {fileSystemInfo.Name} to {Path.Combine(this.OriginalFilesSubDir, fileSystemInfo.Name)}");

                    Directory.Move(fileSystemInfo.FullName, Path.Combine(this._projectPath, this.OriginalFilesSubDir, fileSystemInfo.Name));
                }
                else
                {
                    if (fileSystemInfo.Extension == this.ResourceFileExt)
                    {
                        if (!string.IsNullOrEmpty(this.ResourceFilePath)) continue;

                        this.ResourceFilePath = fileSystemInfo.FullName;
                        this.DisplayService?.WriteMsg($"Found resource file: {fileSystemInfo.FullName}");
                    }
                    else if (fileSystemInfo.Extension == this.SourceFileExt)
                    {
                        if(!string.IsNullOrEmpty(this.SourcesFilePath)) continue;

                        this.SourcesFilePath = fileSystemInfo.FullName;
                        this.DisplayService?.WriteMsg($"Found dex file: {fileSystemInfo.FullName}");
                    }
                    else if (fileSystemInfo.Name == "AndroidManifest.xml")
                    {
                        if (!string.IsNullOrEmpty(this.ManifestFilePath)) continue;

                        this.ManifestFilePath = fileSystemInfo.FullName;
                        this.DisplayService?.WriteMsg($"Found android manifest: {fileSystemInfo.FullName}");
                    }
                    else
                    {
                        this.DisplayService?.WriteMsg($"Move {fileSystemInfo.Name} to dir {this.OriginalFilesSubDir}");

                        File.Move(
                            fileSystemInfo.FullName,
                            Path.Combine(this._projectPath, this.OriginalFilesSubDir, fileSystemInfo.Name));

                    }
                }
            }
        }

        public void ExtractAll()
        {
            this.Extract();

            if (!string.IsNullOrEmpty(this.ResourceFilePath))
                this.ExtractResources(this.ResourceFilePath);

            if(!string.IsNullOrEmpty(this.ManifestFilePath))
                this.DecompileManifest(this.ManifestFilePath);

            if (!string.IsNullOrEmpty(this.SourcesFilePath))
            {
                this.ExtractSources(this.SourcesFilePath);
                this.ConvertSources(Path.Combine(this._projectPath, this.SmaliFilesSubDir), this.ResourcesTable);
            }
        }

        /// <summary>
        /// извлекает ресурсы из arsc файла, десериализует файлы в папке с ресурсами
        /// </summary>
        /// <param name="resFilePath"></param>
        public void ExtractResources(string resFilePath)
        {
            //this.ResourcesTable = ApkResourcesManager.Extact(resFilePath, Path.Combine(this._projectPath, this.ResourcesSubDir));
        }

        /// <summary>
        /// Извлекает smali файлы из dex файла
        /// </summary>
        /// <param name="sourcesFile"></param>
        public void ExtractSources(string sourcesFile)
        {
            Directory.CreateDirectory(Path.Combine(this._projectPath, this.SmaliFilesSubDir));
        }

        /// <summary>
        /// Конвертирует smali файлы в java, испольует отладочную информицию, чтобы дать корректные имена файлам
        /// </summary>
        /// <param name="smaliSourcesPath"></param>
        /// <param name="resources"></param>
        public void ConvertSources(string smaliSourcesPath, Dictionary<string, string> resources)
        {
            Directory.CreateDirectory(Path.Combine(this._projectPath, "java"));
        }

        public void DecompileManifest(string manifestFile)
        {
            if(manifestFile == null) return;

            File.Copy(manifestFile, Path.Combine(this._projectPath, this.OriginalFilesSubDir, Path.GetFileName(manifestFile)), true);

            this._androidManifestManager.Deserialize(manifestFile);
        }

        public void GenerateProject()
        {
            
        }

    }
}
