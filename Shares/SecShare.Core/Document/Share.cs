using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Document;
public class Share : BaseClass.BaseClass
{
    public Guid DocumentId { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
    public byte[] EncryptedAESKey { get; set; }
    public string Permissions { get; set; }
}
