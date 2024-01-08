using Common.Utils.Helpers;
using Common.Utils.Resources;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Bank;
using TopMai.Domain.DTO.Transactions.HistoricalTransactions;
using TopMai.Domain.Services.Payments.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Payments
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class BankAccountController : ControllerBase
    {
        #region Attributes
        private readonly IBankAccountService _bankAccountService;
        #endregion

        #region Builder
        public BankAccountController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }
        #endregion

        #region Services
        [HttpPost("getAll")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Get()
        {
            return Ok(this._bankAccountService.GetAll());
        }



        [HttpPost("create")]
        public ActionResult Post([FromBody] BankAccount bankAccount)
        {
            var res = _bankAccountService.Post(bankAccount);
            if (res.Result.GetType() == typeof(BankAccount))
                return Ok(new { id = bankAccount.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("defaultBankAccount")]
        public async Task<IActionResult> DefaultBankAccount(Guid idBankAccount)
        {
            IActionResult action;

            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            bool result = await _bankAccountService.DefaultBankAccount(Guid.Parse(idUser), idBankAccount);
            var response = new ResponseDto()
            {
                IsSuccess = result,
                Message = result ? GeneralMessages.ItemUpdated : GeneralMessages.ItemNoUpdated,
                Error = result ? string.Empty : GeneralMessages.ItemNoUpdated,
                Result = result,
            };

            if (result)
                action = Ok(response);
            else
                action = BadRequest(response);

            return action;
        }

        [HttpGet("getActualBankAccount")]
        public async Task<IActionResult> GetActualBankAccount()
        {
            ConsultBankAccountDto result = await _bankAccountService.GetActualBankAccount();
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Error = string.Empty,
                Result = result
            };

            return Ok(response);
        }

        [HttpPost("getByUser")]

        public ActionResult GetByUser([FromBody] IdRequest idRequest)
        {
            var res = _bankAccountService.GetByUser(idRequest.id);
            if (res.GetType() == typeof(List<BankAccount>))
                return Ok(res);

            return BadRequest(res);
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(Guid idBankAccount)
        {
            IActionResult action;

            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            bool result = await _bankAccountService.Delete(Guid.Parse(idUser), idBankAccount);
            var response = new ResponseDto()
            {
                IsSuccess = result,
                Message = result ? GeneralMessages.ItemDeleted : GeneralMessages.ItemNoDeleted,
                Error = result ? string.Empty : GeneralMessages.ItemNoDeleted,
                Result = result,
            };

            if (result)
                action = Ok(response);
            else
                action = BadRequest(response);

            return action;
        }
        #endregion
    }
}
