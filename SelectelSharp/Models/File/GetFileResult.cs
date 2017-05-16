using SelectelSharp.Headers;
using System.Collections.Specialized;

namespace SelectelSharp.Models.File
{
    public class GetFileResult : FileInfo
    {                
        public byte[] File { get; set; }

        public GetFileResult(byte[] file, string name, NameValueCollection headers)
        {
            HeaderParsers.ParseHeaders(this, headers);
            this.File = file;
            this.Name = name;
        }
    }
}
