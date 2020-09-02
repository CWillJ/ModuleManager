namespace ModuleRetriever
{
    using System;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    /// A class used to serialize interfaces as well as objects.
    /// </summary>
    /// <typeparam name="T">Type of what you want to serialize.</typeparam>
    public sealed class XmlSerialize<T> : IXmlSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerialize{T}"/> class.
        /// </summary>
        public XmlSerialize()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerialize{T}"/> class.
        /// </summary>
        /// <param name="t">Type of what you want to serialize.</param>
        public XmlSerialize(T t)
        {
            Value = t;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Writes to an xml file using the passed in XmlWriter.
        /// </summary>
        /// <param name="writer">XmlWriter used to write to an xml file.</param>
        public void WriteXml(XmlWriter writer)
        {
            if (Value == null)
            {
                writer.WriteAttributeString("type", "null");
                return;
            }

            Type type = Value.GetType();
            XmlSerializer serializer = new XmlSerializer(type);

            writer.WriteAttributeString("type", type.AssemblyQualifiedName);
            serializer.Serialize(writer, Value);
        }

        /// <summary>
        /// Reads from an xml file using the passed in XmlReader.
        /// </summary>
        /// <param name="reader">XmlReader used to read from an xml file.</param>
        public void ReadXml(XmlReader reader)
        {
            if (!reader.HasAttributes)
            {
                throw new FormatException("expected a type attribute!");
            }

            string type = reader.GetAttribute("type");
            reader.Read(); // consume the value

            // leave T at default value if type is null
            if (type == "null")
            {
                return;
            }

            XmlSerializer serializer = new XmlSerializer(Type.GetType(type));

            Value = (T)serializer.Deserialize(reader);
            reader.ReadEndElement();
        }

        /// <summary>
        /// Implements the GetSchema method from IXmlSerializable interface.
        /// </summary>
        /// <returns>null by default.</returns>
        public XmlSchema GetSchema()
        {
            return null;
        }
    }
}
