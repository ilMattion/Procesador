using Procesador.Entities;
using Procesador.Enums;
using Procesador.Interfaces;
using Procesador.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

public class ProcessRepository : Repository, IProcessRepository
{
    private readonly IBlobRepository blobRepository;

    public ProcessRepository(ProcesadorContext procesadorContext, IBlobRepository blobRepository) : base(procesadorContext)
    {
        this.blobRepository = blobRepository ?? throw new ArgumentNullException(nameof(blobRepository));
    }

    public List<Process> GetAll()
    {
        return procesadorContext
            .Processes
            .ToList();
    }
    public Process Get(Guid processIdentifier)
    {
        if (processIdentifier == Guid.Empty)
        {
            throw new ArgumentException("The parameter cannot be empty.", nameof(processIdentifier));
        }

        return procesadorContext.Processes.FirstOrDefault(process => process.Id == processIdentifier);
    }

    public Guid Create()
    {
        Guid processIdentifier = Guid.NewGuid();

        procesadorContext.Add(new Process()
        {
            Id = processIdentifier,
            Status = ProcessStatus.Created,
            Blob = null
        });

        procesadorContext.SaveChanges();

        return processIdentifier;
    }

    public void AssignBlob(Guid processIdentifier, int blobIdentifier)
    {
        if (processIdentifier == Guid.Empty)
        {
            throw new ArgumentException("The parameter cannot be empty.", nameof(processIdentifier));
        }

        if (blobIdentifier < 1)
        {
            throw new ArgumentException("The parameter is not a valid identifier.", nameof(blobIdentifier));
        }

        Process currentProcess = procesadorContext.Processes.FirstOrDefault(process => process.Id == processIdentifier);

        if (currentProcess == null)
        {
            throw new Exception($"Cannot find process entity with identifier {processIdentifier}.");
        }

        currentProcess.Blob = blobRepository.Get(blobIdentifier);
        procesadorContext.SaveChanges();
    }

    public void UpdateStatus(Guid processIdentifier, ProcessStatus processStatus)
    {
        if (processIdentifier == Guid.Empty)
        {
            throw new ArgumentException("The parameter cannot be empty.", nameof(processIdentifier));
        }

        Process currentProcess = procesadorContext.Processes.FirstOrDefault(process => process.Id == processIdentifier);

        if (currentProcess == null)
        {
            throw new Exception($"Cannot find process entity with identifier {processIdentifier}.");
        }

        currentProcess.Status = processStatus;
        procesadorContext.SaveChanges();
    }

}