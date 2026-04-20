using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JeuPuissance4
{
    public partial class Form1 : Form
    {
        int joueurActuel = 1; // 1 = Rouge, 2 = Jaune
        int[] colCount = new int[7];
        int moveCounter = 0;
        bool gameEnded = false;
        bool redStarts = false;
        int scoreRed = 0, scoreYellow = 0;

        List<(int col, int lig)> moves = new List<(int col, int lig)>();
        List<(int col, int lig)> redo = new List<(int col, int lig)>();

        int[,] grille = new int[7, 6];
        CaseP4[,] casesVisuelles = new CaseP4[7, 6];

        HowToPlay how;

        private string ToBgr(Color c) => $"{c.B:X2}{c.G:X2}{c.R:X2}";

        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        const int DWWMA_CAPTION_COLOR = 35;
        void CustomWindow(Color captionColor, IntPtr handle)
        {
            try
            {
                int[] caption = new int[] { int.Parse(ToBgr(captionColor), System.Globalization.NumberStyles.HexNumber) };
                DwmSetWindowAttribute(handle, DWWMA_CAPTION_COLOR, caption, 4);
            }
            catch { }
        }


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

            UpdateStatus();
        }


        private void ChangeTurn()
        {
            joueurActuel = (joueurActuel == 1) ? 2 : 1;
            UpdateStatus();
        }




        private void GererClic(object sender, EventArgs e)
        {

            if (gameEnded) return;


            CaseP4 caseCliquee = (CaseP4)sender;
            int col = tableLayoutPanel1.GetColumn(caseCliquee);
            JouerColonne(col);
        }

        private void JouerColonne(int col)
        {
            if (gameEnded) return;
            if (colCount[col] >= 6) return;

            for (int lig = 5; lig >= 0; lig--)
            {
                if (grille[col, lig] == 0)
                {
                    grille[col, lig] = joueurActuel;
                    casesVisuelles[col, lig].Etat = joueurActuel;
                    colCount[col]++;
                    moves.Add((col, lig));
                    redo.Clear();
                    moveCounter++;

                    CheckForWinner(col, lig);
                    ChangeTurn();
                    return;
                }
            }
        }




        private void UpdateStatus()
        {
            // Tour
            if (joueurActuel == 1)
            {
                lbTurn.Text = "Tour : ROUGE";
                lbTurn.ForeColor = Color.Crimson;
            }
            else
            {
                lbTurn.Text = "Tour : JAUNE";
                lbTurn.ForeColor = Color.Goldenrod;
            }

            // Dernier coup
            if (moves.Count > 0)
            {
                var last = moves.Last();
                lblLastMove.Text = $"Dernier coup : C{last.col + 1} L{last.lig + 1}";
            }
            else
            {
                lblLastMove.Text = "Dernier coup : N/A";
            }

            lbTurn.Text = $"Coup : {moveCounter}";
            lblRed.Text = $"RED : {scoreRed}";
            lblYellow.Text = $"YELLOW : {scoreYellow}";
        }





        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnHow_Click(object sender, EventArgs e)
        {
            if (how == null)
            {
               HowToPlay h = new HowToPlay();
                h.Location = this.Location;
                h.Size = this.Size;
                h.Show();
                how = h;
            }
           else { 
              how.Location = this.Location;
                how.Size = this.Size;
                how.Show();
            }
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            for (int col = 0; col < 7; col++)
                for (int lig = 0; lig < 6; lig++)
                {
                    grille[col, lig] = 0;
                    casesVisuelles[col, lig].Etat = 0;
                }

            for (int i = 0; i < 7; i++) colCount[i] = 0;
            moveCounter = 0;
            gameEnded = false;
            moves.Clear();
            redo.Clear();

            if (!redStarts)
            {
                joueurActuel = 2;
                redStarts = true;
            }
            else
            {
                joueurActuel = 1;
                redStarts = false;
            }

            UpdateStatus();
        }


        private void CheckForWinner(int lastCol, int lastLig)
        {
            int j = joueurActuel;

            bool victoire =
                CheckDir(j, lastCol, lastLig, 1, 0) ||  // horizontal
                CheckDir(j, lastCol, lastLig, 0, 1) ||  // vertical
                CheckDir(j, lastCol, lastLig, 1, 1) ||  // diag droite
                CheckDir(j, lastCol, lastLig, 1, -1);   // diag gauche

            if (victoire)
            {
                gameEnded = true;
                string nom = (j == 1) ? "Rouge" : "Jaune";
                MessageBoxIcon icon = (j == 1) ? MessageBoxIcon.Error : MessageBoxIcon.Warning;

                if (j == 1) scoreRed++;
                else scoreYellow++;

                UpdateStatus();

                DialogResult result = MessageBox.Show(
                    $"{nom} a gagné la partie !!!\nCommencer une nouvelle partie ?",
                    $"{nom} gagne",
                    MessageBoxButtons.YesNo,
                    icon);

                if (result == DialogResult.Yes)
                    btnRestart_Click(null, null);

                return;
            }

            if (moveCounter == 42)
            {
                gameEnded = true;
                DialogResult result = MessageBox.Show(
                    "Match nul !!!\nCommencer une nouvelle partie ?",
                    "Nul",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                    btnRestart_Click(null, null);
            }
        }


        private bool CheckDir(int joueur, int col, int lig, int dc, int dl)
        {
            int count = 1;
            count += CountInDirection(joueur, col, lig, dc, dl);
            count += CountInDirection(joueur, col, lig, -dc, -dl);
            return count >= 4;
        }


        private int CountInDirection(int joueur, int col, int lig, int dc, int dl)
        {
            int count = 0;
            int c = col + dc, l = lig + dl;
            while (c >= 0 && c < 7 && l >= 0 && l < 6 && grille[c, l] == joueur)
            {
                count++;
                c += dc;
                l += dl;
            }
            return count;
        }




}
}
