namespace ModuleManager.Common.Classes.Data
{
    using System;

    /// <summary>
    /// Stores a view object and ways to access that view.
    /// </summary>
    public class ViewData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewData"/> class.
        /// </summary>
        public ViewData()
            : this(new object(), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewData"/> class.
        /// </summary>
        /// <param name="viewObject">A view <see cref="object"/>.</param>
        /// <param name="viewType">The <see cref="Type"/> of the view.</param>
        public ViewData(object viewObject, Type viewType)
        {
            ViewObject = viewObject;
            ViewType = viewType;
        }

        /// <summary>
        /// Gets the <see cref="object"/> of view objects.
        /// </summary>
        public object ViewObject { get; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> of the assembly name.
        /// </summary>
        public Type ViewType { get; set; }
    }
}
