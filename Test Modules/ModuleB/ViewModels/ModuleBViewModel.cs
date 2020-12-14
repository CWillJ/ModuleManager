namespace ModuleManager.TestModules.ModuleB.ViewModels
{
    using Prism.Mvvm;

    /// <summary>
    /// Module B View Model.
    /// </summary>
    public class ModuleBViewModel : BindableBase
    {
        private string _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleBViewModel"/> class.
        /// </summary>
        public ModuleBViewModel()
        {
            _text = @"Module B ViewObject";
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