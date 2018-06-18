﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneTimeProgress.BussinessEntity;
using OneTimeProgress.BussinessLayer;

namespace OneTimeProgress.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff

        Bussines bussines = new Bussines();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (bussines.LoginValidator(loginModel))
            {
                return RedirectToAction("FlightsPage");
            }
            ViewBag.Message = "Sorry we dont find you";
            return View();
        }

        public ActionResult FlightsPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FlightsPage(string flightNumber)
        {
            string flightNuamber = flightNumber;
            return RedirectToAction("TaskDetails");
        }
        public ActionResult TaskDetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TaskDetails(string data)
        {
            return View();
        }
        public string InsertTasks()
        {
            return "Inserted into Task List Table";
        }
    }
}