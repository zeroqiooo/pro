using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PrimusMobileApp.EntityClass
{
    [DataContract]
    public class SmsStatusList
    {
        [DataMember]
        private String id;
        [DataMember]
        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        private String receiver;
        [DataMember]
        public String Receiver
        {
            get { return receiver; }
            set { receiver = value; }
        }

        [DataMember]
        private int sendStatus;
        [DataMember]
        public int SendStatus
        {
            get { return sendStatus; }
            set { sendStatus = value; }
        }

        [DataMember]
        private String sendTime;
        [DataMember]
        public String SendTime
        {
            get { return sendTime; }
            set { sendTime = value; }
        }

        [DataMember]
        private String batchID;
        [DataMember]
        public String BatchID
        {
            get { return batchID; }
            set { batchID = value; }
        }

        [DataMember]
        private String destNumber;
        [DataMember]
        public String DestNumber
        {
            get { return destNumber; }
            set { destNumber = value; }
        }
    }
}