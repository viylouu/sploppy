#nullable disable

partial class sploppy {
    //misc
    static Random r = new Random();
    static ColorF shadowcol = new ColorF(0,0,0,.25f);
    static float starttime, timeofdeath, cursorsize = 1;
    static bool gameover, fullscreen, darkclouds;
    static byte diff = 1;
    const float bgscrollspeed = 240 / 8f;
    static uint highscoreRM, highscoreHM, highscoreMM, scoredisp = 0;
    static Vector2 cursorpos;
    static screenshader sshad = new();

    //camerashake
    static Vector2 camshake;
    static Vector2 camv /*vel*/, camep /*equilibrium pos*/;
    const float camk = 400 /*stiff*/, camb = 10000 /*damping*/, camm = 1 /*mass*/;

    //bg grad
    static dithergradient bggrad = new();
    static ColorF bggradc1RM = new Color(77,166,255).ToColorF(), bggradc2RM = new Color(102,255,227).ToColorF(),
                  bggradc1HM = new Color(176,48,92).ToColorF(), bggradc2HM = new Color(235,86,75).ToColorF(),
                  bggradc1MM = new Color(128,54,107).ToColorF(), bggradc2MM = new Color(189,72,130).ToColorF();
    static Vector2 bggradsp = new(0,135), bggradep = new(240,0);

    //debug
    static bool debug = false, showtraj = false;

    //bot
    static bool bot = false, travelling = false;
    static float lasttraveltime = 0;

    //player stuff
    static byte gravity = 82, gunforce = 112;
    static Vector2 gunpos;
    static float gunrot;

    //rain
    static List<Vector2> rainposses;
    static float lastraintime, rainspeed = 1024, rainspawnfreq = .006125f, raindir = 11*pi/6f, pid4 = pi/4f, pid2pd4 = pi / 2f + pid4;

    //font
    static ITexture dfonttex;
    static font dfont;

    //textures
    static ITexture sploppertex,flippedsploppertex,guntex,flippedguntex,cloudstex,bgtex,ammotex,gootex,darkcloudstex,bghardtex,bgmastertex,raintex,
                    shelltex,cursoroltex,cursortex,telecrysttex;

    //audio
    static ISound music,shootsfx,shootnoammosfx,collectammosfx,collectgoosfx,windsfx,gameoversfx,startgamesfx,collecttelesfx,highpads,usecrystalsfx,fadebacksfx;
    static SoundPlayback musicpb,windsfxpb,highpadspb;

    //ammo
    static ushort ammo = 10;
    static List<collectible> ammos,ammosalt;
    static byte startammo,maxammo,minammo,totalammo,collammo;

    //shells
    static List<shells> shells;

    //goo
    static collectible goo = new(), gooalt = new();
    static bool hasgoo, hasgooalt;
    static float lastgootime, goospawntime = 6;

    //telecrystals
    static byte crystals = 0;
    static float maxcursorsize = 3.5f;
    static float highness = 0;
    static float gamespeedmult = .5f;
    static bool high = false;
    static bool canshoot = true;
    static float totaltime = 0;

    //canvas tex
    static ITexture canvas;

    //delta time
    static float delta;

    //particles
    static Color polcol = new Color(39,39,54); //particle outline color (p-ol-col)
    static List<particle> particles;
}