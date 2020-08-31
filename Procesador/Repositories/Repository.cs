using Procesador.Entities;
using System;

namespace Procesador.Repositories
{
    public abstract class Repository
    {
        protected readonly ProcesadorContext procesadorContext;

        public Repository(ProcesadorContext procesadorContext)
        {
            this.procesadorContext = procesadorContext ?? throw new ArgumentNullException(nameof(procesadorContext));
        }
    }
}
