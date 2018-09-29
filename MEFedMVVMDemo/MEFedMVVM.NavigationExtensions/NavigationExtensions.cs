using System;
using System.Linq;
using System.Windows;
using MEFedMVVM.NavigationExtensions.Core;
using MEFedMVVM.ViewModelLocator;

namespace MEFedMVVM.NavigationExtensions
{
	public class NavigationExtensions
	{
		#region ChainToElement

		/// <summary>
		/// ChainToElement Attached Dependency Property
		/// </summary>
		public static readonly DependencyProperty ChainToElementProperty =
			DependencyProperty.RegisterAttached("ChainToElement", typeof(DependencyObject), typeof(NavigationExtensions),
				new PropertyMetadata((DependencyObject)null,
					new PropertyChangedCallback(OnChainToElementChanged)));

		/// <summary>
		/// Gets the ChainToElement property. This dependency property 
		/// indicates ....
		/// </summary>
		public static DependencyObject GetChainToElement(DependencyObject d)
		{
			return (DependencyObject)d.GetValue(ChainToElementProperty);
		}

		/// <summary>
		/// Sets the ChainToElement property. This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetChainToElement(DependencyObject d, DependencyObject value)
		{
			d.SetValue(ChainToElementProperty, value);
		}

		/// <summary>
		/// Handles changes to the ChainToElement property.
		/// </summary>
		private static void OnChainToElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DependencyObject oldChainToElement = (DependencyObject)e.OldValue;
			DependencyObject newChainToElement = (DependencyObject)d.GetValue(ChainToElementProperty);

			SetNavigationHandler(d, GetNavigationHandler(newChainToElement));
			var newNavigateTo = (string)d.GetValue(NavigateToProperty);
			RegisterNavigationHandlerToControl(d, newNavigateTo);
		}

		#endregion

		#region NavigationHandler

		/// <summary>
		/// NavigationHandler Attached Dependency Property
		/// </summary>
		public static readonly DependencyProperty NavigationHandlerProperty =
			DependencyProperty.RegisterAttached("NavigationHandler", typeof(ControlNavigationHandler), typeof(NavigationExtensions),
				new PropertyMetadata((ControlNavigationHandler)null, new PropertyChangedCallback(OnNavigationHanderChanged)));

		private static void OnNavigationHanderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var newNavigateTo = (string)d.GetValue(NavigateToProperty);
			RegisterNavigationHandlerToControl(d, newNavigateTo);
		}

		/// <summary>
		/// Gets the NavigationHandler property. This dependency property 
		/// indicates ....
		/// </summary>
		public static ControlNavigationHandler GetNavigationHandler(DependencyObject d)
		{
			return (ControlNavigationHandler)d.GetValue(NavigationHandlerProperty);
		}

		/// <summary>
		/// Sets the NavigationHandler property. This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetNavigationHandler(DependencyObject d, ControlNavigationHandler value)
		{
			d.SetValue(NavigationHandlerProperty, value);
		}

		#endregion

		#region NavigateTo

		/// <summary>
		/// NavigateTo Attached Dependency Property
		/// </summary>
		public static readonly DependencyProperty NavigateToProperty =
			DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavigationExtensions),
				new PropertyMetadata((string)null,
					new PropertyChangedCallback(OnNavigateToChanged)));

		/// <summary>
		/// Gets the NavigateTo property. This dependency property 
		/// indicates ....
		/// </summary>
		public static string GetNavigateTo(DependencyObject d)
		{
			return (string)d.GetValue(NavigateToProperty);
		}

		/// <summary>
		/// Sets the NavigateTo property. This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetNavigateTo(DependencyObject d, string value)
		{
			d.SetValue(NavigateToProperty, value);
		}

		/// <summary>
		/// Handles changes to the NavigateTo property.
		/// </summary>
		private static void OnNavigateToChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var newNavigateTo = (string)d.GetValue(NavigateToProperty);
			RegisterNavigationHandlerToControl(d, newNavigateTo);
		}

		private static void RegisterNavigationHandlerToControl(DependencyObject d, string newNavigateTo)
		{
			var handler = EnsureNavigationHandler(d);
			handler.UnregisterNavigation(d);
			handler.HandleNavigation(d, newNavigateTo);

			OnNavigateOnceLoadedPropertyChanged(d, default(DependencyPropertyChangedEventArgs));
		}

		private static ControlNavigationHandler EnsureNavigationHandler(DependencyObject d)
		{
			ControlNavigationHandler handler = GetNavigationHandler(d);

			if (handler == null)
			{
				var handlers = MEFedContainer.Instance.Resolver.Container.GetExportedValues<ControlNavigationHandler>();
				handler = handlers.FirstOrDefault(x => x.CanSupportControl(d));
				if (handler == null)
					throw new InvalidOperationException(d.GetType().FullName +
					                                    " does not support the NavigateTo. Note: you can create your own by implementing a ControlNavigationHandler and exporting it as type ControlNavigationHandler");

				SetNavigationHandler(d, handler); // store the handler in the control itself
			}

			return handler;
		}

		#endregion

		#region NavigationHost

		/// <summary>
		/// NavigationHost Attached Dependency Property
		/// </summary>
		public static readonly DependencyProperty NavigationHostProperty =
			DependencyProperty.RegisterAttached("NavigationHost", typeof(FrameworkElement), typeof(NavigationExtensions),
				new PropertyMetadata((FrameworkElement)null,
					new PropertyChangedCallback(OnNavigationHostChanged)));

		/// <summary>
		/// Gets the NavigationHost property. This dependency property 
		/// indicates ....
		/// </summary>
		public static FrameworkElement GetNavigationHost(DependencyObject d)
		{
			return (FrameworkElement)d.GetValue(NavigationHostProperty);
		}

		/// <summary>
		/// Sets the NavigationHost property. This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetNavigationHost(DependencyObject d, FrameworkElement value)
		{
			d.SetValue(NavigationHostProperty, value);
		}

		/// <summary>
		/// Handles changes to the NavigationHost property.
		/// </summary>
		private static void OnNavigationHostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var handler = GetNavigationHandler(d);
			if(handler == null)
				return;

			var newNavigationHost = (FrameworkElement)d.GetValue(NavigationHostProperty);
			handler.PrepareHost(newNavigationHost);
		}

		#endregion

		#region NavigationParameter

		/// <summary>
		/// NavigationParameter Attached Dependency Property
		/// </summary>
		public static readonly DependencyProperty NavigationParameterProperty =
			DependencyProperty.RegisterAttached("NavigationParameter", typeof(object), typeof(NavigationExtensions),
				new PropertyMetadata((object)null,
					new PropertyChangedCallback(OnNavigationParameterChanged)));

		/// <summary>
		/// Gets the NavigationParameter property. This dependency property 
		/// indicates ....
		/// </summary>
		public static object GetNavigationParameter(DependencyObject d)
		{
			return (object)d.GetValue(NavigationParameterProperty);
		}

		/// <summary>
		/// Sets the NavigationParameter property. This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetNavigationParameter(DependencyObject d, object value)
		{
			d.SetValue(NavigationParameterProperty, value);
		}

		/// <summary>
		/// Handles changes to the NavigationParameter property.
		/// </summary>
		private static void OnNavigationParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var handler = GetNavigationHandler(d);
			if(handler == null)
				return;

			object newNavigationParameter = (object)d.GetValue(NavigationParameterProperty);

			handler.PrepareNavigationParameter(newNavigationParameter);
		}

		#endregion

		#region NavigateOnceLoaded

		/// <summary>
		/// NavigateOnceLoaded Attached Dependency Property
		/// </summary>
		public static readonly DependencyProperty NavigateOnceLoadedProperty =
			DependencyProperty.RegisterAttached("NavigateOnceLoaded", typeof(bool), typeof(NavigationExtensions),
				new PropertyMetadata((bool)false, new PropertyChangedCallback(OnNavigateOnceLoadedPropertyChanged)));

		private static void OnNavigateOnceLoadedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			string navigateTo = GetNavigateTo(d);
			if (GetNavigateOnceLoaded(d) && !String.IsNullOrEmpty( navigateTo ))
			{
				ControlNavigationHandler handler = GetNavigationHandler(d);
				if(handler == null)
					return;

				var element = (FrameworkElement)d;
				RoutedEventHandler elementOnLoaded = null; // fire this only once
				elementOnLoaded = delegate
				{
					handler.OnEventFired(navigateTo);
					element.Loaded -= elementOnLoaded;
				};
				element.Loaded += elementOnLoaded;
			}
		}

		/// <summary>
		/// Gets the NavigateOnceLoaded property. This dependency property 
		/// indicates ....
		/// </summary>
		public static bool GetNavigateOnceLoaded(DependencyObject d)
		{
			return (bool)d.GetValue(NavigateOnceLoadedProperty);
		}

		/// <summary>
		/// Sets the NavigateOnceLoaded property. This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetNavigateOnceLoaded(DependencyObject d, bool value)
		{
			d.SetValue(NavigateOnceLoadedProperty, value);
		}

		#endregion
	}
}