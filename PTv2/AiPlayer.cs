using System;
using System.Linq;
using System.Windows.Forms;
using BlokusDll;

namespace AiCollection
{
    public class AiPlayer : IBlokusAi
    {
        Player me = Player.None;

        FormPlayer playerForm = new FormPlayer();

        public event EventHandler<string> OnDonePlaying;
        public event EventHandler<PaintEventArgs> Redraw;

        Hand hand = Hand.FullHand.Clone();

        public Piece CurPiece;
        public int XPos, YPos;

        public bool Validate()
        {
            return true;
        }

        public bool Start(Player player)
        {
            me = player;

            playerForm.Show();

            playerForm.onPieceSelected += playerForm_onPieceSelected;
            playerForm.KeyDown += playerForm_KeyDown;

            return true;
        }

        public void playerForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.A:
                    XPos--; 
                    break;
                case Keys.W:
                    YPos++; 
                    break;
                case Keys.S:
                    YPos--; 
                    break;
                case Keys.D:
                    XPos++; 
                    break;
                case Keys.Q:
                    CurPiece = CurPiece.Rot90().Rot90().Rot90();
                    break;
                case Keys.E:
                    CurPiece = CurPiece.Rot90();
                    break;
                case Keys.X:
                    CurPiece = CurPiece.ReflectY();
                    break;
                case Keys.P:
                    DisableAll();
                    hand.UsePiece(CurPiece.id);
                    OnDonePlaying(null, null);
                    break;
            }


        }

        void playerForm_onPieceSelected(object sender, string e)
        {
            if (e == "STOP")
            {
                OnDonePlaying(this, e);
                return;
            }

            CurPiece = PieceLoader.matricaSvega.orbitale [PieceLoader.StrToInt [e]] [0];
            Redraw(null, null);
        }

        public int Modify(string name, string value)
        {
            throw new NotImplementedException();
        }

        public bool Play(ref GameGrid grid)
        {
            EnableAll();
            return true;
        }

        public Hand GetHand()
        {
            return hand;
        }

        private void EnableAll()
        {
            foreach (var button in playerForm.Controls)
            {
                if (button is Button)
                {
                    Button b = (Button)button;
                    if (b.Text == "Skip" || hand[PieceLoader.StrToInt[b.Text]])
                    {
                        b.Enabled = true;
                    }
                }
            }
        }

        private void DisableAll()
        {
            foreach (var button in playerForm.Controls)
            {
                if (button is Button)
                {
                    ((Button)button).Enabled = false;
                }
            }
        }
    }
}
