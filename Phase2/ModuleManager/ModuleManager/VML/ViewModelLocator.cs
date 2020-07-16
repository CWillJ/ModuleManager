namespace ModuleManager.VML
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// A ViewModelLocator class designed to hook up a view model to it's view.
    /// </summary>
    public static class ViewModelLocator
    {
        //// xmlns:vml="clr-namespace:ModuleManager.VML"
        //// vml:ViewModelLocator.AutoHookedUpViewModel="True"

        /// <summary>
        /// Using a DependencyProperty as the backing store for AutoHookedUpViewModel.This
        /// enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty AutoHookedUpViewModelProperty =
            DependencyProperty.RegisterAttached(
                "AutoHookedUpViewModel",
                typeof(bool),
                typeof(ViewModelLocator),
                new PropertyMetadata(false, AutoHookedUpViewModelChanged));

        /// <summary>
        /// Gets view models associated with the view.
        /// </summary>
        /// <param name="obj">DependencyObject.</param>
        /// <returns>Boolean value.</returns>
        public static bool GetAutoHookedUpViewModel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoHookedUpViewModelProperty);
        }

        /// <summary>
        /// Sets the AutoHookedUpViewModel property.
        /// </summary>
        /// <param name="obj">DependencyObject</param>
        /// <param name="value">Boolean value</param>
        public static void SetAutoHookedUpViewModel(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoHookedUpViewModelProperty, value);
        }

        private static void AutoHookedUpViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d))
            {
                return;
            }

            var viewType = d.GetType();
            string str = viewType.FullName;
            str = str.Replace(".Views.", ".ViewModel.");
            var viewTypeName = str;
            var viewModelTypeName = "ModuleManager.ViewModels.ModuleManagerViewModel"; // viewTypeName + "Model";
            var viewModelType = Type.GetType(viewModelTypeName);
            var viewModel = Activator.CreateInstance(viewModelType);
            ((FrameworkElement)d).DataContext = viewModel;
        }
    }
}
