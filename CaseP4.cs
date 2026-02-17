using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeuPuissance4
{
    public partial class CaseP4 : UserControl
    {

        private int _etat = 0;

        public int Etat
        {
            get => _etat;
            set
            {
                _etat = value;
                this.Invalidate(); // Force le cercle à se redessiner quand l'état change
            }
        }
        public CaseP4()
        {
            InitializeComponent();
        }

        private void CaseP4_Load(object sender, EventArgs e)
        {

        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // On crée un chemin en forme d'ellipse qui fait la taille du contrôle
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddEllipse(0, 0, this.Width, this.Height);
                // On applique ce chemin comme "forme" officielle du contrôle
                this.Region = new Region(path);
            }
        }

        private void CaseP4_Paint(object sender, PaintEventArgs e)
        {
            // On active l'anti-aliasing pour un cercle bien lisse
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // On choisit la couleur du pinceau selon l'état
            Brush pinceau;
            switch (Etat)
            {
                case 1: pinceau = Brushes.Red; break;
                case 2: pinceau = Brushes.Yellow; break;
                default: pinceau = Brushes.LightGray; break; // Case vide
            }

            // On dessine le cercle (on laisse une marge de 4 pixels pour que ce soit joli)
            int marge = 4;
            e.Graphics.FillEllipse(pinceau, marge, marge, Width - (marge * 2), Height - (marge * 2));

            // Optionnel : on dessine un contour noir fin pour mieux voir le jeton
            e.Graphics.DrawEllipse(Pens.Black, marge, marge, Width - (marge * 2), Height - (marge * 2));
        }
    }
}
