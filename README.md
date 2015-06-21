#C#.Fun

[![Build status](https://ci.appveyor.com/api/projects/status/dr07py0f19ibgun3)](https://ci.appveyor.com/project/brenoferreira/csharp-fun)

Implementation of a few Monadic types in C# to make life easier on a few pain points of working with C#.

For now, it supports two types that help with `null` values and `Exceptions`, `Option` and `Try` respectively.

##Examples

###Option
Option type provides better support for working with `null` values, and avoiding `NullReferenceExceptions`.

```
var option = Option.From<string>(null);

option.Should().Be(Option.None);
```

Includes support for Nullable types
```
int? value = null;
var option = Option.From(value);

option.Should().Be(Option.None);
```

And also LINQ query operators.
```
var person = new Person
{
    Name = "Robb Stark",
    Address = new Address
    {
        City = "Winterfell"
    }
};

var res = from p in Option.From(person)
          from e in p.Address.ToOption()
          select e.City;

res.Should().Be(Option.From("Winterfell"));
```

```
var person = new Person
{
    Name = "Robb Stark"
};

var res = (from p in Option.From(person)
           from e in p.Address.ToOption()
           select e.City).GetOrElse("Address not specified");

res.Should().Be("Address not specified");
```

###Try

Try provides a different approach to handling exeptions. It removes the necessity for try/catch blocks (specially nested ones) with its higher order functions and LINQ operators.

```
var tryVal = from x in Try.From(2)
             from y in Try.From<int>(() => {throw new Exception();})
             select x * y;

tryVal.IsSuccess.Should().BeFalse();
```

```
var failure = Try.From<int>(() => { throw new Exception(); });

var success = failure.Recover((Exception ex) => 1);

success.Value.Should().Be(1);
```
