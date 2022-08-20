using HtmlAgilityPack;
using Newtonsoft.Json;
using ProxerMeToMyAnimeList.Models.ProxerMe;
using ProxerMeToMyAnimeList.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ProxerMeToMyAnimeList.Services
{
    internal static class ProxerMe
    {
        static string PROXER_URL = "https://proxer.me";
        static string PROXER_ANIME_LIST = $@"{AppDomain.CurrentDomain.BaseDirectory}/ProxerAnimeList.json";

        //Scrapper... Proxer.Me wont give me API access
        public static List<AnimeListItem> GetAnimeList()
        {
            List<AnimeListItem> animeItems = new List<AnimeListItem>();

            if (File.Exists(PROXER_ANIME_LIST))
                animeItems = JsonConvert.DeserializeObject<List<AnimeListItem>>(File.ReadAllText(PROXER_ANIME_LIST));

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(GetHtmlString(Settings.Default.PROXER_ANIMELIST_URL));

            //Collect all Animes
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                // Get the value of the HREF attribute
                string hrefValue = link.GetAttributeValue("href", string.Empty);
                if (hrefValue.Contains("/info/") && hrefValue.Contains("#top"))
                    if (animeItems.Where(item => item.URL == hrefValue).Count() == 0)
                        animeItems.Add(new AnimeListItem() { URL = hrefValue, OriginalName = link.InnerText });
            }

            for (int i = 0; i < animeItems.Count - 1; i++)
            {
                File.WriteAllText(PROXER_ANIME_LIST, JsonConvert.SerializeObject(animeItems, Formatting.Indented));
                if (animeItems[i].JapName == null || animeItems[i].JapName == "")
                {
                    Console.WriteLine($"{i + 1}/{animeItems.Count} - working on: {animeItems[i].OriginalName}");
                    animeItems[i].JapName = GetJapName(PROXER_URL + animeItems[i].URL);
                    File.WriteAllText(PROXER_ANIME_LIST, JsonConvert.SerializeObject(animeItems, Formatting.Indented));
                }
            }

            return animeItems;
        }

        public static string GetJapName(string Url)
        {
            HtmlDocument animeDoc = new HtmlDocument();
            animeDoc.LoadHtml(GetHtmlString(Url));
            HtmlNode titleNode = animeDoc.DocumentNode.SelectSingleNode("//b[.='Japanischer Titel']");
            string japName = "";
            if (titleNode == null)
            {
                if (animeDoc.DocumentNode.InnerText.Contains("Bitte logge dich ein, um diesen Bereich betreten zu"))
                    Console.WriteLine("Login required for " + Url);
                else
                {
                    AskForCaptchaSolving();
                    japName = GetJapName(Url);
                }

            }
            else
            {
                HtmlNode fullTitleNode = titleNode.ParentNode.ParentNode;
                japName = fullTitleNode.ChildNodes[3].InnerHtml;
            }

            return japName;
        }

        static void AskForCaptchaSolving()
        {
            Console.WriteLine("Captcha needed, please solve it and press enter to continue...");
            Console.ReadLine();
            Console.WriteLine("working again...");
            Console.WriteLine("");
        }

        static string GetHtmlString(string url)
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", "Only a test!");
            return wc.DownloadString(url);
        }
    }
}
