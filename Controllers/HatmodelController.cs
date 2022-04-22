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
        MaterialRepository MaterialRepository = new MaterialRepository();
        static List<ColorMaterialViewModel> TygMaterial = new Service.Material().GetTyg();
        static List<ColorMaterialViewModel> DekorationMaterial = new Service.Material().GetDecoration();
        static List<ColorMaterialViewModel> TrådMaterial = new Service.Material().GetTrad();

        // GET: Hatmodel
        public ActionResult Hatmodel(bool isAdded)
        {
            var model = new HatmodelViewModel()
            {
                TygMaterial = TygMaterial,
                DekorationMaterial = DekorationMaterial,
                TrådMaterial = TrådMaterial,
                IsAdded = isAdded
            };
            
            ViewBag.MaterialsToPickFrom = new Service.Material().GetSelectListMaterials();
            return View(model);
        }

        [HttpPost]
        public ActionResult Hatmodel(HatmodelViewModel hatmodel, HttpPostedFileBase[] file) 

        {
            hatmodel.TygMaterial = TygMaterial;
            hatmodel.TrådMaterial = TrådMaterial;
            hatmodel.DekorationMaterial = DekorationMaterial;
            //hatmodel.IsAdded = false;
            
            var valdMaterial = TygMaterial.Union(DekorationMaterial).Union(TrådMaterial).Where(s => s.State.Equals(true)).Select(s => s.MaterialId).ToList();

            if (valdMaterial.Count != 0)
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
                    if (file != null)
                    {
                        var path = Server.MapPath(@"~\NewFolder1");
                    var images = new Service.Image().AddImages(file, path);
                    newHatmodel.Images = images;
                    }
                    using (var context = new ApplicationDbContext())
                    {
                        foreach (var Id in valdMaterial)
                        {
                            newHatmodel.Material.Add(MaterialRepository.GetMaterial(Id));
                        }
                        context.HatModels.Add(newHatmodel);
                        context.SaveChanges();
                        TygMaterial = new Service.Material().ResetTygList(TygMaterial);
                        DekorationMaterial = new Service.Material().ResetDecorationList(DekorationMaterial);
                        TrådMaterial = new Service.Material().ResetTradList(TrådMaterial);
                    }
                    return RedirectToAction("Hatmodel", "Hatmodel", new { IsAdded = true });
                
                else
                {
                    ViewBag.MaterialsToPickFrom = new Service.Material().GetSelectListMaterials();
                    return View(hatmodel);
                }
            }
            else
            {
                TempData["materialError"] = "Du måste välja minst 1 material";
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
                    CustomerEmail = customerEmail,
                    Images = hatModel.Images
                };
                hatmodelViewModels.Add(newHatmodelViewModel);
            }
            return View(hatmodelViewModels);
        }

        public ActionResult PickMaterialModel(int Id)
        {
            foreach (var item in TygMaterial)
            {
                if (item.MaterialId.Equals(Id))
                {
                    item.State = !item.State;
                }
            }
            foreach (var item in DekorationMaterial)
            {
                if (item.MaterialId.Equals(Id))
                {
                    item.State = !item.State;
                }
            }
            foreach (var item in TrådMaterial)
            {
                if (item.MaterialId.Equals(Id))
                {
                    item.State = !item.State;
                }
            }

            return RedirectToAction("Hatmodel", new { IsAdded = false });
        }
    }
}