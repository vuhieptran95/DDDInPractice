using DDDInPractice.Domains;

namespace DDDInPractice.UI.Web.Controllers
{
    public partial class V1VendingMachinesController
    {
        public class MoneyViewModel
        {
            public int Five { get; set; }
            public int Ten { get; set; }
            public int Twenty { get; set; }
            public int Fifty { get; set; }
            public int OneHundred { get; set; }
            public int TwoHundred { get; set; }
            public int FiveHundred { get; set; }

            public Money CreateMoney()
            {
                return new Money(Five, Ten, Twenty, Fifty, OneHundred, TwoHundred, FiveHundred);
            }
        }
    }
}