using Common.Utils.Helpers;
using Common.Utils.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Transactions;
using TopMai.Domain.Services.Transactions;
using TopMai.Domain.Services.Transactions.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Transactions
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class TransactionController : ControllerBase
    {
        private readonly ITransacionServices _transacionServices;

        public TransactionController(ITransacionServices transacionServices)
        {
            _transacionServices = transacionServices;
        }


        [HttpPost("NewTransaction")]
        public async Task<IActionResult> NewTransaction(AddTransaction_Dto add)
        {
            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            await _transacionServices.NewTransactions(add, Guid.Parse(idUser));
            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = "Transación Agregada Exitosamente!"
            });
        }

        [HttpGet("CancelTransaction")]
        public async Task<IActionResult> CancelTransaction(Guid idTransaction)
        {
            IActionResult actionResult;

            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            bool result = await _transacionServices.CancelTransaction(idTransaction, Guid.Parse(idUser));
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Message = result ? GeneralMessages.ItemProcess : GeneralMessages.ItemNotProcess,
                Error = result ? GeneralMessages.ItemProcess : GeneralMessages.ItemNotProcess,
                Result = result
            };

            if (result)
                actionResult = Ok(response);
            else
                actionResult = BadRequest(response);

            return actionResult;
        }

        [HttpGet("GetAllTransactionsByWallet")]
        public IActionResult GetAllTransactionsByWallet(Guid idWallet)
        {
            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            var result = _transacionServices.GetAllTransactionsByWallet(idWallet, Guid.Parse(idUser));
            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = result
            });
        }

        [HttpGet("GetByIdTransactionsByWallet")]
        public IActionResult GetByIdTransactionsByWallet(Guid idTransaction)
        {
            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            var result = _transacionServices.GetByIdTransactionsByWallet(idTransaction);
            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = result
            });
        }
    }
}
