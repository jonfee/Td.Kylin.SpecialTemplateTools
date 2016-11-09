using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateFactory.Core
{
    public static class FileReader
    {
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T Read<T>(string path, Encoding encoding) where T : class, new()
        {
            T t = default(T);

            try
            {
                using (var sr = new StreamReader(path, encoding))
                {
                    string content = sr.ReadToEnd();

                    t = JsonConvert.DeserializeObject<T>(content);
                }
            }
            catch
            {

            }

            return t;
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadToString(string path, Encoding encoding)
        {
            string content = string.Empty;

            try
            {
                using (var sr = new StreamReader(path, encoding))
                {
                    content = sr.ReadToEnd();
                }
            }
            catch
            {

            }

            return content;
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <param name="removeRows"></param>
        /// <returns></returns>
        public static List<T> ReadToList<T>(string path, Encoding encoding, int removeRows = 0) where T : class, new()
        {
            List<T> list = new List<T>();

            try
            {
                using (var sr = new StreamReader(path, encoding))
                {
                    string line;
                    int idx = 0;
                    while (idx++ >= removeRows && (line = sr.ReadLine()) != null)
                    {
                        var item = JsonConvert.DeserializeObject<T>(line);
                        if (item != null)
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            catch
            {

            }

            return list;
        }
    }
}
