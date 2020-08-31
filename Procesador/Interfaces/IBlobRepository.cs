using Procesador.Entities;
using System;

namespace Procesador.Interfaces
{
    public interface IBlobRepository
    {
        Blob Get(int blobIdentifier);

        Blob Get(Guid processIdentifier);

        int Create(string blobName, Uri uri);
        void AssingBlobProcessed(int blobIdentifier, Uri uri);
    }
}
