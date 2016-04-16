using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Fablab_Creature
{
    class Program
    {
        static void Main(string[] args)
        {
            GameWindow window = new GameWindow(800, 600, OpenTK.Graphics.GraphicsMode.Default, "This is the title", GameWindowFlags.Default);
            Game game = new Game(window);

            window.Run();
        }
    }
}
