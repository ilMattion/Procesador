using Procesador.Entities;
using Procesador.Enums;
using System;
using System.Collections.Generic;

namespace Procesador.Interfaces
{
    public interface IProcessRepository
    {
        List<Process> GetAll();
        Process Get(Guid processIdentifier);
        
        Guid Create();

        void AssignBlob(Guid processIdentifier, int blobIdentifier);

        void UpdateStatus(Guid processIdentifier, ProcessStatus processStatus);
    }
}
