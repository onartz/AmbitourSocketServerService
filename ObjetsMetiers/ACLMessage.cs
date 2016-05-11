using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SocketServer
{
    [Serializable]
    public class ACLMessage
    {
        static int REQUEST = 0;
        string sender;
        string conversationId;
        Content content;

        public Content Content
        {
            get { return content; }
            set { content = value; }
        }

        public string ConversationId
        {
            get { return conversationId; }
            set { conversationId = value; }
        }

        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }
        string receiver;

        public string Receiver
        {
            get { return receiver; }
            set { receiver = value; }
        }



        /// <summary>
        /// Create message from string
        /// (REQUEST\r\n
        ///  :sender ( agent-identifier :name a@10.10..68.55:1099/Jade :addresses ) )
        /// </summary>
        /// <param name="fileContent"></param>
        public ACLMessage(string fileContent)
        {
            string pat1 = @"(([A-Za-z0-9\-]+)\r\n$";
            Regex r = new Regex(pat1, RegexOptions.IgnoreCase);
            Match m = r.Match(fileContent);
            while (m.Success)
            {
                //Console.WriteLine("Match" + m.Value);
            }


        }

        public ACLMessage()
        {
            // TODO: Complete member initialization
        }

      
    }
}
