namespace TelecomShop.FrontendModels
{
    public class UsageStats
    {

        public required DateTime Date { get; set; }


        public int? DataUsed { get; set; }

        public int? VoiceUsed { get; set; }

        public int? SmsUsed { get; set; }

        public float? MoneySpent { get; set; }

    }
}
