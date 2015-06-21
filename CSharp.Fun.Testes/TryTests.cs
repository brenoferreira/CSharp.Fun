using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
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
                    throw new InvalidOperationException(); ;
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

            [Test]
            public void MapToVoidReturnsUnit()
            {
                var tryVal = Try.From(1);

                tryVal.Map(Console.WriteLine).Map(unit => unit.Should().Be(Unit.Instance));
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
                             select x * y;

                tryVal.Should().Be(Try.From(6));
            }

            [Test]
            public void SelectManyWithTwoTrySourcesAndOneIsFailure()
            {
                var tryVal = from x in Try.From(2)
                             from y in Try.From<int>(() => { throw new Exception(); })
                             select x * y;

                tryVal.IsSuccess.Should().BeFalse();
            }
        }

        public class RecoveryTests
        {
			private static int ThrowExceptions(string value)
			{
				if (value == "InvalidOperationException")
				{
					throw new InvalidOperationException();
				}
				else if (value == "NullReferenceException")
				{
					throw new NullReferenceException();
				}
				else if (value == "ArgumentException")
				{
					throw new ArgumentException();
				}
				else
				{
					throw new Exception();
				}
			}

            [Test]
            public void Recover()
            {
                var failure = Try.From<int>(() => { throw new Exception(); });

				var success = failure.Recover((Exception ex) => 1);

                success.Value.Should().Be(1);
            }

            [Test]
            public void RecoverThrowingSecondException()
            {
				var failure = Try.From(() => ThrowExceptions(""));

				var stillFailure = failure.Recover((Exception ex) => ThrowExceptions (""));

                stillFailure.IsSuccess.Should().BeFalse();
            }

            [Test]
            public void RecoverNonFailedTry()
            {
				var success = Try.From(1).Recover((Exception ex) => 2);

                success.Value.Should().Be(1);
            }

			[Test]
			public void RecoverTypedException()
			{
				var failure = Try.From<int>(() => ThrowExceptions("InvalidOperationException"));

				var stillFailure = failure.Recover((InvalidOperationException ex) => 1);

				stillFailure.Value.Should().Be(1);
			}

			[Test]
			public void RecoverTypedExceptionButWrongExceptionType()
			{
				var failure = Try.From<int>(() => ThrowExceptions("InvalidOperationException"));

				var stillFailure = failure.Recover((FormatException ex) => 1);

				stillFailure.IsSuccess.Should().BeFalse();
			}

            [Test]
            public void RecoverWith()
            {
                var failure = Try.From<int>(() => { throw new Exception(); });

				var success = failure.RecoverWith((Exception ex) => Try.From(1));

                success.Value.Should().Be(1);
            }

            [Test]
            public void RecoverWithThrowingSecondException()
            {
				var failure = Try.From(() => ThrowExceptions(""));

				var stillFailure = failure.RecoverWith((Exception ex) => Try.From(() => ThrowExceptions("")));

                stillFailure.IsSuccess.Should().BeFalse();
            }

			[Test]
			public void RecoverWithTypedException()
			{
				var failure = Try.From(() => ThrowExceptions("InvalidOperationException"));

				var success = failure.RecoverWith((InvalidOperationException ex) => Try.From(1));

				success.Value.Should().Be(1);
			}

			[Test]
			public void RecoverWithTypedExceptionButWrongExceptionType()
			{
				var failure = Try.From(() => ThrowExceptions("InvalidOperationException"));

				var stillFailure = failure.RecoverWith((FormatException ex) => Try.From(1));

				stillFailure.IsSuccess.Should().BeFalse();
			}

            [Test]
            public void RecoverWithNonFailedTry()
            {
				var success = Try.From(1).RecoverWith((Exception ex) => Try.From(2));

                success.Value.Should().Be(1);
            }
        }

        public class UncatchableExceptions
        {
            /// <summary>
            /// Ignored because StackOverflowException terminate application execution.
            /// If running this test is necessary, run with debugger attached.
            /// Just for documentation and proof that Try does not catch StackOverflowException.
            /// </summary>
            [Test]
            [Ignore]
            public void UncatchableStackOverflowException()
            {
                Try.From(UnboundRecursiveFunction);
            }

            private void UnboundRecursiveFunction()
            {
                UnboundRecursiveFunction();
            }

            [Test]
            [ExpectedException(typeof(ThreadAbortException))]
            public void UncatchableThreadAbortException()
            {
                Try.From(() => Thread.CurrentThread.Abort());
            }

            [Test]
            [ExpectedException(typeof(OutOfMemoryException))]
            public void UncatchableOutOfMemoryException()
            {
                Try.From(() =>
                {
                    var list = new List<IEnumerable<int>>();

                    for (int i = 0; i < int.MaxValue; i++)
                    {
                        var array = new int[int.MaxValue];
                        array[0] = 1;
                        array[int.MaxValue - 1] = 2;
                        list.Add(array);
                    }
                });
            }
        }
    }
}
