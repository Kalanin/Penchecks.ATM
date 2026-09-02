using ATM.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace ATM.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ATMController(IATMService atmService) : ControllerBase
    {
        private readonly IATMService _atmService = atmService;
    }
}
