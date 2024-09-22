#nullable disable

partial class sploppy {
    //misc
    static Random r = new Random();
    static ColorF shadowcol = new ColorF(0,0,0,.25f);
    static float starttime;
    static float timeofdeath;
    static bool gameover;
    static byte diff = 1;
    const float bgscrollspeed = 240 / 8f;

    //player stuff
    static byte gravity = 82;
    static byte gunforce = 112;

    //rain
    static float rainspeed = 1024;
    static List<Vector2> rainposses;
    static float lastraintime;
    static float rainspawnfreq = .006125f;
    static float raindir = 11*pi/6f;
    static float pid4 = pi / 4f;
    static float pid2pd4 = pi / 2f + pid4;

    //font
    static ITexture dfonttex;
    static font dfont;

    //textures
    static ITexture sploppertex;
    static ITexture flippedsploppertex;
    static ITexture guntex;
    static ITexture flippedguntex;
    static ITexture cloudstex;
    static ITexture bgtex;
    static ITexture ammotex;
    static ITexture gootex;
    static ITexture darkcloudstex;
    static ITexture bghardtex;
    static ITexture bgmastertex;
    static ITexture raintex;

    //audio
    static ISound music;
    static SoundPlayback musicpb;
    static ISound shootsfx;
    static ISound shootnoammosfx;
    static ISound collectammosfx;
    static ISound collectgoosfx;
    static ISound windsfx;
    static SoundPlayback windsfxpb;
    static ISound gameoversfx;
    static ISound startgamesfx;

    //ammo
    static ushort ammo = 10;
    static List<collectible> ammos;
    static byte totalammo;
    static byte collammo;
    static List<collectible> ammosalt;

    //goo
    static collectible goo = new();
    static collectible gooalt = new();
    static bool hasgoo = false;
    static bool hasgooalt = false;
    static float lastgootime = 0;
    static float goospawntime = 6;
}