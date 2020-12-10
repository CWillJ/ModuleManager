namespace ModuleManager.Common.Services
{
    using System;
    using System.Collections.Generic;
    using ModuleManager.Common.Interfaces;

    /// <summary>
    /// Implements <see cref="IModuleStartUpService"/> to inject views into the shell view on start up.
    /// </summary>
    public class ModuleStartUpService : IModuleStartUpService
    {
        private readonly List<Action> _viewInjectionActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleStartUpService"/> class.
        /// </summary>
        public ModuleStartUpService()
        {
            _viewInjectionActions = new List<Action>();
        }

        /// <inheritdoc/>
        public List<Action> ViewInjectionActions
        {
            get
            {
                return _viewInjectionActions;
            }
        }

        /// <inheritdoc/>
        public void AddViewInjectionAction(Action action)
        {
            _viewInjectionActions.Add(action);
        }
    }
}
