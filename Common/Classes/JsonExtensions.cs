namespace ModuleManager.Common.Classes
{
    using System.IO;
    using Newtonsoft.Json;

    /// <summary>
    /// Extension methods for generic objects.
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Deep copy an object using serialization.
        /// </summary>
        /// <typeparam name="T">The type of the object to be copied.</typeparam>
        /// <param name="objectToBeCopied">The object to be copied.</param>
        /// <returns>A new copy of the <typeparamref name="T"/> object.</returns>
        public static T Copy<T>(this T objectToBeCopied)
        {
            var serializedObject = JsonConvert.SerializeObject(objectToBeCopied, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            var copy = JsonConvert.DeserializeObject<T>(serializedObject, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }) !;
            return copy;
        }

        /// <summary>
        /// Save an object to disk using serialization.
        /// </summary>
        /// <param name="objectToSave">The object to be saved.</param>
        /// <param name="filePath">The <see cref="string"/> path to the file to be created or saved.</param>
        public static void Save(this object objectToSave, string filePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            var serializedObject = JsonConvert.SerializeObject(objectToSave, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            using (StreamWriter stream = new StreamWriter(filePath, false))
            {
                stream.Write(serializedObject);
            }
        }

        /// <summary>
        /// Load an object from disk using deserialization.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize to.</typeparam>
        /// <param name="filePath">The <see cref="string"/> path to the file to be loaded.</param>
        /// <returns>The newly loaded object.</returns>
        public static T Load<T>(string filePath)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }) !;
        }
    }
}
