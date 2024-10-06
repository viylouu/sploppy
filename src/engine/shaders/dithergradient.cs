public class dithergradient : CanvasShader {
    public int[] dithermatrix = new int[64] {
        0, 48, 12, 60,  3, 51, 15, 63,
        32, 16, 44, 28, 35, 19, 47, 31,
        8, 56,  4, 52, 11, 59,  7, 55,
        40, 24, 36, 20, 43, 27, 39, 23,
        2, 50, 14, 62,  1, 49, 13, 61,
        34, 18, 46, 30, 33, 17, 45, 29,
        10, 58,  6, 54,  9, 57,  5, 53,
        42, 26, 38, 22, 41, 25, 37, 21
    };

    public int maxmatval = 8 * 8 - 1;

    public Vector2 startpos;
    public Vector2 endpos;

    public ColorF col1;
    public ColorF col2;
    public ColorF darkcol;
    public ColorF lightcol;

    public override ColorF GetPixelColor(Vector2 pos) {
        float distratio = getdistratio(startpos, endpos, pos);
        int intensity = (int)(distratio * maxmatval);

        int colidx = intensity<dithermatrix[(int)pos.X%8+(int)pos.Y%8*8]?1:2;

        if(pos.Y < 13)
            colidx--;
        else if(pos.Y > 114)
            colidx--;
        else if(pos.X > 237)
            colidx--;
        else if(pos.X < 3)
            colidx--;

        if(Floor(pos.X) == 1 && pos.Y > 1-float.Epsilon && pos.Y < 134+float.Epsilon)
            colidx+=2;
        if(Floor(pos.X) == 238 && pos.Y > 1-float.Epsilon && pos.Y < 134+float.Epsilon)
            colidx+=2;
        if(Floor(pos.Y) == 1 && pos.X > 2-float.Epsilon && pos.X < 238+float.Epsilon)
            colidx+=2;
        if(Floor(pos.Y) == 11 && pos.X > 2-float.Epsilon && pos.X < 238+float.Epsilon)
            colidx+=2;
        if(Floor(pos.Y) == 133 && pos.X > 2-float.Epsilon && pos.X < 238+float.Epsilon)
            colidx+=2;
        if(Floor(pos.Y) == 115 && pos.X > 2-float.Epsilon && pos.X < 238+float.Epsilon)
            colidx+=2;

        ColorF retcol = ColorF.CornflowerBlue;

        if(AsBool(AsInt(colidx <= 0)))
            retcol = darkcol;
        else if(AsBool(AsInt(colidx == 1)))
            retcol = col1;
        else if(AsBool(AsInt(colidx == 2)))
            retcol = col2;
        else
            retcol = lightcol;

        return retcol;
    }

    float getdistratio(Vector2 start, Vector2 end, Vector2 pos) {
        float dx = end.X - start.X;
        float dy = end.Y - start.Y;

        float totalDistance = Sqrt(dx * dx + dy * dy);

        float nx = dx / totalDistance;
        float ny = dy / totalDistance;

        float px = pos.X - start.X;
        float py = pos.Y - start.Y;

        float projection = px * nx + py * ny;

        float ratio = projection/totalDistance;
        return Clamp(ratio,0,1);
    }
}