namespace ApkReverse.Core.Sevices
{
    /// <summary>
    /// Предоставлет метод для преобразования AndroidManifest.xml из бинарного вида в текстровый
    /// </summary>
    public interface IAndroidManifestManager
    {
        /// <summary>
        /// Десераилизует бинарный файл и перезаписывает его текстовой реализацией
        /// </summary>
        /// <param name="binaryManifestPath"></param>
        void Deserialize(string binaryManifestPath);
    }
}