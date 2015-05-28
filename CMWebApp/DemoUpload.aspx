<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/default.master" AutoEventWireup="true" CodeFile="DemoUpload.aspx.cs" Inherits="DemoUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
    <input type="button" value="upload" id="uploadImg" />
    <iframe id='iframeUpload' ></iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" Runat="Server">
    <script type="text/javascript">
        $(function () {
            setEvent();


            function setEvent() {
                uploadImage();
            }


            function uploadImage() {//上傳圖片
                $('#uploadImg').click(function() {
                    var codeIfm;
                    codeIfm = '<form enctype="multipart/form-data" action="/gunner/cmwebapp/ashx/demohandler.ashx" method="post">';
                    codeIfm += '<input type="text" name="action" value="uploadimg">';
                    codeIfm += '<input type="file" id="file" name="file" value="" onchange="document.forms[0].submit();return false;" />';
                    codeIfm += '</form>';

                    $('#iframeUpload').contents().find('html').html(codeIfm).find('#file').click();
                });
            }

        });
    </script>
</asp:Content>

