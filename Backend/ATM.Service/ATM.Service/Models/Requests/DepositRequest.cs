namespace ATM.Service.Models.Requests
{
    public class DepositRequest
    {
        public required string AccountName { get; set; }
        public required decimal Amount { get; set; }
    }
}
