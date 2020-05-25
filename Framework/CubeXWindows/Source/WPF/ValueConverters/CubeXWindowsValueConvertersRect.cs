﻿//=====================================================================================================================
// Проект: CubeXWindows
// Раздел: Модуль работы с WPF
// Подраздел: Конвертеры данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXWindowsValueConvertersRect.cs
*		Конвертеры типа Rect в различные типы данных.
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
		/// Конвертер типа Rect в строку
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[ValueConversion(typeof(Size), typeof(String))]
		public class RectToStringConverter : IValueConverter
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация типа Rect в строковый тип
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="target_type">Целевой тип</param>
			/// <param name="parameter">Дополнительный параметр</param>
			/// <param name="culture">Культура</param>
			/// <returns>Строка</returns>
			//---------------------------------------------------------------------------------------------------------
			public Object Convert(Object value, Type target_type, Object parameter, CultureInfo culture)
			{
				Rect val = (Rect)value;
				return ("X = " + val.X.ToString("F2") + "; Y = " + val.Y.ToString("F2") + "; Width = " +
					val.Width.ToString("F2") + "; Height = " + val.Height.ToString("F2") + ";");
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация строкового типа в тип Rect
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="target_type">Целевой тип</param>
			/// <param name="parameter">Дополнительный параметр</param>
			/// <param name="culture">Культура</param>
			/// <returns>тип Rect</returns>
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