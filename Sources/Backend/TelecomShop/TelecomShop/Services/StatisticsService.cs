using OfficeOpenXml;
using System.Data;
using TelecomShop.UnitOfWork;

namespace TelecomShop.Services
{
    public class StatisticsService
    {
        private readonly IUnitOfWork unitOfWork;

        public StatisticsService (IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public void GenerateExcelFile(DateTime dateStart, DateTime dateEnd)
        {
            if (dateStart > dateEnd)
            {
                throw new ArgumentException("Start date should be smaller");
            }


        }
           
    }
}
