using Authintication.Data.Interfaces;
using Authintication.DTOs;
using Authintication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherRepo publisherRepo;

        public PublisherController(IPublisherRepo publisherRepo)
        {
            this.publisherRepo = publisherRepo;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddPublisher(PublisherDto dto)
        {
            var publisher = new Publisher()
            {
                Name = dto.Name
            };
            var nawPublisher = publisherRepo.AddPublisher(publisher);
            return Created("", new { Message = "Publisher Add!", Publisher = nawPublisher });
        }

        [HttpGet("ListPublishers")]
        public IActionResult ListPublishers()
        {
            var publishers = publisherRepo.GetPublishers();

            return Ok(new { Message = "List Of All Publishers!", Publishers = publishers });
        }

        [HttpGet("GetPublisherById")]
        public IActionResult GetPublisherById(int id)
        {
            var publisher = publisherRepo.GetPublisherById(id);

            return Ok(new { Message = $"Publisher {id}", Publisher = publisher });
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePublisher(int id)
        {
            var publisher = publisherRepo.DeletePublisher(id);
            if(publisher == null) return NotFound(new { Message = $"Publisher ${id} Not Found!" });
            return Ok(new { Message = $"Publisher ${publisher.Name} Deleted!", Publisher = publisher });
        }


    }
}
