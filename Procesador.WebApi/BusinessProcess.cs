using Procesador.Interfaces;
using System;
using System.IO;

public class BusinessProcess : IProcess
{
    private readonly IBlobService blobService;

    public BusinessProcess(IBlobService blobService)
    {
        this.blobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
    }

    public Stream Process(Guid processIdentifier)
    {
        // Download your blob
        Stream currentBlob = blobService.Download(processIdentifier);

        // Process it
        MemoryStream memoryStream = new MemoryStream();
        currentBlob.CopyTo(memoryStream);
        memoryStream.Position = 0;

        StreamWriter writer = new StreamWriter(memoryStream);
        writer.Write($"Processed document at ${DateTime.Now} by Procesador.");

        return memoryStream;
    }
}