using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonServiceLocator;
using SolrNet;
using SolrNet.Attributes;
using SolrNet.Commands.Parameters;

namespace querysolr
{
    class Program
    {
        static void Main(string[] args)
        {
            SolrNet.Startup.Init<Property>("http://localhost:8983/solr/taxsaleinfo");
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Property>>();


            FileStream fileStream = new FileStream($"queries.txt", FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {

                string line = reader.ReadLine();

                var pieces = line.Split(";");
                var id = pieces[0];
                var search = pieces[1];
                var words = search.Split(" ");

                AbstractSolrQuery query = new SolrQuery(search);

                var results = solr.Query(new LocalParams {{"type", "dismax"},{"qf", "title comments"}} + query);
                using (StreamWriter writetext = new StreamWriter($"queryresults\\results{id}.txt"))
                {
                    foreach (var result in results.Take(20))
                    {

                        writetext.WriteLine(result.Link[0] + " ^^^ " + result.Title[0]);

                    }
                }

            }
        }
    }

    public class Property
    {
        [SolrUniqueKey("id")]
        public string Id { get; set; }

        [SolrField("title")]
        public string[] Title { get; set; }

        [SolrField("comments")]
        public string[] Comments { get; set; }

        [SolrField("link")]
        public string[] Link { get; set; }

    }
}
