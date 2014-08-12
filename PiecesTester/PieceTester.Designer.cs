namespace PiecesTester
{
    partial class PieceTester
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnPiece = new System.Windows.Forms.Button();
            this.txtPieceName = new System.Windows.Forms.TextBox();
            this.btnOrbitPrev = new System.Windows.Forms.Button();
            this.btnOrbitNext = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.bgwAiWorker = new System.ComponentModel.BackgroundWorker();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPiece
            // 
            this.btnPiece.Location = new System.Drawing.Point(614, 38);
            this.btnPiece.Name = "btnPiece";
            this.btnPiece.Size = new System.Drawing.Size(148, 25);
            this.btnPiece.TabIndex = 0;
            this.btnPiece.Text = "Get Piece";
            this.btnPiece.UseVisualStyleBackColor = true;
            this.btnPiece.Click += new System.EventHandler(this.btnPiece_Click);
            // 
            // txtPieceName
            // 
            this.txtPieceName.Location = new System.Drawing.Point(614, 12);
            this.txtPieceName.Name = "txtPieceName";
            this.txtPieceName.Size = new System.Drawing.Size(148, 20);
            this.txtPieceName.TabIndex = 1;
            // 
            // btnOrbitPrev
            // 
            this.btnOrbitPrev.Location = new System.Drawing.Point(614, 69);
            this.btnOrbitPrev.Name = "btnOrbitPrev";
            this.btnOrbitPrev.Size = new System.Drawing.Size(72, 23);
            this.btnOrbitPrev.TabIndex = 2;
            this.btnOrbitPrev.Text = "Prev Orbit";
            this.btnOrbitPrev.UseVisualStyleBackColor = true;
            this.btnOrbitPrev.Click += new System.EventHandler(this.btnOrbitPrev_Click);
            // 
            // btnOrbitNext
            // 
            this.btnOrbitNext.Location = new System.Drawing.Point(692, 69);
            this.btnOrbitNext.Name = "btnOrbitNext";
            this.btnOrbitNext.Size = new System.Drawing.Size(70, 23);
            this.btnOrbitNext.TabIndex = 3;
            this.btnOrbitNext.Text = "Next Orbit";
            this.btnOrbitNext.UseVisualStyleBackColor = true;
            this.btnOrbitNext.Click += new System.EventHandler(this.btnOrbitNext_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(614, 464);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(153, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(614, 98);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(148, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Start AI";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // PieceTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 499);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnOrbitNext);
            this.Controls.Add(this.btnOrbitPrev);
            this.Controls.Add(this.txtPieceName);
            this.Controls.Add(this.btnPiece);
            this.Name = "PieceTester";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PieceTester_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPiece;
        private System.Windows.Forms.TextBox txtPieceName;
        private System.Windows.Forms.Button btnOrbitPrev;
        private System.Windows.Forms.Button btnOrbitNext;
        private System.Windows.Forms.Button button1;
        private System.ComponentModel.BackgroundWorker bgwAiWorker;
        private System.Windows.Forms.Button button2;
    }
}

