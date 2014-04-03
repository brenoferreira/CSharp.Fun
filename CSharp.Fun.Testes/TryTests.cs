using System;
using FluentAssertions;
using NUnit.Framework;

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
                var tryVal = Try.From(() => 1).FlatMap(n => Try.From(() => n + 1));

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
                }).FlatMap(n => Try.From(() => n + 1));

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
                }).FlatMap(n => Try.From(() => n.ToString()));

                tryVal.IsSuccess.Should().BeFalse();
                tryVal.As<Failure<string>>().Exception.Should().Be(exception);
            }

            [Test]
            public void Map()
            {
                var tryVal = Try.From(1).FlatMap(n => Try.From(n + 1));

                tryVal.IsSuccess.Should().BeTrue();
                tryVal.Value.Should().Be(2);
            }
        }
    }
}
