namespace ATM.Service.Models.Requests
{
    //Exact same as DepositRequest, but for Withdrawing
    public class WithdrawRequest
    {
        public required string AccountName { get; set; }
        public required decimal Amount { get; set; }
    }
}
