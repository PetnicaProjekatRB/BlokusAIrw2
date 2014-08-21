using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BlokusDll;
using System.Threading;

namespace PiecesTester
{
    public partial class PieceTester : Form
    {
        KomadiciOrbitale koms;
        GameGrid grid = new GameGrid();
        GameGrid pomocni = new GameGrid();
        Piece[] pieceFamily;
        int orbitCounter;

        public PieceTester()
        {
            InitializeComponent();
            koms = PieceLoader.LoadPieces();
        }

        private void btnPiece_Click(object sender, EventArgs e)
        {
            /*foreach (var a in koms.orbitale)
            {
                if (a[0].name == txtPieceName.Text)
                {
                    pieceFamily = a;
                    orbitCounter = 0;
                    updateBoard();
                    return;
                }
            }*/
            pomocni.Place(1, 1, PieceLoader.matricaSvega.orbitale[7][0], Player.PL1, false);
            RaisePaintEvent(null, null);
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

                    for (int i = 0; i < 6; i++)
                    {
                        graphics.DrawLine(pen, checkBox1.Location.X, checkBox1.Location.Y + (i * 21) + 30, checkBox1.Location.X + 105, checkBox1.Location.Y + (i * 21) + 30);
                        graphics.DrawLine(pen, checkBox1.Location.X + (i * 21), checkBox1.Location.Y + 30, checkBox1.Location.X + (i * 21), checkBox1.Location.Y + 105 + 30);
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

                        foreach (var square in pomocni.Squares)
                        {
                            if (square.Key.Item1 > 4 || square.Key.Item2 > 4)
                                continue;

                            var ax = square.Key.Item1 * 21 + checkBox1.Location.X + 1;
                            var ay = 106 - (square.Key.Item2 * 21 + 21) + checkBox1.Location.Y + 30;
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
            var ai1 = new TestAI();
            var ai2 = new minimaxSize.Odbacivalje(); 
            ai1.Start(Player.PL1);
            ai2.Start(Player.PL2);
            ai1.hand = Hand.FullHand.Clone();
            ai1.hand.UsePiece(7);
            ai2.hand.UsePiece(7);
            if(checkBox1.Checked)
            {
                ai1.hand.UsePiece(0);
                ai2.hand.UsePiece(0);
                ai2.hand.UsePiece(1);
                ai2.potez += 2;
            }
            grid = new GameGrid();

            grid.Place(8, 8, PieceLoader.matricaSvega.orbitale[7][0], Player.PL2);
            this.RaisePaintEvent(null, null);
            Thread.Sleep(1000);
            grid.Place(3, 3, PieceLoader.matricaSvega.orbitale[7][0], Player.PL1);
            this.RaisePaintEvent(null, null);
            Thread.Sleep(1000);
            if (checkBox1.Checked)
            {

                grid.Place(12, 11, PieceLoader.matricaSvega.orbitale[1][7], Player.PL2, false);
                this.RaisePaintEvent(null, null);
                Thread.Sleep(1000);
                grid.Place(9, 6, PieceLoader.matricaSvega.orbitale[0][2], Player.PL1, false);
                this.RaisePaintEvent(null, null);
                Thread.Sleep(1000);
                grid.Place(4, 7, PieceLoader.matricaSvega.orbitale[0][0], Player.PL2, false);
                this.RaisePaintEvent(null, null);
               
            }
            this.RaisePaintEvent(null, null);
            bool both = true;
            while (both)
            {
                bool first = ai1.Play(ref grid);
                this.RaisePaintEvent(null, null);
                bool second = ai2.Play(ref grid);
                this.RaisePaintEvent(null, null);
                both = first && second;
            }

            while (ai1.Play(ref grid))
            {

                this.RaisePaintEvent(null, null);
            }

            while (ai2.Play(ref grid))
            {

                this.RaisePaintEvent(null, null);
            }

            int scoreO = 0;
            int scoreP = 0;

            foreach (var s in grid.Squares)
            {
                if (s.Value.Owner == Player.PL1)
                    scoreO++;
                else
                    scoreP++;
            }

            MessageBox.Show((
                (scoreO > scoreP) ? "Orange won! " : (scoreO < scoreP) ? "Purple won! " : "Draw! ") +
                            scoreO.ToString() + " / " + scoreP.ToString());

            updateBoard(false);
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


    }
}
