using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Procesador.Entities;
using Procesador.Enums;
using Procesador.Interfaces;
using Procesador.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace Procesador.Tests.Services
{
    [TestClass]
    public class ProcessServiceTests
    {
        private Fixture fixture;
        private Mock<IProcess> process;
        private Mock<IProcessRepository> processRepository;
        private Mock<IBlobService> blobService;
        private ProcessService sut;

        [TestInitialize]
        public void Initialize()
        {
            fixture = new Fixture();
            process = new Mock<IProcess>();
            processRepository = new Mock<IProcessRepository>();
            blobService = new Mock<IBlobService>();
            sut = new ProcessService(process.Object, blobService.Object, processRepository.Object);
        }

        [TestMethod]
        public void Ctor_NoThrowException()
        {
            // Arrange
            // Act
            // Assert
            new ProcessService(process.Object, blobService.Object, processRepository.Object);
        }


        [TestMethod]
        public void GetAll_RepositoryReturnEmptyList_ReturnEmptyList()
        {
            // Arrange
            processRepository
                .Setup(repo => repo.GetAll())
                .Returns(new List<Process>());

            // Act
            var result = sut.GetAll();

            // Assert
            Assert.AreEqual(result.Count, 0);
        }

        [TestMethod]
        public void GetAll_RepositoryReturnSomething_ReturnRepositoryData()
        {
            // Arrange
            var expected = fixture.Create<List<Process>>();

            processRepository
                .Setup(repo => repo.GetAll())
                .Returns(expected);

            // Act
            var result = sut.GetAll();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ExecuteProcess_CallBlobServiceInsert()
        {
            // Arrange
            Stream stream = new MemoryStream();

            // Act
            sut.Create(stream);

            // Assert
            blobService
                .Verify(service => service.Load(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ExecuteProcess_CallProcessRepositoryNewProcess()
        {
            // Arrange
            Stream stream = new MemoryStream();

            // Act
            sut.Create(stream);

            // Assert
            processRepository
                .Verify(repo => repo.Create(), Times.Once());
        }

        [TestMethod]
        public void ExecuteProcess_ParameterStreamNull_ThrowArgumentNullException()
        {
            // Arrange
            // Act
            Action testAction = () => sut.Create(null);

            // Assert
            Assert.ThrowsException<ArgumentNullException>(testAction);
        }

        [TestMethod]
        public void ExecuteProcess_ProcessThrowException_NotThrowException()
        {
            // Arrange
            Stream stream = new MemoryStream();

            process
                .Setup(process => process.Process(It.IsAny<Guid>()))
                .Throws(new Exception());

            // Act
            sut.Create(stream);

            // Assert
        }

        [TestMethod]
        public void ExecuteProcess_ProcessThrowException_CallProcessRepositoryUpdateStatus()
        {
            // Arrange
            Stream stream = new MemoryStream();

            process
                .Setup(process => process.Process(It.IsAny<Guid>()))
                .Throws(new Exception());

            // Act
            sut.Create(stream);

            // Assert
            processRepository
                .Verify(repo => repo.UpdateStatus(It.IsAny<Guid>(), ProcessStatus.Failed), Times.Once());
        }

        [TestMethod]
        public void ExecuteProcess_ProcessExecuteCorrectly_CallProcessRepositoryUpdateStatus()
        {
            // Arrange
            Stream stream = new MemoryStream();

            process
                .Setup(process => process.Process(It.IsAny<Guid>()));

            // Act
            sut.Create(stream);

            // Assert
            processRepository
                .Verify(repo => repo.UpdateStatus(It.IsAny<Guid>(), ProcessStatus.Succeded), Times.Once());
        }

        [TestMethod]
        public void ExecuteProcess_ProcessExecuteCorrectly_ReturnProcessGuid()
        {
            // Arrange
            Guid expected = Guid.NewGuid();
            Stream stream = new MemoryStream();

            processRepository
                .Setup(process => process.Create())
                .Returns(expected);

            process
                .Setup(process => process.Process(It.IsAny<Guid>()));

            // Act
            Guid guid = sut.Create(stream);

            // Assert
            Assert.AreEqual(expected, guid);
        }
    }
}
