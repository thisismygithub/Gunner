<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/default.master" AutoEventWireup="true" CodeFile="DemoUpload.aspx.cs" Inherits="DemoUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <%=Util.GetLinkElement("/cmwebapp/content/jasny-bootstrap/css/jasny-bootstrap.css") %>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
    <h4>Hard Code</h4>
    <input type="button" value="upload" id="uploadImg" />
    <img src="" alt="preview" id="previewImg" style="display: none;" />
    <iframe id="iframeUpload" style="display: none;" ></iframe>
    <hr/>
    <h4>Jasny Bootstrap fileupload </h4>    
    <p><a href="http://jasny.github.io/bootstrap/javascript/">ref</a></p>
<div class="fileinput fileinput-new" data-provides="fileinput">
        <div class="fileinput-preview thumbnail" data-trigger="fileinput" style="width: 200px; height: 150px; line-height: 150px;"></div>
        <div>
          <span class="btn btn-default btn-file"><span class="fileinput-new">Select image</span>
              <span class="fileinput-exists">Change</span>              
              <input type="hidden" value="" name="">
              <input type="file" name="file" class="ephoto-upload" accept="image/*"></span>            
          <a href="#" class="btn btn-default fileinput-exists" data-dismiss="fileinput">Remove</a>
        </div>
      </div>
    <hr />
    <h4>bootstrap-fileinput</h4>
    <p></p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" Runat="Server">
    <%=Util.GetScriptElement("/content/jasny-bootstrap/js/jasny-bootstrap.js") %>        
    <%=Util.GetScriptElement("/content/kartik-v-bootstrap-fileinput/js/fileinput.js") %>     
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

