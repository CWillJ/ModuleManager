namespace ModuleManager.UI.Events
{
    using Prism.Events;

    /// <summary>
    /// Event that updates the progress bar text.
    /// </summary>
    public class UpdateProgressBarTextEvent : PubSubEvent<string>
    {
    }
}