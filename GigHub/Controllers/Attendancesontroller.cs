﻿using GigHub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GigHub.Controllers
{
    public class AttendanceDto
    {
        public int GigId { get; set; }
    }

    [Authorize]
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext _context;

        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            var userId = User.Identity.GetUserId();
            if(_context.Attendances.Any(a => a.AttendeeId == User.Identity.GetUserId() && a.GigId == dto.GigId))
            {
                return BadRequest("The attendance already exists.");
            }
            
            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = User.Identity.GetUserId()
            };
            _context.Attendances.Add(attendance);
            _context.SaveChanges();

            return Ok();
        }
    }
}
