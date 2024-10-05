partial class sploppy {
    static void rend(ICanvas c) {

        //Update
        Player.Updateplayer();

        mainmenu.Updatemenu();

        //camera shake effect
        Vector2 cforce = -camk*camshake;
        Vector2 cdamp = -camb*camv*Time.DeltaTime;
        Vector2 caccel = (cforce+cdamp)/camm;
        camv += caccel * Time.DeltaTime;
        camshake += camv * Time.DeltaTime;

        //clear background
        c.Clear(Color.CornflowerBlue);

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

        c.DrawTexture(darkclouds?darkcloudstex:cloudstex, new(-Time.TotalTime * bgscrollspeed % 240 - 1-camshake.X, 1-camshake.Y, 240, 135, Alignment.TopLeft), shadowcol);
        c.DrawTexture(darkclouds?darkcloudstex:cloudstex, new(-Time.TotalTime * bgscrollspeed % 240 + 239-camshake.X, 1-camshake.Y, 240, 135, Alignment.TopLeft), shadowcol);
        c.DrawTexture(darkclouds?darkcloudstex:cloudstex, -Time.TotalTime * bgscrollspeed % 240-camshake.X, -camshake.Y, Alignment.TopLeft);
        c.DrawTexture(darkclouds?darkcloudstex:cloudstex, -Time.TotalTime * bgscrollspeed % 240 + 240-camshake.X, -camshake.Y, Alignment.TopLeft);

        //ingame ui
        rendertext(c, dfont, ammo + " ammo", new Vector2(3, 4), shadowcol);
        rendertext(c, dfont, ammo + " ammo", new Vector2(4, 3), Color.White);
        rendertext(c, dfont, Player.Score + "", new Vector2(236-predicttextwidth(dfont,Player.Score+""), 4), shadowcol);
        rendertext(c, dfont, Player.Score + "", new Vector2(237-predicttextwidth(dfont, Player.Score + ""), 3), Color.White);
        rendertext(c, dfont, scoredisp + "", new Vector2(Window.Width/2-predicttextwidth(dfont, scoredisp + "")/2-1,4), shadowcol);
        rendertext(c, dfont, scoredisp + "", new Vector2(Window.Width/2-predicttextwidth(dfont, scoredisp + "")/2,3), Color.White);

        //ingame ammo collectible system
        for (int i = 0; i < ammos.Count; i++) {
            float scalemult = easeoutelastic(Time.TotalTime-ammos[i].spawntime);
            c.DrawTexture(ammotex, new (ammos[i].pos + new Vector2(-1,sin(Time.TotalTime*3+i*4)*3+1)-camshake, new Vector2(6,8)*scalemult, Alignment.Center), shadowcol);
            c.DrawTexture(ammotex, ammos[i].pos + new Vector2(0,sin(Time.TotalTime*3+i*4)*3)-camshake, new Vector2(6,8)*scalemult, Alignment.Center);

            if (debug) {
                c.Stroke(Color.Red);
                c.DrawCircle(ammos[i].pos, 4);
            }

            if (dist(Player.Ppos, ammos[i].pos) < 8) {
                collectammosfx.Play();
                ammo++;
                collammo++;
                ammosalt.Add(new() { pos = ammos[i].pos, spawntime = Time.TotalTime, vely = -72f });
                ammos.RemoveAt(i);
                i--;
                cursorsize += .75f;
            }
        }

        //goo spawning system
        if (!hasgoo && lastgootime + goospawntime <= Time.TotalTime && !gameover) {
            goo.pos = new Vector2(r.Next(12, 228), r.Next(12, 100));
            goo.spawntime = Time.TotalTime + (float)r.NextDouble()/6f;
            hasgoo = true;
        }

        //ingame goo collectible system
        if (hasgoo) {
            float scalemult = easeoutelastic(Time.TotalTime-goo.spawntime);
            c.DrawTexture(gootex, new (goo.pos + new Vector2(-1,sin(Time.TotalTime*3)*3+1)-camshake, new Vector2(8,8)*scalemult, Alignment.Center), shadowcol);
            c.DrawTexture(gootex, goo.pos + new Vector2(0,sin(Time.TotalTime*3)*3)-camshake, new Vector2(8,8)*scalemult, Alignment.Center);

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
                lastgootime = Time.TotalTime;
                gooalt.pos = goo.pos;
                gooalt.spawntime = Time.TotalTime;
                gooalt.vely = -72f;
                cursorsize += sqr((totalammo-collammo+1)*1.25f);

                for (int i = 0; i < ammos.Count; i++)
                    ammosalt.Add(new() { pos = ammos[i].pos, spawntime = Time.TotalTime, vely = -72f });
            }
        }

        //clear the bullets to add more bullets
        if (collammo == totalammo) {
            ammos.Clear();
            totalammo = (byte)r.Next(minammo, maxammo);
            collammo = 0;

            for (int i = 0; i < totalammo; i++)
                ammos.Add(new() { pos = new Vector2(r.Next(12, 228), r.Next(12, 100)), spawntime = Time.TotalTime + (float)r.NextDouble()/6f });
        }

        //bullet despawning particle effect
        for(int i = 0; i < ammosalt.Count; i++) {
            float scalemult = 1-easeinback(2*(Time.TotalTime-ammosalt[i].spawntime));
            c.DrawTexture(ammotex, new(ammosalt[i].pos + new Vector2(-1, sin(Time.TotalTime * 3 + i * 4) * 3 + 1)-camshake, new Vector2(6, 8) * scalemult, Alignment.Center), shadowcol);
            c.DrawTexture(ammotex, ammosalt[i].pos + new Vector2(0, sin(Time.TotalTime * 3 + i * 4) * 3)-camshake, new Vector2(6, 8) * scalemult, Alignment.Center);

            ammosalt[i].vely += Time.DeltaTime * gravity;
            ammosalt[i].pos += new Vector2(0, ammosalt[i].vely * Time.DeltaTime);

            if (Time.TotalTime >= ammosalt[i].spawntime+.5f) {
                ammosalt.RemoveAt(i);
                i--;
            }
        }

        //goo despawning particle effect
        if (hasgooalt) { 
            float scalemult = 1-easeinback(2*(Time.TotalTime-gooalt.spawntime));
            c.DrawTexture(gootex, new(gooalt.pos + new Vector2(-1, sin(Time.TotalTime * 3) * 3 + 1)-camshake, new Vector2(8, 8) * scalemult, Alignment.Center), shadowcol);
            c.DrawTexture(gootex, gooalt.pos + new Vector2(0, sin(Time.TotalTime * 3) * 3)-camshake, new Vector2(8, 8) * scalemult, Alignment.Center);

            gooalt.vely += Time.DeltaTime * gravity;
            gooalt.pos += new Vector2(0, gooalt.vely * Time.DeltaTime);

            if (Time.TotalTime >= gooalt.spawntime+.5f)
                hasgooalt = false;
        }

        //draw player
        Player.Drawplayer(c);

        //rain
        if (diff > 1) {
            if (Time.TotalTime >= lastraintime + rainspawnfreq) {
                rainposses.Add(new Vector2(r.Next(0, 360), -16));
                lastraintime = Time.TotalTime;
            }

            for (int i = 0; i < rainposses.Count; i++) {
                c.Translate(rainposses[i]-camshake);
                c.Rotate(raindir+pid4);
                c.DrawTexture(raintex, 0, 0, 1, 32, Alignment.Center);
                c.ResetState();

                rainposses[i] += new Vector2(cos(raindir+pid2pd4),sin(raindir+pid2pd4))*rainspeed*Time.DeltaTime;

                if (rainposses[i].Y > Window.Height + 16) {
                    rainposses.RemoveAt(i);
                    i--;
                }
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
            rendertext(c,dfont, "Press r to restart", new Vector2(120-predicttextwidth(dfont, "Press r to restart")/2f-1,round(67.5f+67.5f*(1-easeoutback((Time.TotalTime-timeofdeath)/2f)))+9-dfont.charh), shadowcol);
            rendertext(c,dfont, "Press r to restart", new Vector2(120-predicttextwidth(dfont, "Press r to restart")/2f,round(67.5f+67.5f*(1-easeoutback((Time.TotalTime-timeofdeath)/2f)))+8-dfont.charh),Color.White);
            rendertext(c,dfont,"Game Over",new Vector2(98,round(67.5f+67.5f*(1-easeoutback((Time.TotalTime-timeofdeath)/2f)))+1-dfont.charh), shadowcol);
            rendertext(c,dfont,"Game Over",new Vector2(99,round(67.5f+67.5f*(1-easeoutback((Time.TotalTime-timeofdeath)/2f)))-dfont.charh),Color.White);

            if (Keyboard.IsKeyPressed(Key.R)) {
                gameover = false;
                Player.Ppos = new Vector2(120, 64);
                Player.canmove = false;
                Player.right = true;
                ammo = startammo;
                totalammo = (byte)r.Next(minammo, maxammo);
                for (int i = 0; i < totalammo; i++)
                    ammos.Add(new() { pos = new Vector2(r.Next(12, 228), r.Next(12, 100)), spawntime = Time.TotalTime + (float)r.NextDouble() / 6f });
                starttime = Time.TotalTime;
            }

            lastgootime = Time.TotalTime;
        }
    }
}