namespace TelecomShop.Services
{
    interface IStatisticService
    {
        public void GenerateExcelFile(DateTime dateStart, DateTime dateEnd);
    }
}
