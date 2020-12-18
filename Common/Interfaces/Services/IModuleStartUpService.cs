namespace ModuleManager.Common.Interfaces
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements <see cref="IModuleStartUpService"/> to inject views into the shell view on start up.
    /// </summary>
    public interface IModuleStartUpService
    {
        /// <summary>
        /// Gets a list of actions that store views.
        /// </summary>
        List<Action> StoreViewActions { get; }

        /// <summary>
        /// Gets a list of actions that perform view injection.
        /// </summary>
        List<Action> ViewInjectionActions { get; }

        /// <summary>
        /// Add a new <see cref="Action"/> to the list of ViewObject Registration actions.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to be added.</param>
        void AddStoreViewAction(Action action);

        /// <summary>
        /// Add a new <see cref="Action"/> to the list of ViewObject Registration actions.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to be added.</param>
        void AddViewInjectionAction(Action action);
    }
}