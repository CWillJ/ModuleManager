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
        private readonly Dictionary<string, Action> _loadActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleLoadingService"/> class.
        /// </summary>
        public ModuleLoadingService()
        {
            _storeViewActions = new List<Action>();
            _unloadActions = new Dictionary<string, Action>();
            _loadActions = new Dictionary<string, Action>();
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
        public Dictionary<string, Action> LoadActions
        {
            get
            {
                return _loadActions;
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
            if (!_unloadActions.TryGetValue(moduleName, out Action actionContained))
            {
                _unloadActions.Add(moduleName, action);
            }
        }

        /// <inheritdoc/>
        public void LoadModule(string moduleName, Action action)
        {
            if (!_loadActions.TryGetValue(moduleName, out Action actionContained))
            {
                _loadActions.Add(moduleName, action);
            }
        }
    }
}
