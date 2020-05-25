//=====================================================================================================================
using System;
using System.Windows;
using System.Windows.Threading;
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.WindowsAPICodePack.Shell;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Windows;
//=====================================================================================================================
namespace CubeX
{
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public partial class MainWindow : Window
	{
		#region =========================================== ДАННЫЕ ====================================================
		private const double topOffset = 20;
		private const double leftOffset = 380;
		//readonly CubeXGrowlNotification growlNotifications = new CubeXGrowlNotification();
		private ICubeXRepository repository;
		#endregion

		#region =========================================== КОНСТРУКТОРЫ ==============================================
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public MainWindow()
		{
			InitializeComponent();

			//growlNotifications.Top = SystemParameters.WorkArea.Top + topOffset;
			//growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset;

			//browser.ExplorerBrowserControl.Navigate((ShellObject)KnownFolders.Desktop);

			repository = new CRepositoryFileRecord("Данные");
			repository.Connect(XApplicationManager.GetPathFileData("PersonData.xml"), true);
			dataRecord.Scheme = repository.Scheme;
			dataRecord.ListRecords = repository.IRecords;

			propertyInspector.SelectedObject = repository;

		}
		#endregion

		#region =========================================== ОБРАБОТЧИКИ СОБЫТИЙ =======================================
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Загрузка окна и готовность его к отображению
		/// </summary>
		/// <param name="sender">Источник события</param>
		/// <param name="args">Аргументы события</param>
		//-------------------------------------------------------------------------------------------------------------
		private void OnWindow_Loaded(Object sender, RoutedEventArgs args)
		{
			//Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
			//{
			//	repository = new CRepositoryFileRecord("Данные");
			//	repository.Connect(XApplicationManager.GetPathFileData("PersonData.xml"), true);
			//	dataTest.Scheme = repository.Scheme;
			//	dataTest.ListRecords = repository.IRecords;
			//}));
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Показать окно сообщения
		/// </summary>
		/// <param name="sender">Источник события</param>
		/// <param name="args">Аргументы события</param>
		//-------------------------------------------------------------------------------------------------------------
		private void OnShowNotifyButton_Click(Object sender, RoutedEventArgs args)
		{
			//growlNotifications.AddNotification(TNotificationType.Error, 
			//	"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
		}
		#endregion
	}
}
//=====================================================================================================================