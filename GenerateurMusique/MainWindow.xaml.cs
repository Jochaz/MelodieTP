using System;
using System.Collections.Generic;
using System.IO;
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
using MasaSam.Controls;
using Microsoft.VisualBasic;
//using System.Windows.Forms;


namespace GenerateurMusique
{
    public partial class MainWindow : Window
    {
        static Random rand = new Random();
        MediaPlayer mplayer;
        Boolean isPlaying;
        string strFileName;
        int nbFile = 0;
        Population population;

        public MainWindow()
        {
            InitializeComponent();

            // Initialisation du lecteur
            mplayer = new MediaPlayer();
            mplayer.MediaEnded += mplayer_MediaEnded;
            isPlaying = false;

            population = new Population();
            LblNumGeneration.Content = population.NbGeneration;
            // On s'abonne à la fermeture du programme pour pouvoir nettoyer le répertoire et les fichiers midi
            this.Closed += MainWindow_Closed;

            for (int i = 1; i <= 12; i++)
            {
                CboRecord.Items.Add(i.ToString());
            }
            CboRecord.SelectedIndex = 0;
        }
        // On efface les fichiers .mid que l'on avait créé à la fin du programme
        void MainWindow_Closed(object sender, EventArgs e)
        {
            // s'il y a un fichier en cours de lecture on l'arrête 
            if (isPlaying)
            {
                mplayer.Stop();
                mplayer.Close();
                isPlaying = false;
            }
            var files = Directory.EnumerateFiles("./", "Fichier*.mid");
            foreach (string file in files)
                File.Delete(file);

        }

        // Lancé lorsque le fichier a fini sa lecture, pour le fermer proprement
        void mplayer_MediaEnded(object sender, EventArgs e)
        {
            mplayer.Stop();
            mplayer.Close();
            isPlaying = false;
        }

        // Clic sur le bouton : on reset la population
        private void BtnRestart(object sender, System.Windows.RoutedEventArgs e)
        {

            LstAverage.Items.Clear();
            resetRating();
            mplayer.Stop();
            mplayer.Close();
            isPlaying = false;

            population = new Population();
            LblNumGeneration.Content = population.NbGeneration;
        }

        // Méthode principale
        void CreateAndPlayMusic(int songNumber, bool onlyRecord) 
        {
            // s'il y a un fichier en cours de lecture on l'arrête 
            if (isPlaying)
            {
                mplayer.Stop();
                mplayer.Close();
                isPlaying = false;
            }
            // Générateur aléatoire

            Individu individu = population.Individus[songNumber];
            // 1) Créer le fichier MIDI
            // a. Créer un fichier et une piste audio ainsi que les informations de tempo
            MIDISong song = new MIDISong();
            song.AddTrack("Piste1");
            song.SetTimeSignature(0, 4, 4);
            song.SetTempo(0, individu.Tempo);

            // b. Choisir un instrument entre 1 et 128 
            // Liste complète ici : http://fr.wikipedia.org/wiki/General_MIDI

            song.SetChannelInstrument(0, 0, individu.Instrument);

            // c. Ajouter des notes
            foreach(Note note in individu.Genes)
                song.AddNote(0, 0, note.Valeur, note.Duration);

            // d. Enregistrer le fichier .mid (lisible dans un lecteur externe par exemple)
            // on prépare le flux de sortie
            MemoryStream ms = new MemoryStream();
            song.Save(ms);
            ms.Seek(0, SeekOrigin.Begin);
            byte[] src = ms.GetBuffer();
            byte[] dst = new byte[src.Length];
            for (int i = 0; i < src.Length; i++)
                dst[i] = src[i];

            ms.Close();
            // et on écrit le fichier
            try
            {
                strFileName = "Fichier" + nbFile + ".mid";
                FileStream objWriter = File.Create(strFileName);
                objWriter.Write(dst, 0, dst.Length);
                objWriter.Close();
                objWriter.Dispose();
                objWriter = null;
            }
            catch (Exception E)
            {
                MessageBox.Show("Veuillez fermer la mélodie qui est en cours de lecture", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            if (!onlyRecord) { 
                // 2) Jouer un fichier MIDI
                mplayer.Open(new Uri(strFileName, UriKind.Relative));
                nbFile++;
                isPlaying = true;
                mplayer.Play();
            }
        }

        private void OnClickButton(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var songNumber = int.Parse(button.Tag.ToString());
            CreateAndPlayMusic(songNumber, false);
        }

        private void BtnNextGen(object sender, RoutedEventArgs e)
        {
            mplayer.Stop();
            mplayer.Close();
            isPlaying = false;
            if (allIsRated())
            {
                //log();
                valideStar();
                population.saveGenAverage();
                LstAverage.Items.Add((population.NbGeneration) + "                 " + Math.Round(population.Averages[population.Averages.Count - 1],2));

                population.NextGeneration();

                LblNumGeneration.Content = population.NbGeneration;
                resetRating();
            }
            else
                MessageBox.Show("Vous n'avez pas attribué de note à toutes les mélodies, impossible de passer à la génération suivante !");

        }

        public bool allIsRated()
        {
            Rating rate;

            var stackPanels = canvas.Children.OfType<StackPanel>().ToList();
            foreach (var stackPanel in stackPanels)
            {
                rate = (Rating)stackPanel.Children[0];
                if (rate.Value == 0)
                    return false;
            }

            return true;
        }
        public void valideStar()
        {
            Rating rate;
            int index;
            var stackPanels = canvas.Children.OfType<StackPanel>().ToList();
            foreach (var stackPanel in stackPanels)
            {
                rate = (Rating)stackPanel.Children[0];
                index = int.Parse(rate.Tag.ToString()) - 1;
                population.Individus[index].Note = rate.Value;
            }
        }

        public void log()
        {

            Individu individu;
            Console.WriteLine("------ Génération du log ------");

            for (int i = 0; i <= population.Individus.Count - 1; i++ )
            {
                individu = population.Individus[i];
                Console.WriteLine("----------- Individu " + i + " : " + individu.SongNumber + " ------------");
                Console.WriteLine("Instrument :" + individu.Instrument);
                Console.WriteLine("Gènes : ");
                foreach(Note note in individu.Genes)
                    Console.Write(note.Valeur + " ");

                Console.WriteLine(" ");
            }

        }

        public void eraseRating()
        {
            //Efface les rates dans les stackpanels
            var stackPanels = canvas.Children.OfType<StackPanel>().ToList();
            foreach (var stackPanel in stackPanels)
                stackPanel.Children.Clear();
        }


        public void resetRating()
        {
            eraseRating();
            int indexCurrentRating = 1;
            //Reinitialise les notes
            var stackPanels = canvas.Children.OfType<StackPanel>().ToList();
            foreach (var stackPanel in stackPanels)
            {
                Rating currentRate = new Rating
                {
                    Maximum = 5,
                    Minimum = 0,
                    Tag = indexCurrentRating,
                    Name = "rtFive" + indexCurrentRating.ToString()
                };
                stackPanel.Children.Add(currentRate);
                indexCurrentRating++;
            }
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            CreateAndPlayMusic(int.Parse(CboRecord.SelectedValue.ToString()), true);
        }

    }
}
