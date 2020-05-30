using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Inventory
{
    /// <summary>
    /// PdfWrapper je klasa koja predstavlja jedan nivo apstrakcije 
    /// za rad sa biblioteko iText 7
    /// Implementira IDisposable interfejs pa moze da se korsiti sa using blokom
    /// </summary>
    public class PdfWrapper : IDisposable
    {
        private String path;

        private PdfWriter writer;
        private PdfDocument pdfDoc;
        private Document document;

        private PdfFont fontTimes;
        private PdfFont fontTimesBold;

        /// <summary>
        /// Instancira objekat klase PdfWrapper
        /// </summary>
        /// <param name="file">Pdf fajl u kome ce se upisivati podaci</param>
        public PdfWrapper(String path)
        {
            this.path = path;
            writer = new PdfWriter(path);
            pdfDoc = new PdfDocument(writer);
            document = new Document(pdfDoc, PageSize.A4);
            fontTimes = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN, PdfEncodings.CP1250, true);
            fontTimesBold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD, PdfEncodings.CP1250, true);
        }


        /// <summary>
        /// Dodaje u pdf tabelu koju pravi od liste objekata
        /// Tabela ima i header cells koji odgovaraju nazivima atributa tih objekata
        /// </summary>
        /// <typeparam name="T">Tip liste</typeparam>
        /// <param name="lista">Lista objekata koje treba uneti u tabelu</param>
        public void DodajTabelu<T>(List<T> lista)
        {
            if (lista.Count == 0)
                return;
            PropertyInfo[] allProperties = lista[0].GetType().GetProperties();

            var table = new Table(allProperties.Count<object>());
            table.SetWidth(UnitValue.CreatePercentValue(100));

            // Dodavanje header celija sa nazivima atributa klase
            foreach (var i in allProperties)
            {
                Cell cell = new Cell().Add(new Paragraph(i.Name));
                cell.SetBackgroundColor(new DeviceCmyk(0, 0, 0, 0.5f));
                table.AddHeaderCell(cell);
            }

            // Dodavanje celija sa podacima u tabelu
            foreach (var item in lista)
            {
                foreach (var prop in allProperties)
                    table.AddCell(prop.GetValue(item).ToString());
            }

            SetStyle(table);
            document.Add(table);
        }

        /// <summary>
        /// Dodaje u pdf jedan paragraf, prva linija je uvucena
        /// </summary>
        /// <param name="text">Tekst koji ce paragraf da sadrzi</param>
        public void DodajParagraf(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var p = new Paragraph(text);
                SetStyle(p);
                document.Add(p);
            }
        }

        /// <summary>
        /// Dodaje jedan naslov u pdf, naslov je bold i centriran
        /// </summary>
        /// <param name="text">Tekst koji ce biti u naslovu</param>
        /// <param name="fontSize">Velicina fonta naslova</param>
        public void DodajNaslov(string text, float fontSize = 22)
        {
            var p = new Paragraph();
            SetStyle(p);
            p.SetFontSize(fontSize);
            p.SetTextAlignment(TextAlignment.CENTER);
            p.Add(text);
            p.SetFont(fontTimesBold);
            document.Add(p);
        }

        /// <summary>
        /// Dodaje sliku u pdf, slika je centrirana
        /// </summary>
        /// <param name="slika">Slika koja ce biti dodata u pdf</param>
        /// <param name="scale">Skaliranje velicine slike</param>
        public void DodajSliku(iText.Layout.Element.Image slika, float scale = 1)
        {
            slika.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            slika.Scale(scale, scale);
            document.Add(slika);
        }

        /// <summary>
        /// Dodaje sliku u pdf, slika je centrirana
        /// </summary>
        /// <param name="path">Putanja do slike koja ce biti dodata u pdf</param>
        /// <param name="scale">Skaliranje velicine slike</param>
        public void DodajSliku(string path, float scale = 1)
        {
            var image = new iText.Layout.Element.Image(ImageDataFactory.Create(path));
            DodajSliku(image, scale);
        }


        /// <summary>
        /// Dodaje n praznih paragrafa i na taj nacin stvara razmak
        /// Ova funkcija je suvisna jer se isto to postize preko margine ali mi se ne brise
        /// </summary>
        /// <param name="velicina">Broj praznih paragrafa</param>
        public void DodajRazmak(int velicina = 5)
        {
            for (int i = 0; i < velicina; i++)
                DodajParagraf(" ");
        }

        /// <summary>
        /// Dodaje M.P. i mesto za potpis ovlascenog lica
        /// </summary>
        public void DodajMPiPotpis()
        {
            // Kreira se tabela od tri kolone jednake sirine i bez okvira
            // Prva je prazna, druga ima "M.P." i treba ima mesto za potpis
            // Na taj nacin postizem da se nalaze u istom redu
            var table = new Table(UnitValue.CreatePercentArray(new float[] { 5, 5, 5 }));
            table.SetWidth(UnitValue.CreatePercentValue(100));

            table.AddCell(new Cell()
                .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell()
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .Add(new Paragraph("M.P.")));
            table.AddCell(new Cell()
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("Ovlašćeno lice fakulteta \n\n ___________________________")));

            table.SetMarginTop(20);
            document.Add(table);
        }

        /// <summary>
        /// Dodaje mesto za potpis profesora 
        /// </summary>
        public void DodajPotpisProfesora()
        {
            //Radi slicno kao i funkcija iznad (dodajMPiPotpis)
            var table = new Table(UnitValue.CreatePercentArray(new float[] { 5, 5, 5 }));
            table.SetWidth(UnitValue.CreatePercentValue(100));

            table.AddCell(new Cell()
                .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell()
                .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell()
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("Profesor \n\n ___________________________")));

            table.SetMarginTop(20);
            document.Add(table);
        }

        /// <summary>
        /// Zatvara dokument
        /// </summary>
        public void Dispose()
        {
            document.Close();
        }

        /// <summary>
        /// Otvara fajl ili ispisuje gresku ako to nije moguce
        /// </summary>
        /// <param name="file">Fajl koji treba otvoriti</param>
        /// <returns></returns>
        public static void OtvoriFile(String path)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {path}"));
        }

        /// <summary>
        /// Postavlja neke atribute koji se cesto ponavljaju
        /// Postize se uniforman izgleda pdf-a
        /// </summary>
        /// <param name="element">Element za koji treba postaviti stil</param>
        private void SetStyle<T>(BlockElement<T> element) where T : IElement
        {
            element.SetFont(fontTimes);
            element.SetFontSize(14);
            element.SetMarginTop(10);
            element.SetMarginBottom(10);
        }
    }
}
