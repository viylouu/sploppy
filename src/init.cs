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

        music = Audio.LoadSound(@"assets\audio\song.wav");
        musicpb = music.Loop();
        shootsfx = Audio.LoadSound(@"assets\audio\shoot.wav");
        shootnoammosfx = Audio.LoadSound(@"assets\audio\shootnoammo.wav");
    }
}