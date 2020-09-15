# What is LINQ?
LINQ is used to work with data. 

In the past to access data you had to use different APIs to work with data.

For IN memory Objects, you'd have to use the different generic methods provided on object to access.
For SQL there was an API created called ADO.net
For XML there was a set called Xpath/ESLT

LINQ gives you strongly typed compiled time checks queries on all of these.


LINQ can use providers to query - Objexts, MongoDB, CSV Files, File System, SQL Databases, HL7/XML, JSON.


# LINQ and CSharp
CSharp features that make Linq work

## IEnumerable
Both List and Arrays implent and interface IEnumerable<T>, this is why you can foreach over both types.
They have a method called GetEnumerator.

All Linq queries operate over this interface and one other one we'll talk about later.

IEnumrable can be used to hide a lot of structures, it doesnt matter what the implementation is. This is why many classes work against IEnumarble of T and it can go to database go someplace else and MoveNext().

Linq uses extension methods on IEnumerable to add a lot of different operations.

## Extension Methods
Have to be static
Have to have at his keyword as parameter - tells the compiler it's an extension method
They type passed into the parameter is the type that will be extended, in this case string
```C#
public static class StringExtensions
{
    // Extension method cause of this and string extension
    static public double ToDouble(this string data)
    {
        double result = double.Parse(data);
        return resut.
    }
}
```

```C#
// Now can be used

string text = "43.45"
double data = text.ToDouble();
```

Behind the scenes the c# compiler is simply calling toDouble and passing in the text to it.

Linq uses extensions heavily.

* Extension methods can extend any time, Interface, Sealed Class, or even object so that it is inherited everywhere
* Cannot hide or chagne already implemetned methods.
* Namespace is important as it will ahve to be imported with the using statement if it's not in the same namespace
* If two same extensions are brought in, compiler will throw an error. So you can use explicit typing to tell the namespace or remove the confliciting using.

## Understanding Lambda Expressions
Instead of creating a whole function you can create a lambda exprssion. A lambda expression creates an annonymous delegate function. It was introduced along with linq so that the syntax looks clean. It creates an annonymous delegate.


Named method
```C#
IEnumerable<string> filteredList = cities.Where(StartsWith);

public bool StartsWith(string name)
{
    return name.StrtsWith("L");
}
```

Annonymous Method:
```C#
IEnumerable<string> filteredList = cities.Where(delegate(string s) { return s.StartsWith("L")});
```

Lambda
```C#
IEnumerable<string> filteredList = cities.Where(s => s.StartsWith("L"));
```

* Lambda expresssions use {} for multi-line expressions, if {} are used then you have to use the return keyword.
* Type doesn't have to be specified becuase you know what it's being run on, they are optional.


### Func Types and Action
Funct types in signature:
Func type was introduced as an easy way to work with delegates. Func can take from one to 17 generic paramanters, the last parameter is the return type.

So to create a funct type:
```C#
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
```

Action types are just like Func types expect they do not return anything.

```C#
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
```

## Using var for implicit typing
var came along with Linq, it infers the type, so no need to specify the type explcitly.

var cleans up code, it's only used for local variables, cannto be used for parameter definitions.

var needs an assignment, so the var needs to be initialized.

```C#
var name = "Scott";
var x = 3.0;
var y = 2;
var z = x * y;

// all print True
Console.WriteLine(name is string);
Console.WriteLine(x is double);
Console.WriteLine(y is int);
Console.WriteLine(z is double);
```

## Query vs Method syntax
Linq has query syntax adn method syntax.


### Query Syntax
* Query syntax starts with from
* Alwasys has to finish with select (What you want to return) or gorup


```C#
string[] cities = { "Boston", "Los Angeles", "Seattle", "London", "Hayderbad" };

IEnumerable<string> filteredCities =
    from city in cities
    where city.StartsWith("L") && city.Length < 15
    orderby city
    select city;
```

In SQL the select comes first but in Query syntax it comes last. This is because the first part (from) specifies the collection so that the environment knows what you are querying so that you can have intellisense thorught your query. In SQL the slect clause is the last thing the query language figures out anyway it's just that it's ass backwards. 

Most queryies can be written qeury or method syntax but sometimes you must use method syntax because every operator is not available in query syntax.


Here we can return only the string Name from the object.
Behind the scenes C# will create a query syntax taht will invoke the same extension methods.
```C#
      var query = from developer in developers
                        where developer.Name.Length == 4
                        orderby developer.Name
                        select developer.Name;
```

Equivelant
```C#
            // Query syntax
            var query = from developer in developers
                        where developer.Name.Length == 4
                        orderby developer.Name descending
                        select developer.Name;

            // Equivelant method syntax
            var query2 = developers.Where(e => e.Name.Length == 4)
                                    .OrderByDescending(e => e.Name)
                                    .Select(e => e.Name);

            foreach (var employee in query2)
            {
                Console.WriteLine($"Employee: {employee}");
            }
```

* There is no Count, Take, Skip in query syntax.
* One or the other is mostly a preference

# LINQ Queries

## Deferred Execution
Linq uses deferred exection. It's not unitl you run a foreach loop that the IEnumerable is making use of the predicate. This is done due to the use of the yeild keyword.

yeild, will yield back to foreach so that foreach can do it's thing. So the query isn't retuning the full collection and then you foreach, on each iteration of your forech the predcitate is run.

Query doesn't do any work until you force the query to excecute.
Any operation the inspects the results will force query to exectue, serialization to JSON or XML, data binding into a Grid Control. All operations tht iterate over the results.


### Taking adventage of Deferred Exection
You have to check MSDN documentations to see if the Linq method provides deferred exection. In the remarks it says if the method uses deferred execution.

This is useful becuase you might not want to look at all the movies, maybe you just want to take the first one. For example using Take(1) you'll only check 1 item.
Another benefit is that you can write a query and pass it to someplace else and continue to compose the query before its executed.


### Avoiding pitfalls of Deferred Exection
Some methods like Count() do not have deferred execution, it forces the query to execute immedietly. You have to be careful that you are not doing twice the work.

To avoid doing twice the work you can execute the function right away:
```C#
var query = movies.Where(m => m.Year > 2000).ToList();

// Now you can get the 
query.Count

or 
// iterate over the list and you'll have only done one excution
foreach (ver movie in movies)
{
    //do domehting
}
```

### Exceptions and Deferred Execution
Make sure you put your try catch block around the actual exceution not the query.

### Streaming Operators
Deferred operators can either be streaming or non streaming.

For example:
Where - Streaming -> reads though only each item and yeilds back
OrderBy/OrderByDescending => Not Streaming => Looks at all items ,then returns.

SO when working to in memory data it's important t odo things like filter before sorting, it's more efficient. 
If using LINQ in database by using EF it's not as important

"Classification of Linq Opertions by Manner"

Streaming operators kind of work similar to rxjs in that stuff is streaming and you can pick/modify tack on what you want to see as an output.


# Filtering, Sorting, Projecting

First() -  finds the first item and retuns just one item, no longer an IEnumarable. Like Where it can take an expession to find which one you need.
Last() - find the last item that matches the query expression.

FirstOrDefault and LastOrDefault will not thrwo an expression, instead it will return the default vlaue.

## Quntifiers
These return true or false if anything is found, the do not offer deferred execution.

Any() - Does anything match, takes an expression and returns true or false
All() - Sees if all all items match.
Contains() - pass in an object to see if it's there, not experession.

Although they don't have deferred execution, they are performant becuase once found they will exit.


## Projecting to anonymous objects
```C#
            var query = from car in cars
                        where car.Manufacturer == "BMW" && car.Year == 2016
                        orderby car.Combined descending, car.Name ascending
                        select new
                        {
                            Manufacturer = car.Manufacturer,
                            Name = car.Name,
                            Combined = car.Combined
                        };



                        // Shortcut
            var query = from car in cars
                    where car.Manufacturer == "BMW" && car.Year == 2016
                    orderby car.Combined descending, car.Name ascending
                    select new
                    {
                        car.Manufacturer,
                        car.Name,
                        car.Combined
                    };
```

## Flattening with SelectMany()
SelectMany transforms a collection of collection or sequances of sequances into a single one.

Since strings are collections of characters we can drill into each string.
```C#

            var someString = "Scott";
            IEnumerable<char> characters = "Scott";

            // Here we have a collection of Cars and we pull the name which is a string
            // Then SelectMany drills into the string and selected all the items in that string.
            // This happens for every car in the collection.
            var result = cars.SelectMany(c => c.Name)
                .OrderBy(c => c);

            foreach (var character in result)
            {
                Console.WriteLine(character);
            }
```







# Joining, Goruping & Aggregating
Joining is similar to inner join

In joining first specify the variable and the source
Then what you want to join on.

Since it acts like an inner join, only cars that join to manufactures will be returned.

Noramlly you want the inner sequence to be smaller than outer sequance for perfromance reasons.


## Joining data with query syntax

```C#
            var cars = ProcessCars("fuel.csv");
            var manufacturers = ProcessManufacturers("manufacturers.csv");

            //var query = cars.OrderByDescending(car => car.Combined)
            //                .ThenBy(car => car.Name);

            var query = from car in cars
                        join manufacturer in manufacturers 
                            on car.Manufacturer equals manufacturer.Name
                        orderby car.Combined descending, car.Name ascending
                        select new
                        {
                            manufacturer.Headquarters,
                            car.Name,
                            car.Combined
                        };

            foreach (var car in query.Take(10))
            {
                Console.WriteLine($"{car.Headquarters} {car.Name} : {car.Combined}");
            }
```

## Joining with method syntax
The method syntax is a little different then query

First yo uneed to give it the result set you want to join on,
Then the outer item property you want to join on
Then the inner item property you want to join on
Finally a projection, sicne you can only return one type, you have to do some kind of annonymous object or new type.
The final projection will be passed both the car and the manufactur
```C#
            var query2 = cars.Join(manufacturers,
                c => c.Manufacturer,
                m => m.Name, (c, m) =>
                new
                {
                    m.Headquarters,
                    c.Name,
                    c.Combined
                })
                .OrderByDescending(c => c.Combined)
                .ThenBy(c => c.Name);
```


## Joining on two items (composite keys)
```C#
            var query = from car in cars
                        join manufacturer in manufacturers 
                            // Create anew anonymous object to match on
                            on new { car.Manufacturer, car.Year} 
                            equals 
                            // The objects must match so we set Manufacture to manufacturer.Name so it matches above.
                            new { Manufacturer = manufacturer.Name, manufacturer.Year }
                        orderby car.Combined descending, car.Name ascending
                        select new
                        {
                            manufacturer.Headquarters,
                            car.Name,
                            car.Combined
                        };

            var query2 = cars.Join(manufacturers,
                c => new { c.Manufacturer, c.Year },
                m => new { Manufacturer = m.Name, m.Year }, 
                (c, m) =>
                new
                {
                    m.Headquarters,
                    c.Name,
                    c.Combined
                })
                .OrderByDescending(c => c.Combined)
                .ThenBy(c => c.Name);
```

## Grouping
Group by manufaturer

Group by place all your items in a bucket of key value pairs.


```C#
          var query = from car in cars
                        group car by car.Manufacturer;

                        
            foreach (var result in query)
            {
                Console.WriteLine($"{result.Key} has {result.Count()} cars");
            }
```

Or top most 2 efficient cars by manufacturer:
```C#
            foreach (var group in query)
            {
                Console.WriteLine(group.Key);
                foreach(var car in group.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }
```


Fix the query to make sure all manufacture names are uppercase so that the join works.
Need the use of into syntax to make use of other operators.

```C#
           var query = from car in cars
                        group car by car.Manufacturer.ToUpper() into manufacturer
                        orderby manufacturer.Key
                        select manufacturer;
```


In Method syntax:
```C#
   var query2 = cars.GroupBy(c => c.Manufacturer.ToUpper())
                             .OrderBy(g => g.Key);
```


## Using GroupJoin for Heirachrical Data
Offers joining and grouping feature together. When it joins items it creates a grouping.

Her we want to add the manfuacture to get he country

```C#
            var query =
                // This object will be used to gorup
                from manufacturer in manufacturers
                join car in cars on manufacturer.Name equals car.Manufacturer
                    // group join happeing here. carGroup is now a variable
                    into carGroup
                orderby manufacturer.Name
                select new
                {
                    Manufacturer = manufacturer,
                    Cars = carGroup
                };

            foreach (var group in query)
            {
                Console.WriteLine($"{group.Manufacturer.Name} : {group.Manufacturer.Headquarters}");
                foreach(var car in group.Cars.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }
```

Using method syntax:
```C#
            var query2 = manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) =>
                        new
                        {
                            Manufacturer = m,
                            Cars = g
                        })
                .OrderBy(m  => m.Manufacturer.Name);
```


## Getting top three countries 

```C#
            var query =
                // This object will be used to gorup
                from manufacturer in manufacturers
                join car in cars on manufacturer.Name equals car.Manufacturer
                    // group join happeing here. carGroup is now a variable
                    into carGroup
                orderby manufacturer.Name
                select new
                {
                    Manufacturer = manufacturer,
                    Cars = carGroup
                } into result
                group result by result.Manufacturer.Headquarters;


            var query2 = manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) =>
                        new
                        {
                            Manufacturer = m,
                            Cars = g
                        })
                .GroupBy(m => m.Manufacturer.Headquarters);


            foreach (var group in query)
            {
                Console.WriteLine($"{group.Key}");
                foreach(var car in group.SelectMany(g => g.Cars).OrderByDescending(c => c.Combined).Take(3))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }
```

## Aggregating Data

Geting the average and min max of combined by manufacturer:

```C#
           var query =
                from car in cars
                group car by car.Manufacturer into carGroup
                orderby carGroup.Key
                select new
                {
                    Name = carGroup.Key,
                    Max = carGroup.Max(c => c.Combined),
                    Min = carGroup.Min(c => c.Combined),
                    Average = carGroup.Average(c => c.Combined)
                };

            foreach (var result in query)
            {
                Console.WriteLine($"{result.Name}");
                Console.WriteLine($"\t Max: {result.Max}, Min: {result.Min}, Average: {result.Average}");
            }
```

Ordering by max:

```C#
            var query =
                from car in cars
                group car by car.Manufacturer into carGroup
                select new
                {
                    Name = carGroup.Key,
                    Max = carGroup.Max(c => c.Combined),
                    Min = carGroup.Min(c => c.Combined),
                    Average = carGroup.Average(c => c.Combined)
                } into result
                orderby result.Max descending
                select result;
```

## Efficient aggregation with Aggregate Extension Method

The method syntax has an aggregate extension method that works like Reduce in javascript.
Basically you get an accumulator, each item, and the result.


In many cases it's best ot create a class for readability

The aggregate is more efficent becaue you only loop once and set your Max, Min, Avg rather then doing 3 loops as obove.
```C#
        var query2 = cars.GroupBy(c => c.Manufacturer)
            .Select(g =>
            {
                var results = g.Aggregate(new CarStatistics(), (acc, car) => acc.Accumulate(car), acc => acc.Compute());
                return new
                {
                    Name = g.Key,
                    Avg = results.Average,
                    Min = results.Min,
                    Max = results.Max
                };
            })
            .OrderByDescending(r => r.Max);

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

```

# Linq to XML
System.Xml.Linq is used to work with XML, methods all start with X, and dervie from Xnode.



## E;ement Approach
In this approach a Cars document will have many Car and each Car will ahve an element of Name and Combined
```C#
        static void Main(string[] args)
        {
            var records = ProcessCars("fuel.csv");

            var document = new XDocument();
            var cars = new XElement("Cars");

            // Build a loop to go through each record and create a car node and attach and name and compined 
            foreach (var record in records)
            {
                var car = new XElement("Car");
                var name = new XElement("Name", record.Name);
                var combined = new XElement("Combined", record.Combined);
                car.Add(name);
                car.Add(combined);

                cars.Add(car);
            }
            document.Add(cars);
            // Save the document.
            document.Save("fuel.xml");
        }
```

## Attribute Approach
Here we add the name and combined as attributes on Car

```C#
        static void Main(string[] args)
        {
            var records = ProcessCars("fuel.csv");

            var document = new XDocument();
            var cars = new XElement("Cars");
            foreach (var record in records)
            {
                var car = new XElement("Car");
                var name = new XAttribute("Name", record.Name);
                var combined = new XAttribute("Combined", record.Combined);
                car.Add(name);
                car.Add(combined);

                cars.Add(car);
            }
            document.Add(cars);
            document.Save("fuel.xml");
        }
```

## Functional Construction
Lets you almost pass anything to the constructor and it will figure out what to do.
```C#
        static void Main(string[] args)
        {
            var records = ProcessCars("fuel.csv");

            var document = new XDocument();
            var cars = new XElement("Cars");
            foreach (var record in records)
            {
                // Pass in name and combined upon creation and the API will nest them.
                var car = new XElement("Car", 
                    new XAttribute("Name", record.Name), 
                    new XAttribute("Combined", record.Combined),
                    new XAttribute("Manufacturer", record.Manufacturer));

                cars.Add(car);
            }
            document.Add(cars);
            document.Save("fuel.xml");
        }
```

But we can even replace foreach with 
```C#
        static void Main(string[] args)
        {
            var records = ProcessCars("fuel.csv");

            var document = new XDocument();
            var cars = new XElement("Cars");

            var elements = 
                from record in records
                select new XElement("Car",
                    new XAttribute("Name", record.Name),
                    new XAttribute("Combined", record.Combined),
                    new XAttribute("Manufacturer", record.Manufacturer));
            cars.Add(elements);
            document.Add(cars);
            document.Save("fuel.xml");
        }
```

And can even nest it futher:
```C#
        static void Main(string[] args)
        {
            var records = ProcessCars("fuel.csv");

            var document = new XDocument();
            var cars = new XElement("Cars",
                from record in records
                select new XElement("Car",
                    new XAttribute("Name", record.Name),
                    new XAttribute("Combined", record.Combined),
                    new XAttribute("Manufacturer", record.Manufacturer))
                );
            document.Add(cars);
            document.Save("fuel.xml");
        }
```


## Querying XML with Linq

Get only BMW cars.
```C#
        private static void QueryXml()
        {
            // Does not stream
            var document = XDocument.Load("fuel.xml");

            // Gets the first child element with the specified name.
            // THen gets all the elemetns in Cars
            var query =
                from element in document.Element("Cars").Elements("Car")
                    // get only elemetns that have MBW
                where element.Attribute("Manufacturer").Value == "BMW"
                select element.Attribute("Name").Value;

            foreach (var name in query)
            {
                Console.WriteLine(name);
            }
                
        }
```

If you're looking for an attribure and it's optional:
Normally you'll get an error if the attribute doesn't exist.
There is no strongly typed variables in XML

You can use ?
```C#
        private static void QueryXml()
        {
            // Does not stream
            var document = XDocument.Load("fuel.xml");

            // Gets the first child element with the specified name.
            // THen gets all the elemetns in Cars
            var query =
                from element in document.Element("Cars").Elements("Car")
                    // get only elemetns that have MBW
                where element.Attribute("Manufacturer2")?.Value == "BMW"
                select element.Attribute("Name").Value;

            foreach (var name in query)
            {
                Console.WriteLine(name);
            }
                
        }
```

Can also use Descendents but it's not as explicit as it will look for any descendant
```C#
        private static void QueryXml()
        {
            // Does not stream
            var document = XDocument.Load("fuel.xml");

            // Gets the first child element with the specified name.
            // THen gets all the elemetns in Cars
            var query =
                from element in document.Descendants("Car")
                    // get only elemetns that have MBW
                where element.Attribute("Manufacturer")?.Value == "BMW"
                select element.Attribute("Name").Value;

            foreach (var name in query)
            {
                Console.WriteLine(name);
            }
                
        }
```


## Working with XML Namespaces

### Creating Files with Namesapaces
```C#
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
```

### Reading using namespaces

```C#
        private static void QueryXml()
        {

            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";
            // Does not stream
            var document = XDocument.Load("fuel.xml");

            // Gets the first child element with the specified name.
            // THen gets all the elemetns in Cars
            var query =
                                            // Bulletproof the query to make sure that no exception is thrown if element is not found.
                from element in document.Element(ns + "Cars")?.Elements(ex + "Car") ?? IEnumerable.Empty<XElement>()
                    // get only elemetns that have MBW
                where element.Attribute("Manufacturer")?.Value == "BMW"
                select element.Attribute("Name").Value;

            foreach (var name in query)
            {
                Console.WriteLine(name);
            }
                
        }
```

# EF and LINQ
How does this work, how does linq issue SQL statements?

Instead of IEnumerable, DbSet returns an IQueryable.


IQueryable takes an expression not a Func


## Difference between an expression and a Func
```C#
// Func
Func<int, int> squre = x => x * x;
Func<int, int, int> add = (x, y) => x + y;

square(add(3,5));



// Expression
Excpresssion(Func<int, int> squre = x => x * x);
Expression(Func<int, int, int> add = (x, y) => x + y);

// can no longer invoke Add. The code is no longer compied to something taht can be excecuted.
// Instead you get the code at runime. You can walk though the tree and figure out waht to do.
// It becomes an object that describes the Func.
```

This is how LINQ works agains EF. 
Any code passed in is compiled to an Exprsssion and EF then inspects the expression and figures out what you are looking for.

Expressions can be turned in to executable code but you'll have to Compile() it.

```C#
add.Compile();
```












