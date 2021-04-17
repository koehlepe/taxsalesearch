# Tax Sale Search

The aim of this project is to create a toy search engine for CSC 7991 (Information Retrieval) at Wayne State University.

**Usage**
---
``` 
cd SearchTaxSales

docker-compose build

docker-compose up -d
```

Navigate to http://localhost:8000/.


**Project Overview**
---

First, I needed to gather data. I decided to try out Scrapy to scrape https://www.tax-sale.info for property data. I followed this [tutorial](https://docs.scrapy.org/en/latest/intro/tutorial.html). I wrote the spider you'll see in /taxsaleinfo/taxsaleinfo/spiders/taxsaleinfospider.py, which can be run with the following command.

``` scrapy crawl taxsaleinfo -o properties.json```

Next, I needed to load the data into a search platform. For this I chose to use [Solr](https://solr.apache.org/).

To get up and running quickly, I chose to use a docker image for Solr and used the docker-compose.yml from [docker-solr](https://github.com/docker-solr/docker-solr).

To create an MVP product, I needed a user interface. For this, I started with an ASP.NET Core application from this example [aspnetapp](https://github.com/dotnet/dotnet-docker/tree/main/samples/aspnetapp). I updated the app and added a search box on the main page and a results view.

I had to integrate the user interface with Solr, so I added the necessary package. Then I added a simple call to query Solr on the Title and Comments of a Property.

Next Steps:

I'm going to create a script to load the data into Solr, so that I don't have to log into the Administration User Interface to do so.

After that, I will add additional settings/tuning for Solr and the queries I make from the User Interface.

Maybe I'll get to look at LTR as well.