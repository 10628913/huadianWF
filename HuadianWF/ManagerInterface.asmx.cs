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
    /// ManagerInterface 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ManagerInterface : System.Web.Services.WebService
    {
        public static string connstr = ConfigurationManager.ConnectionStrings["connectStr"].ConnectionString;


        [WebMethod(Description = "管理员登录")]
        public void managerLogin(string username,string password)
        {
            string ret = "";
            SqlConnection conn = new SqlConnection(connstr);
            string queryUsernameSql = "select * from xx_xitong_user where name = '"+username+"'";
            SqlCommand command = new SqlCommand(queryUsernameSql,conn);
            SqlDataAdapter sda = new SqlDataAdapter();
            DataSet ds = new DataSet();
            sda.SelectCommand = command;
            try
            {
                conn.Open();
                sda.SelectCommand.BeginExecuteNonQuery();
                conn.Close();
                sda.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds = new DataSet();
                    string querySql = "select * from xx_xitong_user where name = '" + username + "' and password = '" + password + "'";
                    command = new SqlCommand(querySql, conn);
                    sda.SelectCommand = command;
                    conn.Open();
                    sda.SelectCommand.BeginExecuteNonQuery();
                    conn.Close();
                    sda.Fill(ds);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ret = JsonUtil.ToJson(ds.Tables[0], "data", "200", "登录成功");
                        Context.Response.Write(ret);
                        return;
                    }
                    else
                    {
                        Context.Response.Write("{\"code\":\"-1\",\"msg\":\"密码错误\"}");
                        return;
                    }
                }
                else
                {
                    Context.Response.Write("{\"code\":\"-1\",\"msg\":\"用户名不存在\"}");
                    return;
                }
            }
            catch (Exception)
            {
                ret = "{\"code\":\"404\",\"msg\":\"请求错误\"}";
                Context.Response.Write(ret);
                return;
            }
            
        }

        [WebMethod(Description = "获取管理端菜单别表")]
        public void getMenuList()
        {
            Context.Response.Write("获取管理端菜单别表");
        }

        [WebMethod(Description = "获取灰口排队信息")]
        public void getGreyCastPaiduiInfo() {
            string ret = "";
            SqlConnection conn = new SqlConnection(connstr);
            string querySql = "select COUNT(*) as paiduiNum,灰口名称 as greyCastName from av_bangdanxinxi_paiduichaxun group by 灰口名称";
            SqlCommand command = new SqlCommand(querySql, conn);
            SqlDataAdapter sda = new SqlDataAdapter();
            DataSet ds = new DataSet();
            sda.SelectCommand = command;
            try
            {
                conn.Open();
                sda.SelectCommand.BeginExecuteNonQuery();
                conn.Close();
                sda.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ret = JsonUtil.ToJson(ds.Tables[0], "data", "200", "灰口信息获取成功");
                }
                else {
                    ret = "{\"code\":\"-1\",\"msg\":\"灰口信息获取失败\"}";
                }
            }
            catch (Exception)
            {
                ret = "{\"code\":\"404\",\"msg\":\"请求错误\"}"; ;
            }
            Context.Response.Write(ret);
        }

        [WebMethod(Description = "获取灰口排队详情")]
        public void getGreyCastPaiduiDetail(string greyCastName) {
            string ret = "";
            SqlConnection conn = new SqlConnection(connstr);
            string querySql = "select * from av_bangdanxinxi_paiduichaxun where 灰口名称 = '"+greyCastName+"' order by 排队编码 asc";
            SqlCommand command = new SqlCommand(querySql, conn);
            SqlDataAdapter sda = new SqlDataAdapter();
            DataSet ds = new DataSet();
            sda.SelectCommand = command;
            try
            {
                conn.Open();
                sda.SelectCommand.BeginExecuteNonQuery();
                conn.Close();
                sda.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ret = JsonUtil.ToJson(ds.Tables[0], "data", "200", "灰口排队信息获取成功");
                }
                else
                {
                    ret = "{\"code\":\"-1\",\"msg\":\"灰口排队信息获取失败\"}";
                }
            }
            catch (Exception)
            {
                ret = "{\"code\":\"404\",\"msg\":\"请求错误\"}"; ;
            }
            Context.Response.Write(ret);

        }
        [WebMethod(Description = "获取灰口列表")]
       
        public void getGreyCastList() {
            string ret = "";
            SqlConnection conn = new SqlConnection(connstr);
            string querySql = "select number,name,label_Type,huikou_name,shifoupaidui,shifoukeyipaidui from xx_jichuxinxi_huixing order by name asc";
            SqlCommand command = new SqlCommand(querySql, conn);
            SqlDataAdapter sda = new SqlDataAdapter();
            DataSet ds = new DataSet();
            sda.SelectCommand = command;
            try
            {
                conn.Open();
                sda.SelectCommand.BeginExecuteNonQuery();
                conn.Close();
                sda.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ret = JsonUtil.ToJson(ds.Tables[0], "data", "200", "灰口列表拉取成功");
                }
                else
                {
                    ret = "{\"code\":\"-1\",\"msg\":\"暂无灰口信息\"}";
                }
            }
            catch (Exception)
            {
                ret = "{\"code\":\"404\",\"msg\":\"请求错误\"}"; ;
            }
            Context.Response.Write(ret);
        }

        [WebMethod(Description = "获取灰口详情")]
        public void getGreyCastDetail(int number) {
            string ret = "";
            SqlConnection conn = new SqlConnection(connstr);
            string querySql = "select * from xx_jichuxinxi_huixing where number = '"+number+"'";
            SqlCommand command = new SqlCommand(querySql, conn);
            SqlDataAdapter sda = new SqlDataAdapter();
            DataSet ds = new DataSet();
            sda.SelectCommand = command;
            try
            {
                conn.Open();
                sda.SelectCommand.BeginExecuteNonQuery();
                conn.Close();
                sda.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ret = JsonUtil.ToJson(ds.Tables[0], "data", "200", "灰口详情获取成功");
                }
                else
                {
                    ret = "{\"code\":\"-1\",\"msg\":\"灰口详情获取失败\"}";
                }
            }
            catch (Exception)
            {
                ret = "{\"code\":\"404\",\"msg\":\"请求错误\"}"; ;
            }
            Context.Response.Write(ret);
        }

        [WebMethod(Description = "修改灰口信息")]
        ///<summary></summary>
        ///<param name="number">编号(查询条件)</param>
        ///<param name="greyCastNum">灰口编号</param>
        ///<param name="greyCastName">灰口名称</param>
        ///<param name="kongzhiqiNum">控制器门号</param>
        ///<param name="labelType">卡片类型</param>
        ///<param name="max">最大上限</param>
        ///<param name="min">最小下限</param>
        ///<param name="shifoupaidui">灰口类型</param>
        ///<param name="shifoukeyipadui">是否可以排队</param>
        ///<param name="shifoujinchang">是否可以进场</param>
        public void updateGreyCastInfo(int number,string greyCastNum, string greyCastName, string kongzhiqiNum,
            string labelType, string max, string min, string shifoupaidui, string shifoukeyipaidui, string shifoujinchang)
        {


            StringBuilder sb = new StringBuilder();
            sb.Append("update xx_jichuxinxi_huixing set ");
            StringBuilder setSql = new StringBuilder();
            if (!string.IsNullOrEmpty(greyCastNum)) {
                setSql.Append(",name='"+ greyCastNum + "'");
            }
            if (!string.IsNullOrEmpty(greyCastName)) {
                setSql.Append(",huikou_name='"+greyCastName+"'");
            }
            if (!string.IsNullOrEmpty(kongzhiqiNum))
            {
                setSql.Append(",kongzhiqi_num='" + kongzhiqiNum + "'");
            }
            if (!string.IsNullOrEmpty(labelType))
            {
                setSql.Append(",label_Type='" + labelType + "'");
            }
            if (!string.IsNullOrEmpty(max))
            {
                setSql.Append(",max='" + Convert.ToDecimal(max) + "'");
            }
            if (!string.IsNullOrEmpty(min))
            {
                setSql.Append(",min='" + Convert.ToDecimal(min) + "'");
            }
            if (!string.IsNullOrEmpty(shifoupaidui))
            {
                setSql.Append(",shifoupaidui='" + shifoupaidui + "'");
            }
            if (!string.IsNullOrEmpty(shifoukeyipaidui))
            {
                setSql.Append(",shifoukeyipaidui='" + shifoukeyipaidui + "'");
            }
            if (!string.IsNullOrEmpty(shifoujinchang))
            {
                setSql.Append(",shifoujinchang='" + shifoujinchang + "'");
            }
            if (setSql == null || setSql.Length == 0)
            {
                Context.Response.Write("{\"code\":\"-1\",\"msg\":\"无更改\"}");
                return;
            }
            else {
                string str = setSql.ToString().Substring(1);
                sb.Append(str);
            }
            sb.Append(" where number = '"+number+"'");
            //Context.Response.Write(sb.ToString());
            //sql 拼接结束
            SqlConnection conn = new SqlConnection(connstr);
            SqlCommand command = new SqlCommand(sb.ToString(), conn);
            string ret = "";
            try
            {
                conn.Open();
                int num = Convert.ToInt32(command.ExecuteNonQuery());
                if (num > 0)
                {
                    Context.Response.Write("{\"code\":\"200\",\"msg\":\"灰口信息修改成功\"}");
                }
                else
                {
                    Context.Response.Write("{\"code\":\"-1\",\"msg\":\"灰口信息修改失败\"}");
                }
                conn.Close();
                return;
            }
            catch (Exception)
            {
                ret = "{\"code\":\"-1\",\"msg\":\"灰口信息修改失败\"}";
                Context.Response.Write(ret);
            }
        }

        [WebMethod(Description = "查询榜单(时间)")]
        public void getBangdanHistory(int pageNum, int pageSize, string startTime, string endTime)
        {

            int totalCount = pageNum * pageSize;
            int startCount = (pageNum - 1) * pageSize;
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from (select top " + totalCount + " ROW_NUMBER() over(order by pd_time) as rowid,* from pd_xitong_bangdanxinxi where 1=1");
            if (!string.IsNullOrEmpty(startTime))
            {
                sb.Append(" and pd_time > '" + startTime + "'");
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                sb.Append(" and pd_time < '" + endTime + "'");
            }
            sb.Append(") as temp1 where rowid>" + startCount);
            string querySql = sb.ToString();
            //Context.Response.Write(querySql);
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
        public void getBangdanDetail(string number)
        {
            string querySql = "select * from pd_xitong_bangdanxinxi where number = '" + number + "'";
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

        [WebMethod(Description = "发布系统消息")]
        public void pubMessage(int uid,string title,string content) {
            string insertSql = "insert into xx_xitongxiaoxi (uid,message_title,message_content,message_addtime) values ('"+uid+"','"+title+"','"+title+"','"+DateTime.Now+"')";
            SqlConnection conn = new SqlConnection(connstr);
            SqlCommand command = new SqlCommand(insertSql, conn);
            try
            {
                conn.Open();
                int num = Convert.ToInt32(command.ExecuteNonQuery());
                string ret = "";
                if (num > 0)
                {
                    ret = "{\"code\":\"200\",\"msg\":\"消息发布成功\"}";
                }
                else {
                    ret = "{\"code\":\"-1\",\"msg\":\"消息发布失败\"}";
                }
                conn.Close();
                Context.Response.Write(ret);
            }
            catch (Exception)
            {
                Context.Response.Write("{\"code\":\"404\",\"msg\":\"请求错误\"}");
            }
        }
        [WebMethod(Description = "删除系统消息")]
        public void delMessage(int id)
        {
            string delSql = "delete from xx_xitongxiaoxi where id = '"+id+"' ";
            SqlConnection conn = new SqlConnection(connstr);
            SqlCommand command = new SqlCommand(delSql, conn);
            try
            {
                conn.Open();
                int num = command.ExecuteNonQuery();
                if (num > 0)
                {
                    Context.Response.Write("{\"code\":\"200\",\"msg\":\"操作成功\"}");
                }
                else {
                    Context.Response.Write("{\"code\":\"-1\",\"msg\":\"操作失败\"}");
                }
                conn.Close();
                
            }
            catch (Exception)
            {
                Context.Response.Write("{\"code\":\"404\",\"msg\":\"请求错误\"}");
            }
        }
        [WebMethod(Description = "获取可挂起的车辆")]
        public void getKeGuaqiList(int pageNum,int pageSize) {
            int totalCount = pageNum * pageSize;
            int startCount = (pageNum - 1) * pageSize;

            string sql = "select number,label_siji,label_chepai,status,huixing_name,huozhu_name from (SELECT TOP " + totalCount + " ROW_NUMBER() OVER(ORDER BY number desc) AS ROWID,* FROM pd_xitong_bangdanxinxi where status != '完成' and status != '重新排队') as temp1 where ROWID > " + startCount;
            SqlConnection conn = new SqlConnection(connstr);
            SqlDataAdapter sda = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(sql, conn);
            sda.SelectCommand = command;
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                sda.SelectCommand.BeginExecuteNonQuery();
                conn.Close();
                sda.Fill(ds);
                string ret = "";
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ret = JsonUtil.ToJson(ds.Tables[0], "data", "200", "可挂起榜单列表拉取成功");
                }
                else
                {
                    if (pageNum == 1)
                    {
                        ret = "{\"code\":\"-1\",\"msg\":\"暂无可挂起榜单信息\"}";
                    }
                    else {
                        ret = "{\"code\":\"-1\",\"msg\":\"已无更多可挂起榜单信息\"}";
                    }
                }
                Context.Response.Write(ret);
            }
            catch (Exception)
            {
                Context.Response.Write("{\"code\":\"404\",\"msg\":\"请求错误\"}");
            }
        }
        [WebMethod(Description = "获取可解挂的车辆")]
        public void getKeJieguaList(int pageNum, int pageSize)
        {
            int totalCount = pageNum * pageSize;
            int startCount = (pageNum - 1) * pageSize;

            string sql = "select number,label_siji,label_chepai,status,huixing_name,huozhu_name from (SELECT TOP " + totalCount + " ROW_NUMBER() OVER(ORDER BY number desc) AS ROWID,* FROM pd_xitong_bangdanxinxi where status = '挂起') as temp1 where ROWID > " + startCount;
            SqlConnection conn = new SqlConnection(connstr);
            SqlDataAdapter sda = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(sql, conn);
            sda.SelectCommand = command;
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                sda.SelectCommand.BeginExecuteNonQuery();
                conn.Close();
                sda.Fill(ds);
                string ret = "";
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ret = JsonUtil.ToJson(ds.Tables[0], "data", "200", "可解挂榜单列表拉取成功");
                }
                else
                {
                    if (pageNum == 1)
                    {
                        ret = "{\"code\":\"-1\",\"msg\":\"暂无可解挂榜单信息\"}";
                    }
                    else
                    {
                        ret = "{\"code\":\"-1\",\"msg\":\"已无更多可解挂榜单信息\"}";
                    }
                }
                Context.Response.Write(ret);
            }
            catch (Exception)
            {
                Context.Response.Write("{\"code\":\"404\",\"msg\":\"请求错误\"}");
            }
        }
        [WebMethod(Description = "车辆榜单挂起")]
        public void bangdanGuaqi(int number) {
            string sql = "update pd_xitong_bangdanxinxi set guaqiqian_status = status,status = '挂起',gq_time1 = '"+DateTime.Now+"' where number = "+number;
            SqlConnection conn = new SqlConnection(connstr);
            SqlCommand command = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                int num = command.ExecuteNonQuery();
                if (num > 0)
                {
                    Context.Response.Write("{\"code\":\"200\",\"msg\":\"榜单挂起成功\"}");
                }
                else
                {
                    Context.Response.Write("{\"code\":\"-1\",\"msg\":\"榜单挂起失败\"}");
                }
                conn.Close();

            }
            catch (Exception)
            {
                Context.Response.Write("{\"code\":\"404\",\"msg\":\"请求错误\"}");
            }
        }

        [WebMethod(Description = "车辆榜单解挂")]
        ///<summary></summary>
        ///<param name="type">1:解挂到挂起前状态2:解挂为重新排队</param>
        public void bangdanJiegua(int number,int type)
        {
            string sql = "update pd_xitong_bangdanxinxi set gq_time2 = '"+DateTime.Now+"'";
            switch (type)
            {
                case 1:
                    sql += ",status = guaqiqian_status";
                    break;
                case 2:
                    sql += ",status = '重新排队'";
                    break;
            }
            sql += " where number = " + number;
            SqlConnection conn = new SqlConnection(connstr);
            SqlCommand command = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                int num = command.ExecuteNonQuery();
                if (num > 0)
                {
                    Context.Response.Write("{\"code\":\"200\",\"msg\":\"榜单解挂成功\"}");
                }
                else
                {
                    Context.Response.Write("{\"code\":\"-1\",\"msg\":\"榜单解挂失败\"}");
                }
                conn.Close();

            }
            catch (Exception)
            {
                Context.Response.Write("{\"code\":\"404\",\"msg\":\"请求错误\"}");
            }
        }
        [WebMethod(Description = "获取车辆信息")]
        ///<summary></summary>
        ///<param name="type">1:非黑名单车辆2:黑名单车辆</param>
        public void getNolockCarList(int pageNum,int pageSize,int type) {
            int totalCount = pageNum * pageSize;
            int startCount = (pageNum - 1) * pageSize;
            string str = "";
            if (type == 1)
            {
                str = "where status != '黑名单'";
            }
            else {
                str = "where status = '黑名单'";
            }
            string sql = "select number,chepai,siji,dianhua,label_num,status from (select TOP "+totalCount+ " ROW_NUMBER() OVER(ORDER BY number asc) AS ROWID,* FROM xx_yonghu_cheliangxinxi "+ str + ") as temp1 where ROWID > " + startCount;
            SqlConnection conn = new SqlConnection(connstr);
            SqlDataAdapter sda = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(sql,conn);
            sda.SelectCommand = command;
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                sda.SelectCommand.BeginExecuteNonQuery();
                conn.Close();
                sda.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Context.Response.Write(JsonUtil.ToJson(ds.Tables[0], "data", "200", "车辆列表拉取成功"));
                }
                else
                {
                    if (pageNum == 1)
                    {
                        Context.Response.Write("{\"code\":\"-1\",\"msg\":\"尚无车辆信息\"}");
                    }
                    else {
                        Context.Response.Write("{\"code\":\"-1\",\"msg\":\"已无更多车辆信息\"}");
                    }
                    
                }
            }
            catch (Exception)
            {
                Context.Response.Write("{\"code\":\"404\",\"msg\":\"请求错误\"}");
            }

        }

        [WebMethod(Description = "车辆拉黑/解除拉黑")]
        ///<summary></summary>
        ///<param name="type">1:拉黑:解除拉黑</param>
        public void editCarStatus(int number,int type) {
            string status = "";
            if (type == 1) {
                status = "黑名单";
            } else {
                status = "正常";
            }
            string sql = "update xx_yonghu_cheliangxinxi set status = '" + status + "' where number = '" + number + "'";
            SqlConnection conn = new SqlConnection(connstr);
            SqlCommand command = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                int num = command.ExecuteNonQuery();
                conn.Close();
                if (num > 0)
                {
                    Context.Response.Write("{\"code\":\"200\",\"msg\":\"操作成功\"}");
                }
                else
                {
                    Context.Response.Write("{\"code\":\"-1\",\"msg\":\"操作失败\"}");
                }
                return;
            }
            catch (Exception)
            {
                Context.Response.Write("{\"code\":\"404\",\"msg\":\"请求错误\"}");
            }
        }
        [WebMethod(Description = "获取货主列表")]
        ///<summary></summary>
        ///<param name="type">1:未冻结2:已冻结</param>
        public void getClientList(int pageNum,int pageSize,int type) {
            int totalCount = pageNum * pageSize;
            int startCount = (pageNum - 1) * pageSize;
            string str = "";
            if (type == 1)
            {
                str = "where status = '正常'";
            }
            else
            {
                str = "where status = '冻结'";
            }
            string sql = "select number,Company,bieming,dianhua,type,status from (select top " + totalCount + " ROW_NUMBER() OVER(ORDER BY number asc) AS ROWID,* FROM xx_jichuxinxi_huozhuxinxi " + str + ") as temp1 where ROWID > " + startCount;
            SqlConnection conn = new SqlConnection(connstr);
            SqlDataAdapter sda = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(sql, conn);
            sda.SelectCommand = command;
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                sda.SelectCommand.BeginExecuteNonQuery();
                conn.Close();
                sda.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Context.Response.Write(JsonUtil.ToJson(ds.Tables[0], "data", "200", "货主列表拉取成功"));
                }
                else
                {
                    if (pageNum == 1)
                    {
                        Context.Response.Write("{\"code\":\"-1\",\"msg\":\"尚无货主信息\"}");
                    }
                    else
                    {
                        Context.Response.Write("{\"code\":\"-1\",\"msg\":\"已无更多货主信息\"}");
                    }

                }
            }
            catch (Exception)
            {
                Context.Response.Write("{\"code\":\"404\",\"msg\":\"请求错误\"}");
            }

        }
        [WebMethod(Description = "货主冻结/解冻")]
        ///<summary></summary>
        ///<param name="type">1:冻结2:解冻</param>
        public void editClientStatus(int number,int type) {
            string status = "";
            if (type == 1)
            {
                status = "冻结";
            }
            else
            {
                status = "正常";
            }
            string sql = "update xx_jichuxinxi_huozhuxinxi set status = '" + status + "' where number = '" + number + "'";
            SqlConnection conn = new SqlConnection(connstr);
            SqlCommand command = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                int num = command.ExecuteNonQuery();
                conn.Close();
                if (num > 0)
                {
                    Context.Response.Write("{\"code\":\"200\",\"msg\":\"操作成功\"}");
                }
                else
                {
                    Context.Response.Write("{\"code\":\"-1\",\"msg\":\"操作失败\"}");
                }
                return;
            }
            catch (Exception)
            {
                Context.Response.Write("{\"code\":\"404\",\"msg\":\"请求错误\"}");
            }
        }
    }
}
