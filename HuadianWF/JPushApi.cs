using cn.jpush.api;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using System;
using System.Configuration;

namespace HuadianWF
{
    public class JPushApi
    {
        //run the DeviceApiExample first,it will add mobile,tags,alias to the device:
        //首先运行DeviceApiExample，为设备添加手机号码，标签别名，再运行JPushApiExample,ScheduleApiExample，步骤如下：
        //1.设置cn.jpush.api.example为启动项
        //2.在cn.jpush.api.example项目，右键选择属性，然后选择应用程序，最后在启动对象下拉框中选择DeviceApiExample
        //3.按照2的步骤设置，运行JPushApiExample,ScheduleApiExample.

        public static String TITLE = "Test from C# v3 sdk";
        public static String ALERT = "Test from  C# v3 sdk - alert";
        public static String MSG_CONTENT = "Test from C# v3 sdk - msgContent";
        public static String REGISTRATION_ID = "0900e8d85ef";
        public static String SMSMESSAGE = "Test from C# v3 sdk - SMSMESSAGE";
        public static int DELAY_TIME = 1;
        public static String TAG = "tag_api";
        public static String app_key_driver = ConfigurationManager.AppSettings["jush_app_key_driver"];
        public static String master_secret_driver = ConfigurationManager.AppSettings["jpush_master_secret_driver"];
        public static String app_key_client = ConfigurationManager.AppSettings["jush_app_key_client"];
        public static String master_secret_client = ConfigurationManager.AppSettings["jpush_master_secret_client"];

        //public static void pushToDriver() {
        //    JPushClient client = new JPushClient(app_key_driver, master_secret_driver);
        //    PushPayload tags = PushObject_android_and_ios(tags,title,content,extra);
        //}
        public static void pushToDriver1(string number,string title,string content) {
            JPushClient client = new JPushClient(app_key_driver, master_secret_driver);

            PushPayload tags = PushObject_Android_Tag_AlertWithTitle(number, content, title);
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

            }
            catch (APIRequestException e)
            {
                System.Diagnostics.Debug.Write("Error response from JPush server. Should review and fix it. ");
                System.Diagnostics.Debug.Write("HTTP Status: " + e.Status);
                System.Diagnostics.Debug.Write("Error Code: " + e.ErrorCode);
                System.Diagnostics.Debug.Write("Error Message: " + e.ErrorMessage);
            }
            catch (APIConnectionException e)
            {
                System.Diagnostics.Debug.Write(e.Message);
            }

        }
        public static void push(string tag,string alert,string title)
        {
           System.Diagnostics.Debug.Write("*****开始发送******");
            JPushClient client = new JPushClient(app_key_client, master_secret_client);
            /**
            PushPayload payload = PushObject_All_All_Alert();
            try
            {
                var result = client.SendPush(payload);
                //由于统计数据并非非是即时的,所以等待一小段时间再执行下面的获取结果方法
                System.Threading.Thread.Sleep(10000);
                //如需查询上次推送结果执行下面的代码
                var apiResult = client.getReceivedApi(result.msg_id.ToString());
                var apiResultv3 = client.getReceivedApi_v3(result.msg_id.ToString());
                //如需查询某个messageid的推送结果执行下面的代码
                var queryResultWithV2 = client.getReceivedApi("1739302794"); 
                var querResultWithV3 = client.getReceivedApi_v3("1739302794");

            }
            catch (APIRequestException e)
            {
               System.Diagnostics.Debug.Write("Error response from JPush server. Should review and fix it. ");
               System.Diagnostics.Debug.Write("HTTP Status: " + e.Status);
               System.Diagnostics.Debug.Write("Error Code: " + e.ErrorCode);
               System.Diagnostics.Debug.Write("Error Message: " + e.ErrorMessage);
            }
            catch (APIConnectionException e)
            {
               System.Diagnostics.Debug.Write(e.Message);
            }

            //send   smsmessage
            PushPayload pushsms = PushSendSmsMessage();
            try
            {
                var result = client.SendPush(pushsms);
                //由于统计数据并非非是即时的,所以等待一小段时间再执行下面的获取结果方法
                System.Threading.Thread.Sleep(10000);
                //如需查询上次推送结果执行下面的代码
                var apiResult = client.getReceivedApi(result.msg_id.ToString());
                var apiResultv3 = client.getReceivedApi_v3(result.msg_id.ToString());
                //如需查询某个messageid的推送结果执行下面的代码
                var queryResultWithV2 = client.getReceivedApi("1739302794");
                var querResultWithV3 = client.getReceivedApi_v3("1739302794");

            }
            catch (APIRequestException e)
            {
               System.Diagnostics.Debug.Write("Error response from JPush server. Should review and fix it. ");
               System.Diagnostics.Debug.Write("HTTP Status: " + e.Status);
               System.Diagnostics.Debug.Write("Error Code: " + e.ErrorCode);
               System.Diagnostics.Debug.Write("Error Message: " + e.ErrorMessage);
            }
            catch (APIConnectionException e)
            {
               System.Diagnostics.Debug.Write(e.Message);
            }
            */
            PushPayload tags = PushObject_Android_Tag_AlertWithTitle(tag,alert,title);
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

            }
            catch (APIRequestException e)
            {
               System.Diagnostics.Debug.Write("Error response from JPush server. Should review and fix it. ");
               System.Diagnostics.Debug.Write("HTTP Status: " + e.Status);
               System.Diagnostics.Debug.Write("Error Code: " + e.ErrorCode);
               System.Diagnostics.Debug.Write("Error Message: " + e.ErrorMessage);
            }
            catch (APIConnectionException e)
            {
               System.Diagnostics.Debug.Write(e.Message);
            }



           System.Diagnostics.Debug.Write("*****结束发送******");
        }

        public static PushPayload PushObject_All_All_Alert()
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.all();
            pushPayload.audience = Audience.all();
            pushPayload.notification = new Notification().setAlert(ALERT);
            return pushPayload;
        }
        public static PushPayload PushObject_all_alias_alert()
        {

            PushPayload pushPayload_alias = new PushPayload();
            pushPayload_alias.platform = Platform.android();
            pushPayload_alias.audience = Audience.s_alias("alias1");
            pushPayload_alias.notification = new Notification().setAlert(ALERT);
            return pushPayload_alias;
        }
        public static PushPayload PushObject_Android_Tag_AlertWithTitle(string tag,string title,string alert)
        {
            PushPayload pushPayload = new PushPayload();

            pushPayload.platform = Platform.android();
            pushPayload.audience = Audience.s_tag(tag); 
            pushPayload.notification =  Notification.android(alert,title);
            return pushPayload;
        }
        public static PushPayload PushObject_android_and_ios(string tags,string title,string content,string type)
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            var audience = Audience.s_tag(tags);
            pushPayload.audience = audience;
            var notification = new Notification().setAlert(content);
            notification.AndroidNotification = new AndroidNotification().setTitle(title);
            //notification.IosNotification = new IosNotification();
            //notification.IosNotification.incrBadge(1);
            //notification.IosNotification.AddExtra("extra_key", "extra_value");
            notification.AndroidNotification.AddExtra("type",type);

            pushPayload.notification = notification.Check();

            pushPayload.message = Message.content(content).AddExtras("type", type);

            

            return pushPayload;
        }
        public static PushPayload PushObject_ios_tagAnd_alertWithExtrasAndMessage()
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            pushPayload.audience = Audience.s_tag_and("tag1", "tag_all");
            var notification = new Notification();
            notification.IosNotification = new IosNotification().setAlert(ALERT).setBadge(5).setSound("happy").AddExtra("from","JPush");
            notification.AndroidNotification = new AndroidNotification().setAlert("666").setTitle("777");
            pushPayload.notification = notification;
            pushPayload.message = Message.content(MSG_CONTENT);
            return pushPayload;

        }
        public static PushPayload PushObject_ios_audienceMore_messageWithExtras()
        {
            
            var pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            pushPayload.audience = Audience.s_tag("tag1","tag2");
            pushPayload.message = Message.content(MSG_CONTENT).AddExtras("from", "JPush");
            return pushPayload;

        }

        public static PushPayload PushSendSmsMessage()
        {
            var pushPayload = new PushPayload();
            pushPayload.platform = Platform.all();
            pushPayload.audience = Audience.all();
            pushPayload.notification = new Notification().setAlert(ALERT);
            SmsMessage sms_message = new SmsMessage();
            sms_message.setContent(SMSMESSAGE);
            sms_message.setDelayTime(DELAY_TIME);
            pushPayload.sms_message = sms_message;
            return pushPayload;
        }

    }
}
