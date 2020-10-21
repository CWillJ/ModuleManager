namespace ModuleManager.UI.Events
{
    using System.Collections.ObjectModel;
    using ModuleManager.ModuleObjects.Classes;
    using Prism.Events;

    /// <summary>
    /// Event for updating the collection of AssemblyData objects.
    /// </summary>
    public class UpdateAssemblyCollectionEvent : PubSubEvent<ObservableCollection<AssemblyData>>
    {
    }
}