namespace ModuleManager.Common.Services
{
    using System;
    using System.Collections.Generic;
    using ModuleManager.Common.Interfaces;

    /// <inheritdoc cref="IModuleLoadingService"/>.
    public class ModuleLoadingService : IModuleLoadingService
    {
        private readonly List<Action> _storeViewActions;
        private readonly Dictionary<string, Action> _unloadActions;
        private readonly Dictionary<string, Action> _reloadActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleLoadingService"/> class.
        /// </summary>
        public ModuleLoadingService()
        {
            _storeViewActions = new List<Action>();
            _unloadActions = new Dictionary<string, Action>();
            _reloadActions = new Dictionary<string, Action>();
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
        public Dictionary<string, Action> UnloadActions
        {
            get
            {
                return _unloadActions;
            }
        }

        /// <inheritdoc/>
        public Dictionary<string, Action> ReloadActions
        {
            get
            {
                return _reloadActions;
            }
        }

        /// <inheritdoc/>
        public void AddStoreViewAction(Action action)
        {
            _storeViewActions.Add(action);
        }

        /// <inheritdoc/>
        public void UnloadModule(string moduleName, Action action)
        {
            _unloadActions.Add(moduleName, action);
        }

        /// <inheritdoc/>
        public void ReloadModule(string moduleName, Action action)
        {
            _reloadActions.Add(moduleName, action);
        }
    }
}