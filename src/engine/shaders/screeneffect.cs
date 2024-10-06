public class screenshader : CanvasShader {
    public ITexture backbuffer;
    public float time;
    public float highness;

    public override ColorF GetPixelColor(Vector2 pos) {
        ColorF fincol = backbuffer.Sample(pos);
        if(highness > 0) {
            float st = Sin(time*4+(pos.X+pos.Y)/24)*10;
            float fadeamt = 3*highness;
            Vector2 sintime = MakeVector2(st,st);
            fincol += new ColorF(backbuffer.Sample(pos+sintime).R/fadeamt,backbuffer.Sample(pos+sintime+MakeVector2(3,0)).G/fadeamt,backbuffer.Sample(pos+sintime-MakeVector2(3,0)).B/fadeamt);
            fincol -= backbuffer.Sample(pos)/fadeamt;
        }
        return ColorF.Lerp(fincol,ColorF.Black,Round(Distance(pos, MakeVector2(120,67.5f))*.15f)/24*highness);
    }
}