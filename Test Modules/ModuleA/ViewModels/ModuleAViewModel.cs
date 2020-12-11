namespace ModuleManager.ModuleA.ViewModels
{
    using Prism.Mvvm;

    /// <summary>
    /// Module A View Model.
    /// </summary>
    public class ModuleAViewModel : BindableBase
    {
        private string _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleAViewModel"/> class.
        /// </summary>
        public ModuleAViewModel()
        {
            _text = @"Module A View";
        }

        /// <summary>
        /// Gets or sets the text of the view.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
    }
}