using System;
using System.IO;
using System.Security.Permissions;
using Microsoft.Extensions.Configuration;

namespace DIS
{
    public class FileWatch
    {
        public IConfiguration _configuration { get; }

        public FileWatch(IConfiguration configuration){
            _configuration = configuration;
        }

        public void Run()
        {

            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = _configuration.GetConnectionString("HotFolder");
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            //watcher.Filter = "*.txt";

            // Add event handlers.
            //watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
            //while (true) ;
        }

        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            
            var db = new DIS.Models.DataAccess(_configuration);
            string Base64 = null;
            string hashcode = null;

            if (db.GetFile(e.Name) == null)
            {
                // Specify what is done when a file is changed, created, or deleted.
                Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
                Base64 = Convert_to_base64(e.FullPath);
                var dbpayload = new DIS.Models.Files();
                dbpayload.FileName = e.Name;
                dbpayload.DocumentType = Path.GetExtension(e.Name);
                dbpayload.GeneratedAt = DateTime.Now;
                dbpayload.Base64EncodedFile = Base64;
                dbpayload.HashCode = hashcode;
                db.Create(dbpayload);
                var rabbitmq = new DIS.MessagingQueue(_configuration);
                rabbitmq.Publish(e.Name);
            }

        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }

        private static string Convert_to_base64(string path)
        {
            Byte[] bytes = File.ReadAllBytes(path);
            String file = Convert.ToBase64String(bytes);
            return file;
        }

    }
}