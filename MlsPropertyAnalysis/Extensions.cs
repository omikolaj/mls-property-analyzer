using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MlsPropertyAnalysis
{
    public static class Extensions
    {
        public static void SetValue(this Range range, string cellAddress, object value)
        {
            var cell = range.get_Range(cellAddress);
            Object[] args1 = new Object[1];
            args1[0] = value;
            cell.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, cell, args1);
        }

        public static object GetValue(this Range range, string cellAddress)
        {
            var cell = range.get_Range(cellAddress);            
            return cell.GetType().InvokeMember("Value", BindingFlags.GetProperty, null, cell, null);
        }
    }
}
