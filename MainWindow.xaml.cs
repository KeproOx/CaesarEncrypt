using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//Needed for using an OpenFileDialog and a streamReader

using System.IO;
using Microsoft.Win32;



namespace caesarEncrypt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        char[] cTextAfter;

        //Variables
        int iKey = 0; // value of the key
        string text; // contains the text given by the user
        char[] cText; // contains the text in ASCII code
        int idx = 0; //loop index
        int iValue; // value in int of the ASCII caracter

        int iNewLetter; //The new ASCII caracter
        bool bCorrect = false; //test if operation done correctly

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Button encrypting the given text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            //Algorythm

            txtResult.Clear();
            text = txtText.Text;
            cText = new char[text.Length];
            cTextAfter = new char[text.Length];

            text = text.ToLower();
            try
            {
                iKey = int.Parse(txtKey.Text);
            }
            catch (Exception)
            {

                txtKey.Text = "Put a correct key";

            }
            //Test on the key

            if (iKey >= 1 && iKey <= 26)
            {
                //all the process
                bCorrect = true;
                ControlLetter();  
            }
            else
            {
                txtKey.Text = "Put a correct Key";
            }

            //display mothafucka
            //if the boolean is true, write the result (counter some bug)
            if (bCorrect)
            {
                ShowText();
            }
            
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string ligne;                       // pour la lecture de la ligne en cours du texte source
            int iLigne;                         // Index de boucle
            int nbreLignes;

            OpenFileDialog ofd; //déclaration du choix de sélection
            ofd = new OpenFileDialog(); //instanciation de celui ci                          
            ofd.InitialDirectory = Directory.GetCurrentDirectory(); //On indique dans quel répertoire il doit démarrer                       
            ofd.ShowDialog(); //On l'affiche ensuite à l'écran

            if (ofd.FileName != "")
            {
                FileInfo fFichierSrce = new FileInfo(ofd.FileName); //On déclare un fichier FileInfo qui stockera les informations du répertoire    
                StreamWriter strmr = new StreamWriter(ofd.FileName, false, Encoding.Default); //On ouvre un flux de lecture dans le répertoire de la boite de sélection

                //On lis les lignes et on les stockes dans les variables

                nbreLignes = txtResult.LineCount;
                for (iLigne = 0; iLigne < nbreLignes; iLigne++)
                {
                    ligne = txtResult.GetLineText(iLigne);
                    strmr.Write(ligne);
                }

                //fermeture du flux
                strmr.Close();

                //On lance notepad++
                try
                {

                    System.Diagnostics.Process.Start("NotePad++.exe", ofd.FileName);

                }
                catch (Exception)
                {
                    lblAlert.Content = "Impossible d'ouvrir NotePad++";
                }
            }
            else
            {
                lblAlert.Content = "Veuillez choisir un fichier";
            }
        }

        private void btnShowWin_Click(object sender, RoutedEventArgs e)
        {
            WinDecrypt win = new WinDecrypt();
            win.Show();
        }

        public void ShowText()
        {
            int iKeyCrypted = 96 + iKey;
            char cKeyCrypted = (char)iKeyCrypted;
            txtResult.Text += " " +cKeyCrypted;

        }
     
        public void ControlLetter()
        {
            foreach (char c in text)
            {

                cText[idx] = c;
                iValue = cText[idx];

                //if value is an ' word caracter
                if (iValue == 8217)
                {
                    iValue = 39;
                }

                iNewLetter = iValue + iKey;

                //if the new letter is superior to z and different thant an space and 
                if (iNewLetter > 122 && iNewLetter != 32 && iValue <= 122)
                {
                    int iReste = iNewLetter - 122;
                    int iNouvValeur = 96 + iReste;
                    cTextAfter[idx] = (char)iNouvValeur;
                }
                if (iNewLetter <= 122 && iNewLetter >= 96)
                {
                    cTextAfter[idx] = (char)iNewLetter;
                }
                else
                {
                    if (iValue >= 0 && iValue <= 64 || iValue > 200)
                    {
                        cTextAfter[idx] = (char)iValue;
                    }
                    else
                    {
                        if (iValue == 233 || iValue == 232 || iValue == 234)
                        {
                            int ValeurCorrecte = 101 + iKey;
                            cTextAfter[idx] = (char)ValeurCorrecte;
                        }
                        if (iValue == 224)
                        {
                            int ValeurCorrecte = 97 + iKey;
                            cTextAfter[idx] = (char)ValeurCorrecte;
                        }
                    }
                    if (iValue == 123 || iValue == 125 || iValue == 91 || iValue == 93)
                    {
                        cTextAfter[idx] = (char)iValue;
                    }
                }
                txtResult.Text += cTextAfter[idx].ToString();
                idx++;
            }
        }

        private void btnEncryptVig_Click(object sender, RoutedEventArgs e)
        {
            //Variables
            int idxList = 0;
            char c;
            int LetterA = 96;
            int iEcart = 0;
            int iEcartTotal = 0;
            List<char> charList;
            charList = new List<char>();
            List<bool> bPositionLetter = new List<bool>(); //Permettra de savoir l'emplacement du mot dans le tableau

            while (idxList < 26*26)
            {
                LetterA++;
                iEcartTotal = LetterA + iEcart;
                
                c = (char)iEcartTotal;

                if (c <= 122 && c > 96)
                {
                    charList.Add(c);
                }
                else
                {
                    iEcart++;
                    LetterA = 96;
                    iEcartTotal = LetterA + iEcart;
                    charList.Add(c);
                }
                idxList++;
            }
            txtResult.Text = charList[idxList].ToString();

            
        }
    }
}
