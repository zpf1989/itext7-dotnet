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

namespace iText.StyledXmlParser.Jsoup.Parser {
    /// <summary>A Parse Error records an error in the input HTML that occurs in either the tokenisation or the tree building phase.
    ///     </summary>
    public class ParseError {
        private int pos;

        private String errorMsg;

        internal ParseError(int pos, String errorMsg) {
            this.pos = pos;
            this.errorMsg = errorMsg;
        }

        internal ParseError(int pos, String errorFormat, params Object[] args) {
            this.errorMsg = MessageFormatUtil.Format(errorFormat, args);
            this.pos = pos;
        }

        /// <summary>Retrieve the error message.</summary>
        /// <returns>the error message.</returns>
        public virtual String GetErrorMessage() {
            return errorMsg;
        }

        /// <summary>Retrieves the offset of the error.</summary>
        /// <returns>error offset within input</returns>
        public virtual int GetPosition() {
            return pos;
        }

        public override String ToString() {
            return pos + ": " + errorMsg;
        }
    }
}
