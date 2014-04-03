using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace CSharp.Fun.Testes
{
    public class UnitTests
    {
        [Test]
        public void UnitInstanceIsUnique()
        {
            Unit.Instance.Should().Be(Unit.Instance);
        }

        [Test]
        public void UnitEqualsUnit()
        {
            Unit.Instance.Equals(Unit.Instance).Should().BeTrue();
        }

        [Test]
        public void UnitIsNotDifferentFromUnit()
        {
            (Unit.Instance != Unit.Instance).Should().BeFalse();
        }

        [Test]
        public void EqualsOperatorReturnsTrue()
        {
            (Unit.Instance == Unit.Instance).Should().BeTrue();
        }
    }
}
