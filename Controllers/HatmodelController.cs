﻿using Hattmakarens_system.Models;
using Hattmakarens_system.Repositories;
using Hattmakarens_system.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hattmakarens_system.Controllers
{
    public class HatmodelController : Controller
    {
        HatmodelRepository hatModelRepository = new HatmodelRepository();

        // GET: Hatmodel
        public ActionResult Hatmodel()
        {
            ViewBag.MaterialsToPickFrom = new Service.Material().GetSelectListMaterials();
            return View();
        }

        [HttpPost]
        public ActionResult Hatmodel(HatmodelViewModel hatmodel, IEnumerable<string> PickedMaterials, HttpPostedFileBase [] file) 
        {

            if (ModelState.IsValid) 
            { 

                var newHatmodel = new HatModels
                {
                    Name = hatmodel.Name,
                    Description = hatmodel.Description,
                    Price = hatmodel.Price,
                    Material = new List<MaterialModels>(),
                    Images = new List<ImageModels>()
                };

                var path = Server.MapPath("~/Images");
                var images = new Service.Image().AddImages(file, path);
                newHatmodel.Images = images;

                using (var context = new ApplicationDbContext())
                {
                    foreach (var material in PickedMaterials)
                    {
                        var id = int.Parse(material);
                        var aMaterial = context.Material.ToList().FirstOrDefault(m => m.Id == id);
                        newHatmodel.Material.Add(aMaterial);
                    }
                    context.HatModels.Add(newHatmodel);
                    context.SaveChanges();
                }

                return RedirectToAction("Hatmodel", "Hatmodel");
            }
            else
            {
                ViewBag.MaterialsToPickFrom = new Service.Material().GetSelectListMaterials();
                return View(hatmodel);
            }
        }

        public ActionResult SearchHatModel(int orderId, string customerEmail)
        {
            var hatModels = hatModelRepository.GetAllHatmodels();
            List<HatmodelViewModel> hatmodelViewModels = new List<HatmodelViewModel>();
            foreach(var hatModel in hatModels)
            {
                HatmodelViewModel newHatmodelViewModel = new HatmodelViewModel
                {
                    Id = hatModel.Id,
                    Name = hatModel.Name,
                    Description = hatModel.Description,
                    Price = hatModel.Price,
                    OrderId = orderId,
                    CustomerEmail = customerEmail
                };
                hatmodelViewModels.Add(newHatmodelViewModel);
            }
            return View(hatmodelViewModels);
        }
    }
}