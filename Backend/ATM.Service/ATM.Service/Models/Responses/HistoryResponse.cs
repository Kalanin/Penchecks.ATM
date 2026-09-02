namespace ATM.Service.Models.Responses
{
    public class HistoryResponse
    {
        public DateTime Timestamp { get; set; }
        public required string Type { get; set; }
        public required string Log { get; set; }
    }
}
