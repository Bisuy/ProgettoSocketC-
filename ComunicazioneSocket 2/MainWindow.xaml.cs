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
//aggiunta
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
/*
 * FILIPPO BISULLI CL 4'L DATA 17/05/2021
 * ESERCIZIO SOCKET UDP SASSO-CARTA-FORBICE (B)
 */
namespace ComunicazioneSocket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool Ricevuto = false;//ricevuto dati
        string scf = ""; //mio messaggio
        string messaggio;//mex avversario
        bool Inviato = false;//inviato
        bool Giocata = false;
        int giocate = 0;//giocate 
        public MainWindow()
        {
            InitializeComponent();
            IPEndPoint localendpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 56001);//setto il mio local endpoint
            Thread t1 = new Thread(new ParameterizedThreadStart(SocketReceive));//faccio partire il t1 sul metodo
            t1.Start(localendpoint);//lo starto
            Thread t2 = new Thread(Controllo);//parte il t2 sul controllo
            t2.Start();//lo starto
        }
        /// <summary>
        /// Ricevo i dati
        /// </summary>
        /// <param name="sourceEndPoint"></param>
        public async void SocketReceive (object sourceEndPoint)
        {

            IPEndPoint sourceEP = (IPEndPoint)sourceEndPoint;// il mio source
            Socket t = new Socket(sourceEP.AddressFamily, SocketType.Dgram, ProtocolType.Udp);//utilizzo il protocollo UDP
            t.Bind(sourceEP);
            Byte[] byteRicevuti = new byte[256];
            string message = "";
            int bytes = 0;
            await Task.Run(() =>//runno la task
            {
                while (true)
                {
                    if(t.Available>0)
                    {
                        message = "";
                        bytes = t.Receive(byteRicevuti, byteRicevuti.Length, 0);//ricevo i dati
                        message = message + Encoding.ASCII.GetString(byteRicevuti, 0, bytes);//decoddo il mex
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            messaggio = message;//lo metto in un cdc
                            Ricevuto = true;//ricevuto = true
                        }));
                    }
                }
            });
        }
        /// <summary>
        /// Controllo
        /// </summary>
        public async void Controllo()
        {
            await Task.Run(() =>//runno la task
            {
                while(true)
                {
                    if (Ricevuto && Inviato)//se è stato ricevuto e inviato
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>//runno la task
                        {
                            if (scf == messaggio)//se è uguale
                            {
                                imgWLD.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(@"..\..\..\..\pareggio.jpg");
                            }
                            //altri controlli
                            else if (scf == "sasso" && messaggio == "carta")
                            {
                                imgWLD.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(@"..\..\..\..\lose.jpg");
                            }
                            else if (scf == "sasso" && messaggio == "forbice")
                            {
                                imgWLD.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(@"..\..\..\..\vittoria.png");
                            }
                            else if (scf == "carta" && messaggio == "forbice")
                            {
                                imgWLD.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(@"..\..\..\..\lose.jpg");
                            }
                            else if (scf == "carta" && messaggio == "sasso")
                            {
                                imgWLD.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(@"..\..\..\..\vittoria.png");
                            }
                            else if (scf == "forbice" && messaggio == "sasso")
                            {
                                imgWLD.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(@"..\..\..\..\lose.jpg");
                            }
                            else if (scf == "forbice" && messaggio == "carta")
                            {
                                imgWLD.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(@"..\..\..\..\vittoria.png");
                            }
                            lblGiocataAvversario.Content = "L'Avversario ha tirato: " + messaggio;//dico cosa ha tirato l'avversario
                            Ricevuto = false;
                            btnRigioca.IsEnabled = true;
                            Inviato = false;
                        }));
                    }
                }
            });
        }
        /// <summary>
        /// bottone gioca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInvia_Click(object sender, RoutedEventArgs e)
        {
            if (lstSCF.SelectedIndex != -1 && txtDestPort.Text != "" && txtIpAdd.Text != "")//vari controlli
            {
                if (lstSCF.SelectedIndex == 0)
                {
                    scf = "sasso";
                    imgSCF.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(@"..\..\..\..\sasso.png");
                }
                else if (lstSCF.SelectedIndex == 1)
                {
                    scf = "carta";
                    imgSCF.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(@"..\..\..\..\carta.jpg");
                }
                else if (lstSCF.SelectedIndex == 2)
                {
                    scf = "forbice";
                    imgSCF.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(@"..\..\..\..\forbici.jpg");
                }
                txtDestPort.IsEnabled = false;//disabilito le text box
                txtIpAdd.IsEnabled = false;
                IPAddress ipDest = IPAddress.Parse(txtIpAdd.Text);//parso il dIP
                int portDest = int.Parse(txtDestPort.Text);
                IPEndPoint remoteEndPoint = new IPEndPoint(ipDest, portDest);//creo un end point
                Socket s = new Socket(ipDest.AddressFamily, SocketType.Dgram, ProtocolType.Udp);//protocollo UDP
                Byte[] byteInviati = Encoding.ASCII.GetBytes(scf);
                s.SendTo(byteInviati, remoteEndPoint);//mando i dati
                btnGioca.IsEnabled = false;
                lblGiocataAvversario.Content = "Aspettando che giochi . . .";//aspetto che giochi
                Inviato = true;
                giocate++;
            }
            else
                MessageBox.Show("Errore, campi non compilati!", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);//errore
        }
        /// <summary>
        /// Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SCF_Loaded(object sender, RoutedEventArgs e)
        {
            StreamReader sr = new StreamReader(@"..\..\..\..\scf.txt");//leggo il file
            while(!sr.EndOfStream)//per ogni elemento
            {
                lstSCF.Items.Add(sr.ReadLine());//lo scrivo nella listbox
            }
            sr.Close();
            btnRigioca.IsEnabled = false;
            lblRound.Content = "Round: " + giocate; //primo round
        }
        /// <summary>
        /// btn Rigioca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRigioca_Click(object sender, RoutedEventArgs e)
        {
            imgWLD.Source = null;
            imgSCF.Source = null;
            btnGioca.IsEnabled = true;
            btnRigioca.IsEnabled = false;
            lblGiocataAvversario.Content = "L'avversario ha giocato: ";//gli dico cosa ha giocato l'avversario
            lblRound.Content = "Round: " + giocate;//n round 
        }
    }
}
