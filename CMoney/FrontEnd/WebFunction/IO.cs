using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CMoney.WebBackend.WebFunction
{
    /// <summary>
    /// IO 相關類別。
    /// </summary>
    public class IO
    {
        /// <summary>
        /// IO類別預設建構子
        /// </summary>
        public IO()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        /// <summary>
        /// 取得檔案內容(follow)
        /// </summary>
        /// <param name="fileName">檔案路徑</param>
        /// <returns></returns>
        public static string GetFileContent(string fileName)
        {
            if (!File.Exists(fileName))
            {
                //表示失敗
                return "";
            }
            var stringBulilder = new StringBuilder();
            using (var streamReader = new StreamReader(fileName))
            {
                string line;
                streamReader.ReadLine();

                while ((line = streamReader.ReadLine()) != null)
                {
                    stringBulilder.Append(line);
                }
            }

            return stringBulilder.ToString();
        }

        /// <summary>
        /// 取得檔案內容
        /// </summary>
        /// <param name="fileName">檔案路徑</param>
        /// <param name="encoding">取得的內容編碼格式</param>
        /// <returns></returns>
        [Obsolete("好像沒人用")]
        public static string GetFileContent(string fileName, Encoding encoding)
        {
            if (!File.Exists(fileName))
            {
                //表示失敗
                return "";
            }

            var stringBulilder = new StringBuilder();
            using (var streamReader = new StreamReader(fileName, encoding))
            {
                string line;
                streamReader.ReadLine();

                while ((line = streamReader.ReadLine()) != null)
                {
                    stringBulilder.Append(line);
                }
            }

            return stringBulilder.ToString();
        }
    }
}
