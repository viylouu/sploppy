partial class sploppy {
    public class Player {
        const byte gravity = 82;
        const byte drag = 24;
        const byte gunforce = 112;

        public static uint Score;

        public static Vector2 Ppos = new Vector2(120, 64);
        static Vector2 Pvel = Vector2.Zero;

        static bool canmove = false;

        public static void Updateplayer() {
            if(canmove) {
                //Update Position
                Ppos += Pvel * Time.DeltaTime;

                //Bounce
                if (Ppos.Y < 3) { Pvel.Y = abs(Pvel.Y); Ppos.Y = 3; }
                if (Ppos.X < 4) { Pvel.X = abs(Pvel.X); Ppos.X = 4; }
                if (Ppos.X > 236) { Pvel.X = -abs(Pvel.X); Ppos.X = 236; }

                //Score
                Score = (uint)MathF.Floor((Time.TotalTime-starttime)*4);

                //Gravity
                Pvel.Y += gravity * Time.DeltaTime;

                if(Pvel.X > 0) Pvel.X -= drag * Time.DeltaTime;
                else if(Pvel.X < 0) Pvel.X += drag * Time.DeltaTime;
            }

            //Input
            if (ammo > 0 && (Mouse.IsButtonPressed(MouseButton.Left) || Keyboard.IsKeyPressed(Key.Space)))
            { Pvel = -Vector2.Normalize(Mouse.Position - Ppos) * gunforce; ammo--; shootsfx.Play(); canmove = true; }
            else if (Mouse.IsButtonPressed(MouseButton.Left) || Keyboard.IsKeyPressed(Key.Space))
                shootnoammosfx.Play();
        }

        public static void Drawplayer(ICanvas canvas) {
            // Drawing the Player
            canvas.DrawTexture(sploppertex, new(Ppos.X-1, Ppos.Y+1, 8,6, Alignment.Center), shadowcol);
            canvas.DrawTexture(sploppertex, Ppos ,Alignment.Center);

            canvas.Translate(Ppos.X-1, Ppos.Y+1);
            canvas.Rotate(atan2(Vector2.Normalize(Mouse.Position - Ppos)));
            canvas.Translate(8, 0);
            canvas.DrawTexture(guntex, new(Vector2.Zero, new(16,8*(Mouse.Position.X<Ppos.X?-1:1)),Alignment.CenterLeft), shadowcol);
            canvas.ResetState();

            canvas.Translate(Ppos);
            canvas.Rotate(atan2(Vector2.Normalize(Mouse.Position-Ppos)));
            canvas.Translate(8,0);
            canvas.DrawTexture(guntex, Vector2.Zero, new(16, 8*(Mouse.Position.X<Ppos.X?-1:1)), Alignment.CenterLeft);
            canvas.ResetState();
        }
    }
}