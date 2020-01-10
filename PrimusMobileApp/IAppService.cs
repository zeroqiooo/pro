using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PrimusMobileApp
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IAppService
    {
        [OperationContract]
        bool ValidateUser(string LoginAccount, string Password, int TimeOut,
            int BusinessCode, out string msg, out string SessionID, out UserModule um);

        [OperationContract]
        string GetSMSContentInfo(string sessionID);

        [OperationContract]
        string UpdateSMSContentInfo(string con);
    }


    [DataContract]
    public class UserModule
    {
        private string _UserID = "";
        private string _PersonID = "";
        private string _UserAccount = "";
        private string _UserEmployeeID = "";
        private string _UserChineseName = "";
        private string _UserEnglishName = "";
        private string _UserDepartmentName = "";
        private string _UserPosition = "";
        private bool _IsLock = false;

        [DataMember]
        public bool IsLock
        {
            get { return _IsLock; }
            set { _IsLock = value; }
        }

        [DataMember]
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        [DataMember]
        public string PersonID
        {
            get { return _PersonID; }
            set { _PersonID = value; }
        }

        [DataMember]
        public string UserAccount
        {
            get { return _UserAccount; }
            set { _UserAccount = value; }
        }

        [DataMember]
        public string UserEmployeeID
        {
            get { return _UserEmployeeID; }
            set { _UserEmployeeID = value; }
        }

        [DataMember]
        public string UserChineseName
        {
            get { return _UserChineseName; }
            set { _UserChineseName = value; }
        }

        [DataMember]
        public string UserEnglishName
        {
            get { return _UserEnglishName; }
            set { _UserEnglishName = value; }
        }

        [DataMember]
        public string UserDepartmentName
        {
            get { return _UserDepartmentName; }
            set { _UserDepartmentName = value; }
        }

        [DataMember]
        public string UserPosition
        {
            get { return _UserPosition; }
            set { _UserPosition = value; }
        }
    }
}
