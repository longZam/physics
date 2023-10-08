using Collections.Pooled;
using FixedMath.NET;

namespace Physics;



public struct AABB
{
    public Vector2 min, max;

    public Vector2 Center
    {
        get => (min + max) / 2;
        set
        {
            Vector2 delta = value - Center;
            min += delta;
            max += delta;
        }
    }
    public Vector2 Extends => max - min;


    public AABB(Vector2 min, Vector2 max)
    {
        this.min = min;
        this.max = max;
    }

    /// <summary>
    /// 두 AABB의 겹침 여부 계산
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool Intersect(AABB a, AABB b)
    {
        if (a.max.x <= b.min.x || a.min.x >= b.max.x) return false;
        if (a.max.y <= b.min.y || a.min.y >= b.max.y) return false;

        return true;
    }

    /// <summary>
    /// b에 대한 a의 침투 벡터를 계산
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="direction">침투 방향 단위 벡터</param>
    /// <param name="distance">침투 벡터 크기</param>
    /// <returns></returns>
    public static bool ComputePenetration(AABB a, AABB b, out Vector2 direction, out Fix64 distance)
    {
        direction = Vector2.zero;
        distance = Fix64.Zero;

        // 겹치지 않으면 false
        if (!Intersect(a, b))
            return false;

        // x, y축에 각각 투영한 길이 재기
        Fix64 overlapX = FixedMath.NET.Math.Max(a.max.x, b.max.x) - FixedMath.NET.Math.Min(a.min.x, b.min.x);
        Fix64 overlapY = FixedMath.NET.Math.Max(a.max.y, b.max.y) - FixedMath.NET.Math.Min(a.min.y, b.min.y);
        // 교집합 구하기
        overlapX = a.Extends.x + b.Extends.x - overlapX;
        overlapY = a.Extends.y + b.Extends.y - overlapY;
        // 방향성
        overlapX *= (Fix64)Fix64.Sign(a.Center.x - b.Center.x);
        overlapY *= (Fix64)Fix64.Sign(a.Center.y - b.Center.y);

        // out 인자 채우기
        Vector2 penetration;

        if (overlapX < overlapY)
            penetration = new Vector2(overlapX, Fix64.Zero);
        else
            penetration = new Vector2(Fix64.Zero, overlapY);

        if (penetration.Magnitude() == Fix64.Zero)
            return false;

        direction = penetration.Normalize();
        distance = penetration.Magnitude();

        return true;
    }

    public bool Raycast(Vector2 origin, Vector2 direction, Fix64 maxDistance, out RaycastHit result)
    {
        static Vector2 GetIntersectionPoint(Vector2 v0, Vector2 v1, Vector2 w0, Vector2 w1)
        {
            Fix64 a = v0.x * v1.y - v1.x * v0.y;
            Fix64 b = w0.x * w1.y - w1.x * w0.y;

            return new Vector2(
                a * (w0.x - w1.x) - (v0.x - v1.x) * b,
                a * (w0.y - w1.y) - (v0.y - v1.y) * b
            ) / ((v0.x - v1.x) * (w0.y - w1.y) - (v0.y - v1.y) * (w0.x - w1.x));
        }

        using PooledList<Vector2> aabbVertices = new PooledList<Vector2>(4, ClearMode.Always)
        {
            min,                       // bottomLeft
            new Vector2(max.x, min.y), // bottomRight
            new Vector2(min.x, max.y), // topLeft
            max                        // topRight
        };
        using PooledList<Vector2> normals = new PooledList<Vector2>(4, ClearMode.Always)
        {
            Vector2.down,
            Vector2.left,
            Vector2.right,
            Vector2.up
        };
        using PooledList<int> aabbIndices = new PooledList<int>(8, ClearMode.Always)
        {
            0, 1, // bottom
            0, 2, // left
            1, 3, // right
            2, 3, // top
        };

        Vector2 endPoint = origin + direction.Normalize() * maxDistance;

        for (int i = 0; i < 4; i++)
        {
            // 해당 면의 normal 벡터와 내적 연산 결과가 음수일 때만 ray와 충돌 가능성 있음.
            if (Vector2.Dot(normals[i], direction) >= Fix64.Zero)
                continue;

            Vector2 start = aabbVertices[aabbIndices[i * 2]];
            Vector2 end = aabbVertices[aabbIndices[i * 2 + 1]];

            // 겹쳐지는지 확인
            if (!Vector2.Intersect(origin, endPoint, start, end))
                continue;

            // ray 충돌 계산
            result = new RaycastHit(
                point: GetIntersectionPoint(origin, endPoint, start, end),
                normal: normals[i]
            );

            return true;
        }

        result = new RaycastHit();
        return false;
    }
}
