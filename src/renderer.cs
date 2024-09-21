partial class sploppy {
    static void rend(ICanvas c) {
        c.Clear(Color.Black);

        rendertext(c, dfont, $"{round(1/Time.DeltaTime)} fps", new Vector2(3,3), Color.White);
    }
}