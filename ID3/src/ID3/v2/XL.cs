using Excel = Microsoft.Office.Interop.Excel;

namespace ID3.src.ID3.v2
{
    public class XL
    {
        private static Excel.Application m_ExcelApplication;
        private static Excel.Workbook m_WorkBook;
        private static Excel.Worksheet[] m_WorkSheetCollection;

        public static void Create(string[][] data)
        {
            m_ExcelApplication = new Excel.Application();
            m_WorkBook = m_ExcelApplication.Workbooks.Add();
            m_WorkSheetCollection = new Excel.Worksheet[1];

            m_WorkSheetCollection[0] =
                m_WorkBook.Worksheets.Add();
            m_WorkSheetCollection[0].Name = "Name";
            m_WorkSheetCollection[0].StandardWidth
                = 30;

            for (int i = 0; i < data.Length; i++)
            {
                m_WorkSheetCollection[0].Cells[i + 1, 1] =
                        data[i][0];
                m_WorkSheetCollection[0].Cells[i + 1, 2] =
                        data[i][1];
                m_WorkSheetCollection[0].Cells[i + 1, 3] =
                        data[i][2];
                m_WorkSheetCollection[0].Cells[i + 1, 4] =
                        data[i][3];
                m_WorkSheetCollection[0].Cells[i + 1, 5] =
                        data[i][4];
                m_WorkSheetCollection[0].Cells[i + 1, 6] =
                        data[i][5];
            }

            m_ExcelApplication.Visible = true;
            m_ExcelApplication.UserControl = true;
        }
    }
}
