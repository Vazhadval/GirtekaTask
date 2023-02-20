namespace AggregationApi.Data.Entities
{
    public class ElectricityData
    {
        public int Id { get; set; }
        public string Network { get; set; }
        public decimal SumPPlus { get; set; }
        public decimal SumPMinus { get; set; }
    }
}
