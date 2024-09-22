using System;

partial class sploppy {
    public class Player {
        const int gravity = 48;
        const int Drag = 2;

        static Vector2 Ppos = new Vector2(100, 20);
        static Vector2 Pvel = new Vector2(0, 0);

        public static void Updateplayer() {
            //Update Position
            Ppos += Pvel * Time.DeltaTime;

            //Gravity
            Pvel.Y += gravity * Time.DeltaTime;

            //Input
            if (Keyboard.IsKeyPressed(Key.E)) Pvel.Y = -64;
        }

        public static void Drawplayer(ICanvas canvas) {
            // Drawing the Player
            canvas.DrawTexture(sploppertex, Ppos ,Alignment.Center);

            canvas.Translate(Ppos);
            canvas.Rotate(atan2(Vector2.Normalize(Mouse.Position-Ppos)));
            canvas.Translate(8,0);
            canvas.DrawTexture(guntex, Vector2.Zero, new Vector2(16, 8 * (Mouse.Position.X<Ppos.X?-1:1)), Alignment.CenterLeft);
            canvas.ResetState();
        }
    }
}