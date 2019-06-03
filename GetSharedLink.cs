using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.ComponentModel;
using Dropbox.Api;
using System.IO;
using Dropbox.Api.Files;
namespace DropBox
{
    public class GetSharedLink : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<String> AuthToken { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<String> FilePath { get; set; }

        
        [Category("Output")]
        public OutArgument<String> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var AccessToken = AuthToken.Get(context);
            var DbPath = FilePath.Get(context);
            DropboxClient DBClient = new DropboxClient(AccessToken);
            var shareFile = DBClient.Sharing.CreateSharedLinkWithSettingsAsync(DbPath);
            var url = shareFile.Result.Url.ToString();
            Result.Set(context,url);
        }
    }
}
