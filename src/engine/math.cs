partial class sploppy {
    const float pi = 3.1415926535897932384626f;
    static float round(float a) => MathF.Round(a);
    static float atan2(Vector2 a) => MathF.Atan2(a.Y,a.X);
    static float abs(float a) => Math.Abs(a);
    static float sin(float a) => MathF.Sin(a);
    static float sqr(float a) => a * a;
    static float dist(Vector2 a, Vector2 b) => MathF.Sqrt(sqr(b.X-a.X) + sqr(b.Y-a.Y));
    static float pow(float a, float b) => MathF.Pow(a, b);
    static float clamp(float a, float b, float c) => Math.Clamp(a, b, c);
    static float max(float a, float b) => MathF.Max(a, b);
    static float min(float a, float b) => MathF.Min(a, b);
    static float cos(float a) => MathF.Cos(a);
}