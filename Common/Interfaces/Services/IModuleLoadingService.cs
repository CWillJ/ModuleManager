namespace ModuleManager.Common.Interfaces
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements <see cref="IModuleLoadingService"/> to inject views into the shell view on start up.
    /// </summary>
    public interface IModuleLoadingService
    {
        /// <summary>
        /// Gets a list of actions that store views.
        /// </summary>
        List<Action> StoreViewActions { get; }

        /// <summary>
        /// Gets a list of unloading actions.
        /// </summary>
        Dictionary<string, Action> UnloadActions { get; }

        /// <summary>
        /// Gets a list of loading actions.
        /// </summary>
        Dictionary<string, Action> LoadActions { get; }

        /// <summary>
        /// Add a new <see cref="Action"/> to the list of ViewObject Registration actions.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to be added.</param>
        void AddStoreViewAction(Action action);

        /// <summary>
        /// Add a new <see cref="Action"/> to the list of ViewObject unloading actions.
        /// </summary>
        /// <param name="moduleName">The <see cref="string"/> name of the module.</param>
        /// <param name="action">The <see cref="Dictionary{String, Action}"/> to be added.</param>
        void UnloadModule(string moduleName, Action action);

        /// <summary>
        /// Add a new <see cref="Dictionary{String, Action}"/> to the list of ViewObject loading actions.
        /// </summary>
        /// <param name="moduleName">The <see cref="string"/> name of the module.</param>
        /// <param name="action">The <see cref="Action"/> to be added.</param>
        void LoadModule(string moduleName, Action action);
    }
}
