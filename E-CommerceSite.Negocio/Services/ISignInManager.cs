using E_CommerceSite.Datos.Customer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_CommerceSite.Negocio.Services
{
    public interface ISignInManager
    {
        public SignInManager<UserEntity> GetSignInManager();
    }

    public class ManageSignIn : SignInManager<UserEntity>, ISignInManager
    {
        public ManageSignIn(UserManager<UserEntity> userManager, IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<UserEntity> claimsFactory, IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<UserEntity>> logger, IAuthenticationSchemeProvider schemes,
            IUserConfirmation<UserEntity> confirmation) :
            base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        public SignInManager<UserEntity> GetSignInManager()
        {
            return this;
        }
    }
}
