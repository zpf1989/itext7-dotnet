/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using System.Collections.Generic;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Utils;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using iText.Test;

namespace iText.Layout {
    public class CollapsingMarginsTest : ExtendedITextTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/layout/CollapsingMarginsTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/layout/CollapsingMarginsTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateOrClearDestinationFolder(destinationFolder);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void CollapsingMarginsTest01() {
            String outFileName = destinationFolder + "collapsingMarginsTest01.pdf";
            String cmpFileName = sourceFolder + "cmp_collapsingMarginsTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            DrawPageBorders(pdfDocument, 4);
            String textByron = "When a man hath no freedom to fight for at home,\n" + "    Let him combat for that of his neighbours;\n"
                 + "Let him think of the glories of Greece and of Rome,\n" + "    And get knocked on the head for his labours.\n"
                 + "\n" + "To do good to Mankind is the chivalrous plan,\n" + "    And is always as nobly requited;\n"
                 + "Then battle for Freedom wherever you can,\n" + "    And, if not shot or hanged, you'll get knighted.";
            Document doc = new Document(pdfDocument);
            doc.SetProperty(Property.COLLAPSING_MARGINS, true);
            doc.Add(new Paragraph("marker text").SetMargin(0));
            Paragraph p = new Paragraph(textByron);
            for (int i = 0; i < 5; i++) {
                p.Add(textByron);
            }
            Div div1 = new Div();
            Div div2 = new Div();
            div1.Add(p).SetBackgroundColor(new DeviceRgb(65, 151, 29));
            div2.Add(p).SetBackgroundColor(new DeviceRgb(209, 247, 29));
            div1.SetMarginBottom(20);
            div2.SetMarginTop(150);
            div2.SetMarginBottom(150);
            Div div = new Div().SetMarginTop(20).SetMarginBottom(10).SetBackgroundColor(new DeviceRgb(78, 151, 205));
            div.Add(div1);
            div.Add(div2);
            doc.Add(div);
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void CollapsingMarginsTest02() {
            String outFileName = destinationFolder + "collapsingMarginsTest02.pdf";
            String cmpFileName = sourceFolder + "cmp_collapsingMarginsTest02.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            DrawPageBorders(pdfDocument, 3);
            String textByron = "When a man hath no freedom to fight for at home,\n" + "    Let him combat for that of his neighbours;\n"
                 + "Let him think of the glories of Greece and of Rome,\n" + "    And get knocked on the head for his labours.\n"
                 + "\n" + "To do good to Mankind is the chivalrous plan,\n" + "    And is always as nobly requited;\n"
                 + "Then battle for Freedom wherever you can,\n" + "    And, if not shot or hanged, you'll get knighted.";
            Document doc = new Document(pdfDocument);
            doc.SetProperty(Property.COLLAPSING_MARGINS, true);
            doc.Add(new Paragraph("marker text").SetMargin(0));
            Paragraph p = new Paragraph(textByron);
            for (int i = 0; i < 3; i++) {
                p.Add(textByron);
            }
            p.Add("When a man hath no freedom to fight for at home,\n" + "    Let him combat for that of his neighbours;\n"
                 + "Let him think of the glories of Greece and of Rome,\n" + "    And get knocked on the head for his labours.\n"
                 + "\n" + "To do good to Mankind is the chivalrous plan,\n");
            Div div1 = new Div();
            Div div2 = new Div();
            div1.Add(p).SetBackgroundColor(new DeviceRgb(65, 151, 29));
            div2.Add(p).SetBackgroundColor(new DeviceRgb(209, 247, 29));
            div1.SetMarginBottom(40);
            div2.SetMarginTop(20);
            div2.SetMarginBottom(150);
            Div div = new Div().SetMarginTop(20).SetMarginBottom(10).SetBackgroundColor(new DeviceRgb(78, 151, 205));
            div.Add(div1);
            div.Add(div2);
            doc.Add(div);
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void CollapsingMarginsTest03() {
            String outFileName = destinationFolder + "collapsingMarginsTest03.pdf";
            String cmpFileName = sourceFolder + "cmp_collapsingMarginsTest03.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            DrawPageBorders(pdfDocument, 3);
            String textByron = "When a man hath no freedom to fight for at home,\n" + "    Let him combat for that of his neighbours;\n"
                 + "Let him think of the glories of Greece and of Rome,\n" + "    And get knocked on the head for his labours.\n"
                 + "\n" + "To do good to Mankind is the chivalrous plan,\n" + "    And is always as nobly requited;\n"
                 + "Then battle for Freedom wherever you can,\n" + "    And, if not shot or hanged, you'll get knighted.";
            Document doc = new Document(pdfDocument);
            doc.SetProperty(Property.COLLAPSING_MARGINS, true);
            doc.Add(new Paragraph("marker text").SetMargin(0));
            Paragraph p = new Paragraph(textByron);
            for (int i = 0; i < 3; i++) {
                p.Add(textByron);
            }
            p.Add("When a man hath no freedom to fight for at home,\n" + "    Let him combat for that of his neighbours;\n"
                 + "Let him think of the glories of Greece and of Rome,\n" + "    And get knocked on the head for his labours.\n"
                 + "To do good to Mankind is the chivalrous plan,\n");
            Div div1 = new Div();
            Div div2 = new Div();
            div1.Add(p).SetBackgroundColor(new DeviceRgb(65, 151, 29));
            div2.Add(p).SetBackgroundColor(new DeviceRgb(209, 247, 29));
            div1.SetMarginBottom(80);
            div2.SetMarginTop(80);
            div2.SetMarginBottom(150);
            doc.Add(div1);
            doc.Add(div2);
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void CollapsingMarginsTest04() {
            String outFileName = destinationFolder + "collapsingMarginsTest04.pdf";
            String cmpFileName = sourceFolder + "cmp_collapsingMarginsTest04.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            DrawPageBorders(pdfDocument, 3);
            String textByron = "When a man hath no freedom to fight for at home,\n" + "    Let him combat for that of his neighbours;\n"
                 + "Let him think of the glories of Greece and of Rome,\n" + "    And get knocked on the head for his labours.\n"
                 + "\n" + "To do good to Mankind is the chivalrous plan,\n" + "    And is always as nobly requited;\n"
                 + "Then battle for Freedom wherever you can,\n" + "    And, if not shot or hanged, you'll get knighted.";
            Document doc = new Document(pdfDocument);
            doc.SetProperty(Property.COLLAPSING_MARGINS, true);
            doc.Add(new Paragraph("marker text").SetMargin(0));
            Paragraph p = new Paragraph(textByron);
            for (int i = 0; i < 3; i++) {
                p.Add(textByron);
            }
            p.Add("When a man hath no freedom to fight for at home,\n" + "    Let him combat for that of his neighbours;\n"
                 + "Let him think of the glories of Greece and of Rome,\n" + "    And get knocked on the head for his labours.\n"
                 + "To do good to Mankind is the chivalrous plan,\n");
            p.Add(new Text("small text").SetFontSize(5.1f));
            p.Add("\nAnd is always as nobly requited;\n" + "Then battle for Freedom wherever you can,\n" + "And, if not shot or hanged, you'll get knighted."
                );
            Div div1 = new Div();
            Div div2 = new Div();
            div1.Add(p).SetBackgroundColor(new DeviceRgb(65, 151, 29));
            div2.Add(p).SetBackgroundColor(new DeviceRgb(209, 247, 29));
            div1.SetMarginBottom(80);
            div2.SetMarginTop(80);
            div2.SetMarginBottom(150);
            doc.Add(div1);
            doc.Add(div2);
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void CollapsingMarginsTest05() {
            String outFileName = destinationFolder + "collapsingMarginsTest05.pdf";
            String cmpFileName = sourceFolder + "cmp_collapsingMarginsTest05.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            DrawPageBorders(pdfDocument, 2);
            String textByron = "When a man hath no freedom to fight for at home,\n" + "    Let him combat for that of his neighbours;\n"
                 + "Let him think of the glories of Greece and of Rome,\n" + "    And get knocked on the head for his labours.\n"
                 + "\n" + "To do good to Mankind is the chivalrous plan,\n" + "    And is always as nobly requited;\n"
                 + "Then battle for Freedom wherever you can,\n" + "    And, if not shot or hanged, you'll get knighted.";
            Document doc = new Document(pdfDocument);
            doc.SetProperty(Property.COLLAPSING_MARGINS, true);
            Paragraph p = new Paragraph(textByron).SetBackgroundColor(ColorConstants.YELLOW);
            for (int i = 0; i < 3; i++) {
                p.Add(textByron);
            }
            doc.Add(p);
            p.SetMarginTop(80);
            Div div = new Div();
            div.Add(p).SetBackgroundColor(new DeviceRgb(65, 151, 29));
            doc.Add(div);
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        private void DrawPageBorders(PdfDocument pdfDocument, int pageNum) {
            for (int i = 1; i <= pageNum; ++i) {
                while (pdfDocument.GetNumberOfPages() < i) {
                    pdfDocument.AddNewPage();
                }
                PdfCanvas canvas = new PdfCanvas(pdfDocument.GetPage(i));
                canvas.SaveState();
                canvas.SetLineDash(5, 10);
                canvas.Rectangle(36, 36, 595 - 36 * 2, 842 - 36 * 2);
                canvas.Stroke();
                canvas.RestoreState();
            }
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ColumnRendererTest() {
            NUnit.Framework.Assert.That(() =>  {
                // TODO DEVSIX-2901 the exception should not be thrown
                String outFileName = destinationFolder + "columnRendererTest.pdf";
                String cmpFileName = sourceFolder + "cmp_columnRendererTest.pdf";
                PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
                String textByron = "When a man hath no freedom to fight for at home,\n" + "    Let him combat for that of his neighbours;\n"
                     + "Let him think of the glories of Greece and of Rome,\n" + "    And get knocked on the head for his labours.\n"
                     + "\n" + "To do good to Mankind is the chivalrous plan,\n" + "    And is always as nobly requited;\n"
                     + "Then battle for Freedom wherever you can,\n" + "    And, if not shot or hanged, you'll get knighted.";
                Document doc = new Document(pdfDocument);
                doc.SetProperty(Property.COLLAPSING_MARGINS, true);
                Paragraph p = new Paragraph();
                for (int i = 0; i < 10; i++) {
                    p.Add(textByron);
                }
                Div div = new Div().Add(p);
                IList<Rectangle> areas = new List<Rectangle>();
                areas.Add(new Rectangle(30, 30, 150, 600));
                areas.Add(new Rectangle(200, 30, 150, 600));
                areas.Add(new Rectangle(370, 30, 150, 600));
                div.SetNextRenderer(new CollapsingMarginsTest.CustomColumnDocumentRenderer(div, areas));
                doc.Add(div);
                doc.Close();
                NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                    , "diff"));
            }
            , NUnit.Framework.Throws.InstanceOf<ArgumentOutOfRangeException>())
;
        }

        private class CustomColumnDocumentRenderer : DivRenderer {
            private IList<Rectangle> areas;

            public CustomColumnDocumentRenderer(Div modelElement, IList<Rectangle> areas)
                : base(modelElement) {
                this.areas = areas;
            }

            public override LayoutResult Layout(LayoutContext layoutContext) {
                LayoutResult result = base.Layout(layoutContext);
                return result;
            }

            public override IList<Rectangle> InitElementAreas(LayoutArea area) {
                return areas;
            }

            public override IRenderer GetNextRenderer() {
                return new CollapsingMarginsTest.CustomColumnDocumentRenderer((Div)modelElement, areas);
            }
        }
    }
}
