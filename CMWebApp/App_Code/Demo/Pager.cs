using System.Collections.Generic;
using System.Linq;

namespace Demo
{
    /// <summary>
    /// Summary description for Pager
    /// </summary>
    public class Pager
    {
        private string content = string.Empty;
        public Pager(string filePath)
        {
            content = IO.GetFileContent(filePath);
        }

        public List<PagerData> Data
        {
            get
            {
                var result = (List<PagerData>)DataConverter.Deserialize(content, typeof(List<PagerData>));
                return result;
            }
        }

        public object GetData(int targetPage=1,int showSize=10)
        {
            var data = (List<PagerData>)DataConverter.Deserialize(content, typeof(List<PagerData>));
            var total = data.Count();
            var totalPages = total/showSize;
            if (total%showSize > 0 )
            {
                totalPages ++;
            }
            var skipSize = showSize * (targetPage - 1);
            var result =new 
            {
                data =  data.Skip(skipSize).Take(showSize),
                totalPages
            } ;
            return result;
        }

  

    }




    #region DEMO PagerData Class for Serialize and Deserialize

    public class PagerData
    {
        public uint PostId { get; set; }
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
    }

    #endregion
}