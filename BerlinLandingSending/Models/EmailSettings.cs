using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerlinLandingSending.Models
{
    public class EmailSettings
    {
        public String SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public String SmtpLogin { get; set; }

        public String SmtpPassword { get; set; }

        public String FromEmail { get; set; }

        public String DomainName { get; set; }
    }
}
