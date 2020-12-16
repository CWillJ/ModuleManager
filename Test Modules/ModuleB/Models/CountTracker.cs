namespace ModuleManager.TestModules.ModuleB.Models
{
    /// <summary>
    /// Keeps track of an integer.
    /// </summary>
    public class CountTracker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountTracker"/> class.
        /// </summary>
        public CountTracker()
        {
            Count = 0;
        }

        /// <summary>
        /// Gets or sets an <see cref="int"/>.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Adds 1 to the Count property.
        /// </summary>
        public void AddOne()
        {
            Count++;
        }
    }
}