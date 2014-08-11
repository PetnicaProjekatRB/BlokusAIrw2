﻿using System;
using System.Linq;

namespace BlokusDll
{
    struct Piece
    {
        public string  name;
        public int[][] coords;
        public int[][] liberty;
        public int[]   orbits;

        public Piece Rot90()
        {
            var p = new Piece(coords.Length,liberty.Length);
            for (int i = 0; i < coords.Length; i++)
            {
                p.coords[i] = new int[2];
                p.coords[i][0] =  this.coords[i][1];
                p.coords[i][1] = -this.coords[i][0];
            }

            for (int i = 0; i < liberty.Length; i++)
            {
                p.liberty[i] = new int[3];
                p.liberty[i][0] =  this.liberty[i][1];
                p.liberty[i][1] = -this.liberty[i][0];
                p.liberty[i][2] = (this.liberty[i][2] << 1) & (this.liberty[i][2] > 7 ? 1 : 0);
            }
            return p;
        }

        public Piece ReflectY()
        {
            var p = new Piece(coords.Length, liberty.Length);
            for (int i = 0; i < coords.Length; i++)
            {
                p.coords[i] = new int[2];
                p.coords[i][0] = -this.coords[i][0];
                p.coords[i][1] = this.coords[i][1];
            }
            for (int i = 0; i < liberty.Length; i++)
            {
                p.liberty[i] = new int[3];
                p.liberty[i][0] = -this.liberty[i][0]; 
                p.liberty[i][1] = this.liberty[i][1];
                p.liberty[i][2] = (int)Reverse((uint)this.liberty[i][2]);
            }
            return p;
        }

        public Piece(int i, int j)
        {
            name = "";
            coords = new int[i][];
            liberty = new int[j][];
            orbits = new int[8];
        }

        static uint Reverse(uint x)
        {
            uint y = 0;
            for (int i = 0; i < 4; ++i)
            {
                y <<= 1;
                y |= (x & 1);
                x >>= 1;
            }
            return y;
        }
    }
}
