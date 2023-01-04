using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigizuiteClientExample.Requests
{
    internal class AssetReplaceRequest
    {
        public string FileName { get; set; }
        public int AssetId { get; set; }
        public byte[] File { get; set; }
    }
}
