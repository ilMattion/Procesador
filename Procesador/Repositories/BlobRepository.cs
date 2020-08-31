using Procesador.Entities;
using Procesador.Interfaces;
using System;
using System.Linq;

namespace Procesador.Repositories
{
    public class BlobRepository : Repository, IBlobRepository
    {
        public BlobRepository(ProcesadorContext procesadorContext) : base(procesadorContext)
        {
        }

        public Blob Get(int blobIdentifier)
        {
            if (blobIdentifier < 1)
            {
                throw new ArgumentException("The parameter is not a valid identifier.", nameof(blobIdentifier));
            }

            return procesadorContext.Blobs.FirstOrDefault(blob => blob.Id == blobIdentifier);
        }

        public Blob Get(Guid processIdentifier)
        {


            return procesadorContext
                .Processes
                .FirstOrDefault(process => process.Id == processIdentifier)
                .Blob;
        }

        public int Create(string blobName, Uri uri)
        {
            if (string.IsNullOrEmpty(blobName))
            {
                throw new ArgumentException($"'{nameof(blobName)}' cannot be null o empty.", nameof(blobName));
            }

            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            var currentBlob = new Blob()
            {
                FileName = blobName,
                Path = uri.ToString(),
                ProcessedBlobPath = null
            };

            procesadorContext.Blobs.Add(currentBlob);
            procesadorContext.SaveChanges();

            return currentBlob.Id;
        }

        public void AssingBlobProcessed(int blobIdentifier, Uri uri)
        {
            if (blobIdentifier < 1)
            {
                throw new ArgumentException("The parameter is not a valid identifier.", nameof(blobIdentifier));
            }

            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            Blob currentBlob = procesadorContext.Blobs.FirstOrDefault(blob => blob.Id == blobIdentifier);
            currentBlob.ProcessedBlobPath = uri.ToString();

            procesadorContext.SaveChanges();
        }
    }
}
