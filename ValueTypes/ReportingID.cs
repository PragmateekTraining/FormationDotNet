using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueTypes
{
    struct ReportID
    {
        public string UserName { get; private set; }
        public int Folio { get; private set; }

        public ReportID(string userName, int folio)
            : this()
        {
            UserName = userName;
            Folio = folio;
        }
    }
}
