using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using aspnetapp.Models;

namespace aspnetapp.Pages
{
    public class ResultsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ResultsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Results = new List<Result>(){new Result{Title="Lake House",Comment="This is an example listing. I would expect the search to show some part of the comment", Link="http://www.lakehouse.com"}};
        }


        [BindProperty]
        public Search Search { get; set; }

        [BindProperty]
        public List<Result> Results {get;set;}

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("./Results",new { q = Search.Input});
        }
    }
}
