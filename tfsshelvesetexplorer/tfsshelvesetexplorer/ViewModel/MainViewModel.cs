using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using tfsshelvesetexplorer.Models;
using tfsshelvesetexplorer.Services;

namespace tfsshelvesetexplorer.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
		private bool _isBusy;
		private string _serverUrl;
		private string _userName;
		private string _ownerName;
		private string _status;
		private ObservableCollection<MainRowViewModel> _rows;
		private readonly IShelvesetService _shelvesetService;
		private readonly IBackupService _backupService;
		private readonly IDefaultParameterService _defaultParameterService;

		public bool IsBusy { get { return _isBusy; } set { Set(ref _isBusy, value); } }
		public string ServerUrl { get { return _serverUrl; } set { Set(ref _serverUrl, value); } }
		public string UserName { get { return _userName; } set { Set(ref _userName, value); } }
		public string OwnerName { get { return _ownerName; } set { Set(ref _ownerName, value); } }
		public ObservableCollection<MainRowViewModel> Rows { get { return _rows; } set { Set(ref _rows, value); } }
		public MainRowViewModel SelectedRow { get; set; }
		public string Status { get { return _status; } set { Set(ref _status, value); } }

		public ICommand ViewCommand { get; private set; }

		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainViewModel(
			IShelvesetService shelvesetService, 
			IBackupService backupService, 
			IDefaultParameterService defaultParameterService,
			IMessenger messenger)
			: base(messenger)
        {
			////if (IsInDesignMode)
			////{
			////    // Code runs in Blend --> create design time data.
			////}
			////else
			////{
			////    // Code runs "for real"
			////}
			_shelvesetService = shelvesetService;
			_backupService = backupService;
			_defaultParameterService = defaultParameterService;
			_serverUrl = _defaultParameterService.DefaultServerUrl;
			_userName = _defaultParameterService.DefaultUserName;
			_rows = new ObservableCollection<MainRowViewModel>();

			ViewCommand = new RelayCommand(() => View());
			messenger.Register<MainWindowDownloadMessage>(this, (m) => Download(m));
        }

		private async void View()
		{
			if (new[] { _serverUrl, _userName, _ownerName }.All(s => !string.IsNullOrWhiteSpace(s)))
			{
				DispatcherHelper.CheckBeginInvokeOnUI(() =>
				{
					IsBusy = true;
					Status = string.Empty;
				});

				var rows = new List<MainRowViewModel>();
				try
				{
					List<IShelveset> shelvesets = await Task.Run(() => _shelvesetService.GetShelvesetByOwner(_serverUrl, _userName, _ownerName)
						.OrderByDescending(e => e.LastModificationTime)
						.ToList());
					rows = shelvesets.Select(s => new MainRowViewModel(s)).ToList();
				}
				catch(Exception ex)
				{
					DispatcherHelper.CheckBeginInvokeOnUI(() =>
					{
						Status = $"we have a problems: {ex.Message}";
					});
				}
				finally
				{
					DispatcherHelper.CheckBeginInvokeOnUI(() =>
					{
						IsBusy = false;
						Rows = new ObservableCollection<MainRowViewModel>(rows);
					});
				}

				_defaultParameterService.DefaultServerUrl = _serverUrl;
				_defaultParameterService.DefaultUserName = _userName;
			}
		}

		private async void Download(MainWindowDownloadMessage message)
		{
			if (SelectedRow != null)
			{
				DispatcherHelper.CheckBeginInvokeOnUI(() => IsBusy = true);

				IShelveset shelveset = SelectedRow.ToShelveset();
				int fileCount = await Task.Run(() => _backupService.DownloadShelveset(message.DestinationFolder, new[] { shelveset }));

				DispatcherHelper.CheckBeginInvokeOnUI(() => IsBusy = false);
			}
		}
    }

	public class MainRowViewModel
	{
		public string Id => _shelveset.Id;
		public string Name => _shelveset.Name;
		public string Owner => _shelveset.Owner;
		public DateTime Timestamp => _shelveset.LastModificationTime;

		private readonly IShelveset _shelveset;

		public MainRowViewModel(IShelveset shelveset)
		{
			_shelveset = shelveset;
		}

		public IShelveset ToShelveset()
		{
			return _shelveset;
		}
	}
}