namespace ModuleManager.Core.UI.TemplateSelectors
{
    using System.Windows;
    using System.Windows.Controls;
    using ModuleManager.Common.Classes;

    /// <summary>
    /// Used to selected a DataTemplate in the description.
    /// </summary>
    public class DescriptionTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Used to select the DataTemplate for the description.
        /// </summary>
        /// <param name="item">A <see cref="object"/> sent in.</param>
        /// <param name="container">The <see cref="DependencyObject"/>.</param>
        /// <returns>A <see cref="DataTemplate"/> depending on the object passed in.</returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element && container != null && item != null)
            {
                if (item is AssemblyData)
                {
                    return element.FindResource(@"assemblyTemplate") as DataTemplate;
                }

                if (item is TypeData) //// typeData)
                {
                    ////if (typeData.Type.BaseType != null && typeData.Type.BaseType.Name == @"UserControl")
                    ////{
                    ////    return element.FindResource(@"viewTemplate") as DataTemplate;
                    ////}
                    ////else
                    ////{
                        return element.FindResource(@"typeTemplate") as DataTemplate;
                    ////}
                }

                if (item is TypeConstructor)
                {
                    return element.FindResource(@"typeConstructorTemplate") as DataTemplate;
                }

                if (item is TypeProperty)
                {
                    return element.FindResource(@"typePropertyTemplate") as DataTemplate;
                }

                if (item is TypeMethod)
                {
                    return element.FindResource(@"typeMethodTemplate") as DataTemplate;
                }

                if (item is MemberParameter)
                {
                    return element.FindResource(@"memberParameterTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}