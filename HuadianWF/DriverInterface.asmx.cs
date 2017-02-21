using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace HuadianWF
{
    /// <summary>
    /// DriverInterface 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class DriverInterface : System.Web.Services.WebService
    {
        public static string connstr = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;


        [WebMethod(Description = "用户deviceId校验")]
        public void userValidate(string deviceId)
        {
            SqlConnection conn = new SqlConnection(connstr);
            string sql = "select * from xx_yonghu_cheliangxinxi where sn = '" + deviceId + "'";

            SqlDataAdapter data = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(sql, conn);
            data.SelectCommand = command;

            DataSet ds = new DataSet();
            conn.Open();
            data.SelectCommand.BeginExecuteNonQuery();
            conn.Close();
            data.Fill(ds);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["flag"].ToString() == "" || ds.Tables[0].Rows[0]["flag"].ToString() == "未审核") {
                    Context.Response.Write("{\"code\":\"-1\",\"msg\":\"用户尚未通过审核\"}");
                    return;
                }
                if (ds.Tables[0].Rows[0]["flag"].ToString() == "黑名单")
                {
                    Context.Response.Write("{\"code\":\"-1\",\"msg\":\"用户已在黑名单中!请联系管理员\"}");
                    return;
                }
                //deviceId校验成功，返回用户数据
                string jsonData = JsonUtil.ToJson(ds.Tables[0], "data", "200", "司机信息校验成功");
                Context.Response.Write(jsonData.ToString());
            }
            else {
                //deviceId校验失败
                Context.Response.Write("{\"code\":\"1001\",\"msg\":\"用户信息校验失败\"}");
            }
        }

        [WebMethod(Description = "用户绑定（sn）")]
        public void userComplete(string mobile, string sn) {
            SqlConnection conn = new SqlConnection(connstr);

            string updateSql = "update xx_yonghu_cheliangxinxi set sn = '" + sn + "' where dianhua = '" + mobile + "'";
            SqlCommand command = new SqlCommand(updateSql, conn);

            try
            {
                conn.Open();
                int num = Convert.ToInt32(command.ExecuteNonQuery());
                if (num > 0) {
                    Context.Response.Write("{\"code\":\"200\",\"msg\":\"用户绑定成功，等待审核\"}");
                } else {
                    Context.Response.Write("{\"code\":\"-1\",\"msg\":\"用户绑定失败\"}");
                }
                conn.Close();
                return;
                
            }
            catch (Exception)
            {
                Context.Response.Write("{\"code\":\"1002\",\"msg\":\"用户绑定失败\"}");
            }
        }

        [WebMethod(Description = "获取货主列表")]
        public void getClientList(string license,int pageNum = 1,int pageSize = 10) {
            int totalCount = pageNum * pageSize;
            int startCount = (pageNum - 1) * pageSize;
            SqlConnection conn = new SqlConnection(connstr);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            string selectSql = "SELECT * FROM(SELECT TOP "+ totalCount + " ROW_NUMBER() OVER(ORDER BY label_num ASC) AS ROWID,* FROM av_paidui_huozhucheliangguanxibiao where chepai='"+license+"') as temp1 WHERE ROWID>"+startCount;
            SqlCommand command = new SqlCommand(selectSql,conn);
            DataSet ds = new DataSet();
            sqlDataAdapter.SelectCommand = command;
            conn.Open();
            sqlDataAdapter.SelectCommand.BeginExecuteNonQuery();
            conn.Close();
            sqlDataAdapter.Fill(ds);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string jsonData = JsonUtil.ToJson(ds.Tables[0], "data", "200", "货主列表拉取成功");
                Context.Response.Write(jsonData);
            }
            else
            {
                string str = "";
                if (pageNum == 1)
                {
                    str = "{\"code\":\"1004\",\"msg\":\"货主列表为空\"}";
                }
                else {
                    str = "{\"code\":\"1005\",\"msg\":\"已无更多货主信息\"}";
                }

                Context.Response.Write(str);
            }
        }
        [WebMethod(Description = "获取货主能买的产品")]
        public void getGoodsListByClient(string clientName,string cardType) {
            SqlConnection conn = new SqlConnection(connstr);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            string selectSql = "select 产品编号,产品名称 from av_jichuxinxi_huozhuhuixingbiao where 货主名称='"+clientName+"' and 卡类型='"+ cardType + "'";
            SqlCommand command = new SqlCommand(selectSql, conn);
            DataSet ds = new DataSet();
            sqlDataAdapter.SelectCommand = command;
            conn.Open();
            sqlDataAdapter.SelectCommand.BeginExecuteNonQuery();
            conn.Close();
            sqlDataAdapter.Fill(ds);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string jsonData = JsonUtil.ToJson(ds.Tables[0], "data", "200", "产品列表拉取成功");
                Context.Response.Write(jsonData.ToString());
            }
            else
            {
                Context.Response.Write("{\"code\":\"1006\",\"msg\":\"货主可买商品为空\"}");
            }
        }

        [WebMethod(Description = "通过货品获取可选灰口")]
        public void getGreyCastByGoods(string goodsNum) {
            SqlConnection conn = new SqlConnection(connstr);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            string selectSql = "select * from xx_jichuxinxi_zaishouchanpin where chanpin_num = '"+ goodsNum + "'";
            SqlCommand command = new SqlCommand(selectSql, conn);
            DataSet ds = new DataSet();
            sqlDataAdapter.SelectCommand = command;
            conn.Open();
            sqlDataAdapter.SelectCommand.BeginExecuteNonQuery();
            conn.Close();
            sqlDataAdapter.Fill(ds);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string jsonData = JsonUtil.ToJson(ds.Tables[0], "data", "200", "灰口列表拉取成功");
                Context.Response.Write(jsonData);
            }
            else
            {
                Context.Response.Write("{\"code\":\"1007\",\"msg\":\"无对应灰口\"}");
            }
        }
        [WebMethod(Description = "排队")]
        ///<summary>
        ///</summary>
        ///<param name="clientName"></param>客户名称
        ///<param name="goodsName"></param>货品名称
        ///<param name="greyCastName"></param>灰口名称
        ///<param name="labelNum"></param>卡号
        public void queueUp(string labelNum,string clientName,string goodsName,string greyCastName,string license,string driverName) {
            SqlConnection conn = new SqlConnection(connstr);
            try
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                SqlCommand command = new SqlCommand("PaiDui_ChanShengBangDan", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@label_num", labelNum);
                command.Parameters.AddWithValue("@huozhu", clientName);
                command.Parameters.AddWithValue("@chanpin", goodsName);
                command.Parameters.AddWithValue("@huikou_name", greyCastName);
                //定义输出参数
                SqlParameter output = command.Parameters.Add("@fanhui", SqlDbType.Int);
                //参数类型为output
                output.Direction = ParameterDirection.Output;
                //临时参数，用来接收存储过程返回值
                //SqlParameter sp = new SqlParameter("@return", SqlDbType.Int);
                //sp.Direction = ParameterDirection.ReturnValue;
                //command.Parameters.Add(sp);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                string returnCode = output.Value.ToString();
                string returnStr = "";
                switch (returnCode)
                {
                    case "11":
                        
                        string querySql = "select * from [av_bangdanxinxi_paiduichaxun] where 收货单位='" + clientName + "' and 灰口名称='" + greyCastName + "' and 车牌号='" + license + "' and 司机='" + driverName + "'";
                        command = new SqlCommand(querySql,conn);
                        DataSet ds = new DataSet();
                        sqlDataAdapter.SelectCommand = command;
                        conn.Open();
                        sqlDataAdapter.SelectCommand.BeginExecuteNonQuery();
                        conn.Close();
                        sqlDataAdapter.Fill(ds);
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            returnStr = JsonUtil.ToJson(ds.Tables[0], "data", "200", "排队成功");
                        }
                        else
                        {
                            returnStr = "{\"code\":\"200\",\"msg\":\"排队成功\"}";
                        }
                        break;
                    case "12":
                        returnStr = "{\"code\":\"1012\",\"msg\":\"排队失败,灰口已被占用\"}";
                        break;
                    case "13":
                        returnStr = "{\"code\":\"1013\",\"msg\":\"排队失败,余额不足\"}";
                        break;
                    default:
                        returnStr = "{\"code\":\"1011\",\"msg\":\"排队失败,未知错误\"}";
                        break;
                }
                Context.Response.Write(returnStr);
            }
            catch (Exception)
            {
                Context.Response.Write("{\"code\":\"1009\",\"msg\":\"排队失败，请联系管理员\"}");
            }
        }

        [WebMethod(Description = "获取前方排队人数")]
        public void getQueueNum(string goodsName,string greyCastName,string queueCode) {

            SqlConnection conn = new SqlConnection(connstr);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            string selectSql = "select COUNT(*) as queue_count from DX01_WF.dbo.av_bangdanxinxi_paiduichaxun where 产品名称='"+goodsName+"' and 灰口名称='"+greyCastName+"' and 排队状态='排队' and 排队编码<'"+queueCode+"'";
            SqlCommand command = new SqlCommand(selectSql, conn);
            DataSet ds = new DataSet();
            sqlDataAdapter.SelectCommand = command;
            conn.Open();
            sqlDataAdapter.SelectCommand.BeginExecuteNonQuery();
            conn.Close();
            sqlDataAdapter.Fill(ds);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string jsonData = JsonUtil.ToJson(ds.Tables[0], "data", "200", "前方排队人数获取成功");
                Context.Response.Write(jsonData);
            }
            else
            {
                Context.Response.Write("{\"code\":\"1014\",\"msg\":\"排队人数获取失败\"}");
            }
        }
    }
}
