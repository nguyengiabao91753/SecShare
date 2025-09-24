using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Group;
public class GroupShare : BaseClass.BaseClass
{

    public Guid GroupId { get; set; }
    public Guid DocumentId { get; set; }
    public string SenderId { get; set; }
    public byte[] EncryptedAESKey { get; set; }
    public string Permissions { get; set; }
}
