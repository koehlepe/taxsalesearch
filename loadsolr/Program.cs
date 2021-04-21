using System;
using System.Collections.Generic;
using System.IO;
using CommonServiceLocator;
using Newtonsoft.Json;
using SolrNet;
using SolrNet.Attributes;

namespace loadsolr
{
    class Program
    {
        static void Main(string[] args)
        {
            SolrNet.Startup.Init<Property>("http://localhost:8983/solr/taxsaleinfo");
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Property>>();
            var i = 1;
            for (int year = 2010; year < 2021; year++)
            {
                var propertyList = new List<Property> ();
                FileStream fileStream = new FileStream($"data\\properties_{year}.json", FileMode.Open);
                using (StreamReader reader = new StreamReader(fileStream))
                {

                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        try
                        {
                            if (line.EndsWith(","))
                            {
                                line = line.Remove(line.Length - 1, 1);
                            }                            
                            
                            Property property = JsonConvert.DeserializeObject<Property>(line);
                            if(!String.IsNullOrWhiteSpace(property.Title)&&!String.IsNullOrWhiteSpace(property.Comments)){
                                property.Id = i.ToString();
                                i++;
                                propertyList.Add(property);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        finally
                        {
                            if(propertyList.Count == 500){
                                solr.AddRange(propertyList);
                                solr.Commit();
                                propertyList.Clear();
                                Console.WriteLine("writing a batch of 500 properties");
                            }
                            line = reader.ReadLine();
                        }
                        
                    }
                }
                if(propertyList.Count>0){
                    solr.AddRange(propertyList);
                    solr.Commit();
                    propertyList.Clear();
                }
            }
        }

        public class Property
        {
            [SolrUniqueKey("id")]
            public string Id { get; set; }

            [SolrField("title")]
            public string Title { get; set; }

            [SolrField("comments")]
            public string Comments { get; set; }

            [SolrField("link")]
            public string Link { get; set; }

        }
    }
}
