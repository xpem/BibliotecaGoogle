using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class BookStatus
    {
        public string Key { get; set; }
        public string IdBook { get; set; }

        public int IdUsuario { get; set; }

        public int Status { get; set; }

        public int Avaliacao { get; set; }
    }
}
