partial class sploppy {
    static void rend(ICanvas c) {
        c.Clear(Color.Black);

        c.DrawTexture(bgtex);

        c.DrawTexture(cloudstex, -Time.TotalTime*bgscrollspeed%240, 0, Alignment.TopLeft);
        c.DrawTexture(cloudstex, -Time.TotalTime*bgscrollspeed%240+240, 0, Alignment.TopLeft);
    }
}