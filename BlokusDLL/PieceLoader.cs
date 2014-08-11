using System;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace BlokusDll
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
            var deckText = File.ReadAllText(@"..\..\komadici.json");
            Komadici komadi = JsonConvert.DeserializeObject<Komadici>(deckText);
            var matricaSvega = new KomadiciOrbiatle();
            matricaSvega.orbitale = new Piece[komadi.pieces.Length][];
            for (int i = 0; i < komadi.pieces.Length; i++)
            {
                var numOrb = komadi.pieces[i].orbits.Count<int>((int x) => (x == 1));
                var orbCnt = 0;
                matricaSvega.orbitale[i] = new Piece[numOrb];

                if (komadi.pieces[i].orbits[0] == 1)
                    matricaSvega.orbitale[i][orbCnt++] = komadi.pieces[i];

                if (komadi.pieces[i].orbits[1] == 1)
                    matricaSvega.orbitale[i][orbCnt++] = komadi.pieces[i].Rot90();

                if (komadi.pieces[i].orbits[2] == 1)
                    matricaSvega.orbitale[i][orbCnt++] = komadi.pieces[i].Rot90().Rot90();

                if (komadi.pieces[i].orbits[3] == 1)
                    matricaSvega.orbitale[i][orbCnt++] = komadi.pieces[i].Rot90().Rot90().Rot90();

                if (komadi.pieces[i].orbits[4] == 1)
                    matricaSvega.orbitale[i][orbCnt++] = komadi.pieces[i].ReflectY();

                if (komadi.pieces[i].orbits[5] == 1)
                    matricaSvega.orbitale[i][orbCnt++] = komadi.pieces[i].ReflectY().Rot90();

                if (komadi.pieces[i].orbits[6] == 1)
                    matricaSvega.orbitale[i][orbCnt++] = komadi.pieces[i].ReflectY().Rot90().Rot90();

                if (komadi.pieces[i].orbits[7] == 1)
                    matricaSvega.orbitale[i][orbCnt++] = komadi.pieces[i].ReflectY().Rot90().Rot90().Rot90();
            }
            var orbitaleText = JsonConvert.SerializeObject(matricaSvega);
            File.WriteAllText(@"..\..\komadiciOrbitale.json", orbitaleText);
        }

        public static int Main(string[] argv)
        {
            LoadPieces();
            return 0;
        }
    }
}
