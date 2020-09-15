using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateXml();
            QueryXml();

            //var manufacturers = ProcessManufacturers("manufacturers.csv");

            ////var query = cars.OrderByDescending(car => car.Combined)
            ////                .ThenBy(car => car.Name);

            //var query =
            //    from car in cars
            //    group car by car.Manufacturer into carGroup
            //    select new
            //    {
            //        Name = carGroup.Key,
            //        Max = carGroup.Max(c => c.Combined),
            //        Min = carGroup.Min(c => c.Combined),
            //        Average = carGroup.Average(c => c.Combined)
            //    } into result
            //    orderby result.Max descending
            //    select result;


            //var query2 = cars.GroupBy(c => c.Manufacturer)
            //    .Select(g =>
            //    {
            //        var results = g.Aggregate(new CarStatistics(), (acc, car) => acc.Accumulate(car), acc => acc.Compute());
            //        return new
            //        {
            //            Name = g.Key,
            //            Avg = results.Average,
            //            Min = results.Min,
            //            Max = results.Max
            //        };
            //    })
            //    .OrderByDescending(r => r.Max);

            ////var top = cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
            ////    .OrderByDescending(c => c.Combined)
            ////    .ThenBy(c => c.Name)
            ////    .Select(c => c)
            ////    .First();

            ////var result = cars.SelectMany(c => c.Name)
            ////    .OrderBy(c => c);

            ////foreach (var character in result)
            ////{
            ////    Console.WriteLine(character);
            ////}

            ////var someString = "Scott";
            ////IEnumerable<char> characters = "Scott";



            ////Console.WriteLine(top.Name);

            //foreach (var result in query)
            //{
            //    Console.WriteLine($"{result.Name}");
            //    Console.WriteLine($"\t Max: {result.Max}, Min: {result.Min}, Average: {result.Average}");
            //}
        }

        private static void QueryXml()
        {

            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";
            // Does not stream
            var document = XDocument.Load("fuel.xml");

            // Gets the first child element with the specified name.
            // THen gets all the elemetns in Cars
            var query =
                from element in document.Element(ns + "Cars").Elements(ex + "Car")
                    // get only elemetns that have MBW
                where element.Attribute("Manufacturer")?.Value == "BMW"
                select element.Attribute("Name").Value;

            foreach (var name in query)
            {
                Console.WriteLine(name);
            }
                
        }

        private static void CreateXml()
        {

            // Convert string to namespace
            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";

            var records = ProcessCars("fuel.csv");

            var document = new XDocument();
            var cars = new XElement(ns + "Cars",
                from record in records
                select new XElement(ex + "Car",
                    new XAttribute("Name", record.Name),
                    new XAttribute("Combined", record.Combined),
                    new XAttribute("Manufacturer", record.Manufacturer))
                );

            // Prefix
            cars.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));
            document.Add(cars);
            document.Save("fuel.xml");
        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            return File.ReadAllLines(path)
                .Where(l => l.Length > 1)
                .Select(l =>
                {
                    var columns = l.Split(",");
                    return new Manufacturer
                    {
                        Name = columns[0],
                        Headquarters = columns[1],
                        Year = int.Parse(columns[2])
                    };
                })
                .ToList();

        }

        private static List<Car> ProcessCars(string path)
        {
            // Open file
            //return File.ReadAllLines(path)
            //    .Where(line => line.Length > 1)
            //    // Skip the headers
            //    .Skip(1)
            //    .Select(Car.ParseFromCsv)
            //    .ToList();

            return File.ReadAllLines(path)
                .Skip(1)
                .Where(l => l.Length > 1)
                .ToCar()
                .ToList();
        }

        private static Car TransformToCar(string arg1, int arg2)
        {
            throw new NotImplementedException();
        }

    }

    public class CarStatistics
    {
        public CarStatistics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;
        }

        public int Max { get; set; }
        public int Min { get; set; }
        public int Average { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }

        public CarStatistics Accumulate(Car car)
        {
            Total += car.Combined;
            Count += 1;
            Max = Math.Max(Max, car.Combined);
            Min = Math.Min(Max, car.Combined);
            return this;
        }

        public CarStatistics Compute()
        {
            Average = Total / Count;
            return this;
        }
    }
    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(",");

                yield return new Car
                {
                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])
                };
            }
        }
    }
}
