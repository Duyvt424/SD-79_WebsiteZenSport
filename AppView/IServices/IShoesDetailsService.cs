using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IServices
{
    public interface IShoesDetailsService
    {
        public bool AddShoesDetails(ShoesDetails shoesDetails);
        public bool UpdateShoesDetails(ShoesDetails shoesDetails);
        public bool DeleteShoesDetails(Guid id);
        public List<ShoesDetails> GetAllShoesDetails();
        public ShoesDetails GetShoesDetailsById(Guid id);
    }
}
