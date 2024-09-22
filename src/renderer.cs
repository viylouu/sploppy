partial class sploppy {
    static void rend(ICanvas c) {

        //Update
        Player.Updateplayer();

        //Draw Calls
        c.Clear(Color.CornflowerBlue);

        Player.Drawplayer(c);

        c.DrawTexture(bgtex);

        c.DrawTexture(cloudstex, -Time.TotalTime * bgscrollspeed % 240, 0, Alignment.TopLeft);
        c.DrawTexture(cloudstex, -Time.TotalTime * bgscrollspeed % 240 + 240, 0, Alignment.TopLeft);

        Player.Drawplayer(c);

        rendertext(c, dfont, ammo + " ammo", new Vector2(4,4), Color.Black);
        rendertext(c, dfont, ammo + " ammo", new Vector2(3,3), Color.White);
    }
}