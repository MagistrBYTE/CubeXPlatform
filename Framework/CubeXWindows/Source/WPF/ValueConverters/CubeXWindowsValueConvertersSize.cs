﻿//=====================================================================================================================
// Проект: CubeXWindows
// Раздел: Модуль работы с WPF
// Подраздел: Конвертеры данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXWindowsValueConvertersSize.cs
*		Конвертеры типа Size в различные типы данных.
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
		/// Конвертер типа Size в строку
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[ValueConversion(typeof(Size), typeof(String))]
		public class SizeToStringConverter : IValueConverter
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация типа Size в строковый тип
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="target_type">Целевой тип</param>
			/// <param name="parameter">Дополнительный параметр</param>
			/// <param name="culture">Культура</param>
			/// <returns>Строка</returns>
			//---------------------------------------------------------------------------------------------------------
			public Object Convert(Object value, Type target_type, Object parameter, CultureInfo culture)
			{
				Size val = (Size)value;
				return ("Ширина = " + val.Width.ToString("F0") + "; Высота = " + val.Height.ToString("F0") + ";");
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация строкового типа в тип Size
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="target_type">Целевой тип</param>
			/// <param name="parameter">Дополнительный параметр</param>
			/// <param name="culture">Культура</param>
			/// <returns>тип Size</returns>
			//---------------------------------------------------------------------------------------------------------
			public Object ConvertBack(Object value, Type target_type, Object parameter, CultureInfo culture)
			{
				return (null);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================