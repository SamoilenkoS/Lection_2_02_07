using AutoFixture;
using Lection_2_BL.Services.BooksService;
using Lection_2_DAL;
using Lection_2_DAL.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Lection_2_BL.Tests
{
    public class BooksServiceTests
    {
        private Fixture _fixture;
        private IBooksService _booksService;
        private Mock<IGenericRepository<Book>> _genericBookRepositoryMock;
        private Mock<IBooksRepository> _booksRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _genericBookRepositoryMock = new Mock<IGenericRepository<Book>>();
            _booksRepositoryMock = new Mock<IBooksRepository>();
            _booksService = new BooksService(
                _genericBookRepositoryMock.Object,
                _booksRepositoryMock.Object);
        }

        [Test]
        public async Task AddBook_WhenCalled_ShouldCallAddOnRepository()
        {
            var book = _fixture.Create<Book>();
            var expectedGuid = Guid.NewGuid();
            _genericBookRepositoryMock
                .Setup(x => x.Add(book))
                .ReturnsAsync(expectedGuid)
                .Verifiable();

            var actualGuid = await _booksService.AddBook(book);

            Assert.AreEqual(expectedGuid, actualGuid);
            _genericBookRepositoryMock.Verify();
        }
    }
}
