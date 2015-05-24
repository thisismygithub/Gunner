<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Default.master" AutoEventWireup="true" CodeFile="DemoPager.aspx.cs" Inherits="DemoPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
<span class="text-center js-pager"></span>
<table class="table">
    <thead>
        <tr>
        <th>Id</th>
        <th>Name</th>
        <th>Email</th>
        <th>Body</th>
        </tr>
    </thead>
    <tbody class="js-tbody">

    </tbody>
</table>    
<span class="text-center js-pager"></span>
<form runat="server">
    <asp:HiddenField ClientIDMode="Static" ID="hidPageData" runat="server" />
</form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" Runat="Server">
        <script type="text/template" id="datarow">
        <tr>
            <td>{Id}</td>
            <td>{Name}</td>
            <td>{Email}</td>
            <td>{Body}</td>            
        </tr>
        </script>
    <script type="text/javascript">
        $(function () {
            var pageData = JSON.parse($('#hidPageData').val());
            setEvent();
            getPage(pageData.page);

            function setEvent() {
                $('.js-pager').on('click', '.js-other-page', function() {
                    var key = +$(this).attr('key');
                    var stateObject = {};
                    var title = "page " + key;
                    var newUrl = window.location.href.split('?')[0]+"?page="+key;
                    history.pushState(stateObject, title, newUrl);
                    getPage(key);
                });
            }

            function getPage(defaultPage) {
                $.ajax({
                    url: "/cmwebapp/ashx/demoHandler.ashx",
                    data: {
                        action: 'getpagerdata',
                        page: defaultPage
                    },
                    type: "post",
                    cache: false,
                    async: true,
                    dataType: "json",
                }).done(function (response) {
                    
                    renderTable(response.data);
                    renderPager(defaultPage, response.totalPages);
                });
            }

            function renderTable(data) {
                
                var temp = $('#datarow').html();
                var html = '';
                for (var i = 0, len = data.length; i < len; i++) {
                    html += complie(temp, data[i]);
                }
                $(".js-tbody").html(html);
            }

            function renderPager(target, total) {
                var result = '<nav><ul class="pagination">';
                // 顯示的頁數範圍
                var range = 3;
                //如果當前頁大於1
                if (target > 1) {
                    //第一頁
                    result += '<li><a class="js-other-page" key="1"  >First</a></li>';
                    //前一頁
                    result += '<li><a class="js-other-page" key="' + (target-1) + '"  >Prev</a></li>';
                }
                
                
                for (var x = ((target - range) - 1) ; x < ((target + range) + 1) ; x++)
                {
                    // 如果這是一個正確的頁數...
                    if ((x > 0) && (x <= total))
                    {                        
                        if (x == target)
                        {
                            // 如果這一頁等於當前頁數...
                            //不使用連結, 但用高亮度顯示                      
                            result += '<li class="active"><a class="js-target-page" href="javascript:void(null);" key=' + x + '>' + x + '</a></li>';
                            
                        } else {
                            // 如果這一頁不是當前頁數...
                            // 顯示連結
                            result += '<li><a class="js-other-page" href="javascript:void(null);" key="' + x + '">' + x + '</a></li>';
                        } 
                    } else {
                        //TODO
                    } 
                }
                //如果當前頁小於總頁數
                if (target < total) {
                    //下一頁
                    result += '<li><a href="javascript:void(null);" class="js-other-page" key="' + (target + 1) + '">Next</a></li>';
                    //最末頁
                    result += '<li><a href="javascript:void(null);" class="js-other-page" key="' + total + '">Last</a></li>';
                }
                
                result +='</ul></nav>';

                $('.js-pager').html(result);
            }

            function complie(template, data) {
                return template.replace(/\{\s?([\w\s\.]*)\s?\}/g, function (str, key) {
                    key = $.trim(key);
                    var v = data[key];
                    return (typeof v !== 'undefined' && v !== null) ? v : '';
                });
            }

        });
    </script>
</asp:Content>

