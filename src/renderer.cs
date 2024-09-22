using Silk.NET.OpenGL;

partial class sploppy {
    static void rend(ICanvas c) {
        //Update
        Player.Updateplayer();

        //Draw Calls
        c.Clear(Color.CornflowerBlue);

        //BG
        c.DrawTexture(bgtex);

        //Clouds
        c.DrawTexture(cloudstex, new(-Time.TotalTime * bgscrollspeed % 240-1, 1, 240,135, Alignment.TopLeft), shadowcol);
        c.DrawTexture(cloudstex, new(-Time.TotalTime * bgscrollspeed % 240 + 239, 1, 240,135, Alignment.TopLeft), shadowcol);
        c.DrawTexture(cloudstex, -Time.TotalTime * bgscrollspeed % 240, 0, Alignment.TopLeft);
        c.DrawTexture(cloudstex, -Time.TotalTime * bgscrollspeed % 240 + 240, 0, Alignment.TopLeft);

        //UI
        rendertext(c, dfont, ammo + " ammo", new Vector2(3, 4), shadowcol);
        rendertext(c, dfont, ammo + " ammo", new Vector2(4, 3), Color.White);
        rendertext(c, dfont, Player.Score + "", new Vector2(119, 4), shadowcol);
        rendertext(c, dfont, Player.Score + "", new Vector2(120, 3), Color.White);

        //Spawn ammo
        for (int i = 0; i < ammos.Count; i++) {
            float scalemult = easeoutelastic(Time.TotalTime-ammos[i].spawntime);
            c.DrawTexture(ammotex, new (ammos[i].pos + new Vector2(-1,sin(Time.TotalTime*3+i*4)*3+1), new Vector2(6,8)*scalemult, Alignment.Center), shadowcol);
            c.DrawTexture(ammotex, ammos[i].pos + new Vector2(0,sin(Time.TotalTime*3+i*4)*3), new Vector2(6,8)*scalemult, Alignment.Center);

            if (dist(Player.Ppos, ammos[i].pos) < 8) {
                collectammosfx.Play();
                ammo++;
                collammo++;
                ammosalt.Add(new() { pos = ammos[i].pos, spawntime = Time.TotalTime, vely = -72f });
                ammos.RemoveAt(i);
                i--;
            }
        }
        //Move my goo
        if (!hasgoo && lastgootime + goospawntime <= Time.TotalTime && !gameover) {
            goo.pos = new Vector2(r.Next(12, 228), r.Next(12, 100));
            goo.spawntime = Time.TotalTime + (float)r.NextDouble()/6f;
            hasgoo = true;
        }

        //Consume Goo
        if (hasgoo) {
            float scalemult = easeoutelastic(Time.TotalTime-goo.spawntime);
            c.DrawTexture(gootex, new (goo.pos + new Vector2(-1,sin(Time.TotalTime*3)*3+1), new Vector2(8,8)*scalemult, Alignment.Center), shadowcol);
            c.DrawTexture(gootex, goo.pos + new Vector2(0,sin(Time.TotalTime*3)*3), new Vector2(8,8)*scalemult, Alignment.Center);

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

                for (int i = 0; i < ammos.Count; i++)
                    ammosalt.Add(new() { pos = ammos[i].pos, spawntime = Time.TotalTime, vely = -72f });
            }
        }

        //More BOOLET
        if (collammo == totalammo) {
            ammos.Clear();
            totalammo = (byte)r.Next(3, 5);
            collammo = 0;

            for (int i = 0; i < totalammo; i++)
                ammos.Add(new() { pos = new Vector2(r.Next(12, 228), r.Next(12, 100)), spawntime = Time.TotalTime + (float)r.NextDouble()/6f });
        }

        //The boolets
        for (int i = 0; i < ammosalt.Count; i++) {
            float scalemult = 1-easeinback(2*(Time.TotalTime-ammosalt[i].spawntime));
            c.DrawTexture(ammotex, new(ammosalt[i].pos + new Vector2(-1, sin(Time.TotalTime * 3 + i * 4) * 3 + 1), new Vector2(6, 8) * scalemult, Alignment.Center), shadowcol);
            c.DrawTexture(ammotex, ammosalt[i].pos + new Vector2(0, sin(Time.TotalTime * 3 + i * 4) * 3), new Vector2(6, 8) * scalemult, Alignment.Center);

            ammosalt[i].vely += Time.DeltaTime * gravity;
            ammosalt[i].pos += new Vector2(0, ammosalt[i].vely * Time.DeltaTime);

            if (Time.TotalTime >= ammosalt[i].spawntime+.5f) {
                ammosalt.RemoveAt(i);
                i--;
            }
        }

        //The Goo
        if (hasgooalt) { 
            float scalemult = 1-easeinback(2*(Time.TotalTime-gooalt.spawntime));
            c.DrawTexture(gootex, new(gooalt.pos + new Vector2(-1, sin(Time.TotalTime * 3) * 3 + 1), new Vector2(8, 8) * scalemult, Alignment.Center), shadowcol);
            c.DrawTexture(gootex, gooalt.pos + new Vector2(0, sin(Time.TotalTime * 3) * 3), new Vector2(8, 8) * scalemult, Alignment.Center);

            gooalt.vely += Time.DeltaTime * gravity;
            gooalt.pos += new Vector2(0, gooalt.vely * Time.DeltaTime);

            if (Time.TotalTime >= gooalt.spawntime+.5f)
                hasgooalt = false;
        }

        //Player
        Player.Drawplayer(c);

        mainmenu.DrawMenu(c);

        //Game Over
        if (gameover) {
            rendertext(c, dfont, "Press m to go to the menu", new Vector2(120 - predicttextwidth(dfont, "Press m to go to the menu") / 2f - 1, round(67.5f + 67.5f * (1 - easeoutback((Time.TotalTime - timeofdeath) / 2f))) + 17 - dfont.charh), shadowcol);
            rendertext(c, dfont, "Press m to go to the menu", new Vector2(120 - predicttextwidth(dfont, "Press m to go to the menu") / 2f, round(67.5f + 67.5f * (1 - easeoutback((Time.TotalTime - timeofdeath) / 2f))) + 16 - dfont.charh), Color.White);
            rendertext(c,dfont, "Press r to restart", new Vector2(120-predicttextwidth(dfont, "Press r to restart")/2f-1,round(67.5f+67.5f*(1-easeoutback((Time.TotalTime-timeofdeath)/2f)))+9-dfont.charh), shadowcol);
            rendertext(c,dfont, "Press r to restart", new Vector2(120-predicttextwidth(dfont, "Press r to restart")/2f,round(67.5f+67.5f*(1-easeoutback((Time.TotalTime-timeofdeath)/2f)))+8-dfont.charh),Color.White);
            rendertext(c,dfont,"Game Over",new Vector2(98,round(67.5f+67.5f*(1-easeoutback((Time.TotalTime-timeofdeath)/2f)))+1-dfont.charh), shadowcol);
            rendertext(c,dfont,"Game Over",new Vector2(99,round(67.5f+67.5f*(1-easeoutback((Time.TotalTime-timeofdeath)/2f)))-dfont.charh),Color.White);

            if (Keyboard.IsKeyPressed(Key.R)) {
                gameover = false;
                Player.Ppos = new Vector2(120, 64);
                Player.canmove = false;
                Player.right = true;
                ammo = 10;
                totalammo = (byte)r.Next(3, 5);
                for (int i = 0; i < totalammo; i++)
                    ammos.Add(new() { pos = new Vector2(r.Next(12, 228), r.Next(12, 100)), spawntime = Time.TotalTime + (float)r.NextDouble() / 6f });
                lastgootime = Time.TotalTime;
                starttime = Time.TotalTime;
            }

            if (Keyboard.IsKeyPressed(Key.M)) {
                gameover = false;
                Player.Ppos = new Vector2(120, 64);
                Player.canmove = false;
                Player.right = true;
                ammo = 10;
                totalammo = (byte)r.Next(3, 5);
                for (int i = 0; i < totalammo; i++)
                    ammos.Add(new() { pos = new Vector2(r.Next(12, 228), r.Next(12, 100)), spawntime = Time.TotalTime + (float)r.NextDouble() / 6f });
                lastgootime = Time.TotalTime;
                starttime = Time.TotalTime;
                menu = true;
            }
        }
    }
}