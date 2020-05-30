using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplePizzaApp.Models;
using SimplePizzaApp.Services;
using SimplePizzaApp.Web.Models.Pizza;

namespace SimplePizzaApp.Web.Controllers
{
    public class PizzaController : Controller
    {
        private IPizzaService service;
        private IIngredientService ingredientService;

        public PizzaController(IPizzaService service, IIngredientService ingredientService)
        {
            this.service = service;
            this.ingredientService = ingredientService;
        }

        // GET: Pizza
        public ActionResult Index()
        {
            var pizzaList = this.service.Index();
            var pizzas = new List<IndexPizzaViewModel>();
            foreach (var pizza in pizzaList)
            {
                pizzas.Add(new IndexPizzaViewModel 
                { 
                    Id = pizza.Id, 
                    Name = pizza.Name, 
                    Description = pizza.Description, 
                    Price = pizza.Price,
                    UpdatedAt = pizza.UpdatedAt 
                });
            }
            return View(pizzas);
        }

        // GET: Pizza/Details/5
        public ActionResult Details(int id)
        {
            var pizza = this.service.Show(id);
            var pizzaModel = new ShowPizzaViewModel
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Price = pizza.Price,
                Description = pizza.Description,
                Ingredients = pizza.Ingredients
            };
            return View(pizzaModel);
        }

        // GET: Pizza/Create
        public ActionResult Create()
        {
            var ingredients = this.ingredientService.Index();
            ViewBag.Ingredients = ingredients;

            return View();
        }

        // POST: Pizza/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            var ingredientsList = this.ingredientService.Index();
            ViewBag.Ingredients = ingredientsList;
            try
            {
                var ingredients = new List<Ingredient>();

                if (collection["selectedIngredients"].Count != 0)
                {
                    foreach (var ingredient in collection["selectedIngredients"])
                    {
                        var id = int.Parse(ingredient);
                        ingredients.Add(this.ingredientService.Show(id));
                    }
                }

                this.service.Store(collection["name"], collection["description"], decimal.Parse(collection["price"]), ingredients);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pizza/Edit/5
        public ActionResult Edit(int id)
        {
            var pizza = this.service.Show(id);
            PopulateSelectedIngredients(pizza);
            var pizzaModel = new EditPizzaViewModel 
            { 
                Name = pizza.Name,
                Description = pizza.Description,
                Price = pizza.Price 
            };
            return View(pizzaModel);
        }

        /// <summary>
        /// Populates a list of SelectedPizzaIngredientViewModel and puts it into the ViewBag. The list contains data about pizza's selected ingredients.
        /// </summary>
        /// <param name="pizza"></param>
        private void PopulateSelectedIngredients(Pizza pizza)
        {
            var allIngredients = this.ingredientService.Index();
            var pizzaIngredients = new HashSet<int>(pizza.Ingredients.Select(i => i.IngredientId));
            var viewModel = new List<SelectedPizzaIngredientViewModel>();
            foreach (var ingredient in allIngredients)
            {
                viewModel.Add(new SelectedPizzaIngredientViewModel
                {
                    Id = ingredient.Id,
                    Name = ingredient.Name,
                    Selected = pizzaIngredients.Contains(ingredient.Id)
                });
            }
            ViewBag.Ingredients = viewModel;
        }

        // POST: Pizza/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var ingredients = new List<IngredientPizza>();
                var name = collection["name"];
                var description = collection["description"];
                var price = decimal.Parse(collection["price"]);

                var pizza = new Pizza { Name = name, Description = description, Price = price };

                if (collection["selectedIngredients"].Count != 0)
                {
                    foreach (var ingredient in collection["selectedIngredients"])
                    {
                        var ingredientId = int.Parse(ingredient);
                        var ingredientModel = this.ingredientService.Show(ingredientId);
                        ingredients.Add(new IngredientPizza { Ingredient = ingredientModel, Pizza = pizza });
                    }
                }

                pizza.Ingredients = ingredients;
                this.service.Update(id, pizza);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pizza/Delete/5
        public ActionResult Delete(int id)
        {
            var pizza = this.service.Show(id);
            var pizzaModel = new DeletePizzaViewModel { Id = pizza.Id, Name = pizza.Name };

            return View(pizzaModel);
        }

        // POST: Pizza/Delete/5
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