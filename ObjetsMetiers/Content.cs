using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace SocketServer
{
    [XmlInclude(typeof(Handle))]
    public abstract class Content
    {
    }
}
