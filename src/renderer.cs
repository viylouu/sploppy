partial class sploppy {
    static void rend(ICanvas c) {

        //Update
        Player.Updateplayer();

        //Draw Calls
        c.Clear(Color.CornflowerBlue);

        Player.Drawplayer(c);

        c.DrawTexture(bgtex);

        c.DrawTexture(cloudstex, -Time.TotalTime*bgscrollspeed%240, 0, Alignment.TopLeft);
        c.DrawTexture(cloudstex, -Time.TotalTime*bgscrollspeed%240+240, 0, Alignment.TopLeft);
    }
}