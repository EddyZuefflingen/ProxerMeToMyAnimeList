using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxerMeToMyAnimeList.Models.ProxerMe
{
    internal class AnimeListItem
    {
        public string URL { get; set; }
        public string OriginalName { get; set; }
        public string JapName { get; set; }
        public string ManualEnteredMALName { get; set; }
        public bool IngoreOnSync { get; set; }
        public DateTime LastSync { get; set; }
    }
}
