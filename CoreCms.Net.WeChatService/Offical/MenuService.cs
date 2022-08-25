using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoreCms.Net.WeChatService.Offical
{


    /// <summary>
    ///微信公众号底部菜单设置
    /// </summary>
    public class MenuService
    {

    //    /// <summary>
    //    // 创建菜单
    //    /// </summary>
    //    /// <param name="json"></param>
    //    /// <returns></returns>
    //    public string createMenu(string json) 
    //    {

    //        //1、将创建菜单的JSON数据保存到menu.txt了，也可以直接将JSON写成一个string串
    //        FileStream fs1 = new FileStream(Server.MapPath("~/") + "\\App_Data\\menu.txt", FileMode.Open);
    //        StreamReader sr = new StreamReader(fs1, Encoding.GetEncoding("GBK"));
    //        string menu = sr.ReadToEnd();
    //        sr.Close();
    //        fs1.Close();

    //        //2、获取我的访问令牌
    //        string my_token = getMyWeChatToken();

    //        //3、组合成创建菜单的URL
    //        string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", my_token);

    //        //4、调用创建菜单的方法，并返回结果
    //        return createMenu(url, menu);


    //    }

    //     //获取访问令牌
    //    public string getMyWeChatToken()
    //    {
    //        if (HttpContext.Cache["access_token"] == null)
    //        {
    //            string urltemp = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
				
				////需要填写自己的APPID和APPSECRET
    //            string url = string.Format(urltemp, "1111111", "1111111111");

    //            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
    //            request.Method = "GET";
    //            request.ContentType = "application/x-www-form-urlencoded";//链接类型
    //            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

    //            string access_token = "";
    //            using (Stream s = response.GetResponseStream())
    //            {
    //                StreamReader reader = new StreamReader(s, Encoding.UTF8);
    //                access_token= reader.ReadToEnd();
    //            }


    //            JObject my_toke_obj =(JObject) JsonConvert.DeserializeObject(access_token);

    //            string my_token = my_toke_obj["access_token"].ToString();
            
    //            //将访问令牌存储到Cache中
    //            Cache cache = HttpContext.Cache;
    //            cache.Insert("access_token", my_token, null, DateTime.Now.AddSeconds(7000), Cache.NoSlidingExpiration);

    //            return my_toke_obj["access_token"].ToString();
    //        }
    //        else
    //        {
    //            return HttpContext.Cache["access_token"].ToString();
    //        }
    //    }
    //    private string createMenu(string url, string menu_txt)
    //    {
    //        Stream outstream = null;
    //        Stream instream = null;
    //        StreamReader sr = null;
    //        HttpWebResponse response = null;
    //        HttpWebRequest request = null;
    //        Encoding encoding = Encoding.UTF8;
    //        byte[] data = encoding.GetBytes(menu_txt);

    //        request = WebRequest.Create(url) as HttpWebRequest;//创建请求

    //        //写入数据
    //        CookieContainer cookieContainer = new CookieContainer();
    //        request.CookieContainer = cookieContainer;
    //        request.AllowAutoRedirect = true;
    //        request.Method = "POST";
    //        request.ContentType = "application/x-www-form-urlencoded";
    //        request.ContentLength = data.Length;
    //        outstream = request.GetRequestStream();
    //        outstream.Write(data, 0, data.Length);
    //        outstream.Close();

    //        //读取返回结果
    //        response = request.GetResponse() as HttpWebResponse;
    //        instream = response.GetResponseStream();
    //        sr = new StreamReader(instream, encoding);
    //        string content = sr.ReadToEnd();

    //        //读取操作码
    //        JObject my_toke_obj = (JObject)JsonConvert.DeserializeObject(content);
    //        string error_code = my_toke_obj["errcode"].ToString();

    //        return error_code;
    //    }

    }
}
