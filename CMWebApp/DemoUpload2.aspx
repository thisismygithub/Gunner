<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/default.master" AutoEventWireup="true" CodeFile="DemoUpload2.aspx.cs" Inherits="DemoUpload2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
        <%=Util.GetLinkElement("/cmwebapp/content/kartik-v-bootstrap-fileinput/css/fileinput.css") %> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
    <h4>bootstrap-fileinput</h4>
    <p></p>        
    <div class="page-header">
    <h1>Bootstrap File Input Example :<small><a href="https://github.com/kartik-v/bootstrap-fileinput-samples"><i class="glyphicon glyphicon-download"></i> Download Sample Files</a></small></h1>
    </div>

    <form enctype="multipart/form-data">
        <div class="form-group">
            <input id="file-1" type="file" multiple class="file" data-overwrite-initial="false" data-min-file-count="1">
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" Runat="Server">
    <%=Util.GetScriptElement("/gunner/cmwebapp/content/kartik-v-bootstrap-fileinput/js/fileinput.js") %>     
    <script type="text/javascript">
        $("#file-1").fileinput({
            uploadUrl: '/gunner/cmwebapp/ashx/demohandler.ashx?action=uploadallImg', // you must set a valid URL here else you will get an error
            allowedFileExtensions: ['jpg', 'png', 'gif'],
            overwriteInitial: false,
            maxFileSize: 1000,
            maxFilesNum: 10,
            //allowedFileTypes: ['image', 'video', 'flash'],
            slugCallback: function (filename) {
                return filename.replace('(', '_').replace(']', '_');
            }
        });
    </script>
</asp:Content>

