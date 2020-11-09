﻿namespace ModuleManager.ModuleObjects.Interfaces
{
    /// <summary>
    /// Module object interface.
    /// </summary>
    public interface IModuleMemberData : ITreeViewData
    {
        /// <summary>
        /// Gets or sets the description of the module.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the Type.
        /// </summary>
        public string TypeName { get; set; }
    }
}