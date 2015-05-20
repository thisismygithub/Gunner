<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Default.master" AutoEventWireup="true" CodeFile="DemoPager.aspx.cs" Inherits="DemoPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
<table class="table">
    <thead>
        <tr>
        <th>編號</th>
        <th>長度</th>
        <th>重量</th>
        </tr>
    </thead>
    <tbody class="js-tbody">

    </tbody>
</table>    
<ul class="pager">
  <li><a href="#">Previous</a></li>
  <li><a href="#">Next</a></li>
</ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" Runat="Server">
        <script type="text/template" id="datarow">
        <tr>
            <td>{No}</td>
            <td>{Length}</td>
            <td>{Weight}</td>
        </tr>
    </script>
    <script type="text/javascript">
        $(function () {
            $.ajax({
                url: "/cmwebapp/ashx/demoHandler.ashx",
                data: {
                    action: 'getpagerdata'
                },
                type: "post",
                cache: false,
                async: true,

                dataType: "json",
                success: function(response) {
                    console.log(response);
                    
                }
            });
        });
    </script>
</asp:Content>

