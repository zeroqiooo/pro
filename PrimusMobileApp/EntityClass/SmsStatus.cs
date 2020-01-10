using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PrimusMobileApp.EntityClass
{
    [DataContract]
    public class SmsStatus
    {
        [DataMember]
        private String sessionId;
        [DataMember]
        public String SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        [DataMember]
        private List<SmsStatusList> statusList;
        [DataMember]
        public List<SmsStatusList> StatusList
        {
            get { return statusList; }
            set { statusList = value; }
        }

    }
}