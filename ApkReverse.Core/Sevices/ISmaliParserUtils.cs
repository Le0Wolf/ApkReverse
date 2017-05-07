namespace ApkReverse.Core.Sevices
{
    using System.IO;

    internal interface ISmaliParserUtils
    {
        /// <summary>
        /// Проверяет, содержится ли в путях проекта указанный неймспейс
        /// </summary>
        /// <param name="namespace"></param>
        /// <returns></returns>
        bool ContainsNamespace(string @namespace);

        /// <summary>
        /// ПРоверяет, содержится ли в путях проекта указанный неймспейс и указанный файл в нем
        /// </summary>
        /// <param name="namespace">Неймспейс. Имя файла может быть задано либо здесь, либо в следуюющем параметре</param>
        /// <param name="fileName">имя файла в указнном неймспейсе</param>
        /// <param name="fileWthExtension">Признак того, что имя файла уже содержит расшиирение, если false то к пути добавляется расширение по умолчанию</param>
        /// <returns></returns>
        bool NamespaceContainsFile(string @namespace, string fileName, bool fileWthExtension = true);

        /// <summary>
        /// Открывает указанный файл только для чтения. Если файл не существует, 
        /// к нему отсутствует доступ илии попытаться записать в проект источник то бросает исключение.
        /// </summary>
        /// <param name="namespace"></param>
        /// <param name="file"></param>
        /// <param name="fileWthExtension"></param>
        /// <returns></returns>
        Stream GetProjectFileStream(string @namespace, string file, bool fileWthExtension = true);

        /// <summary>
        /// Преобразует неймспейс и имя файла в путь в файловой системе
        /// </summary>
        /// <param name="namespace"></param>
        /// <param name="file"></param>
        /// <param name="fileWthExtension"></param>
        /// <returns></returns>
        string NamespaceToPath(string @namespace, string file = null, bool fileWthExtension = true);
    }
}