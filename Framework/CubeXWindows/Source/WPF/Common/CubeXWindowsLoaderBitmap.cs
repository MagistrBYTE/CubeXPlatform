//=====================================================================================================================
// Проект: CubeXWindows
// Раздел: Модуль работы с WPF
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXWindowsLoaderBitmap.cs
*		Статический класс для реализации методов загрузки BitmapSource из различных источников.
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
		/// Статический класс для реализации методов загрузки <see cref="BitmapSource"/> из различных источников
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XWindowsLoaderBitmap
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка изображения из ресурсов сборки
			/// </summary>
			/// <param name="resource_name">Имя ресурса</param>
			/// <returns>Изображение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static BitmapSource LoadBitmapFromResource(String resource_name)
			{
				//Object image = Properties.XResources.Instance.GetObject(resource_name);
				Object image = Properties.Resources.ResourceManager.GetObject(resource_name);
				System.Drawing.Bitmap source = (System.Drawing.Bitmap)image;

				if (source != null)
				{

					var h_bitmap = source.GetHbitmap();
					var result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(h_bitmap, IntPtr.Zero,
						Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

					XNative.DeleteObject(h_bitmap);

					return (result);
				}

				return (null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка изображения из ресурсов сборки
			/// </summary>
			/// <param name="resource_manager">Менеджер ресурсов</param>
			/// <param name="resource_name">Имя ресурса</param>
			/// <returns>Изображение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static BitmapSource LoadBitmapFromResource(System.Resources.ResourceManager resource_manager, String resource_name)
			{
				Object image = resource_manager.GetObject(resource_name);
				System.Drawing.Bitmap source = (System.Drawing.Bitmap)image;

				if (source != null)
				{
					var h_bitmap = source.GetHbitmap();
					var result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(h_bitmap, IntPtr.Zero,
						Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

					XNative.DeleteObject(h_bitmap);

					return (result);
				}

				return (null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка изображения из файла
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			/// <returns>Изображение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static BitmapSource LoadBitmapFromFile(String file_name)
			{
				FileStream file_stream = new FileStream(file_name, FileMode.Open, FileAccess.Read);

				BitmapImage bitmap = new BitmapImage(new Uri(file_name));
				//bitmap.BeginInit();
				//bitmap.StreamSource = file_stream;
				//bitmap.EndInit();

				file_stream.Close();

				return (bitmap);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение иконки файла связанного с типом файла
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			/// <returns>Изображение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static BitmapSource GetIconFromFileTypeFromShell(String file_name)
			{
				IntPtr icon_small = XNative.SHGetFileInfo(file_name, 0, ref XNative.ShellFileInfoDefault,
					(UInt32)Marshal.SizeOf(XNative.ShellFileInfoDefault), XNative.SHGFI_ICON | XNative.SHGFI_LARGEICON);

				//The icon is returned in the hIcon member of the shinfo struct
				System.Drawing.Icon icon = System.Drawing.Icon.FromHandle(XNative.ShellFileInfoDefault.IconHandle);

				var h_bitmap = icon.ToBitmap().GetHbitmap();
				var result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(h_bitmap, IntPtr.Zero,
					Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

				XNative.DeleteObject(h_bitmap);

				return (result);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение иконки файла связанного с типом файла
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			/// <returns>Изображение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static BitmapSource GetIconFromFileTypeFromExtract(String file_name)
			{
				if (Path.HasExtension(file_name))
				{
					var sysicon = System.Drawing.Icon.ExtractAssociatedIcon(file_name);
					var bmp_src = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
								sysicon.Handle,
								Int32Rect.Empty,
								BitmapSizeOptions.FromEmptyOptions());
					sysicon.Dispose();

					return (bmp_src);
				}
				else
				{
					return (GetIconFromFileTypeFromShell(file_name));
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================