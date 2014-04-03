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
    }
}
