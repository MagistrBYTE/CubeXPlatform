//=====================================================================================================================
// Проект: CubeXWindows
// Раздел: Модуль работы с WPF
// Подраздел: Элементы интерфейса
// Группа: Элементы для работы с данными
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXDataGrid.xaml.cs
*		Элемент управления отображением данных с расширенной функциональностью.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 24.05.2020
//=====================================================================================================================
using System;
using System.ComponentModel;
using System.Collections;
using System.Data;
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
		//! \defgroup WindowsWPFControlsData Элементы для работы с данными
		//! \ingroup WindowsWPFControls
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Элемент управления отображением данных с расширенной функциональностью
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public partial class CubeXDataGrid : DataGrid
		{
			#region ======================================= ОПРЕДЕЛЕНИЕ СВОЙСТВ ЗАВИСИМОСТИ ===========================
			//
			// ОБЩИЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Схема данных
			/// </summary>
			public static readonly DependencyProperty SchemeProperty = DependencyProperty.Register(nameof(Scheme),
				typeof(CSchemeFlatData), typeof(CubeXDataGrid), new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.None,
					OnSchemeChanged));

			/// <summary>
			/// Список записей
			/// </summary>
			public static readonly DependencyProperty ListRecordsProperty = DependencyProperty.Register(nameof(ListRecords),
				typeof(IList), typeof(CubeXDataGrid), new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.None,
					OnListRecordsChanged));

			/// <summary>
			/// Словарь записей
			/// </summary>
			public static readonly DependencyProperty DictionaryRecordsProperty = DependencyProperty.Register(nameof(DictionaryRecords),
				typeof(IDictionary), typeof(CubeXDataGrid), new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.None,
					OnDictionaryRecordsChanged));

			/// <summary>
			/// Таблица записей
			/// </summary>
			public static readonly DependencyProperty TableRecordsProperty = DependencyProperty.Register(nameof(TableRecords),
				typeof(DataTable), typeof(CubeXDataGrid), new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.None,
					OnTableRecordsChanged));

			//
			// ФИЛЬТРАЦИЯ ДАННЫХ
			//
			/// <summary>
			/// Предикат фильтрации
			/// </summary>
			public static readonly DependencyProperty FilterPredicateProperty = DependencyProperty.Register(nameof(FilterPredicate),
				typeof(Predicate<System.Object>), typeof(CubeXDataGrid), new FrameworkPropertyMetadata(FilterPredicateDefault, 
					FrameworkPropertyMetadataOptions.None));

			/// <summary>
			/// Статус отображения элементов фильтрования у столбцов
			/// </summary>
			public static readonly DependencyProperty IsShowFilterColumnProperty = DependencyProperty.Register(nameof(IsShowFilterColumn),
				typeof(Boolean), typeof(CubeXDataGrid), new FrameworkPropertyMetadata(false,
					FrameworkPropertyMetadataOptions.AffectsRender|FrameworkPropertyMetadataOptions.AffectsMeasure,
					OnShowFilterColumnChanged));
			#endregion

			#region ======================================= МЕТОДЫ СВОЙСТВ ЗАВИСИМОСТИ ================================
			/// <summary>
			/// Предикат фильтрации по умолчанию
			/// </summary>
			private static Predicate<System.Object> FilterPredicateDefault = delegate { return (true); };

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик события изменения схемеы данных
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnSchemeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
				CubeXDataGrid data_viewer = (CubeXDataGrid)sender;
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
				CubeXDataGrid data_viewer = (CubeXDataGrid)sender;
				IList new_value = (IList)args.NewValue;
				if (new_value != null)
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
				CubeXDataGrid data_viewer = (CubeXDataGrid)sender;
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
				CubeXDataGrid data_viewer = (CubeXDataGrid)sender;
				DataTable new_value = (DataTable)args.NewValue;
				if (new_value != null)
				{
					data_viewer.ItemsSource = new_value.DefaultView;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик события изменения статуса отображения элементов фильтрования у столбцов
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnShowFilterColumnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
				CubeXDataGrid data_grid = (CubeXDataGrid)sender;
				Boolean new_value = (Boolean)args.NewValue;
				if (new_value)
				{
					data_grid.ShowFilterColunm();
				}
				else
				{
					data_grid.HideFilterColunm();
				}
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			private CQuery FilterQuery;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОБЩИЕ ПАРАМЕТРЫ
			//
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

			//
			// ФИЛЬТРАЦИЯ ДАННЫХ
			//
			/// <summary>
			/// Статус отображения элементов фильтрования у столбцов
			/// </summary>
			[Browsable(false)]
			public Boolean IsShowFilterColumn
			{
				get { return (Boolean)GetValue(IsShowFilterColumnProperty); }
				set { SetValue(IsShowFilterColumnProperty, value); }
			}

			/// <summary>
			/// Предикат фильтрации
			/// </summary>
			[Browsable(false)]
			public Predicate<System.Object> FilterPredicate
			{
				get { return (Predicate<System.Object>)GetValue(FilterPredicateProperty); }
				set { SetValue(FilterPredicateProperty, value); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CubeXDataGrid()
			{
				InitializeComponent();
				SetResourceReference(StyleProperty, typeof(DataGrid));
				FilterQuery = new CQuery();
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
				if (ListRecords != null && ListRecords.Count > 0)
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

					dataRecord.ItemsSource = TableRecords.DefaultView;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ФИЛЬТРОВАНИЯ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показ фильтров в столбцах
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void ShowFilterColunm()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Скрытие фильтров в столбцах
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void HideFilterColunm()
			{

			}
			#endregion

			#region ======================================= ОБРАБОТЧИКИ СОБЫТИЙ =======================================
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
					args.Column.Header = "Код";
					return;
				}

				if (args.PropertyName == "name")
				{
					args.Column.Header = "Наименование";
					return;
				}

				if (args.PropertyName == "desc")
				{
					args.Column.Header = "Описание";
					return;
				}

				//if (args.PropertyType == typeof(String))
				//{
				//	ControlTemplate filter_template = FindResource("ControlStringFilterKey") as ControlTemplate;
				//	args.Column.SetTemplate(filter_template);
				//	args.Column.HeaderTemplate = 
				//}

				args.Cancel = false;
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
				if (TableRecords != null)
				{
					for (Int32 i = 0; i < dataRecord.Columns.Count; i++)
					{
						DataGridColumn column = dataRecord.Columns[i];
					}

					//System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
					//timer.Interval = TimeSpan.FromMilliseconds(0.000001);
					//timer.Tick += new EventHandler(delegate (object s, EventArgs a)
					//{
					//	timer.Stop();
					//	var cp = GetVisualChild<ContentPresenter>(Content_Option);
					//	var txt = Content_Option.ContentTemplate.FindName("Path_Cover", cp) as TextBox;
					//	txt.Text = "teSt";
					//});
					//timer.Start();
				}
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
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================