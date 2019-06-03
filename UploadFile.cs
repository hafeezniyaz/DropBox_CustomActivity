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
    public class UploadFile : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<String> AuthToken { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<String> FilePath { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<String> Destination { get; set; }

        [Category("Output")]
        public OutArgument<Boolean> Result { get; set; }


        protected override void Execute(CodeActivityContext context)
        {
            var AccessToken = AuthToken.Get(context);
            var DbPath = Destination.Get(context);
            var LocalFilePath = FilePath.Get(context);
            DropboxClient DBClient = new DropboxClient(AccessToken);


            if (!DbPath.EndsWith("/"))
            {
                DbPath = DbPath + "/";
            }

           
            var localFileName = Path.GetFileName(LocalFilePath);
         

            FileStream fs = File.OpenRead(LocalFilePath);

            var response = DBClient.Files.UploadAsync(DbPath + localFileName, WriteMode.Overwrite.Instance, body: fs);

            Result.Set(context, true);
        

    }
    }
}
