partial class sploppy {
    static void init() {
        Window.Title = "sploppy";

        dfonttex = Graphics.LoadTexture(@"assets\fonts\font.png");
        dfont = genfont(dfonttex, " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz");
    }
}