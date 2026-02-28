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
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Minesweeper_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        int randuri, coloane, bombe, stegulete, first_click = 1;
        public event PropertyChangedEventHandler PropertyChanged;
        List<List<Element>> btns = new List<List<Element>>();
        string steag = char.ConvertFromUtf32(0x1F3F4);
        string bomba = char.ConvertFromUtf32(0x1F4A3);
        public int Stegulete { get => stegulete; set { stegulete = value; NotifyPropertyChanged("Stegulete"); } }
        internal List<List<Element>> Btns { get => btns; set { btns = value; NotifyPropertyChanged("Btns"); } }

        Dictionary<int, Brush> culori = new Dictionary<int, Brush>()
        {
            {1,Brushes.Blue },
            {2,Brushes.Green },
            {3,Brushes.Red },
            {4,Brushes.DarkBlue },
            {5,Brushes.DarkRed },
            {6,Brushes.AliceBlue },
            {7,Brushes.Black },
            {8,Brushes.White }
        };
        public MainWindow()
        {         
           InitializeComponent();
        }
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }

        }

        int Verificare_bombe_first_click(Element e,int a,int b)
        {
            
            for (int m = e.X - 1; m <= e.X + 1; m++)
                for (int n = e.Y - 1; n <= e.Y + 1; n++)
                    if (m > -1 && n > -1 && m < randuri && n < coloane)
                        if (a == m && b == n)
                            return 0;
            return 1;
        }

        void Adaugare_Bombe(Element x)
        {
          
            Random rnd = new Random();
            int a, b;
            while (bombe > 0)
            {
                a = rnd.Next(0, randuri);
                b = rnd.Next(0, coloane);
                if (Btns[a][b].Valoare!=-1 && Verificare_bombe_first_click(x,a,b)==1 )
                {
                    Btns[a][b].Valoare = -1;
                    bombe--;
                    for (int m = a - 1; m <= a + 1; m++)
                        for (int n = b - 1; n <= b + 1; n++)
                            if (m > -1 && n > -1 && m < randuri && n < coloane)
                                if (Btns[m][n].Valoare != -1)
                                    Btns[m][n].Valoare++;
                }
                
            }

        }

        void Adaugare_Butoane()
        {
            for (int i = 0; i < randuri; i++)
            {
                Btns.Add(new List<Element>());
                for (int j = 0; j < coloane; j++)
                    Btns[i].Add(new Element(true,Brushes.DimGray,Brushes.Black,i,j,"",0));
            }
        }

        private void Button_Click(object sender,  EventArgs e)
        {
            ic.ItemsSource = null;
            Btns.Clear();
            Adaugare_Butoane();          
            ic.ItemsSource = btns;
            first_click = 1;
            bombe = int.Parse(Bombe.Text);
            Stegulete = bombe;
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Element a = ((Button)sender).Tag as Element;
            if (e.RightButton == MouseButtonState.Pressed && first_click==0)
            {
                if (a.Afisare != steag)
                {
                    a.Afisare = steag;
                    Stegulete--;
                }

                else
                    if (a.Afisare == steag)
                {
                    a.Afisare = "";
                    a.Nespart = true;
                    Stegulete++;
                }

            }
            else 
                if(a.Afisare!=steag)
            {
                if (first_click == 1)
                {
                    Adaugare_Bombe(a);
                    Spargere(a);
                    first_click = 0;

                }
                else
                {
                    switch (Btns[a.X][a.Y].Valoare)
                    {
                        case -1: GameOver(0); break;
                        case 0: Spargere(a); break;
                        default: Afisare_Buton(a); break;
                    }
                    if (Win() == 1 && a.Valoare != -1)
                        GameOver(1);
                }
            }
           
        }

        void Afisare_Buton(Element a)
        {
            a.Nespart = false;
            a.Culoare_background = Brushes.LightGray;       
            if (a.Valoare>0)
            {
                a.Afisare = Btns[a.X][a.Y].Valoare.ToString();
                a.Culoare_text = culori.FirstOrDefault(x => x.Key == Btns[a.X][a.Y].Valoare).Value;
            }
            
        }

        private void Incepe_Jocu(object sender, RoutedEventArgs e)
        {
            int r = int.Parse(Randuri.Text);
            int c = int.Parse(Coloane.Text);
            int b = int.Parse(Bombe.Text);
            if (Bombe.Text!=string.Empty && Coloane.Text!=string.Empty && Randuri.Text != string.Empty && r>=4 && c>=4 && b<(c*r)-10)
            {
                randuri =r;
                coloane = c;
                bombe = b;
                Stegulete = bombe;
                panel.Visibility = Visibility.Hidden;
                Adaugare_Butoane();
                ic.ItemsSource = Btns;
                first_click = 1;              
            }
        }

        private void Txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            var fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            int val;
            e.Handled = !int.TryParse(fullText, out val);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Randuri.Text = string.Empty;
            Coloane.Text = string.Empty;
            Bombe.Text = string.Empty;
            panel.Visibility = Visibility.Visible;
            ic.ItemsSource = null;
            btns.Clear();
        }

        void GameOver(int c)
        {
            
            for (int i=0;i<randuri;i++)
                 foreach(Element a in Btns[i].Where(x=>x.Nespart==true))
                {
                    a.Nespart = false;
                    if (c == 0 && a.Valoare == -1)
                        if (a.Afisare == steag )
                            a.Culoare_text = Brushes.DarkGreen;
                        else
                            a.Afisare = bomba;
                    if (a.Valoare != -1 && a.Afisare == steag)
                        a.Culoare_text = Brushes.DarkRed;

              
                }
            if (c == 1)
                MessageBox.Show("Bravo!");
            else
                MessageBox.Show("Boom!");
        }

        int Win()
        {
            for (int i = 0; i < randuri; i++)
                foreach (Element x in Btns[i])
                    if (x.Valoare>0 && x.Nespart==true)
                        return 0;
            return 1;
        }

        void Spargere(Element a)
        {
            Afisare_Buton(a);          
            for (int m = a.X - 1; m <= a.X + 1; m++)
                for (int n = a.Y - 1; n <= a.Y + 1; n++)
                    if (m > -1 && n > -1 && m < randuri  && n < coloane )
                        if (Btns[m][n].Valoare == 0)
                        {           
                            Afisare_vecini(Btns[m][n]);
                            if (Btns[m][n].Nespart==true)
                                 Spargere(Btns[m][n]);
                        }
        }

        void Afisare_vecini(Element a)
        {

            for (int m = a.X - 1; m <= a.X + 1; m++)
                for (int n = a.Y - 1; n <= a.Y + 1; n++)
                    if (m > -1 && n > -1 && m < randuri  && n < coloane )
                        if (Btns[m][n].Valoare > 0 && Btns[m][n].Nespart==true && Btns[m][n].Afisare!=steag)
                            Afisare_Buton(Btns[m][n]);
        }
    }
    }
