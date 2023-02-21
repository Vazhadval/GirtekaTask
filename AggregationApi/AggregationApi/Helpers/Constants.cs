namespace AggregationApi.Helpers
{
    public class Constants
    {
        public const string ElectricityDataBaseUrl = "https://data.gov.lt";
        public const string ObtNameFilter = "butas";
        public static string[] CsvFileUrls = new string[]
        {
            "/dataset/1975/download/10766/2022-05.csv",
            "/dataset/1975/download/10765/2022-04.csv",
            "/dataset/1975/download/10764/2022-03.csv",
            "/dataset/1975/download/10763/2022-02.csv",
        };
    }
}
