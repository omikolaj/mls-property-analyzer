using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlsPropertyAnalysis
{
    class Cells
    {
        public const string PurchasePrice = "D3";
        public const string Improvements = "D6";
        public const string DownPaymentPercentage = "H3";
        public const string InterestRatePercentage = "H6";
        public const string MortgageTerm = "H7";
        public const string NumberOfUnits = "L3";
        public const string OtherMonthlyRev = "L5";
        public const string VacancyRatePercentage = "L8";
        public const string UnitOneBreakDown = "T3";
        public const string UnitTwoBreakDown = "T4";
        public const string AfterRepairValue = "V3";

        public const string PropertyTaxes = "E24";
        public const string PropertyInsurance = "E25";
        /// <summary>
        /// Property management value is a percentage of monthly rent
        /// </summary>
        public const string PropertyManagement_MonthlyRentPercentage = "E25";
        public const string PropertyMaintenanceAndRepairs = "E27";
        public const string Advertising = "E28";
        public const string PropertyUtilities = "E29";

        public const string NOI_CashAvailable = "E39";
        public const string Mortgage = "E40";
        public const string TotalCashFlow = "E41";

        public const string FirstYear_CashROIPercentage = "F42";
    }
}
