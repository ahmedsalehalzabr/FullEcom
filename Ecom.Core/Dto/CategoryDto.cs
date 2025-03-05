using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Dto
{
    // record لكي يعرف المبرمج بعدي انه عباره عن نقل بيانات وليس كلاس
    public record CategoryDto
    (string Name,string Description);
    public record UbdataCategoryDto
        (string Name,string Description,int id);
}
