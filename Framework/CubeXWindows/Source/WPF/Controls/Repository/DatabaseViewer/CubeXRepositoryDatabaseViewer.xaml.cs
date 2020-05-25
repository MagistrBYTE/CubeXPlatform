//=====================================================================================================================
// Проект: CubeXWindows
// Раздел: Модуль работы с WPF
// Подраздел: Элементы интерфейса
// Группа: Элементы для работы с репозиториями
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXRepositoryDatabaseViewer.xaml.cs
*		Основной элемент для просмотра репозитория в виде базы данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 24.05.2020
//=====================================================================================================================
using System;
using System.Data;
using System.ComponentModel;
using System.Globalization;
//---------------------------------------------------------------------------------------------------------------------
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
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
		/// Конвертер типа <see cref="DataColumn"/> в соответствующую графическую пиктограмму
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[ValueConversion(typeof(DataColumn), typeof(BitmapSource))]
		public sealed class DataColumnToBitmapSourceConverter : IValueConverter
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Пиктограмма для обычного столбца
			/// </summary>
			public BitmapSource Default { get; set; }

			/// <summary>
			/// Пиктограмма для уникального столбца
			/// </summary>
			public BitmapSource Unique { get; set; }

			/// <summary>
			/// Пиктограмма для уникального столбца с внешним ключом
			/// </summary>
			public BitmapSource ForeignKey { get; set; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация объекта DataColumn в соответствующую графическую пиктограмму
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="target_type">Целевой тип</param>
			/// <param name="parameter">Дополнительный параметр</param>
			/// <param name="culture">Культура</param>
			/// <returns>Графическая пиктограмма</returns>
			//---------------------------------------------------------------------------------------------------------
			public Object Convert(Object value, Type target_type, Object parameter, CultureInfo culture)
			{
				DataColumn column = (DataColumn)value;
				BitmapSource bitmap = Default;

				if(column.IsForeignKey())
				{
					bitmap = ForeignKey;
				}
				else
				{
					if (column.Unique)
					{
						bitmap = Unique;
					}
				}

				return (bitmap);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация графической пиктограммы в тип DataColumn
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="target_type">Целевой тип</param>
			/// <param name="parameter">Дополнительный параметр</param>
			/// <param name="culture">Культура</param>
			/// <returns>Объект TLogType</returns>
			//---------------------------------------------------------------------------------------------------------
			public Object ConvertBack(Object value, Type target_type, Object parameter, CultureInfo culture)
			{
				return (null);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Селектор шаблона данных для соответствующего типа данных репозитория
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CRepositoryDataEntitySelector : DataTemplateSelector
		{
			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Шаблон для представления таблицы
			/// </summary>
			public DataTemplate Table { get; set; }

			/// <summary>
			/// Шаблон для представления столбца
			/// </summary>
			public DataTemplate Column { get; set; }
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выбор шаблона привязки данных
			/// </summary>
			/// <param name="item">Объект</param>
			/// <param name="container">Контейнер</param>
			/// <returns>Нужный шаблон</returns>
			//---------------------------------------------------------------------------------------------------------
			public override DataTemplate SelectTemplate(Object item, DependencyObject container)
			{
				DataTable table = item as DataTable;
				if(table != null)
				{
					return (Table);
				}
				else
				{
					return (Column);
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Селектор шаблона данных для соответствующего ограничения данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CRepositoryDataConstraintSelector : DataTemplateSelector
		{
			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Шаблон для представления ограничения уникального значения
			/// </summary>
			public DataTemplate Unique { get; set; }

			/// <summary>
			/// Шаблон для представления ограничения внешнего ключа
			/// </summary>
			public DataTemplate ForeignKey { get; set; }
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выбор шаблона привязки данных
			/// </summary>
			/// <param name="item">Объект</param>
			/// <param name="container">Контейнер</param>
			/// <returns>Нужный шаблон</returns>
			//---------------------------------------------------------------------------------------------------------
			public override DataTemplate SelectTemplate(Object item, DependencyObject container)
			{
				UniqueConstraint unique = item as UniqueConstraint;
				if (unique != null)
				{
					return (Unique);
				}
				else
				{
					return (ForeignKey);
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Основной элемент для просмотра репозитория в виде базы данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public partial class CubeXRepositoryDatabaseViewer : Window
		{
			#region ======================================= ОПРЕДЕЛЕНИЕ СВОЙСТВ ЗАВИСИМОСТИ ===========================
			/// <summary>
			/// Репозиторий базы данных
			/// </summary>
			public static readonly DependencyProperty RepositoryProperty = DependencyProperty.Register(nameof(Repository),
				typeof(RepositoryDatabase), typeof(CubeXRepositoryDatabaseViewer), new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.None,
					OnRepositoryChanged));

			/// <summary>
			/// Выбранная таблица
			/// </summary>
			public static readonly DependencyProperty SelectedTableProperty = DependencyProperty.Register(nameof(SelectedTable),
				typeof(DataTable), typeof(CubeXRepositoryDatabaseViewer), new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.None,
					OnSelectedTableChanged));

			/// <summary>
			/// Выбранная строка
			/// </summary>
			public static readonly DependencyProperty SelectedRowProperty = DependencyProperty.Register(nameof(SelectedRow),
				typeof(DataRow), typeof(CubeXRepositoryDatabaseViewer), new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.None,
					OnSelectedRowChanged));

			/// <summary>
			/// Статус группирования по выбранному столбцу
			/// </summary>
			public static readonly DependencyProperty IsGroupingProperty = DependencyProperty.Register(nameof(IsGrouping),
				typeof(Boolean), typeof(CubeXRepositoryDatabaseViewer), new FrameworkPropertyMetadata(false,
					FrameworkPropertyMetadataOptions.None,
					OnIsGroupingChanged));

			/// <summary>
			/// Статус фильтрации данных при отображении
			/// </summary>
			public static readonly DependencyProperty IsFiltrationProperty = DependencyProperty.Register(nameof(IsFiltration),
				typeof(Boolean), typeof(CubeXRepositoryDatabaseViewer), new FrameworkPropertyMetadata(false,
					FrameworkPropertyMetadataOptions.None,
					OnIsFiltrationChanged));
			#endregion

			#region ======================================= МЕТОДЫ СВОЙСТВ ЗАВИСИМОСТИ ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик события изменения репозитория
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnRepositoryChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
				CubeXRepositoryDatabaseViewer data_viewer = (CubeXRepositoryDatabaseViewer)sender;
				RepositoryDatabase new_value = (RepositoryDatabase)args.NewValue;
				if (new_value != null)
				{
					data_viewer.SetRepository();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик события изменения выбранной таблицы
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnSelectedTableChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик события изменения выбранной строки
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnSelectedRowChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик события изменения статуса группирования
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnIsGroupingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
				CubeXRepositoryDatabaseViewer data_viewer = (CubeXRepositoryDatabaseViewer)sender;
				Boolean new_value = (Boolean)args.NewValue;

				if(new_value)
				{
					data_viewer.SetGroupings();
				}
				else
				{
					data_viewer.UnsetGroupings();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик события изменения статуса фильтрации данных
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnIsFiltrationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
				CubeXRepositoryDatabaseViewer data_viewer = (CubeXRepositoryDatabaseViewer)sender;
				Boolean new_value = (Boolean)args.NewValue;

				if (new_value)
				{
					if(data_viewer.SelectedTable != null)
					{
						//data_viewer.SelectedTable.DefaultView.RowFilter
					}
				}
				else
				{
					data_viewer.UnsetGroupings();
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Репозиторий базы данных
			/// </summary>
			public RepositoryDatabase Repository
			{
				get { return (RepositoryDatabase)GetValue(RepositoryProperty); }
				set { SetValue(RepositoryProperty, value); }
			}

			/// <summary>
			/// Выбранная таблица
			/// </summary>
			public DataTable SelectedTable
			{
				get { return (DataTable)GetValue(SelectedTableProperty); }
				set { SetValue(SelectedTableProperty, value); }
			}

			/// <summary>
			/// Выбранная строка
			/// </summary>
			public DataRow SelectedRow
			{
				get { return (DataRow)GetValue(SelectedRowProperty); }
				set { SetValue(SelectedRowProperty, value); }
			}

			/// <summary>
			/// Статус группирования по выбранному столбцу
			/// </summary>
			[Browsable(false)]
			public Boolean IsGrouping
			{
				get { return (Boolean)GetValue(IsGroupingProperty); }
				set { SetValue(IsGroupingProperty, value); }
			}

			/// <summary>
			/// Статус фильтрации данных при отображении
			/// </summary>
			[Browsable(false)]
			public Boolean IsFiltration
			{
				get { return (Boolean)GetValue(IsGroupingProperty); }
				set { SetValue(IsGroupingProperty, value); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CubeXRepositoryDatabaseViewer()
			{
				InitializeComponent();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка репозитория
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetRepository()
			{
				treeTables.ItemsSource = Repository.DataSet.Tables;
			}
			#endregion

			#region ======================================= МЕТОДЫ ФИЛЬТРОВАНИЯ СВОЙСТВ ===============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на соответствие фильтру
			/// </summary>
			/// <param name="item">Объект</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			protected virtual Boolean OnPropertyViewFilter(Object item)
			{
				//if (String.IsNullOrEmpty(FilterString))
				//{
					return (true);
				//}
				//else
				//{
				//	CPropertyModelBase property_model = item as CPropertyModelBase;
				//	return (property_model.DisplayName.Contains(FilterString, StringComparison.OrdinalIgnoreCase));
				//}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение строки фильтра
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnTextFilterPropertyTextChanged(Object sender, TextChangedEventArgs args)
			{
				//FilterString = textFilterProperty.Text;
			}
			#endregion

			#region ======================================= МЕТОДЫ ГРУППИРОВАНИЯ СВОЙСТВ ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка группирования
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void SetGroupings()
			{
				//if (mPropertiesView != null)
				//{
				//	mPropertiesView.GroupDescriptions.Clear();
				//	mPropertiesView.GroupDescriptions.Add(PropertyGroupDescriptionGroup);
				//}

				//SelectedTable.DefaultView.Grop
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаления группирования
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void UnsetGroupings()
			{
				//if (mPropertiesView != null)
				//{
				//	mPropertiesView.GroupDescriptions.Remove(PropertyGroupDescriptionGroup);
				//}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Группирование свойств по категории
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnRadioButtonGroupChecked(Object sender, RoutedEventArgs args)
			{
				//toogleButtonAlphabetically.IsChecked = false;
				//SetGroupings();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Группирование свойств по алфавиту
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnRadioButtonAlphabeticallyChecked(Object sender, RoutedEventArgs args)
			{
				//toogleButtonGroup.IsChecked = false;
				//UnsetGroupings();
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
			/// Сохранение базы данных
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnButtonSaveDatabase_Click(Object sender, RoutedEventArgs args)
			{
				if (Repository != null)
				{
					Repository.Save();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Восстановление базы данных к моменту последнего сохранения
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnButtonRestoreDatabase_Click(Object sender, RoutedEventArgs args)
			{
				if (Repository != null)
				{
					Repository.Restore();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выбор таблицы для отображения данных
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnTreeTables_SelectedItemChanged(Object sender, RoutedPropertyChangedEventArgs<Object> args)
			{
				DataTable table = treeTables.SelectedItem as DataTable;
				if (table != null)
				{
					SelectedTable = table;
					dataTable.Columns.Clear();
					dataTable.ItemsSource = SelectedTable.DefaultView;
					listConstraint.ItemsSource = SelectedTable.Constraints;
					comboGroupColumn.ItemsSource = SelectedTable.Columns;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Процесс генерирование столбцов
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnDataTable_AutoGeneratingColumn(Object sender, DataGridAutoGeneratingColumnEventArgs args)
			{
				// Есть ли ограничение
				if(SelectedTable != null)
				{
					for (Int32 i = 0; i < SelectedTable.Constraints.Count; i++)
					{
						Constraint constraint = SelectedTable.Constraints[i];
						if(constraint is ForeignKeyConstraint foreign_key)
						{
							if(foreign_key.Columns[0].ColumnName == args.PropertyName)
							{
								DataGridComboBoxColumn comboBoxColumn = new DataGridComboBoxColumn();
								args.Column = comboBoxColumn;
								args.Column.Header = "Город";
								args.Column.Visibility = Visibility.Visible;
								args.Cancel = false;

								// Загружаеи из источника
								comboBoxColumn.ItemsSource = foreign_key.RelatedTable.DefaultView;
								comboBoxColumn.DisplayMemberPath = "short_name";
								comboBoxColumn.SelectedValuePath = "id";

								Binding binding = new Binding();
								binding.Path = new PropertyPath(args.PropertyName);
								//binding.Source = textStatus;
								//binding.Converter = StringToIntConverter.Instance;

								comboBoxColumn.SelectedValueBinding = binding;

								//Binding binding = new Binding();
								//binding.Path = new PropertyPath(args.PropertyName);
								//binding.Mode = BindingMode.TwoWay;
								//binding.Converter = DataRowToIntConverter.Instance;
								//binding.ConverterParameter = foreign_key.RelatedColumns[i].ColumnName;
								//comboBoxColumn.SelectedItemBinding = binding;


								return;
							}
						}
					}
				}

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

				args.Cancel = false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончание генерирование столбцов
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnDataTable_AutoGeneratedColumns(Object sender, EventArgs args)
			{
				// Обновляем заголовки
				if(SelectedTable != null)
				{
					for (Int32 i = 0; i < dataTable.Columns.Count; i++)
					{
						if (SelectedTable.Columns[i].Caption != SelectedTable.Columns[i].ColumnName)
						{
							if(dataTable.Columns[i].Header.ToString().Contains("full"))
							{
								DataGridTextColumn textColumn = dataTable.Columns[i] as DataGridTextColumn;
								//textColumn.Wra
							}

							dataTable.Columns[i].Header = SelectedTable.Columns[i].Caption;

						}
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
			private void OnDataTable_CellEditEnding(Object sender, DataGridCellEditEndingEventArgs args)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавить запись справочника
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnButtonAddRecord_Click(Object sender, RoutedEventArgs args)
			{
				//Handbook.Records.Add(new CHandbookRecord());
				//Handbook.Save();
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