using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxerMeToMyAnimeList.Models
{
    internal class AnimeListStatus
    {
        public enum statusTypes { watching, completed, on_hold, dropped, plan_to_watch };
        public bool is_rewatching { get; set; }
        public int score { get; set; }
        public int num_watched_episodes { get; set; }

        private statusTypes _status { get; set; }
        public statusTypes status
        {
            get { return _status; }
            set 
            {
                _status = value;
                if (_status == statusTypes.completed) num_watched_episodes = 999999;
            }
        }

    }
}
