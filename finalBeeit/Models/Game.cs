namespace finalBeeit.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string Name { get; set; }
        public DateTime LastModified { get; set; }
        public int PriceChangeNumber { get; set; }
    }
}
