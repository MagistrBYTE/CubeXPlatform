﻿//=====================================================================================================================
// Проект: CubeXWindows
// Раздел: Модуль работы с WPF
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXWindowsIconCommand.cs
*		Определение стандартной команды WPF c дополнительной иконкой.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 24.05.2020
//=====================================================================================================================
using System;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Input;
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
		/// Определение стандартной команды WPF c дополнительной иконкой
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class RoutedIconUICommand : RoutedUICommand, INotifyPropertyChanged
		{
			#region =========================================== СТАТИЧЕСКИЕ ДАННЫЕ ====================================
			private static readonly PropertyChangedEventArgs PropertyArgsMiddleIcon = new PropertyChangedEventArgs(nameof(MiddleIcon));
			private static readonly PropertyChangedEventArgs PropertyArgsLargeIcon = new PropertyChangedEventArgs(nameof(LargeIcon));
			#endregion

			#region =========================================== ДАННЫЕ ================================================
			private BitmapSource mMiddleIcon;
			private BitmapSource mLargeIcon;
			#endregion

			#region =========================================== СВОЙСТВА ==============================================
			/// <summary>
			/// Источник изображения средней иконки
			/// </summary>
			public BitmapSource MiddleIcon
			{
				get { return (mMiddleIcon); }
				set
				{
					mMiddleIcon = value;
					NotifyPropertyChanged(PropertyArgsMiddleIcon);
				}
			}

			/// <summary>
			/// Источник изображения большой иконки
			/// </summary>
			public BitmapSource LargeIcon
			{
				get { return (mLargeIcon); }
				set
				{
					mLargeIcon = value;
					NotifyPropertyChanged(PropertyArgsLargeIcon);
				}
			}
			#endregion

			#region =========================================== КОНСТРУКТОРЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public RoutedIconUICommand()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="text">Описательный текст для команды</param>
			/// <param name="middle_icon">Иконка команды</param>
			//---------------------------------------------------------------------------------------------------------
			public RoutedIconUICommand(String text, BitmapSource middle_icon)
			{
				Text = text;
				mMiddleIcon = middle_icon;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="text">Описательный текст для команды</param>
			/// <param name="middle_icon">Иконка команды</param>
			/// <param name="large_icon">Иконка команды</param>
			//---------------------------------------------------------------------------------------------------------
			public RoutedIconUICommand(String text, BitmapSource middle_icon, BitmapSource large_icon)
			{
				Text = text;
				mMiddleIcon = middle_icon;
				mLargeIcon = large_icon;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="text">Описательный текст для команды</param>
			/// <param name="name">Объявленное имя команды для сериализации</param>
			/// <param name="middle_icon">Иконка команды</param>
			//---------------------------------------------------------------------------------------------------------
			public RoutedIconUICommand(String text, String name, BitmapSource middle_icon)
				: base(text, name, typeof(Window))
			{
				mMiddleIcon = middle_icon;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="text">Описательный текст для команды</param>
			/// <param name="name">Объявленное имя команды для сериализации</param>
			/// <param name="middle_icon">Иконка команды</param>
			/// <param name="large_icon">Иконка команды</param>
			//---------------------------------------------------------------------------------------------------------
			public RoutedIconUICommand(String text, String name, BitmapSource middle_icon, BitmapSource large_icon)
				: base(text, name, typeof(Window))
			{
				mMiddleIcon = middle_icon;
				mLargeIcon = large_icon;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="text">Описательный текст для команды</param>
			/// <param name="name">Объявленное имя команды для сериализации</param>
			/// <param name="owner_type">Тип, регистрирующий команду</param>
			//---------------------------------------------------------------------------------------------------------
			public RoutedIconUICommand(String text, String name, Type owner_type)
				: base(text, name, owner_type)

			{

			}
			#endregion

			#region =========================================== ДАННЫЕ INotifyPropertyChanged =========================
			/// <summary>
			/// Событие срабатывает ПОСЛЕ изменения свойства
			/// </summary>
			public event PropertyChangedEventHandler PropertyChanged;

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вспомогательный метод для нотификации изменений свойства
			/// </summary>
			/// <param name="property_name">Имя свойства</param>
			//---------------------------------------------------------------------------------------------------------
			public void NotifyPropertyChanged(String property_name = "")
			{
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs(property_name));
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вспомогательный метод для нотификации изменений свойства
			/// </summary>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			public void NotifyPropertyChanged(PropertyChangedEventArgs args)
			{
				if (PropertyChanged != null)
				{
					PropertyChanged(this, args);
				}
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================