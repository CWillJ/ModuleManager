namespace ModuleManager.UI.Events
{
    using Prism.Events;

    /// <summary>
    /// Event for updating the assembly name of the progress bar.
    /// </summary>
    public class UpdateProgressBarAssemblyNameEvent : PubSubEvent<string>
    {
    }
}