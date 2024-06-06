namespace TelecomShop.FrontendModels
{
    public class UsageStats
    {

        public required DateTime DateStart { get; set; }

        public required DateTime DateEnd { get; set; }

        public int? DataUsed { get; set; }

        public int? VoiceUsed { get; set; }

        public int? SmsUsed { get; set; }

        public float? MoneySpent { get; set; }

    }
}
