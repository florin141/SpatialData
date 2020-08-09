using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Spatial.Core
{
    public class AixmSerializer<T> : IAixmSerializer<T>
    {
        private readonly XmlSerializer _serializer;
        private XmlSerializerNamespaces _ns;

        public bool UnknownAttributeFound { get; private set; }
        public bool UnknownElementFound { get; private set; }
        public bool UnknownNodeFound { get; private set; }
        public bool UnreferencedObjectFound { get; private set; }


        #region Namespaces

        private IDictionary<string, string> GetNamespacesFromSource(string source)
        {
            XPathDocument doc = new XPathDocument(new StringReader(source));
            XPathNavigator namespaceNavigator = doc.CreateNavigator();
            namespaceNavigator.MoveToFollowing(XPathNodeType.Element);
            return namespaceNavigator.GetNamespacesInScope(XmlNamespaceScope.All);
        }

        private void InitializeXmlSerializerNamespaces()
        {
            _ns = new XmlSerializerNamespaces();
            // aero/aixm/v5_1_1
            _ns.Add("aixm", "http://www.aixm.aero/schema/5.1.1");

            // aero/aixm/schema/_5_1_1/extensions/eur/adr
            _ns.Add("adr", "http://www.aixm.aero/schema/5.1.1/extensions/EUR/ADR");

            // aero/fixm/v4_1_0/base
            _ns.Add("fb", "http://www.fixm.aero/base/4.1");
            // aero/fixm/v4_1_0/flight
            _ns.Add("fx", "http://www.fixm.aero/flight/4.1");
            // aero/fixm/v4_1_0/messaging
            _ns.Add("mesg", "http://www.fixm.aero/messaging/4.1");
            // aero/fixm/v4_1_0/nm/v1_2
            _ns.Add("nm", "http://www.fixm.aero/nm/1.2");

            // _int/eurocontrol/cfmu/b2b/adrmessage
            _ns.Add("message", "http://www.eurocontrol.int/cfmu/b2b/ADRMessage");

            // net/opengis/gml/_3
            _ns.Add("gml", "http://www.opengis.net/gml/3.2");

            // org/isotc211/_2005/gts
            _ns.Add("gts", "http://www.isotc211.org/2005/gts");
            // org/isotc211/_2005/gss
            _ns.Add("gss", "http://www.isotc211.org/2005/gss");
            // org/isotc211/_2005/gsr
            _ns.Add("gsr", "http://www.isotc211.org/2005/gsr");
            // org/isotc211/_2005/gmd
            _ns.Add("gmd", "http://www.isotc211.org/2005/gmd");
            // org/isotc211/_2005/gco
            _ns.Add("gco", "http://www.isotc211.org/2005/gco");

            _ns.Add("xlink", "http://www.w3.org/1999/xlink");
            _ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            _ns.Add("xs", "http://www.w3.org/2001/XMLSchema");
        }

        #endregion

        public AixmSerializer()
        {
            _serializer = new XmlSerializer(typeof(T));

            _serializer.UnknownAttribute += SerializerOnUnknownAttribute;
            _serializer.UnknownElement += SerializerOnUnknownElement;
            _serializer.UnknownNode += SerializerOnUnknownNode;
            _serializer.UnreferencedObject += SerializerOnUnreferencedObject;

            InitializeXmlSerializerNamespaces();
        }

        private void SerializerOnUnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        {
            UnreferencedObjectFound = true;

            Trace.TraceWarning($"{sender}: {e}");
        }

        private void SerializerOnUnknownNode(object sender, XmlNodeEventArgs e)
        {
            UnknownNodeFound = true;

            Trace.TraceWarning($"{sender}: {e}");
        }

        private void SerializerOnUnknownElement(object sender, XmlElementEventArgs e)
        {
            UnknownElementFound = true;

            Trace.TraceWarning($"{sender}: {e}");
        }

        private void SerializerOnUnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            UnknownAttributeFound = true;

            Trace.TraceWarning($"{sender}: {e}");
        }

        public T Deserialize(string input)
        {
            return (T)_serializer.Deserialize(new StringReader(input));
        }

        public T Deserialize(Stream stream)
        {
            return (T)_serializer.Deserialize(stream);
        }

        public string Serialize(T obj, bool namespacePrefixes = false)
        {
            StringWriter textWriter = new StringWriter();

            if (namespacePrefixes)
            {
                _serializer.Serialize(textWriter, obj, _ns);
            }
            else
            {
                _serializer.Serialize(textWriter, obj);
            }

            string result = textWriter.ToString();
            return result;
        }
    }
}
