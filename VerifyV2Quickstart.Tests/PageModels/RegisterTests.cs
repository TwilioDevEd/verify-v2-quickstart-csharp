using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VerifyV2Quickstart.Areas.Identity.Pages.Account;
using VerifyV2Quickstart.Models;
using VerifyV2Quickstart.Services;
using Xunit;

namespace VerifyV2Quickstart.Tests.PageModels
{
    public class RegisterTests
    {
        private readonly Mock<IVerification> _verificationService;
        private readonly Mock<FakeSignInManager> _signInManage;
        private readonly Mock<IUserStore<ApplicationUser>> _userStore;
        private readonly Mock<ILogger<RegisterModel>> _logger;

        public RegisterTests()
        {
            _userStore = new Mock<IUserStore<ApplicationUser>>();
            _verificationService = new Mock<IVerification>();
            _logger = new Mock<ILogger<RegisterModel>>();
            _signInManage = new Mock<FakeSignInManager>();
        }

        private UserManager<ApplicationUser> GetUserManager()
        {
            var hasher = new Mock<IPasswordHasher<ApplicationUser>>();

            return new UserManager<ApplicationUser>(_userStore.Object, null, hasher.Object, null,
                null, null, null, null, null);
        }


        [Fact]
        public void OnGetReturnUrlIsAssigned()
        {
            // Arrange
            var registerModel = new RegisterModel(GetUserManager(), _signInManage.Object, _verificationService.Object, _logger.Object);

            // Act
            registerModel.OnGet("returnUrl");

            // Assert
            Assert.Equal("returnUrl", registerModel.ReturnUrl);
        }

        [Fact]
        public async Task OnPostWithInvalidModelStateThenReturnsPage()
        {
            var registerModel = new RegisterModel(GetUserManager(),_signInManage.Object,  _verificationService.Object, _logger.Object);

            // Arrange
            registerModel.ModelState.AddModelError("key", "Another error");

            // Act
            var result = await registerModel.OnPostAsync("");

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostWithValidModelStateAndUserCreationFailsThenReturnsPage()
        {
            // Arrange
            _userStore.As<IUserPasswordStore<ApplicationUser>>().Setup(x => x.CreateAsync(
                It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()
            )).ReturnsAsync(
                IdentityResult.Failed(new IdentityError {Code = "1", Description = "Error"})
            );

            var registerModel = new RegisterModel(GetUserManager(), _signInManage.Object, _verificationService.Object, _logger.Object);

            registerModel.Input = new RegisterModel.InputModel
            {
                UserName = "username", Password = "Pas$w0rd", FullPhoneNumber = "+1234567890"
            };

            // Act
            var result = await registerModel.OnPostAsync("");

            // Assert
            Assert.False(registerModel.ModelState.IsValid);
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostWithValidModelAndUserCreatedAndVerificationStartFailsThenInvalidateModel()
        {
            // Arrange
            _userStore.As<IUserPasswordStore<ApplicationUser>>().Setup(x => x.CreateAsync(
                It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()
            )).ReturnsAsync(IdentityResult.Success);

            _verificationService.Setup(
                x => x.StartVerificationAsync(It.IsAny<string>(), It.IsAny<string>())
            ).ReturnsAsync(new VerificationResult(new List<string> {"Error"}));

            var registerModel = new RegisterModel(GetUserManager(), _signInManage.Object, _verificationService.Object, _logger.Object);

            registerModel.Input = new RegisterModel.InputModel
            {
                UserName = "username", Password = "Pas$w0rd", FullPhoneNumber = "+1234567890", Channel = "sms"
            };

            // Act
            var result = await registerModel.OnPostAsync("");

            // Assert
            Assert.False(registerModel.ModelState.IsValid);
            Assert.IsType<PageResult>(result);
            _verificationService.Verify(x => x.StartVerificationAsync("+1234567890", "sms"), Times.Once);
        }

        [Fact]
        public async Task OnPostWithValidModelAndUserCreatedAndVerificationStartedThenRedirectToVerify()
        {
            // Arrange
            _userStore.As<IUserPasswordStore<ApplicationUser>>().Setup(x => x.CreateAsync(
                It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()
            )).ReturnsAsync(IdentityResult.Success);

            _verificationService.Setup(
                x => x.StartVerificationAsync(It.IsAny<string>(), It.IsAny<string>())
            ).ReturnsAsync(new VerificationResult("SID"));

            var registerModel = new RegisterModel(GetUserManager(), _signInManage.Object, _verificationService.Object, _logger.Object);

            registerModel.Input = new RegisterModel.InputModel
            {
                UserName = "username", Password = "Pas$w0rd", FullPhoneNumber = "+1234567890", Channel = "sms"
            };

            var context = new Mock<HttpContext>();
            context.Setup(x => x.Session).Returns(new Mock<ISession>().Object);
            registerModel.PageContext.HttpContext = context.Object;

            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.Setup(x => x.Content(It.IsAny<string>())).Returns("redirect");
            registerModel.Url = urlHelper.Object;

            // Act
            var result = await registerModel.OnPostAsync("return");

            // Assert
            Assert.True(registerModel.ModelState.IsValid);
            Assert.IsType<LocalRedirectResult>(result);
            Assert.Equal("redirect", (result as LocalRedirectResult)?.Url);
            urlHelper.Verify(x => x.Content("~/Identity/Account/Verify/?returnUrl=return"), Times.Once);
            _verificationService.Verify(x => x.StartVerificationAsync("+1234567890", "sms"), Times.Once);
        }
    }
}