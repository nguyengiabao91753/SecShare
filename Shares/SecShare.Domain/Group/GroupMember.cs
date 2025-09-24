using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Group;
public class GroupMember : BaseClass.BaseClass
{

    public Guid GroupId { get; set; }
    public string UserId { get; set; }
    public string Role { get; set; }
}
