namespace ModuleManager.TestModules.ModuleA
{
    using System;
    using Prism.Ioc;
    using Prism.Modularity;

    public class ModuleAModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            throw new NotImplementedException();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            throw new NotImplementedException();
        }
    }
}
