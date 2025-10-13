using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Dtos;
public class UploadMyFileDto
{
    public AttachFile AttachFile { get; set; }
    public string UserId { get; set; }
}
