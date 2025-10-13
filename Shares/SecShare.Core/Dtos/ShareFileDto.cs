using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Dtos;
public class ShareFileDto
{
    public Guid DocumentId { get; set; }
    [EmailAddress]
    [Required]
    public string ReceiverEmail { get; set; }
    public string? Permissions { get; set; }
}
