using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using ProjetoAspNetMVC01.Repository.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProjetoAspNetMVC01.Reports.Pdf
{
    public class TarefasReportPdf
    {
        public static byte[] Create(DateTime dataMin, DateTime dataMax,
  Usuario usuario, List<Tarefa> tarefas)
        {
            //criando um documento PDF
            var memoryStream = new MemoryStream();
            var pdf = new PdfDocument(new PdfWriter(memoryStream));

            using (var doc = new Document(pdf))
            {
                var fmtTitulo = new Style();
                fmtTitulo.SetFontSize(24);
                fmtTitulo.SetBold();

                var fmtTexto = new Style();
                fmtTexto.SetFontSize(12);

                var img = ImageDataFactory.Create
("https://www.cotiinformatica.com.br/imagens/ logo - coti - informatica.png");
                doc.Add(new Image(img));

                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph
("Relatório de Tarefas").AddStyle(fmtTitulo));

                doc.Add(new Paragraph($"Data de início: { dataMin.ToString("dd/MM/yyyy") }").AddStyle(fmtTexto));
                doc.Add(new Paragraph($"Data de término: { dataMax.ToString("dd/MM/yyyy") }").AddStyle(fmtTexto));


                doc.Add(new Paragraph($"Nome do Usuário: { usuario.Nome }").AddStyle(fmtTexto));
                doc.Add(new Paragraph($"Email: { usuario.Email }").AddStyle(fmtTexto));
                doc.Add(new Paragraph("\n"));

                //tabela para exibir as tarefas
                var table = new Table(5); //5 colunas
                table.SetWidth(UnitValue.CreatePercentValue(100));

                table.AddHeaderCell("Nome da tarefa");
                table.AddHeaderCell("Data");
                table.AddHeaderCell("Hora");
                table.AddHeaderCell("Descrição");
                table.AddHeaderCell("Prioridade");

                foreach (var item in tarefas)
                {
                    table.AddCell(item.Nome);
                    table.AddCell(item.Data.ToString("dd/MM/yyyy"));
                    table.AddCell(item.Hora.ToString(@"hh\:mm"));
                    table.AddCell(item.Descricao);
                    table.AddCell(item.Prioridade);
                }

                doc.Add(table);
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph($"Quantidade de tarefas: { tarefas.Count }").AddStyle(fmtTexto));
                doc.Add(new Paragraph($"Relatório gerado em: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}").AddStyle(fmtTexto));
            }

            return memoryStream.ToArray();
        }
    }
}
