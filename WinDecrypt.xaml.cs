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
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace caesarEncrypt
{
    /// <summary>
    /// Logique d'interaction pour WinDecrypt.xaml
    /// </summary>
    public partial class WinDecrypt : Window
    {


        //Variables
        int iKey = 0;
        string text;
        char[] cText;
        int idx = 0;
        int iValue;
        int iGap;
        int iNewLetter;
        bool bCorrect = false;


        string sLigne;                      // variable utilisée pour stocker les lignes du fichier texte
        string sLignes;                     // variable stockant le total du fichier texe   
        int iNbrLignes = 0;                 // variable stockant le nombres de lignes du fichier




        public WinDecrypt()
        {
            InitializeComponent();
        }

        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {

            
            

            //Algorythm
            char[] cTextAfter;

            txtDecrypted.Clear();

            text = txtDecrypt.Text;
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

            iGap = iKey;

            //Test on the key

            if (iKey >= 1 && iKey <= 26)
            {
                //all the process
                bCorrect = true;

                foreach (char c in text)
                {
                    cText[idx] = c;
                    iValue = cText[idx];
                    iNewLetter = iValue - iGap;

                    cTextAfter[idx] = (char)iNewLetter;

                    if (iNewLetter <96 && iValue != 32 && iValue <= 122 && iValue >64)
                    {
                        int iReste = 96 - iNewLetter;
                        int iNouvValeur = 122 - iReste;
                        cTextAfter[idx] = (char)iNouvValeur;
                    }
                    else
                    {
                        if (iValue >= 0 && iNewLetter <= 64 + iGap || iValue > 200 || iValue == 32)
                        {
                            cTextAfter[idx] = (char)iValue;
                        }
                        if (iValue - iGap == 39)
                        {
                            cTextAfter[idx] = (char)iValue;
                        }
                        if (iValue == 123 || iValue == 125 || iValue == 91 || iValue == 93)
                        {
                            cTextAfter[idx] = (char)iValue;
                        } 
                    }
                    txtDecrypted.Text += cTextAfter[idx].ToString();
                    idx++;
                    
                }
            }
            else
            {
                txtKey.Text = "Put a correct Key";
            }
            //affichage

            
            
                
                    
                
                              
        }

        private void btnOpF_Click(object sender, RoutedEventArgs e)
        {
            
            

            OpenFileDialog ofd; //déclaration du choix de sélection
            ofd = new OpenFileDialog(); //instanciation de celui ci                          
            ofd.InitialDirectory = Directory.GetCurrentDirectory(); //On indique dans quel répertoire il doit démarrer
            ofd.DefaultExt = "txt";
            // Filtre sur les fichiers
            ofd.Filter = "fichiers textes (*.txt)|*.txt|Tous les fichiers (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.ShowDialog();//On l'affiche ensuite à l'écran
            


            if (ofd.FileName != "")
            {
                FileInfo fFichierSrce = new FileInfo(ofd.FileName); //On déclare un fichier FileInfo qui stockera les informations du répertoire    
                StreamReader strmr = new StreamReader(ofd.FileName, Encoding.Default); //On ouvre un flux de lecture dans le répertoire de la boite de sélection

                //On lis les lignes et on les stockes dans les variables

                sLigne = "";
                sLignes = "";
                do
                {
                    sLigne = strmr.ReadLine();
                    if (sLigne != null)
                    {
                        sLignes += sLigne + "\n";
                        iNbrLignes += 1;
                    }
                } while (sLigne != null);

                //fermeture du flux
                strmr.Close();

                //On affiche les résultats
                lblFileName.Content += ofd.FileName.ToString();
                txtDecrypt.Text = sLignes;
            }
            VerifyKey();
        }




        private void VerifyKey()
        {
          char cCaracKey = char.Parse(sLignes.Substring(sLignes.Length - 2, 1));
          iKey = cCaracKey - 96;
          txtKey.Text = iKey.ToString();
        }



    }
}
