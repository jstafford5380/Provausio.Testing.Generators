using System.Collections.Generic;
using Provausio.Testing.Generators.Shared;
using Xunit;

namespace Provausio.Testing.Generators.Tests.Shared
{
    public class IteratorStepperTests
    {
        [Fact]
        public void GetNext_OutOfBounds_Throws()
        {
            // arrange
            var stepper = new IteratorStepper<int>(GetIterator(10));

            // act
            Assert.Throws<EndOfIteratorException>(() =>
            {
                for (var i = 0; i < 20; i++)
                {
                    stepper.GetNext();
                }
            });
        }

        private IEnumerable<int> GetIterator(int count)
        {
            for(var i = 0; i < count; i++)
            {
                yield return i;
            }
        }
    }
}
