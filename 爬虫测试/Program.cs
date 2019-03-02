using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;
using System.Drawing;

namespace 爬虫测试
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = "https://music.163.com/song?id=28754102";
            string cookie =
                "_ntes_nnid=bc420d072e9aa6ddc44ed4dd64869e50,1544976169775; _ntes_nuid=bc420d072e9aa6ddc44ed4dd64869e50; vinfo_n_f_l_n3=04b1dc28ff6b9fea.1.0.1544976169790.0.1544976176253; __f_=1545300396140; _iuqxldmzr_=32; WM_TID=YhnWnDkCOuNBBQUBFFI4O6MpWAOQo%2Fv0; __utmz=94650624.1545567173.8.3.utmcsr=baidu|utmccn=(organic)|utmcmd=organic; WM_NI=xX4QIAOUJ52wCR7JLrEC71Rac62sH0ZExiU1karixGnCY%2BCccLTJ7ebFJI7239U3wmt7nEReuo9MKiKIQhw7t7GNCvETMQulX8Ok%2B2y3saE4a5jqijsCDluwZocGwz0FeHo%3D; WM_NIKE=9ca17ae2e6ffcda170e2e6eed7f77082b2868ec948b5eb8eb3d14e829b8fabb76eed948297bc34f3f5868ee62af0fea7c3b92abc88fcb2cc4b8d8b86d6b17d818ef9abcf7a879df9d2d16ba1eafe94cb3c9a9bb88ad83ba9bfbe8afb4dae9d97ccc269a1949cabb66fa29a86aab63af1e79bd4f667b8e9a2a6d16fb7b1f7aeca6389979f89f63db8e989afc27e878c83b2f8689ce8818caa3abbedbd82c840a3eba692ec62b19d8c8df87d9ab1ada8f672f49aaba9ee37e2a3; JSESSIONID-WYYY=EQBqEniDpJNyrqkpFoTUJay1kVmrsoNvcZ4%2F7%2Br42RMId0G866KUCV%5Cvg6PkMlsDdMnZOtgDgipxJGrnpmfRVQ1osJ%2FUtq9Y9o1XWNi0e%2BQYXgpV%2BHcJt3syV7ekb89yJ1mA6Cpy%5ChEYia5zzJuz%2BmD5E6QEmOjjRHrVaV1S3GGd4%2Fmy%3A1545572449994; __utma=94650624.1386255508.1545485792.1545567173.1545570650.9; __utmc=94650624; __utmb=94650624.10.10.1545570650";
            string param = null;
            //HtmlDocument html=new HtmlDocument();
            HttpWebResponse web = SentInfo(source, cookie, param);
            Stream responseStream = web.GetResponseStream();
            if (web.ContentEncoding.ToLower().Contains("gzip"))
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            else if (web.ContentEncoding.ToLower().Contains("deflate"))
                responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
            HtmlDocument html = new HtmlDocument();
            html.Load(responseStream, Encoding.UTF8);
            HtmlNodeCollection datanodes = html.DocumentNode.SelectNodes("//meta[@name='keywords']");
            HtmlNodeCollection imgnodes = html.DocumentNode.SelectNodes("//meta[@property='og:image']");
            string[] songdata = datanodes[0].Attributes["content"].Value.Split(new char[] { '，' });

            string title = songdata[0];
            string album = songdata[1];
            string artist = songdata[2];
            Console.WriteLine("歌手："+artist+"\n专辑："+album+"\n标题："+title);
            GetImg(imgnodes[0].Attributes["content"].Value);
            Console.ReadKey();

        }

        static void GetImg(string URL)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(URL, @"D:\test\testpic.jpg");
        }

        /// <summary>
        /// 上传用户信息
        /// </summary>
        /// <param name="deviceinfo">设备信息</param>
        /// <param name="userinfo">用户登录信息</param>
        /// <param name="cookie">小饼干</param>
        /// <returns></returns>
        static public HttpWebResponse SentInfo(string source, string cookie, string param)
        {
            //转换输入参数的编码类型，获取bytep[]数组 
            //byte[] byteArray = Encoding.UTF8.GetBytes(param);
            //初始化新的webRequst
            //1． 创建httpWebRequest对象
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(source));
            webRequest.KeepAlive = false;
            webRequest.Host = "music.163.com";
            //webRequest.ContentLength = length;
            //webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36";
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            webRequest.Referer = "https://music.163.com/";
            //webRequest.Headers.Add("Host", "music.163.com");
            //webRequest.Headers.Add("Content-Length", "338");
            //webRequest.Headers.Add("Origin", "https://music.163.com");
            //webRequest.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            webRequest.Headers.Add("Upgrade-Insecure-Requests", "1");
            //webRequest.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36");
            //webRequest.Headers.Add("Accept", "*/*");
            //webRequest.Headers.Add("Referer", "https://music.163.com/");
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            webRequest.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9,en-US;q=0.8,en;q=0.7");
            webRequest.Headers.Add("Cookie", cookie);
            //2． 初始化HttpWebRequest对象
            webRequest.Method = "GET";
            //webRequest.ContentLength = byteArray.Length;
            //3． 附加要POST给服务器的数据到HttpWebRequest对象(附加POST数据的过程比较特殊，它并没有提供一个属性给用户存取，需要写入HttpWebRequest对象提供的一个stream里面。)
            //Stream newStream = webRequest.GetRequestStream(); //创建一个Stream,赋值是写入HttpWebRequest对象提供的一个stream里面
            //newStream.Write(byteArray, 0, byteArray.Length);
            //newStream.Close();
            //4． 读取服务器的返回信息
            return (HttpWebResponse)webRequest.GetResponse();
            /*StreamReader php = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string phpend = php.ReadToEnd();
            return phpend;*/
        }
    }

}
