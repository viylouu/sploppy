partial class sploppy {
    static void rend(ICanvas canv) {
        ICanvas c = canvas.GetCanvas();

        delta = clamp(Time.DeltaTime,0.00001f,0.1f) * gamespeedmult;

        if(crystals > 0 && Keyboard.IsKeyPressed(Key.C)) { 
            high = true;
            crystals--;
            usecrystalsfx.Play();
        }

        if(high && (Mouse.IsButtonPressed(MouseButton.Left) || Keyboard.IsKeyPressed(Key.Space)) && Mouse.Position.X>0&&Mouse.Position.X<240&&Mouse.Position.Y>0&&Mouse.Position.Y<135) {
            high = false;
            Player.Ppos = Mouse.Position+camshake;
            fadebacksfx.Play();
            canshoot = false;
        }

        totaltime += delta;

        highness += ((high?1:0)-highness)/(16/(Time.DeltaTime*60));
        gamespeedmult = 1-highness;

        musicpb.Volume = 1-highness;
        windsfxpb.Volume = 1-highness;
        highpadspb.Volume = highness;

        if(high) { 
            if(Time.TotalTime%.025f<=0.001f)
                particles.Add(
                    new() { 
                        pos = Mouse.Position+camshake+new Vector2(((float)r.NextDouble()*2-1)*2), 
                        vel = Vector2.Zero, 
                        size = 2.5f, 
                        col = new Color(255,255,235), 
                        dcol = new Color(194,194,209), 
                        gas = true, 
                        spawntime = Time.TotalTime, 
                        lasttime = 1,
                        startsize = 2.5f,
                        ignoretime = true
                    }
                );
        }

        //Update
        Player.Updateplayer();

        canshoot = true;

        mainmenu.Updatemenu();

        //camera shake effect
        Vector2 cforce = -camk*camshake;
        Vector2 cdamp = -camb*camv*delta;
        Vector2 caccel = (cforce+cdamp)/camm;
        camv += caccel * delta;
        camshake += camv * delta;

        if(float.IsNaN(camshake.X))
            camshake = Vector2.Zero;

        if(float.IsNaN(camv.X))
            camv = Vector2.Zero;

        //detect and set background properties
        switch(diff) {
            case 1:
                darkclouds = false;
                scoredisp = highscoreRM;
                bggrad.col1 = bggradc1RM;
                bggrad.col2 = bggradc2RM;
                bggrad.darkcol = new Color(75,91,171).ToColorF();
                bggrad.lightcol = new Color(255,255,235).ToColorF();
                break;
            case 2:
                darkclouds = true;
                scoredisp = highscoreHM;
                bggrad.col1 = bggradc1HM;
                bggrad.col2 = bggradc2HM;
                bggrad.darkcol = new Color(115,39,92).ToColorF();
                bggrad.lightcol = new Color(255,145,102).ToColorF();
                break;
            case 3:
                darkclouds = true;
                scoredisp = highscoreMM;
                bggrad.col1 = bggradc1MM;
                bggrad.col2 = bggradc2MM;
                bggrad.darkcol = new Color(90,38,94).ToColorF();
                bggrad.lightcol = new Color(255,107,151).ToColorF();
                break;
        }

        bggrad.startpos = round(bggradsp-camshake);
        bggrad.endpos = round(bggradep-camshake);

        //draw backround
        c.Fill(bggrad);
        c.DrawRect(0,0,240,135);

        c.DrawTexture(darkclouds?darkcloudstex:cloudstex, new(-totaltime * bgscrollspeed % 240 - 1-camshake.X, 1-camshake.Y, 240, 135, Alignment.TopLeft), shadowcol);
        c.DrawTexture(darkclouds?darkcloudstex:cloudstex, new(-totaltime * bgscrollspeed % 240 + 239-camshake.X, 1-camshake.Y, 240, 135, Alignment.TopLeft), shadowcol);
        c.DrawTexture(darkclouds?darkcloudstex:cloudstex, -totaltime * bgscrollspeed % 240-camshake.X, -camshake.Y, Alignment.TopLeft);
        c.DrawTexture(darkclouds?darkcloudstex:cloudstex, -totaltime * bgscrollspeed % 240 + 240-camshake.X, -camshake.Y, Alignment.TopLeft);

        //ingame ui
        rendertext(c, dfont, ammo + " ammo", new Vector2(3, 4), shadowcol);
        rendertext(c, dfont, ammo + " ammo", new Vector2(4, 3), Color.White);
        rendertext(c, dfont, Player.Score + "", new Vector2(236-predicttextwidth(dfont,Player.Score+""), 4), shadowcol);
        rendertext(c, dfont, Player.Score + "", new Vector2(237-predicttextwidth(dfont, Player.Score + ""), 3), Color.White);
        rendertext(c, dfont, scoredisp + "", new Vector2(Window.Width/2-predicttextwidth(dfont, scoredisp + "")/2-1,4), shadowcol);
        rendertext(c, dfont, scoredisp + "", new Vector2(Window.Width/2-predicttextwidth(dfont, scoredisp + "")/2,3), Color.White);

        //crystal update logic
        if(cursorsize >= maxcursorsize) {
            cursorsize -= maxcursorsize-1;
            collecttelesfx.Play();
            crystals++;
        }

        //draw the crystals in ui
        for(int i = 0; i < crystals; i++) {
            c.DrawTexture(telecrysttex,new(3+i*9,132+sin((totaltime+i/3f)*6)*2,7,14,Alignment.BottomLeft),shadowcol);
            c.DrawTexture(telecrysttex,4+i*9,131+sin((totaltime+i/3f)*6)*2,Alignment.BottomLeft);
        }

        //particles
        for(int i = 0; i < particles.Count; i++) {
            c.Fill(shadowcol);
            c.DrawCircle(particles[i].pos-camshake+new Vector2(-1,1), particles[i].size+.75f);

            c.Stroke(polcol);
            c.DrawCircle(particles[i].pos-camshake, particles[i].size+.35f);

            c.Fill(polcol);
            c.DrawCircle(particles[i].pos-camshake, particles[i].size);
        }

        for(int i = 0; i < particles.Count; i++) {
            c.Fill(particles[i].dcol);
            c.DrawCircle(particles[i].pos-camshake, particles[i].size);

            c.Fill(particles[i].col);
            c.DrawCircle(particles[i].pos-camshake+particles[i].size*new Vector2(.25f,-.25f), particles[i].size*.75f);

            if(particles[i].vel.X > 0)
                particles[i].vel -= new Vector2(24*delta,0);
            else if(particles[i].vel.X < 0)
                particles[i].vel += new Vector2(24*delta,0);

            if(!particles[i].ignoretime) {
                particles[i].vel += new Vector2(0,(particles[i].gas?-1:1)*gravity*delta);
                particles[i].pos += particles[i].vel*delta;
                particles[i].size = particles[i].startsize*(-1/particles[i].lasttime*(totaltime-particles[i].spawntime)+1);
            } else {
                particles[i].vel += new Vector2(0,(particles[i].gas?-1:1)*gravity*Time.DeltaTime);
                particles[i].pos += particles[i].vel*Time.DeltaTime;
                particles[i].size = particles[i].startsize*(-1/particles[i].lasttime*(Time.TotalTime-particles[i].spawntime)+1);
            }

            if(particles[i].size <= -1) { 
                particles.RemoveAt(i);
                i--;
            } else if(particles[i].pos.X < -particles[i].size-1) {
                particles.RemoveAt(i);
                i--;
            } else if(particles[i].pos.X > 240+particles[i].size+1) {
                particles.RemoveAt(i);
                i--;
            } else {
                if(!particles[i].gas) {
                    if(particles[i].pos.Y > 135+particles[i].size+1) {
                        particles.RemoveAt(i);
                        i--;
                    }
                } else 
                    if(particles[i].pos.Y < -particles[i].size-1) {
                        particles.RemoveAt(i);
                        i--;
                    }
            }
        }

        //ingame ammo collectible system
        for (int i = 0; i < ammos.Count; i++) {
            float scalemult = easeoutelastic(totaltime-ammos[i].spawntime);
            c.DrawTexture(ammotex, new (ammos[i].pos + new Vector2(-1,sin(totaltime*3+i*4)*3+1)-camshake, new Vector2(6,8)*scalemult, Alignment.Center), shadowcol);
            c.DrawTexture(ammotex, ammos[i].pos + new Vector2(0,sin(totaltime*3+i*4)*3)-camshake, new Vector2(6,8)*scalemult, Alignment.Center);

            if (debug) {
                c.Stroke(Color.Red);
                c.DrawCircle(ammos[i].pos, 4);
            }

            if (dist(Player.Ppos, ammos[i].pos) < 8) {
                collectammosfx.Play();
                ammo++;
                collammo++;
                ammosalt.Add(new() { pos = ammos[i].pos, spawntime = totaltime, vely = -72f });
                ammos.RemoveAt(i);
                i--;
                cursorsize += .75f;
            }
        }

        //goo spawning system
        if (!hasgoo && lastgootime+goospawntime <= totaltime && !gameover) {
            goo.pos = new Vector2(r.Next(12, 228), r.Next(12, 100));
            goo.spawntime = totaltime + (float)r.NextDouble()/6f;
            hasgoo = true;
        }

        //ingame goo collectible system
        if (hasgoo) {
            float scalemult = easeoutelastic(totaltime-goo.spawntime);
            c.DrawTexture(gootex, new (goo.pos + new Vector2(-1,sin(totaltime*3)*3+1)-camshake, new Vector2(8,8)*scalemult, Alignment.Center), shadowcol);
            c.DrawTexture(gootex, goo.pos + new Vector2(0,sin(totaltime*3)*3)-camshake, new Vector2(8,8)*scalemult, Alignment.Center);

            if(totaltime%.025f<=0.001f)
                particles.Add(
                    new() { 
                        pos = goo.pos+new Vector2(((float)r.NextDouble()*2-1)*3,0), 
                        vel = new Vector2(((float)r.NextDouble()*2-1)*48,((float)r.NextDouble()*2-1)*48), 
                        size = 3, 
                        col = new Color(143,222,93), 
                        dcol = new Color(60,163,112), 
                        gas = false, 
                        spawntime = totaltime, 
                        lasttime = 1,
                        startsize = 3,
                    }
                );

            if (debug) {
                c.Stroke(Color.Red);
                c.DrawCircle(goo.pos, 6);
            }

            if (dist(Player.Ppos, goo.pos) < 10) {
                collectgoosfx.Play();
                ammo+=(ushort)(totalammo-collammo+1);
                collammo = totalammo;
                hasgoo = false;
                hasgooalt = true;
                lastgootime = totaltime;
                gooalt.pos = goo.pos;
                gooalt.spawntime = totaltime;
                gooalt.vely = -72f;
                cursorsize += sqr((totalammo-collammo+1)*1.25f);

                for (int i = 0; i < ammos.Count; i++)
                    ammosalt.Add(new() { pos = ammos[i].pos, spawntime = totaltime, vely = -72f });
            }
        }

        //clear the bullets to add more bullets
        if (collammo == totalammo) {
            ammos.Clear();
            totalammo = (byte)r.Next(minammo, maxammo);
            collammo = 0;

            for (int i = 0; i < totalammo; i++)
                ammos.Add(new() { pos = new Vector2(r.Next(12, 228), r.Next(12, 100)), spawntime = totaltime + (float)r.NextDouble()/6f });
        }

        //bullet despawning particle effect
        for(int i = 0; i < ammosalt.Count; i++) {
            float scalemult = 1-easeinback(2*(totaltime-ammosalt[i].spawntime));
            c.DrawTexture(ammotex, new(ammosalt[i].pos + new Vector2(-1, sin(totaltime * 3 + i * 4) * 3 + 1)-camshake, new Vector2(6, 8) * scalemult, Alignment.Center), shadowcol);
            c.DrawTexture(ammotex, ammosalt[i].pos + new Vector2(0, sin(totaltime * 3 + i * 4) * 3)-camshake, new Vector2(6, 8) * scalemult, Alignment.Center);

            ammosalt[i].vely += delta * gravity;
            ammosalt[i].pos += new Vector2(0, ammosalt[i].vely * delta);

            if (totaltime >= ammosalt[i].spawntime+.5f) {
                ammosalt.RemoveAt(i);
                i--;
            }
        }

        //goo despawning particle effect
        if (hasgooalt) { 
            float scalemult = 1-easeinback(2*(totaltime-gooalt.spawntime));
            c.DrawTexture(gootex, new(gooalt.pos + new Vector2(-1, sin(totaltime * 3) * 3 + 1)-camshake, new Vector2(8, 8) * scalemult, Alignment.Center), shadowcol);
            c.DrawTexture(gootex, gooalt.pos + new Vector2(0, sin(totaltime * 3) * 3)-camshake, new Vector2(8, 8) * scalemult, Alignment.Center);

            gooalt.vely += delta * gravity;
            gooalt.pos += new Vector2(0, gooalt.vely * delta);

            if (totaltime >= gooalt.spawntime+.5f)
                hasgooalt = false;
        }

        //shell particles
        for(int i = 0; i < shells.Count; i++) {
            c.Translate(shells[i].pos.X-1-camshake.X,shells[i].pos.Y+1-camshake.Y);
            c.Rotate(shells[i].rot);
            c.DrawTexture(shelltex,new (0,0,8,8,Alignment.Center), shadowcol);
            c.ResetState();

            c.Translate(shells[i].pos-camshake);
            c.Rotate(shells[i].rot);
            c.DrawTexture(shelltex,0,0,8,8,Alignment.Center);
            c.ResetState();

            if(shells[i].pos.Y < 3) { 
                shells[i].pvel = new Vector2(shells[i].pvel.X,abs(shells[i].pvel.Y)); 
                shells[i].pos = new Vector2(shells[i].pos.X,3);
            }
            if(shells[i].pos.X < 4) { 
                shells[i].pvel = new Vector2(abs(shells[i].pvel.X),shells[i].pvel.Y); 
                shells[i].pos = new Vector2(4, shells[i].pos.Y);
            }
            if(shells[i].pos.X > 236) { 
                shells[i].pvel = new Vector2(-abs(shells[i].pvel.X),shells[i].pvel.Y);
                shells[i].pos = new Vector2(236, shells[i].pos.Y); 
            }

            if(shells[i].pvel.X > 0)
                shells[i].pvel -= new Vector2(24 * delta,0);
            if(shells[i].pvel.X < 0)
                shells[i].pvel += new Vector2(24 * delta,0);

            shells[i].pvel += new Vector2(0,gravity * delta);
            shells[i].pos += shells[i].pvel * delta;

            if(shells[i].rvel > 0)
                shells[i].rvel -= 24 * delta;
            if(shells[i].rvel < 0)
                shells[i].rvel += 24 * delta;

            shells[i].rot += shells[i].rvel * delta;

            if(shells[i].pos.Y > 248) { shells.RemoveAt(i); i--; }
        }

        //draw player
        Player.Drawplayer(c);

        //rain
        if (diff > 1 && totaltime >= lastraintime+rainspawnfreq) {
            rainposses.Add(new Vector2(r.Next(0, 360), -16));
            lastraintime = totaltime;
        }

        for (int i = 0; i < rainposses.Count; i++) {
            c.Translate(rainposses[i]-camshake);
            c.Rotate(raindir+pid4);
            c.DrawTexture(raintex, 0, 0, 1, 32, Alignment.Center);
            c.ResetState();

            rainposses[i] += new Vector2(cos(raindir+pid2pd4),sin(raindir+pid2pd4))*rainspeed*delta;

            if (rainposses[i].Y > Window.Height + 16) {
                rainposses.RemoveAt(i);
                i--;
            }
        }

        //draw the menu
        mainmenu.DrawMenu(c);

        //fullscreening
        if (Keyboard.IsKeyPressed(Key.F)) {
            fullscreen = !fullscreen;

            if (fullscreen)
                Window.EnterFullscreen();
            else
                Window.ExitFullscreen();
        }

        //quit the game
        if (Keyboard.IsKeyPressed(Key.Escape))
            Environment.Exit(0);

        //game over screen
        if (gameover) {
            rendertext(c,dfont, "Press r to restart", new Vector2(120-predicttextwidth(dfont, "Press r to restart")/2f-1,round(67.5f+67.5f*(1-easeoutback((totaltime-timeofdeath)/2f)))+9-dfont.charh), shadowcol);
            rendertext(c,dfont, "Press r to restart", new Vector2(120-predicttextwidth(dfont, "Press r to restart")/2f,round(67.5f+67.5f*(1-easeoutback((totaltime-timeofdeath)/2f)))+8-dfont.charh),Color.White);
            rendertext(c,dfont,"Game Over",new Vector2(98,round(67.5f+67.5f*(1-easeoutback((totaltime-timeofdeath)/2f)))+1-dfont.charh), shadowcol);
            rendertext(c,dfont,"Game Over",new Vector2(99,round(67.5f+67.5f*(1-easeoutback((totaltime-timeofdeath)/2f)))-dfont.charh),Color.White);

            if (Keyboard.IsKeyPressed(Key.R)) {
                gameover = false;
                Player.Ppos = new Vector2(120, 64);
                Player.canmove = false;
                Player.right = true;
                ammo = startammo;
                totalammo = (byte)r.Next(minammo, maxammo);
                for (int i = 0; i < totalammo; i++)
                    ammos.Add(new() { pos = new Vector2(r.Next(12, 228), r.Next(12, 100)), spawntime = totaltime + (float)r.NextDouble() / 6f });
                starttime = totaltime;
            }

            lastgootime = totaltime;
        }

        c.Flush();
        sshad.backbuffer = canvas;
        sshad.time = Time.TotalTime;
        sshad.highness = highness;
        canv.Fill(sshad);
        canv.DrawRect(0,0,240,135);
    }
}