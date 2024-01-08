using AutoMapper;
using Common.Utils.Helpers;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Profiles;
using TopMai.Domain.Services.Profiles.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;
using Profile = Infraestructure.Entity.Entities.Profiles.Profile;

namespace TopMai.Controllers.Profiles
{
	[Route("api/[controller]")]
	[Authorize]
	[ApiController]
	[TypeFilter(typeof(CustomExceptionHandler))]
	public class ProfileController : ControllerBase
	{
		#region Attributes
		private readonly IProfileService _profileService;
		private readonly IMapper _mapper;
		private readonly IImageService _imageService;
		#endregion

		#region Builder
		public ProfileController(IProfileService profileService, IMapper mapper, IImageService imageService)
		{
			_profileService = profileService;
			_mapper = mapper;
			_imageService = imageService;
		}
		#endregion

		#region Services

		[HttpPost("getAll")]
		public ActionResult Get()
		{
			return Ok(this._profileService.GetAll());
		}

		[HttpGet("get")]
		[AllowAnonymous]
		public async Task<IActionResult> Get(Guid idProfile)
		{
			ConsultProfileDto result = await _profileService.Get(idProfile);
			ResponseDto response = new ResponseDto()
			{
				IsSuccess = true,
				Message = string.Empty,
				Error = string.Empty,
				Result = result,
			};

			return Ok(response);
		}
		
		[HttpPost("update")]
		public async Task<IActionResult> Put([FromBody] ConsultProfileDto request)
		{
			var profile = _mapper.Map<Profile>(request);

			var saved = await _profileService.Put(profile);

			return saved ? Ok(new { value = profile.Id.ToString() }) : BadRequest();
		}

		[HttpPost("connectToChat")]
		public IActionResult Connect([FromBody] ConnectionRequest connectionRequest)
		{
			var profile = _profileService.GetProfile((Guid)connectionRequest.id);
			profile.ConnectionId = connectionRequest.connectionId;

			_profileService.Put(profile);
			return Ok(new { value = "Conectado" });
		}

		[HttpPost("sendFriendRequest")]
		public ActionResult SendFriendRequest([FromBody] FromToRequest fromToRequest)
		{
			var res = _profileService.SendFriendRequest(fromToRequest.FromId, fromToRequest.ToId);
			if (res.GetType() == typeof(FriendRequest))
			{
				FriendRequest friendRequest = (FriendRequest)res;
				return Ok(new { value = friendRequest.Id });
			}
			return BadRequest(res);
		}

		[HttpPost("getAllFriendRequest")]
		public ActionResult GetAllFriendRequest([FromBody] IdRequest idRequest)
		{
			var res = _profileService.GetAllFriendRequest(idRequest.id);
			if (res.GetType() == typeof(List<FriendRequest>))
			{
				return Ok(res);
			}
			return BadRequest(res);
		}

		[HttpPost("getAllFriends")]
		public ActionResult GetAllFriends([FromBody] IdRequest idRequest)
		{
			var res = _profileService.GetAllFriends(idRequest.id);
			return Ok(res);
		}

		[HttpPost("getAllFriendsToInvite")]
		public ActionResult GetAllFriendsToInvite([FromBody] IdRequest idRequest)
		{
			var res = _profileService.GetAllFriendsToInvite(idRequest.id);

			return Ok(res);
		}


		[HttpPost("getSellerLevelByProfileId")]
		[AllowAnonymous]
		public ActionResult GetSellerLevelByProfileId([FromBody] IdRequest idRequest)
		{
			var res = _profileService.GetSellerLevelByProfileId(idRequest.id);
			if (res.GetType() == typeof(int))
			{
				return Ok(new { value = res });
			}

			return BadRequest(res);
		}

		[HttpPost("acceptFriendRequest")]
		public ActionResult AcceptFriendRequest([FromBody] IdRequest idRequest)
		{
			var res = _profileService.AcceptFriendRequest(idRequest.id);
			if (res.GetType() == typeof(FriendRequest))
			{
				return Ok(res);
			}

			return BadRequest(res);
		}

		[HttpPost("addPublicationToCart")]
		public ActionResult AddPublicationToCart([FromBody] CartRequest cartRequest)
		{
			if (cartRequest.publicationId == null)
				return BadRequest("PublicationId no puede ser nulo");

			var res = _profileService.AddToCart(cartRequest.id, (Guid)cartRequest.publicationId, cartRequest.amount);
			if (res.GetType() == typeof(CartPublication))
			{
				CartPublication cartPublication = (CartPublication)res;
				return Ok(new { value = cartPublication.Id });
			}

			return BadRequest(res);
		}

		[HttpPost("editCartPublication")]
		public ActionResult EditCartPublication([FromBody] CartRequest cartRequest)
		{
			var res = _profileService.EditCartPublication(cartRequest.id, cartRequest.amount);
			if (res.GetType() == typeof(CartPublication))
			{
				CartPublication cartPublication = (CartPublication)res;
				return Ok(new { value = cartPublication.Id });
			}

			return BadRequest(res);
		}

		[HttpPost("deleteCartPublication")]
		public ActionResult DeleteCartPublication([FromBody] IdRequest idRequest)
		{
			var res = _profileService.DeleteCartPublication(idRequest.id);
			if (res.GetType() == typeof(CartPublication))
			{
				return Ok(res);
			}

			return BadRequest(res);
		}

		[HttpPost("getCartByProfile")]
		public ActionResult GetCartByProfile([FromBody] IdRequest idRequest)
		{
			var res = _profileService.GetCart(idRequest.id);
			if (res.GetType() == typeof(Cart))
			{
				return Ok(res);
			}

			return BadRequest(res);
		}

		[HttpPost("search")]
		public ActionResult Search([FromBody] SearchRequest searchRequest)
		{
			var token = Request.Headers["Authorization"];
			string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);

			var res = _profileService.Search(searchRequest.query, Guid.Parse(idUser));
			if (res.GetType() == typeof(List<Profile>))
				return Ok(res);

			return BadRequest(res);
		}

		[HttpPost("searchContacts")]
		public ActionResult SearchContacts([FromBody] SearchRequest searchRequest)
		{
			var token = Request.Headers["Authorization"];
			string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);

			var res = _profileService.SearchContacts(searchRequest.query, Guid.Parse(idUser));
			if (res.GetType() == typeof(List<Contact>))
				return Ok(res);

			return BadRequest(res);
		}
		[HttpPost("delete")]
		public ActionResult Delete([FromBody] IdRequest idRequest)
		{
			var res = _profileService.Delete(idRequest.id);
			if (res.GetType() == typeof(Profile))
				return Ok(res);

			return BadRequest(res);
		}
		#endregion
	}
}
