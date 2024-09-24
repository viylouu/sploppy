partial class sploppy {
    public class Player {
        const byte drag = 24;

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
                    if (!canmove) {
                        startgamesfx.Play();
                        starttime = Time.TotalTime;
                        ++ammo;
                    }
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

            if (debug) {
                canvas.Stroke(Color.Purple);
                canvas.DrawRect(Ppos, new Vector2(8,6), Alignment.Center);
                canvas.Stroke(Color.Red);
                canvas.DrawCircle(Ppos, 4);

                canvas.Stroke(Color.LimeGreen);
                canvas.DrawLine(Ppos, Ppos+Vector2.Normalize(Mouse.Position - Ppos)*24);

                if(showtraj) {
                    List<Vector2> traj = new List<Vector2>();

                    Vector2 _ppos = Ppos;
                    Vector2 _pvel = Pvel;
                    float delta = 1f / 20;

                    for (int i = 0; i < 40; i++) {
                        _ppos += _pvel * delta;

                        if (_ppos.Y < 3) { _pvel.Y = abs(_pvel.Y); _ppos.Y = 3; }
                        if (_ppos.X < 4) { _pvel.X = abs(_pvel.X); _ppos.X = 4; }
                        if (_ppos.X > 236) { _pvel.X = -abs(_pvel.X); _ppos.X = 236; }

                        _pvel.Y += gravity * delta;
                        if (_pvel.X > 0) _pvel.X -= drag * delta;
                        else if (_pvel.X < 0) _pvel.X += drag * delta;

                        traj.Add(_ppos);
                    }

                    canvas.Stroke(Color.Lime);

                    for (int i = -1; i < traj.Count-1; i++) {
                        if(i == -1)
                            canvas.DrawLine(Ppos, traj[0]);
                        else
                            canvas.DrawLine(traj[i], traj[i+1]);
                    }

                    traj.Clear();

                    _ppos = Ppos;
                    _pvel = -Vector2.Normalize(Mouse.Position - Ppos) * gunforce;

                    for (int i = 0; i < 40; i++) {
                        _ppos += _pvel * delta;

                        if (_ppos.Y < 3) { _pvel.Y = abs(_pvel.Y); _ppos.Y = 3; }
                        if (_ppos.X < 4) { _pvel.X = abs(_pvel.X); _ppos.X = 4; }
                        if (_ppos.X > 236) { _pvel.X = -abs(_pvel.X); _ppos.X = 236; }

                        _pvel.Y += gravity * delta;
                        if (_pvel.X > 0) _pvel.X -= drag * delta;
                        else if (_pvel.X < 0) _pvel.X += drag * delta;

                        traj.Add(_ppos);
                    }

                    canvas.Stroke(Color.Green);

                    for (int i = -1; i < traj.Count-1; i++) {
                        if(i == -1)
                            canvas.DrawLine(Ppos, traj[0]);
                        else
                            canvas.DrawLine(traj[i], traj[i+1]);
                    }
                }
            }
        }
    }
}