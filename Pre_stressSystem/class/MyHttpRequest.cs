using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows;

namespace Pre_stressSystem
{
    class MyHttpRequest
{
        /// <summary>
        /// GET方法
        /// </summary>

        /// <returns></returns>
        public static string GetWebResponse_Get(string strUrl, Encoding encode, bool Decompress)
        {

            Uri uri = new Uri(strUrl);
            WebRequest webreq = WebRequest.Create(uri);
            try
            {
                Stream s = Decompress ? new System.IO.Compression.GZipStream(webreq.GetResponse().GetResponseStream(), System.IO.Compression.CompressionMode.Decompress)
                                 : webreq.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, encode);
                string all = sr.ReadToEnd();         //读取网站返回的数据
                s.Close();
                sr.Close();
                return all;
            }
            catch(Exception e)
            {
                MessageBox.Show("访问："+ "strUrl"+" 时服务器出错");
            }
            return null;
           
        }

        /// <summary>
        /// POST方法
        /// </summary>

        public static string GetWebResponse_Post(string strUrl, Encoding encode, bool Decompress, String postData)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            Uri uri = new Uri(strUrl);
            WebRequest webreq = WebRequest.Create(uri);
            webreq.Method = "POST";
            webreq.ContentType = "application/x-www-form-urlencoded";
            webreq.ContentLength = byteArray.Length;

            Stream dataStream = webreq.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = webreq.GetResponse();
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader sr = new StreamReader(dataStream, encode);

            string all = sr.ReadToEnd();
            sr.Close();
            dataStream.Close();
            response.Close();
            return all;
        }
    }
}
