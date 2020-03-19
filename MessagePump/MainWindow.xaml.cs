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
using System.Threading;
using System.Threading.Tasks;



namespace WpfAwait
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource cancellationTokenSource;
        CancellationToken cancellationToken;
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void btnProcess_Click(object sender,RoutedEventArgs e)
        {
            btnProcess.IsEnabled = false;
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            int completedPercent = 0;
            for(int i = 0; i<10; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                try
                {
                    await Task.Delay(500, cancellationToken);
                    completedPercent = (i + 1) * 10;
                }
                catch(TaskCanceledException ex)
                {
                    completedPercent = i * 10;

                }
                progressBar.Value = completedPercent;
            }
            string message = cancellationToken.IsCancellationRequested
                    ? string.Format($"Process was cancelled at {completedPercent}%.")
                    : "Process completed normally";
            MessageBox.Show(message, "Completion Status");
            progressBar.Value = 0;
            btnProcess.IsEnabled = true;
            btnCancel.IsEnabled = true;

        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (!btnProcess.IsEnabled)
            {
                btnCancel.IsEnabled = false;
                cancellationTokenSource.Cancel();
            }
        }
    }
}
