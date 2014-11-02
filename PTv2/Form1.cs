﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BlokusDll;
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
            
        }


    }
}