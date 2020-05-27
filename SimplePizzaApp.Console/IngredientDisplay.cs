using SimplePizzaApp.Models;
using SimplePizzaApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePizzaApp.Console
{
    /// <summary>
    ///  Handles console input/output for Ingredient and communicates to the service.
    /// </summary>
    internal class IngredientDisplay
    {
        private IIngredientService service;

        /// <summary>
        ///  Initialize the class.
        /// </summary>
        /// <param name="service">Used to communicate to the business logic layer.</param>
        public IngredientDisplay(IIngredientService service)
        {
            this.service = service;
        }
        /// <summary>
        ///  Handles console IO for creating a record and passes the data to the service.
        /// </summary>
        public void Create()
        {
            System.Console.WriteLine("Добавяне на съставка:");
            System.Console.Write("Име:");
            var name = System.Console.ReadLine();
            this.service.Store(name);
            System.Console.WriteLine("Успешно добавяне на съставка!");
        }
        /// <summary>
        ///  Retrieves data from the service and outputs it.
        /// </summary>
        public void List()
        {
            System.Console.WriteLine("Всички съставки:");
            var ingredients = this.service.Index();
            foreach (var ingredient in ingredients)
            {
                System.Console.WriteLine($"№: {ingredient.Id} Име: {ingredient.Name}");
            }
        }
        /// <summary>
        ///  Handles console IO and gets data from the service for retrieving a record.
        /// </summary>
        public void Show()
        {
            System.Console.WriteLine("Извеждане на съставка:");
            System.Console.Write("Номер на съставката: ");
            var id = int.Parse(System.Console.ReadLine());
            var ingredient = this.service.Show(id);
            System.Console.WriteLine($"№: {ingredient.Id} Име: {ingredient.Name}");
        }
        /// <summary>
        ///  Handles console IO for updating a record and passes the data to the service.
        /// </summary>
        public void Update()
        {
            System.Console.WriteLine("Промяна на съставка:");
            System.Console.Write("Номер на съставката: ");
            var id = int.Parse(System.Console.ReadLine());
            System.Console.Write("Ново име: ");
            var name = System.Console.ReadLine();
            var ingredient = new Ingredient { Name = name };
            this.service.Update(id, ingredient);
            System.Console.WriteLine("Успешна промяна на съставката!");
        }
        /// <summary>
        ///  Handles console IO for deleting a record and passes the data to the service.
        /// </summary>
        public void Delete()
        {
            System.Console.WriteLine("Изтриване на съставка:");
            System.Console.Write("Номер на съставката: ");
            var id = int.Parse(System.Console.ReadLine());
            this.service.Delete(id);
            System.Console.WriteLine("Успешно изтривнае на съставката!");
        }
    }
}
