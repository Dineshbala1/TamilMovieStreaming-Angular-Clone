using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using IronWebScraper;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace TamildboxScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var scraper = new BlogScraper();
            scraper.Start();
        }
    }

    class BlogScraper : WebScraper
    {
        public override void Init()
        {
            this.LoggingLevel = WebScraper.LogLevel.All;
            this.Request("http://www.tamildbox.best/", Parse);
        }

        public override void Parse(Response response)
        {
            ParseNavigationHeaderItems(response);
            GetCarouselInformation(response);
            GetFeatured1Items(response);
            Console.WriteLine(JsonConvert.SerializeObject(headerNavigationDatas, Formatting.Indented));
            Console.ReadLine();
        }

        private IList<HeaderNavigationData> headerNavigationDatas = new List<HeaderNavigationData>();

        private void ParseNavigationHeaderItems(Response response)
        {
            var element = response.GetElementById("myNavbar");
            var parentNodeList = element.GetElementsByTagName("UL");
            if (parentNodeList != null)
            {
                var menuItems = FilterElementNodes(parentNodeList[0]);
                if (menuItems != null)
                {
                    foreach (var menu in menuItems)
                    {
                        ProcessNavigationHeaderValues(menu.ChildNodes);
                    }
                }
            }

            Scrape(headerNavigationDatas, "HeaderData.json");
        }

        private void GetCarouselInformation(Response response)
        {
            var carouselInformation = new List<CarouselData>();
            var carouselElement = response.GetElementById("myCarousel");
            if (carouselElement != null)
            {
                var validNodeElements = FilterElementNodes(carouselElement);
                if (validNodeElements.Any())
                {
                    var carouselItems = validNodeElements.CSS(".item");
                    foreach (var carouselItem in carouselItems)
                    {
                        carouselInformation.Add(ParseCarouselData(carouselItem));
                    }
                }

                Scrape(carouselInformation, "Carousel.json");
            }
        }

        private void GetFeatured1Items(Response response)
        {
            var featuredItems = new List<MovieDetails>();
            var featuredTab = response.GetElementById("featured1");
            var listingItems = featuredTab.Css("div.listbox");
            if (listingItems.Any())
            {
                foreach (var listingItem in listingItems)
                {
                    featuredItems.Add(new MovieDetails
                    {
                        MovieThumb = listingItem.Css("img.thumb").First().Attributes.Last().Value,
                        MovieName = listingItem.Css("div.name").First().TextContentClean,
                        Quality = listingItem.Css("span.overlay").First().TextContentClean,
                        WatchLink = listingItem.Css("div.play").First().Attributes.Last().Value
                            .Replace("location=", "").Replace("'","").Replace(";","")
                    });
                }

                Scrape(featuredItems, "Featured1.json");
            }
        }

        private CarouselData ParseCarouselData(HtmlNode carouselHtmlNode)
        {
            var movieDetailInformation = new MovieDetails();
            // CarouselHtmlNode is the parent div <div class="item active"> this has the carousel image and the
            // data for the carousel item i.e Movie name, cast and crew etc.
            var movieImage = carouselHtmlNode.Css("img")?.First().Attributes;
            movieDetailInformation.MovieName =
                carouselHtmlNode.Css("div.carousel-caption h3")?.First().TextContentClean;
            var movieDetails = carouselHtmlNode.Css("div.carousel-caption p span.tags");
            if (movieDetails.Any())
            {
                movieDetailInformation.RunningTime = movieDetails[0].TextContentClean;
                movieDetailInformation.Quality = movieDetails[1].TextContentClean;
                movieDetailInformation.Country = movieDetails[2].TextContentClean;
                movieDetailInformation.ReleaseYear = movieDetails[3].TextContentClean;
            }

            movieDetailInformation.MovieSummary =
                carouselHtmlNode.Css("div.carousel-caption div.text")?.First().TextContentClean;
            return new CarouselData
                {MovieBannerUrl = movieImage.Values.First().ToString(), MovieDetail = movieDetailInformation};
        }

        private void ProcessNavigationHeaderValues(HtmlNode[] menu)
        {
            if (menu.Length > 0 && menu.Length < 2)
            {
                var parsedValue = GetMappedChildNodeData(menu.First());
                headerNavigationDatas.Add(new HeaderNavigationData
                { NavigationData = parsedValue.Item2, PresenterName = parsedValue.Item1 });
            }
            else
            {
                // Get Genre data
                headerNavigationDatas.Add(new HeaderNavigationData { PresenterName = "Genre" });
            }
        }

        private Tuple<string, string> GetMappedChildNodeData(HtmlNode pareHtmlNode)
        {
            return new Tuple<string, string>(pareHtmlNode.TextContentClean, GetAttributeValue(pareHtmlNode));
        }

        private string GetAttributeValue(HtmlNode parentNode)
        {
            return parentNode.Attributes.First().Value;
        }

        private HtmlNode[] FilterElementNodes(HtmlNode parentHtmlNode)
        {
            return parentHtmlNode.ChildNodes.Where(xnode => xnode.NodeType == "ELEMENT_NODE").ToArray();
        }
    }


    public class HeaderNavigationData
    {
        public string PresenterName { get; set; }
        public string NavigationData { get; set; }
    }

    public class CarouselData
    {
        public MovieDetails MovieDetail { get; set; }
        public string MovieBannerUrl { get; set; }
    }

    public class MovieDetails
    {
        public string MovieName { get; set; }
        public string MovieSummary { get; set; }
        public string RunningTime { get; set; }
        public string Quality { get; set; }
        public string Country { get; set; }
        public string ReleaseYear { get; set; }
        public string WatchLink { get; set; }
        public string MovieThumb { get; set; }
    }
}
