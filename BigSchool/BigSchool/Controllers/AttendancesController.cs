﻿using BigSchool.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BigSchool.Controllers
{
    public class AttendancesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attend(Course attendanceDto)
        {
            var userID = User.Identity.GetUserId();
            DatabaseContext cx = new DatabaseContext();
            if( cx.Attendances.Any(p=> p.Attendee== userID && p.CourseId== attendanceDto.Id))
            {
                //return BadRequest("The attendance already exists!");
                cx.Attendances.Remove(cx.Attendances.SingleOrDefault(p => p.Attendee == userID && p.CourseId == attendanceDto.Id));
                cx.SaveChanges();
                return Ok("cancel");
            }
            var attendance = new Attendance() { CourseId = attendanceDto.Id, Attendee = User.Identity.GetUserId() };
            cx.Attendances.Add(attendance);
            cx.SaveChanges();
            return Ok();
        }
    }
}
