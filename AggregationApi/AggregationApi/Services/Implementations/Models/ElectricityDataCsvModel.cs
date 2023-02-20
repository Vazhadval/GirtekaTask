using CsvHelper.Configuration;

namespace AggregationApi.Services.Implementations.Models
{
    public class ElectricityDataCsvModel
    {
        public string Network { get; set; }
        public string ObtName { get; set; }
        public string ObjGvType { get; set; }
        public string ObjNumber { get; set; }
        public decimal? PPlus { get; set; }
        public DateTime? PLT { get; set; }
        public decimal? PMinus { get; set; }
    }

    public class CsvDataMap : ClassMap<ElectricityDataCsvModel>
    {
        public CsvDataMap()
        {
            Map(m => m.Network).Name("TINKLAS");
            Map(m => m.ObtName).Name("OBT_PAVADINIMAS");
            Map(m => m.ObjGvType).Name("OBJ_GV_TIPAS");
            Map(m => m.ObjNumber).Name("OBJ_NUMERIS");
            Map(m => m.PPlus).Name("P+");
            Map(m => m.PLT).Name("PL_T");
            Map(m => m.PMinus).Name("P-");
        }
    }
}
