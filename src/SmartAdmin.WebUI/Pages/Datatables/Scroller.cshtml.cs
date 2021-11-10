using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Datatables
{
    public class ScrollerModel : PageModel
    {
        private readonly ILogger<ScrollerModel> _logger;

        public ScrollerModel(ILogger<ScrollerModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
