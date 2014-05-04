using FluentAssertions;
using NUnit.Framework;

namespace CSharp.Fun.Testes
{
    public class OptionTests
    {
        class CreationTests
        {
            [Test]
            public void FromValue()
            {
                var option = Option.From(1);
                Assert.AreEqual(option, Option.From(1));
                option.Should().Be(Option.From(1));
            }

            [Test]
            public void ToOption()
            {
                var option = 1.ToOption();

                option.Should().Be(Option.From(1));
            }

            [Test]
            public void FromNull()
            {
                var option = Option.From<string>(null);

                option.Should().Be(Option.None);
            }

            [Test]
            public void FromNonNullNullable()
            {
                int? value = 1;
                var option = Option.From(value);
                Assert.IsTrue(option.Equals(Option.From(1)));
                option.Should().Be(Option.From(1));
            }

            [Test]
            public void NonNullNullableToOption()
            {
                int? value = 1;
                var option = value.ToOption();

                option.Should().Be(Option.From(1));
            }

            [Test]
            public void FromNullNullable()
            {
                int? value = null;
                var option = Option.From(value);

                option.Should().Be(Option.None);
            }

            [Test]
            public void NullNullableToOption()
            {
                int? value = null;
                var option = value.ToOption();

                option.Should().Be(Option.None);
            }
        }

        class HighOrderFunctionsTests
        {
            [Test]
            public void OptionNoneTeste()
            {
                var person = new Person
                {
                    Name = "Robb Stark"
                };

                var optionperson = Option.From(person);

                var logradouro = optionperson
                                        .Map(p => p.Address)
                                        .Map(e => e.City)
                                        .GetOrElse("Não informado");

                logradouro.Should().Be("Não informado");
            }

            [Test]
            public void OptionSomeTeste()
            {
                var person = new Person
                {
                    Name = "Robb Stark",
                    Address = new Address
                    {
                        City = "Winterfell"
                    }
                };

                var optionperson = Option.From(person);

                var logradouro = optionperson
                                        .Map(p => p.Address)
                                        .Map(e => e.City)
                                        .GetOrElse("Não informado");

                logradouro.Should().Be("Winterfell");
            }
        }

        class LinqTests
        {
            [Test]
            public void LinqSelect()
            {
                var res = from x in Option.From(1)
                          select x;

                res.Should().Be(Option.From(1));
            }

            [Test]
            public void LinqSelectMany()
            {
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
            }

            [Test]
            public void LinqSelectManyWithNull()
            {
                var person = new Person
                {
                    Name = "Robb Stark"
                };

                var res = (from p in Option.From(person)
                           from e in p.Address.ToOption()
                           select e.City).GetOrElse("Sem endereco");

                res.Should().Be("Sem endereco");
            }

            [Test]
            public void WhereOperatorWithTruePredicate()
            {
                var person = new Person
                {
                    Name = "Robb Stark"
                };

                var res = from p in person.ToOption()
                          where p.Name == "Robb Stark"
                          select p.Name;

                res.GetOrElse("").Should().Be("Robb Stark");
            }

            [Test]
            public void WhereOperatorWithFalsePredicate()
            {
                var person = new Person
                {
                    Name = "Robb Stark"
                };

                var res = from p in person.ToOption()
                          where p.Name == "Jon Snow"
                          select p;

                res.Should().Be(Option.None);
            }
        }
    }

    class Person
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public Address Address { get; set; }
    }

    class Worker : Person
    {
        public string Profession { get; set; }
    }

    class Address
    {
        public string City { get; set; }
    }
}
