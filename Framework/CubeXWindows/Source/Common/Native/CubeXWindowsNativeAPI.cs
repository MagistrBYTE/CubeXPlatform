﻿//=====================================================================================================================
// Проект: CubeXWindows
// Раздел: Общий модуль Windows
// Подраздел: Подсистема нативного доступа
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXWindowsNativeAPI.cs
*		Реализация доступа к нативными методами Windows.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 24.05.2020
//=====================================================================================================================
using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
//=====================================================================================================================
namespace CubeX
{
	namespace Windows
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup WindowsCommonNative Подсистема нативного доступа
		//! \ingroup WindowsCommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс для реализации доступа к нативным методам Windows
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XNative
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			public const Int32 SHGFI_ICON = 0x100;
			public const Int32 SHGFI_SMALLICON = 0x1;
			public const Int32 SHGFI_LARGEICON = 0x0;
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			public static ShellFileInfo ShellFileInfoDefault;
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление графического объекта GDI
			/// </summary>
			/// <param name="h_object">Дескриптор объекта</param>
			/// <returns>Статус операции</returns>
			//---------------------------------------------------------------------------------------------------------
			[DllImport("gdi32.dll")]
			public static extern Boolean DeleteObject(IntPtr h_object);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции курсора
			/// </summary>
			/// <param name="point">Позиция курсора</param>
			/// <returns>Статус операции</returns>
			//---------------------------------------------------------------------------------------------------------
			[DllImport("user32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern Boolean GetCursorPos(ref Win32Point point);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение указателя на дескриптор экрана
			/// </summary>
			/// <returns>Дескриптор экрана</returns>
			//---------------------------------------------------------------------------------------------------------
			[DllImport("user32.dll", SetLastError = false)]
			public static extern IntPtr GetDesktopWindow();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Уничтожение иконки
			/// </summary>
			/// <param name="handle">Дескриптор иконки</param>
			/// <returns>Статус операции</returns>
			//---------------------------------------------------------------------------------------------------------
			[DllImport("user32.dll")]
			public static extern Boolean DestroyIcon(IntPtr handle);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализирует библиотеку COM для дополнительных функциональных возможностей
			/// </summary>
			/// <param name="reserved">Зарезервировано</param>
			/// <returns>Код ошибки</returns>
			[DllImport("ole32.dll")]
			//---------------------------------------------------------------------------------------------------------
			public static extern UInt32 OleInitialize([In] IntPtr reserved);
			#endregion

			#region ======================================= МЕТОДЫ SHELL ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение иконки связанное с данным файлом
			/// </summary>
			/// <param name="path">Путь к файлу</param>
			/// <param name="file_attributes">Атрибуты файла</param>
			/// <param name="shell_file_info">Структура для записи данных</param>
			/// <param name="file_info_size">Размер структры в байтах</param>
			/// <param name="flags">Флаги получения данных</param>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			[DllImport("Shell32.dll")]
			public static extern IntPtr SHGetFileInfo(String path, UInt32 file_attributes, ref ShellFileInfo shell_file_info,
				UInt32 file_info_size, UInt32 flags);
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================