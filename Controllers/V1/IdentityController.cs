using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PostChan.Services;
using PostChan.Contracts.V1;
using PostChan.Contracts.V1.Requests;
using PostChan.Contracts.V1.Responses;

namespace PostChan.Controllers.V1
{
    public class IdentityController: Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid) //is email model state valid
            {
                return BadRequest(new AuthFailResponse
                {
                    Errors = ModelState.Values.SelectMany(x => 
                    x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);
            if(!authResponse.Succes)
            {
                return BadRequest(new AuthFailResponse
                {
                    Errors = authResponse.Errors
                });
            }
            return Ok(new AuthSuccesResponse 
            {
                Token = authResponse.Token
            });
        }


        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);
            if (!authResponse.Succes)
            {
                return BadRequest(new AuthFailResponse
                {
                    Errors = authResponse.Errors
                });
            }
            return Ok(new AuthSuccesResponse
            {
                Token = authResponse.Token
            });
        }
    }
}
