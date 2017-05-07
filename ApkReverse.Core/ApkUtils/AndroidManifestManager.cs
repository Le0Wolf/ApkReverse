namespace ApkReverse.Core.ApkUtils
{
    using System;

    using ApkReverse.Core.Sevices;

    class AndroidManifestManager : IAndroidManifestManager
    {
        public IDisplayService DisplayService;

        public void Deserialize(string binaryManifestPath)
        {
            throw new NotImplementedException();
        }
    }
}
