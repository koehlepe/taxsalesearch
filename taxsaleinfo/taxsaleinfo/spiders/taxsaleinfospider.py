import scrapy
import w3lib.html
#PHPSESSID=jauai3ivg2m769875dafvmsjr8; REMEMBERME=QXBwQnVuZGxlXEVudGl0eVxVc2VyczphMjlsYUd4bGNHVXg6MTYxNzc4Nzc3MzpmY2ZhZDI4ODdiMzUzY2FmNmI4NmMyNjc5NDgzYzk5MTNmZDgyYjAyODAyYjg4YWNlOWExNTYwMmM4N2Y0OTE1

class TaxSaleInfoSpider(scrapy.Spider):
    name = "taxsaleinfo"
    def start_requests(self):
        url = 'https://www.tax-sale.info/lot/show/id/120216'
        yield scrapy.Request(url,cookies=[{'name': 'PHPSESSID',
                                        'value': '<redacted>',
                                        'domain': 'www.tax-sale.info',
                                        'path': '/'}], callback=self.parse)

    def parse(self, response):

        comments = ' '.join([w3lib.html.remove_tags(item).strip() for item in response.xpath('/html/body/div[1]/section/div/div[1]/div[5]/div[2]/div/div/div[1]/div[2]/div').extract()])
        yield {
                'title': response.css('h4.center::text').get(),
                'comments': comments
            }