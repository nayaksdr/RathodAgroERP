namespace JaggeryAgro.Core.ViewModel
{
    public class DailyPointDto
    {
        public DailyPointDto(string label, double value)
        {
            Label = label;
            Value = value;
        }
        public string Label { get; set; }
        public double Value { get; set; }
    }

    // Moved out from DailyPointDto
    public class DailyJaggerySellDto
    {
        public DailyJaggerySellDto(string date, double qty, double amount)
        {
            Date = date;
            Qty = qty;
            Amount = amount;
        }

        public string Date { get; set; }
        public double Qty { get; set; }
        public double Amount { get; set; }
    }

    public class DashboardDailyVm
    {
        public List<string> Labels { get; set; } = new List<string>();
        public double JagerySellTotal { get; set; }

        public List<DailyPointDto> AttendanceDaily { get; set; } = new List<DailyPointDto>();
        public List<DailyPointDto> AdvancesDaily { get; set; } = new List<DailyPointDto>();
        public List<DailyPointDto> ExpensesDaily { get; set; } = new List<DailyPointDto>();
        public List<DailyPointDto> CanePurchaseTonsDaily { get; set; } = new List<DailyPointDto>();
        public List<DailyPointDto> ProduceQtyDaily { get; set; } = new List<DailyPointDto>();
        public List<DailyPointDto> SalesQtyDaily { get; set; } = new List<DailyPointDto>();
        public List<DailyPointDto> SalesAmountDaily { get; set; } = new List<DailyPointDto>();
        public List<DailyJaggeryShareDto> JaggeryShareDaily { get; set; } = new();

        public List<DailyJaggerySellDto> JagerySellDaily { get; set; } = new List<DailyJaggerySellDto>();

        // Today tiles
        public int TodayPresentCount { get; set; }
        public double TodayAdvance { get; set; }
        public double TodayExpense { get; set; }
        public double TodayCaneTons { get; set; }
        public double TodayProduceQty { get; set; }
        public double TodaySalesQty { get; set; }
        public double TodaySalesAmount { get; set; }
        public double TodayJaggerySellAmount { get; set; }
        public double TodayAdvanceDealer { get; set; }
        public double TodayCaneAdvace { get; set; }

        public decimal TodayJaggeryShareTotal { get; set; }
        public decimal TodayJaggerySharePaid { get; set; }
        public decimal TodayJaggerySharePending { get; set; }

    }
}
