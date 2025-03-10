namespace finalBeeit.Models
{
    public class SteamAppListResponse
    {
        public SteamAppListInner Response { get; set; }
    }

    public class SteamAppListInner
    {
        public List<SteamApp> Apps { get; set; }
        public bool HaveMoreResults { get; set; } // Optional, based on full response
        public int LastAppId { get; set; }       // Optional, based on full response
    }

    public class SteamApp
    {
        public int Appid { get; set; }           // Matches "appid" in JSON
        public string Name { get; set; }         // Matches "name" in JSON
        public long LastModified { get; set; }   // Matches "last_modified"
        public int PriceChangeNumber { get; set; } // Matches "price_change_number"
    }

    public class GameInfo
    {
        public int AppId { get; set; }           // Your target model
        public string Name { get; set; }
    }
}
