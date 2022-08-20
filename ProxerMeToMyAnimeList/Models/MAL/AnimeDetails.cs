using System;
using System.Collections.Generic;

namespace ProxerMeToMyAnimeList.Models.MAL
{
    public class AlternativeTitles
    {
        public List<object> synonyms { get; set; }
        public string en { get; set; }
        public string ja { get; set; }
    }

    public class Broadcast
    {
        public string day_of_the_week { get; set; }
        public string start_time { get; set; }
    }

    public class Genre
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class MainPicture
    {
        public string medium { get; set; }
        public string large { get; set; }
    }

    public class MyListStatus
    {
        public string status { get; set; }
        public int score { get; set; }
        public int num_episodes_watched { get; set; }
        public bool is_rewatching { get; set; }
        public DateTime updated_at { get; set; }
        public string start_date { get; set; }
    }

    public class Node
    {
        public int id { get; set; }
        public string title { get; set; }
        public MainPicture main_picture { get; set; }
    }

    public class Picture
    {
        public string medium { get; set; }
        public string large { get; set; }
    }

    public class Recommendation
    {
        public Node node { get; set; }
        public int num_recommendations { get; set; }
    }

    public class RelatedAnime
    {
        public Node node { get; set; }
        public string relation_type { get; set; }
        public string relation_type_formatted { get; set; }
    }

    public class AnimeDetails
    {
        public int id { get; set; }
        public string title { get; set; }
        public MainPicture main_picture { get; set; }
        public AlternativeTitles alternative_titles { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string synopsis { get; set; }
        public double mean { get; set; }
        public int rank { get; set; }
        public int popularity { get; set; }
        public int num_list_users { get; set; }
        public int num_scoring_users { get; set; }
        public string nsfw { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string media_type { get; set; }
        public string status { get; set; }
        public List<Genre> genres { get; set; }
        public MyListStatus my_list_status { get; set; }
        public int num_episodes { get; set; }
        public StartSeason start_season { get; set; }
        public Broadcast broadcast { get; set; }
        public string source { get; set; }
        public int average_episode_duration { get; set; }
        public string rating { get; set; }
        public List<Picture> pictures { get; set; }
        public string background { get; set; }
        public List<RelatedAnime> related_anime { get; set; }
        public List<object> related_manga { get; set; }
        public List<Recommendation> recommendations { get; set; }
        public List<Studio> studios { get; set; }
        public Statistics statistics { get; set; }
    }

    public class StartSeason
    {
        public int year { get; set; }
        public string season { get; set; }
    }

    public class Statistics
    {
        public Status status { get; set; }
        public int num_list_users { get; set; }
    }

    public class Status
    {
        public string watching { get; set; }
        public string completed { get; set; }
        public string on_hold { get; set; }
        public string dropped { get; set; }
        public string plan_to_watch { get; set; }
    }

    public class Studio
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
