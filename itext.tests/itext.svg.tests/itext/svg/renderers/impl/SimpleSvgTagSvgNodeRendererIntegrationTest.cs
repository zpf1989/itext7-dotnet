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
using iText.IO.Util;
using iText.StyledXmlParser.Exceptions;
using iText.Svg.Exceptions;
using iText.Svg.Renderers;
using iText.Test;
using iText.Test.Attributes;

namespace iText.Svg.Renderers.Impl {
    public class SimpleSvgTagSvgNodeRendererIntegrationTest : SvgIntegrationTest {
        private static readonly String SOURCE_FOLDER = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/svg/renderers/impl/RootSvgNodeRendererTest/svg/";

        private static readonly String DESTINATION_FOLDER = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/svg/renderers/impl/RootSvgNodeRendererTest/svg/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            ITextTest.CreateDestinationFolder(DESTINATION_FOLDER);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void EverythingPresentAndValidTest() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "everythingPresentAndValid");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        [LogMessage(SvgLogMessageConstant.MISSING_HEIGHT)]
        public virtual void AbsentHeight() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "absentHeight");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        [LogMessage(SvgLogMessageConstant.MISSING_WIDTH)]
        public virtual void AbsentWidth() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "absentWidth");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        [LogMessage(SvgLogMessageConstant.MISSING_WIDTH)]
        [LogMessage(SvgLogMessageConstant.MISSING_HEIGHT)]
        public virtual void AbsentWidthAndHeight() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "absentWidthAndHeight");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void AbsentWHViewboxPresent() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "absentWHViewboxPresent");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void AbsentX() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "absentX");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void AbsentY() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "absentY");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void InvalidHeight() {
            NUnit.Framework.Assert.That(() =>  {
                ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "invalidHeight");
            }
            , NUnit.Framework.Throws.InstanceOf<StyledXMLParserException>().With.Message.EqualTo(MessageFormatUtil.Format(iText.StyledXmlParser.LogMessageConstant.NAN, "abc")))
;
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void InvalidWidth() {
            NUnit.Framework.Assert.That(() =>  {
                ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "invalidWidth");
            }
            , NUnit.Framework.Throws.InstanceOf<StyledXMLParserException>().With.Message.EqualTo(MessageFormatUtil.Format(iText.StyledXmlParser.LogMessageConstant.NAN, "abc")))
;
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void InvalidX() {
            NUnit.Framework.Assert.That(() =>  {
                ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "invalidX");
            }
            , NUnit.Framework.Throws.InstanceOf<StyledXMLParserException>())
;
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void InvalidY() {
            NUnit.Framework.Assert.That(() =>  {
                ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "invalidY");
            }
            , NUnit.Framework.Throws.InstanceOf<StyledXMLParserException>())
;
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void NegativeEverything() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "negativeEverything");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void NegativeHeight() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "negativeHeight");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void NegativeWidth() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "negativeWidth");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void NegativeWidthAndHeight() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "negativeWidthAndHeight");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void NegativeX() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "negativeX");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void NegativeXY() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "negativeXY");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void NegativeY() {
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "negativeY");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        [LogMessage(iText.StyledXmlParser.LogMessageConstant.UNKNOWN_ABSOLUTE_METRIC_LENGTH_PARSED, Count = 2)]
        public virtual void PercentInMeasurement() {
            //TODO: update after DEVSIX-2377
            ConvertAndCompareVisually(SOURCE_FOLDER, DESTINATION_FOLDER, "percentInMeasurement");
        }
    }
}
