﻿using System;
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

namespace GenerateurMusique
{
    public partial class MainWindow : Window
    {

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

            displayPopulation();
            // On s'abonne à la fermeture du programme pour pouvoir nettoyer le répertoire et les fichiers midi
            this.Closed += MainWindow_Closed;
        }

        public void displayPopulation()
        {
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
            {
                File.Delete(file);
            }

        }

        // Lancé lorsque le fichier a fini sa lecture, pour le fermer proprement
        void mplayer_MediaEnded(object sender, EventArgs e)
        {
            mplayer.Stop();
            mplayer.Close();
            isPlaying = false;
        }

        // Clic sur le bouton : on lance la création d'un fichier et on le joue
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            mplayer.Stop();
            mplayer.Close();
            isPlaying = false;

            population = new Population();



        }

        // Méthode principale
        void CreateAndPlayMusic(int songNumber) 
        {


            // s'il y a un fichier en cours de lecture on l'arrête 
            if (isPlaying)
            {
                mplayer.Stop();
                mplayer.Close();
                isPlaying = false;
            }
            // Générateur aléatoire
            Random rand = new Random();
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
            // Chaque note est comprise entre 0 et 127 (12 correspond au type de note, fixe ici à des 1/4)
            // L'équivalence avec les notes / octaves est disponible ici : https://andymurkin.files.wordpress.com/2012/01/midi-int-midi-note-no-chart.jpg
            // Ici 16 notes aléatoire entre 16 et 96 (pour éviter certaines notes trop aigues ou trop graves)
            //for (int i = 0; i < 16; i++)
            //{
            //    int note = rand.Next(24, 96);
            //    song.AddNote(0, 0, note, 24);
            //}

            foreach(Note note in individu.Notes)
            {
                song.AddNote(0, 0, note.Id, note.Duration);
            }

            // d. Enregistrer le fichier .mid (lisible dans un lecteur externe par exemple)
            // on prépare le flux de sortie
            MemoryStream ms = new MemoryStream();
            song.Save(ms);
            ms.Seek(0, SeekOrigin.Begin);
            byte[] src = ms.GetBuffer();
            byte[] dst = new byte[src.Length];
            for (int i = 0; i < src.Length; i++)
            {
                dst[i] = src[i];
            }
            ms.Close();
            // et on écrit le fichier
            strFileName = "Fichier" + nbFile + ".mid";
            FileStream objWriter = File.Create(strFileName);
            objWriter.Write(dst, 0, dst.Length);
            objWriter.Close();
            objWriter.Dispose();
            objWriter = null;

            // 2) Jouer un fichier MIDI
            mplayer.Open(new Uri(strFileName, UriKind.Relative));
            nbFile++;
            isPlaying = true;
            mplayer.Play();
        }

        private void BtnSong1_Click(object sender, RoutedEventArgs e)
        {
            CreateAndPlayMusic(0);
        }

        private void BtnSong2_Click(object sender, RoutedEventArgs e)
        {
            CreateAndPlayMusic(1);
        }

        private void BtnSong3_Click(object sender, RoutedEventArgs e)
        {
            CreateAndPlayMusic(2);
        }

        private void BtnSong4_Click(object sender, RoutedEventArgs e)
        {
            CreateAndPlayMusic(3);
        }

        private void BtnSong5_Click(object sender, RoutedEventArgs e)
        {
            CreateAndPlayMusic(4);
        }

        private void BtnSong6_Click(object sender, RoutedEventArgs e)
        {
            CreateAndPlayMusic(5);
        }

        private void BtnSong7_Click(object sender, RoutedEventArgs e)
        {
            CreateAndPlayMusic(6);
        }

        private void BtnSong8_Click(object sender, RoutedEventArgs e)
        {
            CreateAndPlayMusic(7);
        }

        private void BtnSong9_Click(object sender, RoutedEventArgs e)
        {
            CreateAndPlayMusic(8);
        }

        private void BtnSong10_Click(object sender, RoutedEventArgs e)
        {
            CreateAndPlayMusic(9);
        }

        private void BtnSong11_Click(object sender, RoutedEventArgs e)
        {
            CreateAndPlayMusic(10);
        }

        private void BtnSong12_Click(object sender, RoutedEventArgs e)
        {
            CreateAndPlayMusic(11);
        }

        private void BtnSong13_Click(object sender, RoutedEventArgs e)
        {
            CreateAndPlayMusic(12);
        }
    }
}