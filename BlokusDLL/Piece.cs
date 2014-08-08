using System;
using System.Linq;

namespace BlokusDLL
{
    struct Piece
    {
        public int[][] coords;
        public int[][] liberty;

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
                p.liberty[i] = new int[2];
                p.liberty[i][0] =  this.liberty[i][1];
                p.liberty[i][1] = -this.liberty[i][0];
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
                p.liberty[i] = new int[2];
                p.liberty[i][0] = -this.liberty[i][0]; 
                p.liberty[i][1] = this.liberty[i][1];
            }
            return p;
        }

        public Piece(int i, int j)
        {
            coords = new int[i][];
            liberty = new int[j][];
        }
    }
}
