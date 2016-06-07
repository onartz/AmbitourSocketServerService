using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using AmbitourSocketServerService.ObjetsMetiers;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Serialization;
using SocketServer;
using System.IO;

namespace AmbitourSocketServerService
{
    /// <summary>
    /// Windows service to hold request coming from agents.
    /// Server is listening on wifi ip address on port 11000
    /// This service write status in a Windows eventLog MyNewLog
    /// 
    /// When a new request arrives, the server responds with the same data and try to deserialize it as a ACLMessage
    /// and store it as an xml file in the Request Queue on the disk to be processed later by Ambitour
    /// 
    /// The request must contain <EOF> at the end
    /// </summary>
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists("Ambitour"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Ambitour", "MyNewLog");
            }
            eventLog1.Source = "Ambitour";
            eventLog1.Log = "MyNewLog";
            eventLog1.WriteEntry("Init Service OK");
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Ambitour socket server started");
            try
            {
                Thread thread = new Thread(StartListening);
                thread.Start();
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry(ex.Message);
            }
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Ambitour socket server stopped");
        }

        public static string data = null;

        public void StartListening()
        {    
            XmlSerializer SerializerObj = new XmlSerializer(typeof(ACLMessage));
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];
            TextWriter tw = null;
            IPAddress ipAddress = IPAddress.Parse(Settings1.Default.ServerIPAddress);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Settings1.Default.Port);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                eventLog1.WriteEntry(String.Format("Server listening on : {0}", localEndPoint.Address.ToString()));

                // Start listening for connections.
                while (true)
                {
                    // Program is suspended while waiting for an incoming connection.
                    Socket handler = listener.Accept();
                    eventLog1.WriteEntry(String.Format("Client connected : {0}", handler.RemoteEndPoint.ToString()));
                    //handler.ReceiveTimeout = Settings1.Default.SocketReceivedTimeout;
                    data = null;
                    try
                    {
                       // while(handler.Poll(1000
                        // An incoming connection needs to be processed.
                        while (true)
                        {
                            bytes = new byte[1024];
                            int bytesRec = handler.Receive(bytes);
                            data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            if (data.IndexOf("<EOF>") > -1)
                            {
                                break;
                            }
                        }
                        eventLog1.WriteEntry(String.Format("Text received : {0}", data));

                        // Echo the data back to the client.
                        byte[] response = Encoding.ASCII.GetBytes(data);

                        handler.Send(response);
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                        String request = data.Replace("<EOF>", "");
                        // eventLog1.WriteEntry(String.Format("Text received : {0}", data));

                        //Deserialize message to ACLMessage : if OK, save to queue

                        XmlReader xmlReader = XmlReader.Create(new StringReader(request));
                        ACLMessage msg = (ACLMessage)SerializerObj.Deserialize(xmlReader);

                        if (msg != null)
                        {
                            tw = new StreamWriter(Settings1.Default.IncomingQueue + Guid.NewGuid() + Settings1.Default.FileExtension);
                            SerializerObj.Serialize(tw, msg);
                            tw.Close();
                        }
                        else
                            eventLog1.WriteEntry("Msg = null");
                    }
                   
                    catch (Exception e)
                    {
                        if(tw != null)
                            tw.Close();
                        eventLog1.WriteEntry(String.Format(e.ToString()));
                    }
                }
            }
            catch (Exception e)
            {
                eventLog1.WriteEntry(String.Format(e.ToString()));
            }
           
        }  
    }
}


 


