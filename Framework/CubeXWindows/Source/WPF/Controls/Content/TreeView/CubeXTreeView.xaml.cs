//=====================================================================================================================
// Проект: CubeXWindows
// Раздел: Модуль работы с WPF
// Подраздел: Элементы интерфейса
// Группа: Элементы для отображения контента
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTreeView.xaml.cs
*		Дерево для отображения иерархической информации с поддержкой иерархических моделей данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 24.05.2020
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Effects;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Windows
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup WindowsWPFControlsContent Элементы для отображения контента
		//! \ingroup WindowsWPFControls
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Дерево для отображения иерархической информации с поддержкой иерархических моделей данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public partial class CubeXTreeView : TreeView, ICubeXUIExploreModel, INotifyPropertyChanged
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			private static readonly PropertyChangedEventArgs PropertyArgsSelectedCollection = new PropertyChangedEventArgs(nameof(SelectedCollection));
			private static readonly PropertyChangedEventArgs PropertyArgsPresentedCollection = new PropertyChangedEventArgs(nameof(PresentedCollection));
			private static readonly PropertyChangedEventArgs PropertyArgsSelectedModel = new PropertyChangedEventArgs(nameof(SelectedModel));
			private static readonly PropertyChangedEventArgs PropertyArgsIsNotifySelectedInspector = new PropertyChangedEventArgs(nameof(IsNotifySelectedInspector));
			private static readonly PropertyChangedEventArgs PropertyArgsIsGroupHierarchy = new PropertyChangedEventArgs(nameof(IsGroupHierarchy));
			private static readonly PropertyChangedEventArgs PropertyArgsIsDragging = new PropertyChangedEventArgs(nameof(IsDragging));
			#endregion

			#region ======================================= ОПРЕДЕЛЕНИЕ СВОЙСТВ ЗАВИСИМОСТИ ===========================
			/// <summary>
			/// Перетаскиваемая модель
			/// </summary>
			public static readonly DependencyProperty DraggedModelProperty = DependencyProperty.Register(nameof(DraggedModel),
				typeof(ICubeXModel), typeof(CubeXTreeView),	new FrameworkPropertyMetadata(null));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			private ICubeXCollectionModel mSelectedCollection;
			private ICubeXCollectionModelView mPresentedCollection;
			private ICubeXModelOwned mSelectedModel;
			private Boolean mIsNotifySelectedInspector;
			private Boolean mIsGroupHierarchy;

			// Элементы представления
			private FlowDocumentScrollViewer mTextPresenter;
			private DataGrid mTablePresenter;

			// Модель перетаскивания
			private Boolean mIsDragging;
			private Point mDragLastMouseDown;
			private Popup mPopupHand;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Выбранная коллекция модели
			/// </summary>
			public ICubeXCollectionModel SelectedCollection
			{
				get { return (mSelectedCollection); }
				set
				{
					mSelectedCollection = value;
					NotifyPropertyChanged(PropertyArgsSelectedCollection);
				}
			}

			/// <summary>
			/// Отображаемая коллекция модели которая поддерживает концепцию просмотра и управления
			/// </summary>
			public ICubeXCollectionModelView PresentedCollection
			{
				get { return (mPresentedCollection); }
				set
				{
					mPresentedCollection = value;
					NotifyPropertyChanged(PropertyArgsPresentedCollection);
				}
			}

			/// <summary>
			/// Выбранная модель
			/// </summary>
			public ICubeXModelOwned SelectedModel
			{
				get { return (mSelectedModel); }
				set
				{
					mSelectedModel = value;
					NotifyPropertyChanged(PropertyArgsSelectedModel);
				}
			}

			/// <summary>
			/// Информирование инспектора свойств о смене выбранного элемента
			/// </summary>
			public Boolean IsNotifySelectedInspector
			{
				get { return (mIsNotifySelectedInspector); }
				set
				{
					mIsNotifySelectedInspector = value;
					NotifyPropertyChanged(PropertyArgsIsNotifySelectedInspector);
				}
			}

			/// <summary>
			/// Статус иерархического группирования
			/// </summary>
			public Boolean IsGroupHierarchy
			{
				get { return (mIsGroupHierarchy); }
				set
				{
					mIsGroupHierarchy = value;
					NotifyPropertyChanged(PropertyArgsIsGroupHierarchy);
				}
			}

			//
			// ЭЛЕМЕНТЫ ПРЕДСТАВЛЕНИЯ
			//
			/// <summary>
			/// Элемент для представления текстовых данных
			/// </summary>
			public FlowDocumentScrollViewer TextPresenter
			{
				get { return (mTextPresenter); }
				set
				{
					mTextPresenter = value;
				}
			}

			/// <summary>
			/// Элемент для представления табличных данных
			/// </summary>
			public DataGrid TablePresenter
			{
				get { return (mTablePresenter); }
				set
				{
					mTablePresenter = value;
				}
			}

			//
			// МОДЕЛЬ ПЕРЕТАСКИВАНИЯ
			//
			/// <summary>
			/// Статус перетаскивания модели
			/// </summary>
			public Boolean IsDragging
			{
				get { return (mIsDragging); }
				set
				{
					mIsDragging = value;
					NotifyPropertyChanged(PropertyArgsIsDragging);
				}
			}

			/// <summary>
			/// Перетаскиваемая модель
			/// </summary>
			public ICubeXModel DraggedModel
			{
				get { return (ICubeXModel)GetValue(DraggedModelProperty); }
				set { SetValue(DraggedModelProperty, value); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CubeXTreeView()
			{
				InitializeComponent();
				SetResourceReference(StyleProperty, typeof(TreeView));
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка статуса представления коллекции
			/// </summary>
			/// <param name="status">Статус представления коллекции</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetStatusPresentedCollection(Boolean status)
			{
				if(mPresentedCollection is ICubeXModelView)
				{
					ICubeXModelView model_view = mPresentedCollection as ICubeXModelView;
					model_view.IsPresented = status;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на иерархическую модель
			/// </summary>
			/// <param name="value">Объект</param>
			/// <returns>Статус поддержки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean CheckModelHierarchy(System.Object value)
			{
				if (value is ICubeXModelHierarchy)
				{
					ICubeXModelHierarchy model_hierarchy = value as ICubeXModelHierarchy;

					// Если иерархия закрывается на этом элементе
					if (model_hierarchy.IModels == null)
					{
						SelectedModel = model_hierarchy;
						SelectedCollection = model_hierarchy.IOwner;
					}
					else
					{
						SelectedModel = model_hierarchy;
						SelectedCollection = model_hierarchy;
					}

					return (true);
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на коллекцию моделей
			/// </summary>
			/// <param name="value">Объект</param>
			/// <returns>Статус поддержки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean CheckCollectionModel(System.Object value)
			{
				if (value is ICubeXCollectionModel)
				{
					ICubeXCollectionModel сollection_model = value as ICubeXCollectionModel;

					SelectedModel = null;
					SelectedCollection = сollection_model;

					return (true);
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Бросание модели над коллекцией моделей
			/// </summary>
			/// <param name="model">Модель</param>
			/// <param name="item">Коллекция</param>
			/// <param name="args">Аргументы события</param>
			/// <returns>Статус обработки события</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean DropModelOverCollectionModel(ICubeXModel model, TreeViewItem item, DragEventArgs args)
			{
				ICubeXCollectionModel collection_model = item.DataContext as ICubeXCollectionModel;

				// Если коллекция поддерживает данную модель
				if (collection_model != null && collection_model.IsSupportModel(model as ICubeXModelOwned))
				{
					// Проверка на поддержку операции добавления
					ICubeXCollectionSupportAdd operation_add = item.DataContext as ICubeXCollectionSupportAdd;
					if (operation_add != null)
					{
						if (args.Effects == DragDropEffects.Move)
						{
							ICubeXModelOwned model_owned = model as ICubeXModelOwned;

							SetGroupProperties(model_owned, operation_add.IModels[0] as ICubeXModelOwned);

							operation_add.AddExistingModel(model_owned);
						}
						else
						{
							if (args.Effects == DragDropEffects.Copy)
							{
								ICubeXModelOwned model_owned = model.Clone() as ICubeXModelOwned;

								SetGroupProperties(model_owned, operation_add.IModels[0] as ICubeXModelOwned);

								operation_add.AddExistingModel(model_owned);
							}
						}
					}

					return (true);
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Бросание модели над другой моделью
			/// </summary>
			/// <param name="model">Модель</param>
			/// <param name="item">Модель</param>
			/// <param name="args">Аргументы события</param>
			/// <returns>Статус обработки события</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean DropModelOverModel(ICubeXModel model, TreeViewItem item, DragEventArgs args)
			{
				// Элемент находится над моделей
				ICubeXModelOwned model_over = item.DataContext as ICubeXModelOwned;
				if (model_over != null)
				{
					// Сначала проверяем возможность вставки в конкретное место
					ICubeXCollectionSupportInsert operation_insert = model_over.IOwner as ICubeXCollectionSupportInsert;
					if (operation_insert != null && operation_insert.IsSupportModel(model as ICubeXModelOwned))
					{
						if (args.Effects == DragDropEffects.Move)
						{
							ICubeXModelOwned model_owned = model as ICubeXModelOwned;

							SetGroupProperties(model_owned, model_over);

							operation_insert.InsertAfterModel(model_over, model_owned);
						}
						else
						{
							if (args.Effects == DragDropEffects.Copy)
							{
								ICubeXModelOwned model_owned = model.Clone() as ICubeXModelOwned;

								SetGroupProperties(model_owned, model_over);

								operation_insert.InsertAfterModel(model_over, model_owned);
							}
						}
					}
					else
					{
						// Проверяем возможность простого добавления
						ICubeXCollectionSupportAdd operation_add = model_over.IOwner as ICubeXCollectionSupportAdd;
						if (operation_add != null && operation_add.IsSupportModel(model as ICubeXModelOwned))
						{
							if (args.Effects == DragDropEffects.Move)
							{
								ICubeXModelOwned model_owned = model as ICubeXModelOwned;

								SetGroupProperties(model_owned, model_over);

								operation_add.AddExistingModel(model_owned);
							}
							else
							{
								if (args.Effects == DragDropEffects.Copy)
								{
									ICubeXModelOwned model_owned = model.Clone() as ICubeXModelOwned;

									SetGroupProperties(model_owned, model_over);

									operation_add.AddExistingModel(model_owned);
								}
							}
						}
					}
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка свойств по группированию
			/// </summary>
			/// <param name="model">Модель</param>
			/// <param name="over">Другой элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetGroupProperties(ICubeXModel model, ICubeXModel over)
			{
				if (mIsGroupHierarchy)
				{
					if (over != null)
					{
						if (over is ICubeXModelOwned)
						{
							if (model is ICubeXModelOwned)
							{
								ICubeXModelOwned model_owned = model as ICubeXModelOwned;
								ICubeXModelOwned over_owned = over as ICubeXModelOwned;
								model_owned.SetPropertyValueFromGroupingHierarchy(over_owned.GetGroupingFromHierarchyOfOwner());
							}
						}
					}
					else
					{

					}
				}
			}
			#endregion

			#region ======================================= ОБРАБОТЧИКИ СОБЫТИЙ TreeView ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка компонента
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnTreeView_Loaded(Object sender, RoutedEventArgs args)
			{
				mPopupHand = this.Resources["popupHandKey"] as Popup;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выбор элемента
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnTreeView_SelectedItemChanged(Object sender, RoutedPropertyChangedEventArgs<Object> args)
			{
				// Проверка на иерархическую модель
				if (CheckModelHierarchy(args.NewValue))
				{
				}
				else
				{
					// Проверка на коллекцию моделей
					if (CheckCollectionModel(args.NewValue))
					{
					}
					else
					{
						if (args.NewValue is ICubeXModelOwned)
						{
							SelectedModel = args.NewValue as ICubeXModelOwned;
							SelectedCollection = SelectedModel.IOwner;
						}
					}
				}


				// Выключаем выбор для предыдущего элемента
				if (args.OldValue != null && args.OldValue is ICubeXModelView)
				{
					ICubeXModelView model_view = args.OldValue as ICubeXModelView;
					if (model_view != null)
					{
						model_view.IsSelected = false;
					}
				}

				// Включем выбор для нового элемента
				if (args.NewValue != null && args.NewValue is ICubeXModelView)
				{
					ICubeXModelView model_view = args.NewValue as ICubeXModelView;
					if (model_view != null)
					{
						model_view.IsSelected = true;
					}
				}

				if (IsNotifySelectedInspector && XWindowManager.PropertyInspector != null)
				{
					XWindowManager.PropertyInspector.SelectedObject = args.NewValue;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Щелчок левой кнопки
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnTreeView_PreviewMouseLeftButtonDown(Object sender, MouseButtonEventArgs args)
			{
				if (treeExplorer.IsMouseOver && AllowDrop)
				{
					mDragLastMouseDown = args.GetPosition(treeExplorer);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отпускания левой кнопки мыши
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnTreeView_PreviewMouseLeftButtonUp(Object sender, MouseButtonEventArgs args)
			{
				IsDragging = false;
				DraggedModel = null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение курсора мыши
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnTreeView_PreviewMouseMove(Object sender, MouseEventArgs args)
			{
				// Получаем позицию курсора
				Point mouse_pos = args.GetPosition(treeExplorer);
				Vector diff = mDragLastMouseDown - mouse_pos;

				// Проверяем смещение
				if (AllowDrop && args.LeftButton == MouseButtonState.Pressed &&
					(Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
					Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
				{
					// Получаем визуальный элемент
					TreeViewItem drag_tree_view_item = ((DependencyObject)args.OriginalSource).FindVisualParent<TreeViewItem>();

					if (drag_tree_view_item == null)
					{
						return;
					}

					IsDragging = true;

					// Получаем данные
					ICubeXModel model = drag_tree_view_item.DataContext as ICubeXModel;
					DraggedModel = model;

					// Если модель поддерживает
					if (model is ICubeXUIModelSupportExplore)
					{
						ICubeXUIModelSupportExplore model_support_explore = model as ICubeXUIModelSupportExplore;
						if (model_support_explore.IsDraggableModel == false)
						{
							return;
						}
					}

					var drag_data = new DataObject(nameof(ICubeXModel), model);

					if (Keyboard.IsKeyDown(Key.LeftCtrl))
					{
						// Копируем
						DragDrop.DoDragDrop(drag_tree_view_item, drag_data, DragDropEffects.Copy);
					}
					else
					{
						// Переносим
						DragDrop.DoDragDrop(drag_tree_view_item, drag_data, DragDropEffects.Move);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка типа перетаскиваемых данных и определение типа разрешаемой операции
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnTreeView_DragEnter(Object sender, DragEventArgs args)
			{
				// Если перетаскиваемая объект не содержит модель или тому над которым происходит перетаскивание 
				if (!args.Data.GetDataPresent(nameof(ICubeXModel)) || sender == args.Source)
				{
					args.Effects = DragDropEffects.None;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Событие постоянно возникает при перетаскивании данных над объектом-приемником
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnTreeView_DragOver(Object sender, DragEventArgs args)
			{
				if (args.Data.GetDataPresent(nameof(ICubeXModel)))
				{
					ICubeXModel model = args.Data.GetData(nameof(ICubeXModel)) as ICubeXModel;

					// Над этим элементом находится перетаскиваемый объект
					TreeViewItem over_item = ((DependencyObject)args.OriginalSource).FindVisualParent<TreeViewItem>();
					if (over_item != null)
					{
						ICubeXCollectionModel collection_model = over_item.DataContext as ICubeXCollectionModel;
						if (collection_model != null && collection_model.IsSupportModel(model as ICubeXModelOwned))
						{

						}
						else
						{
							ICubeXModelOwned model_owned = over_item.DataContext as ICubeXModelOwned;
							if (model_owned != null)
							{
								ICubeXCollectionModel model_owned_collection = model_owned.IOwner;
								if (model_owned_collection != null && model_owned_collection.IsSupportModel(model as ICubeXModelOwned))
								{

								}
								else
								{
									args.Effects = DragDropEffects.None;
								}
							}
							else
							{
								args.Effects = DragDropEffects.None;
							}
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Событие постоянно возникает при покидании объекта-приемника
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnTreeView_DragLeave(Object sender, DragEventArgs args)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Постоянная информация о перетаскивания
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnTreeView_GiveFeedback(Object sender, GiveFeedbackEventArgs args)
			{
				Size popup_size = new Size(mPopupHand.ActualWidth, mPopupHand.ActualHeight);

				Win32Point mouse_pos = new Win32Point();
				XNative.GetCursorPos(ref mouse_pos);
				Point cursopr_pos = new Point(mouse_pos.X, mouse_pos.Y);

				mPopupHand.PlacementRectangle = new Rect(cursopr_pos, popup_size);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Событие возникает, когда данные сбрасываются над объектом-приемником; по умолчанию это происходит 
			/// при отпускании кнопки мыши
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnTreeView_Drop(Object sender, DragEventArgs args)
			{
				if (args.Data.GetDataPresent(nameof(ICubeXModel)))
				{
					ICubeXModel model = args.Data.GetData(nameof(ICubeXModel)) as ICubeXModel;

					// Над этим элементом находится перетаскиваемый объект
					TreeViewItem over_item = ((DependencyObject)args.OriginalSource).FindVisualParent<TreeViewItem>();
					if (over_item != null)
					{
						if (DropModelOverCollectionModel(model, over_item, args))
						{

						}
						else
						{
							if (DropModelOverModel(model, over_item, args))
							{

							}
							else
							{

							}
						}
					}
				}
			}
			#endregion

			#region ======================================= ОБРАБОТЧИКИ СОБЫТИЙ TreeViewItem ==========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Формирование контекстного меню у элемента
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnTreeViewItem_ContextMenuOpening(Object sender, ContextMenuEventArgs args)
			{
				TreeViewItem item_sender = ((DependencyObject)sender).FindVisualParent<TreeViewItem>();
				TreeViewItem item_source = ((DependencyObject)args.OriginalSource).FindVisualParent<TreeViewItem>();
				if (item_sender != null && item_source != null)
				{
					// Делаем событие обработанным чтобы оно не поднималось вверх
					// В случае необходимости меню открываем вручную
					args.Handled = true;

					// Только если оригинальный источник и текущий совпадает
					// В иных случая это означает что событие обрабатывается иным элементом
					if (item_sender == item_source)
					{
						ICubeXUIModelContextMenu support_contex_menu = item_source.DataContext as ICubeXUIModelContextMenu;
						if (support_contex_menu != null)
						{
							ContextMenu context_menu = item_source.ContextMenu;
							if (context_menu == null)
							{
								context_menu = new ContextMenu();
								item_source.ContextMenu = context_menu;
							}

							support_contex_menu.OpenContextMenu(context_menu);
							context_menu.IsOpen = true;
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Двойной щелчок по элементу
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnTreeViewItem_MouseDoubleClick(Object sender, MouseButtonEventArgs args)
			{
				TreeViewItem item = ((DependencyObject)args.OriginalSource).FindVisualParent<TreeViewItem>();
				if(item != null && item.DataContext != null && item.DataContext is ICubeXCollectionModelView)
				{
					ICubeXCollectionModelView current_presented_view = item.DataContext as ICubeXCollectionModelView;

					// Если был уже выбран
					if (mPresentedCollection != null)
					{
						// И не совпадает
						if(mPresentedCollection != current_presented_view)
						{
							SetStatusPresentedCollection(false);
							PresentedCollection = current_presented_view;
							SetStatusPresentedCollection(true);
						}
						else
						{
							// Если совпадают то нет
							SetStatusPresentedCollection(false);
							PresentedCollection = null;
						}
					}
					else
					{
						PresentedCollection = current_presented_view;
						SetStatusPresentedCollection(true);
					}

					// Отображаем
					if (PresentedCollection is ICubeXPresentEntity)
					{
						ICubeXPresentEntity present_entity = PresentedCollection as ICubeXPresentEntity;
						switch (present_entity.PresentType)
						{
							case TPresentType.Text:
								{
									if (TextPresenter != null)
									{
										present_entity.LoadPresentData(TextPresenter);
									}
								}
								break;
							case TPresentType.Table:
								{
									if(TablePresenter != null)
									{
										present_entity.LoadPresentData(TablePresenter);
									}
								}
								break;
							case TPresentType.Image:
								break;
							case TPresentType.Map:
								break;
							case TPresentType.Special:
								break;
							default:
								break;
						}
					}
				}
			}
			#endregion

			#region ======================================= ДАННЫЕ INotifyPropertyChanged =============================
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
