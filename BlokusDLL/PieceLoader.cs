using System;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace BlokusDLL
{
    class PieceLoader
    {
        public struct Komadici
        {
            public Piece[] pieces;
        }

        public struct KomadiciOrbiatle
        {
            public Piece[][] orbitale;
        }

        public static void LoadPieces()
        {
            var deckText = File.ReadAllText("C:\\komadici.json");
            Komadici komadi = JsonConvert.DeserializeObject<Komadici>(deckText);
            var matricaSvega = new KomadiciOrbiatle();
            matricaSvega.orbitale = new Piece[komadi.pieces.Length][];
            for (int i = 0; i < komadi.pieces.Length; i++)
            {
                matricaSvega.orbitale[i] = new Piece[8];
                matricaSvega.orbitale[i][0] = komadi.pieces[i];
                matricaSvega.orbitale[i][1] = komadi.pieces[i].Rot90();
                matricaSvega.orbitale[i][2] = komadi.pieces[i].Rot90().Rot90();
                matricaSvega.orbitale[i][3] = komadi.pieces[i].Rot90().Rot90().Rot90();
                matricaSvega.orbitale[i][4] = komadi.pieces[i].ReflectY();
                matricaSvega.orbitale[i][5] = komadi.pieces[i].ReflectY().Rot90();
                matricaSvega.orbitale[i][6] = komadi.pieces[i].ReflectY().Rot90().Rot90();
                matricaSvega.orbitale[i][7] = komadi.pieces[i].ReflectY().Rot90().Rot90().Rot90();
            }
            var orbitaleText = JsonConvert.SerializeObject(matricaSvega);
            File.WriteAllText("C:\\komadiciOrbitale.json", orbitaleText);
        }

        public static int Main(string[] argv)
        {
            LoadPieces();
            return 0;
        }
    }
}
