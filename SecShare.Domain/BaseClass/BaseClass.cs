using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.BaseClass;
public abstract class BaseClass
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //db tự động tăng 1
    [Key]
    public int ID { get; set; } = 0;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
}
