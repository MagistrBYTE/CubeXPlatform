﻿//=====================================================================================================================
// Проект: CubeXWindows
// Раздел: Модуль работы с WPF
// Подраздел: Методы расширения
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXWindowsExtensionGeometry2D.cs
*		Методы расширения для работы с 2D геометрией WPF.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 24.05.2020
//=====================================================================================================================
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
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
		/// Статический класс реализующий методы расширения для типа <see cref="Geometry"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionWindowsGeometry
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции геометрии
			/// </summary>
			/// <param name="geometry">Геометрия</param>
			/// <param name="x">Позиция по X</param>
			/// <param name="y">Позиция по Y</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetPosition(this Geometry geometry, Double x, Double y)
			{
				TransformGroup transform_group = geometry.Transform as TransformGroup;
				if (transform_group != null)
				{
					TranslateTransform translate_in_group = transform_group.Children[0] as TranslateTransform;
					if (translate_in_group != null)
					{
						translate_in_group.X = x;
						translate_in_group.Y = y;
						return;
					}
				}

				TranslateTransform translate = geometry.Transform as TranslateTransform;
				if (translate != null)
				{
					translate.X = x;
					translate.Y = y;
					return;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции геометрии
			/// </summary>
			/// <param name="geometry">Геометрия</param>
			/// <param name="point">Позиция</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetPosition(this Geometry geometry, Point point)
			{
				TransformGroup transform_group = geometry.Transform as TransformGroup;
				if (transform_group != null)
				{
					TranslateTransform translate_in_group = transform_group.Children[0] as TranslateTransform;
					if (translate_in_group != null)
					{
						translate_in_group.X = point.X;
						translate_in_group.Y = point.Y;
						return;
					}
				}

				TranslateTransform translate = geometry.Transform as TranslateTransform;
				if (translate != null)
				{
					translate.X = point.X;
					translate.Y = point.Y;
					return;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка угла поворота геометрии
			/// </summary>
			/// <param name="geometry">Геометрия</param>
			/// <param name="angle">Угол поворота</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetAngle(this Geometry geometry, Double angle)
			{
				TransformGroup transform_group = geometry.Transform as TransformGroup;
				if (transform_group != null)
				{
					RotateTransform rotation_in_group = transform_group.Children[1] as RotateTransform;
					if (rotation_in_group != null)
					{
						Rect bounds_rect = Rect.Empty;
						rotation_in_group.Angle = angle;
						rotation_in_group.CenterX = bounds_rect.Left + bounds_rect.Width / 2;
						rotation_in_group.CenterY = bounds_rect.Top + bounds_rect.Height / 2;
						return;
					}
				}

				RotateTransform rotation = geometry.Transform as RotateTransform;
				if (rotation != null)
				{
					rotation.Angle = angle;
					return;
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================