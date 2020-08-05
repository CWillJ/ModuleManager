﻿namespace ModuleManager.Classes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Xml;

    /// <summary>
    /// XmlDescriptionRetriever is used to get text form an xml file.
    /// </summary>
    public class XmlDescriptionRetriever
    {
        private string _dllFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDescriptionRetriever"/> class.
        /// </summary>
        public XmlDescriptionRetriever()
        {
            _dllFilePath = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDescriptionRetriever"/> class.
        /// </summary>
        /// <param name="dllFilePath">The path to the dll file.</param>
        public XmlDescriptionRetriever(string dllFilePath)
        {
            _dllFilePath = dllFilePath;
        }

        /// <summary>
        /// Gets or sets the file path to the dll file.
        /// </summary>
        public string DllFilePath
        {
            get { return _dllFilePath; }
            set { _dllFilePath = value; }
        }

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

            try
            {
                paramList = method.GetParameters();
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Cannot Load Parameters For " + method.Name);
                return GetParametersFromXml(method, memberIndex);
            }
            catch (FileLoadException)
            {
                Debug.WriteLine("Cannot Load Parameters For " + method.Name);
                return GetParametersFromXml(method, memberIndex);
            }
            catch (TypeLoadException)
            {
                Debug.WriteLine("Cannot Load Parameters For " + method.Name);
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
        private ObservableCollection<MemberParameter> GetParametersFromXml(MethodBase method, int memberIndex = 0)
        {
            ObservableCollection<MemberParameter> parameters = new ObservableCollection<MemberParameter>();
            XmlNode xmlNode = GetMemberXmlNode(method, memberIndex);
            XmlAttributeCollection attributeCollection;
            string innerXml;

            string paramName = string.Empty;
            string paramDescription = string.Empty;
            string paramType = string.Empty;

            if (xmlNode == null)
            {
                return null;
            }

            foreach (XmlNode xmlParamNode in xmlNode.SelectNodes("param"))
            {
                if (xmlParamNode == null)
                {
                    return null;
                }

                innerXml = xmlParamNode.InnerXml;

                if (string.IsNullOrEmpty(innerXml))
                {
                    paramDescription = string.Empty;
                }
                else
                {
                    innerXml = Regex.Replace(innerXml, @"\s+", " ").Trim();
                    paramDescription = xmlNode.InnerText.Trim();
                }

                attributeCollection = xmlParamNode.Attributes;

                //// XmlNodeList bull1 = null;
                //// XmlNode bull2 = null;
                //// XmlAttributeCollection bull3 = null;
                //// XmlNode bull4 = null;
                //// string bull5 = string.Empty;

                // TODO get this to not throw exception
                paramType = xmlParamNode.ChildNodes.Item(0).Attributes.GetNamedItem("cref").InnerText;

                paramName = attributeCollection.GetNamedItem("name").Value;

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
        private XmlNode GetMemberXmlNode(MethodBase method, int nodeIndex = 0)
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
        private XmlNode GetModuleXmlNode(Type type)
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
        private XmlNode GetPropertyXmlNode(PropertyInfo property)
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
        private string GetXmlNodeString(XmlNode xmlNode, string xmlTag, int index = 0)
        {
            string s = null;

            XmlNodeList xmlNodeList = xmlNode.SelectNodes(xmlTag);
            if (xmlNodeList[index] == null)
            {
                return s;
            }

            s = xmlNodeList[index].InnerXml;
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            s = Regex.Replace(s, @"\s+", " ");
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return s.Trim();
        }
    }
}