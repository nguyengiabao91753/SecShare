using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecShare.Core.Dtos;

namespace SecShare.Web.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        public RegistrationRequestDto registrationRequestDto { get; set; }
        public void OnGet()
        {
        }
    }
}
