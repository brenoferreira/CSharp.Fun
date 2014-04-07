using System;
using System.Security.Cryptography;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace CSharp.Fun.Testes
{
    public class TryTests
    {
        [Test]
        public void CreateSuccess()
        {
            var tryVal = Try.From(() => 1);

            tryVal.IsSuccess.Should().BeTrue();
            tryVal.Value.Should().Be(1);
        }

        [Test]
        public void CreateSuccessFromValue()
        {
            var tryVal = Try.From(1);

            tryVal.IsSuccess.Should().BeTrue();
            tryVal.Value.Should().Be(1);
        }

        [Test]
        public void CreateFailure()
        {
            var exception = new InvalidOperationException();
            var tryVal = Try.From<int>(() =>
            {
                throw exception;
            });

            tryVal.As<Failure<int>>().IsSuccess.Should().BeFalse();
            tryVal.As<Failure<int>>().Exception.Should().Be(exception);
        }

        [Test]
        public void CreateFromVoidReturningFunction()
        {
            var tryVal = Try.From(() => Console.WriteLine("void function"));

            tryVal.IsSuccess.Should().BeTrue();
            tryVal.Value.Should().Be(Unit.Instance);
        }

        [Test]
        public void FinallyActionIsExecuted()
        {
            var executedFinally = false;
            var tryVal = Try.From(() => 1, () => executedFinally = true);

            tryVal.IsSuccess.Should().BeTrue();
            tryVal.Value.Should().Be(1);
            executedFinally.Should().BeTrue();
        }

        [Test]
        public void FinallyActionIsExecutedWhenExceptionIsThrown()
        {
            var executedFinally = false;
            var tryVal = Try.From<int>(() =>
            {
                throw new Exception();
            }, () => executedFinally = true);

            tryVal.IsSuccess.Should().BeFalse();
            executedFinally.Should().BeTrue();
        }

        public class HigherOrderFunctions
        {
            [Test]
            public void FlatMapOnSuccess()
            {
                var tryVal = Try.From(1).FlatMap(n => Try.From(n + 1));

                tryVal.IsSuccess.Should().BeTrue();
                tryVal.Value.Should().Be(2);
            }

            [Test]
            public void FlatMapOnFailure()
            {
                var exception = new Exception();
                var tryVal = Try.From<int>(() =>
                {
                    throw exception;
                }).FlatMap(n => Try.From(n + 1));

                tryVal.IsSuccess.Should().BeFalse();
                tryVal.As<Failure<int>>().Exception.Should().Be(exception);
            }

            [Test]
            public void FlatMapOnFailureWithDifferentType()
            {
                var exception = new Exception();
                var tryVal = Try.From<int>(() =>
                {
                    throw exception;
                }).FlatMap(n => Try.From(n.ToString()));

                tryVal.IsSuccess.Should().BeFalse();
                tryVal.As<Failure<string>>().Exception.Should().Be(exception);
            }

            [Test]
            public void NewFailureIsNotFlatMapped()
            {
                var exception = new Exception();
                var tryVal = Try.From<int>(() =>
                {
                    throw exception;
                }).FlatMap<int, int>(n =>
                {
                    throw new InvalidOperationException(); ;
                });

                tryVal.IsSuccess.Should().BeFalse();
                tryVal.As<Failure<int>>().Exception.Should().Be(exception);
            }

            [Test]
            public void Map()
            {
                var tryVal = Try.From(1).Map(n => n + 1);

                tryVal.IsSuccess.Should().BeTrue();
                tryVal.Value.Should().Be(2);
            }

            [Test]
            public void MapDifferentType()
            {
                var tryVal = Try.From(1).Map(n => n.ToString());

                tryVal.IsSuccess.Should().BeTrue();
                tryVal.Value.Should().Be("1");
            }

            [Test]
            public void MapFailure()
            {
                var exception = new Exception();
                var tryVal = Try.From<int>(() =>
                {
                    throw exception;
                }).Map(n => n.ToString());

                tryVal.IsSuccess.Should().BeFalse();
                tryVal.As<Failure<string>>().Exception.Should().Be(exception);
            }

            [Test]
            public void NewFailureIsNotMapped()
            {
                var exception = new Exception();
                var tryVal = Try.From<int>(() =>
                {
                    throw exception;
                }).Map<int, int>(n =>
                {
                    throw new InvalidOperationException();;
                });

                tryVal.IsSuccess.Should().BeFalse();
                tryVal.As<Failure<int>>().Exception.Should().Be(exception);
            }

            [Test]
            public void Filter()
            {
                var tryVal = Try.From(1).Filter(n => n == 1);

                tryVal.IsSuccess.Should().BeTrue();
                tryVal.Value.Should().Be(1);
            }

            [Test]
            public void FilterWithFalsePredicate()
            {
                var tryVal = Try.From(1).Filter(n => n != 1);

                tryVal.IsSuccess.Should().BeFalse();
                tryVal.As<Failure<int>>().Exception.Should().BeOfType<NoSuchElementException>();
            }

            [Test]
            public void FilterWithFailure()
            {
                var exception = new Exception();
                var tryVal = Try.From(() =>
                {
                    throw exception;
                }).Filter(x => x != null);

                tryVal.IsSuccess.Should().BeFalse();
                tryVal.As<Failure<Unit>>().Exception.Should().Be(exception);
            }
        }

        public class TryLinqTests
        {
            [Test]
            public void SelectManyOnSuccess()
            {
                var tryVal = Try.From(1).SelectMany(n => Try.From(n + 1));

                tryVal.IsSuccess.Should().BeTrue();
                tryVal.Value.Should().Be(2);
            }

            [Test]
            public void SelectManyOnFailure()
            {
                var exception = new Exception();
                var tryVal = Try.From<int>(() =>
                {
                    throw exception;
                }).SelectMany(n => Try.From(n + 1));

                tryVal.IsSuccess.Should().BeFalse();
                tryVal.As<Failure<int>>().Exception.Should().Be(exception);
            }

            [Test]
            public void SelectManyOnFailureWithDifferentType()
            {
                var exception = new Exception();
                var tryVal = Try.From<int>(() =>
                {
                    throw exception;
                }).SelectMany(n => Try.From(n.ToString()));

                tryVal.IsSuccess.Should().BeFalse();
                tryVal.As<Failure<string>>().Exception.Should().Be(exception);
            }

            [Test]
            public void NewFailureIsNotSelectedMany()
            {
                var exception = new Exception();
                var tryVal = Try.From<int>(() =>
                {
                    throw exception;
                }).SelectMany<int, int>(n =>
                {
                    throw new InvalidOperationException(); ;
                });

                tryVal.IsSuccess.Should().BeFalse();
                tryVal.As<Failure<int>>().Exception.Should().Be(exception);
            }

            [Test]
            public void Select()
            {
                var tryVal = Try.From(1).Select(n => n + 1);

                tryVal.IsSuccess.Should().BeTrue();
                tryVal.Value.Should().Be(2);
            }

            [Test]
            public void SelectDifferentType()
            {
                var tryVal = Try.From(1).Select(n => n.ToString());

                tryVal.IsSuccess.Should().BeTrue();
                tryVal.Value.Should().Be("1");
            }

            [Test]
            public void SelectFailure()
            {
                var exception = new Exception();
                var tryVal = Try.From<int>(() =>
                {
                    throw exception;
                }).Select(n => n.ToString());

                tryVal.IsSuccess.Should().BeFalse();
                tryVal.As<Failure<string>>().Exception.Should().Be(exception);
            }

            [Test]
            public void NewFailureIsNotSelected()
            {
                var exception = new Exception();
                var tryVal = Try.From<int>(() =>
                {
                    throw exception;
                }).Select<int, int>(n =>
                {
                    throw new InvalidOperationException(); ;
                });

                tryVal.IsSuccess.Should().BeFalse();
                tryVal.As<Failure<int>>().Exception.Should().Be(exception);
            }

            [Test]
            public void Where()
            {
                var tryVal = Try.From(1).Where(n => n == 1);

                tryVal.IsSuccess.Should().BeTrue();
                tryVal.Value.Should().Be(1);
            }

            [Test]
            public void WhereWithFalsePredicate()
            {
                var tryVal = Try.From(1).Where(n => n != 1);

                tryVal.IsSuccess.Should().BeFalse();
                tryVal.As<Failure<int>>().Exception.Should().BeOfType<NoSuchElementException>();
            }

            [Test]
            public void WhereWithFailure()
            {
                var exception = new Exception();
                var tryVal = Try.From(() =>
                {
                    throw exception;
                }).Where(x => x != null);

                tryVal.IsSuccess.Should().BeFalse();
                tryVal.As<Failure<Unit>>().Exception.Should().Be(exception);
            }

            [Test]
            public void SelectManyWithTwoTrySources()
            {
                var tryVal = from x in Try.From(2)
                             from y in Try.From(3)
                             select x*y;

                tryVal.Should().Be(Try.From(6));
            }

            [Test]
            public void SelectManyWithTwoTrySourcesAndOneIsFailure()
            {
                var tryVal = from x in Try.From(2)
                             from y in Try.From<int>(() => {throw new Exception();})
                             select x * y;

                tryVal.IsSuccess.Should().BeFalse();
            }
        }
    }
}
