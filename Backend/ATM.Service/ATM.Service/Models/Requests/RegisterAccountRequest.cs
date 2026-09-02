namespace ATM.Service.Models.Requests
{
    public class RegisterAccountRequest
    {
        public required List<Account> Accounts { get; set; }
    }
}
