namespace ModuleManager.Common.Services
{
    using System;
    using System.Collections.Generic;
    using ModuleManager.Common.Interfaces;

    /// <inheritdoc cref="ICoreModuleStartUpService"/>
    public class CoreModuleStartUpService : ICoreModuleStartUpService
    {
        private readonly List<Action> _viewInjectionAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreModuleStartUpService"/> class.
        /// </summary>
        public CoreModuleStartUpService()
        {
            _viewInjectionAction = new List<Action>();
        }

        /// <inheritdoc/>
        public List<Action> ViewInjectionActions
        {
            get { return _viewInjectionAction; }
        }

        /// <inheritdoc/>
        public void AddViewInjectionAction(Action action)
        {
            _viewInjectionAction.Add(action);
        }
    }
}