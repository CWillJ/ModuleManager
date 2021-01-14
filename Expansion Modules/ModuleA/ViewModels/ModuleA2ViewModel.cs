namespace ModuleManager.Expansion.ModuleA.ViewModels
{
    using Prism.Mvvm;

    /// <summary>
    /// Module A2 View Model.
    /// </summary>
    public class ModuleA2ViewModel : BindableBase
    {
        private string _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleA2ViewModel"/> class.
        /// </summary>
        public ModuleA2ViewModel()
        {
            _text = @"Module A ViewObject 2";
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