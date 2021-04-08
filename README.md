# Tax Sale Search

I'm starting by gathering data/documents using Scrapy following this tutorial:
https://docs.scrapy.org/en/latest/intro/tutorial.html

To create an json output file from scrapy, we simply have to run the following command:
scrapy crawl taxsaleinfo -o properties.json

Adding the docker-compose.yml from the docker-solr repo:
https://github.com/docker-solr/docker-solr

Just run:
docker-compose up -d
To start it up.

We can import the file from the crawl into solr from the admin user interface.