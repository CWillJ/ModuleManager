namespace ModuleManager.Classes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Xml;
    using ModuleManager.DataObjects;

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
        /// <param name="type">Type to get the string from.</param>
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
        /// GetMethodDescription returns a string from the inner xml of the method
        /// description of the member.
        /// </summary>
        /// <param name="method">MethodInfo to get the string from.</param>
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
        /// GetConstructorDescription returns a string from the inner xml of the
        /// constructor description of the member.
        /// </summary>
        /// <param name="constructor">ConstructorInfo to get the string from.</param>
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
        /// GetProperyDescription will return a string from the inner xml of
        /// the property desctiption.
        /// </summary>
        /// <param name="property">PropertyInfo to get the description from.</param>
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
        /// GetMemberParameterDescription returns a string from the inner xml of the
        /// parameter description of the member.
        /// </summary>
        /// <param name="method">MemberInfo to get the string from.</param>
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
        /// GetParametersFromList will return an ObservableCollection of MemberParameter
        /// type from a list of ParameterInfo type.
        /// </summary>
        /// <param name="method">The method to get the parameters from.</param>
        /// <param name="memberIndex">Integer index of member.</param>
        /// <returns>An ObservableCollection of MemberParameter type.</returns>
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
                    string pType = p.ParameterType.Name.ToString();
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
        /// GetMemberReturnDescription returns a string from the inner xml of the
        /// return description of the member.
        /// </summary>
        /// <param name="method">MethodBase to get the string from.</param>
        /// <returns>String representation of the return description.</returns>
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
        /// GetParametersFromXml will return an ObservableCollection of MemberParameters.
        /// </summary>
        /// <param name="method">The MethodBase to get parameters from.</param>
        /// <param name="memberIndex">Member index.</param>
        /// <returns>An ObservableCollection of MemberParameters.</returns>
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
            string paramType;

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
                    paramType = string.Empty;
                }
                else
                {
                    attributeCollection = tempNode.Attributes;
                    paramType = attributeCollection.GetNamedItem(@"cref").Value;
                }

                attributeCollection = xmlParamNode.Attributes;
                paramName = attributeCollection.GetNamedItem(@"name").Value;

                parameters.Add(new MemberParameter(paramType, paramName, paramDescription));
            }

            return parameters;
        }

        /// <summary>
        /// GetMemberXmlNode returns an XmlNode of the specified MemberInfo.
        /// </summary>
        /// <param name="method">The MethodBase to get the XmlNode from.</param>
        /// <param name="nodeIndex">The specified node index to handle members with the same name.</param>
        /// <returns>XmlNode.</returns>
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
        /// GetModuleXmlNode returns an XmlNode of the specified Type.
        /// </summary>
        /// <param name="type">The Type to get the XmlNode from.</param>
        /// <returns>XmlNode.</returns>
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
        /// GetPropertyXmlNode returns an XmlNode of the specified property.
        /// </summary>
        /// <param name="property">The property to get the XmlNode from.</param>
        /// <returns>XmlNode.</returns>
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
        /// GetXmlNodeString will take an XmlNode, string xml tag, and an index and return the inner xml.
        /// </summary>
        /// <param name="xmlNode">The member XmlNode.</param>
        /// <param name="xmlTag">This is the string of the xml tag.</param>
        /// <param name="index">Index of the XmlNodeList, defaults to 0. (used for more than one parameter).</param>
        /// <returns>InnerXml of the XmlNode.</returns>
        public string GetXmlNodeString(XmlNode xmlNode, string xmlTag, int index = 0)
        {
            XmlNodeList xmlNodeList = xmlNode.SelectNodes(xmlTag);

            return GetXmlNodeText(xmlNodeList[index]);
        }

        /// <summary>
        /// Returns the formatted text of the inner xml.
        /// </summary>
        /// <param name="xmlNode">XmlNode to get text from.</param>
        /// <returns>Formatted string of the XmlNode.InnerXml.</returns>
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