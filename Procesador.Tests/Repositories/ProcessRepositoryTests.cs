using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Procesador.Entities;
using Procesador.Interfaces;
using System;
using System.Collections.Generic;

namespace Procesador.Tests.Repositories
{
    [TestClass]
    public class ProcessRepositoryTests : RepositoryTests
    {
        public Mock<IBlobRepository> blobRepository;
        public ProcessRepository sut;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            blobRepository = new Mock<IBlobRepository>();
            sut = new ProcessRepository(ProcesadorContext, blobRepository.Object);
        }

        [TestMethod]
        public void GetAll_ContextHasNotProcess_ReturnEmptyList()
        {
            // Arrange
            var expected = new List<Process>();

            // Act
            var result = sut.GetAll();

            // Assert
            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestMethod]
        public void GetAll_HasProcess_ReturnFilledList()
        {
            // Arrange
            var expected = fixture.Create<List<Process>>();

            ProcesadorContext.AddRange(expected);
            ProcesadorContext.SaveChanges();

            // Exercise
            var result = sut.GetAll();

            // Assert
            CollectionAssert.AreEquivalent(expected, result);
        }


        [TestMethod]
        public void GetById_ParameterProcessIdentifierIsEmpty_ThrowArgumentException()
        {
            // Arrange
            Guid processIdentifier = Guid.Empty;

            // Act
            Action testAction = () => sut.Get(processIdentifier);

            // Assert
            var exception = Assert.ThrowsException<ArgumentException>(testAction);
            Assert.AreEqual(exception.ParamName, nameof(processIdentifier));
        }

        [TestMethod]
        public void GetById_ParameterProcessIdentifierNotExistent_ReturnNull()
        {
            // Arrange
            Guid processIdentifier = Guid.NewGuid();

            // Exercise
            var result = sut.Get(processIdentifier);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetById_ParameterProcessIdentifierExistent_ReturnRelatedProcess()
        {
            // Arrange
            var expected = fixture.Create<Process>();

            ProcesadorContext.Add(expected);
            ProcesadorContext.SaveChanges();

            // Act
            var result = sut.Get(expected.Id);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
