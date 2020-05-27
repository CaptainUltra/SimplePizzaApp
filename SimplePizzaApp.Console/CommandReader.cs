using Microsoft.EntityFrameworkCore;
using SimpleOrderApp.Console;
using SimplePizzaApp.Data;
using SimplePizzaApp.Models;
using SimplePizzaApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SimplePizzaApp.Console
{
    /// <summary>
    ///  Handles input in the console interface.
    /// </summary>
    class CommandReader
    {
        private IIngredientService ingredientService; 
        private IPizzaService pizzaService;
        private IOrderService orderService;

        /// <summary>
        ///  Initialize the class and start command input.
        /// </summary>
        public CommandReader()
        {
            var options = new DbContextOptionsBuilder<SimplePizzaAppDbContext>().UseMySql(Data.Configuration.ConnectionString).Options;
            var context = new SimplePizzaAppDbContext(options);
            this.ingredientService = new IngredientService(context);
            this.pizzaService = new PizzaService(context);
            this.orderService = new OrderService(context);

            try
            {
                Input();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Грешка!");
                System.Console.WriteLine(ex.Message);
                System.Console.WriteLine("Продължаване? (1 - да; 0 - не)");
                var option = int.Parse(System.Console.ReadLine());
                if (option == 0) return;
                Input();
            }
        }
        /// <summary>
        ///  Print gudie and start parsing input commands.
        /// </summary>
        private void Input()
        {
            System.Console.WriteLine(new string('-', 60));
            System.Console.WriteLine("Конзолен интерфейс");
            System.Console.WriteLine("Просто пица приложение");
            System.Console.WriteLine(new string('-', 60));
            System.Console.WriteLine("Въвеждайте командите си във вид: <обект> <действие>");
            System.Console.WriteLine("Въжможни обекти: ingredient, pizza, order");
            System.Console.WriteLine("Въжможни действия: create, list, show, update, delete");
            System.Console.WriteLine("Команда за изход: exit");
            System.Console.WriteLine(new string('-', 60));

            
            var input = System.Console.ReadLine();
            while(input != "exit")
            {
                var command = input.Split();
                var objectString = command[0];
                var action = command[1];
                ParseObjectCommand(objectString, action);
                System.Console.WriteLine();
                input = System.Console.ReadLine();
            }
        }

        /// <summary>
        ///  Determines object type.
        /// </summary>
        /// <param name="objectString">String to be processed.</param>
        /// <param name="action">Action string to be passed on to the appropriate handler.</param>
        private void ParseObjectCommand(string objectString, string action)
        {
            switch (objectString)
            {
                case "ingredient":
                    {
                        ParseIngredientCommand(action);
                        break;
                    }
                case "pizza":
                    {
                        ParsePizzaCommand(action);
                        break;
                    }
                case "order":
                    {
                        ParseOrderCommand(action);
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        ///  Determines action string and calls the appropriate function for Ingredient actions.
        /// </summary>
        private void ParseIngredientCommand(string command)
        {
            var display = new IngredientDisplay(this.ingredientService);
            switch (command)
            {
                case "create":
                    {
                        display.Create();
                        break;
                    }
                case "list":
                    {
                        display.List();
                        break;
                    }
                case "show":
                    {
                        display.Show();
                        break;
                    }
                case "update":
                    {
                        display.Update();
                        break;
                    }
                case "delete":
                    {
                        display.Delete();
                        break;
                    }
                default:
                    break;
            }
        }
        /// <summary>
        ///  Determines action string and calls the appropriate function for Pizza actions.
        /// </summary>
        private void ParsePizzaCommand(string command)
        {
            var display = new PizzaDisplay(this.pizzaService, this.ingredientService);
            switch (command)
            {
                case "create":
                    {
                        display.Create();
                        break;
                    }
                case "list":
                    {
                        display.List();
                        break;
                    }
                case "show":
                    {
                        display.Show();
                        break;
                    }
                case "update":
                    {
                        display.Update();
                        break;
                    }
                case "delete":
                    {
                        display.Delete();
                        break;
                    }
                default:
                    break;
            }
        }
        /// <summary>
        ///  Determines action string and calls the appropriate function for Order actions.
        /// </summary>
        private void ParseOrderCommand(string command)
        {
            var display = new OrderDisplay(this.orderService, this.pizzaService);
            switch (command)
            {
                case "create":
                    {
                        display.Create();
                        break;
                    }
                case "list":
                    {
                        display.List();
                        break;
                    }
                case "show":
                    {
                        display.Show();
                        break;
                    }
                case "update":
                    {
                        display.Update();
                        break;
                    }
                case "delete":
                    {
                        display.Delete();
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
