using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace DiagramDesigner
{
    class CSVFile
    {
        /// <summary>
        /// 데이터를 csv 파일로 출력한다.
        /// </summary>
        /// <param name="ResizeDatas">데이터의 값들</param>
        public void CreateCSVFile(ArrayList ResizeDatas, string CsvPath)
        {
            StringBuilder CsvContent = new StringBuilder();

            for (int i = 0; i < ResizeDatas.Count; i++)
            {
                CsvContent.Append(ResizeDatas[i]);
                CsvContent.Append(",");

                if (i == ResizeDatas.Count - 1)
                {
                    CsvContent.Append(ResizeDatas[i]);
                    CsvContent.Append("\n");
                }
            }
            
            File.AppendAllText(CsvPath, CsvContent.ToString());

            ResizeDatas.Clear();
        }
    }
}
