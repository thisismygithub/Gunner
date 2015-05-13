<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Default.master" AutoEventWireup="true" CodeFile="CMDemo.aspx.cs" Inherits="CmDemo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
    <form runat="server">
        <div class="container-fluid js-jsondata">
  
        </div>
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
    <asp:HiddenField ClientIDMode="Static" ID="hidPageData" runat="server" />
        </form>
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
            var pageData = JSON.parse($("#hidPageData").val());
            $(".js-jsondata").html($("#hidPageData").val());
            debugger;
        });
    </script>
</asp:Content>

