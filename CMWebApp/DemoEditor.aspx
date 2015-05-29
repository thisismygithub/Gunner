<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/default.master" AutoEventWireup="true" CodeFile="DemoEditor.aspx.cs" Inherits="DemoEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div id="section1">CKEditor</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" Runat="Server">
    <%=Util.GetScriptElement("/gunner/cmwebapp/content/ckeditor/ckeditor.js") %>        
    <script type="text/javascript">
        CKEDITOR.appendTo('section1',
            null,
            '<p>This is some <strong>sample text</strong>. You are using <a href="http://ckeditor.com/">CKEditor</a>.</p>'
        );
    </script>
</asp:Content>

