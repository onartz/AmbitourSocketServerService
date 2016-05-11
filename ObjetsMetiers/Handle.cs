using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SocketServer
{
    public class Handle : Content
    {
        private string sender;

        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }
        private string receiver;

        public string Receiver
        {
            get { return receiver; }
            set { receiver = value; }
        }
        private string productLotId;

        public string ProductLotId
        {
            get { return productLotId; }
            set { productLotId = value; }
        }
        private int quantity;

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        private string productId;

        public string ProductId
        {
            get { return productId; }
            set { productId = value; }
        }


        //public Handle(string request)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    try
        //    {
        //        doc.LoadXml(request);
        //    }
        //    catch (XmlException ex)
        //    {
        //        throw ex;
        //    }
        //    if(doc.SelectSingleNode("java/object").Attributes["class"].ToString().Contains("logistic.Handle")){
        //        productLotId = ((XmlElement)doc.SelectSingleNode("java/object/void[@property='productLot']/object/void[@property='lotId']/string")).InnerText;
        //        quantity = Int16.Parse(((XmlElement)doc.SelectSingleNode("java/object/void[@property='productLot']/object/void[@property='quantity']/string")).InnerText);
        //        sender = ((XmlElement)doc.SelectSingleNode("java/object/void[@property='sender']/object/void[@property='adress']/object/void[property='localName']/string")).InnerText;
        //        receiver = ((XmlElement)doc.SelectSingleNode("java/object/void[@property='recipient']/object/void[@property='adress']/object/void[property='localName']/string")).InnerText;
        //    }
        //}

    }
}
