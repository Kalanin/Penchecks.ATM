namespace ATM.Service.Models
{
    public class History
    {
        public required DateTime Timestamp { get; set; }
        public required HistoryType Type { get; set; }
        public required string Log { get; set; }
    }

    public enum HistoryType
    {
        Deposit,
        Withdrawal,
        Transfer
    }
}
