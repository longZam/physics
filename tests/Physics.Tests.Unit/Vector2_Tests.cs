using FixedMath.NET;

namespace Physics.Tests.Unit;


public class Vector2_Tests
{
    [Fact]
    public void Intersect()
    {
        Vector2 a0 = new Vector2(0, 0);
        Vector2 a1 = new Vector2(1, 1);
        Vector2 b0 = new Vector2(0, 1);
        Vector2 b1 = new Vector2(1, 0);

        Assert.True(Vector2.Intersect(a0, a1, b0, b1));
    }
}
