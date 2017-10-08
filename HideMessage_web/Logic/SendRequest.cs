using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using HideMessage_web.Data;
using HideMessage_web.Models;
using Newtonsoft.Json.Linq;

namespace HideMessage_web.Logic
{
    public class SendRequest
    {
        string appkey = "fvc9hPAZXo8wmYyFvv7Zl7";
        string mastersecret = "WTP7tt51ZVAsI8X0O3v745";
        string appid = "p2ams8icLC99LgUCZQYHt2";

        public SendRequest(Messages message)
        {
			using (var context = new DataContext())
			{
				Devices device = context.Devices.FirstOrDefault();
                if (device == null) {
					message.message_request_time = DateTime.Now;
					message.phone_state = "分配设备失败";
					context.Messages.Update(message);
					context.SaveChanges();
                    return;
                }
				message.message_request_time = DateTime.Now;
				message.phone_state = "向设备发送请求中";
				message.send_phone_id = device.device_id;
				context.Messages.Update(message);
				context.SaveChanges();
                
                string token = GetAuthToken();
                if(token == "") {
                    message.phone_state = "分配设备时验证失败";
                    message.error_message = "web get token equals null";
					context.Messages.Update(message);
					context.SaveChanges();
                    return;
                } else {
                    JObject tokenJson = JObject.Parse(token);
                    string tokenString = tokenJson["result"].ToString();
                    if (!tokenString.Equals("ok")) {
						message.phone_state = "请求验证时出错";
						message.error_message = token;
						context.Messages.Update(message);
						context.SaveChanges();
                        return;
                    } else {
                        token = tokenJson["auth_token"].ToString();
                    }
                }

                string result = SendMessageRequest(message, device, token);
				if (result == "")
				{
					message.phone_state = "分配设备时请求失败";
                    message.error_message = "web get send result equals null, token is " + token;
					context.Messages.Update(message);
					context.SaveChanges();
					return;
				}
				else
				{
					JObject resultJson = JObject.Parse(result);
					string resultString = resultJson["result"].ToString();
					if (!resultString.Equals("ok"))
					{
						message.phone_state = "设备请求时出错";
						message.error_message = result;
						context.Messages.Update(message);
						context.SaveChanges();
						return;
					}
					else
					{
						result = resultJson["taskid"].ToString();
						message.phone_state = "等待设备反馈";
                        message.task_id = result;
						context.Messages.Update(message);
						context.SaveChanges();
					}
				}
			}
        }

        public string SendMessageRequest(Messages message, Devices device, string token) {
			string url = "https://restapi.getui.com/v1/" + appid + "/push_single";
			JObject data = new JObject();
            data["cid"] = device.device_client_id;
            data["requestid"] = appid + message.message_id.ToString();
			JObject dataMessage = new JObject();
            dataMessage["appkey"] = appkey;
            dataMessage["is_offline"] = false;
            dataMessage["msgtype"] = "transmission";
            data["message"] = dataMessage;
			JObject dataTransmission = new JObject();
            dataTransmission["transmission_content"] = message.message_phone_number + "," + message.message_id;
			data["transmission"] = dataTransmission;
            string result = HttpPost(url, data.ToString(), "authtoken:"+token);
			return result;
        }

        public string GetAuthToken() {
            string url = "https://restapi.getui.com/v1/" + appid + "/auth_sign";
            string timestamp = ConvertToTimeStamp(DateTime.Now).ToString();
            string sign = GetSHA256hash(appkey + timestamp + mastersecret);
            JObject data = new JObject();
            data["sign"] = sign;
            data["timestamp"] = timestamp;
            data["appkey"] = appkey;
            string token = HttpPost(url, data.ToString());
            return token;
        }

		string HttpPost(string url, string data, string header = "")
		{
			try
			{
				HttpWebRequest req = WebRequest.CreateHttp(new Uri(url));
                if (!header.Equals("")) {
                    req.Headers.Add(header);
                }
                req.ContentType = "application/json;charset=utf-8";

				req.Method = "POST";
				req.ContinueTimeout = 60000;

                Encoding encoding = Encoding.UTF8;
				byte[] postData = encoding.GetBytes(data);
				Stream reqStream = req.GetRequestStreamAsync().Result;
				reqStream.Write(postData, 0, postData.Length);
				reqStream.Dispose();

				var rsp = (HttpWebResponse)req.GetResponseAsync().Result;
				var result = GetResponseAsString(rsp, encoding);
				return result;

			}
			catch (Exception ex)
			{
                JObject error = new JObject();
                error["result"] = "error";
                error["error"] = ex.Message;
                return error.ToString();
			}
		}

	    string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
		{
			Stream stream = null;
			StreamReader reader = null;

			try
			{
				// 以字符流的方式读取HTTP响应
				stream = rsp.GetResponseStream();
				reader = new StreamReader(stream, encoding);
				return reader.ReadToEnd();
			}
			finally
			{
				// 释放资源
				if (reader != null) reader.Dispose();
				if (stream != null) stream.Dispose();
				if (rsp != null) rsp.Dispose();
			}
		}

		public static long ConvertToTimeStamp(DateTime time)
        {
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long) (time.AddHours(-8) - Jan1st1970).TotalMilliseconds;
        }

		public string GetSHA256hash(string input)
		{
			byte[] clearBytes = Encoding.UTF8.GetBytes(input);
			SHA256 sha256 = new SHA256Managed();
			sha256.ComputeHash(clearBytes);
			byte[] hashedBytes = sha256.Hash;
			sha256.Clear();
			string output = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

			return output;
		}
    }
}
