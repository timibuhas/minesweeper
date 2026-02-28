using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace Minesweeper_WPF
{
    
    class Element:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string afisare;
        private bool nespart;
        private int valoare;
        private Brush culoare_background;
        private Brush culoare_text;
        private int x;
        private int y;

        public Element(bool nespart, Brush culoare_background, Brush culoare_text, int x, int y, string afisare, int valoare)
        {
            this.Nespart = nespart;
            this.Culoare_background = culoare_background;
            this.Culoare_text = culoare_text;
            this.X = x;
            this.Y = y;
            this.Afisare = afisare;
            this.Valoare = valoare;
        }

        public bool Nespart { get => nespart; set { nespart = value; this.NotifyPropertyChanged("Nespart"); } }
        public Brush Culoare_background { get => culoare_background; set { culoare_background = value; this.NotifyPropertyChanged("Culoare_background"); } }
        public Brush Culoare_text { get => culoare_text; set { culoare_text = value; this.NotifyPropertyChanged("Culoare_text"); } }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public string Afisare { get => afisare; set { afisare = value; this.NotifyPropertyChanged("Afisare"); } }
        public int Valoare { get => valoare; set { valoare = value; this.NotifyPropertyChanged("Valoare"); } }

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }

        }
    }
}
