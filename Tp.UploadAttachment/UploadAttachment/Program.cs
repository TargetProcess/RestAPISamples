using System;
using System.Configuration;
using System.IO;
using Tp.AttachmentServiceProxy;
using Tp.FileServiceProxy;
using Tp.RequestServiceProxy;

namespace UploadAttachment
{
    class Program
    {

        private static ServiceManager serviceManager;

        private static void Main()
        {
            serviceManager = new ServiceManager();

            var filePath = ConfigurationManager.AppSettings["FilePath"];
            var fileName = ConfigurationManager.AppSettings["FileName"];
            FileAttachment fileAttachment = new FileAttachment
            {
                FileStream = new FileStream(filePath+fileName, FileMode.Open),
                OriginalName = fileName
            };

            var requestId = Int32.Parse(ConfigurationManager.AppSettings["RequestId"]);
            AddAttachmentToRequest(requestId, fileAttachment);
        }

        private static void AddAttachmentToRequest(int requestId, FileAttachment attachment)
        {
            var fileName = UploadAttachment(attachment, 20 * 1024);

            var requestSvc = serviceManager.GetService<RequestService>();
            var attachSvc = serviceManager.GetService<AttachmentService>();

            var attachmentId = requestSvc.AddAttachmentToRequest(requestId, fileName, attachment.Description);
            var request = requestSvc.GetByID(requestId);
            var attachmentDto = attachSvc.GetByID(attachmentId);
            attachmentDto.OriginalFileName = attachment.OriginalName;
            attachmentDto.OwnerID = request.OwnerID;
            attachSvc.Update(attachmentDto);
        }

        private static string UploadAttachment(FileAttachment attachment, int chunkSize)
        {
            var fileName = string.Format("{0}_{1}", Guid.NewGuid(), attachment.OriginalName);
            var fileSvc = serviceManager.GetService<FileService>();

            using (var stream = attachment.FileStream)
            {
                var buffer = new byte[chunkSize];
                var bytesRead = stream.Read(buffer, 0, chunkSize);
                long uploadedLength = 0;

                while (bytesRead > 0)
                {
                    fileSvc.AppendChunk(fileName, buffer, uploadedLength, bytesRead);
                    uploadedLength += bytesRead;
                    buffer = new byte[chunkSize];
                    bytesRead = stream.Read(buffer, 0, chunkSize);
                }
            }
            return fileName;
        }
    }

}