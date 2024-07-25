﻿namespace Stark.Starter.ElasticSearch.Config
{
    public class EsOptions
    {
        public string Urls { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public List<Uri> Uris => Urls.Split('|').Select(x => new Uri(x)).ToList();
    }
}
