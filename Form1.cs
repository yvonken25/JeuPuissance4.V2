namespace JeuPuissance4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            for (int col = 0; col < 7; col++)
            {
                for (int lig = 0; lig < 6; lig++)
                {
                    // On récupère le contrôle situé à cette position dans le TableLayoutPanel
                    CaseP4 c = (CaseP4)tableLayoutPanel1.GetControlFromPosition(col, lig);
                    casesVisuelles[col, lig] = c;

                    // On abonne toutes les cases au même événement
                    c.Click += GererClic;
                }
            }
        }




        int[,] grille = new int[7, 6];
        int joueurActuel = 1; // 1 = Rouge, 2 = Jaune

        // Un tableau pour accéder facilement à tes contrôles visuels
        CaseP4[,] casesVisuelles = new CaseP4[7, 6];


        private void GererClic(object sender, EventArgs e)
        {
            CaseP4 caseCliquee = (CaseP4)sender;
            int col = tableLayoutPanel1.GetColumn(caseCliquee);

            // On cherche la ligne vide (0) en partant du bas (ligne 5)
            for (int lig = 5; lig >= 0; lig--)
            {
                if (grille[col, lig] == 0)
                {
                    // 1. Mise à jour de la donnée
                    grille[col, lig] = joueurActuel;

                    // 2. Mise à jour du visuel
                    casesVisuelles[col, lig].Etat = joueurActuel;

                    // 3. Vérifier la victoire (on verra l'algo après)
                    // VerifierVictoire(col, lig);

                    // 4. Changer de joueur
                    joueurActuel = (joueurActuel == 1) ? 2 : 1;

                    return; // On a posé le jeton, on arrête la boucle
                }
            }

            // Si on arrive ici, c'est que la colonne est pleine
            MessageBox.Show("Colonne pleine !");
        }


     /*   private void panel_Paint(object sender, PaintEventArgs e)
        {
            // On active l'anti-aliasing pour que le rond soit lisse (pas d'escaliers)
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Définir la couleur selon l'état de la case (0, 1 ou 2 dans ton tableau)
            Brush couleurJeton = Brushes.LightGray; // Par défaut vide

            // Exemple de logique :
            // if (valeurGrille == 1) couleurJeton = Brushes.Red;
            // if (valeurGrille == 2) couleurJeton = Brushes.Yellow;

            // On dessine le cercle avec une petite marge (Padding)
            int marge = 5;
            e.Graphics.FillEllipse(couleurJeton, marge, marge,
                                   panel.Width - (marge * 2),
                                   panel.Height - (marge * 2));
        }*/

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
