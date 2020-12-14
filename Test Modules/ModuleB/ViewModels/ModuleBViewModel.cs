namespace ModuleManager.TestModules.ModuleB.ViewModels
{
    using Prism.Mvvm;

    public class ModuleBViewModel : BindableBase
    {
        private string _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleBViewModel"/> class.
        /// </summary>
        public ModuleBViewModel()
        {
            _text = @"Module B View";
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