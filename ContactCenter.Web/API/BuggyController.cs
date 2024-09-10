﻿using ContactCenter.Data;
using EDRSM.API.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EDRSM.API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly EDRSMContext _context;
        public BuggyController(EDRSMContext context)
        {
            _context = context;
        }

        [HttpGet("testauth")]
        [Authorize]
        public ActionResult<string> GetAuthyString()
        {
            return "This is an auth string";
        }

        //[HttpGet("notfound")]
        //public ActionResult GetNotFoundRequest()
        //{
        //    var thing = _context.EdrsmUsers.Find(42);

        //    if (thing == null) return NotFound(new ApiResponse(404));

        //    return Ok();
        //}

        //[HttpGet("servererror")]
        //public ActionResult GetServerError()
        //{
        //    var thing = _context.EdrsmUsers.Find(42);

        //    var thingToReturn = thing.ToString();

        //    return Ok();
        //}

        //[HttpGet("badrequest")]
        //public ActionResult GetBadRequest()
        //{
        //    return BadRequest(new ApiResponse(400));
        //}

        //[HttpGet("badrequest/{id}")]
        //public ActionResult GetNotFoundRequest(int id)
        //{
        //    return Ok();
        //}
    }
}
