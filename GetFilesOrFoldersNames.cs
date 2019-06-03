using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.ComponentModel;
using Dropbox.Api;

namespace DropBox
{
    public enum Type
    {
        Folder,
        File
    }
    public class ListFolders : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<String> AuthToken { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public Type Type { get; set; }

        [Category("Input")]
        public InArgument<String> FolderPath { get; set; }

        [Category("Output")]
        public OutArgument<List<String>> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var AccessToken = AuthToken.Get(context);
            var Path = FolderPath.Get(context);
           
            
            
            DropboxClient DBClient = new  DropboxClient(AccessToken);
            List<String> ListFolder = new List<String>();
            Task<Dropbox.Api.Files.ListFolderResult> ListOfFolders;

            if (Type.ToString().Equals("Folder"))
            {



            }
            
            if (String.IsNullOrEmpty(Path))
            {
                 ListOfFolders = DBClient.Files.ListFolderAsync("");
            }
            else
            {
                ListOfFolders = DBClient.Files.ListFolderAsync(Path);
            }

            
            if(Type.ToString().Equals("Folder"))
            {
                foreach (var x in ListOfFolders.Result.Entries.Where(i => i.IsFolder))
                {

                    ListFolder.Add(x.Name);
                }
            }
            else
            {
                foreach (var x in ListOfFolders.Result.Entries.Where(i => i.IsFile))
                {

                    ListFolder.Add(x.Name);
                }
            }
            Result.Set(context, ListFolder);
        }
            

          
        }
    }


