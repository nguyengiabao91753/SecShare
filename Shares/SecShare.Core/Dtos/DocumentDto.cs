using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Dtos;
public class DocumentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public long FileSize { get; set; }

    public string? OwnerEmail { get; set; }
    public string FileType { get; set; }

    public DateTime UpdatedAt { get; set; }
}
