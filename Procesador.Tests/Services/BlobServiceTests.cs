using System;
using System.IO;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Procesador.Interfaces;

namespace Procesador.Tests.Services
{
    [TestClass]
    public class BlobServiceTests
    {
        private Fixture fixture;
        private Mock<IProcessRepository> processRepository;
        private Mock<IBlobRepository> blobRepository;
        private BlobService sut;

        [TestInitialize]
        public void Initialize()
        {
            fixture = new Fixture();
            processRepository = new Mock<IProcessRepository>();
            blobRepository = new Mock<IBlobRepository>();
            sut = new BlobService(null, processRepository.Object, blobRepository.Object);
        }

        [TestMethod]
        public void Load_ParameterStreamNull_ThrowArgumentNullException()
        {
            // Arrange
            string fileName = fixture.Create<string>();

            // Act
            Action testAction = () => sut.Load(null, fileName);

            // Assert
            Assert.ThrowsException<ArgumentNullException>(testAction);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void Load_ParameterBlobNameNullOrEmpty_ThrowArgumentException(string blobName)
        {
            // Arrange
            Stream stream = new MemoryStream();

            // Act
            Action testCode = () => sut.Load(stream, blobName);

            // Assert
            Assert.ThrowsException<ArgumentException>(testCode);
        }
    }
}