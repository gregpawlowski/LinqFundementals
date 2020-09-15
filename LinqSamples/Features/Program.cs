using System;
using System.Collections.Generic;
using System.Linq;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, int> namedSquare = Square;

            // Named Method
            static int Square(int x)
            {
                return x * x;
            }

            //Lambda example
            Func<int, int> square = x => x * x;

            Console.WriteLine(namedSquare(3));
            Console.WriteLine(square(3));

            Func<int, int, int> add = (x, y) => x + y;
            Console.WriteLine(add(6,6));

            Action<int> write = x => Console.WriteLine(x);

            write(square(add(3, 5)));



            IEnumerable<Employee> developers = new Employee[]
            {
                new Employee { Id = 1, Name = "Greg" },
                new Employee { Id = 1, Name = "Scott" }
            };

            IEnumerable<Employee> sales = new List<Employee>()
            {
                new Employee { Id = 3, Name = "Alex"}
            };

            Console.WriteLine(sales.Count());

            foreach (var person in developers)
            {
                Console.WriteLine(person.Name);
            }

            IEnumerator<Employee> enumerator = developers.GetEnumerator();
            while(enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Name);
            }

            // Query syntax
            var query = from developer in developers
                        where developer.Name.Length == 4
                        orderby developer.Name
                        select developer.Name;

            // Equivelant method syntax
            var query2 = developers.Where(e => e.Name.Length == 4)
                                    .OrderBy(e => e.Name)
                                    .Select(e => e.Name);

            foreach (var employee in query2)
            {
                Console.WriteLine($"Employee: {employee}");
            }


            string[] cities = { "Boston", "Los Angeles", "Seattle", "London", "Hayderbad" };

            IEnumerable<string> filteredCities =
                from city in cities
                where city.StartsWith("L") && city.Length < 15
                orderby city
                select city;


        }
    }
}
