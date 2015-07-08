#define GUI

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AiCollection;
using BlokusDll;

namespace PTv2
{
    public partial class Form1 : Form
    {
        KomadiciOrbitale koms;
        GameGrid grid = new GameGrid();
        Piece[] pieceFamily;
        int orbitCounter;

        public IBlokusAi ai1, ai2;

        public static Form1 instance;

        public Form1()
        {
            InitializeComponent();

            instance = this;

            koms = PieceLoader.LoadPieces();
        }

        private void btnPiece_Click(object sender, EventArgs e)
        {

        }
        private void updateBoard(bool defalt)
        {
            if (defalt)
            {
                grid = new GameGrid();
                grid.Place(5, 5, pieceFamily[orbitCounter], Player.PL1, false);
            }
            PieceTester_Paint(null, null);
        }

        private void updateBoard()
        { updateBoard(true); }

        private void btnOrbitPrev_Click(object sender, EventArgs e)
        {
            if (orbitCounter > 0)
            {
                orbitCounter--;
                updateBoard();
            }
        }

        private void btnOrbitNext_Click(object sender, EventArgs e)
        {
            if (orbitCounter < pieceFamily.Length - 1)
            {
                orbitCounter++;
                updateBoard();
            }
        }

        private void PieceTester_Paint(object sender, PaintEventArgs e)
        {
#if GUI
            using (var graphics = CreateGraphics())
            {
                graphics.Clear(this.BackColor);
                using (var pen = new Pen(Color.FromArgb(255, 64, 64, 64)))
                {
                    for (int i = 0; i < 15; i++)
                    {
                        graphics.DrawLine(pen, 0, i * 21, 294, i * 21);
                        graphics.DrawLine(pen, i * 21, 0, i * 21, 294);
                    }
                }

                using (var brushOrange = new SolidBrush(Color.Orange))
                {
                    using (var brushPurple = new SolidBrush(Color.Purple))
                    {
                        foreach (var square in grid.Squares)
                        {
                            drawMyThing(square.Key.Item1, square.Key.Item2, (square.Value.Owner == Player.PL1 ? brushOrange : brushPurple), graphics);
                        }

                        
                    }
                }

                AiPlayer player = ai1 as AiPlayer;
                if (player != null)
                {
                    if (player.CurPiece != null)
                    {
                        Color c = Color.Red;
                        if (grid.Validate(player.XPos, player.YPos, player.CurPiece, Player.PL1))
                            c = Color.Green;

                        using (var brushPlayer = new SolidBrush(c))
                        {
                            foreach (var s in player.CurPiece.coords)
                            {
                                drawMyThing(s [0] + player.XPos, s [1] + player.YPos, brushPlayer, graphics);
                            }
                        }
                    }
                }
            }
#endif
        }

        private void drawMyThing(int x, int y, SolidBrush brush, Graphics graphics)
        {
            int ax = x * 21 + 1;
            int ay = 295 - (y * 21 + 21);

            graphics.FillRectangle(brush, ax, ay, 20, 20);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PieceLoader.RebuildPieces();
            koms = PieceLoader.LoadPieces();
        }

        private void button2_Click(object sender, EventArgs e)
        {
#if !GUI
            int[,,] scoregrid = new int[10, 10, 10];
            for(int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    for (int k = 0; k < 7; k++)
                    {
                        for (int n = 0; n < 7; n++)
                        {
                            scoregrid[i,j,k] += PlayGame(1,3.5f*i,3.5f*j,3.5f*k,1,1,1,1);
                        }
                    }
                }
            }
            Console.WriteLine(JsonConvert.SerializeObject(scoregrid));
#endif
            Random r = new Random();

            ai1 = new AiSmart((float)r.NextDouble() * 5, 
                (float)r.NextDouble() * 5, (float)r.NextDouble() * 5, (float)r.NextDouble() * 5, 1.5f);

            ai2 = new AiSmart((float)r.NextDouble() * 5, 
                (float)r.NextDouble() * 5, (float)r.NextDouble() * 5, (float)r.NextDouble() * 5, 1);

            var game = PlayGame(ai1, ai2);
            MessageBox.Show(game > 0 ? "Narandzasti pobedio!" : (game < 0 ? "Ljubicasti pobedio!" : "Nereseno!"));
        }

        private int PlayGame(IBlokusAi ai1, IBlokusAi ai2)
        {
            grid = new GameGrid();
            this.PieceTester_Paint(null, null);
            ai1.Start(Player.PL1);
            ai2.Start(Player.PL2);
            bool k1 = true, k2 = true;
            while (k1 && k2)
            {
                k1 = ai1.Play(ref grid);
                this.PieceTester_Paint(null, null);

                k2 = ai2.Play(ref grid);
                this.PieceTester_Paint(null, null);
            }

            while (k1)
            {
                k1 = ai1.Play(ref grid);
                this.PieceTester_Paint(null, null);
            }

            while (k2)
            {
                k2 = ai2.Play(ref grid);
                //this.PieceTester_Paint(null, null);
            }

            int p1 = 0;
            int p2 = 0;

            for (int k = 0; k < 14; k++)
            {
                for (int l = 0; l < 14; l++)
                {
                    switch (grid.GetSquareCache(k, l))
                    {
                        case Player.PL1: p1++; break;
                        case Player.PL2: p2++; break;
                    }
                }
            }

            return p1 - p2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ai1 = new AiPlayer();

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;

            Random r = new Random();
            ai2 = new AiSmart((float)r.NextDouble() * 5,
                (float)r.NextDouble() * 5, (float)r.NextDouble() * 5, (float)r.NextDouble() * 5);

            ai1.Start(Player.PL1);
            ai2.Start(Player.PL2);

            ((AiPlayer)ai1).OnDonePlaying += Form1_onDonePlaying;
            ((AiPlayer)ai1).Redraw += PieceTester_Paint;

        }

        void Form1_onDonePlaying(object sender, string e)
        {
            bool stop = e == "STOP";
            
            AiPlayer pl = (AiPlayer)ai1;

            if (!stop)
            {
                grid.Place(pl.XPos, pl.YPos, pl.CurPiece, Player.PL1);

                pl.CurPiece = null;

                ai2.Play(ref grid);
            }
            PieceTester_Paint(null, null);

            if (stop) 
            {
                while (ai2.Play(ref grid))
                {
                    PieceTester_Paint(null, null);
                }

                int score = 0;

                for (int k = 0; k < 14; k++)
                {
                    for (int l = 0; l < 14; l++)
                    {
                        switch (grid.GetSquareCache(k, l))
                        {
                            case Player.PL1: score++; break;
                            case Player.PL2: score--; break;
                        }
                    }
                }

                MessageBox.Show(""+score);
            }
            ai1.Play(ref grid);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            ((AiPlayer)ai1).playerForm_KeyDown(this, e);
            PieceTester_Paint(null, null);
        }
    }
}