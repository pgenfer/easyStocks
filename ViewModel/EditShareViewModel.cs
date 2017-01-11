using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public class EditShareViewModel
    { 
        public void ResetStopQuote()
        {
        }



        public DateTime BuyingDate { get; set; }
        
        public float BuyingRate { get; set; }

        public string ShareSymbol { get; }
        public string ShareName { get; }
    }
}
