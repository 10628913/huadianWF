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
    public class Appintface : System.Web.Services.WebService
    {

        [WebMethod()]
        //公开的方法 ，返回加密的公钥，xml的形式  
        public string GetPublicKey()
        {
            RSACryptoServiceProvider cryot = GetKeyFromState();
            return cryot.ToXmlString(false);
        }
        //生成一个 RSA算法的对象，该对象生成的时候，就自动生成出公钥，私钥  
        private RSACryptoServiceProvider GetKeyFromState()
        {
            RSACryptoServiceProvider crypt = null;
            if (Application["Key"] == null)
            {
                //把RSA对象放置在全局的缓冲区，方便解密时使用  
                CspParameters param = new CspParameters();
                param.Flags = CspProviderFlags.UseMachineKeyStore;
                crypt = new RSACryptoServiceProvider(param);
                Application["Key"] = crypt;
            }
            else
            {
                crypt = (RSACryptoServiceProvider)Application["Key"];
            }
            return crypt;
        }
        [WebMethod(Description = "Parameters to this method must be encrypted with the web service public key")]
        public string DepositFunds(byte[] encryptedAccountNumber, byte[] encryptedAmount)
        {
            //模拟的一个公开的方法，给客户传递加密的用户名和钱的数量  
            RSACryptoServiceProvider crypt = GetKeyFromState();
            string decryptedAccountNumber;
            decimal decryptedAmount;
            System.Text.UnicodeEncoding ncd = new System.Text.UnicodeEncoding();
            //解密过程   
            decryptedAccountNumber = ncd.GetString(crypt.Decrypt(encryptedAccountNumber, false));
            decryptedAmount = Decimal.Parse(ncd.GetString(crypt.Decrypt(encryptedAmount, false)));
            //向调用客户返回一个字符串，明文，带有解密信息  
            return decryptedAccountNumber + "has used" + decryptedAmount.ToString();

        }

        [WebMethod()]
        public string getuser(byte[] username)
        { 
            RSACryptoServiceProvider crypt = GetKeyFromState();
            string deusername;
            System.Text.UnicodeEncoding ncd = new System.Text.UnicodeEncoding();
            deusername = ncd.GetString(crypt.Decrypt(username, false));
            return deusername;
        }
        [WebMethod()]
        public string test() {
            //数据库连接串
            string connStr = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;
            //数据库连接对象
            SqlConnection myConn = new SqlConnection(connStr);
            
            string sql = "select * from xx_xitong_user";
            //数据适配器
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            SqlCommand sqlCommand = new SqlCommand(sql,myConn);
            dataAdapter.SelectCommand = sqlCommand;

            DataSet dataSet = new DataSet();
            myConn.Open();
            dataAdapter.SelectCommand.BeginExecuteNonQuery();
            myConn.Close();
            dataAdapter.Fill(dataSet);
            if (dataSet.Tables.Count > 0  && dataSet.Tables[0].Rows.Count > 0) {
                string jsonData = JsonUtil.ToJson(dataSet.Tables[0],"data","200","成功");
                return jsonData.ToString();
            }

            return "";
        }
        [WebMethod()]
        public void testJpush()
        {
            JPushApi.push();


        }
    }
}
