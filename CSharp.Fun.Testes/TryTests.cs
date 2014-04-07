using System;
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
            public void Select()
            {
                var tryVal = from t in Try.From(1)
                        select t * 2;

                tryVal.Should().Be(Try.From(2));
            }

            [Test]
            public void SelectThrowingException()
            {
                var tryVal = from t in Try.From<int>(() => {throw new Exception();})
                             select t * 2;

                tryVal.IsSuccess.Should().BeFalse();
            }
        }
    }
}
