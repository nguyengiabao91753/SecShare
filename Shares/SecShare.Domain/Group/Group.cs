using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Group;
public class Group
{
    [Key]
    public Guid GroupId { get; set; }
    public string GroupName { get; set; }
    public string OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
}
