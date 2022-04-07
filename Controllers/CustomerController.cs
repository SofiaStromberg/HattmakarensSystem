﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hattmakarens_system.Models;
using Hattmakarens_system.Repositories;
using Hattmakarens_system.ViewModels;

namespace Hattmakarens_system.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult AddCustomer()
        {
            return View();
        }
        [HttpPost]
         public ActionResult AddCustomer(CostumerViewModel customerViewModel)
        {
            try
            {
                var cusRepo = new CustomerRepository();
                var customer = new CustomerModels
                {
                    Adress = customerViewModel.Adress,
                    Name = customerViewModel.Name,
                    Email = customerViewModel.Email,
                    Comment = customerViewModel.Comment,
                    Phone = customerViewModel.Phone
                };
                cusRepo.SaveCostumer(customer);
                return View();
            }
            catch
            {
                return View("Error");
            }
        }
        public ActionResult ChangeCustomer(int id)
        {
            var showCustomerInfo = new Service.Costumer().GetCustomerInfo(id);
            return View(showCustomerInfo);
        }
        [HttpPost]
        public ActionResult ChangeCustomer(CostumerViewModel model)
        {
            
            var status = new Service.Costumer().EditCustomerInfo(model);
            if(status == true)
            {
                return RedirectToAction("DisplayCustomer");
            }
            else
            {
                return View();
            }

                
        }

        public ActionResult DisplayCustomer()
        {
            int id = 1;
            var showCustomerInfo = new Service.Costumer().GetCustomerInfo(id);
            return View(showCustomerInfo);
        }

        public ActionResult DeleteCustomer(int id)
        {
            if(id != 0)
            {
                new CustomerRepository().DeleteCostumer(id);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Debug.WriteLine("Något har gått fel med id´t");
                return View();
            }
            
        }
    }
}