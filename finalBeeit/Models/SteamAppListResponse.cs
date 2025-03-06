namespace finalBeeit.Models
{
    public class SteamAppListResponse
    {
        public Applist Applist { get; set; }
    }

    public class Applist
    {
        public List<App> Apps { get; set; }
    }

    public class App
    {
        public int AppId { get; set; }
        public string Name { get; set; }
    }
}
