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
using iText.IO.Util;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Filespec;
using iText.Kernel.Pdf.Navigation;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Utils;
using iText.Test;

namespace iText.Kernel.Pdf {
    public class PdfDestinationTest : ExtendedITextTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/kernel/pdf/PdfDestinationTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/kernel/pdf/PdfDestinationTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateOrClearDestinationFolder(destinationFolder);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void DestTest01() {
            String srcFile = sourceFolder + "simpleNoLinks.pdf";
            String outFile = destinationFolder + "destTest01.pdf";
            String cmpFile = sourceFolder + "cmp_destTest01.pdf";
            PdfDocument document = new PdfDocument(new PdfReader(srcFile), new PdfWriter(outFile));
            PdfPage firstPage = document.GetPage(1);
            PdfLinkAnnotation linkExplicitDest = new PdfLinkAnnotation(new Rectangle(35, 785, 160, 15));
            linkExplicitDest.SetAction(PdfAction.CreateGoTo(PdfExplicitDestination.CreateFit(document.GetPage(2))));
            firstPage.AddAnnotation(linkExplicitDest);
            PdfLinkAnnotation linkStringDest = new PdfLinkAnnotation(new Rectangle(35, 760, 160, 15));
            PdfExplicitDestination destToPage3 = PdfExplicitDestination.CreateFit(document.GetPage(3));
            String stringDest = "thirdPageDest";
            document.AddNamedDestination(stringDest, destToPage3.GetPdfObject());
            linkStringDest.SetAction(PdfAction.CreateGoTo(new PdfStringDestination(stringDest)));
            firstPage.AddAnnotation(linkStringDest);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFile, cmpFile, destinationFolder, "diff_"
                ));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void DestCopyingTest01() {
            String srcFile = sourceFolder + "simpleWithLinks.pdf";
            String outFile = destinationFolder + "destCopyingTest01.pdf";
            String cmpFile = sourceFolder + "cmp_destCopyingTest01.pdf";
            PdfDocument srcDoc = new PdfDocument(new PdfReader(srcFile));
            PdfDocument destDoc = new PdfDocument(new PdfWriter(outFile));
            srcDoc.CopyPagesTo(JavaUtil.ArraysAsList(1, 2, 3), destDoc);
            destDoc.Close();
            srcDoc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFile, cmpFile, destinationFolder, "diff_"
                ));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void DestCopyingTest02() {
            String srcFile = sourceFolder + "simpleWithLinks.pdf";
            String outFile = destinationFolder + "destCopyingTest02.pdf";
            String cmpFile = sourceFolder + "cmp_destCopyingTest02.pdf";
            PdfDocument srcDoc = new PdfDocument(new PdfReader(srcFile));
            PdfDocument destDoc = new PdfDocument(new PdfWriter(outFile));
            srcDoc.CopyPagesTo(JavaUtil.ArraysAsList(1), destDoc);
            destDoc.Close();
            srcDoc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFile, cmpFile, destinationFolder, "diff_"
                ));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void DestCopyingTest03() {
            String srcFile = sourceFolder + "simpleWithLinks.pdf";
            String outFile = destinationFolder + "destCopyingTest03.pdf";
            String cmpFile = sourceFolder + "cmp_destCopyingTest03.pdf";
            PdfDocument srcDoc = new PdfDocument(new PdfReader(srcFile));
            PdfDocument destDoc = new PdfDocument(new PdfWriter(outFile));
            srcDoc.CopyPagesTo(JavaUtil.ArraysAsList(1, 2), destDoc);
            destDoc.Close();
            srcDoc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFile, cmpFile, destinationFolder, "diff_"
                ));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void DestCopyingTest04() {
            String srcFile = sourceFolder + "simpleWithLinks.pdf";
            String outFile = destinationFolder + "destCopyingTest04.pdf";
            String cmpFile = sourceFolder + "cmp_destCopyingTest04.pdf";
            PdfDocument srcDoc = new PdfDocument(new PdfReader(srcFile));
            PdfDocument destDoc = new PdfDocument(new PdfWriter(outFile));
            srcDoc.CopyPagesTo(JavaUtil.ArraysAsList(1, 3), destDoc);
            destDoc.Close();
            srcDoc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFile, cmpFile, destinationFolder, "diff_"
                ));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void DestCopyingTest05() {
            String srcFile = sourceFolder + "simpleWithLinks.pdf";
            String outFile = destinationFolder + "destCopyingTest05.pdf";
            String cmpFile = sourceFolder + "cmp_destCopyingTest05.pdf";
            PdfDocument srcDoc = new PdfDocument(new PdfReader(srcFile));
            PdfDocument destDoc = new PdfDocument(new PdfWriter(outFile));
            srcDoc.CopyPagesTo(JavaUtil.ArraysAsList(1, 2, 3, 1), destDoc);
            destDoc.Close();
            srcDoc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFile, cmpFile, destinationFolder, "diff_"
                ));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void DestCopyingTest06() {
            String srcFile = sourceFolder + "sourceWithNamedDestination.pdf";
            String outFile = destinationFolder + "destCopyingTest06.pdf";
            String cmpFile = sourceFolder + "cmp_destCopyingTest06.pdf";
            PdfDocument srcDoc = new PdfDocument(new PdfReader(srcFile));
            PdfDocument destDoc = new PdfDocument(new PdfWriter(outFile));
            srcDoc.CopyPagesTo(JavaUtil.ArraysAsList(1, 2, 1), destDoc);
            destDoc.Close();
            srcDoc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFile, cmpFile, destinationFolder, "diff_"
                ));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void DestCopyingTest07() {
            String srcFile = sourceFolder + "sourceStringDestWithPageNumber.pdf";
            String outFile = destinationFolder + "destCopyingTest07.pdf";
            String cmpFile = sourceFolder + "cmp_destCopyingTest07.pdf";
            PdfDocument srcDoc = new PdfDocument(new PdfReader(srcFile));
            PdfDocument destDoc = new PdfDocument(new PdfWriter(outFile));
            srcDoc.CopyPagesTo(JavaUtil.ArraysAsList(1, 2, 1), destDoc);
            destDoc.Close();
            srcDoc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFile, cmpFile, destinationFolder, "diff_"
                ));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void StructureDestination01Test() {
            String srcFile = sourceFolder + "customRolesMappingPdf2.pdf";
            String outFile = destinationFolder + "structureDestination01Test.pdf";
            String cmpFile = sourceFolder + "cmp_structureDestination01Test.pdf";
            PdfDocument document = new PdfDocument(new PdfReader(srcFile), new PdfWriter(outFile));
            PdfStructElem imgElement = new PdfStructElem((PdfDictionary)document.GetPdfObject(13));
            PdfStructureDestination dest = PdfStructureDestination.CreateFit(imgElement);
            PdfPage secondPage = document.AddNewPage();
            PdfLinkAnnotation linkExplicitDest = new PdfLinkAnnotation(new Rectangle(35, 785, 160, 15));
            linkExplicitDest.SetAction(PdfAction.CreateGoTo(dest));
            secondPage.AddAnnotation(linkExplicitDest);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFile, cmpFile, destinationFolder, "diff_"
                ));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void StructureDestination02Test() {
            String srcFile = sourceFolder + "customRolesMappingPdf2.pdf";
            String outFile = destinationFolder + "structureDestination02Test.pdf";
            String cmpFile = sourceFolder + "cmp_structureDestination02Test.pdf";
            PdfDocument document = new PdfDocument(new PdfReader(srcFile), new PdfWriter(outFile));
            PdfStructElem imgElement = new PdfStructElem((PdfDictionary)document.GetPdfObject(13));
            PdfStructureDestination dest = PdfStructureDestination.CreateFit(imgElement);
            PdfPage secondPage = document.AddNewPage();
            PdfPage thirdPage = document.AddNewPage();
            PdfLinkAnnotation linkExplicitDest = new PdfLinkAnnotation(new Rectangle(35, 785, 160, 15));
            PdfAction gotoStructAction = PdfAction.CreateGoTo(PdfExplicitDestination.CreateFit(thirdPage));
            gotoStructAction.Put(PdfName.SD, dest.GetPdfObject());
            linkExplicitDest.SetAction(gotoStructAction);
            secondPage.AddAnnotation(linkExplicitDest);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFile, cmpFile, destinationFolder, "diff_"
                ));
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void MakeDestination01Test() {
            String srcFile = sourceFolder + "cmp_structureDestination01Test.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(srcFile));
            PdfObject destObj = ((PdfLinkAnnotation)pdfDocument.GetPage(2).GetAnnotations()[0]).GetAction().Get(PdfName
                .D);
            PdfDestination destWrapper = PdfDestination.MakeDestination(destObj);
            NUnit.Framework.Assert.AreEqual(typeof(PdfStructureDestination), destWrapper.GetType());
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void RemoteGoToDestinationTest() {
            String cmpFile = sourceFolder + "cmp_remoteGoToDestinationTest.pdf";
            String outFile = destinationFolder + "remoteGoToDestinationTest.pdf";
            PdfDocument @out = new PdfDocument(new PdfWriter(outFile));
            @out.AddNewPage();
            IList<PdfDestination> destinations = new List<PdfDestination>(7);
            destinations.Add(PdfExplicitRemoteGoToDestination.CreateFit(1));
            destinations.Add(PdfExplicitRemoteGoToDestination.CreateFitH(1, 10));
            destinations.Add(PdfExplicitRemoteGoToDestination.CreateFitV(1, 10));
            destinations.Add(PdfExplicitRemoteGoToDestination.CreateFitR(1, 10, 10, 10, 10));
            destinations.Add(PdfExplicitRemoteGoToDestination.CreateFitB(1));
            destinations.Add(PdfExplicitRemoteGoToDestination.CreateFitBH(1, 10));
            destinations.Add(PdfExplicitRemoteGoToDestination.CreateFitBV(1, 10));
            int y = 785;
            foreach (PdfDestination destination in destinations) {
                PdfLinkAnnotation linkExplicitDest = new PdfLinkAnnotation(new Rectangle(35, y, 160, 15));
                PdfAction action = PdfAction.CreateGoToR(new PdfStringFS("Some fake destination"), destination);
                linkExplicitDest.SetAction(action);
                @out.GetFirstPage().AddAnnotation(linkExplicitDest);
                y -= 20;
            }
            @out.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFile, cmpFile, destinationFolder, "diff_"
                ));
        }
    }
}
