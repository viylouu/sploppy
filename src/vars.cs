#nullable disable

partial class sploppy {
    //misc
    static Random r = new Random();
    static ColorF shadowcol = new ColorF(0,0,0,.25f);
    static float starttime;
    static float timeofdeath;
    const byte gravity = 82;
    static bool gameover;

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

    //bg
    const float bgscrollspeed = 240 / 8f;

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