using Silk.NET.OpenGL;

partial class sploppy {
    static void rend(ICanvas c) {
        //Update
        Player.Updateplayer();

        //Draw Calls
        c.Clear(Color.CornflowerBlue);

        Player.Drawplayer(c);

        c.DrawTexture(bgtex);

        c.DrawTexture(cloudstex, new(-Time.TotalTime * bgscrollspeed % 240-1, 1, 240,135, Alignment.TopLeft), shadowcol);
        c.DrawTexture(cloudstex, new(-Time.TotalTime * bgscrollspeed % 240 + 239, 1, 240,135, Alignment.TopLeft), shadowcol);
        c.DrawTexture(cloudstex, -Time.TotalTime * bgscrollspeed % 240, 0, Alignment.TopLeft);
        c.DrawTexture(cloudstex, -Time.TotalTime * bgscrollspeed % 240 + 240, 0, Alignment.TopLeft);

        rendertext(c, dfont, ammo + " ammo", new Vector2(3, 4), shadowcol);
        rendertext(c, dfont, ammo + " ammo", new Vector2(4, 3), Color.White);
        rendertext(c, dfont, Player.Score + "", new Vector2(119, 4), shadowcol);
        rendertext(c, dfont, Player.Score + "", new Vector2(120, 3), Color.White);

        for (int i = 0; i < ammos.Count; i++) {
            float scalemult = easeoutelastic(Time.TotalTime-ammos[i].spawntime);
            c.DrawTexture(ammotex, new (ammos[i].pos + new Vector2(-1,sin(Time.TotalTime*3+i*4)*3+1), new Vector2(6,8)*scalemult, Alignment.Center), shadowcol);
            c.DrawTexture(ammotex, ammos[i].pos + new Vector2(0,sin(Time.TotalTime*3+i*4)*3), new Vector2(6,8)*scalemult, Alignment.Center);

            if (dist(Player.Ppos, ammos[i].pos) < 8) {
                collectammosfx.Play();
                ammo++;
                collammo++;
                ammosalt.Add(new() { pos = ammos[i].pos, spawntime = Time.TotalTime });
                ammos.RemoveAt(i);
                i--;
            }
        }

        if (!hasgoo && lastgootime + goospawntime <= Time.TotalTime) {
            goo.pos = new Vector2(r.Next(12, 228), r.Next(12, 100));
            goo.spawntime = Time.TotalTime + (float)r.NextDouble()/6f;
            hasgoo = true;
        }

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

                for (int i = 0; i < ammos.Count; i++)
                    ammosalt.Add(new() { pos = ammos[i].pos, spawntime = Time.TotalTime });
            }
        }

        if (collammo == totalammo) {
            ammos.Clear();
            totalammo = (byte)r.Next(3, 5);
            collammo = 0;

            for (int i = 0; i < totalammo; i++)
                ammos.Add(new() { pos = new Vector2(r.Next(12, 228), r.Next(12, 100)), spawntime = Time.TotalTime + (float)r.NextDouble()/6f });
        }

        for (int i = 0; i < ammosalt.Count; i++) {
            float scalemult = 1-easeinback(2*(Time.TotalTime-ammosalt[i].spawntime));
            c.DrawTexture(ammotex, new(ammosalt[i].pos + new Vector2(-1, sin(Time.TotalTime * 3 + i * 4) * 3 + 1), new Vector2(6, 8) * scalemult, Alignment.Center), shadowcol);
            c.DrawTexture(ammotex, ammosalt[i].pos + new Vector2(0, sin(Time.TotalTime * 3 + i * 4) * 3), new Vector2(6, 8) * scalemult, Alignment.Center);

            if (Time.TotalTime >= ammosalt[i].spawntime+.5f) {
                ammosalt.RemoveAt(i);
                i--;
            }
        }

        if (hasgooalt) { 
            float scalemult = 1-easeinback(2*(Time.TotalTime-gooalt.spawntime));
            c.DrawTexture(gootex, new(gooalt.pos + new Vector2(-1, sin(Time.TotalTime * 3) * 3 + 1), new Vector2(8, 8) * scalemult, Alignment.Center), shadowcol);
            c.DrawTexture(gootex, gooalt.pos + new Vector2(0, sin(Time.TotalTime * 3) * 3), new Vector2(8, 8) * scalemult, Alignment.Center);

            if (Time.TotalTime >= gooalt.spawntime+.5f)
                hasgooalt = false;
        }

        Player.Drawplayer(c);
        if (Player.gameover) {
            rendertext(c,dfont,"Game Over",new Vector2(98,round(67.5f+67.5f*(1-easeoutback((Time.TotalTime-timeofdeath)/2f)))+1), shadowcol);
            rendertext(c,dfont,"Game Over",new Vector2(99,round(67.5f+67.5f*(1-easeoutback((Time.TotalTime-timeofdeath)/2f)))),Color.White);
        }
    }
}