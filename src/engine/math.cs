partial class sploppy {
    static float round(float a) => MathF.Round(a);
    static float atan2(Vector2 a) => MathF.Atan2(a.Y,a.X);
    static float abs(float a) => Math.Abs(a);
    static float sin(float a) => MathF.Sin(a);
    static float sqr(float a) => a * a;
    static float dist(Vector2 a, Vector2 b) => MathF.Sqrt(sqr(b.X-a.X) + sqr(b.Y-a.Y));
}