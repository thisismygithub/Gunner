using System;
using System.Collections.Specialized;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
// Demo Namespace底下，只放著demo用的class 
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
            string response;
            context.Response.Clear();
            switch (action.ToLower())
            {
                case "getpagerdata":
                    var pagerdata = GetPagerData();
                    response = DataConverter.Serialize(pagerdata);
                    break;

                case "uploadimg":
                    response = UploadImg();
                    break;
                case "uploadallimg":
                    var uploadedInfo = UploadAllImg();
                    response = DataConverter.Serialize(uploadedInfo);
                    break;
                default:
                    response = "UNKNOW ACTION";
                    break;
            }
            context.Response.Write(response);
            context.Response.End();
        }
        private static object GetPagerData()
        {
            var physicalPath = string.Format(@"{0}\App_Code\Demo\pagerdata.json", HttpContext.Current.Server.MapPath(AppPath));
            var page = new Pager(physicalPath);

            int targetPage;
            int.TryParse(ReqParams["page"] ?? "1", out targetPage);

            return page.GetData(targetPage, 18);
        }

        private static UploadedInfo UploadImage()
        {
            var fileName = string.Empty;
            var previewPath = string.Empty;
            var errorMsg = string.Empty;
            var httpContext = HttpContext.Current;
            var files = httpContext.Request.Files;
            if (files.Count == 0)
            {
                //todo:return error
                errorMsg = "NO FILE";
            }
            else
            {
                var file = files[0];
                fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var ext = Path.GetExtension(file.FileName);
                if (string.IsNullOrEmpty(ext))
                {
                    errorMsg = "UNKOWN EXTENSION";
                }
                else
                {
                    //判斷檔案是否為圖檔
                    ext = ext.ToLower();

                    if (".jpg.jpeg.gif.png".IndexOf(ext, StringComparison.Ordinal) == -1)
                    {
                        errorMsg = "MUST BE .JPG .JPEG .GIF .PNG";
                    }
                    else
                    {
                        #region 判斷圖片size是否有合乎規格
                        var image = System.Drawing.Image.FromStream(file.InputStream);

                        #endregion

                        //取得新檔名
                        fileName = string.Format("{0}{1}{2}", fileName, DateTime.Now.Ticks, ext);

                        #region 降低圖片品質(縮小檔案大小)

                        // 指定圖片品質
                        var quality = new EncoderParameter(Encoder.Quality, 60L);

                        var codecs = ImageCodecInfo.GetImageEncoders();
                        var epParameters = new EncoderParameters(1);
                        epParameters.Param[0] = quality;
                        var jpegCodec = codecs.FirstOrDefault(t => t.MimeType == "image/jpeg");

                        #endregion
                        previewPath = string.Format(@"{0}\Image\Temp\", AppPath);
                        var dicPath = httpContext.Server.MapPath(previewPath);
                        var path = string.Format("{0}{1}", dicPath, fileName);

                        var dicInfo = new DirectoryInfo(dicPath);
                        if (!dicInfo.Exists)
                        {
                            dicInfo.Create();
                        }
                        file.SaveAs(path);
                        //image = ScaleImage(image, 219);
                        image.Save(path, jpegCodec, epParameters);
                    }
                }
            }
            return new UploadedInfo { ErrorMsg = errorMsg, FileName = fileName, PreviewPath = previewPath };
        }

        private static string UploadImg()
        {
            var uploadedInfo = UploadImage();
            var jsonParam = DataConverter.Serialize(uploadedInfo);
            var result = string.Format(@"<script type='text/javascript'>        
                    function fo(rep) {{            
                        if (rep.ErrorMsg !== '') {{
                            alert(rep.errorMsg);
                        }} else {{                
                            parent.$('#previewImg').show();
                            parent.$('#previewImg').attr('src', rep.PreviewPath + rep.FileName);
                        }}
                    }};
                    fo({0});</script>", jsonParam);
            return result;
        }

        private static object UploadAllImg()
        {
            var uploadedInfo = UploadImage();
            if (string.IsNullOrEmpty(uploadedInfo.ErrorMsg))
            {
                return new object();
            }
            return new {error = uploadedInfo.ErrorMsg};
        }

    }

    public class UploadedInfo
    {
        public string FileName { get; set; }
        public string ErrorMsg { get; set; }
        public string PreviewPath { get; set; }
    }
}