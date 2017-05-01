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
        /// <summary>
        /// timestamp of the last change of the portfolio, used to sync local with remote portfolio
        /// </summary>
        public DateTime LastChange { get; set; }
        public List<AccountItemDto> AccountItems { get; set; } = new List<AccountItemDto>();
    }
}
