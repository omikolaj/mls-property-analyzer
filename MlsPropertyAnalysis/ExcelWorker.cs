using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MlsPropertyAnalysis
{
    class ExcelWorker
    {
        public PropertyInfo PropertyInfo { get; set; }

        public void AnalyzeProperty()
        {
            Workbook propertyAnalysis = null;
            Application oExcel = null;
            Worksheet analysisWorkSheet = null;

            try
            {
                //create a instance for the Excel object  
                oExcel = new Application();

                string filepath = @"C:\Users\omikolajczyk\source\repos\MlsPropertyAnalysis\MlsPropertyAnalysis\property-analysis_filled.ods";

                //pass that to workbook object  
                propertyAnalysis = oExcel.Workbooks.Open(filepath);

                // grab first work sheet which is analysis
                analysisWorkSheet = (Worksheet)propertyAnalysis.Worksheets[1];

                analysisWorkSheet = SetDefaultValues(analysisWorkSheet);

                Thread.Sleep(500);

                var cashFlowObject = analysisWorkSheet.Cells.GetValue(Cells.TotalCashFlow);

                if (cashFlowObject != null)
                {
                    var cashFlow = String.Format("{0:0.00}", analysisWorkSheet.Cells.GetValue(Cells.TotalCashFlow));
                    if (cashFlow != string.Empty)
                    {
                        var cashFlowDecimal = decimal.Parse(cashFlow);
                        if (cashFlowDecimal >= 400)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Property: {PropertyInfo.Address} has desired cash flow!");
                            Console.ResetColor();
                            // make sure this file doesn't already exist
                            if (File.Exists($@"C:\Users\omikolajczyk\source\repos\MlsPropertyAnalysis\MlsPropertyAnalysis\houses\{PropertyInfo.Address}.ods") == false)
                            {
                                propertyAnalysis.SaveAs($@"C:\Users\omikolajczyk\source\repos\MlsPropertyAnalysis\MlsPropertyAnalysis\houses\{PropertyInfo.Address}.ods", XlFileFormat.xlOpenDocumentSpreadsheet, Type.Missing, Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Property: {PropertyInfo.Address} does NOT meet desired cash flow");
                            Console.ResetColor();
                        }
                    }
                }
            }
            finally
            {
                propertyAnalysis.Close(false);
                Marshal.ReleaseComObject(oExcel);
                Marshal.ReleaseComObject(analysisWorkSheet);
                Marshal.ReleaseComObject(propertyAnalysis);                
            }

        }

        private Worksheet SetDefaultValues(Worksheet analysisWorkSheet)
        {
            string afterRepairValue = (6500 + int.Parse(PropertyInfo.Price)).ToString();

            analysisWorkSheet.Cells.SetValue(Cells.PurchasePrice, PropertyInfo.Price);
            analysisWorkSheet.Cells.SetValue(Cells.PropertyTaxes, PropertyInfo.AnnualTaxes);
            analysisWorkSheet.Cells.SetValue(Cells.Improvements, 6500);
            analysisWorkSheet.Cells.SetValue(Cells.DownPaymentPercentage, 0.04);
            analysisWorkSheet.Cells.SetValue(Cells.InterestRatePercentage, 0.035);
            analysisWorkSheet.Cells.SetValue(Cells.MortgageTerm, 30);
            analysisWorkSheet.Cells.SetValue(Cells.NumberOfUnits, 2);
            analysisWorkSheet.Cells.SetValue(Cells.OtherMonthlyRev, 0);
            analysisWorkSheet.Cells.SetValue(Cells.VacancyRatePercentage, 0.12);
            analysisWorkSheet.Cells.SetValue(Cells.UnitOneBreakDown, 750);
            analysisWorkSheet.Cells.SetValue(Cells.UnitTwoBreakDown, 750);
            analysisWorkSheet.Cells.SetValue(Cells.AfterRepairValue, afterRepairValue);
            analysisWorkSheet.Cells.SetValue(Cells.PropertyInsurance, 850);
            analysisWorkSheet.Cells.SetValue(Cells.PropertyManagement_MonthlyRentPercentage, 0);
            analysisWorkSheet.Cells.SetValue(Cells.PropertyMaintenanceAndRepairs, 1200);
            analysisWorkSheet.Cells.SetValue(Cells.Advertising, 0);
            analysisWorkSheet.Cells.SetValue(Cells.PropertyUtilities, 1200);

            return analysisWorkSheet;
        }
    }
}
