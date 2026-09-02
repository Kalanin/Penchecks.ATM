namespace ATM.Service.Models.Requests
{
    public class TransferRequest
    {
        public required string ToAccount { get; set; }
        public required string FromAccount { get; set; }
        public required decimal Amount { get; set; }
    }
}
