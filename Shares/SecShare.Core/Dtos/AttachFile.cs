using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SecShare.Core.Dtos;
public class AttachFile
{
    public string FileName { get; set; }

    public string FileNameChange { get; set; }

    public string Type { get; set; }

    public long FileSize { get; set; } 
    public IFormFile File { get; set; }


}
