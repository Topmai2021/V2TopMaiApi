using Common.Utils.Helpers;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.Json;
using TopMai.Domain.DTO;
using TopMai.Domain.Hubs;
using TopMai.Domain.Services.Payments.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Payments
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class WalletController : ControllerBase
    {
        #region Attributes
        private readonly IWalletService _walletService;
        private readonly IDevolutionService _devolutionService;

        private readonly IHubContext<MessageHub> _messageHub;
        #endregion

        #region Builder
        public WalletController(IWalletService walletService, IDevolutionService devolutionService)
        {
            _walletService = walletService;
            _devolutionService = devolutionService;

            
        }
        #endregion

        #region Services

        [HttpGet("getBalance")]
        public ActionResult GetBalance(Guid idWallet)
        {
            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            float result = _walletService.GetBalance(idWallet, Guid.Parse(idUser));

            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = result
            });
        }

        [HttpGet("records")]
public IActionResult GetAllWalletRecordsWithProfiles()
{
    try
    {
        var walletRecords = _walletService.GetAllWalletRecordsWithProfiles();
        return Ok(walletRecords);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}


        [HttpPost("acreditPayments")]
        public async Task<IActionResult> AcreditPayments()
        {
            try
            {
                var res = _walletService.AcreditPayments();

                // automatic states transitions for devolutions
                await _devolutionService.CheckDevolutionStatus();
                return Ok(new { value = res });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("transfer")]
        public async Task<ActionResult> Transfer([FromBody] TransferRequest transferRequest)
        {
            var res = await _walletService.Transfer(transferRequest.idFrom, transferRequest.idTo, transferRequest.amount);
            return Ok(res);
        }

        [HttpPost("createWalletPin")]
        public ActionResult CreateWalletPin([FromBody] WalletPinRequest walletPinRequest)
        {
            var res = _walletService.CreateWalletPin(walletPinRequest.userId, walletPinRequest.pin);
            if (res.GetType() == typeof(Wallet))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("hasPin")]
        public ActionResult HasPin([FromBody] IdRequest idRequest)
        {
            var res = _walletService.HasPin(idRequest.id);
            if (res.GetType() == typeof(bool))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("validatePin")]
        public ActionResult ValidatePin([FromBody] WalletPinRequest walletPinRequest)
        {
            var res = _walletService.ValidatePin(walletPinRequest.userId, walletPinRequest.pin);
            if (res.GetType() == typeof(Wallet))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("changePin")]
        public ActionResult ChangePin([FromBody] WalletPinRequest walletPinRequest)
        {
            if (walletPinRequest.oldPin == null)
            {
                return BadRequest(new { error = "Debe ingresar el pin anterior" });
            }
            var res = _walletService.ChangePin(walletPinRequest.userId, (int)walletPinRequest.oldPin, walletPinRequest.pin);
            if (res.GetType() == typeof(Wallet))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("recoverPin")]
        public ActionResult RecoverPin([FromBody] WalletPinRequest walletPinRequest)
        {
            var res = _walletService.RecoverPin(walletPinRequest.userId, walletPinRequest.pin);
            if (res.GetType() == typeof(Wallet))
                return Ok(res);

            return BadRequest(res);
        }

        //[HttpPost("conekta")]
        //public ActionResult Conekta([FromBody] JsonElement jsonElement)
        //{
        //    try
        //    {

        //        var json = jsonElement.ToString();

        //        var res = _walletService.Conekta(json);
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        [HttpPost("getPendingAmountByUserId")]
        public ActionResult GetPendingAmountByUserId([FromBody] IdRequest idRequest)
        {
            var res = _walletService.getPendingAmountByUserId(idRequest.id);
            if (res.GetType() == typeof(float))
                return Ok(res);

            return BadRequest(res);
        }



        [HttpPost("getTotalBalance")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult GetTotalBalance()
        {
            try
            {
                var res = _walletService.getTotalBalance();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost("payWithCard")]
        public ActionResult PayWithCard([FromBody] PayCardRequest payCardRequest)
        {
            try
            {
                var res = _walletService.PayWithCard((Guid)payCardRequest.cardId, payCardRequest.profileId, payCardRequest.total, (string)payCardRequest.deviceSessionId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("payInStore")]
        public ActionResult PayInStore([FromBody] PayCardRequest payInStoreRequest)
        {
            try
            {
                var res = _walletService.PayInStore(payInStoreRequest.profileId, payInStoreRequest.total);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("openPayWebhook")]
        [AllowAnonymous]
        public ActionResult OpenPayWebhook([FromBody] JsonElement request)
        {
            var json = request.ToString();

            return Ok(_walletService.OpenPayWebHook(json));
        }


        [HttpGet("checkout")]
        public ContentResult Checkout(string checkoutRequestId)
        {
            string html = @"<html>
    <head>
        <meta name=""viewport"" content=""user - scalable = no, width = device - width, initial - scale = 1"">


                      <title> Checkout </title>
  
          <script type = ""text/javascript"" src = ""https://pay.conekta.com/v1.0/js/conekta-checkout.min.js"" > </script>  
               </head>
               <body style=""zoom:200%;"">
   
               <div id = ""conektaIframeContainer"" style = ""height: 200%; max-width:100%""> </div>
      
                      <script type = ""text/javascript"" >
                           window.ConektaCheckoutComponents.Integration({
            targetIFrame: ""#conektaIframeContainer"",
                        checkoutRequestId: ""#checkoutRequestId#"", // checkout request id
               publicKey: ""key_twRutnxNxtE9Ss6JgpwtEA"",
                        options: { },
                        styles: { },
                        onFinalizePayment: function(event) {
            console.log(event);
        }
    })
                    </script>
            </body>
</html>";

            html = html.Replace("#checkoutRequestId#", checkoutRequestId);
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = html
            };

        }
        #endregion
    }
}
