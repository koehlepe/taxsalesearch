using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SearchTaxSales.Models;
using CommonServiceLocator;
using SolrNet;
using SolrNet.Commands.Parameters;


namespace SearchTaxSales.Pages
{
    public class ResultsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ResultsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Results = new List<Result>();
        }


        [BindProperty(SupportsGet = true)]
        public String Search { get; set; }

        [BindProperty]
        public List<Result> Results {get;set;}

        public void OnGet()
        {
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Property>>();
            
            AbstractSolrQuery query = new SolrQuery(Search);

            var results = solr.Query(new LocalParams {{"type", "dismax"},{"qf", "title comments"}} +query, new QueryOptions
            {
                StartOrCursor = new StartOrCursor.Start(0),
                Rows = 10
            });

            Results = new List<Result>();
            foreach (var result in results)
            {
                var comment = "";
                if(result.Comments != null && result.Comments.Length>0){
                    var commentLength = result.Comments[0].Length;
                    comment = result.Comments[0].Substring(0, Math.Min(commentLength, 80));
                }
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
