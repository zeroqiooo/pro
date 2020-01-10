using GAIA.Platform.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PrimusMobileApp.EntityClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PrimusMobileApp
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    public class Service1 : IAppService
    {
        private string MD5New(string toHash)
        {
            MD5CryptoServiceProvider crypto = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(toHash);
            bytes = crypto.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte num in bytes)
            {
                sb.AppendFormat("{0:x2}", num);
            }
            return sb.ToString();        //32位
        }

        private string MD5Old(string strold)
        {
            if (strold.Trim() != "")
            {
                MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();
                UnicodeEncoding _ascii = new UnicodeEncoding();
                Byte[] _byte = _ascii.GetBytes(strold);
                Byte[] _byteEncrypt = _md5.ComputeHash(_byte);
                string _strReturn = BitConverter.ToString(_byteEncrypt);
                return _strReturn;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 校验用户帐号密码
        /// </summary>
        /// <param name="LoginAccount"></param>
        /// <param name="Password"></param>
        /// <param name="TimeOut"></param>
        /// <param name="BusinessCode"></param>
        /// <param name="msg"></param>
        /// <param name="SessionID"></param>
        /// <param name="um"></param>
        /// <returns></returns>
        public bool ValidateUser(string LoginAccount, string Password, int TimeOut,
            int BusinessCode, out string msg, out string SessionID, out UserModule um)
        {
            um = new UserModule();
            msg = "";
            SessionID = "";

            PersistBroker b = PersistBroker.Instance();
            b.TimeOut = TimeOut;

            try
            {
                DataTable dt = b.ExecuteSQLForDst(@"select LOGINNAME,TRUENAME,FIRSTNAME,d.NAMES as POSITIONNAME,
                        b.PERSONID,c.UNITNAME,a.ISLOCK,b.EMPLOYEEID,a.USERID,a.PASSWORD from secuser a 
                    left join psnaccount b on a.personid = b.PERSONID
                    left join ORGStdStruct c on c.unitid = b.branchid
                    left join cpcjobcode d on d.JOBCODEID = b.jobcode 
                    where b.PERSONID is not null AND LOGINNAME = '" + LoginAccount.Replace("'", "''") + "' ").Tables[0];

                if (dt.Rows.Count == 0)
                {
                    msg = "帐号不存在";
                    return false;
                }

                if (dt.Rows[0]["ISLOCK"].ToString() == "1")
                {
                    msg = "帐号已禁用";
                    return false;
                }

                string DBKey = dt.Rows[0]["PASSWORD"].ToString();
                if (MD5New(Password) == DBKey || MD5Old(Password) == DBKey)
                {
                    um.UserAccount = dt.Rows[0]["LOGINNAME"].ToString();
                    um.IsLock = false;
                    um.UserChineseName = dt.Rows[0]["TRUENAME"].ToString();
                    um.UserEmployeeID = dt.Rows[0]["EMPLOYEEID"].ToString();
                    um.UserEnglishName = dt.Rows[0]["FIRSTNAME"].ToString();
                    um.UserDepartmentName = dt.Rows[0]["UNITNAME"].ToString();
                    um.UserID = dt.Rows[0]["USERID"].ToString();
                    um.PersonID = dt.Rows[0]["PERSONID"].ToString();
                    um.UserPosition = dt.Rows[0]["POSITIONNAME"].ToString();

                    SessionID = Guid.NewGuid().ToString();

                    b.ExecuteSQL("update SECUSER set USERITEM4 = '" + SessionID + "' where USERID='" + um.UserID + "'");

                    return true;
                }
                else
                {
                    msg = "密码不正确";
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            finally
            {
                b.Close();
            }

        }

        public string GetSMSContentInfo(string sessionID)
        {
            DataSet ds = new DataSet();
            PersistBroker b = PersistBroker.Instance();
            try
            {
                if (sessionID != "")
                {
                    //验证用户是否已通过登录验证
                    DataSet dtUser = b.ExecuteSQLForDst("select USERID from secuser where USERITEM4 = '" + sessionID + "'");
                    if (dtUser.Tables[0].Rows.Count > 0)
                    {
                        DataSet tempDS = new DataSet();
                        DataTable tempDT = new DataTable();
                        tempDS = b.ExecuteSQLForDst(@"
                                SELECT T_PICI FROM SMSCONTENT
                                WHERE S_STATE <> '2'  AND CELLPHONE IS NOT NULL
                                GROUP BY T_PICI
                                ");
                        if (tempDS != null && tempDS.Tables[0] != null)
                        {
                            int icount = tempDS.Tables[0].Rows.Count > 10 ? 10 : tempDS.Tables[0].Rows.Count;
                            for (int i = 0; i < tempDS.Tables[0].Rows.Count; i++)
                            {
                                if (i < 10)
                                {
                                    tempDT = new DataTable();
                                    tempDT.Merge(b.ExecuteSQLForDst(@"
                                    SELECT ID
                                        ,PERSONID
                                        ,EMPLOYEEID
                                        ,ENAME
                                        ,PCOMTENT
                                        ,PTIME
                                        ,CELLPHONE
                                        ,T_PICI
                                        ,P_SALARYMONTH
                                        ,BATCHID
                                        ,'" + icount + @"' AS COUNT
                                    FROM SMSCONTENT WHERE S_STATE = '1' AND CELLPHONE IS NOT NULL AND T_PICI = '" + tempDS.Tables[0].Rows[i]["T_PICI"].ToString() + @"'
                                ").Tables[0]);
                                    tempDT.TableName = (i + 1).ToString();
                                    ds.Tables.Add(tempDT);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                b.Close();
            }
            return JsonConvert.SerializeObject(ds, new DataTableConverter());
        }

        /// <summary>
        /// 0 and '' ZGAIA03358 待发送
        ///2 ZGAIA03355 发送成功
        ///? ZGAIA03354 发送失败
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public string UpdateSMSContentInfo(string con)
        {
            if (!string.IsNullOrEmpty(con))
            {
                DataSet dt = new DataSet();
                PersistBroker b = PersistBroker.Instance();
                try
                {
                    SmsStatus s = (SmsStatus)JsonConvert.DeserializeObject(con, typeof(SmsStatus));
                    if (s.SessionId != "")
                    {
                        //验证用户是否已通过登录验证
                        DataSet dtUser = b.ExecuteSQLForDst("select USERID from secuser where USERITEM4 = '" + s.SessionId + "'");
                        if (dtUser.Tables[0].Rows.Count > 0)
                        {
                            List<SmsStatusList> n = s.StatusList;
                            b.BeginTrans();
                            foreach (SmsStatusList item in n)
                            {
                                dt = b.ExecuteSQLForDst(@"
                                        UPDATE SMSCONTENT
                                           SET S_STATE = '" + item.SendStatus + @"'
                                              ,STIME ='" + item.SendTime + @"'
                                         WHERE ID = '" + item.Id + @"'
                                ");
                            }
                            b.CommitTrans();
                        }
                    }
                }
                catch (Exception ex)
                {
                    b.RollbackTrans();
                    return JsonConvert.SerializeObject("The data is abnormal", new DataTableConverter());
                }
                finally
                {
                    b.Close();
                    //Conn.Close();
                }
                return JsonConvert.SerializeObject("The update is successful", new DataTableConverter());
            }
            else
            {
                return JsonConvert.SerializeObject("Data is empty", new DataTableConverter());
            }
        }
    }
}
