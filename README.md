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

Next I wanted to start working on the User Interface, I decided to start by pulling a sample app from here https://github.com/dotnet/dotnet-docker/tree/main/samples/aspnetapp, I updated the app and added a search box on the main page.
 The commands to start up the User Interface are 

docker build --pull -t aspnetapp .   

Followed by:

docker run --rm -it -p 8000:80 aspnetapp