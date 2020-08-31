using Procesador.Entities;
using Procesador.Enums;
using Procesador.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace Procesador.Services
{
    public class ProcessService : IProcessService
    {
        private readonly IProcess process;
        private readonly IBlobService blobService;
        private readonly IProcessRepository processRepository;

        public ProcessService(IProcess process, IBlobService blobService, IProcessRepository processRepository)
        {
            this.process = process ?? throw new ArgumentNullException(nameof(process));
            this.blobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
            this.processRepository = processRepository ?? throw new ArgumentNullException(nameof(processRepository));
        }

        public List<Process> GetAll()
        {
            return processRepository.GetAll();
        }

        public Guid Create(Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            Guid processIdentifier = processRepository.Create();
            int blobIdentifier = blobService.Load(stream, processIdentifier.ToString());

            processRepository.AssignBlob(processIdentifier, blobIdentifier);

            ProcessStatus processStatus = ProcessStatus.Started;
            processRepository.UpdateStatus(processIdentifier, processStatus);
            
            try
            {
                Stream processed = process.Process(processIdentifier);

                blobService.AssingBlobProcessed(blobIdentifier, processed);

                processStatus = ProcessStatus.Succeded;
            }
            catch (Exception ex)
            {
                processStatus = ProcessStatus.Failed;
            }
            finally
            {
                processRepository.UpdateStatus(processIdentifier, processStatus);
            }

            return processIdentifier;
        }

    }
}
