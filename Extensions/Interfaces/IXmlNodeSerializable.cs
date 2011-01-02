using System.Xml;

namespace gnux.Extensions.Interfaces
{
    public interface IXmlNodeSerializable
    {
        string XML_NODE_NAME { get; }

        /// <summary>
        /// Write object to a XML Node
        /// </summary>
        /// <param name="node"></param>
        void Write(XmlNode node);

        /// <summary>
        /// Read object from a XML node
        /// </summary>
        /// <param name="node"></param>
        void Read(XmlNode node);
    }
}
