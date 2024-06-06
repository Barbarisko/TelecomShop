namespace TelecomShop.Models
{
    public class Superpower
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }

        public float? PriceOneTimeTotal { get; set; }

        public float? PriceRecurrentTotal { get; set; }

        public Dictionary<string, string> Characteristics { get; set; }
        public Dictionary<string, string> CharacteristicListValues { get; set; }
    }
}
