using System;
using Kore.Data;
using Kore.Demos.ConsoleApp.Data.Domain;
using Kore.Demos.ConsoleApp.Infrastructure;
using Kore.Infrastructure;

namespace Kore.Demos.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            EngineContext.Default = new DemoEngine();
            EngineContext.Initialize(false);

            var repository = EngineContext.Current.Resolve<IRepository<Person>>();

            if (repository.Count() == 0)
            {
                repository.Insert(new Person
                {
                    FamilyName = "Doe",
                    GivenNames = "John"
                });
                repository.Insert(new Person
                {
                    FamilyName = "Doe",
                    GivenNames = "Jane"
                });
            }

            foreach (var person in repository.Find())
            {
                Console.WriteLine(person.GivenNames + " " + person.FamilyName);
            }

            Console.WriteLine("Done");

            AutofacRequestLifetimeHelper.Dispose();
            Console.ReadLine();
        }
    }
}