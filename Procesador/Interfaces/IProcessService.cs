using Procesador.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Procesador.Interfaces
{
    public interface IProcessService
    {
        List<Process> GetAll();

        Guid Create(Stream stream);


    }
}
