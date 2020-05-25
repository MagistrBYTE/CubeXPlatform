//=====================================================================================================================
// Проект: CubeXWindows
// Раздел: Модуль работы с WPF
// Подраздел: Элементы интерфейса
// Группа: Элементы для работы с репозиториями
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXRepositoryDataViewer.xaml.cs
*		Основной элемент для просмотра данных репозитория.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 24.05.2020
//=====================================================================================================================
using System;
using System.Data;
using System.Collections;
//---------------------------------------------------------------------------------------------------------------------
using System.Windows;
using System.Windows.Controls;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Windows
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup WindowsWPFControlsRepository
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Тип предоставления данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TDataViewerType
		{
			/// <summary>
			/// Примитивные данные
			/// </summary>
			/// <remarks>
			/// Примитивные данные - это базовые данные, отображается индекс, наименование и при наличии описание
			/// </remarks>
			Primitive,

			/// <summary>
			/// Модель
			/// </summary>
			/// <remarks>
			/// Модель отображает свои свойства по умолчанию.
			/// Если есть файл схемы организации данных то они отображается в соответствии с этой схемой
			/// </remarks>
			Model,

			/// <summary>
			/// Таблица
			/// </summary>
			/// <remarks>
			/// Таблица отображается в соответствии со своими столбцами по умолчанию.
			/// Если есть таблица схемы организации данных то они отображается в соответствии с этой схемой
			/// </remarks>
			Table
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Основной элемент для просмотра данных репозитория
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public partial class CubeXRepositoryDataViewer : Window, ICubeXUIRepositoryDataViewer
		{
			#region ======================================= ОПРЕДЕЛЕНИЕ СВОЙСТВ ЗАВИСИМОСТИ ===========================
			/// <summary>
			/// Схема данных
			/// </summary>
			public static readonly DependencyProperty SchemeProperty = DependencyProperty.Register(nameof(Scheme),
				typeof(CSchemeFlatData), typeof(CubeXRepositoryDataViewer), new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.None,
					OnSchemeChanged));

			/// <summary>
			/// Список записей
			/// </summary>
			public static readonly DependencyProperty ListRecordsProperty = DependencyProperty.Register(nameof(ListRecords), 
				typeof(IList), typeof(CubeXRepositoryDataViewer), new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.None,
					OnListRecordsChanged));

			/// <summary>
			/// Словарь записей
			/// </summary>
			public static readonly DependencyProperty DictionaryRecordsProperty = DependencyProperty.Register(nameof(DictionaryRecords),
				typeof(IDictionary), typeof(CubeXRepositoryDataViewer), new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.None,
					OnDictionaryRecordsChanged));

			/// <summary>
			/// Таблица записей
			/// </summary>
			public static readonly DependencyProperty TableRecordsProperty = DependencyProperty.Register(nameof(TableRecords),
				typeof(DataTable), typeof(CubeXRepositoryDataViewer), new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.None,
					OnTableRecordsChanged));

			/// <summary>
			/// Выражение для фильтра записей
			/// </summary>
			public static readonly DependencyProperty RowFilterProperty = DependencyProperty.Register(nameof(RowFilter),
				typeof(String), typeof(CubeXRepositoryDataViewer), new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.None,
					OnRowFilterChanged));

			/// <summary>
			/// Выбранная запись
			/// </summary>
			public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem),
				typeof(System.Object), typeof(CubeXRepositoryDataViewer), new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.None));

			/// <summary>
			/// Выбранный индекс записи
			/// </summary>
			public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof(SelectedIndex),
				typeof(Int32), typeof(CubeXRepositoryDataViewer), new FrameworkPropertyMetadata(-1,
					FrameworkPropertyMetadataOptions.None));
			#endregion

			#region ======================================= МЕТОДЫ СВОЙСТВ ЗАВИСИМОСТИ ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик события изменения схемеы данных
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnSchemeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
				CubeXRepositoryDataViewer data_viewer = (CubeXRepositoryDataViewer)sender;
				CSchemeFlatData new_value = (CSchemeFlatData)args.NewValue;
				if (new_value != null)
				{
					data_viewer.SetScheme();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик события изменения списка записей
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnListRecordsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
				CubeXRepositoryDataViewer data_viewer = (CubeXRepositoryDataViewer)sender;
				IList new_value = (IList)args.NewValue;
				if(new_value != null)
				{
					data_viewer.SetListRecords();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик события изменения словаря записей
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnDictionaryRecordsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
				CubeXRepositoryDataViewer data_viewer = (CubeXRepositoryDataViewer)sender;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик события изменения таблицы записей
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnTableRecordsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
				CubeXRepositoryDataViewer data_viewer = (CubeXRepositoryDataViewer)sender;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик события изменения выражения для фильтра записей
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnRowFilterChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
				CubeXRepositoryDataViewer data_viewer = (CubeXRepositoryDataViewer)sender;
				String new_value = (String)args.NewValue;
				if (data_viewer.TableRecords != null)
				{
					data_viewer.TableRecords.DefaultView.RowFilter = new_value;
				}
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			internal TDataViewerType mDataViewerType;
			internal ICubeXRepository mRepository;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Схема данных
			/// </summary>
			public CSchemeFlatData Scheme
			{
				get { return (CSchemeFlatData)GetValue(SchemeProperty); }
				set { SetValue(SchemeProperty, value); }
			}

			/// <summary>
			/// Список записей
			/// </summary>
			public IList ListRecords
			{
				get { return (IList)GetValue(ListRecordsProperty); }
				set { SetValue(ListRecordsProperty, value); }
			}

			/// <summary>
			/// Словарь записей
			/// </summary>
			public IDictionary DictionaryRecords
			{
				get { return (IDictionary)GetValue(DictionaryRecordsProperty); }
				set { SetValue(DictionaryRecordsProperty, value); }
			}

			/// <summary>
			/// Таблица записей
			/// </summary>
			public DataTable TableRecords
			{
				get { return (DataTable)GetValue(TableRecordsProperty); }
				set { SetValue(TableRecordsProperty, value); }
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXUIRepositoryDataViewer =====================
			/// <summary>
			/// Репозиторий
			/// </summary>
			public ICubeXRepository Repository
			{
				get { return (mRepository); }
				set
				{
					mRepository = value;
					if(mRepository !=null)
					{
						SetRepository();
					}
				}
			}

			/// <summary>
			/// Выражение для фильтра записей
			/// </summary>
			public String RowFilter
			{
				get { return (String)GetValue(RowFilterProperty); }
				set { SetValue(RowFilterProperty, value); }
			}

			/// <summary>
			/// Выбранная запись
			/// </summary>
			public System.Object SelectedItem
			{
				get { return (System.Object)GetValue(SelectedItemProperty); }
				set { SetValue(SelectedItemProperty, value); }
			}

			/// <summary>
			/// Выбранный индекс записи
			/// </summary>
			public Int32 SelectedIndex
			{
				get { return (Int32)GetValue(SelectedIndexProperty); }
				set { SetValue(SelectedIndexProperty, value); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CubeXRepositoryDataViewer()
			{
				InitializeComponent();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка схемы данных
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetScheme()
			{
				if (Scheme != null && Scheme.Columns.Count > 0)
				{
					if (TableRecords == null)
					{
						TableRecords = new DataTable();
					}

					TableRecords.TableName = Scheme.Name;

					TableRecords.Columns.Clear();
					for (Int32 i = 0; i < Scheme.Columns.Count; i++)
					{
						// Столбцы
						DataColumn column_data = new DataColumn();
						column_data.ColumnName = Scheme.Columns[i].Name;
						column_data.Caption = Scheme.Columns[i].Caption;
						column_data.DataType = Scheme.Columns[i].DataType;
						TableRecords.Columns.Add(column_data);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка отображения простого списка записей
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetListRecords()
			{
				if(ListRecords != null && ListRecords.Count > 0)
				{
					IList list = ListRecords;

					// Заполняем таблицу данных
					for (Int32 r = 0; r < list.Count; r++)
					{
						IList record = list[r] as IList;
						if (record != null && record.Count > 0)
						{
							DataRow row = TableRecords.NewRow();
							for (Int32 c = 0; c < record.Count; c++)
							{
								row[c] = record[c];
							}

							TableRecords.Rows.Add(row);
						}
					}

					// Элемент управления
					dataRecords.Columns.Clear();
					dataRecords.ItemsSource = TableRecords.DefaultView;
					mDataViewerType = TDataViewerType.Table;

					labelCountRecord.Content = TableRecords.DefaultView.Count.ToString();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка репозитория
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetRepository()
			{
				// Устанавливаем схему
				Scheme = Repository.Scheme;

				// Устанавливаем список данных
				ListRecords = Repository.IRecords;
			}
			#endregion

			#region ======================================= ОБРАБОТЧИКИ СОБЫТИЙ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка окна и готовность его к отображению
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnWindow_Loaded(Object sender, RoutedEventArgs args)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Процесс генерирование столбцов
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnDataRecords_AutoGeneratingColumn(Object sender, DataGridAutoGeneratingColumnEventArgs args)
			{
				if (args.PropertyName == "id")
				{
					args.Cancel = false;
					args.Column.Header = "Код";
					return;
				}

				if (args.PropertyName == "name")
				{
					args.Cancel = false;
					args.Column.Header = "Наименование";
					return;
				}

				if (args.PropertyName == "desc")
				{
					args.Cancel = false;
					args.Column.Header = "Описание";
					return;
				}

				args.Cancel = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончание генерирование столбцов
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnDataRecords_AutoGeneratedColumns(Object sender, EventArgs args)
			{
				// Обновляем заголовки
				for (Int32 i = 0; i < dataRecords.Columns.Count; i++)
				{
					if (Scheme.Columns[i].IsHide)
					{
						dataRecords.Columns[i].Visibility = Visibility.Collapsed;
					}
					else
					{
						dataRecords.Columns[i].Header = TableRecords.Columns[i].Caption;
					}
				}

				Int32 index = ((DataGrid)sender).Columns.Count;
				DataGridColumn column = ((DataGrid)sender).Columns[index - 1];
				column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Редактирование данных
			/// </summary>
			/// <remarks>
			/// Возникает перед выходом ячейки из режима редактирования
			/// </remarks>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnDataRecords_CellEditEnding(Object sender, DataGridCellEditEndingEventArgs args)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавить запись
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnButtonAddRecord_Click(Object sender, RoutedEventArgs args)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление выбранной записи справочника
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnButtonRemoveRecord_Click(Object sender, RoutedEventArgs args)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выбор записи
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnButtonSelectRecord_Click(Object sender, RoutedEventArgs args)
			{
				DataRowView row_view = dataRecords.SelectedItem as DataRowView;
				if (row_view != null)
				{
					DataRow row = row_view.Row;
					switch (mDataViewerType)
					{
						case TDataViewerType.Primitive:
							{
								SelectedIndex = (Int32)row[0];
								SelectedItem = row[1];
							}
							break;
						case TDataViewerType.Model:
							{
								//SelectedIndex = (Int32)row[0];
								SelectedItem = row;
							}
							break;
						case TDataViewerType.Table:
							{
								//SelectedIndex = (Int32)row[0];
								SelectedItem = row;
							}
							break;
						default:
							break;
					}
				}

				Close();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена выбора записи
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnButtonCancelRecord_Click(Object sender, RoutedEventArgs args)
			{

			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================