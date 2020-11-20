namespace ModuleManager.ModuleLoader.Classes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Xml;
    using ModuleManager.ModuleObjects.Classes;

    /// <summary>
    /// XmlDescriptionRetriever is used to get text form an xml file.
    /// </summary>
    public class XmlDescriptionRetriever
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDescriptionRetriever"/> class.
        /// </summary>
        public XmlDescriptionRetriever()
        {
            DllFilePath = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDescriptionRetriever"/> class.
        /// </summary>
        /// <param name="dllFilePath">The path to the dll file.</param>
        public XmlDescriptionRetriever(string dllFilePath)
        {
            DllFilePath = dllFilePath;
        }

        /// <summary>
        /// Gets or sets the file path to the dll file.
        /// </summary>
        public string DllFilePath { get; set; }

        /// <summary>
        /// GetModuleDescription returns a clean string from the inner xml
        /// of the class description of the Type.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to get the string desctiption from.</param>
        /// <returns>String representation of the class description.</returns>
        public string GetModuleDescription(Type type)
        {
            XmlNode xmlNode = GetModuleXmlNode(type);

            if (xmlNode == null)
            {
                return null;
            }

            return GetXmlNodeString(xmlNode, "summary");
        }

        /// <summary>
        /// Returns a string from the inner xml of the method description.
        /// </summary>
        /// <param name="method"><see cref="MethodInfo"/> to get the string from.</param>
        /// <param name="index">Index used for methods/constructors with same name.</param>
        /// <returns>String representation of the method description.</returns>
        public string GetMethodDescription(MethodInfo method, int index = 0)
        {
            XmlNode xmlNode = GetMemberXmlNode(method, index);

            if (xmlNode == null)
            {
                return null;
            }

            return GetXmlNodeString(xmlNode, "summary");
        }

        /// <summary>
        /// Returns a string from the inner xml of the constructor description.
        /// </summary>
        /// <param name="constructor"><see cref="ConstructorInfo"/> to get the string from.</param>
        /// <param name="index">Index used for methods/constructors with same name.</param>
        /// <returns>String representation of the method description.</returns>
        public string GetConstructorDescription(ConstructorInfo constructor, int index = 0)
        {
            XmlNode xmlNode = GetMemberXmlNode(constructor, index);

            if (xmlNode == null)
            {
                return null;
            }

            return GetXmlNodeString(xmlNode, "summary");
        }

        /// <summary>
        /// Returns a string from the inner xml of the property desctiption.
        /// </summary>
        /// <param name="property"><see cref="PropertyInfo"/> to get the description from.</param>
        /// <returns>String representation of the property description.</returns>
        public string GetPropertyDescription(PropertyInfo property)
        {
            XmlNode xmlNode = GetPropertyXmlNode(property);

            if (xmlNode == null)
            {
                return null;
            }

            return GetXmlNodeString(xmlNode, "summary");
        }

        /// <summary>
        /// Returns a string from the inner xml of the parameter description.
        /// </summary>
        /// <param name="method"><see cref="MemberInfo"/> to get the string from.</param>
        /// <param name="parameterIndex">Integer index of parameter.</param>
        /// <param name="memberIndex">Integer index of member.</param>
        /// <returns>String representation of the parameter description.</returns>
        public string GetMemberParameterDescription(MethodBase method, int parameterIndex, int memberIndex = 0)
        {
            XmlNode xmlNode = GetMemberXmlNode(method, memberIndex);

            if (xmlNode == null)
            {
                return null;
            }

            return GetXmlNodeString(xmlNode, "param", parameterIndex);
        }

        /// <summary>
        /// Returns an <see cref="ObservableCollection{MemberParameter}"/> from a list of <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="method">The <see cref="MethodBase"/> to get the parameters from.</param>
        /// <param name="memberIndex">Integer index of member.</param>
        /// <returns>An <see cref="ObservableCollection{MemberParameter}"/>.</returns>
        public ObservableCollection<MemberParameter> GetParametersFromList(MethodBase method, int memberIndex = 0)
        {
            ObservableCollection<MemberParameter> parameters = new ObservableCollection<MemberParameter>();
            ParameterInfo[] paramList;

            // If the parameter cannot be loaded because the assembly isn't loaded,
            // attempt to get the parameter type from the xml comments.
            try
            {
                paramList = method.GetParameters();
            }
            catch (FileNotFoundException)
            {
                return GetParametersFromXml(method, memberIndex);
            }
            catch (FileLoadException)
            {
                return GetParametersFromXml(method, memberIndex);
            }
            catch (TypeLoadException)
            {
                return GetParametersFromXml(method, memberIndex);
            }

            if (paramList == null || paramList.Length == 0)
            {
                parameters.Add(new MemberParameter());
            }
            else
            {
                foreach (var p in paramList)
                {
                    Type pType = p.ParameterType;
                    string pName = p.Name;
                    string pDescription = GetMemberParameterDescription(
                        method,
                        Array.IndexOf(paramList, p),
                        memberIndex);

                    parameters.Add(new MemberParameter(
                        pType,
                        pName,
                        pDescription));
                }
            }

            return parameters;
        }

        /// <summary>
        /// Returns a string desctiption of the return value of a <see cref="MethodBase"/>.
        /// </summary>
        /// <param name="method"><see cref="MethodBase"/> to get the string desctiption of the return value from.</param>
        /// <returns>String desctiption of the return value.</returns>
        public string GetMemberReturnDescription(MethodBase method)
        {
            XmlNode xmlNode = GetMemberXmlNode(method);

            if (xmlNode == null)
            {
                return null;
            }

            return GetXmlNodeString(xmlNode, "returns");
        }

        /// <summary>
        /// Returns an <see cref="ObservableCollection{MemberParameter}"/>.
        /// </summary>
        /// <param name="method">The <see cref="MethodBase"/> to get parameters from.</param>
        /// <param name="memberIndex">Member index.</param>
        /// <returns>An <see cref="ObservableCollection{MemberParameter}"/>.</returns>
        public ObservableCollection<MemberParameter> GetParametersFromXml(
            MethodBase method,
            int memberIndex = 0)
        {
            ObservableCollection<MemberParameter> parameters =
                new ObservableCollection<MemberParameter>();
            XmlNode xmlNode = GetMemberXmlNode(method, memberIndex);
            XmlAttributeCollection attributeCollection;

            string paramName;
            string paramDescription;
            string paramTypeName;

            Debug.WriteLine("Cannot Load Parameters For " + method.Name);

            if (xmlNode == null)
            {
                parameters.Add(new MemberParameter());
                return parameters;
            }

            foreach (XmlNode xmlParamNode in xmlNode.SelectNodes(@"param"))
            {
                if (xmlParamNode == null)
                {
                    continue;
                }

                paramDescription = GetXmlNodeText(xmlParamNode);

                XmlNode tempNode = xmlParamNode.SelectSingleNode(@"see");
                if (tempNode == null)
                {
                    paramTypeName = string.Empty;
                }
                else
                {
                    attributeCollection = tempNode.Attributes;
                    paramTypeName = attributeCollection.GetNamedItem(@"cref").Value;
                }

                attributeCollection = xmlParamNode.Attributes;
                paramName = attributeCollection.GetNamedItem(@"name").Value;

                parameters.Add(new MemberParameter(paramTypeName, paramName, paramDescription));
            }

            return parameters;
        }

        /// <summary>
        /// Returns an <see cref="XmlNode"/> of the specified <see cref="MemberInfo"/>.
        /// </summary>
        /// <param name="method">The <see cref="MethodBase"/> to get the <see cref="XmlNode"/> from.</param>
        /// <param name="nodeIndex">The specified node index to handle members with the same name.</param>
        /// <returns><see cref="XmlNode"/>.</returns>
        public XmlNode GetMemberXmlNode(MethodBase method, int nodeIndex = 0)
        {
            string xmlPath = DllFilePath.Substring(0, DllFilePath.LastIndexOf(".")) + @".XML";
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList xmlNodeList;
            string path;

            try
            {
                xmlDoc.Load(xmlPath);
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Cannot Load Member XML For " + method.Name);
                return null;
            }
            catch (XmlException)
            {
                Debug.WriteLine("Cannot Load Member XML For " + method.Name);
                return null;
            }

            if (method.Name == ".ctor")
            {
                path = @"M:" + method.DeclaringType.FullName + @".#" + method.Name.Substring(1);
            }
            else
            {
                path = @"M:" + method.DeclaringType.FullName + @"." + method.Name;
            }

            xmlNodeList = xmlDoc.SelectNodes(@"//member[starts-with(@name, '" + path + @"')]");

            return xmlNodeList[nodeIndex];
        }

        /// <summary>
        /// Returns an <see cref="XmlNode"/> of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get the <see cref="XmlNode"/> from.</param>
        /// <returns><see cref="XmlNode"/> that holds info about the passed in <see cref="Type"/>.</returns>
        public XmlNode GetModuleXmlNode(Type type)
        {
            string xmlPath = DllFilePath.Substring(0, DllFilePath.LastIndexOf(".")) + @".XML";
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(xmlPath);
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Cannot Load Module XML For " + type.Name);
                return null;
            }

            string path = @"T:" + type.FullName;
            XmlNode xmlNode = xmlDoc.SelectSingleNode(@"//member[starts-with(@name, '" + path + @"')]");

            return xmlNode;
        }

        /// <summary>
        /// Returns an <see cref="XmlNode"/> of the specified <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to get the <see cref="XmlNode"/> from.</param>
        /// <returns><see cref="XmlNode"/> that holds info about the passed in <see cref="PropertyInfo"/>.</returns>
        public XmlNode GetPropertyXmlNode(PropertyInfo property)
        {
            string xmlPath = DllFilePath.Substring(0, DllFilePath.LastIndexOf(".")) + @".XML";
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(xmlPath);
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Cannot Load Property Description For " + property.Name);
                return null;
            }
            catch (XmlException)
            {
                Debug.WriteLine("Cannot Load Property Description For " + property.Name);
                return null;
            }

            string path = @"P:" + property.DeclaringType.FullName + @"." + property.Name;
            XmlNode xmlNode = xmlDoc.SelectSingleNode(@"//member[starts-with(@name, '" + path + @"')]");

            return xmlNode;
        }

        /// <summary>
        /// Takes an <see cref="XmlNode"/>, string xml tag, and an index and return the inner xml.
        /// </summary>
        /// <param name="xmlNode">The member <see cref="XmlNode"/>.</param>
        /// <param name="xmlTag">This is the string of the xml tag.</param>
        /// <param name="index">Index of the XmlNodeList, defaults to 0. (used for more than one parameter).</param>
        /// <returns>InnerXml of the <see cref="XmlNode"/>.</returns>
        public string GetXmlNodeString(XmlNode xmlNode, string xmlTag, int index = 0)
        {
            XmlNodeList xmlNodeList = xmlNode.SelectNodes(xmlTag);

            return GetXmlNodeText(xmlNodeList[index]);
        }

        /// <summary>
        /// Returns the formatted text of the inner xml.
        /// </summary>
        /// <param name="xmlNode">The <see cref="XmlNode"/> to get text from.</param>
        /// <returns>Formatted string of the <see cref="XmlNode"/> InnerXml.</returns>
        public string GetXmlNodeText(XmlNode xmlNode)
        {
            string s = null;

            if (xmlNode != null)
            {
                s = xmlNode.InnerXml;
                s = Regex.Replace(s, @"\s+", " ");
                s = s.Trim();
            }

            return s;
        }
    }
}