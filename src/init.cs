partial class sploppy {
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

        ammos = new List<collectible>();
        ammosalt = new List<collectible>();
        rainposses = new List<Vector2>();

        totalammo = (byte)r.Next(minammo, maxammo);

        for (int i = 0; i < totalammo; i++)
            ammos.Add(new() { pos = new Vector2(r.Next(12, 228), r.Next(12, 100)), spawntime = Time.TotalTime + (float)r.NextDouble()/6f });

        lastgootime = Time.TotalTime;
        starttime = Time.TotalTime;

        Window.SetIcon(logo);

        lastraintime = Time.TotalTime;

        diff = 1;
        gravity = 82;
        gunforce = 112;
        ammo = 10;
        maxammo = 5;
        minammo = 3;
        startammo = 10;
    }
}