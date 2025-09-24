using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Document;
public class PendingShare : BaseClass.BaseClass
{
    public Guid DocumentId { get; set; }
    public string SenderId { get; set; }
    public string RecipientEmail { get; set; }
    public byte[] AESKeyRaw { get; set; }
    public string Status { get; set; }
}
