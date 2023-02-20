namespace AggregationApi.Services.Implementations.Models
{
    public class ElectricityDataViewModel
    {
        public string Network { get; set; }
        public decimal SumPPlus { get; set; }
        public decimal SumPMinus { get; set; }
    }
}
