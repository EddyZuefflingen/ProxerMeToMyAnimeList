using System;

namespace ProxerMeToMyAnimeList.Models
{
    internal class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string birthday { get; set; }
        public string location { get; set; }
        public DateTime joined_at { get; set; }
    }
}
