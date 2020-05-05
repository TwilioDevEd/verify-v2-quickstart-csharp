using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VerifyV2Quickstart.Models;

namespace VerifyV2Quickstart.Tests
{
    public class FakeSignInManager : SignInManager<ApplicationUser>
    {
        public FakeSignInManager() : base(
            new Mock<FakeUserManager>().Object,
            new HttpContextAccessor(),
            new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
            null, null)
        {
        }
    }
}