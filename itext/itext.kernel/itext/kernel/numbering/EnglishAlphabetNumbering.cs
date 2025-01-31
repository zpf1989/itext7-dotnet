/*

This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

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

namespace iText.Kernel.Numbering {
    /// <summary>
    /// This class is responsible for converting integer numbers to their
    /// English alphabet letter representations.
    /// </summary>
    public class EnglishAlphabetNumbering {
        protected internal static readonly char[] ALPHABET_LOWERCASE;

        protected internal static readonly char[] ALPHABET_UPPERCASE;

        protected internal const int ALPHABET_LENGTH = 26;

        static EnglishAlphabetNumbering() {
            ALPHABET_LOWERCASE = new char[ALPHABET_LENGTH];
            ALPHABET_UPPERCASE = new char[ALPHABET_LENGTH];
            for (int i = 0; i < ALPHABET_LENGTH; i++) {
                ALPHABET_LOWERCASE[i] = (char)('a' + i);
                ALPHABET_UPPERCASE[i] = (char)('A' + i);
            }
        }

        /// <summary>Converts the given number to its English alphabet lowercase string representation.</summary>
        /// <remarks>
        /// Converts the given number to its English alphabet lowercase string representation.
        /// E.g. 1 will be converted to "a", 2 to "b", ..., 27 to "aa", and so on.
        /// </remarks>
        /// <param name="number">the number to be converted</param>
        public static String ToLatinAlphabetNumberLowerCase(int number) {
            return AlphabetNumbering.ToAlphabetNumber(number, ALPHABET_LOWERCASE);
        }

        /// <summary>Converts the given number to its English alphabet lowercase string representation.</summary>
        /// <remarks>
        /// Converts the given number to its English alphabet lowercase string representation.
        /// E.g. 1 will be converted to "A", 2 to "B", ..., 27 to "AA", and so on.
        /// </remarks>
        /// <param name="number">the number to be converted</param>
        public static String ToLatinAlphabetNumberUpperCase(int number) {
            return AlphabetNumbering.ToAlphabetNumber(number, ALPHABET_UPPERCASE);
        }

        /// <summary>Converts the given number to its English alphabet string representation.</summary>
        /// <remarks>
        /// Converts the given number to its English alphabet string representation.
        /// E.g. for <code>upperCase</code> set to false,
        /// 1 will be converted to "a", 2 to "b", ..., 27 to "aa", and so on.
        /// </remarks>
        /// <param name="number">the number to be converted</param>
        /// <param name="upperCase">whether to use uppercase or lowercase alphabet</param>
        public static String ToLatinAlphabetNumber(int number, bool upperCase) {
            return upperCase ? ToLatinAlphabetNumberUpperCase(number) : ToLatinAlphabetNumberLowerCase(number);
        }
    }
}
