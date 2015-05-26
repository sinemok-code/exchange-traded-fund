namespace Etf.Test.Service
{
    using System.Web;

    using ETF.Web.Repository.Interfaces;
    using ETF.Web.Service;
    using ETF.Web.Service.Interfaces;
    using ETF.Web.ViewModel;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class FileServiceTests
    {
        private IFileService fileService;
        private Mock<IFileRepository> mockFileRepository;


        [TestInitialize]
        public void Init()
        {
            this.mockFileRepository = new Mock<IFileRepository>();

            this.fileService = new FileService(this.mockFileRepository.Object);
        }
        
        [TestMethod]
        public void UploadFile_MethodCalled_RepoMethodCalled()
        {
            // Arrange
            var viewModel = new UploadFileViewModel();

            this.mockFileRepository.Setup(x => x.UploadFiles(It.IsAny<HttpPostedFileBase>())).Returns(true);

            // Act
            this.fileService.UploadFile(viewModel);
            
            // Assert
            this.mockFileRepository.Verify(x => x.UploadFiles(It.IsAny<HttpPostedFileBase>()), Times.Once);

            Assert.IsTrue(viewModel.Success);
            Assert.IsTrue(viewModel.DisplayMessage);
        }
    }
}
