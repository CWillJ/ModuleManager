namespace ModuleManager.Common.Services
{
    using System;
    using System.Collections.Generic;
    using ModuleManager.Common.Interfaces;

    /// <inheritdoc cref="IModuleStartUpService"/>.
    public class ModuleStartUpService : IModuleStartUpService
    {
        private readonly List<Action> _storeViewActions;
        private readonly List<Action> _viewInjectionAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleStartUpService"/> class.
        /// </summary>
        public ModuleStartUpService()
        {
            _storeViewActions = new List<Action>();
            _viewInjectionAction = new List<Action>();
        }

        /// <inheritdoc/>
        public List<Action> StoreViewActions
        {
            get
            {
                return _storeViewActions;
            }
        }

        /// <inheritdoc/>
        public List<Action> ViewInjectionActions
        {
            get
            {
                return _viewInjectionAction;
            }
        }

        /// <inheritdoc/>
        public void AddStoreViewAction(Action action)
        {
            _storeViewActions.Add(action);
        }

        /// <inheritdoc/>
        public void AddViewInjectionAction(Action action)
        {
            _viewInjectionAction.Add(action);
        }
    }
}