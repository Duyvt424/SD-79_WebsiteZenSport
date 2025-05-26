using AppData.Models;
using AppView.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppView.IRepositories
{
    public interface IShoesDetailsService
    {
        public bool AddShoesDetails(ShoesDetails shoesDetails);
        public bool UpdateShoesDetails(ShoesDetails shoesDetails);
        public bool DeleteShoesDetails(Guid id);
        public List<ShoesDetails> GetAllShoesDetails();
        public List<shoesDetailsDTO> GetallShoedetailDtO();
        public ShoesDetails GetShoesDetailsById(Guid id);
    }
}
