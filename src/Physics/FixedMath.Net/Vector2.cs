using System.Diagnostics.CodeAnalysis;

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

    public static bool operator ==(Vector2 v, Vector2 w)
    {
        return v.x == w.x && v.y == w.y;
    }

    public static bool operator !=(Vector2 v, Vector2 w)
    {
        return !(v == w);
    }

    public static Fix64 Dot(Vector2 v, Vector2 w)
    {
        return v.x * w.x + v.y * w.y;
    }

    public static bool Intersect(Vector2 a0, Vector2 a1, Vector2 b0, Vector2 b1)
    {
        static int CCW(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            Fix64 s = p0.x * p1.y + p1.x * p2.y + p2.x * p0.y
                - (p1.x * p0.y + p2.x * p1.y + p0.x * p2.y);

            return Fix64.Sign(s);
        }

        static int ComparisonPoints(Vector2 p0, Vector2 p1)
        {
            if (p0 == p1)
                return 0;

            if (p0.x > p1.x)
                return 1;

            if (p0.x == p1.x && p0.y > p1.y)
                return 1;

            return -1;
        }

        int a0a1 = CCW(a0, a1, b0) * CCW(a0, a1, b1);
        int b0b1 = CCW(b0, b1, a0) * CCW(b0, b1, a1);

        // 일직선 상에 존재하는 두 선분
        if (a0a1 == 0 && b0b1 == 0)
        {
            if (ComparisonPoints(a0, a1) < 0)
                (a0, a1) = (a1, a0);
            if (ComparisonPoints(b0, b1) < 0)
                (b0, b1) = (b1, b0);

            return ComparisonPoints(a1, b0) >= 0 && ComparisonPoints(a0, b1) <= 0;
        }

        return a0a1 < 0 && b0b1 < 0;
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

    // TODO: 값 비교 구현 필요
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return base.Equals(obj);
    }

    // TODO: Vector2의 해시코드 생성을 제대로 재정의해야 함.
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
