﻿partial class sploppy {
    static void init() {
        Window.Title = "sploppy";

        Simulation.SetFixedResolution(240, 135, Color.Black, false, false, true);

        ITexture logo = Graphics.LoadTexture(@"assets\logo\logo.png");

        dfonttex = Graphics.LoadTexture(@"assets\fonts\font.png");
        dfont = genfont(dfonttex, " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz");

        sploppertex = Graphics.LoadTexture(@"assets\sprites\splopper.png");
        flippedsploppertex = Graphics.LoadTexture(@"assets\sprites\splopperflipped.png");
        guntex = Graphics.LoadTexture(@"assets\sprites\gun.png");
        flippedguntex = Graphics.LoadTexture(@"assets\sprites\gunflipped.png");
        bgtex = Graphics.LoadTexture(@"assets\sprites\bg.png");
        cloudstex = Graphics.LoadTexture(@"assets\sprites\clouds.png");
        ammotex = Graphics.LoadTexture(@"assets\sprites\ammo.png");
        gootex = Graphics.LoadTexture(@"assets\sprites\goo.png");
        darkcloudstex = Graphics.LoadTexture(@"assets\sprites\darkclouds.png");
        bghardtex = Graphics.LoadTexture(@"assets\sprites\bghard.png");
        bgmastertex = Graphics.LoadTexture(@"assets\sprites\bgmaster.png");
        raintex = Graphics.LoadTexture(@"assets\sprites\rain.png");
        shelltex = Graphics.LoadTexture(@"assets\sprites\shell.png");
        cursoroltex = Graphics.LoadTexture(@"assets\sprites\cursor outline.png");
        cursortex = Graphics.LoadTexture(@"assets\sprites\cursor.png");
        telecrysttex = Graphics.LoadTexture(@"assets\sprites\teleportcrystal.png");

        music = Audio.LoadSound(@"assets\audio\song.wav");
        musicpb = music.Loop();
        shootsfx = Audio.LoadSound(@"assets\audio\shoot.wav");
        shootnoammosfx = Audio.LoadSound(@"assets\audio\shootnoammo.wav");
        collectammosfx = Audio.LoadSound(@"assets\audio\collectammo.wav");
        collectgoosfx = Audio.LoadSound(@"assets\audio\collectgoo.wav");
        windsfx = Audio.LoadSound(@"assets\audio\wind.wav");
        windsfxpb = windsfx.Loop();
        gameoversfx = Audio.LoadSound(@"assets\audio\gameover.wav");
        startgamesfx = Audio.LoadSound(@"assets\audio\start.wav");
        collecttelesfx = Audio.LoadSound(@"assets\audio\collectteleporter.wav");
        highpads = Audio.LoadSound(@"assets\audio\highpads.wav");
        highpadspb = highpads.Loop();
        highpadspb.Volume = 0;
        usecrystalsfx = Audio.LoadSound(@"assets\audio\usecrystal.wav");
        fadebacksfx = Audio.LoadSound(@"assets\audio\fadebackin.wav");

        ammos = new List<collectible>();
        ammosalt = new List<collectible>();
        rainposses = new List<Vector2>();
        shells = new List<shells>();
        particles = new List<particle>();

        totalammo = (byte)r.Next(minammo, maxammo);

        for (int i = 0; i < totalammo; i++)
            ammos.Add(new() { pos = new Vector2(r.Next(12, 228), r.Next(12, 100)), spawntime = totaltime + (float)r.NextDouble()/6f });

        lastgootime = totaltime;
        starttime = totaltime;

        Window.SetIcon(logo);

        lastraintime = totaltime;

        diff = 1;
        gravity = 82;
        gunforce = 112;
        ammo = 10;
        maxammo = 5;
        minammo = 3;
        startammo = 10;

        using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + @"\assets\saves\data.txt")) { 
            string dat = sr.ReadToEnd();
            string[] dats = dat.Split(Environment.NewLine);

            highscoreRM = Convert.ToUInt32(dats[0]);
            highscoreHM = Convert.ToUInt32(dats[1]);
            highscoreMM = Convert.ToUInt32(dats[2]);
        }

        canvas = Graphics.CreateTexture(240,135);

        //Graphics.SwapInterval = 12;
    }
}