namespace StockExampleApp.Entity
{
    public class Report
    {
        public int reportId { get; set; }
        public int productNo { get; set; }
        public string productName { get; set; }
        public DateTime actionTime { get; set; }
        public int currentQuantity { get; set; }

        public int droppedQuantity { get; set; }
        public int addedQuantity { get; set; }
        public string status { get; set; }
    }
}
