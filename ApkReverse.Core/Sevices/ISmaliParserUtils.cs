namespace ApkReverse.Core.Sevices
{
    using System.IO;

    internal interface ISmaliParserUtils
    {
        /// <summary>
        /// ���������, ���������� �� � ����� ������� ��������� ���������
        /// </summary>
        /// <param name="namespace"></param>
        /// <returns></returns>
        bool ContainsNamespace(string @namespace);

        /// <summary>
        /// ���������, ���������� �� � ����� ������� ��������� ��������� � ��������� ���� � ���
        /// </summary>
        /// <param name="namespace">���������. ��� ����� ����� ���� ������ ���� �����, ���� � ���������� ���������</param>
        /// <param name="fileName">��� ����� � �������� ����������</param>
        /// <param name="fileWthExtension">������� ����, ��� ��� ����� ��� �������� �����������, ���� false �� � ���� ����������� ���������� �� ���������</param>
        /// <returns></returns>
        bool NamespaceContainsFile(string @namespace, string fileName, bool fileWthExtension = true);

        /// <summary>
        /// ��������� ��������� ���� ������ ��� ������. ���� ���� �� ����������, 
        /// � ���� ����������� ������ ���� ���������� �������� � ������ �������� �� ������� ����������.
        /// </summary>
        /// <param name="namespace"></param>
        /// <param name="file"></param>
        /// <param name="fileWthExtension"></param>
        /// <returns></returns>
        Stream GetProjectFileStream(string @namespace, string file, bool fileWthExtension = true);

        /// <summary>
        /// ����������� ��������� � ��� ����� � ���� � �������� �������
        /// </summary>
        /// <param name="namespace"></param>
        /// <param name="file"></param>
        /// <param name="fileWthExtension"></param>
        /// <returns></returns>
        string NamespaceToPath(string @namespace, string file = null, bool fileWthExtension = true);
    }
}