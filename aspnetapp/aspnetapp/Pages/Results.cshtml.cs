using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using aspnetapp.Models;
using CommonServiceLocator;
using SolrNet;
using SolrNet.Commands.Parameters;


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


        [BindProperty(SupportsGet = true)]
        public String Search { get; set; }

        [BindProperty]
        public List<Result> Results {get;set;}

        public void OnGet()
        {
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Property>>();                     
            var results = solr.Query(new SolrQueryByField("title", Search) + new SolrQueryByField("comments", Search), new QueryOptions
            {
                StartOrCursor = new StartOrCursor.Start(0),
                Rows = 10
            });

            Results = new List<Result>();
            foreach (var result in results)
            {
                var commentLength = result.Comments[0].Length;
                var comment = result.Comments[0].Substring(0, Math.Min(commentLength, 80));
                Results.Add(new Result{Title=result.Title[0], Comment =comment, Link = result.Link[0]});
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("./Results",new { search = Search});
        }
    }
}
