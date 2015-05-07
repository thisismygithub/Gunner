using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 圖片大小設定
/// </summary>
public class ImageSetting
{
    /// <summary>
    /// 大圖上限大小(網頁預設顯示)
    /// </summary>
    public int OriginalMaxSize { set; get; }

    /// <summary>
    /// 中圖上限大小
    /// </summary>
    public int MiddleMaxSize { set; get; }

    /// <summary>
    /// 小圖上限大小
    /// </summary>
    public int SmallMaxSize { set; get; }

    /// <summary>
    /// 是否存為Jpg
    /// </summary>
    public bool IsSaveAsJpg { set; get; }

    /// <summary>
    /// 圖片大小設定
    /// </summary>
    /// <param name="originalMaxSize"></param>
    /// <param name="middleMaxSize"></param>
    /// <param name="smallMaxSize"></param>
    /// <param name="isSaveAsJpg"></param>
    public ImageSetting(int originalMaxSize, int middleMaxSize, int smallMaxSize, bool isSaveAsJpg)
    {
        this.OriginalMaxSize = originalMaxSize;
        this.MiddleMaxSize = middleMaxSize;
        this.SmallMaxSize = smallMaxSize;
        this.IsSaveAsJpg = isSaveAsJpg;
    }
}
