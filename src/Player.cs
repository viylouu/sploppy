partial class sploppy {
    public class Player {
        const byte drag = 24;
        const byte gunforce = 112;

        public static uint Score;

        public static Vector2 Ppos = new Vector2(120, 64);
        static Vector2 Pvel = Vector2.Zero;

        public static bool canmove = false;

        public static bool right = true;

        public static void Updateplayer() {
            if(canmove) {
                //Update Position
                Ppos += Pvel * Time.DeltaTime;

                if(!gameover) {
                    //Bounce
                    if (Ppos.Y < 3) { Pvel.Y = abs(Pvel.Y); Ppos.Y = 3; }
                    if (Ppos.X < 4) { Pvel.X = abs(Pvel.X); Ppos.X = 4; }
                    if (Ppos.X > 236) { Pvel.X = -abs(Pvel.X); Ppos.X = 236; }

                    //Lose State
                    if (Ppos.Y > 135) { 
                        gameover = true; 
                        timeofdeath = Time.TotalTime; 
                        gameoversfx.Play();
                        for (int i = 0; i < ammos.Count; i++)
                            ammosalt.Add(new() { pos = ammos[i].pos, spawntime = Time.TotalTime, vely = -72f });
                        ammos.Clear();
                        hasgoo = false;
                        hasgooalt = true;
                        lastgootime = Time.TotalTime;
                        gooalt.pos = goo.pos;
                        gooalt.spawntime = Time.TotalTime;
                        gooalt.vely = -72f;
                    }

                    //Score
                    Score = (uint)MathF.Floor((Time.TotalTime-starttime)*4);

                    //Gravity
                    Pvel.Y += gravity * Time.DeltaTime;
                    if (Pvel.X > 0) Pvel.X -= drag * Time.DeltaTime;
                    else if(Pvel.X < 0) Pvel.X += drag * Time.DeltaTime;
                }
            }

            if(!gameover) {
                //Input
                if (ammo > 0 && (Mouse.IsButtonPressed(MouseButton.Left) || Keyboard.IsKeyPressed(Key.Space))) {
                    Vector2 ldir = -Vector2.Normalize(Mouse.Position - Ppos);
                    Pvel = ldir * gunforce; 
                    ammo--; 
                    shootsfx.Play(); 
                    canmove = true;
                    right = ldir.X > 0;
                }
                else if (Mouse.IsButtonPressed(MouseButton.Left) || Keyboard.IsKeyPressed(Key.Space))
                    shootnoammosfx.Play();
            }
        }

        public static void Drawplayer(ICanvas canvas) {
            // Drawing the Player
            canvas.DrawTexture(sploppertex, new(Ppos.X-1, Ppos.Y+1, 8,6, Alignment.Center), shadowcol);

            if(right)
                canvas.DrawTexture(sploppertex, Ppos ,Alignment.Center);
            else
                canvas.DrawTexture(flippedsploppertex, Ppos, Alignment.Center);

            canvas.Translate(Ppos.X-1, Ppos.Y+1);
            canvas.Rotate(atan2(Vector2.Normalize(Mouse.Position - Ppos)));
            canvas.Translate(8, 0);
            canvas.DrawTexture(Mouse.Position.X<Ppos.X?flippedguntex:guntex, new(Vector2.Zero, new(16,8), Alignment.CenterLeft), shadowcol);
            canvas.ResetState();

            canvas.Translate(Ppos);
            canvas.Rotate(atan2(Vector2.Normalize(Mouse.Position-Ppos)));
            canvas.Translate(8,0);
            canvas.DrawTexture(Mouse.Position.X<Ppos.X?flippedguntex:guntex, Vector2.Zero, new(16, 8), Alignment.CenterLeft);
            canvas.ResetState();
        }
    }
}