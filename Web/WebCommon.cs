using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

/// <summary>
/// 網頁通用方法
/// </summary>
public class WebCommon
{
    #region 縮圖相關

    #region 常數設定

    /// <summary>
    /// Jpeg Extension .Jpeg
    /// </summary>
    private const string JPG_EXTENSION = ".Jpeg";

    /// <summary>
    /// 原圖檔名樣板 {0}_O{1}
    /// </summary>
    private const string ORI_FILE_NAME_TEMPALTE = "{0}_O{1}";

    /// <summary>
    /// 大圖檔名樣板 {0}{1}
    /// </summary>
    private const string L_FILE_NAME_TEMPALTE = "{0}{1}";

    /// <summary>
    /// 中圖檔名樣板 {0}_M{1}
    /// </summary>
    private const string M_FILE_NAME_TEMPALTE = "{0}_M{1}";

    /// <summary>
    /// 小圖檔名樣板 {0}_S{1}
    /// </summary>
    private const string S_FILE_NAME_TEMPALTE = "{0}_S{1}";

    /// <summary>
    /// 檔案路徑樣板 @"{0}\{1}"
    /// </summary>
    private const string FILE_PATH_TEMPALTE = @"{0}\{1}";
    #endregion

    /// <summary>
    /// 建立圖片and縮圖
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="filepath"></param>
    /// <param name="newName"></param>
    /// <returns></returns>
    public static string CreateImageAndCompression(Byte[] buffer, string extension, string filepath, string newName, ImageSetting imageSetting)
    {
        var isSaveAsJpg = imageSetting.IsSaveAsJpg;

        if (isSaveAsJpg)
        {
            extension = JPG_EXTENSION;
        }

        string fileNameOri = string.Format(ORI_FILE_NAME_TEMPALTE, newName, extension);
        string fileNameL = string.Format(L_FILE_NAME_TEMPALTE, newName, extension);
        string fileNameM = string.Format(M_FILE_NAME_TEMPALTE, newName, extension);
        string fileNameS = string.Format(S_FILE_NAME_TEMPALTE, newName, extension);

        //個別大小縮圖
        using (var img = Image.FromStream(new MemoryStream(buffer)))
        {
            //原圖
            if (!isSaveAsJpg)
            {
                // 不降低圖片品質 儲存檔案
                using (var fileStrm = File.Create(string.Format(FILE_PATH_TEMPALTE, filepath, fileNameOri)))
                {
                    fileStrm.Write(buffer, 0, buffer.Length);//原圖_O
                }
            }
            else
            {
                SaveThumbPicWidth(img, img.Width, string.Format(FILE_PATH_TEMPALTE, filepath, fileNameOri), isSaveAsJpg);
            }

            //建立 L 縮圖
            if (img.Width <= imageSetting.OriginalMaxSize)
            {
                if (isSaveAsJpg)
                {
                    SaveThumbPicWidth(img, img.Width, string.Format(FILE_PATH_TEMPALTE, filepath, fileNameL), isSaveAsJpg);//縮圖L不加後贅詞
                }
                else
                {
                    SaveImageFile(buffer, filepath, fileNameL);//小於縮圖目標大小，且不須轉為Jpeg
                }
            }
            else
            {
                SaveThumbPicWidth(img, imageSetting.OriginalMaxSize, string.Format(FILE_PATH_TEMPALTE, filepath, fileNameL), isSaveAsJpg);//縮圖L不加後贅詞
            }

            //建立 M 縮圖
            if (img.Width <= imageSetting.MiddleMaxSize)
            {
                if (isSaveAsJpg)
                {
                    SaveThumbPicWidth(img, img.Width, string.Format(FILE_PATH_TEMPALTE, filepath, fileNameM), isSaveAsJpg);//縮圖_M
                }
                else
                {
                    SaveImageFile(buffer, filepath, fileNameM);//小於縮圖目標大小，且不須轉為Jpeg
                }
            }
            else
            {
                SaveThumbPicWidth(img, imageSetting.MiddleMaxSize, string.Format(FILE_PATH_TEMPALTE, filepath, fileNameM), isSaveAsJpg);//縮圖_M
            }

            //建立 S 縮圖
            if (img.Width <= imageSetting.SmallMaxSize)
            {
                if (isSaveAsJpg)
                {
                    SaveThumbPicWidth(img, img.Width, string.Format(FILE_PATH_TEMPALTE, filepath, fileNameS), isSaveAsJpg);//縮圖_S
                }
                else
                {
                    SaveImageFile(buffer, filepath, fileNameS);//小於縮圖目標大小，且不須轉為Jpeg
                }
            }
            else
            {
                SaveThumbPicWidth(img, imageSetting.SmallMaxSize, string.Format(FILE_PATH_TEMPALTE, filepath, fileNameS), isSaveAsJpg);//縮圖_S
            }
        }

        return fileNameL;
    }

    /// <summary>
    /// 直接儲存檔案
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="filepath"></param>
    /// <param name="fileName"></param>
    public static void SaveImageFile(Byte[] buffer, string filepath, string fileName)
    {
        // 不降低圖片品質 儲存檔案        
        using (var fileStrm = File.Create(string.Format(FILE_PATH_TEMPALTE, filepath, fileName)))
        {
            fileStrm.Write(buffer, 0, buffer.Length);//原圖_O
        }
    }

    #region 產生縮圖並儲存

    /// <summary>
    /// 產生縮圖並儲存 寬度維持maxpix，高度等比例
    /// </summary>
    /// <param name="srcImagePath">來源圖片的路徑</param>
    /// <param name="widthMaxPix">超過多少像素就要等比例縮圖</param>
    /// <param name="saveThumbFilePath">縮圖的儲存檔案路徑</param>
    /// <param name="isSaveAsJpg">是否指定存為Jpg檔</param>
    private static void SaveThumbPicWidth(Image img, int targetWidth, string saveThumbFilePath, bool isSaveAsJpg)
    {
        //計算縮放比，這裡不該有0 ，有了會造成除以0
        double rate = targetWidth / (double)(img.Width == 0 ? 1 : img.Width);

        if (rate > 1 || rate <= 0)
        {
            //todo: error control 這裡不該放大，這不是我們要處理的
            throw new Exception("image resize rate out of range");
        }

        //計算縮放大小
        int tarWidth = (int)(img.Width * rate);
        int tarHeight = (int)(img.Height * rate);

        //取得檔案原始像素格式
        var pixelFormat = img.PixelFormat;

        //如果是索引(index)像素圖，要特別做一個轉換的動作
        if (IsPixelFormatIndexed(img.PixelFormat))
        {
            pixelFormat = PixelFormat.Format32bppArgb;
        }

        //取得檔案格式
        var rowFormat = img.RawFormat;
        if (isSaveAsJpg)
        {
            rowFormat = ImageFormat.Jpeg;
        }

        //建立空載體，它使用了 PixelFormat.Format24bppRgb 作為統一的像素格式
        //使用24是因為它不會造成漸層圖的失真(有糊點)，但不支援去背透明，而且胖達說 24 很多了
        //改為圖片原始像素格式，避免透明底在壓縮過後轉換為黑底失真 20140917 Elvis
        using (Bitmap clone = new Bitmap(tarWidth, tarHeight, pixelFormat))
        {
            //以載體建立影像操作器
            using (Graphics gr = Graphics.FromImage(clone))
            {
                if (isSaveAsJpg)
                {
                    // 將透明的底圖改成白色
                    gr.Clear(Color.White);
                }
                //透過操作器，複制影像資料入載體，這時，它已經被縮小了
                gr.DrawImage(img, new Rectangle(0, 0, tarWidth, tarHeight));
            }

            //建制需要的影像編碼器，它其實來自原影像的定義(現需可指定為Jpeg或原始 2015/01/27)
            ImageCodecInfo iEncoder = GetEncoder(rowFormat);

            //利用指定的編碼器輸出
            clone.Save(saveThumbFilePath, iEncoder, null);
        }
    }

    #endregion

    #region 預防索引像素圖的私有方法檢查

    /// <summary>
    /// 索引(index)像素圖的List
    /// </summary>
    private static PixelFormat[] indexedPixelFormats = 
        { 
            PixelFormat.Undefined, 
            PixelFormat.DontCare,
            PixelFormat.Format16bppArgb1555, 
            PixelFormat.Format1bppIndexed, 
            PixelFormat.Format4bppIndexed,
            PixelFormat.Format8bppIndexed
        };

    /// <summary>
    /// 確認是否為索引(index)像素圖
    /// </summary>
    /// <param name="imgPixelFormat"></param>
    /// <returns></returns>
    private static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
    {
        foreach (PixelFormat pf in indexedPixelFormats)
        {
            if (pf.Equals(imgPixelFormat))
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    private static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }

    #endregion

    #region Member靜態公開方法

    #region 轉換成Guid

    /// <summary>
    /// 轉換成Guid
    /// </summary>
    /// <param name="input"></param>
    /// <param name="output"></param>
    /// <returns></returns>
    public static bool GuidTryParse(string input, out Guid output)
    {
        try
        {
            Guid gd = new Guid(input);
            output = gd;

            return true;
        }
        catch
        {
            output = new Guid();

            return false;
        }
    }
    #endregion

    #region 取得Ip位址

    /// <summary>
    /// 取得Ip位址
    /// </summary>
    /// <returns></returns>
    public static string GetIpAddress()
    {
        if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["X-Forwarded-For"]))
        {
            return HttpContext.Current.Request.UserHostAddress ?? "";
        }
        else
        {
            string xForwardedFor = HttpContext.Current.Request.Headers["X-Forwarded-For"];
            string[] ipAddressList = xForwardedFor.Split(',');
            if (ipAddressList.Length > 0)
            {
                return ipAddressList[0].Trim();
            }
            else
            {
                return "";
            }
        }
    }
    #endregion

    #endregion

}
