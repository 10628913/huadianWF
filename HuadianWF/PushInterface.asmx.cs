using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using cn.jpush.api;
using cn.jpush.api.push.mode;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using cn.jpush.api.push.notification;
using System.Xml;

namespace HuadianWF
{
    /// <summary>
    /// Appintface 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class PushInterface : System.Web.Services.WebService
    {

        public static String app_key_driver = ConfigurationManager.AppSettings["jush_app_key_driver"];
        public static String master_secret_driver = ConfigurationManager.AppSettings["jpush_master_secret_driver"];

        public static String app_key_client = ConfigurationManager.AppSettings["jush_app_key_client"];
        public static String master_secret_client = ConfigurationManager.AppSettings["jpush_master_secret_client"];
        //[WebMethod()]
        ////公开的方法 ，返回加密的公钥，xml的形式  
        //public string GetPublicKey()
        //{
        //    RSACryptoServiceProvider cryot = GetKeyFromState();
        //    return cryot.ToXmlString(false);
        //}
        ////生成一个 RSA算法的对象，该对象生成的时候，就自动生成出公钥，私钥  
        //private RSACryptoServiceProvider GetKeyFromState()
        //{
        //    RSACryptoServiceProvider crypt = null;
        //    if (Application["Key"] == null)
        //    {
        //        //把RSA对象放置在全局的缓冲区，方便解密时使用  
        //        CspParameters param = new CspParameters();
        //        param.Flags = CspProviderFlags.UseMachineKeyStore;
        //        crypt = new RSACryptoServiceProvider(param);
        //        Application["Key"] = crypt;
        //    }
        //    else
        //    {
        //        crypt = (RSACryptoServiceProvider)Application["Key"];
        //    }
        //    return crypt;
        //}
        //[WebMethod(Description = "Parameters to this method must be encrypted with the web service public key")]
        //public string DepositFunds(byte[] encryptedAccountNumber, byte[] encryptedAmount)
        //{
        //    //模拟的一个公开的方法，给客户传递加密的用户名和钱的数量  
        //    RSACryptoServiceProvider crypt = GetKeyFromState();
        //    string decryptedAccountNumber;
        //    decimal decryptedAmount;
        //    System.Text.UnicodeEncoding ncd = new System.Text.UnicodeEncoding();
        //    //解密过程   
        //    decryptedAccountNumber = ncd.GetString(crypt.Decrypt(encryptedAccountNumber, false));
        //    decryptedAmount = Decimal.Parse(ncd.GetString(crypt.Decrypt(encryptedAmount, false)));
        //    //向调用客户返回一个字符串，明文，带有解密信息  
        //    return decryptedAccountNumber + "has used" + decryptedAmount.ToString();

        //}
        [WebMethod(Description = "向司机推送消息 ps: number为车辆number,title为推送信息标题,content为内容,type!!! 1:代表只推送信息 2:用户长时间未进场,语音提示")]
        public string pushToDriver(string number,string title, string content,string type)
        {
            JPushClient client = new JPushClient(app_key_driver, master_secret_driver);
            PushPayload tags = JPushApi.PushObject_android_and_ios(number, title, content, type);
            //PushPayload tags = JPushApi.PushSendSmsMessage();
            try
            {

                var result = client.SendPush(tags);
                //由于统计数据并非非是即时的,所以等待一小段时间再执行下面的获取结果方法
                System.Threading.Thread.Sleep(10000);
                //如需查询上次推送结果执行下面的代码
                var apiResult = client.getReceivedApi(result.msg_id.ToString());
                var apiResultv3 = client.getReceivedApi_v3(result.msg_id.ToString());
                //如需查询某个messageid的推送结果执行下面的代码
                var queryResultWithV2 = client.getReceivedApi("1739302794");
                var querResultWithV3 = client.getReceivedApi_v3("1739302794");
                
                return "推送成功";

            }
            catch (APIRequestException e)
            {
                System.Diagnostics.Debug.Write("Error response from JPush server. Should review and fix it. ");
                System.Diagnostics.Debug.Write("HTTP Status: " + e.Status);
                System.Diagnostics.Debug.Write("Error Code: " + e.ErrorCode);
                System.Diagnostics.Debug.Write("Error Message: " + e.ErrorMessage);
                return "推送失败";
            }
            catch (APIConnectionException e)
            {
                return e.message;
            }
        }

        [WebMethod(Description = "向货主推送消息 ps:number为货主number,title为推送标题,content为推送内容,bangdanId:你懂的")]
        public string pushToClient(string number,string title,string content, string bangdanId) {
            JPushClient client = new JPushClient(app_key_client, master_secret_client);
            PushPayload tags = JPushApi.PushObject_android_and_ios(number, title, content, bangdanId);
            //PushPayload tags = JPushApi.PushSendSmsMessage();

            
            try
            {
                var result = client.SendPush(tags);
                //由于统计数据并非非是即时的,所以等待一小段时间再执行下面的获取结果方法
                System.Threading.Thread.Sleep(10000);
                //如需查询上次推送结果执行下面的代码
                var apiResult = client.getReceivedApi(result.msg_id.ToString());
                var apiResultv3 = client.getReceivedApi_v3(result.msg_id.ToString());
                //如需查询某个messageid的推送结果执行下面的代码
                var queryResultWithV2 = client.getReceivedApi("1739302794");
                var querResultWithV3 = client.getReceivedApi_v3("1739302794");

                return "推送成功";

            }
            catch (APIRequestException e)
            {
                System.Diagnostics.Debug.Write("Error response from JPush server. Should review and fix it. ");
                System.Diagnostics.Debug.Write("HTTP Status: " + e.Status);
                System.Diagnostics.Debug.Write("Error Code: " + e.ErrorCode);
                System.Diagnostics.Debug.Write("Error Message: " + e.ErrorMessage);
                return "推送失败";
            }
            catch (APIConnectionException e)
            {
                return e.message;
            }
        }
        [WebMethod()]
        public void test() {
            JPushApi.PushSendSmsMessage();
        }
        
    }
}
