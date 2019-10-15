using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using VerifyV2Quickstart.Areas.Identity.Pages.Account;
using VerifyV2Quickstart.Models;
using VerifyV2Quickstart.Services;
using Xunit;

namespace VerifyV2Quickstart.Tests.PageModels
{
    public class VerifyTests
    {
        private readonly Mock<IVerification> _verificationService;
        private readonly Mock<IUserStore<ApplicationUser>> _userStore;
        private readonly Mock<ILogger<VerifyModel>> _logger;

        public VerifyTests()
        {
            _userStore = new Mock<IUserStore<ApplicationUser>>();
            _verificationService = new Mock<IVerification>();
            _logger = new Mock<ILogger<VerifyModel>>();
        }

        private UserManager<ApplicationUser> GetUserManager()
        {
            var hasher = new Mock<IPasswordHasher<ApplicationUser>>();

            return new UserManager<ApplicationUser>(_userStore.Object, null, hasher.Object, null,
                null, null, null, null, null);
        }

        [Fact]
        public void OnGetReturnUrlIsAssignedAndPageReturned()
        {
            // Arrange
            var verifyModel = new VerifyModel(GetUserManager(), _verificationService.Object, _logger.Object);
            var context = new Mock<HttpContext>();
            context.Setup(x => x.User).Returns(new Mock<ClaimsPrincipal>().Object);
            verifyModel.PageContext.HttpContext = context.Object;

            // Act
            var result = verifyModel.OnGet("returnUrl");

            // Assert
            Assert.Equal("returnUrl", verifyModel.ReturnUrl);
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPosWithoutLoggedInUserThenRedirectToLogin()
        {
            // Arrange
            _userStore.Setup(
                x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(new ApplicationUser());

            var verifyModel = new VerifyModel(GetUserManager(), _verificationService.Object, _logger.Object);
            var context = new Mock<HttpContext>();
            context.Setup(x => x.User).Returns(new Mock<ClaimsPrincipal>().Object);
            verifyModel.PageContext.HttpContext = context.Object;

            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.Setup(x => x.Content(It.IsAny<string>())).Returns("redirect");
            verifyModel.Url = urlHelper.Object;

            // Act
            var result = await verifyModel.OnPostAsync("");

            // Assert
            Assert.IsType<LocalRedirectResult>(result);
        }

        [Fact]
        public async Task OnPostWithLoggedInUserAndInvalidModelStateThenReturnPage()
        {
            // Arrange
            _userStore.Setup(
                x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(new ApplicationUser());

            var verifyModel = new VerifyModel(GetUserManager(), _verificationService.Object, _logger.Object);
            var context = new Mock<HttpContext>();
            var session = new Mock<ISession>();
            var value = (byte[]) null;
            var principal = new Mock<ClaimsPrincipal>();
            principal.Setup(x => x.FindFirst(It.IsAny<string>()))
                .Returns(new Claim("name", "John Doe"));
            session.Setup(x => x.TryGetValue(It.IsAny<string>(), out value)).Returns(false);
            context.Setup(x => x.Session).Returns(session.Object);
            context.Setup(x => x.User).Returns(principal.Object);
            verifyModel.PageContext.HttpContext = context.Object;

            verifyModel.ModelState.AddModelError("key", "Another error");

            // Act
            var result = await verifyModel.OnPostAsync("");

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostWithLoggedInUserAndCodeVerificationFailThenModeStateInvalid()
        {
            // Arrange
            _userStore.Setup(
                x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(new ApplicationUser {PhoneNumber = "+1234567890"});

            var verifyModel = new VerifyModel(GetUserManager(), _verificationService.Object, _logger.Object);
            var context = new Mock<HttpContext>();
            var session = new Mock<ISession>();
            var value = (byte[]) null;
            var principal = new Mock<ClaimsPrincipal>();
            principal.Setup(x => x.FindFirst(It.IsAny<string>()))
                .Returns(new Claim("name", "John Doe"));
            session.Setup(x => x.TryGetValue(It.IsAny<string>(), out value)).Returns(false);
            context.Setup(x => x.Session).Returns(session.Object);
            context.Setup(x => x.User).Returns(principal.Object);

            _verificationService.Setup(
                x => x.CheckVerificationAsync(It.IsAny<string>(), It.IsAny<string>())
            ).ReturnsAsync(new VerificationResult(new List<string> {"Failed"}));

            verifyModel.Input = new VerifyModel.InputModel
            {
                Code = "123456"
            };

            verifyModel.PageContext.HttpContext = context.Object;

            // Act
            var result = await verifyModel.OnPostAsync("");

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(verifyModel.ModelState.IsValid);
            _verificationService.Verify(x => x.CheckVerificationAsync("+1234567890", "123456"), Times.Once);
        }

        [Fact]
        public async Task OnPostWithLoggedInUserAndCodeVerificationFailThenUserUpdateAndRedirectsHome()
        {
            // Arrange
            _userStore.Setup(
                x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(new ApplicationUser {PhoneNumber = "+1234567890"});

            var verifyModel = new VerifyModel(GetUserManager(), _verificationService.Object, _logger.Object);
            var context = new Mock<HttpContext>();
            var session = new Mock<ISession>();
            var value = (byte[]) null;
            var principal = new Mock<ClaimsPrincipal>();
            principal.Setup(x => x.FindFirst(It.IsAny<string>()))
                .Returns(new Claim("name", "John Doe"));
            session.Setup(x => x.TryGetValue(It.IsAny<string>(), out value)).Returns(false);
            context.Setup(x => x.Session).Returns(session.Object);
            context.Setup(x => x.User).Returns(principal.Object);

            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.Setup(x => x.Content(It.IsAny<string>())).Returns("redirect");
            verifyModel.Url = urlHelper.Object;

            _verificationService.Setup(
                x => x.CheckVerificationAsync(It.IsAny<string>(), It.IsAny<string>())
            ).ReturnsAsync(new VerificationResult("SID"));

            verifyModel.Input = new VerifyModel.InputModel
            {
                Code = "123456"
            };

            verifyModel.PageContext.HttpContext = context.Object;

            // Act
            var result = await verifyModel.OnPostAsync("");

            // Assert
            Assert.IsType<LocalRedirectResult>(result);
            Assert.True(verifyModel.ModelState.IsValid);
            _verificationService.Verify(x => x.CheckVerificationAsync("+1234567890", "123456"), Times.Once);
        }
    }
}