partial class sploppy
{
    public class Player
    {
        const int gravity = 8;
        const int Drag = 2;

        static Vector2 Ppos = new Vector2(100, 20);
        static Vector2 Pvel = new Vector2(0, 0);

        public static void Updateplayer()
        {
            //Update Position
            Ppos += Pvel * Time.DeltaTime;

            //Gravity
            Pvel.Y += gravity * Time.DeltaTime;

            //Input
            if (Keyboard.IsKeyPressed(Key.E)) Pvel.Y = -16;
        }

        public static void Drawplayer(ICanvas canvas)
        {
            // Drawing the Player
            canvas.Fill(Color.Green);
            canvas.DrawRect(Ppos,new Vector2(8,6),Alignment.Center);
        }
    }
}
