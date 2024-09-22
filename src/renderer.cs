partial class sploppy {
    static void rend(ICanvas c) {

        //Update
        Player.Updateplayer();

        //Draw Calls
        c.Clear(Color.CornflowerBlue);

        Player.Drawplayer(c);

        rendertext(c, dfont, $"{round(1/Time.DeltaTime)} fps", new Vector2(3,3), Color.White);
    }
}