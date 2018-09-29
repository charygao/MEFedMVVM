using System.ComponentModel.Composition;
using System.Windows;
using System;
using System.Diagnostics;


namespace MEFedMVVM.ViewModelLocator
{
    /// <summary>
    /// Locator for ViewModels.
    /// This defines an attached property to import a ViewModel and it will inject all the required services to the ViewModel
    /// </summary>
    public class ViewModelLocator
    {
        #region ViewModel Attached property

        /// <summary>
        /// ViewModel Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.RegisterAttached("ViewModel", typeof(string), typeof(ViewModelLocator),
                new PropertyMetadata((string)String.Empty,
                    new PropertyChangedCallback(OnViewModelChanged)));

        /// <summary>
        /// Gets the ViewModel property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static string GetViewModel(DependencyObject d)
        {
            return (string)d.GetValue(ViewModelProperty);
        }

        /// <summary>
        /// Sets the ViewModel property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetViewModel(DependencyObject d, string value)
        {
            d.SetValue(ViewModelProperty, value);
        }

        /// <summary>
        /// Handles changes to the ViewModel property.
        /// </summary>
        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string vmContractName = (string)e.NewValue;
            var element = d as FrameworkElement;
            AttachViewModel(element, vmContractName, CreationPolicy.Any);
        }

        #endregion

        #region SharedViewModel

        /// <summary>
        /// SharedViewModel Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty SharedViewModelProperty =
            DependencyProperty.RegisterAttached("SharedViewModel", typeof(string), typeof(ViewModelLocator),
                new PropertyMetadata((string)null,
                    new PropertyChangedCallback(OnSharedViewModelChanged)));

        /// <summary>
        /// Gets the SharedViewModel property. This dependency property 
        /// indicates ....
        /// </summary>
        public static string GetSharedViewModel(DependencyObject d)
        {
            return (string)d.GetValue(SharedViewModelProperty);
        }

        /// <summary>
        /// Sets the SharedViewModel property. This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetSharedViewModel(DependencyObject d, string value)
        {
            d.SetValue(SharedViewModelProperty, value);
        }

        /// <summary>
        /// Handles changes to the SharedViewModel property.
        /// </summary>
        private static void OnSharedViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string vmContractName = (string)e.NewValue;
            var element = d as FrameworkElement;
            AttachViewModel(element, vmContractName, CreationPolicy.Shared);
        }

        #endregion

        #region NonSharedViewModel

        /// <summary>
        /// NonSharedViewModel Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty NonSharedViewModelProperty =
            DependencyProperty.RegisterAttached("NonSharedViewModel", typeof(string), typeof(ViewModelLocator),
                new PropertyMetadata((string)null,
                    new PropertyChangedCallback(OnNonSharedViewModelChanged)));

        /// <summary>
        /// Gets the NonSharedViewModel property. This dependency property 
        /// indicates ....
        /// </summary>
        public static string GetNonSharedViewModel(DependencyObject d)
        {
            return (string)d.GetValue(NonSharedViewModelProperty);
        }

        /// <summary>
        /// Sets the NonSharedViewModel property. This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetNonSharedViewModel(DependencyObject d, string value)
        {
            d.SetValue(NonSharedViewModelProperty, value);
        }

        /// <summary>
        /// Handles changes to the NonSharedViewModel property.
        /// </summary>
        private static void OnNonSharedViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string vmContractName = (string)e.NewValue;
            var element = d as FrameworkElement;
            AttachViewModel(element, vmContractName, CreationPolicy.NonShared);
        }

        #endregion


        private static void AttachViewModel(FrameworkElement element, string vmContractName, CreationPolicy policy)
        {
            if (element == null)
                throw new ArgumentException(InvalidElementForAttachedProperty);

            try
            {
                if (!String.IsNullOrEmpty(vmContractName))
                {
                    ViewModelRepository.AttachViewModelToView(vmContractName, element, policy);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error while resolving ViewModel. " + ex);
            }
        }

        #region Exception Strings
        const string InvalidElementForAttachedProperty = "ViewModelLocator attached properties need to be assigned to a FrameworkElement";

        #endregion
    }
}
