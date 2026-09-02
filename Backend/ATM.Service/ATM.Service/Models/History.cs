namespace ATM.Service.Models
{
    public class History
    {
        public DateTime Timestamp { get; set; }
        public HistoryType Type { get; set; }
        public string Log { get; set; }
    }

    public enum HistoryType
    {
        Deposit,
        Withdrawal,
        Transfer
    }
}
