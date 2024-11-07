using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classic_Snake_Game
{
    class Circle // represents a point or a segment of the snake in the game.
    {
        public int X { get; set; } // X and Y coordinates of the circle
        public int Y { get; set; } // both represent a single segment or food

        public Circle() // constructor used to create a new circle
        {
            X = 0;
            Y = 0;
        }

    }
}
