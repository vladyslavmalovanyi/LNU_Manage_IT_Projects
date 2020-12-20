using DAL.Moldels;
using Domain;
using Domain.Service;

using Domain.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LnuEventHub.Controllers
{

    [Route("api/[controller]")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class EventAsyncController : ControllerBase
    {
        private readonly EventServiceAsync<EventViewModel, Event> _EventServiceAsync;
        public EventAsyncController(EventServiceAsync<EventViewModel, Event> EventServiceAsync)
        {
            _EventServiceAsync = EventServiceAsync;
        }


        //get all
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<EventViewModel>> GetAll()
        {
            var items = await _EventServiceAsync.GetAll();
            return items;
        }


        [AllowAnonymous]
        [HttpGet("GetAllWithPagination")]
        public async Task<IActionResult> GetAllWithPagination([FromQuery] PageParameters parameters)
        {
            var items = await _EventServiceAsync.GetPagination(parameters);
            var metadata = new
            {
                items.TotalCount,
                items.PageSize,
                items.CurrentPage,
                items.TotalPages,
                items.HasNext,
                items.HasPrevious
            };
             Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            //log.LogInfo($"Returned {items.TotalCount} owners from database.");
            return Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("GetSearchWithPagination")]
        public async Task<IActionResult> GetSearchWithPagination([FromQuery]SearchParameters search,  [FromQuery] PageParameters parameters)
        {
            var items = await _EventServiceAsync.GetSearch(search, parameters);
            var metadata = new
            {
                items.TotalCount,
                items.PageSize,
                items.CurrentPage,
                items.TotalPages,
                items.HasNext,
                items.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            //log.LogInfo($"Returned {items.TotalCount} owners from database.");
            return Ok(items);
        }

        //get by predicate example
        //get all active by Eventname
        [Authorize]
        [HttpGet("GetActiveByName/{firstname}")]
        public async Task<IActionResult> GetActiveByName(string name)
        {
            var items = await _EventServiceAsync.Get(a => a.Name == name);
            return Ok(items);
        }


        [Authorize]
        [HttpGet("GetMyEvent/{firstname}")]
        public async Task<IActionResult> GetMyEvent()
        {
            var items = await _EventServiceAsync.Get(a => a.CreatorId == User.Identity.GetUserId<int>());
            return Ok(items);
        }
        //get one
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _EventServiceAsync.GetOne(id);
            if (item == null)
            {
                Log.Error("GetById({ ID}) NOT FOUND", id);
                return NotFound();
            }

            return Ok(item);
        }

        //add
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EventViewModel Event)
        {
            if (Event == null)
                return BadRequest();            
            var id = await _EventServiceAsync.Add(Event);
            return Created($"api/Event/{id}", id);  //HTTP201 Resource created
        }

        //update
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EventViewModel Event)
        {
            if (Event == null || Event.Id != id || User.Identity.GetUserId<int>()!= Event.CreatorId)
                return BadRequest();
            
            int retVal = await _EventServiceAsync.Update(Event);
            if (retVal == 0)
                return StatusCode(304);  //Not Modified
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //412 Precondition Failed  - concurrency
            else
                return Accepted(Event);
        }

        //delete
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id,  [FromBody] EventViewModel Event)
        {
            if(User.Identity.GetUserId<int>() != Event.CreatorId)
                return BadRequest();
            int retVal = await _EventServiceAsync.Remove(id);

            if (retVal == 0)
                return NotFound();  //Not Found 404
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //Precondition Failed  - concurrency
            else
                return NoContent();   	     //No Content 204
        }

    }
}
