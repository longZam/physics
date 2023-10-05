namespace FixedMath.NET;


public struct Vector2
{
    public Fix64 x, y;

    public static readonly Vector2 zero = new Vector2(0, 0);
    public static readonly Vector2 one = new Vector2(1, 1);
    public static readonly Vector2 right = new Vector2(1, 0);
    public static readonly Vector2 left = new Vector2(-1, 0);
    public static readonly Vector2 up = new Vector2(0, 1);
    public static readonly Vector2 down = new Vector2(0, -1);


    public Vector2(int x, int y)
    {
        this.x = (Fix64)x;
        this.y = (Fix64)y;
    }

    public Vector2(Fix64 x, Fix64 y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vector2 operator +(Vector2 v, Vector2 w)
    {
        return new Vector2(v.x + w.x, v.y + w.y);
    }

    public static Vector2 operator -(Vector2 v, Vector2 w)
    {
        return new Vector2(v.x - w.x, v.y - w.y);
    }

    public static Vector2 operator *(Vector2 v, Fix64 scala)
    {
        return new Vector2(v.x * scala, v.y * scala);
    }

    public static Vector2 operator *(Vector2 v, int scala)
    {
        return v * (Fix64)scala;
    }

    public static Vector2 operator /(Vector2 v, Fix64 scala)
    {
        return new Vector2(v.x / scala, v.y / scala);
    }

    public static Vector2 operator /(Vector2 v, int scala)
    {
        return v / (Fix64)scala;
    }

    public static Vector2 operator -(Vector2 v)
    {
        return v * -1;
    }

    public readonly Fix64 SqrMagnitude()
    {
        return this.x * this.x + this.y * this.y;
    }

    public readonly Fix64 Magnitude()
    {
        return Fix64.Sqrt(SqrMagnitude());
    }

    public readonly Vector2 Normalize()
    {
        return this / Magnitude();
    }

    public override string ToString()
    {
        return $"({x}, {y})";
    }
}
