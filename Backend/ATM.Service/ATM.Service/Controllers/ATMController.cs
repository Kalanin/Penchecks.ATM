using ATM.Service.Models;
using ATM.Service.Models.Requests;
using ATM.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace ATM.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ATMController(IATMService atmService) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetBalances() {
            return HandleRequest(() =>
            {
                var balances = atmService.GetCurrentBalance();
                return Ok(balances);
            });
        }

        [HttpGet("history")]
        public IActionResult GetTransactionHistory(HistoryType? type = null) {
            return HandleRequest(() =>
            {
                var history = atmService.GetTransactionHistory(type);
                return Ok(history);
            });
        }

        [HttpPost("deposit")]
        public IActionResult DepositFunds([FromBody] DepositRequest request)
        {
            return HandleRequest(() =>
            {
                atmService.DepositFunds(request.Amount, request.AccountName);
                return Ok();
            });
        }

        [HttpPost("withdraw")]
        public IActionResult WithdrawFunds([FromBody] WithdrawRequest request)
        {
            return HandleRequest(() =>
            {
                atmService.WithdrawFunds(request.Amount, request.AccountName);
                return Ok();
            });
        }

        [HttpPost("transfer")]
        public IActionResult TransferFunds([FromBody] TransferRequest request)
        {
            return HandleRequest(() =>
            {
                atmService.TransferFunds(request.Amount, request.FromAccount, request.ToAccount);
                return Ok();
            });
        }

        [HttpPost("register")]
        public IActionResult RegisterAccounts([FromBody] RegisterAccountRequest request)
        {
            return HandleRequest(() =>
            {
                atmService.RegisterAccountData(request.Accounts);
                return Ok();
            });
        }

        private IActionResult HandleRequest(Func<IActionResult> action)
        {
            try
            {
                return action.Invoke();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
