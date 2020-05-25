﻿//=====================================================================================================================
// Проект: CubeXWindows
// Раздел: Модуль работы с WPF
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXWindowsConverters.cs
*		Реализация типовых конвертаций различных типов и структур в данный WPF.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 24.05.2020
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Maths;
//=====================================================================================================================
namespace CubeX
{
	namespace Windows
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup WindowsWPFCommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс для реализации методов конвертации
		/// </summary>
		/// <remarks>
		/// Используется для некоторых типовых преобразований
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XWindowsConverters
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация изображения из формата Bgra32 в формат Bgr24
			/// </summary>
			/// <param name="bitmap_source">Источник изображения</param>
			/// <returns>Сконвертированное изображение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static BitmapSource Bgra32ToBgr24(BitmapSource bitmap_source)
			{
				Int32 PixelWidth = bitmap_source.PixelWidth;
				Int32 PixelHeight = bitmap_source.PixelHeight;
				Double DpiX = bitmap_source.DpiX;
				Double DpiY = bitmap_source.DpiY;

				PixelFormat InputPixelFormat = bitmap_source.Format;
				BitmapPalette InputPalette = bitmap_source.Palette;
				Int32 InputBitsPerPixel = bitmap_source.Format.BitsPerPixel;
				Int32 InputStride = PixelWidth * InputBitsPerPixel / 8;
				Byte[] InputPixelsArray = new Byte[InputStride * PixelHeight];
				bitmap_source.CopyPixels(InputPixelsArray, InputStride, 0);

				PixelFormat PixelFormat = PixelFormats.Bgr24;
				BitmapPalette Palette = null;
				Int32 BitsPerPixel = 24;
				Int32 Stride = PixelWidth * BitsPerPixel / 8;
				Byte[] PixelsArray = new Byte[InputStride * PixelHeight / 4 * 3];

				Int32 i = 0; Int32 j = 0; Int32 k = 0;
				while (i < InputPixelsArray.Length / 4)
				{
					PixelsArray[k] = InputPixelsArray[j];
					PixelsArray[k + 1] = InputPixelsArray[j + 1];
					PixelsArray[k + 2] = InputPixelsArray[j + 2];

					i = i + 1;
					j = j + 4;
					k = k + 3;
				}

				bitmap_source = BitmapSource.Create(PixelWidth, PixelHeight, DpiX, DpiY, PixelFormat, Palette, PixelsArray, Stride);
				return bitmap_source;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация изображения из формата Bgra32 в формат Gray8
			/// </summary>
			/// <param name="bitmap_source">Источник изображения</param>
			/// <returns>Сконвертированное изображение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static BitmapSource Bgra32ToGray8(BitmapSource bitmap_source)
			{
				Int32 PixelWidth = bitmap_source.PixelWidth;
				Int32 PixelHeight = bitmap_source.PixelHeight;
				Double DpiX = bitmap_source.DpiX;
				Double DpiY = bitmap_source.DpiY;

				PixelFormat InputPixelFormat = bitmap_source.Format;
				BitmapPalette InputPalette = bitmap_source.Palette;
				Int32 InputBitsPerPixel = bitmap_source.Format.BitsPerPixel;
				Int32 InputStride = PixelWidth * InputBitsPerPixel / 8;
				Byte[] InputPixelsArray = new Byte[InputStride * PixelHeight];
				bitmap_source.CopyPixels(InputPixelsArray, InputStride, 0);

				PixelFormat PixelFormat = PixelFormats.Gray8;
				BitmapPalette Palette = null;
				Int32 BitsPerPixel = 8;
				Int32 Stride = PixelWidth * BitsPerPixel / 8;
				Byte[] A_PixelsArray = new Byte[InputStride * PixelHeight / 4];

				Int32 i = 0; Int32 j = 3;
				while (i < InputPixelsArray.Length / 4)
				{
					A_PixelsArray[i] = InputPixelsArray[j];

					i = i + 1;
					j = j + 4;
				}

				bitmap_source = BitmapSource.Create(PixelWidth, PixelHeight, DpiX, DpiY, PixelFormat, Palette, A_PixelsArray, Stride);
				return bitmap_source;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация списка векторов
			/// </summary>
			/// <param name="values">Список векторов одинарной точности</param>
			/// <returns>Список точек WPF</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IList<Point> ConvertToWindowsPoints(this IList<Vector2Df> values)
			{
				Point[] list = new Point[values.Count];
				for (Int32 i = 0; i < values.Count; i++)
				{
					list[i] = new Point(values[i].X, values[i].Y);
				}

				return (list);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация списка векторов
			/// </summary>
			/// <param name="values">Список векторов двойной точности</param>
			/// <returns>Список точек WPF</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IList<Point> ConvertToWindowsPoints(this IList<Vector2D> values)
			{
				Point[] list = new Point[values.Count];
				for (Int32 i = 0; i < values.Count; i++)
				{
					list[i] = new Point(values[i].X, values[i].Y);
				}

				return (list);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация общего курсора в курсор WPF
			/// </summary>
			/// <param name="cursor">Общий курсор</param>
			/// <returns>Курсор WPF</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Cursor ConvertToCursor(TCursor cursor)
			{
				Cursor result = Cursors.Arrow;

				switch (cursor)
				{
					case TCursor.Arrow:
						break;
					case TCursor.Cross:
						result = Cursors.Cross;
						break;
					case TCursor.Hand:
						result = Cursors.Hand;
						break;
					case TCursor.Help:
						result = Cursors.Help;
						break;
					case TCursor.No:
						result = Cursors.No;
						break;
					case TCursor.None:
						result = Cursors.None;
						break;
					case TCursor.Pen:
						result = Cursors.Pen;
						break;
					case TCursor.SizeAll:
						result = Cursors.SizeAll;
						break;
					case TCursor.SizeNESW:
						result = Cursors.SizeNESW;
						break;
					case TCursor.SizeNS:
						result = Cursors.SizeNS;
						break;
					case TCursor.SizeNWSE:
						result = Cursors.SizeNWSE;
						break;
					case TCursor.SizeWE:
						result = Cursors.SizeWE;
						break;
					default:
						break;
				}

				return (result);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация общей клавиши в клавишу WPF
			/// </summary>
			/// <param name="key">Общая клавиша</param>
			/// <returns>Клавиша WPF</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Key ConvertToKey(TKey key)
			{
				Key result = Key.A;

				switch (key)
				{
					case TKey.M:
						result = Key.M;
						break;
					case TKey.Z:
						result = Key.Z;
						break;
					case TKey.F1:
						result = Key.F1;
						break;
					case TKey.F2:
						result = Key.F2;
						break;
					case TKey.F3:
						result = Key.F3;
						break;
					case TKey.F4:
						result = Key.F4;
						break;
					case TKey.F5:
						result = Key.F5;
						break;
					case TKey.F6:
						result = Key.F6;
						break;
					case TKey.F7:
						result = Key.F7;
						break;
					case TKey.F8:
						result = Key.F8;
						break;
					case TKey.F9:
						result = Key.F9;
						break;
					case TKey.F10:
						result = Key.F10;
						break;
					case TKey.Escape:
						result = Key.Escape;
						break;
					case TKey.Enter:
						result = Key.Enter;
						break;
					case TKey.Space:
						result = Key.Space;
						break;
					case TKey.Delete:
						result = Key.Delete;
						break;
					case TKey.LeftControl:
						result = Key.LeftCtrl;
						break;
					case TKey.RightControl:
						result = Key.RightCtrl;
						break;
					case TKey.LeftShift:
						result = Key.LeftShift;
						break;
					case TKey.RightShift:
						result = Key.RightShift;
						break;
					case TKey.LeftArrow:
						result = Key.Left;
						break;
					case TKey.RightArrow:
						result = Key.Right;
						break;
					case TKey.UpArrow:
						result = Key.Up;
						break;
					case TKey.DownArrow:
						result = Key.Down;
						break;
					default:
						break;
				}

				return (result);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================