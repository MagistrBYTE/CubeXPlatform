﻿//=====================================================================================================================
// Проект: CubeXWindows
// Раздел: Модуль работы с WPF
// Подраздел: Методы расширения
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXWindowsExtensionPoint.cs
*		Методы расширения для работы с типом Point.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 24.05.2020
//=====================================================================================================================
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Windows
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup WindowsWPFExtension
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="Point"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionWindowsPoint
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация точки в строку
			/// </summary>
			/// <param name="point">Двухмерная точка</param>
			/// <returns>Строка данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String SerializeToString(this Point point)
			{
				return (String.Format("{0};{1}", point.X, point.Y));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Десереализация двухмерной точки из строки
			/// </summary>
			/// <param name="data">Строка данных</param>
			/// <returns>Двухмерная точка</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Point DeserializeFromString(String data)
			{
				Point point = new Point();
				String[] vector_data = data.Split(';');
				point.X = (XNumbers.ParseDouble(vector_data[0]));
				point.Y = (XNumbers.ParseDouble(vector_data[1]));
				return (point);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства двухмерных точек
			/// </summary>
			/// <param name="point">Первое значение</param>
			/// <param name="other">Второе значение</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean Approximately(this Point point, Point other, Double epsilon)
			{
				if ((Math.Abs(point.X - other.X) < epsilon) && (Math.Abs(point.Y - other.Y) < epsilon))
				{
					return (true);
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства двухмерных точек
			/// </summary>
			/// <param name="point">Первое значение</param>
			/// <param name="other">Второе значение</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean Approximately(this Point point, Point other)
			{
				if ((Math.Abs(point.X - other.X) < 0.001) && (Math.Abs(point.Y - other.Y) < 0.001))
				{
					return (true);
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование в вектор Maths.Vector2D
			/// </summary>
			/// <param name="point">Точка</param>
			/// <returns>Вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Maths.Vector2D ToVector2D(this Point point)
			{
				return (new Maths.Vector2D(point.X, point.Y));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование в вектор Maths.Vector2Df
			/// </summary>
			/// <param name="point">Точка</param>
			/// <returns>Вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Maths.Vector2Df ToVector2Df(this Point point)
			{
				return (new Maths.Vector2Df((Single)point.X, (Single)point.Y));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование в Win32Point
			/// </summary>
			/// <param name="point">Точка</param>
			/// <returns>Точка</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Win32Point ToWin32Point(this Point point)
			{
				Win32Point window_point = new Win32Point();
				window_point.X = (Int32)point.X;
				window_point.Y = (Int32)point.Y;
				return (window_point);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================