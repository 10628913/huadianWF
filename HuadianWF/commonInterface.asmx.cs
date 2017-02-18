using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace HuadianWF
{
    /// <summary>
    /// commonInterface 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class commonInterface : System.Web.Services.WebService
    {
        public static string connstr = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;

        [WebMethod(Description = "获取消息通知")]
        public string HelloWorld(int pageNum,int pageSize)
        {
            int totalCount = pageNum * pageSize;
            int startCount = (pageNum - 1) * pageSize;
            SqlConnection conn = new SqlConnection(connstr);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            string sql = "SELECT * FROM(SELECT TOP " + totalCount + " ROW_NUMBER() OVER(ORDER BY id DESC) AS ROWID, * FROM xx_xitongxiaoxi) as temp1 WHERE ROWID> " + startCount;
            SqlCommand sqlCommand = new SqlCommand(sql,conn);
            sqlDataAdapter.SelectCommand = sqlCommand;
            conn.Open();
            sqlDataAdapter.SelectCommand.BeginExecuteNonQuery();
            conn.Close();
            DataSet ds = new DataSet();
            sqlDataAdapter.Fill(ds);
            string retStr = "";
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                retStr = JsonUtil.ToJson(ds.Tables[0], "data", "200", "消息通知拉取成功");
            }
            else {
                if (pageNum == 1)
                {
                    retStr = "{\"code\":\"-1\",\"msg\":\"暂无消息通知\"}";
                }
                else {
                    retStr = "{\"code\":\"-1\",\"msg\":\"已无更多消息通知\"}";
                }
            }
            return retStr;
        }
    }
}
