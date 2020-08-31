using Procesador.Enums;
using System;

namespace Procesador.Entities
{
    public class Process
    {
        public Guid Id { get; set; }

        public ProcessStatus Status { get; set; }

        public Blob Blob { get; set; }
    }
}
