<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Default.master" AutoEventWireup="true" CodeFile="DemoPageData.aspx.cs" Inherits="DemoPageData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
    <form runat="server">
        <div class="container-fluid js-jsondata">
  
        </div>
    <table class="table">
        <thead>
          <tr>
            <th>id</th>
            <th>name</th>
            <th>email</th>
          </tr>
        </thead>
        <tbody class="js-tbody">

        </tbody>
    </table>
    <asp:HiddenField ClientIDMode="Static" ID="hidPageData" runat="server" />
        </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" Runat="Server">
    <script type="text/template" id="datarow">
        <tr>
            <td>{id}</td>
            <td>{name}</td>
            <td>{email}</td>
        </tr>
    </script>
    <script type="text/javascript">
        $(function () {
            var pageData = JSON.parse($('#hidPageData').val());
            $('.js-jsondata').html($('#hidPageData').val());
            var rowData = pageData.data.Products;
            var temp = $('#datarow').html();
            var html = '';
            for (var i = 0,len = rowData.length; i < len; i++) {
                html += complie(temp, rowData[i]);
            }
            $(".js-tbody").html(html);

            function complie(template, data) {
                return template.replace(/\{\s?([\w\s\.]*)\s?\}/g, function(str, key) {
                    key = $.trim(key);
                    var v = data[key];
                    return (typeof v !== 'undefined' && v !== null) ? v : '';
                });
            }

        });
    </script>
</asp:Content>

