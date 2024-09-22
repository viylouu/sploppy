#nullable disable

partial class sploppy {
    //misc
    static Random r = new Random();
    static ColorF shadowcol = new ColorF(0,0,0,.25f);
    static float starttime;

    //font
    static ITexture dfonttex;
    static font dfont;

    //textures
    static ITexture sploppertex;
    static ITexture guntex;
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

    //bg
    static float bgscrollspeed = 240 / 8f;

    //gameplay
    static ushort ammo = 10;
    static List<Vector2> ammoposses;
    static byte totalammo;
    static byte collammo;

    static Vector2 goopos;
    static bool hasgoo = false;
    static float lastgootime = 0;
    static float goospawntime = 6;
}