using Newtonsoft.Json;
using ProxerMeToMyAnimeList.Classes;
using ProxerMeToMyAnimeList.Models;
using ProxerMeToMyAnimeList.Models.MAL;
using ProxerMeToMyAnimeList.Properties;
using RestSharp;
using System;
using System.Diagnostics;
using System.IO;

namespace ProxerMeToMyAnimeList.Services
{
    internal static class MyAnimeList
    {
        static string MAL_BASE_URI = "https://api.myanimelist.net/v2";
        static string AUTH_BASE_URL = "https://myanimelist.net";

        static string OAUTH_CODE = "";
        static string CODE_VERIFIER = "";
        static AuthToken token;


        public static void GeneratePKCE()
        {
            PKCE gen = new PKCE();
            CODE_VERIFIER = gen.CodeVerifier;
        }

        static void AuthoriseApp()
        {
            GeneratePKCE();

            string ENDPOINT = "/v1/oauth2/authorize";
            string TYPE = "response_type=code";
            string ID = $"client_id={Settings.Default.MAL_CLIENT_ID}";
            string CHALLENGE = $"code_challenge={CODE_VERIFIER}";
            Console.WriteLine("Authorise your application by entering the response URL");

            Process.Start(new ProcessStartInfo($"{AUTH_BASE_URL}{ENDPOINT}?{TYPE}&{ID}&{CHALLENGE}") { UseShellExecute = true, Verb = "open" });

            Console.WriteLine("Response URL:");
            Console.SetIn(new StreamReader(Console.OpenStandardInput(8192)));
            OAUTH_CODE = Console.ReadLine().Replace("http://localhost/oauth?code=", "");
            //OAUTH_CODE = Console.ReadLine().Replace("http://localhost/oauth?session_state=ff6c1226-c3d6-4a2f-b20a-7872e9017082&code=", "");

            if (OAUTH_CODE == "")
            {
                Console.WriteLine("Wrong URL provided");
                Console.WriteLine("Press a key to stop the process");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }
        static void GenerateOAuth2Token()
        {
            if (token != null) return;
            AuthoriseApp();

            var client = new RestClient(AUTH_BASE_URL);
            var request = new RestRequest("/v1/oauth2/token", Method.Post);
            request.AddParameter("client_id", Settings.Default.MAL_CLIENT_ID);
            request.AddParameter("client_secret", Settings.Default.MAL_CLIENT_SECRET);
            request.AddParameter("code", OAUTH_CODE);
            request.AddParameter("code_verifier", CODE_VERIFIER);
            request.AddParameter("grant_type", "authorization_code");

            var response = client.Execute(request);
            token = JsonConvert.DeserializeObject<AuthToken>(response.Content);
        }

        public static User GetUserInformation()
        {
            GenerateOAuth2Token();
            var client = new RestClient(MAL_BASE_URI);
            var request = new RestRequest("/users/@me", Method.Get);
            request.AddHeader("Authorization", $"Bearer {token.access_token}");

            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<User>(response.Content);
        }

        public static AnimeList GetMyAnimeList()
        {
            GenerateOAuth2Token();
            var client = new RestClient(MAL_BASE_URI);
            var request = new RestRequest("/users/@me/animelist", Method.Get);
            request.AddHeader("Authorization", $"Bearer {token.access_token}");
            request.AddParameter("limit", "1000");
            request.AddParameter("fields", "list_status");
            request.AddParameter("nsfw", true);

            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<AnimeList>(response.Content);
        }

        public static AnimeList GetAnimeList(string searchQuery)
        {
            GenerateOAuth2Token();
            var client = new RestClient(MAL_BASE_URI);
            var request = new RestRequest("/anime", Method.Get);
            request.AddHeader("Authorization", $"Bearer {token.access_token}");
            request.AddParameter("q", searchQuery, true);
            request.AddParameter("nsfw", true);

            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<AnimeList>(response.Content);
        }

        public static void UpdateMyAnimeList(int AnimeID, AnimeListStatus status)
        {
            GenerateOAuth2Token();
            var client = new RestClient(MAL_BASE_URI);
            var request = new RestRequest($"anime/{AnimeID}/my_list_status", Method.Put);
            request.AddHeader("Authorization", $"Bearer {token.access_token}");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            //Parameter müssen manuell leider
            request.AddParameter("status", status.status.ToString());
            request.AddParameter("num_watched_episodes", status.num_watched_episodes);

            var response = client.Execute(request);
        }

        public static void DeleteInMyAnimeList(int AnimeID)
        {
            GenerateOAuth2Token();
            var client = new RestClient(MAL_BASE_URI);
            var request = new RestRequest($"anime/{AnimeID}/my_list_status", Method.Delete);
            request.AddHeader("Authorization", $"Bearer {token.access_token}");

            var response = client.Execute(request);
        }

        public static AnimeDetails GetAnimeDetails(int AnimeID)
        {
            GenerateOAuth2Token();
            var client = new RestClient(MAL_BASE_URI);
            var request = new RestRequest($"anime/{AnimeID}", Method.Get);
            request.AddHeader("Authorization", $"Bearer {token.access_token}");
            request.AddParameter("fields", "id,title,main_picture,alternative_titles,start_date,end_date,synopsis,mean,rank,popularity,num_list_users,num_scoring_users,nsfw,created_at,updated_at,media_type,status,genres,my_list_status,num_episodes,start_season,broadcast,source,average_episode_duration,rating,pictures,background,related_anime,related_manga,recommendations,studios,statistics");
            request.AddParameter("nsfw", true);
            
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<AnimeDetails>(response.Content);
        }
    }
}
