using System;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace BlokusDll
{
    public class PieceLoader
    {
        public struct Komadici
        {
#pragma warning disable 0649
            public Piece[] pieces;
#pragma warning restore 0649
        }
        

        public static void RebuildPieces()
        {
            var deckText = File.ReadAllText(@"..\..\..\BlokusDll\komadici.json");
            Komadici komadi = JsonConvert.DeserializeObject<Komadici>(deckText);
            var matricaSvega = new KomadiciOrbitale();
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
            File.WriteAllText(@"..\..\..\BlokusDll\komadiciOrbitale.json", orbitaleText);
        }

        public static KomadiciOrbitale LoadPieces()
        {
            return JsonConvert.DeserializeObject<KomadiciOrbitale>(
                File.ReadAllText(@"..\..\..\BlokusDll\komadiciOrbitale.json"));
        }
    }
}
