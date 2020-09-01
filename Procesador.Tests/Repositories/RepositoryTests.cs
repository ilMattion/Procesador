using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Procesador.Entities;

namespace Procesador.Tests.Repositories
{
    public abstract class RepositoryTests : Tests
    {
        protected DbContextOptionsBuilder<ProcesadorContext> dbContextOptionsBuilder;
        protected ProcesadorContext ProcesadorContext;

        public RepositoryTests()
        {
            dbContextOptionsBuilder = new DbContextOptionsBuilder<ProcesadorContext>()
                .UseInMemoryDatabase("ProcesadorInMemoryDatabase");
        }

        [TestInitialize]
        public virtual void Initialize()
        {
            ProcesadorContext = new ProcesadorContext(dbContextOptionsBuilder.Options);
        }
    }
}
