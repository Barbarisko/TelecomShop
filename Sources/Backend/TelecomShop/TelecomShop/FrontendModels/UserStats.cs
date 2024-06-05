namespace TelecomShop.Models
{
    public class UserStats
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string PhoneNumber { get; set; }
        public required float Balance { get; set; }
        public required float SmsBalance { get; set; }
        public required float SmsLimit { get; set; }
        public required float InternetBalance { get; set; }
        public required float InternetLimit { get; set; }
        public required float VoiceBalance { get; set; }
        public required float VoiceLimit { get; set; }
        public required float OneTimeTotal { get; set; }
        public required float RecurrentTotal { get; set; }


    }
}
