using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SaludYa.Pages.Public
{
    [AllowAnonymous]
    public class BuscarModel : PageModel
    {
        public void OnGet() { }
    }
}