using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proiect_Atestat
{
    public partial class Form1 : Form
    {
        bool goLeft, goRight, jumping, hasKey;

        int jumpSpeed = 10;
        int force = 8;
        int score = 0;

        int playerSpeed = 10;
        int backgroundSpeed = 8;


        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score; //inregistreaza scorul banilor
            player.Top += jumpSpeed;

            if(goLeft == true && player.Left > 60) //nu lasa personajul sa iese din decor
            {
                player.Left -= playerSpeed;
            }
            if(goRight == true && player.Left + (player.Width + 60) < this.ClientSize.Width)
            {
                player.Left += playerSpeed;
            }

            //urmatoare linii de cod misca fundalul in functie de directia in care se misca personajul, pentru a da impresia ca el chiar se misca

            if(goLeft == true && background.Left < 0)
            {
                background.Left += backgroundSpeed;
                MoveGameElements("forward");
            }

            if (goRight == true && background.Left > -795)
            {
                background.Left -= backgroundSpeed;
                MoveGameElements("back");
            }

            if(jumping == true)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }

            if(jumping == true && force < 0)
            {
                jumping = false;
            }

            foreach (Control x in this.Controls)
            {
                //face posibil ca personajul sa sara de pe o platforma pe alta
                if (x is PictureBox && (string)x.Tag == "platform") 
                {
                    if(player.Bounds.IntersectsWith(x.Bounds) && jumping == false)
                    {
                        force = 8;
                        player.Top = x.Top - player.Height;
                        jumpSpeed = 0;
                    }
                    x.BringToFront();
                }
                //face posibil ca personajul sa poata aduna banii
                if(x is PictureBox && (string)x.Tag == "coin")
                {
                    if(player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                    {
                        x.Visible = false;
                        score += 1;
                    }
                }
            }
            //pentru ca personajul sa poata lua cheia
            if(player.Bounds.IntersectsWith(key.Bounds))
            {
                key.Visible = false;
                hasKey = true;
            }
            //daca personajul a facut rost de cheie, va putea deschide a 2 a usa pentru a termina jocul
            if(player.Bounds.IntersectsWith(door2.Bounds) && hasKey == true)
            {
                GameTimer.Stop();
                MessageBox.Show("Felicitari! Ti-ai terminat calatoaria. ^_^" + Environment.NewLine + "Apasa OK pentru a reincepe jocul.");
                RestartGame();
            }
            //daca personajul care de pe platforme, atunci va afisa mesajul scris mai jos si se va termina jocul
            if(player.Top + player.Height > this.ClientSize.Height)
            {
                GameTimer.Stop();
                MessageBox.Show("Nooo, ai murit, ne pare rau." + Environment.NewLine + "Apasa OK pentru a reincepe jocul.");
                RestartGame();
            }
        }

        private void background_Click(object sender, EventArgs e)
        {

        }

        private void CloseGame(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); //inchide aplicatia dupa ce este inchis jocul, pentru a nu folosi multa parte din memorie
        }

        private void KeyIsDown(object sender, KeyEventArgs e)//miscarile personajului
        {
            if(e.KeyCode == Keys.Left)//daca sageata stanga este apasata, merge spre stanga
            {
                goLeft = true;
            }
            if(e.KeyCode == Keys.Right)//daca sageata dreapta este apasata, merge spre dreapta
            {
                goRight = true;
            }
            if(e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
            }
        }

        private void txtScore_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {

        }

        private void KeyIsUp(object sender, KeyEventArgs e) //opusul functiei KeyIsDown
        {
            if(e.KeyCode == Keys.Left) 
            {
                goLeft = false;
            }
            if(e.KeyCode == Keys.Right) 
            {
                goRight = false;
            }
            if(jumping == true) // daca space este apasat, personajul sare, si este pus pe false pentru a nu face doublejump
            {
                jumping = false;
            }
        }

        private void RestartGame()
        {
            Form1 newWindow = new Form1();
            newWindow.Show();
            this.Hide();
        }

        private void MoveGameElements(string direction) //muta background-ul pe moment ce personajul inainteaza
        {
            foreach (Control x in this.Controls)
            {
                if(x is PictureBox && (string)x.Tag == "platform" || x is PictureBox && (string)x.Tag == "coin" || x is PictureBox && (string)x.Tag == "key" || x is PictureBox && (string)x.Tag == "door")
                {
                    if(direction == "back")
                    {
                        x.Left -= backgroundSpeed;
                    }
                    if(direction == "forward")
                    {
                        x.Left += backgroundSpeed;
                    }

                }
            }
        }
    }
}
