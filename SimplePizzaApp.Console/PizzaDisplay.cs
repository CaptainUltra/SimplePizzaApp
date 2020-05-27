using SimplePizzaApp.Models;
using SimplePizzaApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplePizzaApp.Console
{
    /// <summary>
    ///  Handles console input/output for Pizza and communicates to the service.
    /// </summary>
    class PizzaDisplay
    {
        private IPizzaService service;
        private IIngredientService ingredientService;
        /// <summary>
        ///  Initialize the class.
        /// </summary>
        /// <param name="service">Used to communicate to the business logic layer.</param>
        public PizzaDisplay(IPizzaService service, IIngredientService ingredientService)
        {
            this.service = service;
            this.ingredientService = ingredientService;
        }

        /// <summary>
        ///  Handles console IO for creating a record and passes the data to the service.
        /// </summary>
        public void Create()
        {
            System.Console.WriteLine("Добавяне на пица:");
            System.Console.Write("Име: ");
            var name = System.Console.ReadLine();
            System.Console.Write("Описание: ");
            var description = System.Console.ReadLine();
            System.Console.Write("Цена: ");
            var price = decimal.Parse(System.Console.ReadLine());

            System.Console.WriteLine("Добавяне на съставки? (1 - да; 0 - не)");
            var option = int.Parse(System.Console.ReadLine());
            if (option == 0)
            {
                this.service.Store(name, description, price);
                System.Console.WriteLine("Успешно добавяне на пица!");
                return;
            }

            System.Console.WriteLine("Напишете номерата на съставките, които искате да добавите, разделени с интервал: ");
            var ingredientsIds = System.Console.ReadLine().Split().Select(int.Parse);
            List<Ingredient> ingredients = new List<Ingredient>();
            foreach (var id in ingredientsIds)
            {
                var ingredient = this.ingredientService.Show(id);
                ingredients.Add(ingredient);
            }
            this.service.Store(name, description, price, ingredients);
            System.Console.WriteLine("Успешно добавяне на пица!");
        }
        /// <summary>
        ///  Retrieves data from the service and outputs it.
        /// </summary>
        public void List()
        {
            System.Console.WriteLine("Всички пици:");
            var pizzas = this.service.Index();
            foreach (var pizza in pizzas)
            {
                System.Console.WriteLine($"№: {pizza.Id} Име: {pizza.Name} Цена: {pizza.Price:F2} Описание: {pizza.Description}");
            }
        }
        /// <summary>
        ///  Handles console IO and gets data from the service for retrieving a record.
        /// </summary>
        public void Show()
        {
            System.Console.WriteLine("Извеждане на пица:");
            System.Console.Write("Номер на пицата: ");
            var id = int.Parse(System.Console.ReadLine());
            var pizza = this.service.Show(id);
            System.Console.WriteLine($"№: {pizza.Id} Име: {pizza.Name} Описание: {pizza.Description}");
            System.Console.WriteLine("Съставки:");
            foreach (var ingredient in pizza.Ingredients)
            {
                System.Console.WriteLine(ingredient.Ingredient.Name);
            }
        }
        /// <summary>
        ///  Handles console IO for updating a record and passes the data to the service.
        /// </summary>
        public void Update()
        {
            System.Console.WriteLine("Промяна на пица:");
            System.Console.Write("Номер на пицата: ");
            var id = int.Parse(System.Console.ReadLine());
            System.Console.Write("Ново име: ");
            var name = System.Console.ReadLine();
            System.Console.Write("Описание: ");
            var description = System.Console.ReadLine();
            System.Console.Write("Цена: ");
            var price = decimal.Parse(System.Console.ReadLine());

            System.Console.WriteLine("Промяна на съставки? (1 - да; 0 - не)");
            var option = int.Parse(System.Console.ReadLine());
            if (option == 0)
            {
                var existingIngredients = this.service.Show(id).Ingredients;
                var updatePizza = new Pizza { Name = name, Description = description, Price = price, Ingredients = existingIngredients };
                this.service.Update(id, updatePizza);
                System.Console.WriteLine("Успешна промяна на пицата!");
                return;
            }

            System.Console.WriteLine("Напишете номерата на съставките, които искате да добавите, разделени с интервал: ");
            var ingredientsIds = System.Console.ReadLine().Split().Select(int.Parse);

            List<IngredientPizza> ingredients = new List<IngredientPizza>();
            var pizza = new Pizza { Name = name, Description = description, Price = price };

            foreach (var ingredientId in ingredientsIds)
            {
                var ingredient = this.ingredientService.Show(ingredientId);
                ingredients.Add(new IngredientPizza { Ingredient = ingredient, Pizza = pizza });
            }
            pizza.Ingredients = ingredients;

            this.service.Update(id, pizza);
            System.Console.WriteLine("Успешна промяна на пицата!");
        }
        /// <summary>
        ///  Handles console IO for deleting a record and passes the data to the service.
        /// </summary>
        public void Delete()
        {
            System.Console.WriteLine("Изтриване на пица:");
            System.Console.Write("Номер на пицата: ");
            var id = int.Parse(System.Console.ReadLine());
            this.service.Delete(id);
            System.Console.WriteLine("Успешно изтривнае на пицата!");
        }
    }
}
