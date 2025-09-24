using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Document;
public class Document : BaseClass.BaseClass
{
    public string OwnerId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public long FileSize { get; set; }
    public byte[] Ciphertext { get; set; }
    public bool IsDeleted { get; set; }
}
