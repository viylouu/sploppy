partial class sploppy {
    public class mainmenu {
        public static void Updatemenu() {
            if(!Player.canmove) {
                if (Keyboard.IsKeyPressed(Key.Key1))
                    diff = 1;
                if(Keyboard.IsKeyPressed(Key.Key2))
                    diff = 2;
                if (Keyboard.IsKeyPressed(Key.Key3))
                    diff = 3;
            }
        }

        public static void DrawMenu(ICanvas c) {
            if (!Player.canmove) {
                rendertext(c,dfont,"Sploppy",new Vector2(119 - predicttextwidth(dfont,"Sploppy")/2, 44f),shadowcol);
                rendertext(c, dfont, "Sploppy", new Vector2(120 - predicttextwidth(dfont, "Sploppy") / 2, 43f), Color.White);
                rendertext(c, dfont, "Press Space To Begin", new Vector2(119 - predicttextwidth(dfont, "Press Space To Begin") / 2, 84f), shadowcol);
                rendertext(c, dfont, "Press Space To Begin", new Vector2(120 - predicttextwidth(dfont, "Press Space To Begin") / 2, 83f), Color.White);
                rendertext(c, dfont, "Press 1 to 3 to set the difficulty", new Vector2(119 - predicttextwidth(dfont, "Press 1 to 3 to set the difficulty") / 2, 84f+dfont.charh+2), shadowcol);
                rendertext(c, dfont, "Press 1 to 3 to set the difficulty", new Vector2(120 - predicttextwidth(dfont, "Press 1 to 3 to set the difficulty") / 2, 83f+dfont.charh+2), Color.White);
            }
        }
    }
}
