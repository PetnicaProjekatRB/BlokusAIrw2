using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BlokusDll;
using AiCollection;
using System.Threading;

namespace PTv2
{
    public partial class Form1 : Form
    {
        KomadiciOrbitale koms;
        GameGrid grid = new GameGrid();
        Piece[] pieceFamily;
        int orbitCounter;

        public Form1()
        {
            InitializeComponent();
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
                            var ax = square.Key.Item1 * 21 + 1;
                            var ay = 295 - (square.Key.Item2 * 21 + 21);
                            var pl = square.Value.Owner;

                            graphics.FillRectangle((pl == Player.PL1) ? brushOrange : brushPurple,
                                ax, ay, 20, 20);
                        }
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PieceLoader.RebuildPieces();
            koms = PieceLoader.LoadPieces();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            grid = new GameGrid();
            Random r = new Random();
            IBlokusAi ai1 = new AiSmart((float)r.NextDouble() * 5, (float)r.NextDouble() * 5, (float)r.NextDouble() * 5);
            IBlokusAi ai2 = new AiSmart((float)r.NextDouble() * 5, (float)r.NextDouble() * 5, (float)r.NextDouble() * 5);
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
                this.PieceTester_Paint(null, null);
            }

            int p1 = 0; 
            int p2 = 0;

            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    switch (grid.GetSquareCache(i,j))
                    {
                        case Player.PL1: p1++; break;
                        case Player.PL2: p2++; break;
                    }
                }
            }

            MessageBox.Show((p1 > p2 ? "Orange won! " : p1 < p2 ? "Purple won! " : "Draw!") + p1 + "/" + p2);
        }
    }
}