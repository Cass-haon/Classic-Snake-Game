using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classic_Snake_Game
{
    class Settings // used to store configuration settings for the game.
    {
        public static int Width { get; set; } // 
        public static int Height { get; set; }
        public static string directions; // keeps track of the current direction of the snake's movement.

        public Settings() //comstruvt used to store configuration settings for the game.
        { // initalize default values to the game
            Width = 16; // the width of the game board
            Height = 16; // the height of the game board
            directions = "left"; // the initial direction of the snake
        }



    }
}
