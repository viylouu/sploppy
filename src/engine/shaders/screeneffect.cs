public class screenshader : CanvasShader {
    public ITexture backbuffer;
    public float time;
    public float highness;

    public override ColorF GetPixelColor(Vector2 pos) {
        float st = (Sin(time*4+pos.X/6)*2+Cos(time+pos.Y/6)*4)*highness;
        Vector2 sintime = MakeVector2(st,st);
        return new ColorF(backbuffer.Sample(pos+sintime).R,backbuffer.Sample(pos+sintime).G,backbuffer.Sample(pos+sintime).B);
    }
}