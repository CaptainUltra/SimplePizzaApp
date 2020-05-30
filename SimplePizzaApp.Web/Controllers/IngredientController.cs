using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplePizzaApp.Models;
using SimplePizzaApp.Services;
using SimplePizzaApp.Web.Models.Ingredient;

namespace SimplePizzaApp.Web.Controllers
{
    public class IngredientController : Controller
    {
        private IIngredientService service;
        public IngredientController(IIngredientService service)
        {
            this.service = service;
        }
        // GET: Ingredient
        public ActionResult Index()
        {
            var ingredientsList = new List<ShowIngredientViewModel>();
            foreach (var ingredient in this.service.Index())
            {
                ingredientsList.Add(new ShowIngredientViewModel { Id = ingredient.Id, Name = ingredient.Name, UpdatedAt = ingredient.UpdatedAt });
            }
            return View(ingredientsList);
        }

        // GET: Ingredient/Details/5
        public ActionResult Details(int id)
        {
            var ingredient = this.service.Show(id);
            var ingredientModel = new ShowIngredientViewModel { Id = ingredient.Id, Name = ingredient.Name, UpdatedAt = ingredient.UpdatedAt };

            return View(ingredientModel);
        }

        // GET: Ingredient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ingredient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                this.service.Store(collection["name"]);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Ingredient/Edit/5
        public ActionResult Edit(int id)
        {
            var ingredient = this.service.Show(id);
            var ingredientModel = new EditIngredientViewModel { Name = ingredient.Name };

            return View(ingredientModel);
        }

        // POST: Ingredient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var ingredeint = new Ingredient { Name = collection["name"] };
                this.service.Update(id, ingredeint);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Ingredient/Delete/5
        public ActionResult Delete(int id)
        {
            var ingredient = this.service.Show(id);
            var ingredientModel = new DeleteIngredientViewModel { Id = ingredient.Id, Name = ingredient.Name };

            return View(ingredientModel);
        }

        // POST: Ingredient/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                this.service.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}