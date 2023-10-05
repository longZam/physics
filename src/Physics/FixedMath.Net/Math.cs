namespace FixedMath.NET;


public static class Math
{
    public static Fix64 Min(params Fix64[] args)
    {
        int length = args.Length;
        Fix64 result = args[0];

        for (int i = 1; i < length; i++)
            if (args[i] < result)
                result = args[i];

        return result;
    }

    public static Fix64 Max(params Fix64[] args)
    {
        int length = args.Length;
        Fix64 result = args[0];

        for (int i = 1; i < length; i++)
            if (args[i] > result)
                result = args[i];

        return result;
    }
}