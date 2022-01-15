using System;
using System.Text.Json.Serialization;

namespace ServitorDiscordBot.SoundCloud
{
    public class SongUrl
    {
        public string url { get; set; }
    }

    public class Playlist
    {
        [JsonIgnore]
        public string artwork_url { get; set; }
        [JsonIgnore]
        public DateTime created_at { get; set; }
        [JsonIgnore]
        public string description { get; set; }
        [JsonIgnore]
        public int duration { get; set; }
        [JsonIgnore]
        public string embeddable_by { get; set; }
        [JsonIgnore]
        public string genre { get; set; }
        [JsonIgnore]
        public int id { get; set; }
        [JsonIgnore]
        public string kind { get; set; }
        [JsonIgnore]
        public object label_name { get; set; }
        [JsonIgnore]
        public DateTime last_modified { get; set; }
        [JsonIgnore]
        public string license { get; set; }
        [JsonIgnore]
        public int likes_count { get; set; }
        [JsonIgnore]
        public bool managed_by_feeds { get; set; }
        [JsonIgnore]
        public string permalink { get; set; }
        [JsonIgnore]
        public string permalink_url { get; set; }
        [JsonIgnore]
        public bool _public { get; set; }
        [JsonIgnore]
        public object purchase_title { get; set; }
        [JsonIgnore]
        public object purchase_url { get; set; }
        [JsonIgnore]
        public DateTime release_date { get; set; }
        [JsonIgnore]
        public int reposts_count { get; set; }
        [JsonIgnore]
        public object secret_token { get; set; }
        [JsonIgnore]
        public string sharing { get; set; }
        [JsonIgnore]
        public string tag_list { get; set; }
        [JsonIgnore]
        public string title { get; set; }
        [JsonIgnore]
        public string uri { get; set; }
        [JsonIgnore]
        public int user_id { get; set; }
        [JsonIgnore]
        public string set_type { get; set; }
        [JsonIgnore]
        public bool is_album { get; set; }
        [JsonIgnore]
        public DateTime published_at { get; set; }
        [JsonIgnore]
        public DateTime display_date { get; set; }
        [JsonIgnore]
        public User user { get; set; }

        public Track[] tracks { get; set; }

        [JsonIgnore]
        public int track_count { get; set; }
    }

    public class Track
    {
        [JsonIgnore]
        public string artwork_url { get; set; }
        [JsonIgnore]
        public object caption { get; set; }
        [JsonIgnore]
        public bool commentable { get; set; }
        [JsonIgnore]
        public int comment_count { get; set; }
        [JsonIgnore]
        public DateTime created_at { get; set; }
        [JsonIgnore]
        public string description { get; set; }
        [JsonIgnore]
        public bool downloadable { get; set; }
        [JsonIgnore]
        public int download_count { get; set; }

        public int duration { get; set; }

        [JsonIgnore]
        public int full_duration { get; set; }
        [JsonIgnore]
        public string embeddable_by { get; set; }
        [JsonIgnore]
        public string genre { get; set; }
        [JsonIgnore]
        public bool has_downloads_left { get; set; }
        [JsonIgnore]
        public int id { get; set; }
        [JsonIgnore]
        public string kind { get; set; }
        [JsonIgnore]
        public object label_name { get; set; }
        [JsonIgnore]
        public DateTime last_modified { get; set; }
        [JsonIgnore]
        public string license { get; set; }
        [JsonIgnore]
        public int likes_count { get; set; }
        [JsonIgnore]
        public string permalink { get; set; }
        [JsonIgnore]
        public string permalink_url { get; set; }
        [JsonIgnore]
        public int playback_count { get; set; }
        [JsonIgnore]
        public bool _public { get; set; }

        public Publisher_Metadata publisher_metadata { get; set; }

        [JsonIgnore]
        public object purchase_title { get; set; }
        [JsonIgnore]
        public object purchase_url { get; set; }
        [JsonIgnore]
        public DateTime release_date { get; set; }
        [JsonIgnore]
        public int reposts_count { get; set; }
        [JsonIgnore]
        public object secret_token { get; set; }
        [JsonIgnore]
        public string sharing { get; set; }
        [JsonIgnore]
        public string state { get; set; }
        [JsonIgnore]
        public bool streamable { get; set; }
        [JsonIgnore]
        public string tag_list { get; set; }

        public string title { get; set; }

        [JsonIgnore]
        public string track_format { get; set; }
        [JsonIgnore]
        public string uri { get; set; }
        [JsonIgnore]
        public string urn { get; set; }
        [JsonIgnore]
        public int user_id { get; set; }
        [JsonIgnore]
        public object visuals { get; set; }
        [JsonIgnore]
        public string waveform_url { get; set; }
        [JsonIgnore]
        public DateTime display_date { get; set; }

        public Media media { get; set; }

        [JsonIgnore]
        public string station_urn { get; set; }
        [JsonIgnore]
        public string station_permalink { get; set; }

        public string track_authorization { get; set; }

        [JsonIgnore]
        public string monetization_model { get; set; }
        [JsonIgnore]
        public string policy { get; set; }
        [JsonIgnore]
        public User user { get; set; }
    }

    public class Publisher_Metadata
    {
        [JsonIgnore]
        public int id { get; set; }
        [JsonIgnore]
        public string urn { get; set; }

        public string artist { get; set; }

        [JsonIgnore]
        public bool contains_music { get; set; }
        [JsonIgnore]
        public string isrc { get; set; }
        [JsonIgnore]
        public string release_title { get; set; }
    }

    public class Media
    {
        public Transcoding[] transcodings { get; set; }
    }

    public class Transcoding
    {
        public string url { get; set; }

        [JsonIgnore]
        public string preset { get; set; }
        [JsonIgnore]
        public int duration { get; set; }
        [JsonIgnore]
        public bool snipped { get; set; }
        [JsonIgnore]
        public Format format { get; set; }
        [JsonIgnore]
        public string quality { get; set; }
    }

    public class Format
    {
        public string protocol { get; set; }
        public string mime_type { get; set; }
    }

    public class User
    {
        public string avatar_url { get; set; }
        public string city { get; set; }
        public int comments_count { get; set; }
        public string country_code { get; set; }
        public DateTime created_at { get; set; }
        public Creator_Subscriptions[] creator_subscriptions { get; set; }
        public Creator_Subscription creator_subscription { get; set; }
        public string description { get; set; }
        public int followers_count { get; set; }
        public int followings_count { get; set; }
        public string first_name { get; set; }
        public string full_name { get; set; }
        public int groups_count { get; set; }
        public int id { get; set; }
        public string kind { get; set; }
        public DateTime last_modified { get; set; }
        public string last_name { get; set; }
        public int likes_count { get; set; }
        public int playlist_likes_count { get; set; }
        public string permalink { get; set; }
        public string permalink_url { get; set; }
        public int playlist_count { get; set; }
        public object reposts_count { get; set; }
        public int track_count { get; set; }
        public string uri { get; set; }
        public string urn { get; set; }
        public string username { get; set; }
        public bool verified { get; set; }
        public Visuals visuals { get; set; }
        public Badges badges { get; set; }
        public string station_urn { get; set; }
        public string station_permalink { get; set; }
    }

    public class Creator_Subscription
    {
        public Product product { get; set; }
    }

    public class Product
    {
        public string id { get; set; }
    }

    public class Visuals
    {
        public string urn { get; set; }
        public bool enabled { get; set; }
        public Visual[] visuals { get; set; }
        public object tracking { get; set; }
    }

    public class Visual
    {
        public string urn { get; set; }
        public int entry_time { get; set; }
        public string visual_url { get; set; }
    }

    public class Badges
    {
        public bool pro { get; set; }
        public bool pro_unlimited { get; set; }
        public bool verified { get; set; }
    }

    public class Creator_Subscriptions
    {
        public Product1 product { get; set; }
    }

    public class Product1
    {
        public string id { get; set; }
    }

}
