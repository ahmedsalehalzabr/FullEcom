using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Sharing
{
    public class EmailStringBody
    {
        public static string send(string email,string token,string component,string messge)
        {
            string encodeToken=Uri.EscapeDataString(token);
            return $@"

                 <html>
                  <head></head>
                    <body>
                       <h>{messge}</h>
                          <hr>
                          <br>
                         <a href=""http://localhost:4200/Account/{component}?email={email}&code={encodeToken}"">{messge}</a>
                    </body>
                 </html>
                 ";
        }
    }
}
