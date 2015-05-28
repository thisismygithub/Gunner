
// Demo Namespace底下，只放著demo用的class 

using System;
using System.Collections.Specialized;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Demo
{
    public class Process
    {
        #region 組檔案路徑

        private static readonly string AppPath = HttpContext.Current.Request.ApplicationPath;
        #endregion

        protected static NameValueCollection ReqParams = null; 
        public static void Main()
        {
            ReqParams = HttpContext.Current.Request.Params;
            var context = HttpContext.Current;
            string action = ReqParams["action"] ?? string.Empty;
            object response = null;
            switch (action.ToLower())
            {
                case "getpagerdata":
                    response = GetPagerData();                    
                    break;

                case "uploadimg":
                    UploadImg();       
                    break;
                default:
                    break;
            }
            context.Response.Write(DataConverter.Serialize(response));
            context.Response.End();
        }

        private static void UploadImg()
        {
            var errorMsg = string.Empty ;
            var httpContext = HttpContext.Current;
            var files = httpContext.Request.Files;
            if (files.Count == 0)
            {
                //todo:return error
                errorMsg = "NO FILE";
            }
            var file = files[0];
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(ext))
            {
                errorMsg = "UNKOWN EXTENSION";
            }

            //判斷檔案是否為圖檔
            ext = ext.ToLower();

            if (".jpg.jpeg.gif.png".IndexOf(ext, StringComparison.Ordinal) == -1)
            {
                errorMsg = "MUST BE .JPG .JPEG .GIF .PNG";
            }

            #region 判斷圖片size是否有合乎規格
            var image = System.Drawing.Image.FromStream(file.InputStream);
            //var width = image.Width;
            //var height = image.Height;
            //var range = ((double)height / (double)width);
            //var isValidate = range <= 2 && range >= 0.5;
            string code;
            //if (!isValidate)
            //{
            //    code = string.Format("<script type='text/javascript'>new top.Dialog().Show('圖片的長除以寬不得大於2或小於0.5!');</script>");

            //    httpContext.Response.Write(code);
            //    httpContext.Response.End();
            //    return;
            //}
            #endregion

            //取得新檔名
            fileName = string.Format("{0}{1}{2}", fileName, DateTime.Now.Ticks, ext);

            #region 降低圖片品質(縮小檔案大小)
            ImageCodecInfo jpegCodec = null;
            // 指定圖片品質
            var quality = new EncoderParameter(Encoder.Quality, 60L);

            var codecs = ImageCodecInfo.GetImageEncoders();
            var epParameters = new EncoderParameters(1);
            epParameters.Param[0] = quality;
            for (var i = 0; i < codecs.Length; i++)
            {
                if (codecs[i].MimeType != "image/jpeg") continue;

                jpegCodec = codecs[i];
                break;
            }

            #endregion
            var dicPath = httpContext.Server.MapPath(string.Format(@"{0}\Image\Temp\", AppPath));
            var path = string.Format("{0}{1}", dicPath, fileName);

            var dicInfo = new DirectoryInfo(dicPath);
            if (!dicInfo.Exists)
            {
                dicInfo.Create();
            }
            file.SaveAs(path);
            //image = ScaleImage(image, 219);
            image.Save(path, jpegCodec, epParameters);
            code = string.Format("<script type='text/javascript'>new parent.SetExchangeItem().SetImage('{0}',true);</script>", fileName);

            httpContext.Response.Write(code);
            httpContext.Response.End();
        }

        private static object GetPagerData()
        {
            var physicalPath = string.Format(@"{0}\App_Code\Demo\pagerdata.json", HttpContext.Current.Server.MapPath(AppPath));
            var page = new Pager(physicalPath) ;
            
            int targetPage;
            int.TryParse(ReqParams["page"] ?? "1", out targetPage);

            return page.GetData(targetPage, 18);
        }
    }


}