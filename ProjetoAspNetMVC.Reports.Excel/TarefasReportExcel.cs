using OfficeOpenXml;
using OfficeOpenXml.Style;
using ProjetoAspNetMVC01.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ProjetoAspNetMVC01.Reports.Excel
{
    public class TarefasReportExcel
    {
        //método para gerar o conteudo do relatorio
        public static byte[] Create(DateTime dataMin, DateTime dataMax, Usuario usuario, List<Tarefa> tarefas)
        {
            //definindo o uso do EPPLUS atraves de uma licença não comercial
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var white = ColorTranslator.FromHtml("#FFFFFF");
            var darkGrey = ColorTranslator.FromHtml("#363636");
            var lightGrey = ColorTranslator.FromHtml("#DCDCDC");

            //gerando o arquivo do relatorio
            using (var excelPackage = new ExcelPackage())
            {
                //nome da planilha que será criada no arquivo
                var sheet = excelPackage.Workbook.Worksheets.Add("Tarefas");

                //adicionando as celulas
                sheet.Cells["A1"].Value = "Relatório de tarefas";

                var titulo = sheet.Cells["A1:E1"];
                titulo.Merge = true;
                titulo.Style.Font.Size = 16;
                titulo.Style.Font.Bold = true;
                titulo.Style.Font.Color.SetColor(white);
                titulo.Style.Fill.PatternType = ExcelFillStyle.Solid;
                titulo.Style.Fill.BackgroundColor.SetColor(darkGrey);
                titulo.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                sheet.Cells["A3"].Value = "Data de Início:";
                sheet.Cells["A4"].Value = "Data de Término:";
                sheet.Cells["A5"].Value = "Usuário:";

                sheet.Cells["B3"].Value = dataMin.ToString("dd/MM/yyyy");
                sheet.Cells["B4"].Value = dataMax.ToString("dd/MM/yyyy");
                sheet.Cells["B5"].Value = usuario.Nome;

                sheet.Cells["A7"].Value = "Nome da tarefa";
                sheet.Cells["B7"].Value = "Data";
                sheet.Cells["C7"].Value = "Hora";
                sheet.Cells["D7"].Value = "Descrição";
                sheet.Cells["E7"].Value = "Prioridade";

                var cabecalhos = sheet.Cells["A7:E7"];
                cabecalhos.Style.Font.Size = 12;
                cabecalhos.Style.Font.Bold = true;
                cabecalhos.Style.Font.Color.SetColor(white);
                cabecalhos.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cabecalhos.Style.Fill.BackgroundColor.SetColor(darkGrey);
                cabecalhos.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var linha = 8;
                foreach (var item in tarefas)
                {
                    sheet.Cells[$"A{linha}"].Value = item.Nome;
                    sheet.Cells[$"B{linha}"].Value = item.Data.ToString("dd/MM/yyyy");
                    sheet.Cells[$"C{linha}"].Value = item.Hora.ToString(@"hh\:mm");
                    sheet.Cells[$"D{linha}"].Value = item.Descricao;
                    sheet.Cells[$"E{linha}"].Value = item.Prioridade;

                    var celulas = sheet.Cells[$"A{linha}:E{linha}"];
                    if (linha % 2 == 0)
                    {
                        celulas.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        celulas.Style.Fill.BackgroundColor.SetColor(lightGrey);
                    }

                    linha++;
                }

                //ajuste da largura das colunas
                sheet.Cells["A:E"].AutoFitColumns();

                var conteudo = sheet.Cells[$"A7:E{linha - 1}"];
                conteudo.Style.Border.BorderAround(ExcelBorderStyle.Medium);

                //retornar o conteudo do arquivo
                return excelPackage.GetAsByteArray();
            }
        }
    }
}





