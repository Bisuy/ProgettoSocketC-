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
 * ESERCIZIO SOCKET UDP SASSO-CARTA-FORBICE (A)
 */
namespace ComunicazioneSocket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool Ricevuto = false;//dati ricevuti
        string scf = "";//MIO MESSAGGIO
        string messaggio;//messaggio nemico
        bool Inviato = false;//inviato dati
        bool Giocata = false;//giocata
        int giocate = 0;
        
        public MainWindow()
        {
            InitializeComponent();
            IPEndPoint localendpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 56000); //Local EndPoint
            Thread t1 = new Thread(new ParameterizedThreadStart(SocketReceive)); //thread x il ricevimento dei dati
            t1.Start(localendpoint);//lo starto
            Thread t2 = new Thread(Controllo);//thread 2 x i controlli
            t2.Start();//lo starto
        }
        /// <summary>
        /// Ricevo il socket
        /// </summary>
        /// <param name="sourceEndPoint">Source EndPoint</param>
        public async void SocketReceive (object sourceEndPoint)
        {
                IPEndPoint sourceEP = (IPEndPoint)sourceEndPoint; //aggiungo il source end point
                Socket t = new Socket(sourceEP.AddressFamily, SocketType.Dgram, ProtocolType.Udp);//uso il protocollo UDP
                t.Bind(sourceEP);
                Byte[] byteRicevuti = new byte[256];
                string message = "";
                int bytes = 0;
                await Task.Run(() =>//runno la task
                {
                    while (true)
                    {
                        if (t.Available > 0)//se è avaible
                        {
                            message = "";
                            bytes = t.Receive(byteRicevuti, byteRicevuti.Length, 0);
                            message = message + Encoding.ASCII.GetString(byteRicevuti, 0, bytes); //decoddo il messaggio

                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                messaggio = message;//lo assegno ad un cdc
                                Ricevuto = true;//ricevuto il mex
                            }));
                        }
                    }
                });
        }
        /// <summary>
        /// Controlli
        /// </summary>
        public async void Controllo()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (Ricevuto && Inviato)
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if(scf == messaggio)//se è uguale
                            {
                                imgWLD.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(@"..\..\..\..\pareggio.jpg");//mostro l'immagine
                            }
                            //altri controlli
                            else if(scf == "sasso" && messaggio == "carta")
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
                            lblGiocataAvversario.Content = "L'Avversario ha tirato: " + messaggio; //mostro cosa ha tirato:
                            Ricevuto = false;
                            btnRigioca.IsEnabled = true;
                            Inviato = false;
                        }));
                    }
                }
                
            });

        }
        /// <summary>
        /// btn Gioca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInvia_Click(object sender, RoutedEventArgs e)
        {
            //se le textbox sono vuote o non ho selezionato cosa voglio tirare
            if (lstSCF.SelectedIndex != -1 && txtDestPort.Text != "" && txtIpAdd.Text != "")
            {
                if (lstSCF.SelectedIndex == 0) //controlli
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
                txtDestPort.IsEnabled = false; //disabilito le textbox
                txtIpAdd.IsEnabled = false;
                IPAddress ipDest = IPAddress.Parse(txtIpAdd.Text);//parso l'IP
                int portDest = int.Parse(txtDestPort.Text);
                IPEndPoint remoteEndPoint = new IPEndPoint(ipDest, portDest);
                Socket s = new Socket(ipDest.AddressFamily, SocketType.Dgram, ProtocolType.Udp);//utilizzo il protocollo UDP
                Byte[] byteInviati = Encoding.ASCII.GetBytes(scf); //invio i dati
                s.SendTo(byteInviati, remoteEndPoint);
                btnGioca.IsEnabled = false;
                lblGiocataAvversario.Content = "Aspettando che giochi . . ."; //aspetto che giochi
                Inviato = true;
                giocate++; //round ++
            }
            else
                MessageBox.Show("Errore, campi non compilati!","ERRORE",MessageBoxButton.OK, MessageBoxImage.Error);//errore
        }
        /// <summary>
        /// Load del programma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SCF_Loaded(object sender, RoutedEventArgs e)
        {
            StreamReader sr = new StreamReader(@"..\..\..\..\scf.txt");//ciclo su tutto il file
            while(!sr.EndOfStream)//per ogni elemento
            {
                lstSCF.Items.Add(sr.ReadLine());//lo ciclo nella listbox
            }
            sr.Close();
            btnRigioca.IsEnabled = false;
            lblRound.Content = "Round: " + giocate;//giocata visibile
        }
        /// <summary>
        /// Per rigiocare
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRigioca_Click(object sender, RoutedEventArgs e)
        {
            imgWLD.Source = null;
            imgSCF.Source = null;
            btnGioca.IsEnabled = true;
            btnRigioca.IsEnabled = false;
            lblGiocataAvversario.Content = "L'avversario ha giocato: ";
            lblRound.Content = "Round: " + giocate;
        }
    }
}
