using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.ComponentModel;
using Dropbox.Api;
using System.IO;

namespace DropBox
{
   
    public class DownloadFile : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<String> AuthToken { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public Type Type { get; set; }


        [Category("Input")]
        [RequiredArgument]
        public InArgument<String> DropBoxPath { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<String> Destination { get; set; }

        [Category("Output")]
        public OutArgument<Boolean> Result { get; set; }


        protected override void Execute(CodeActivityContext context)
        {
            var AccessToken = AuthToken.Get(context);
            var DbPath = DropBoxPath.Get(context);
            var localPath = Destination.Get(context);
            DropboxClient DBClient = new DropboxClient(AccessToken);

            if (!localPath.EndsWith("/"))
            {
                localPath = localPath + "/";
            }


            if (Type.ToString().Equals("Folder"))
            {


                //download folder
                if (!DbPath.EndsWith("/"))
                {
                    DbPath = DbPath + "/";
                }



                var ListOfFiles = DBClient.Files.ListFolderAsync(DbPath);
                foreach (var x in ListOfFiles.Result.Entries.Where(i => i.IsFile))
                {
                    using (var DownloadFile = DBClient.Files.DownloadAsync(DbPath + x.Name))
                    {
                        var strm = DownloadFile.Result.GetContentAsStreamAsync();

                        if (!Directory.Exists(localPath))
                        {
                            Directory.CreateDirectory(localPath);
                        }
                        var file = File.Create(localPath + DownloadFile.Result.Response.Name);
                        strm.Result.CopyTo(file);
                        file.Close();
                    }
                }
                //end folder
                Result.Set(context,true);
            }


            if (Type.ToString().Equals("File"))
            {


                //download file
                using (var DownloadFile = DBClient.Files.DownloadAsync(DbPath))
                {
                    var strm = DownloadFile.Result.GetContentAsStreamAsync();

                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    var file = File.Create(localPath + DownloadFile.Result.Response.Name);
                    strm.Result.CopyTo(file);
                    file.Close();
                }
                Result.Set(context, true);
            }
        }
    }
}
