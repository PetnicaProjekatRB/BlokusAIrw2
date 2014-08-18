using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BlokusDll;

namespace PiecesTester
{
    public partial class PieceTester : Form
    {
        KomadiciOrbitale koms;
        GameGrid grid = new GameGrid();
        Piece[] pieceFamily;
        int orbitCounter;

        public PieceTester()
        {
            InitializeComponent();
            koms = PieceLoader.LoadPieces();
        }

        private void btnPiece_Click(object sender, EventArgs e)
        {
            foreach (var a in koms.orbitale)
            {
                if (a[0].name == txtPieceName.Text)
                {
                    pieceFamily = a;
                    orbitCounter = 0;
                    updateBoard();
                    return;
                }
            }
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
            var ai1 = new TestAI();
            var ai2 = new minimaxSize.Ai();
            ai1.Start(Player.PL1);
            ai2.Start(Player.PL2);
            ai1.hand = Hand.FullHand.Clone();
            ai2.hand = Hand.FullHand.Clone();
            ai1.hand.UsePiece(7);
            ai2.hand.UsePiece(7);
            grid = new GameGrid();
            grid.Place(3, 3, PieceLoader.matricaSvega.orbitale[7][0], Player.PL1);
            grid.Place(8, 8, PieceLoader.matricaSvega.orbitale[7][0], Player.PL2);

            bool both = true;
            while (both)
            {
                bool first = ai1.Play(ref grid);
                bool second = ai2.Play(ref grid);
                this.RaisePaintEvent(null, null);
                both = first && second;
            }

            while (ai1.Play(ref grid))
            {

                this.RaisePaintEvent(null, null);
            }

            while (ai1.Play(ref grid))
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


    }
}
