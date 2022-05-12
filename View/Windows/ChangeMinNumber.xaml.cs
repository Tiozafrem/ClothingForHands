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

namespace ClothingForHands.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChangeMinNumber.xaml
    /// </summary>
    public partial class ChangeMinNumber : Window
    {
        private Helper.MsgBoxHelper msgBox = new Helper.MsgBoxHelper();
        public int MaxNumber { get; set; }
        public ChangeMinNumber(int maxNumber)
        {
            this.MaxNumber = maxNumber;
            DataContext = this;
            InitializeComponent();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(TxbNumber.Text, out int buff))
            {
                this.DialogResult = true;
            }
            else
            {
                msgBox.Error("Введите корректное число.");
            }

        }
    }
}
