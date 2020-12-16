﻿namespace ModuleManager.TestModules.ModuleB.ViewModels
{
    using ModuleManager.TestModules.ModuleB.Models;
    using Prism.Mvvm;

    /// <summary>
    /// Module B View Model.
    /// </summary>
    public class ModuleBViewModel : BindableBase
    {
        private string _text;
        private string _buttonText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleBViewModel"/> class.
        /// </summary>
        public ModuleBViewModel()
        {
            _text = @"Module B ViewObject";
            _buttonText = @"Click Me";
            ButtonCounter = new CountTracker();

            IncrementCommand = new Prism.Commands.DelegateCommand(IncrementButtonNumber, CanExecute);
        }

        /// <summary>
        /// Gets or sets the text of the view.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        /// <summary>
        /// Gets or sets the text for a button.
        /// </summary>
        public string ButtonText
        {
            get { return _buttonText; }
            set { SetProperty(ref _buttonText, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="CountTracker"/> object.
        /// </summary>
        public CountTracker ButtonCounter { get; set; }

        /// <summary>
        /// Gets or sets the IncrementCommand as a <see cref="Prism.Commands.DelegateCommand"/>.
        /// </summary>
        public Prism.Commands.DelegateCommand IncrementCommand { get; set; }

        /// <summary>
        /// Command method for button.
        /// </summary>
        public void IncrementButtonNumber()
        {
            ButtonCounter.AddOne();
            ButtonText = ButtonCounter.Count.ToString();
        }

        private bool CanExecute()
        {
            return true;
        }
    }
}