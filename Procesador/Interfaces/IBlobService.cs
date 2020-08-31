using System;
using System.IO;

namespace Procesador.Interfaces
{
    public interface IBlobService
    {
        Stream Download(Guid processIdentifier);
        int Load(Stream stream, string blobName);
        void AssingBlobProcessed(int blobIdentifier, Stream processed);
    }
}
