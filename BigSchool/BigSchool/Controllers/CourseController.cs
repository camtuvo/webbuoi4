﻿using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        
        public ActionResult Create()
        {
            DatabaseContext context = new DatabaseContext();
            Course obj = new Course();
            obj.ListCategory = context.Categories.ToList();
            return View(obj);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objCourse)
        {
            DatabaseContext context = new DatabaseContext();
            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                objCourse.ListCategory = context.Categories.ToList();
                return View("Create", objCourse);
            }
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;

            context.Courses.Add(objCourse);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        //Tạo Action Attending
        public ActionResult Attending()
        {
            DatabaseContext context = new DatabaseContext();
            ApplicationUser curr = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var list = context.Attendances.Where(p => p.Attendee == curr.Id).ToList();
            var course = new List<Course>();
            foreach (Attendance t in list)
            {
                Course obj = t.Course;
                obj.LecturerName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                    .FindById(obj.LecturerId).Name;
                course.Add(obj);
            }
            return View(course);
        }
        public ActionResult Mine()
        {
            ApplicationUser curr = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            DatabaseContext context = new DatabaseContext();
            var course = context.Courses.Where(c => c.LecturerId == curr.Id && c.DateTime > DateTime.Now).ToList();
            foreach (Course i in course)
            {
                i.LecturerName = curr.Name;
            }
            return View(course);
        }
    }
}