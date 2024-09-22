partial class sploppy {
    static void init() {
        Window.Title = "sploppy";

        Simulation.SetFixedResolution(240, 135, Color.Black, false, false, true);

        dfonttex = Graphics.LoadTexture(@"assets\fonts\font.png");
        dfont = genfont(dfonttex, " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz");

        sploppertex = Graphics.LoadTexture(@"assets\sprites\splopper.png");
        guntex = Graphics.LoadTexture(@"assets\sprites\gun.png");
        bgtex = Graphics.LoadTexture(@"assets\sprites\bg.png");
        cloudstex = Graphics.LoadTexture(@"assets\sprites\clouds.png");
        ammotex = Graphics.LoadTexture(@"assets\sprites\ammo.png");
        gootex = Graphics.LoadTexture(@"assets\sprites\goo.png");

        music = Audio.LoadSound(@"assets\audio\song.wav");
        musicpb = music.Loop();
        shootsfx = Audio.LoadSound(@"assets\audio\shoot.wav");
        shootnoammosfx = Audio.LoadSound(@"assets\audio\shootnoammo.wav");
        collectammosfx = Audio.LoadSound(@"assets\audio\collectammo.wav");
        collectgoosfx = Audio.LoadSound(@"assets\audio\collectgoo.wav");

        ammoposses = new List<Vector2>();

        totalammo = (byte)r.Next(3,5);

        for (int i = 0; i < totalammo; i++)
            ammoposses.Add(new Vector2(r.Next(12, 228), r.Next(12, 100)));

        lastgootime = Time.TotalTime;
        starttime = Time.TotalTime;
    }
}