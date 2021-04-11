    
using System.ComponentModel.DataAnnotations;
using SolrNet.Attributes;

namespace aspnetapp.Models
{
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