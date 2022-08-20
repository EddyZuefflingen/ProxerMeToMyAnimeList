using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
