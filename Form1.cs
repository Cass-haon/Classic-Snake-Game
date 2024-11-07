using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging; // has a lot of classes for working with images. Jpg compressor in it.

namespace Classic_Snake_Game
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>(); // list of circles for the snake
        private Circle food = new Circle(); // food for the snake

        int maxWidth; // max weight for the snake
        int maxHeight; // max height for the snake

        int score;
        int highScore;

        Random rand = new Random(); // creatinmg random number for the food

        bool goLeft, goRight, goUp, goDown; // as they are instatitaed they will be set to false initially

        public Form1()
        {
            InitializeComponent();
            new Settings(); // alot of the settings are set in the settings class just create a new insatnce
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && Settings.directions != "right") // If the left arrow key (Keys.Left) is pressed and the current direction is not "right", it sets the goLeft flag to true.
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Settings.directions != "left") // If the right arrow key (Keys.Right) is pressed and the current direction is not "left", it sets the goRight flag to true.
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && Settings.directions != "down")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down && Settings.directions != "up")
            {
                goDown = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e) //  handles the event when a key is released. This method is part of the input handling for controlling the snake's movement in the game. 
        {
            if (e.KeyCode == Keys.Left) // If the left arrow key (Keys.Left) is released, it sets the goLeft flag to false.
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right) // If the right arrow key (Keys.Right) is released, it sets the goRight flag to false.
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
            //By setting these flags to false when the key is released, the game ensures that the snake stops moving in that direction unless the key is pressed again.

        }

        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();

        }

        private void TakeSnapShot(object sender, EventArgs e)
        {
            Label caption = new Label(); // a label control is used to display text on the snapshot file
            caption.Text = "You scored: " + score + " and your High Score is: " + highScore + " on the Classic Snake Game!";
            caption.Font = new Font("Arial", 13, FontStyle.Bold);
            caption.ForeColor = Color.AliceBlue;
            caption.AutoSize = false;
            caption.Width = picCanvas.Width;
            caption.Height = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Classic_Snake_Game_SnapShot";
            dialog.DefaultExt = ".jpg";
            dialog.Filter = "JPG Image File | *.jpg";// filter gives the user the option to save the file as a jpg file
            dialog.ValidateNames = true; // validates the name of the file

            if (dialog.ShowDialog() == DialogResult.OK) // if the user clicks ok This line displays a `SaveFileDialog` to the user. If the user clicks "OK" in the dialog, the code inside the `if` block will execute.
            {
                int width  = Convert.ToInt32(picCanvas.Width); //These lines retrieve the width and height of the `picCanvas` control, which is presumably where the game is rendered.
                int height = Convert.ToInt32(picCanvas.Height);
                Bitmap bmp = new Bitmap(width, height); // create a bitmap object
                picCanvas.DrawToBitmap(bmp, new Rectangle(0, 0, width, height)); // draw the picture box to the bitmap
                bmp.Save (dialog.FileName, ImageFormat.Jpeg); // save the bitmap to the file This line saves the `Bitmap` object to a file. The file path and name are specified by the user through the `SaveFileDialog`, and the image is saved in JPEG format.
                picCanvas.Controls.Remove(caption); // remove the caption from the picture box to play game again
            }
        }

        private void GameTimerEvent(object sender, EventArgs e) // this method is called every time the game timer ticks. It is responsible for updating the game state and rendering the game.
        {
            // setting the directions
            if (goLeft)
            {
                Settings.directions = "left"; // if the snake is going left
            }
            if (goRight)
            {
                Settings.directions = "right";
            }
            if (goUp)
            {
                Settings.directions = "up";
            }
            if (goDown)
            {
                Settings.directions = "down";
            }
            // end of the directions settings
            
            for (int i = Snake.Count -1; i >= 0; i--) // how many children are inside that list
            {
                if (i ==0)
                {
                    switch (Settings.directions) // switch statement to determine the direction of the snake de
                    {
                        case "left":
                            Snake[i].X--; // if its move left deduct one from the position of the snake
                            break;
                        case "right":
                            Snake[i].X++; // if its move right add one to the position of the snake
                            break;
                        case "down":
                            Snake[i].Y++; // if its move down add one to the position of the snake
                            break;
                        case "up":
                            Snake[i].Y--; // if its move up deduct one from the position of the snake
                            break;
                    }
                    if (Snake[i].X <0 )
                    {
                        Snake[i].X = maxWidth; // if it goesd to far in one corner, it goes to the other cornerr
                    }
                    if (Snake[i].X >maxWidth) // if the snake goes to far in the x direction
                    {
                        Snake[i].X = 0;
                    }
                    if (Snake[i].Y <0) // if the snake goes to far in the y direction
                    {
                        Snake[i].Y = maxHeight;

                    }
                    if (Snake[i].Y > maxHeight) // if the snake goes to far in the y direction
                    {
                        Snake[i].Y = 0;
                    }

                    if (Snake[i].X == food.X && Snake[i].Y == food.Y) // if the snake eats the food
                    {
                        EatFood(); // call the eat food method
                    }
                    for (int j = 1; j < Snake.Count; j++) // logic if the snake eats hits body
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y) // if the snake hits its body
                        {
                            GameOver(); // call the game over method

                            MessageBox.Show("You have died!   ,--.\r\n                           {    }\r\n                           K,   }\r\n                          /  ~Y`\r\n                     ,   /   /\r\n                    {_'-K.__/\r\n                      `/-.__L._\r\n                      /  ' /`\\_}\r\n                     /  ' /\r\n             ____   /  ' /\r\n      ,-'~~~~    ~~/  ' /_\r\n    ,'             ``~~~  ',\r\n   (                        Y\r\n  {                         I\r\n {      -                    `,\r\n |       ',                   )\r\n |        |   ,..__      __. Y\r\n |    .,_./  Y ' / ^Y   J   )|\r\n \\           |' /   |   |   ||\r\n  \\          L_/    . _ (_,.'(\r\n   \\,   ,      ^^\"\"' / |      )\r\n     \\_  \\          /,L]     /\r\n       '-_~-,       ` `   ./`\r\n          `'{_            )\r\n              ^^\\..___,.--` ");
                        }
                    }



                }
                else
                {
                    Snake[i].X = Snake[i - 1].X; // the body of the snake will follow the head
                    Snake[i].Y = Snake[i - 1].Y; // the body of the snake will follow the head
                }

            }
            picCanvas.Invalidate(); // refresh the picture box
        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics; // create a graphics object linking paint event args to the graphics object
            Brush snakeColour; // create a brush object for the snake

            for (int i = 0; i < Snake.Count; i++)
            {
                if (i == 0)
                {
                    snakeColour = Brushes.Black; // head of the snake
                }
                else
                {
                    snakeColour = Brushes.DarkGreen; // body of the snake
                }

                canvas.FillEllipse(snakeColour, new Rectangle(Snake[i].X * Settings.Width, Snake[i].Y * Settings.Height, Settings.Width, Settings.Height)); // draw the snake
                //canvas.FillEllipse(Brushes.Red, new Rectangle(food.X * Settings.Width, food.Y * Settings.Height, Settings.Width, Settings.Height)); // draw the food
            }
            canvas.FillEllipse(Brushes.DarkRed, new Rectangle(food.X * Settings.Width, food.Y * Settings.Height, Settings.Width, Settings.Height)); // draw the food
        }

        private void RestartGame()
        {
            maxWidth = picCanvas.Width / Settings.Width -1; //  a littlke bit of pid of padding for the snake so it doesnt go off the screen
            maxHeight = picCanvas.Height / Settings.Height - 1; // 
            Snake.Clear(); // any existing child nodes are removed from list

            startbutton.Enabled = false; // start button is disabled
            snapButton.Enabled = false; // if butn is enabled automativcall the buttons
            score = 0;
            txtScore.Text = "Score: " + score;

            Circle head = new Circle { X = 10, Y = 5 }; // head of the snake
            Snake.Add(head); // add the head to the snake. adding the head part of the snake to the list
            for ( int i = 0; i < 10; i++)
            {
                Circle body = new Circle(); // adding the body of the snake
                Snake.Add(body); // adding the body to the snake
            }
            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) }; // random location for the food
            gameTimer.Start(); // start the game timer
        }

        private void EatFood()
        {
            score += 1; // increment the score by 1

            txtScore.Text = "Score: " + score; // update the score

            Circle body = new Circle // create a new body for the snake
            {
                X = Snake[Snake.Count - 1].X, // add the body to the snake
                Y = Snake[Snake.Count - 1].Y // add the body to the snake

            };

            Snake.Add(body); // add to the body when snake eats
            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) }; // random location for the food
        }
        private void GameOver() // 
        {
            gameTimer.Stop();
            startbutton.Enabled = true;
            snapButton.Enabled = true;

            if (score > highScore) // if the score is greater than the high score
            {
                highScore = score;
                txtHighScore.Text = "High Score: " + highScore;// update the high score
                txtHighScore.ForeColor = Color.Maroon; // set the color of the text to maroon
                txtHighScore.TextAlign = ContentAlignment.MiddleCenter; // center the text
            }
        }

    }
}
