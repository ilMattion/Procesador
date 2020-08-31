using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Procesador.Entities;
using Procesador.Interfaces;

namespace Procesador.Tests.Repositories
{
    [TestClass]
    public class ProcessRepositoryTests
    {
        public Mock<ProcesadorContext> procesadorContext;
        public Mock<IBlobRepository> blobRepository;
        public ProcessRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            procesadorContext = new Mock<ProcesadorContext>();
            blobRepository = new Mock<IBlobRepository>();
            sut = new ProcessRepository(procesadorContext.Object, blobRepository.Object);
        }

        [TestMethod]
        public void GetAll_NotThrowException()
        {
            // Arrange
            // Act
            sut.GetAll();
            // Assert
        }
    }
}
