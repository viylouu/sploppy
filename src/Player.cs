﻿partial class sploppy {
    public class Player {
        const byte drag = 24;

        public static uint Score;

        public static Vector2 Ppos = new Vector2(120, 64);
        public static Vector2 Pvel = Vector2.Zero;

        public static bool canmove = false;

        public static bool right = true;

        static Vector2 normgundir;
        static float atannormgundir;

        public static void Updateplayer() {
            if(Mouse.Position != Ppos)
                normgundir = Vector2.Normalize(Mouse.Position-Ppos);

            if(Mouse.Position != Ppos)
                gunpos += (normgundir*8+Ppos-gunpos) / (5/(delta*60));

            cursorsize += (1-cursorsize) / (48/(delta*60));

            atannormgundir = atan2(normgundir);

            if(!high)
                gunrot = Angle.Lerp(gunrot, atannormgundir, clamp(2.5f/delta,0,1));

            if(canmove) {
                //Update Position
                Ppos += Pvel * delta;

                if(!gameover) {
                    //Bounce
                    if (Ppos.Y < 3) { Pvel.Y = abs(Pvel.Y); Ppos.Y = 3; }
                    if (Ppos.X < 4) { Pvel.X = abs(Pvel.X); Ppos.X = 4; }
                    if (Ppos.X > 236) { Pvel.X = -abs(Pvel.X); Ppos.X = 236; }

                    //Lose State
                    if (Ppos.Y > 135) { 
                        gameover = true; 
                        timeofdeath = totaltime; 
                        gameoversfx.Play();
                        for (int i = 0; i < ammos.Count; i++)
                            ammosalt.Add(new() { pos = ammos[i].pos, spawntime = totaltime, vely = -72f });
                        ammos.Clear();
                        hasgoo = false;
                        hasgooalt = true;
                        lastgootime = totaltime;
                        gooalt.pos = goo.pos;
                        gooalt.spawntime = totaltime;
                        gooalt.vely = -72f;
                        crystals = 0;
                        if(high)
                            fadebacksfx.Play();
                        high = false;
                        highness = 0;

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
                    Score = (uint)MathF.Floor((totaltime-starttime)*4);

                    //Gravity
                    Pvel.Y += gravity * delta;
                    if (Pvel.X > 0) Pvel.X -= drag * delta;
                    else if(Pvel.X < 0) Pvel.X += drag * delta;

                    if (bot && ammo > 0)
                        bottick();
                }
            }

            if(!gameover) {
                //Input
                if (!high && ammo > 0 && (Mouse.IsButtonPressed(MouseButton.Left) || Keyboard.IsKeyPressed(Key.Space)) && canshoot) {
                    Vector2 ldir = -Vector2.Normalize(Mouse.Position - Ppos);
                    Pvel = ldir * gunforce; 
                    ammo--; 
                    shootsfx.Play(); 
                    if (!canmove) {
                        startgamesfx.Play();
                        starttime = totaltime;
                        ++ammo;
                    }
                    canmove = true;
                    right = ldir.X > 0;
                    cursorsize += .5f;
                    camshake += ldir*6;
                    camv += ldir*6;

                    shells.Add(
                        new() { 
                            pos = Ppos + -ldir*16, 
                            rot = ((float)r.NextDouble()*2-1)*pi,
                            pvel = new Vector2(cos(atannormgundir-pi/2f)*1.5f,(ldir.X>0?-1:1)*sin(atannormgundir-pi/2f))*64,
                            rvel = (r.Next(0,2)*2-1)*32
                        }
                    );

                    for(int i = 0; i < 24; i++) {
                        Vector2 ndir = new Vector2(cos(gunrot+(float)r.NextDouble()/2f),sin(gunrot+(float)r.NextDouble()/2f));
                        int partic = r.Next(0,3);
                        particles.Add(
                            new() { 
                                pos = gunpos+ndir*16, 
                                vel = ndir*r.Next(128,164), 
                                size = 6, 
                                col = partic==0?new Color(235,86,75):(partic==1?new Color(255,145,102):new Color(255,181,112)), 
                                dcol = partic==0?new Color(176,48,92):(partic==1?new Color(227,105,86):new Color(255,145,102)),
                                gas = true, 
                                spawntime = totaltime, 
                                lasttime = .85f,
                                startsize = 6,
                            }
                        );
                    }
                }
                else if ((Mouse.IsButtonPressed(MouseButton.Left) || Keyboard.IsKeyPressed(Key.Space)) && !high)
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
                        starttime = totaltime;
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

            //drawing the gun
            canvas.Translate(gunpos.X-1-camshake.X, gunpos.Y+1-camshake.Y);
            canvas.Rotate(gunrot);
            canvas.DrawTexture(Mouse.Position.X<Ppos.X?flippedguntex:guntex, new(Vector2.Zero, new(16,8), Alignment.CenterLeft), shadowcol);
            canvas.ResetState();

            canvas.Translate(gunpos-camshake);
            canvas.Rotate(gunrot);
            canvas.DrawTexture(Mouse.Position.X<Ppos.X?flippedguntex:guntex, Vector2.Zero, new(16, 8), Alignment.CenterLeft);
            canvas.ResetState();

            //draw the debugging
            if (debug)
                playerdebug(canvas);

            //update the cursor pos
            cursorpos += (new Vector2(clamp(Mouse.Position.X,cursorsize*5+2,240-cursorsize*5-2),clamp(Mouse.Position.Y,cursorsize*4.5f,135-cursorsize*6-2))-cursorpos)/(4/(delta*60));

            //draw the cursor
            canvas.Translate(cursorpos.X-1,cursorpos.Y+1);
            canvas.Rotate(totaltime*4);
            canvas.DrawTexture(cursoroltex, new Rectangle(0,0, 9*cursorsize, 9*cursorsize, Alignment.Center), shadowcol);
            canvas.ResetState();

            canvas.Translate(cursorpos);
            canvas.Rotate(totaltime*4);
            canvas.DrawTexture(cursoroltex, Vector2.Zero, Vector2.One*9*cursorsize, Alignment.Center);
            canvas.ResetState();

            canvas.DrawTexture(cursortex, new Rectangle(cursorpos.X-1,cursorpos.Y+1,9,9,Alignment.Center), shadowcol);
            canvas.DrawTexture(cursortex, cursorpos, Alignment.Center);

            canvas.Fill(shadowcol);
            canvas.DrawRect(cursorpos.X-cursorsize*6f-1,cursorpos.Y+cursorsize*5f+1,cursorsize*12,2);

            canvas.Fill(new Color(39,39,54));
            canvas.DrawRect(cursorpos.X-cursorsize*6f,cursorpos.Y+cursorsize*5f,cursorsize*12,2);

            canvas.Fill(new Color(255,255,235));
            canvas.DrawRect(cursorpos.X-cursorsize*6f,cursorpos.Y+cursorsize*5f,cursorsize*12*Clamp((cursorsize-1)/(maxcursorsize-1),0,1),2);
        }
    }
}