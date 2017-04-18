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
using GalaSoft.MvvmLight.Messaging;

namespace tfsshelvesetexplorer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly IMessenger _messenger;

		public MainWindow()
		{
			InitializeComponent();
			_messenger = Messenger.Default;
		}

		private void DownloadButtonClick(object sender, RoutedEventArgs e)
		{
			var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
			if (dialog.ShowDialog() == true)
			{
				_messenger.Send(new MainWindowDownloadMessage { DestinationFolder = dialog.SelectedPath });
			}
		}
	}

	public class MainWindowDownloadMessage
	{
		public string DestinationFolder;
	}
}
