using MyIdeaPool.Models;
using Xunit;

namespace IdeaPool.Tests.ModelTests
{
    public class IdeaTests
    {
        [Fact]
        public void TestThatAverageIsCalculatedFromEaseImpactAndConfidence()
        {
            //arrange
            var sut = new Idea
            {
                Impact = 2,
                Ease = 3,
                Confidence = 5
            };
            //act
            var result = sut.Average;
            //assert
            Assert.Equal((double)10/3,result);
        }
    }
}