using System.Collections.Generic;

namespace SwEngHomework.Commissions.Models
{
    class CommissionCalculatorModel
    {
        public IEnumerable<Advisor> advisors { get; set; }
        public IEnumerable<Account> accounts { get; set; }
    }
}
