using NAudio.Wave.Compression;

partial class sploppy {
    public class Player {
        const byte drag = 24;

        public static uint Score;

        public static Vector2 Ppos = new Vector2(120, 64);
        static Vector2 Pvel = Vector2.Zero;

        public static bool canmove = false;

        public static bool right = true;

        static Vector2 normgundir;
        static float atannormgundir;

        public static void Updateplayer() {
            if(Mouse.Position != Ppos)
                normgundir = Vector2.Normalize(Mouse.Position-Ppos);

            if(Mouse.Position != Ppos)
                gunpos += (normgundir*8+Ppos-gunpos) / (5/(Time.DeltaTime*60));

            cursorsize += (1-cursorsize) / (48/(Time.DeltaTime*60));

            atannormgundir = atan2(normgundir);

            gunrot = Angle.Lerp(gunrot, atannormgundir, clamp(2.5f/Time.DeltaTime,0,1));

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

                        if (Score > scoredisp) {
                            switch (diff) {
                                case 1:
                                    highscoreRM = Score; break;
                                case 2:
                                    highscoreHM = Score; break;
                                case 3:
                                    highscoreMM = Score; break;
                            }

                            using (StreamWriter sw = new(Directory.GetCurrentDirectory() + @"\assets\saves\data.txt")) {
                                sw.WriteLine(highscoreRM);
                                sw.WriteLine(highscoreHM);
                                sw.WriteLine(highscoreMM);
                            }
                        }
                    }

                    //Score
                    Score = (uint)MathF.Floor((Time.TotalTime-starttime)*4);

                    //Gravity
                    Pvel.Y += gravity * Time.DeltaTime;
                    if (Pvel.X > 0) Pvel.X -= drag * Time.DeltaTime;
                    else if(Pvel.X < 0) Pvel.X += drag * Time.DeltaTime;

                    if (bot && ammo > 0)
                        bottick();
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
                    cursorsize += .5f;
                    lastshoottime = Time.TotalTime;
                }
                else if (Mouse.IsButtonPressed(MouseButton.Left) || Keyboard.IsKeyPressed(Key.Space))
                    shootnoammosfx.Play();
            }
        }

        static void bottick() { 
            List<Vector2> traj = new List<Vector2>();

            Vector2 _ppos = Ppos;
            Vector2 _pvel = Pvel;
            float delta = 1f / 12;
            float frames = 1f / delta;
            int dirs = 360 / 18;

            List<int> ammostouched = new List<int>();
            int[] ammocounts = new int[dirs+1];

            for (int i = 0; i < frames; i++) {
                _ppos += _pvel * delta;

                if (_ppos.Y < 3) { _pvel.Y = abs(_pvel.Y); _ppos.Y = 3; }
                if (_ppos.X < 4) { _pvel.X = abs(_pvel.X); _ppos.X = 4; }
                if (_ppos.X > 236) { _pvel.X = -abs(_pvel.X); _ppos.X = 236; }

                _pvel.Y += gravity * delta;
                if (_pvel.X > 0) _pvel.X -= drag * delta;
                else if (_pvel.X < 0) _pvel.X += drag * delta;

                traj.Add(_ppos);

                for (int j = 0; j < ammos.Count; j++)
                    if(!ammostouched.Contains(j))
                        if (dist(ammos[j].pos, _ppos) < 8) {
                            ammostouched.Add(j);
                            ammocounts[0]++;
                        }

                if (hasgoo)
                    if (dist(goo.pos, _ppos) < 10)
                        return;
            }

            for (int d = 0; d < dirs; d++) {
                ammostouched.Clear();
                traj.Clear();

                _ppos = Ppos;
                _pvel = -new Vector2(cos(torad(d*18)), sin(torad(d*18))) * gunforce;

                for (int i = 0; i < frames; i++) {
                    _ppos += _pvel * delta;

                    if (_ppos.Y < 3) { _pvel.Y = abs(_pvel.Y); _ppos.Y = 3; }
                    if (_ppos.X < 4) { _pvel.X = abs(_pvel.X); _ppos.X = 4; }
                    if (_ppos.X > 236) { _pvel.X = -abs(_pvel.X); _ppos.X = 236; }

                    _pvel.Y += gravity * delta;
                    if (_pvel.X > 0) _pvel.X -= drag * delta;
                    else if (_pvel.X < 0) _pvel.X += drag * delta;

                    traj.Add(_ppos);

                    for (int j = 0; j < ammos.Count; j++)
                        if(!ammostouched.Contains(j))
                            if (dist(ammos[j].pos, _ppos) < 8) {
                                ammostouched.Add(j);
                                ammocounts[d+1]++;
                            }

                    if (hasgoo)
                        if (dist(goo.pos, _ppos) < 10) {
                            ammocounts[d+1] = totalammo+1;
                            break;
                        }
                }
            }

            int maxval=0, maxi=-1;

            for (int i = 0; i < ammocounts.Length; i++)
                if (ammocounts[i] > maxval) {
                    maxval = ammocounts[i];
                    maxi = i-1;
                }

            if(maxval != 0) {
                if (maxi != -1) { 
                    Vector2 ldir = -new Vector2(cos(torad(maxi*18)), sin(torad(maxi*18)));
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
                    cursorsize += .5f;
                }
                travelling = false;
            }
        }

        static void playerdebug(ICanvas canvas) { 
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
                float delta = 1f / 12;

                for (int i = 0; i < 24; i++) {
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

                    canvas.DrawCircle(traj[i+1],1);
                }

                traj.Clear();

                _ppos = Ppos;
                _pvel = -Vector2.Normalize(Mouse.Position - Ppos) * gunforce;

                for (int i = 0; i < 24; i++) {
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

                    canvas.DrawCircle(traj[i+1],1);
                }
            }
        }

        public static void Drawplayer(ICanvas canvas) {
            // Drawing the Player
            canvas.DrawTexture(sploppertex, new(Ppos.X-1-camshake.X, Ppos.Y+1-camshake.Y, 8,6, Alignment.Center), shadowcol);

            if(right)
                canvas.DrawTexture(sploppertex, Ppos-camshake, Alignment.Center);
            else
                canvas.DrawTexture(flippedsploppertex, Ppos-camshake, Alignment.Center);

            canvas.Translate(gunpos.X-1-camshake.X, gunpos.Y+1-camshake.Y);
            canvas.Rotate(gunrot);
            canvas.DrawTexture(Mouse.Position.X<Ppos.X?flippedguntex:guntex, new(Vector2.Zero, new(16,8), Alignment.CenterLeft), shadowcol);
            canvas.ResetState();

            canvas.Translate(gunpos-camshake);
            canvas.Rotate(gunrot);
            canvas.DrawTexture(Mouse.Position.X<Ppos.X?flippedguntex:guntex, Vector2.Zero, new(16, 8), Alignment.CenterLeft);
            canvas.ResetState();

            if (debug)
                playerdebug(canvas);

            canvas.Translate(Mouse.Position.X-1, Mouse.Position.Y+1);
            canvas.Rotate(Time.TotalTime*4);
            canvas.DrawTexture(cursoroltex, new Rectangle(0,0, 9*cursorsize, 9*cursorsize, Alignment.Center), shadowcol);
            canvas.ResetState();

            canvas.Translate(Mouse.Position);
            canvas.Rotate(Time.TotalTime*4);
            canvas.DrawTexture(cursoroltex, Vector2.Zero, Vector2.One*9*cursorsize, Alignment.Center);
            canvas.ResetState();

            canvas.DrawTexture(cursortex, new Rectangle(Mouse.Position.X-1, Mouse.Position.Y+1, 9,9, Alignment.Center), shadowcol);
            canvas.DrawTexture(cursortex, Mouse.Position, Alignment.Center);
        }
    }
}