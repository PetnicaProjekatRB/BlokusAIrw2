using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AiCollection
{
    public partial class FormPlayer : Form
    {
        public event EventHandler<string> onPieceSelected;
        
        public FormPlayer()
        {
            InitializeComponent();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            onPieceSelected(this, ((Button)sender).Text);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            onPieceSelected(this, "STOP");
        }

    }
}
