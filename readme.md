﻿[![CircleCI](https://circleci.com/gh/jstafford5380/Provausio.Testing.Generators.svg?style=svg)](https://circleci.com/gh/jstafford5380/Provausio.Testing.Generators) [![NuGet](https://img.shields.io/badge/NuGet-Provausio.Testing.Generators-green.svg)](https://www.nuget.org/packages/Provausio.Testing.Generators/)
 
 # Getting started

#### Have a serializable DTO
Currently, this only supports objects with a default constructor and will only fill public properties that have a setter.

``` csharp
public class Employee 
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public int Age { get; set; }
	public decimal Rate { get; set; }
}
```

#### Setup object filler
This example uses some of the premade generators. Configure the filler with the `.For(...)` method. The first argument is an expression that selects the property that will be filled. The second argument is an instance of `IGenerateData`. This package has a handful of premades, but you can implement your own pretty easily.

``` csharp
var filler = new ObjectFill<Employee>()
                .For(p => p.FirstName, new NameProvider(NameType.Given, Gender.Both))
                .For(p => p.LastName, new NameProvider(NameType.Surname))
                .For(p => p.Age, new IntegerProvider(18, 65))
                .For(p => p.Rate, new MoneyProvider(1, 165));
```

#### Run the generator
The `Generate()` method takes a single argument which tells it how many objects to generate. The generator is lazy (objects are yielded); `Generate()` returns and iterator, so keep that in mind before you run `.ToList()` as it will generate all objects into memory.

``` csharp
IEnumerable<Employee> objects = filler.Generate(10);
```

That's basically it. 

#### Full example

``` csharp

public class Employee 
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public decimal Rate { get; set; }
}

var filler = new ObjectFill<Employee>()
    .For(p => p.FirstName, new NameProvider(NameType.Given, Gender.Both))
    .For(p => p.LastName, new NameProvider(NameType.Surname))
    .For(p => p.Age, new IntegerProvider(18, 65))
    .For(p => p.Rate, new MoneyProvider(1, 165));

IEnumerable<Employee> objects = filler.Generate(5);

// output (serialized as json)
[
    {
        "FirstName": "Isidro",
        "LastName": "Hessel",
        "Age": 23,
        "Rate": 36.49
    }, {
        "FirstName": "Clint",
        "LastName": "Pulver",
        "Age": 63,
        "Rate": 42.78
    }, {
        "FirstName": "Tory",
        "LastName": "Basilio",
        "Age": 48,
        "Rate": 43.11
    }, {
        "FirstName": "Cecilia",
        "LastName": "Forst",
        "Age": 42,
        "Rate": 50.82
    }, {
        "FirstName": "Trisha",
        "LastName": "Chambless",
        "Age": 40,
        "Rate": 44.34
    }
]
```

### Currently available generators
All generators in this package are suffixed with "Provider" so you can search for them that way. Otherwise, here is a list of what is currently available:

| Name                         | Description                                                       |
|------------------------------|-------------------------------------------------------------------|
|**NameProvider**              | Generates first names for males, females, or both; and last names.|
|**IntegerProvider**           | Generates integers randomly.                                      |
|**IncrementedIntegerProvider**| Generates integers in an incrementing fashion.                    |
|**MoneyProvider**             | Generates decimals constrained to 2 decimal places.               |
|**WordProvider**              | Generates a single word.                                          |
|**SentenceProvider**          | Generates random sentences using lorem ipsum.                     |
|**ParagraphProvider**         | Generates random paragraphs using lorem ipsum.                    |
|**IdProvider**                | Generates a unique ID (Int, Xid, Guid, Base58 formats available). All IDs are returned as a string.  |

## Using an ad hoc generator
This package includes `CustomGenerator` which can be used as an ad hoc implementation. The generic parameter designatest he return type of the generator and the argument is a delegate that returns that type.

``` csharp
var filler = new ObjectFill<Employee>()
	.For(p => p.FirstName, new CustomGenerator<string>(() => "Jon");

IEnumerable<Employee> objects = filler.Generate(5);

// output (serialized as json)
[
    {
        "FirstName": "Jon",
        "LastName": null,
        "Age": 0,
        "Rate": 0.0
    },
    {
        "FirstName": "Jon",
        "LastName": null,
        "Age": 0,
        "Rate": 0.0
    },
    {
        "FirstName": "Jon",
        "LastName": null,
        "Age": 0,
        "Rate": 0.0
    },
    {
        "FirstName": "Jon",
        "LastName": null,
        "Age": 0,
        "Rate": 0.0
    },
    {
        "FirstName": "Jon",
        "LastName": null,
        "Age": 0,
        "Rate": 0.0
    }
]
```

Noticed that the other properties are default values, because we didn't add them to the filler configuration.

## Implementing your own generator
Super easy, just implement the `IGenerateData` interface and use it in your configuration. As an example, let's turn our custom Jon generator above into and instance of `IGenerateData` but allow it to take any name.

``` csharp
public class StaticNameProvider : IGenerateData
{
    private string _name;

    public Type Type => typeof(string);

    public StaticNameProvider(string name)
    {
        _name = name;
    }

    public object Generate()
    {
        return _name;
    }
}

var filler = new ObjectFill<Employee>()
    .For(p => p.FirstName, new StaticNameProvider("Peter"));

IEnumerable<Employee> objects = filler.Generate(5);

// output (serialized as json)
[
    {
        "FirstName": "Peter",
        "LastName": null,
        "Age": 0,
        "Rate": 0.0
    },
    {
        "FirstName": "Peter",
        "LastName": null,
        "Age": 0,
        "Rate": 0.0
    },
    {
        "FirstName": "Peter",
        "LastName": null,
        "Age": 0,
        "Rate": 0.0
    },
    {
        "FirstName": "Peter",
        "LastName": null,
        "Age": 0,
        "Rate": 0.0
    },
    {
        "FirstName": "Peter",
        "LastName": null,
        "Age": 0,
        "Rate": 0.0
    }
]

```

## Overriding the fill action
There may be cases where you want to do some extra work after the generator runs. For example, maybe you want to generate a decimal but you want to fill a string, allowing you to apply some formatting. There is an available overload that will allow you specify an action to perform against the instance and it will also give you access to the generator.

The generator returns `object` so you'll have to cast. If you need to know what type it is, it is available on the generator at `IGenerateData.Type`. Generally speaking, you should already know what that type is during setup, so it shoulding be an issue.

``` csharp
var configuration = new ObjectFill<SomeObject>()
    .For(p => p.Age, 
            new MoneyProvider(15, 65), 
            (target, generator) => // target is the instance, generator is the generator you specified.
            {
                var wage = (decimal) generator.Generate();
                target.Wage = wage.ToString("c");
            });
```

This is also usefull if the only way you can set a property is through a method. Just do a similar override and call the method on the target.

I condidered writing it in a way that would provide the generated value instead of the generator itself, but besides you losing access to `IGenerateData.Type`, it also convoluted the implementation so I decided against it. This should give you a bit more flexibility.
