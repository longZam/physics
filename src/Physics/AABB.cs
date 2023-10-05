using FixedMath.NET;

namespace Physics;



public struct AABB
{
    public Vector2 min, max;

    public Vector2 Center => (min + max) / 2;
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
}
