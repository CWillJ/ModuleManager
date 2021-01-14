namespace ModuleManager.Expansion.ModuleA.ViewModels
{
    using Prism.Mvvm;

    /// <summary>
    /// Module A ViewObject Model.
    /// </summary>
    public class ModuleAViewModel : BindableBase
    {
        private string _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleAViewModel"/> class.
        /// </summary>
        public ModuleAViewModel()
        {
            _text = @"Module A ViewObject 1";
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
