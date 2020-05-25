//=====================================================================================================================
// Проект: CubeXWindows
// Раздел: Модуль работы с WPF
// Подраздел: Элементы интерфейса
// Группа: Элементы редактирования и выбора контента
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXVector2DEditor.xaml.cs
*		Элемент-редактор для редактирования свойства типа двухмерного вектора.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 24.05.2020
//=====================================================================================================================
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//---------------------------------------------------------------------------------------------------------------------
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Maths;
//=====================================================================================================================
namespace CubeX
{
	namespace Windows
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup WindowsWPFControlsEditor
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Универсальный конвертор типа Vector2D между различными типами представлений 
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class Vector2DToVector2DConverter : IValueConverter
		{
			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация объекта вектор в объект типа <see cref="Vector2D"/>
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="target_type">Целевой тип</param>
			/// <param name="parameter">Дополнительный параметр</param>
			/// <param name="culture">Культура</param>
			/// <returns>Объект <see cref="Vector2D"/></returns>
			//---------------------------------------------------------------------------------------------------------
			public Object Convert(Object value, Type target_type, Object parameter, CultureInfo culture)
			{
				if (value is Vector2D)
				{
					return (value);
				}

				if (value is System.Windows.Vector)
				{
					System.Windows.Vector v = (System.Windows.Vector)value;
					return (new Vector2D(v.X, v.Y));
				}

				if (value is System.Windows.Point)
				{
					System.Windows.Point v = (System.Windows.Point)value;
					return (new Vector2D(v.X, v.Y));
				}

#if USE_ASSIMP
				if (value is Assimp.Vector2D)
				{
					Assimp.Vector2D v = (Assimp.Vector2D)value;
					return (new Vector2D(v.X, v.Y));
				}
#endif

#if USE_SHARPDX
				if (value is SharpDX.Vector2)
				{
					SharpDX.Vector2 v = (SharpDX.Vector2)value;
					return (new Vector2D(v.X, v.Y));
				}
#endif

				return (Vector2D.Zero);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация объекта типа <see cref="Vector2D"/> в объект вектора
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="target_type">Целевой тип</param>
			/// <param name="parameter">Дополнительный параметр(реальный тип для преобразования)</param>
			/// <param name="culture">Культура</param>
			/// <returns>Объект вектора</returns>
			//---------------------------------------------------------------------------------------------------------
			public Object ConvertBack(Object value, Type target_type, Object parameter, CultureInfo culture)
			{
				Type real_type = (Type)parameter;
				if (real_type == null) real_type = target_type;

				if (real_type == typeof(Vector2D))
				{
					return (value);
				}

				if (real_type == typeof(System.Windows.Vector))
				{
					Vector2D v = (Vector2D)value;
					return (new System.Windows.Vector(v.X, v.Y));
				}

				if (real_type == typeof(System.Windows.Point))
				{
					Vector2D v = (Vector2D)value;
					return (new System.Windows.Point(v.X, v.Y));
				}

#if USE_ASSIMP
				if (real_type == typeof(Assimp.Vector2D))
				{
					Vector2D v = (Vector2D)value;
					return (new Assimp.Vector2D((Single)v.X, (Single)v.Y));
				}
#endif

#if USE_SHARPDX
				if (real_type == typeof(SharpDX.Vector2))
				{
					Vector2D v = (Vector2D)value;
					return (new SharpDX.Vector2((Single)v.X, (Single)v.Y));
				}
#endif
				return (value);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Элемент-редактор для редактирования свойства типа двухмерного вектора
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public partial class CubeXVector2DEditor : UserControl, ITypeEditor
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			public static Vector2D VectorCache;
			public static Vector2DToVector2DConverter VectorConverter = new Vector2DToVector2DConverter();
			#endregion

			#region ======================================= ОПРЕДЕЛЕНИЕ СВОЙСТВ ЗАВИСИМОСТИ ===========================
			/// <summary>
			/// Значение вектора
			/// </summary>
			public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(Vector2D),
				typeof(CubeXVector2DEditor), new FrameworkPropertyMetadata(Vector2D.Zero, OnValuePropertyChanged));
			#endregion

			#region ======================================= МЕТОДЫ СВОЙСТВ ЗАВИСИМОСТИ ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение свойства зависимости
			/// </summary>
			/// <param name="obj">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnValuePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
			{
				CubeXVector2DEditor editor_vector = (CubeXVector2DEditor)obj;
				Vector2D? value = ((Vector2D)(args.NewValue));
				if (value.HasValue)
				{
					editor_vector.IsEnabled = false;
					editor_vector.spinnerX.Value = value.Value.X;
					editor_vector.spinnerY.Value = value.Value.Y;
					editor_vector.IsEnabled = true;
				}
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			private PropertyItem mPropertyItem;
			private String mFormatRadix;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Значение вектора
			/// </summary>
			public Vector2D Value
			{
				get { return (Vector2D)GetValue(ValueProperty); }
				set { SetValue(ValueProperty, value); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями.
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CubeXVector2DEditor()
			{
				InitializeComponent();
			}
			#endregion

			#region ======================================= ЭЛЕМЕНТ РЕДАКТОРА =========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Элемент редактор свойства типа Vector2D
			/// </summary>
			/// <param name="property_item">Параметры свойства</param>
			/// <returns>Редактор</returns>
			//---------------------------------------------------------------------------------------------------------
			public FrameworkElement ResolveEditor(PropertyItem property_item)
			{
				var binding = new Binding(nameof(Value));
				binding.Source = property_item;
				binding.ValidatesOnExceptions = true;
				binding.ValidatesOnDataErrors = true;
				binding.Mode = property_item.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;
				binding.Converter = VectorConverter;
				binding.ConverterParameter = property_item.PropertyType;

				// Привязываемся к свойству
				BindingOperations.SetBinding(this, ValueProperty, binding);

				// Сохраняем объект
				mPropertyItem = property_item;

				return (this);
			}
			#endregion

			#region ======================================= ОБРАБОТЧИКИ СОБЫТИЙ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик события изменения координат X
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnSpinnerX_ValueChanged(Object sender, RoutedPropertyChangedEventArgs<Object> args)
			{
				if (spinnerX.Value != null)
				{
					Value = new Vector2D(spinnerX.Value.Value, Value.Y);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик события изменения координат Y
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnSpinnerY_ValueChanged(Object sender, RoutedPropertyChangedEventArgs<Object> args)
			{
				if (spinnerY.Value != null)
				{
					Value = new Vector2D(Value.X, spinnerY.Value.Value);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Открытие контекстного меню
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnButtonMenu_Click(Object sender, RoutedEventArgs args)
			{
				ButtonMenu.ContextMenu.IsOpen = true;
				if (VectorCache != Vector2D.Zero)
				{
					miPaste.Header = "Вставить (" + VectorCache.ToString("F1") + ")";
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка разрядности - ноль цифр после запятой
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnRadixZero_Checked(Object sender, RoutedEventArgs args)
			{
				mFormatRadix = "F0";
				spinnerX.FormatString = mFormatRadix;
				spinnerY.FormatString = mFormatRadix;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка разрядности - одна цифра после запятой
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnRadixOne_Checked(Object sender, RoutedEventArgs args)
			{
				mFormatRadix = "F1";
				spinnerX.FormatString = mFormatRadix;
				spinnerY.FormatString = mFormatRadix;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка разрядности - две цифры после запятой
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnRadixTwo_Checked(Object sender, RoutedEventArgs args)
			{
				mFormatRadix = "F2";
				spinnerX.FormatString = mFormatRadix;
				spinnerY.FormatString = mFormatRadix;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование вектора
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnMenuItemCopyVector_Click(Object sender, RoutedEventArgs args)
			{
				VectorCache = new Vector2D(spinnerX.Value.Value, spinnerY.Value.Value);
				if (VectorCache != Vector2D.Zero)
				{
					miPaste.Header = "Вставить (" + VectorCache.ToStringValue(mFormatRadix) + ")";
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вставка вектора
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnMenuItemPasteVector_Click(Object sender, RoutedEventArgs args)
			{
				spinnerX.Value = VectorCache.X;
				spinnerY.Value = VectorCache.Y;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка значения по умолчанию вектора
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnMenuItemSetDefaultVector_Click(Object sender, RoutedEventArgs args)
			{
				if (mPropertyItem != null)
				{
					for (Int32 i = 0; i < mPropertyItem.PropertyDescriptor.Attributes.Count; i++)
					{
						Attribute attr = mPropertyItem.PropertyDescriptor.Attributes[i];
						if (attr is CubeXDefaultValueAttribute)
						{
							CubeXDefaultValueAttribute def_value = attr as CubeXDefaultValueAttribute;

						//	// Сначала смотрим свойства
						//	Object v = mPropertyItem.Instance.GetObjectPropertyValue(def_value.NamePropertyDefaultValue);
						//	if (v == null)
						//	{
						//		// Смотрим поля
						//		v = mPropertyItem.Instance.GetObjectFieldValue(def_value.NamePropertyDefaultValue);
						//	}

						//	// Если все правильно
						//	if (v != null && v.GetType() == mPropertyItem.PropertyType)
						//	{
						//		// Конвертируем
						//		Value = (Vector2D)VectorConverter.Convert(v, mPropertyItem.PropertyType, mPropertyItem.PropertyType, null);
						//	}
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка вектора
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnMenuItemClearVector_Click(Object sender, RoutedEventArgs args)
			{
				spinnerX.Value = 0;
				spinnerY.Value = 0;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================