namespace ModuleManager.UI.Events
{
    using System.Collections.ObjectModel;
    using ModuleObjects.Classes;
    using Prism.Events;

    /// <summary>
    /// Event for updating the collection of Module objects.
    /// </summary>
    public class UpdateModuleCollectionEvent : PubSubEvent<ObservableCollection<Module>>
    {
    }
}
