using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Services.Utilidades
{
    /// <summary>
    /// Expone diferentes utilidades sobre archivos Excel (xlsx)
    /// </summary>
    public class UtilidadesExcel
    {
        /// <summary>
        /// Determina el total de registros que contiene un archivo Excel en una hoja en concreto
        /// </summary>
        /// <param name="ruta_archivo">Ruta del archivo a verificar</param>
        /// <param name="hoja">Hoja en la cual se realizará el conteo</param>
        /// <returns>La cantidad de regitros presentes en la hoja indicada en el archivo indicado</returns>
        public static string totalRegistros(string ruta_archivo, int hoja)
        {
            string REGISTROS = "0";
            try
            {
                XSSFWorkbook LibroExcel = new XSSFWorkbook(ruta_archivo);
                XSSFSheet HojaExcel = LibroExcel.GetSheetAt(hoja) as XSSFSheet;
                try
                {
                    REGISTROS = HojaExcel.LastRowNum.ToString();
                }
                catch (Exception exe)
                {

                }
                finally
                {
                    LibroExcel.Close();
                }
            }
            catch (Exception exe)
            {

            }

            return REGISTROS;
        }

        /// <summary>
        /// Determinar si un archivo de Excel se encuentra vacío en una hoja en concreto
        /// </summary>
        /// <param name="RUTA">Ruta del archivo a verificar</param>
        /// <param name="hoja">Hoja a verificar dentro del archivo</param>
        /// <returns><see langword="true" /> si el archivo se encuentra vacío en la hoja especificada, <see langword="false" /> en caso contrario</returns>
        public static bool ARCHIVO_VACIO(string RUTA, int hoja)
        {
            bool resultado = true;
            try
            {
                XSSFWorkbook LibroExcel = new XSSFWorkbook(RUTA);
                XSSFSheet HojaExcel = LibroExcel.GetSheetAt(hoja) as XSSFSheet;
                try
                {

                    if (HojaExcel.LastRowNum >= 1)
                    {
                        resultado = false;
                    }
                }
                catch (Exception exe)
                {

                }
                finally
                {
                    LibroExcel.Close();
                }
            }
            catch (Exception exe)
            {

            }

            return resultado;
        }

        public static string getCellValue(int numero_celda, IRow fila)
        {
            string value = "";

            try
            {
                ICell celda = fila.GetCell(numero_celda);
                switch (celda.CellType)
                {
                    case CellType.String:
                        value = celda.StringCellValue;
                        break;
                    case CellType.Numeric:
                        if (DateUtil.IsCellDateFormatted(celda))
                        {
                            try
                            {
                                value = celda.DateCellValue.ToString();
                            }
                            catch (NullReferenceException)
                            {
                                value = DateTime.FromOADate(celda.NumericCellValue).ToString();
                            }
                        }
                        else
                        {
                            value = celda.NumericCellValue.ToString();
                        }
                        break;
                    case CellType.Blank:
                        value = "";
                        break;
                    default:
                        //set default
                        break;
                }
            }
            catch (Exception exe)
            {

            }

            return value;
        }

        public static DateTime? getCellDateValue(int numero_celda, IRow fila)
        {
            DateTime? value = null;

            try
            {
                ICell celda = fila.GetCell(numero_celda);
                switch (celda.CellType)
                {
                    case CellType.Numeric:
                        if (DateUtil.IsCellDateFormatted(celda))
                        {
                            try
                            {
                                value = celda.DateCellValue;
                            }
                            catch (NullReferenceException)
                            {
                                value = DateTime.FromOADate(celda.NumericCellValue);
                            }
                        }
                        break;
                }
            }
            catch (Exception exe)
            {

            }

            return value;
        }

        public static NPOI.XSSF.UserModel.XSSFCellStyle createStyle(byte[] rgb, IWorkbook workbook)
        {
            NPOI.XSSF.UserModel.XSSFColor color = new NPOI.XSSF.UserModel.XSSFColor(rgb);

            NPOI.XSSF.UserModel.XSSFCellStyle boldStyle = (NPOI.XSSF.UserModel.XSSFCellStyle)workbook.CreateCellStyle();
            boldStyle.SetFillForegroundColor(color);
            boldStyle.FillPattern = FillPattern.SolidForeground;
            boldStyle.Alignment = HorizontalAlignment.Center;
            boldStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont font = workbook.CreateFont();
            font.Color = NPOI.SS.UserModel.IndexedColors.White.Index;
            boldStyle.SetFont(font);

            return boldStyle;
        }
    }
}
