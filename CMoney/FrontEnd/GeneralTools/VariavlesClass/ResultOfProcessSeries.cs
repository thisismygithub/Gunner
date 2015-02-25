
namespace CMoney.WebFrontend.GeneralTools.VariavlesClass
{
    /// <summary>
    /// 處理序號程序之回傳物件
    /// </summary>
    public class ResultOfProcessSeries
    {
        /// <summary>
        /// 建構式
        /// </summary>
        public ResultOfProcessSeries()
        {
            IsSuccess = true;

            ErrorType = ErrType.NoneError;

            ErrorCode = 0;

            ErrorMessage = "";

            RedirectPage = "";

            ProductID = 0;

            FunctionID = 0;
        }

        /// <summary>
        /// 產品啟用成功與否
        /// </summary>
        public bool IsSuccess
        {
            get;
            set;
        }

        /// <summary>
        /// 錯誤類型定義
        /// </summary>
        public enum ErrType
        {
            /// <summary>
            /// 沒有錯誤
            /// </summary>
            NoneError = 0,
            /// <summary>
            /// 參數錯誤
            /// </summary>
            ArgumentError = 1,
            /// <summary>
            /// 系統錯誤
            /// </summary>
            SystemError = 2,
            /// <summary>
            /// 金流錯誤
            /// </summary>
            CashFlowError = 3,
            /// <summary>
            /// 序號錯誤
            /// </summary>
            SeriesError = 4,
            /// <summary>
            /// 產品啟用錯誤
            /// </summary>
            ProductActiveError = 5
        }

        /// <summary>
        /// 錯誤類型
        /// </summary>
        public ErrType ErrorType;

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public int ErrorCode;

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// 該產品啟用轉址結果頁面
        /// </summary>
        public string RedirectPage
        {
            get;
            set;
        }

        /// <summary>
        /// 產品代號
        /// </summary>
        public int ProductID
        {
            get;
            set;
        }

        /// <summary>
        /// 產品功能代號
        /// </summary>
        public int FunctionID
        {
            get;
            set;
        }
    }
}
