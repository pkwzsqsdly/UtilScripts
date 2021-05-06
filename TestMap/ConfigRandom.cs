public class ConfigRandom
{
    public const int ST_AAAA = 1;
    public const int ST_BBBB = 2;
    public const int ST_CCCC = 4;
    public const int ST_DDDD = 8;
    public const int ST_EEEE = 16;
    public const int ST_FFFF = 32;
    public const int ST_GGGG = 64;
    public const int ST_HHHH = 128;
    public const int ST_IIII = 256;
    public const int ST_JJJJ = 512;

    public static bool IsContain(int target,int value)
    {
        return (value & target) == target;
    }
}