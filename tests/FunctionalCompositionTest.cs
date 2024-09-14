using FsCheck.Xunit;

namespace Sork.Funk.Tests;

public class CompositionTest
{
    [Property]
    public void Compose_WithAddOneSquared_ShouldSquareAddTwo(int input)
    {
       Func<int, int> addOne = a  => a + 1; 
       Func<int, int> timesTwo = a => a * 2;
       var result = addOne.Compose(timesTwo);
       
       result(input).Should().Be((input + 1 ) * 2);
    }
}
