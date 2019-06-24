using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace Create_documt_sluzhb_infrom
{
    class cl_exel
    {

        private Application application;
        private Workbook workbook;
        private Worksheet worksheet;

        public void ExelAction(int count, string s1, string s2) //int column,
        {
            
            application = new Application();
            const string template = "template.xlsm";
            workbook = application.Workbooks.Open(Path.Combine(Environment.CurrentDirectory, template));
            worksheet = workbook.ActiveSheet as Worksheet;

            worksheet.Range["A2"].Value = s1;
            worksheet.Range["B2"].Value = s2;
            for(int i = 1; i < count; i++)
                worksheet.Cells[1, i] = s1;
            application.Visible = true;
        }
    }
}
