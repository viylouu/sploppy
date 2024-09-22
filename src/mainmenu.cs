partial class sploppy
{
    public class mainmenu
    {
        public static void Updatemenu()
        {

        }
        public static void DrawMenu(ICanvas c)
        {
            if (!Player.canmove) 
            {
                rendertext(c,dfont,"Sploppy",new Vector2(119 - predicttextwidth(dfont,"Sploppy")/2, 44f),shadowcol);
                rendertext(c, dfont, "Sploppy", new Vector2(120 - predicttextwidth(dfont, "Sploppy") / 2, 43f), Color.White);
                rendertext(c, dfont, "Press Space To Begin", new Vector2(119 - predicttextwidth(dfont, "Press Space To Begin") / 2, 84f), shadowcol);
                rendertext(c, dfont, "Press Space To Begin", new Vector2(120 - predicttextwidth(dfont, "Press Space To Begin") / 2, 83f), Color.White);
            }

        }
    }
}
