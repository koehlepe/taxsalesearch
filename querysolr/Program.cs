using System;
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
            GenerateScoreableResults();
        }

        public static void GenerateScoreableResults()
        {
            var queries = File.ReadAllLines("queries.txt");
            SolrNet.Startup.Init<Property>("http://localhost:8983/solr/taxsaleinfo");
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Property>>();

            for (int i = 1; i < 5; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    var resultFile = new FileStream($"results/results_{i}_{j}.txt", FileMode.Create);
                    using (var resultFileWriter = new StreamWriter(resultFile))
                    {
                        foreach (var line in queries)
                        {
                            var pieces = line.Split(";");
                            var id = pieces[0];
                            var search = pieces[1];
                            AbstractSolrQuery query = new SolrQuery(search);
                            int k = 1;
                            var results = solr.Query(new LocalParams { { "type", "dismax" }, { "qf", $"title^{i} comments^{j}" } } + query, new QueryOptions { Fields = new[] { "*", "score" } });
                            foreach (var result in results.Take(20))
                            {
                                resultFileWriter.WriteLine($"{id}   0  {result.Id}  {k}   {result.Score}   searchtaxsales");
                                k++;
                            }
                        }
                    }
                }
            }
        }


        public static void DoRelevanceJudgements()
        {
            var queryDocPairsWithRelevance = new Dictionary<ValueTuple<string, string>, string>();
            for (int i = 1; i <= 20; i++)
            {
                var relevance = new FileStream($"relevancejudgments/qrels{i}.txt", FileMode.Open);
                using (var relevanceReader = new StreamReader(relevance))
                {
                    string line;
                    while ((line = relevanceReader.ReadLine()) != null)
                    {
                        var fields = line.Split(" ");
                        var queryId = fields[0];
                        var docId = fields[2];
                        var relevant = fields[3];
                        queryDocPairsWithRelevance.Add((queryId, docId), relevant);
                    }
                }
            }


            var queryDocPairsWithoutRelevance = new HashSet<ValueTuple<string, string>>();
            SolrNet.Startup.Init<Property>("http://localhost:8983/solr/taxsaleinfo");
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Property>>();
            FileStream fileStream = new FileStream($"queries.txt", FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {

                    var pieces = line.Split(";");
                    var id = pieces[0];
                    var search = pieces[1];
                    var words = search.Split(" ");

                    AbstractSolrQuery query = new SolrQuery(search);
                    for (int i = 1; i < 5; i++)
                    {
                        for (int j = 1; j < 5; j++)
                        {
                            var results = solr.Query(new LocalParams { { "type", "dismax" }, { "qf", $"title^{i} comments^{j}" } } + query);
                            foreach (var result in results.Take(20))
                            {
                                if (!queryDocPairsWithRelevance.ContainsKey((id, result.Id.ToString())))
                                {
                                    var commentLength = result.Comments[0].Length;
                                    Console.WriteLine(result.Id + " ^^^ " + result.Title[0] + result.Comments[0].Substring(0, Math.Min(commentLength, 100)));
                                    Console.WriteLine($"Enter your relevance judgement (0-1)<topic is {search}>:");
                                    var relevance = Console.ReadLine();
                                    queryDocPairsWithRelevance.Add((id, result.Id.ToString()), relevance);
                                }
                            }
                        }
                    }

                }
            }

            Console.WriteLine(queryDocPairsWithoutRelevance.Count);
            var qrels = new FileStream("qrels.txt", FileMode.Create);
            using (var qrelsWriter = new StreamWriter(qrels))
            {
                foreach (var key in queryDocPairsWithRelevance.Keys)
                {
                    qrelsWriter.WriteLine($"{key.Item1} 0 {key.Item2} {queryDocPairsWithRelevance[key]}");
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

            [SolrField("score")]
            public double Score { get; set; }

        }
    }
}



