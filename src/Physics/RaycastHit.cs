using FixedMath.NET;

namespace Physics;


public readonly struct RaycastHit
{
    public readonly Vector2 point;
    public readonly Vector2 normal;


    public RaycastHit(Vector2 point, Vector2 normal)
    {
        this.point = point;
        this.normal = normal;
    }
}
