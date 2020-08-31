
using System.IO;
using System;
using Procesador.Interfaces;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Azure;
using Azure.Storage.Blobs.Models;
using Procesador.Entities;

public class BlobService : IBlobService
{
    private readonly IOptions<BlobStorageOptions> blobStorageOptions;
    private readonly IProcessRepository processRepository;
    private readonly IBlobRepository blobRepository;

    public BlobService(IOptions<BlobStorageOptions> blobStorageOptions, IProcessRepository processRepository, IBlobRepository blobRepository)
    {
        this.blobStorageOptions = blobStorageOptions;
        this.processRepository = processRepository ?? throw new ArgumentNullException(nameof(processRepository));
        this.blobRepository = blobRepository ?? throw new ArgumentNullException(nameof(blobRepository));
    }

    public Stream Download(Guid processIdentifier)
    {
        if (processIdentifier == Guid.Empty)
        {
            throw new ArgumentException("The parameter cannot be empty.", nameof(processIdentifier));
        }

        Process currentProcess = processRepository.Get(processIdentifier);

        if (currentProcess == null)
        {
            throw new Exception($"Cannot found process with identifier {processIdentifier}");
        }
        if (currentProcess.Blob == null)
        {
            throw new Exception($"Current blob have not associated blob.");
        }

        Blob currentBlob = blobRepository.Get(currentProcess.Id);

        BlobContainerClient blobContainerClient = new BlobContainerClient(blobStorageOptions.Value.ConnectionString, blobStorageOptions.Value.BlobContainerName);
        blobContainerClient.CreateIfNotExists();

        BlobClient blobClient = new BlobClient(blobStorageOptions.Value.ConnectionString, blobStorageOptions.Value.BlobContainerName, currentBlob.FileName);
        return blobClient.Download().Value.Content;
    }

    public int Load(Stream stream, string blobName)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }
        if (string.IsNullOrEmpty(blobName))
        {
            throw new ArgumentException("The parameter cannot be null or empty.", nameof(blobName));
        }

        BlobContainerClient blobContainerClient = new BlobContainerClient(blobStorageOptions.Value.ConnectionString, blobStorageOptions.Value.BlobContainerName);
        blobContainerClient.CreateIfNotExists();

        BlobClient blobClient = new BlobClient(blobStorageOptions.Value.ConnectionString, blobStorageOptions.Value.BlobContainerName, blobName);
        blobClient.Upload(stream);

        return blobRepository.Create(blobName, blobClient.Uri);
    }

    public void AssingBlobProcessed(int blobIdentifier, Stream processed)
    {
        if (blobIdentifier < 1)
        {
            throw new ArgumentException("The paramter is not a valid identifier.", nameof(blobIdentifier));
        }

        if (processed is null)
        {
            throw new ArgumentNullException(nameof(processed));
        }

        Blob currentBlob = blobRepository.Get(blobIdentifier);

        BlobContainerClient blobContainerClient = new BlobContainerClient(blobStorageOptions.Value.ConnectionString, blobStorageOptions.Value.BlobContainerName);
        blobContainerClient.CreateIfNotExists();

        BlobClient blobClient = new BlobClient(blobStorageOptions.Value.ConnectionString, blobStorageOptions.Value.BlobContainerName, $"{currentBlob.FileName}.processed");
        blobClient.Upload(processed);

        blobRepository.AssingBlobProcessed(blobIdentifier, blobClient.Uri);

    }
}