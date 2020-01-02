using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
   public class FbBookshelf
    {
        public async static Task<Bookshelf.Totais> GetBookshelfTotais()
        {
            return await AL.ABookshelf.GetBookshelfTotais(1);
        }
    }
}
