using Common.Utils.Helpers;
using Common.Utils.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Transactions.RechargueWallet;
using TopMai.Domain.Services.Transactions.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Transactions
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class RechargueWalletController : ControllerBase
    {
        #region Attributes
        private readonly IRechargueWalletServices _rechargueWalletServices;
        #endregion

        #region Builder
        public RechargueWalletController(IRechargueWalletServices rechargueWalletServices)
        {
            _rechargueWalletServices = rechargueWalletServices;
        }
        #endregion

        #region Services

        [HttpGet("GetAllRechargueWallet")]
        [CustomRolFilterImplement(Roles.Admin)]
        public IActionResult GetAllRechargueWallet()
        {
            var result = _rechargueWalletServices.GetAllRechargueWallet();
            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = result
            });
        }

        [HttpGet("GetAllRechargueWalletByStatus")]
        [CustomRolFilterImplement(Roles.Admin)]
        public IActionResult GetAllRechargueWalletByStatus(int Status)
        {
            var result = _rechargueWalletServices.GetAllRechargueWalletByStatus(Status);
            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = result
            });
        }

        [HttpGet("GetAllRechargueByWallet")]
        public async Task<IActionResult> GetAllRechargueByWallet(Guid idWallet)
        {
            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            var result = await _rechargueWalletServices.GetAllRechargueByWallet(idWallet, Guid.Parse(idUser));
            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = result
            });
        }

        [HttpPost("GetPaymentReference")]
        public async Task<IActionResult> GetPaymentReference(ReferencePayment_Dto reference)
        {
            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            ResultReference_Dto result = await _rechargueWalletServices.GetPaymentReference(reference, Guid.Parse(idUser));
            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = result
            });
        }

        [HttpPost("ConfirmPaymentReference")]
        public async Task<IActionResult> ConfirmPaymentReference([FromForm] ConfirmPaymentReference_Dto confirm)
        {
            IActionResult actionResult;
            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            bool result = await _rechargueWalletServices.ConfirmPaymentReference(confirm, Guid.Parse(idUser));
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

        [HttpGet("CancelRechargueWalletByUser")]
        public async Task<IActionResult> CancelRechargueWalletByUser(Guid idRechargue)
        {
            IActionResult actionResult;
            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            bool result = await _rechargueWalletServices.CancelRechargueWalletByUser(idRechargue, Guid.Parse(idUser));
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

        [HttpGet("PaymentApproved")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> PaymentApproved(Guid idRechargue)
        {
            IActionResult actionResult;

            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            bool result = await _rechargueWalletServices.PaymentApproved(idRechargue, Guid.Parse(idUser));
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


        #endregion
    }
}
