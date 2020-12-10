namespace ModuleManager.Common.Interfaces
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Used to inject views into the shell view on start up.
    /// </summary>
    public interface IModuleStartUpService
    {
        /// <summary>
        /// Gets a list of actions that perform view injection for a Core module.
        /// </summary>
        List<Action> ViewInjectionActions { get; }

        /// <summary>
        /// Add a new <see cref="Action"/> to the list of View Registration actions.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to be added.</param>
        void AddViewInjectionAction(Action action);
    }
}