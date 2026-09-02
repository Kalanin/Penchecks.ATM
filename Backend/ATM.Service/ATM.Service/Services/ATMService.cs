using ATM.Service.Models;

namespace ATM.Service.Services
{
    public interface IATMService
    {
    }

    public class ATMService : IATMService
    {
        private Dictionary<string, Account> _accounts = new Dictionary<string, Account>();
        private List<History> _transactionHistory = new List<History>(); //basic transation logging with string formatting, will adjust if time allows.


        //Use to seed data with initial API call. Acts as a reset of existing data, so it will wipe out any existing accounts and transaction history.
        public async Task RegisterAccountData(List<Account> accounts)
        {
            if (accounts == null || accounts.Count == 0)
            {
                return;
            }
            
            accounts.ForEach(account =>
            {
                _accounts[account.Name] = account; //doesn't matter if account already exists, we want to overwrite it with the new data
            });

            _transactionHistory.Clear(); //reset history since this is a new set of accounts, we don't want to keep old history
        }

        public List<Account> GetCurrentBalance()
        {
            return _accounts.Values.ToList();
        }

        public List<History> GetTransactionHistory(HistoryType? type = null)
        {
            var history = type == null ? _transactionHistory : _transactionHistory.Where(h => h.Type == type).ToList();

            return history.OrderBy(h => h.Timestamp).ToList();
        }

        public void DepositFunds(decimal amount, string accountName)
        {
            if (IsAmountValid(amount) && IsAccountValid(accountName))
            {
                var formattedAmount = RoundToTwoDecimalPlaces(amount);

                _accounts[accountName].Amount += formattedAmount;

                _transactionHistory.Add(new History
                {
                    Timestamp = DateTime.UtcNow,
                    Type = HistoryType.Deposit,
                    Log = $"Deposited {formattedAmount} to {accountName}. New balance: {_accounts[accountName].Amount}"
                });
            }
        }

        public void WithdrawFunds(decimal amount, string accountName)
        {
            if (IsAmountValid(amount) && IsAccountValid(accountName))
            {
                if (amount > _accounts[accountName].Amount)
                {
                    throw new ArgumentException(
                        $"Insufficient funds in account {accountName}. Current balance: {_accounts[accountName].Amount}");
                }

                var formattedAmount = RoundToTwoDecimalPlaces(amount);

                _accounts[accountName].Amount -= formattedAmount;

                _transactionHistory.Add(new History
                {
                    Timestamp = DateTime.UtcNow,
                    Type = HistoryType.Withdrawal,
                    Log = $"Withdrew {formattedAmount} from {accountName}. New balance: {_accounts[accountName].Amount}"
                });
            }
        }

        public void TransferFunds(decimal amount, string fromAccountName, string toAccountName)
        {
            if (IsAmountValid(amount) && IsAccountValid(fromAccountName) && IsAccountValid(toAccountName))
            {
                if (amount > _accounts[fromAccountName].Amount)
                {
                    throw new ArgumentException($"Insufficient funds in account {fromAccountName}. Current balance: {_accounts[fromAccountName].Amount}");
                }

                var formattedAmount = RoundToTwoDecimalPlaces(amount);

                _accounts[fromAccountName].Amount -= formattedAmount;
                _accounts[toAccountName].Amount += formattedAmount;

                AddHistory(HistoryType.Transfer, $"Transferred {formattedAmount} from {fromAccountName} to {toAccountName}. New balances: {fromAccountName}: {_accounts[fromAccountName].Amount}, {toAccountName}: {_accounts[toAccountName].Amount}");
            }
        }

        private decimal RoundToTwoDecimalPlaces(decimal amount)
        {
            return Math.Floor(amount * 100) / 100;
        }

        private void AddHistory(HistoryType type, string log)
        {
            _transactionHistory.Add(new History()
            {
                Timestamp = DateTime.UtcNow,
                Type = type,
                Log = log
            });
        }

        //Validation Methods that throw exceptions if the input is invalid.
        private bool IsAmountValid(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be positive.");
            }

            return true;
        }

        private bool IsAccountValid(string accountName)
        {
            if (!_accounts.ContainsKey(accountName))
            {
                throw new ArgumentException($"Account {accountName} does not exist.");
            }

            return true;
        }
    }
}
