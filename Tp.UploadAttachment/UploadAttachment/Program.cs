using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Tp.AttachmentServiceProxy;
using Tp.FileServiceProxy;
using Tp.RequestServiceProxy;

namespace UploadAttachment
{
    class Program
    {
        private static readonly ServiceManager ServiceManager = new ServiceManager();

        private static void Main()
        {
            var requestId = int.Parse(ConfigurationManager.AppSettings["RequestId"]);
            var filePath = ConfigurationManager.AppSettings["FilePath"];
            var fileName = ConfigurationManager.AppSettings["FileName"];

            var attachmentPath = Path.Combine(filePath, fileName);
            var fileAttachment = new FileAttachment
            {
                FileStream = new FileStream(attachmentPath, FileMode.Open),
                OriginalName = fileName
            };
            
            using (fileAttachment.FileStream)
            {
                AddAttachmentToRequest(fileAttachment, requestId);
            }
        }

        private static void AddAttachmentToRequest(FileAttachment attachment, int requestId)
        {
            var sw = Stopwatch.StartNew();
            var fileName = UploadAttachment(attachment, 128 * 1024);

            using (var requestSvc = ServiceManager.GetService<RequestService>())
            {
                var attachmentId = requestSvc.AddAttachmentToRequest(requestId, fileName, attachment.Description);
                var request = requestSvc.GetByID(requestId);

                using (var attachSvc = ServiceManager.GetService<AttachmentService>())
                {
                    var attachmentDto = attachSvc.GetByID(attachmentId);
                    attachmentDto.OriginalFileName = attachment.OriginalName;
                    attachmentDto.OwnerID = request.OwnerID;
            
                    attachSvc.Update(attachmentDto);
                }
            }
            sw.Stop();
            
            Console.WriteLine("Attachment uploaded in {0} ms.", sw.ElapsedMilliseconds);
        }

        private static string UploadAttachment(FileAttachment attachment, int chunkSizeInBytes)
        {
            var fileName = $"{Guid.NewGuid()}_{attachment.OriginalName}";
            var buffer = new byte[chunkSizeInBytes];
            
            long uploadedBytes = 0;

            using (var fileSvc = ServiceManager.GetService<FileService>())
            {
                var bytesRead = attachment.FileStream.Read(buffer, 0, chunkSizeInBytes);

                while (bytesRead > 0)
                {
                    fileSvc.AppendChunk(fileName, buffer, uploadedBytes, bytesRead);
                
                    uploadedBytes += bytesRead;
                
                    bytesRead = attachment.FileStream.Read(buffer, 0, chunkSizeInBytes);
                }
            }
            
            return fileName;
        }
    }

}