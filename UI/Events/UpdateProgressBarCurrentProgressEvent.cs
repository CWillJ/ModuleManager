namespace ModuleManager.UI.Events
{
    using Prism.Events;

    /// <summary>
    /// Event for progress bar current progress.
    /// </summary>
    public class UpdateProgressBarCurrentProgressEvent : PubSubEvent<double>
    {
    }
}