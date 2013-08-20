using Microsoft.SharePoint;

namespace Sponge.Extensions
{
    public static class SPFileExtensions
    {
        public static string GetFileSize(this SPFile file)
        {
            var fileSize = file.Length;

            string[] suffix = { "bytes", "KB", "MB", "GB" };
            long j = 0;

            while (fileSize > 1024 && j < 4)
            {
                fileSize = fileSize / 1024;
                j++;
            }
            return (fileSize + " " + suffix[j]);
        }
    }
}
