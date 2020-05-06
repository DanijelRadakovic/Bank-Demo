using System.Windows;
using System.Windows.Controls;

namespace Bank.View.Util
{
    /// <summary>
    /// Interaction logic for MenuSideBar.xaml
    /// </summary>
    public partial class MenuSideBar : UserControl
    {
        public MenuSideBar()
        {
            InitializeComponent();
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddTransaction_Click(object sender, RoutedEventArgs e)
        {
            new AddTransactionDialog
            {
                Owner = Application.Current.MainWindow
            }
            .ShowDialog();
        }

        private void AddLoan_Click(object sender, RoutedEventArgs e)
        {
            new AddLoanDialog
            {
                Owner = Application.Current.MainWindow
            }
            .ShowDialog();
        }
    }
}
