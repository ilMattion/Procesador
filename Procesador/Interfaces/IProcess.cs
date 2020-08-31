using System;
using System.IO;

namespace Procesador.Interfaces
{
    public interface IProcess
    {
        Stream Process(Guid processIdentifier);
    }
}
