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

        for (int i = 0; i < ammoposses.Count; i++) {
            c.DrawTexture(ammotex, new (ammoposses[i] + new Vector2(-1,sin(Time.TotalTime*3+i*4)*3+1), new Vector2(6,8), Alignment.Center), shadowcol);
            c.DrawTexture(ammotex, ammoposses[i] + new Vector2(0,sin(Time.TotalTime*3+i*4)*3), new Vector2(6,8), Alignment.Center);

            if (dist(Player.Ppos, ammoposses[i]) < 6) {
                collectammosfx.Play();
                ammo++;
                collammo++;
                ammoposses.RemoveAt(i);
                i--;
            }
        }

        if (!hasgoo && lastgootime + goospawntime <= Time.TotalTime) {
            goopos = new Vector2(r.Next(12, 228), r.Next(12, 100));
            hasgoo = true;
        }

        if (hasgoo) {
            c.DrawTexture(gootex, new (goopos + new Vector2(-1,sin(Time.TotalTime*3)*3+1), new Vector2(8,8), Alignment.Center), shadowcol);
            c.DrawTexture(gootex, goopos + new Vector2(0,sin(Time.TotalTime*3)*3), new Vector2(8,8), Alignment.Center);

            if (dist(Player.Ppos, goopos) < 12) {
                collectgoosfx.Play();
                ammo+=(ushort)(totalammo-collammo+1);
                collammo = totalammo;
                hasgoo = false;
                lastgootime = Time.TotalTime;
            }
        }

        if (collammo == totalammo) {
            ammoposses.Clear();
            totalammo = (byte)r.Next(3, 5);
            collammo = 0;

            for (int i = 0; i < totalammo; i++)
                ammoposses.Add(new Vector2(r.Next(12, 228), r.Next(12, 100)));
        }

        Player.Drawplayer(c);

        rendertext(c, dfont, ammo + " ammo", new Vector2(2,4), shadowcol);
        rendertext(c, dfont, ammo + " ammo", new Vector2(3,3), Color.White);
    }
}