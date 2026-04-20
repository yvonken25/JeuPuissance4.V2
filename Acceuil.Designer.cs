namespace JeuPuissance4
{
    partial class Acceuil
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
            bJouer = new Button();
            SuspendLayout();
            // 
            // bJouer
            // 
            bJouer.BackColor = SystemColors.ButtonShadow;
            bJouer.Cursor = Cursors.Hand;
            bJouer.Location = new Point(770, 315);
            bJouer.Name = "bJouer";
            bJouer.Size = new Size(199, 68);
            bJouer.TabIndex = 0;
            bJouer.Text = "Nouvelle Partie";
            bJouer.UseVisualStyleBackColor = false;
            bJouer.Click += bJouer_Click;
            // 
            // Acceuil
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.p4;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1012, 580);
            Controls.Add(bJouer);
            Name = "Acceuil";
            Text = "Acceuil";
            ResumeLayout(false);
        }

        #endregion

        private Button bJouer;
    }
}