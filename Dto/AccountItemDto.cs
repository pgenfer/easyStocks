using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyStocks.Dto
{
    /// <summary>
    /// used to store information of an account item
    /// in a persistance storage
    /// </summary>
    public class AccountItemDto
    {
        public string Symbol { get; set; }
        public DateTime BuyingDate { get; set; }
        public float BuyingRate { get; set; }
        public float StopRate { get; set; }
    }
}
