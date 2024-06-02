using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TelecomShop.ErrorHandlers;
using TelecomShop.Models;
using TelecomShop.Services;

namespace TelecomShop.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class Superpowers : ControllerBase
    {
        
        private readonly ILogger<Auth> _logger;

        public Superpowers(ILogger<Auth> logger)
        {
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public Superpower[] GetAll()
        {
            return [new Superpower { Name = "1" }, new Superpower { Name = "2" }];
        }


        [Authorize]
        [HttpGet]
        public Superpower[] GetCurrent()
        {
            return [new Superpower { Name = "1" }, new Superpower { Name = "2" }];
        }
    }
}
