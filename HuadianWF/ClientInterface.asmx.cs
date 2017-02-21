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
    /// ClientInterface 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ClientInterface : System.Web.Services.WebService
    {
        public static string connstr = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;


        [WebMethod(Description = "货主deviceId校验")]
        public void clientValidate(string deviceId)
        {
            SqlConnection conn = new SqlConnection(connstr);
            string sql = "select * from xx_jichuxinxi_huozhuxinxi where SN = '" + deviceId + "'";

            SqlDataAdapter data = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(sql, conn);
            data.SelectCommand = command;

            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                data.SelectCommand.BeginExecuteNonQuery();
                conn.Close();
                data.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //deviceId校验成功，返回用户数据
                    string jsonData = JsonUtil.ToJson(ds.Tables[0], "data", "200", "货主信息校验成功");
                    Context.Response.Write(jsonData.ToString());
                }
                else
                {
                    //deviceId校验失败
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("{\"code\":\"1001\",\"msg\":\"货主信息校验失败\"}");
                    Context.Response.Write(strBuilder.ToString());
                }
            }
            catch (Exception)
            {

                Context.Response.Write("{\"code\":\"404\",\"msg\":\"请求错误\"}");
            }
            
        }

        [WebMethod(Description = "货主绑定（sn）")]
        public void clientComplete(string mobile, string sn)
        {
            SqlConnection conn = new SqlConnection(connstr);

            string updateSql = "update xx_jichuxinxi_huozhuxinxi set SN = '" + sn + "' where dianhua = '" + mobile + "'";
            SqlCommand command = new SqlCommand(updateSql, conn);

            try
            {
                conn.Open();
                command.BeginExecuteNonQuery();
                conn.Close();
                Context.Response.Write("{\"code\":\"200\",\"msg\":\"货主绑定成功，等待审核\"}");
            }
            catch (Exception)
            {
                Context.Response.Write("{\"code\":\"1002\",\"msg\":\"货主绑定失败\"}");
            }
        }

        [WebMethod(Description = "获取客户帐户余额")]
        public void getBalance(string clientName) {
            SqlConnection conn = new SqlConnection(connstr);

            string querySql = "select yue from xx_jichuxinxi_huozhuxinxi where Company = '"+clientName+"'";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(querySql, conn);
            sqlDataAdapter.SelectCommand = command;
            DataSet ds = new DataSet();
            string ret = "";
            try
            {
                conn.Open();
                sqlDataAdapter.SelectCommand.BeginExecuteNonQuery();
                conn.Close();
                sqlDataAdapter.Fill(ds);
                
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ret = JsonUtil.ToJson(ds.Tables[0], "data", "200", "余额信息获取成功");
                }
                else {
                    ret = "{\"code\":\"-1\",\"msg\":\"余额信息获取失败\"}";
                }
            }
            catch (Exception)
            {
                ret = "{\"code\":\"404\",\"msg\":\"请求错误\"}";
            }
            Context.Response.Write(ret);
        }

        [WebMethod(Description = "获取排队信息")]
        public void getCarList(string clientName) {
            SqlConnection conn = new SqlConnection(connstr);

            string querySql = "select * from av_bangdanxinxi_paiduichaxun where 收货单位 = '" + clientName + "' order by 排队编码 desc";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(querySql, conn);
            sqlDataAdapter.SelectCommand = command;
            DataSet ds = new DataSet();
            string ret = "";
            try
            {
                conn.Open();
                sqlDataAdapter.SelectCommand.BeginExecuteNonQuery();
                conn.Close();
                sqlDataAdapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ret = JsonUtil.ToJson(ds.Tables[0], "data", "200", "排队信息拉取成功");
                }
                else
                {
                    ret = "{\"code\":\"-1\",\"msg\":\"排队信息拉取失败\"}";
                }
            }
            catch (Exception)
            {
                ret = "{\"code\":\"404\",\"msg\":\"请求错误\"}";
            }
            Context.Response.Write(ret);
        }

        [WebMethod(Description = "获取车辆拉灰详情(分页)")]
        public void getBangdanInfoList(int pageNum,int pageSize,string clientName,string license) {
            int totalCount = pageNum * pageSize;
            int startCount = (pageNum - 1) * pageSize;

            string querySql = "SELECT * FROM(SELECT TOP "+totalCount+" ROW_NUMBER() OVER(ORDER BY number desc) AS ROWID,* FROM pd_xitong_bangdanxinxi where huozhu_name ='"+clientName+"' and label_chepai='"+license+"') as temp1 WHERE ROWID>"+startCount;

            SqlConnection conn = new SqlConnection(connstr);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(querySql,conn);
            sqlDataAdapter.SelectCommand = command;
            DataSet ds = new DataSet();
            string ret = "";
            try
            {
                conn.Open();
                sqlDataAdapter.SelectCommand.BeginExecuteNonQuery();
                conn.Close();
                sqlDataAdapter.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ret = JsonUtil.ToJson(ds.Tables[0], "data", "200", "拉灰信息获取成功");
                }
                else
                {
                    if (pageNum == 1)
                    {
                        ret = "{\"code\":\"-1\",\"msg\":\"尚无拉灰信息\"}";
                    }
                    else {
                        ret = "{\"code\":\"-1\",\"msg\":\"已无更多拉灰信息\"}";
                    }
                    
                }
            }
            catch (Exception)
            {
                ret = "{\"code\":\"404\",\"msg\":\"请求错误\"}";
            }
            //Context.Response.Write(querySql);
            Context.Response.Write(ret);
        }

        [WebMethod(Description = "查询榜单(时间)")]
        public void getBangdanHistory(int pageNum,int pageSize,string startTime,string endTime,string clientName) {

            int totalCount = pageNum * pageSize;
            int startCount = (pageNum - 1) * pageSize;
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from (select top "+totalCount+ " ROW_NUMBER() over(order by pd_time) as rowid,* from pd_xitong_bangdanxinxi where  huozhu_name = '" + clientName + "'");
            if (!string.IsNullOrEmpty(startTime)) {
                sb.Append(" and pd_time > '" + startTime + "'");
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                sb.Append(" and pd_time < '" + endTime + "'");
            }
            sb.Append(") as temp1 where rowid>" + startCount);
            string querySql = sb.ToString();

            SqlConnection conn = new SqlConnection(connstr);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(querySql, conn);
            sqlDataAdapter.SelectCommand = command;
            DataSet ds = new DataSet();

            string ret = "";
            try
            {
                conn.Open();
                sqlDataAdapter.SelectCommand.BeginExecuteNonQuery();
                conn.Close();
                sqlDataAdapter.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ret = JsonUtil.ToJson(ds.Tables[0], "data", "200", "榜单信息获取成功");
                }
                else
                {
                    if (pageNum == 1)
                    {
                        ret = "{\"code\":\"-1\",\"msg\":\"无榜单信息\"}";
                    }
                    else
                    {
                        ret = "{\"code\":\"-1\",\"msg\":\"已无更多榜单信息\"}";
                    }

                }
            }
            catch (Exception)
            {
                ret = "{\"code\":\"404\",\"msg\":\"请求错误\"}";
            }
            Context.Response.Write(ret);
        }

        [WebMethod(Description = "获取榜单详情")]
        public void getBangdanDetail(string number) {
            string querySql = "select * from pd_xitong_bangdanxinxi where number = '"+number+"'";
            SqlConnection conn = new SqlConnection(connstr);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(querySql, conn);
            sqlDataAdapter.SelectCommand = command;
            DataSet ds = new DataSet();

            string ret = "";
            try
            {
                conn.Open();
                sqlDataAdapter.SelectCommand.BeginExecuteNonQuery();
                conn.Close();
                sqlDataAdapter.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ret = JsonUtil.ToJson(ds.Tables[0], "data", "200", "榜单信息获取成功");
                }
                else
                {
                    ret = "{\"code\":\"-1\",\"msg\":\"榜单信息获取失败\"}";
                }
            }
            catch (Exception)
            {
                ret = "{\"code\":\"404\",\"msg\":\"请求错误\"}";
            }
            Context.Response.Write(ret);
        }
    }
}
