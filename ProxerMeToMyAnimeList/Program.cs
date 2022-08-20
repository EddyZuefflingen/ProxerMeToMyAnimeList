using Newtonsoft.Json;
using ProxerMeToMyAnimeList.Models;
using ProxerMeToMyAnimeList.Models.MAL;
using ProxerMeToMyAnimeList.Models.ProxerMe;
using ProxerMeToMyAnimeList.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProxerMeToMyAnimeList
{
    static class Program
    {
        static void Main(string[] args)
        {
            SyncProxerAnimelistToMAL();
        }

        static int FindProxerAnimeIdOnMALEngName(AnimeListItem proxerItem)
        {
            Console.WriteLine($"Searching Original Name: {proxerItem.OriginalName}");
            AnimeList searchItems = MyAnimeList.GetAnimeList(proxerItem.OriginalName);
            if (searchItems.data != null)
                foreach (var data in searchItems.data)
                    if (data.node.title == proxerItem.OriginalName || data.node.title == proxerItem.ManualEnteredMALName)
                        return data.node.id;

            return 0;
        }

        static int FindProxerAnimeIdOnMALJapName(AnimeListItem proxerItem)
        {
            Console.WriteLine($"Searching Jap. Name: {proxerItem.JapName}");
            AnimeList searchItems = MyAnimeList.GetAnimeList(proxerItem.JapName);
            if (searchItems.data != null)
                foreach (var data in searchItems.data)
                {
                    AnimeDetails details = MyAnimeList.GetAnimeDetails(data.node.id);
                    if (details.alternative_titles.ja == proxerItem.JapName || data.node.title == proxerItem.ManualEnteredMALName)
                        return data.node.id;
                }
            return 0;
        }

        static void SyncProxerAnimelistToMAL()
        {
            List<AnimeListItem> ProxerMeList = ProxerMe.GetAnimeList();

            List<AnimeListItem> ProxerAnimeNotFound = new List<AnimeListItem>();
            string path = $@"{AppDomain.CurrentDomain.BaseDirectory}/ProxerAnimeNotFound.json";
            string PROXER_ANIME_LIST = $@"{AppDomain.CurrentDomain.BaseDirectory}/ProxerAnimeList.json";

            for (int i = 0; i < ProxerMeList.Count - 1; i++)
                if (ProxerMeList[i].OriginalName != "" && !ProxerMeList[i].IngoreOnSync && ProxerMeList[i].LastSync == DateTime.MinValue)
                {
                    int malAnimeID = FindProxerAnimeIdOnMALEngName(ProxerMeList[i]);
                    if (malAnimeID == 0) FindProxerAnimeIdOnMALJapName(ProxerMeList[i]);

                    //Wenn immer noch 0 dann nicht gefunden.
                    if (malAnimeID == 0)
                    {
                        ProxerAnimeNotFound.Add(ProxerMeList[i]);
                        Console.WriteLine($"Proxer Anime not found: {ProxerMeList[i].OriginalName}");
                        File.WriteAllText(path, JsonConvert.SerializeObject(ProxerAnimeNotFound, Formatting.Indented));
                    }
                    else
                    {
                        MyAnimeList.UpdateMyAnimeList(malAnimeID, new AnimeListStatus() { status = AnimeListStatus.statusTypes.completed });
                        ProxerMeList[i].LastSync = DateTime.Now;
                        File.WriteAllText(PROXER_ANIME_LIST, JsonConvert.SerializeObject(ProxerMeList, Formatting.Indented));
                        Console.WriteLine($"MAL Updated: {ProxerMeList[i].OriginalName} - {malAnimeID}");
                    }
                }
        }
    }
}
