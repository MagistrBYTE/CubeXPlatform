﻿//=====================================================================================================================
// Проект: CubeXWindows
// Раздел: Модуль работы с WPF
// Подраздел: Конвертеры данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXWindowsValueConvertersEnum.cs
*		Конвертеры типа Enum в различные типы данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 24.05.2020
//=====================================================================================================================
using System;
using System.Globalization;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Automation;
using System.Windows.Markup;
using System.Xaml;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Windows
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup WindowsWPFValueConverters
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Конвертер для <see cref="Enum"/> в целочисленный тип
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[ValueConversion(typeof(Enum), typeof(Int32))]
		public class EnumToIntConverter : IValueConverter
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация типа Enum в целочисленный тип
			/// </summary>
			/// <param name="value">Значение тип Enum</param>
			/// <param name="target_type">Целевой тип</param>
			/// <param name="parameter">Дополнительный параметр</param>
			/// <param name="culture">Культура</param>
			/// <returns>Целочисленный тип</returns>
			//---------------------------------------------------------------------------------------------------------
			public Object Convert(Object value, Type target_type, Object parameter, CultureInfo culture)
			{
				if (value == null)
				{
					return (0);
				}

				Array values = Enum.GetValues(value.GetType());
				for (Int32 i = 0; i < values.Length; i++)
				{
					if (values.GetValue(i).ToString() == value.ToString())
					{
						return (i);
					}
				}

				return (0);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация целочисленного типа в тип Enum
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="target_type">Целевой тип</param>
			/// <param name="parameter">Дополнительный параметр</param>
			/// <param name="culture">Культура</param>
			/// <returns>Тип Enum</returns>
			//---------------------------------------------------------------------------------------------------------
			public Object ConvertBack(Object value, Type target_type, Object parameter, CultureInfo culture)
			{
				Array values = Enum.GetValues(target_type);
				Int32 index_value = 0;
				if (value != null)
				{
					index_value = (Int32)(value);
				}

				return (values.GetValue(index_value));
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Конвертер для <see cref="Enum"/>, преобразовывающий Enum к строке с учетом атрибута <see cref="DescriptionAttribute"/> 
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[ValueConversion(typeof(Enum), typeof(String))]
		public class EnumToStringConverter : IValueConverter
		{
			public static readonly EnumToStringConverter Instance = new EnumToStringConverter();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация типа Enum в строковый тип
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="target_type">Целевой тип</param>
			/// <param name="parameter">Дополнительный параметр</param>
			/// <param name="culture">Культура</param>
			/// <returns>Строка</returns>
			//---------------------------------------------------------------------------------------------------------
			public Object Convert(Object value, Type target_type, Object parameter, CultureInfo culture)
			{
				if (value == null)
				{
					return ("");
				}
				Type type_enum = value.GetType();
				return (XEnum.GetDescriptionOrName(type_enum, (Enum)value));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация строкового типа в тип Enum
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="target_type">Целевой тип</param>
			/// <param name="parameter">Дополнительный параметр</param>
			/// <param name="culture">Культура</param>
			/// <returns>Тип Enum</returns>
			//---------------------------------------------------------------------------------------------------------
			public Object ConvertBack(Object value, Type target_type, Object parameter, CultureInfo culture)
			{
				if (value == null)
				{
					return (null);
				}
				return (XEnum.ConvertFromDescriptionOrName(target_type, value.ToString()));
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================