using System.Web.UI.WebControls;
using DotVVM.Framework.Hosting;
using Netfox.Web.BL.DTO;
using Netfox.Web.BL.Facades;
using Riganti.Utils.Infrastructure.Core;

namespace Netfox.Web.App.ViewModels
{
    public class LoginViewModel : LayoutViewModel
    {
        private readonly LoginFacade loginFacade;

        public override string Title => "Log In";


        public LoginViewModel(LoginFacade loginFacade)
        {
            this.loginFacade = loginFacade;
        }

        public LoginDTO LoginData { get; set; } = new LoginDTO();

        public void DoLogin()
        {
            try
            {
                var identity = this.loginFacade.SignIn(LoginData);
                this.Context.GetAuthentication().SignIn(identity);
                this.Context.RedirectToRoute("Default");
            }
            catch(UIException ex) { this.ErrorMessage = ex.Message; }
            catch(DotvvmInterruptRequestExecutionException e) { throw; }
        }
    }
}

