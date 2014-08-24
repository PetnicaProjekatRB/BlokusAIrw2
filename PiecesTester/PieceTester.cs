using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BlokusAIrw3;
using System.Threading;

namespace PiecesTester
{
    public partial class PieceTester : Form
    {
        Game1v1 game = new Game1v1();
        public static int SQUARE_SIZE = 20;

        private void PieceTester_Paint(object sender, PaintEventArgs e)
        {
            using (var graph = this.CreateGraphics())
            {
                using (Pen pen = new Pen(Color.FromArgb(32, 32, 32)))
                {
                    for (int i = 0; i < Game1v1.BOARD_ROWS + 1; i++)
                    {
                        graph.DrawLine(pen, 0, i * SQUARE_SIZE + i, (SQUARE_SIZE + 1) * (Game1v1.BOARD_ROWS + 1), i * SQUARE_SIZE + i);
                        graph.DrawLine(pen, i * SQUARE_SIZE + i, 0, i * SQUARE_SIZE + i, (SQUARE_SIZE + 1) * (Game1v1.BOARD_ROWS + 1));
                    }
                }
            }
        }

        private void PieceTester_Load(object sender, EventArgs e)
        {
            RaisePaintEvent(null, null);
            Thread.Sleep(5000);
            throw new Exception();
        }


    }
}
