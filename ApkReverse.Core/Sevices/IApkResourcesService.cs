namespace ApkReverse.Core.Sevices
{
    using System.Collections.Generic;

    public interface IApkResourcesService
    {
        Dictionary<string, string> Extact(string resFile, string resDir);
    }
}