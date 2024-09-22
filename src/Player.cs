using System;

partial class sploppy {
    public class Player {
        const byte gravity = 48;
        const byte Drag = 32;

        static Vector2 Ppos = new Vector2(100, 20);
        static Vector2 Pvel = new Vector2(0, 0);

        public static void Updateplayer() {
            //Update Position
            Ppos += Pvel * Time.DeltaTime;

            //Bounce
            if (Ppos.Y < 3) Pvel.Y = -Pvel.Y;
            if (Ppos.X < 4 || Ppos.X > 236) Pvel.X = -Pvel.X;

            //Gravity
            Pvel.Y += gravity * Time.DeltaTime;
            if(Pvel.X > 0)
            {
                Pvel.X -= Drag * Time.DeltaTime;
            }
            else if(Pvel.X < 0)
            {
                Pvel.X += Drag * Time.DeltaTime;
            }

            //Input
            if (Mouse.IsButtonPressed(MouseButton.Left)) Pvel = -Vector2.Normalize(Mouse.Position - Ppos) * 80;
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