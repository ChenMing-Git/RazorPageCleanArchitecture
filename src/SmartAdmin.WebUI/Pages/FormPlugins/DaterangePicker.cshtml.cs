using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.FormPlugins
{
    public class DaterangePickerModel : PageModel
    {
        private readonly ILogger<DaterangePickerModel> _logger;

        public DaterangePickerModel(ILogger<DaterangePickerModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
