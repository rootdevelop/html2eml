using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace Html2Eml
{
    public static class Html2EmlConvert
    {
        /// <summary>
        /// Converts HTML to an EML file
        /// </summary>
        /// <param name="html"></param>
        /// <param name="filename"></param>
        public static void ConvertHtml(string html, string filename = "output.eml")
        {
            if (string.IsNullOrWhiteSpace(html)) return;

            var mailMessage = new MailMessage("youremail@something.com", "recipient@something.com", "test e-mail", html)
            {
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8
            };

            using var fs = new FileStream(filename, FileMode.Create);

            mailMessage.ToEmlStream(fs);
        }

        /// <summary>
        /// Converts a MailMessage to an EML file stream.
        /// Thank you: https://stackoverflow.com/questions/1264672/how-to-save-mailmessage-object-to-disk-as-eml-or-msg-file
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        private static void ToEmlStream(this MailMessage msg, Stream str)
        {
            using var client = new SmtpClient();
            var id = Guid.NewGuid();

            var tempFolder = Path.Combine(Path.GetTempPath(), Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty);

            tempFolder = Path.Combine(tempFolder, "MailMessageToEMLTemp");

            // create a temp folder to hold just this .eml file so that we can find it easily.
            tempFolder = Path.Combine(tempFolder, id.ToString());

            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            client.UseDefaultCredentials = true;
            client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            client.PickupDirectoryLocation = tempFolder;
            client.Send(msg);

            // tempFolder should contain 1 eml file

            var filePath = Directory.GetFiles(tempFolder).Single();

            // stream out the contents
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                fs.CopyTo(str);
            }

            if (Directory.Exists(tempFolder))
            {
                Directory.Delete(tempFolder, true);
            }
        }
    }
}
