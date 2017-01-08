using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyStocks.Dto
{
    /// <summary>
    /// stores portfolio information
    /// </summary>
    public class PortfolioDto
    {
        public List<AccountItemDto> AccountItems { get; set; } = new List<AccountItemDto>();
    }
}
