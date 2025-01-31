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
using System.Text;
using System.Text.RegularExpressions;
using iText.IO.Util;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Svg;
using iText.Svg.Exceptions;
using iText.Svg.Renderers;
using iText.Svg.Renderers.Path;
using iText.Svg.Renderers.Path.Impl;
using iText.Svg.Utils;

namespace iText.Svg.Renderers.Impl {
    /// <summary>
    /// <see cref="iText.Svg.Renderers.ISvgNodeRenderer"/>
    /// implementation for the &lt;path&gt; tag.
    /// </summary>
    public class PathSvgNodeRenderer : AbstractSvgNodeRenderer {
        private const String SPACE_CHAR = " ";

        /// <summary>
        /// The regular expression to find invalid operators in the <a href="https://www.w3.org/TR/SVG/paths.html#PathData">PathData attribute of the &ltpath&gt element</a>
        /// <p>
        /// Find any occurrence of a letter that is not an operator
        /// </summary>
        private const String INVALID_OPERATOR_REGEX = "(?:(?![mzlhvcsqtae])\\p{L})";

        private static Regex invalidRegexPattern = iText.IO.Util.StringUtil.RegexCompile(INVALID_OPERATOR_REGEX, System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );

        /// <summary>
        /// The regular expression to split the <a href="https://www.w3.org/TR/SVG/paths.html#PathData">PathData attribute of the &ltpath&gt element</a>
        /// <p>
        /// Since
        /// <see cref="ContainsInvalidAttributes(System.String)"/>
        /// is called before the use of this expression in
        /// <see cref="ParsePathOperations()"/>
        /// the attribute to be split is valid.
        /// SVG defines 6 types of path commands, for a total of 20 commands:
        /// MoveTo: M, m
        /// LineTo: L, l, H, h, V, v
        /// Cubic Bezier Curve: C, c, S, s
        /// Quadratic Bezier Curve: Q, q, T, t
        /// Elliptical Arc Curve: A, a
        /// ClosePath: Z, z
        /// </summary>
        private static readonly Regex SPLIT_PATTERN = iText.IO.Util.StringUtil.RegexCompile("(?=[mlhvcsqtaz])", System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );

        /// <summary>
        /// The
        /// <see cref="iText.Kernel.Geom.Point"/>
        /// representing the current point in the path to be used for relative pathing operations.
        /// The original value is the origin, and should be set via a
        /// <see cref="iText.Svg.Renderers.Path.Impl.MoveTo"/>
        /// operation before it may be referenced.
        /// </summary>
        private Point currentPoint = new Point(0, 0);

        /// <summary>
        /// The
        /// <see cref="iText.Svg.Renderers.Path.Impl.ClosePath"/>
        /// shape keeping track of the initial point set by a
        /// <see cref="iText.Svg.Renderers.Path.Impl.MoveTo"/>
        /// operation.
        /// The original value is
        /// <see langword="null"/>
        /// , and must be set via a
        /// <see cref="iText.Svg.Renderers.Path.Impl.MoveTo"/>
        /// operation before it may be drawn.
        /// </summary>
        private ClosePath zOperator = null;

        protected internal override void DoDraw(SvgDrawContext context) {
            PdfCanvas canvas = context.GetCurrentCanvas();
            canvas.WriteLiteral("% path\n");
            currentPoint = new Point(0, 0);
            foreach (IPathShape item in GetShapes()) {
                item.Draw(canvas);
            }
        }

        public override ISvgNodeRenderer CreateDeepCopy() {
            PathSvgNodeRenderer copy = new PathSvgNodeRenderer();
            DeepCopyAttributesAndStyles(copy);
            return copy;
        }

        /// <summary>
        /// Gets the coordinates that shall be passed to
        /// <see cref="iText.Svg.Renderers.Path.IPathShape.SetCoordinates(System.String[], iText.Kernel.Geom.Point)"/>
        /// for the current shape.
        /// </summary>
        /// <param name="shape">The current shape.</param>
        /// <param name="previousShape">The previous shape which can affect the coordinates of the current shape.</param>
        /// <param name="pathProperties">
        /// The operator and all arguments as a
        /// <see>String[]</see>
        /// </param>
        /// <returns>
        /// a
        /// <see>String[]</see>
        /// of coordinates that shall be passed to
        /// <see cref="iText.Svg.Renderers.Path.IPathShape.SetCoordinates(System.String[], iText.Kernel.Geom.Point)"/>
        /// </returns>
        private String[] GetShapeCoordinates(IPathShape shape, IPathShape previousShape, String[] pathProperties) {
            if (shape is ClosePath) {
                return null;
            }
            String[] shapeCoordinates = null;
            if (shape is SmoothSCurveTo || shape is QuadraticSmoothCurveTo) {
                String[] startingControlPoint = new String[2];
                if (previousShape != null) {
                    Point previousEndPoint = previousShape.GetEndingPoint();
                    //if the previous command was a Bezier curve, use its last control point
                    if (previousShape is IControlPointCurve) {
                        Point lastControlPoint = ((IControlPointCurve)previousShape).GetLastControlPoint();
                        float reflectedX = (float)(2 * previousEndPoint.GetX() - lastControlPoint.GetX());
                        float reflectedY = (float)(2 * previousEndPoint.GetY() - lastControlPoint.GetY());
                        startingControlPoint[0] = SvgCssUtils.ConvertFloatToString(reflectedX);
                        startingControlPoint[1] = SvgCssUtils.ConvertFloatToString(reflectedY);
                    }
                    else {
                        startingControlPoint[0] = SvgCssUtils.ConvertDoubleToString(previousEndPoint.GetX());
                        startingControlPoint[1] = SvgCssUtils.ConvertDoubleToString(previousEndPoint.GetY());
                    }
                }
                else {
                    // TODO RND-951
                    startingControlPoint[0] = pathProperties[0];
                    startingControlPoint[1] = pathProperties[1];
                }
                shapeCoordinates = Concatenate(startingControlPoint, pathProperties);
            }
            if (shapeCoordinates == null) {
                shapeCoordinates = pathProperties;
            }
            return shapeCoordinates;
        }

        /// <summary>
        /// Processes an individual pathing operator and all of its arguments, converting into one or more
        /// <see cref="iText.Svg.Renderers.Path.IPathShape"/>
        /// objects.
        /// </summary>
        /// <param name="pathProperties">
        /// The property operator and all arguments as a
        /// <see>String[]</see>
        /// </param>
        /// <param name="previousShape">
        /// The previous shape which can affect the positioning of the current shape. If no previous
        /// shape exists
        /// <see langword="null"/>
        /// is passed.
        /// </param>
        /// <returns>
        /// a
        /// <see cref="System.Collections.IList{E}"/>
        /// of each
        /// <see cref="iText.Svg.Renderers.Path.IPathShape"/>
        /// that should be drawn to represent the operator.
        /// </returns>
        private IList<IPathShape> ProcessPathOperator(String[] pathProperties, IPathShape previousShape) {
            IList<IPathShape> shapes = new List<IPathShape>();
            if (pathProperties.Length == 0 || String.IsNullOrEmpty(pathProperties[0]) || SvgPathShapeFactory.GetArgumentCount
                (pathProperties[0]) < 0) {
                return shapes;
            }
            int argumentCount = SvgPathShapeFactory.GetArgumentCount(pathProperties[0]);
            if (argumentCount == 0) {
                // closePath operator
                if (previousShape == null) {
                    throw new SvgProcessingException(SvgLogMessageConstant.INVALID_CLOSEPATH_OPERATOR_USE);
                }
                shapes.Add(zOperator);
                currentPoint = zOperator.GetEndingPoint();
                return shapes;
            }
            for (int index = 1; index < pathProperties.Length; index += argumentCount) {
                if (index + argumentCount > pathProperties.Length) {
                    break;
                }
                IPathShape pathShape = SvgPathShapeFactory.CreatePathShape(pathProperties[0]);
                if (pathShape is MoveTo) {
                    shapes.AddAll(AddMoveToShapes(pathShape, pathProperties));
                    return shapes;
                }
                String[] shapeCoordinates = GetShapeCoordinates(pathShape, previousShape, JavaUtil.ArraysCopyOfRange(pathProperties
                    , index, index + argumentCount));
                if (pathShape != null) {
                    if (shapeCoordinates != null) {
                        pathShape.SetCoordinates(shapeCoordinates, currentPoint);
                    }
                    currentPoint = pathShape.GetEndingPoint();
                    // unsupported operators are ignored.
                    shapes.Add(pathShape);
                }
                previousShape = pathShape;
            }
            return shapes;
        }

        private IList<IPathShape> AddMoveToShapes(IPathShape pathShape, String[] pathProperties) {
            IList<IPathShape> shapes = new List<IPathShape>();
            int argumentCount = 2;
            String[] shapeCoordinates = GetShapeCoordinates(pathShape, null, JavaUtil.ArraysCopyOfRange(pathProperties
                , 1, 3));
            zOperator = new ClosePath(pathShape.IsRelative());
            zOperator.SetCoordinates(shapeCoordinates, currentPoint);
            pathShape.SetCoordinates(shapeCoordinates, currentPoint);
            currentPoint = pathShape.GetEndingPoint();
            shapes.Add(pathShape);
            IPathShape previousShape = pathShape;
            if (pathProperties.Length > 3) {
                for (int index = 3; index < pathProperties.Length; index += argumentCount) {
                    if (index + 2 > pathProperties.Length) {
                        break;
                    }
                    pathShape = pathShape.IsRelative() ? SvgPathShapeFactory.CreatePathShape("l") : SvgPathShapeFactory.CreatePathShape
                        ("L");
                    shapeCoordinates = GetShapeCoordinates(pathShape, previousShape, JavaUtil.ArraysCopyOfRange(pathProperties
                        , index, index + 2));
                    pathShape.SetCoordinates(shapeCoordinates, previousShape.GetEndingPoint());
                    shapes.Add(pathShape);
                    previousShape = pathShape;
                }
            }
            return shapes;
        }

        /// <summary>
        /// Processes the
        /// <see cref="iText.Svg.SvgConstants.Attributes.D"/>
        /// 
        /// <see cref="AbstractSvgNodeRenderer.attributesAndStyles"/>
        /// and converts them
        /// into one or more
        /// <see cref="iText.Svg.Renderers.Path.IPathShape"/>
        /// objects to be drawn on the canvas.
        /// <p>
        /// Each individual operator is passed to
        /// <see cref="ProcessPathOperator(System.String[], iText.Svg.Renderers.Path.IPathShape)"/>
        /// to be processed individually.
        /// </summary>
        /// <returns>
        /// a
        /// <see cref="System.Collections.ICollection{E}"/>
        /// of each
        /// <see cref="iText.Svg.Renderers.Path.IPathShape"/>
        /// that should be drawn to represent the path.
        /// </returns>
        internal virtual ICollection<IPathShape> GetShapes() {
            ICollection<String> parsedResults = ParsePathOperations();
            IList<IPathShape> shapes = new List<IPathShape>();
            foreach (String parsedResult in parsedResults) {
                String[] pathProperties = iText.IO.Util.StringUtil.Split(parsedResult, " +");
                IPathShape previousShape = shapes.Count == 0 ? null : shapes[shapes.Count - 1];
                IList<IPathShape> operatorShapes = ProcessPathOperator(pathProperties, previousShape);
                shapes.AddAll(operatorShapes);
            }
            return shapes;
        }

        private static String[] Concatenate(String[] first, String[] second) {
            String[] arr = new String[first.Length + second.Length];
            Array.Copy(first, 0, arr, 0, first.Length);
            Array.Copy(second, 0, arr, first.Length, second.Length);
            return arr;
        }

        internal virtual bool ContainsInvalidAttributes(String attributes) {
            return SvgRegexUtils.ContainsAtLeastOneMatch(invalidRegexPattern, attributes);
        }

        internal virtual ICollection<String> ParsePathOperations() {
            ICollection<String> result = new List<String>();
            String attributes = attributesAndStyles.Get(SvgConstants.Attributes.D);
            if (attributes == null) {
                throw new SvgProcessingException(SvgExceptionMessageConstant.PATH_OBJECT_MUST_HAVE_D_ATTRIBUTE);
            }
            if (ContainsInvalidAttributes(attributes)) {
                throw new SvgProcessingException(SvgLogMessageConstant.INVALID_PATH_D_ATTRIBUTE_OPERATORS).SetMessageParams
                    (attributes);
            }
            String[] operators = SplitPathStringIntoOperators(attributes);
            foreach (String inst in operators) {
                String instTrim = inst.Trim();
                if (!String.IsNullOrEmpty(instTrim)) {
                    char instruction = instTrim[0];
                    String temp = instruction + SPACE_CHAR + instTrim.Substring(1).Replace(",", SPACE_CHAR).Trim();
                    //Do a run-through for decimal point separation
                    temp = SeparateDecimalPoints(temp);
                    result.Add(temp);
                }
            }
            return result;
        }

        /// <summary>Iterate over the input string and separate numbers from each other with space chars</summary>
        internal virtual String SeparateDecimalPoints(String input) {
            //If a space or minus sign is found reset
            //If a another point is found, add an extra space on before the point
            StringBuilder res = new StringBuilder();
            // We are now among the digits to the right of the decimal point
            bool fractionalPartAfterDecimalPoint = false;
            // We are now among the exponent magnitude part
            bool exponentSignMagnitude = false;
            for (int i = 0; i < input.Length; i++) {
                char c = input[i];
                // Resetting flags
                if (c == '-' || iText.IO.Util.TextUtil.IsWhiteSpace(c)) {
                    fractionalPartAfterDecimalPoint = false;
                }
                if (iText.IO.Util.TextUtil.IsWhiteSpace(c)) {
                    exponentSignMagnitude = false;
                }
                // Add extra space before the next number starting from '.', or before the next number starting with '-'
                if (EndsWithNonWhitespace(res) && (c == '.' && fractionalPartAfterDecimalPoint || c == '-' && !exponentSignMagnitude
                    )) {
                    res.Append(" ");
                }
                if (c == '.') {
                    fractionalPartAfterDecimalPoint = true;
                }
                else {
                    if (c == 'e') {
                        exponentSignMagnitude = true;
                    }
                }
                res.Append(c);
            }
            return res.ToString();
        }

        /// <summary>Gets an array of strings representing operators with their arguments, e.g.</summary>
        /// <remarks>Gets an array of strings representing operators with their arguments, e.g. {"M 100 100", "L 300 100", "L200, 300", "z"}
        ///     </remarks>
        internal static String[] SplitPathStringIntoOperators(String path) {
            return iText.IO.Util.StringUtil.Split(SPLIT_PATTERN, path);
        }

        private static bool EndsWithNonWhitespace(StringBuilder sb) {
            return sb.Length > 0 && !iText.IO.Util.TextUtil.IsWhiteSpace(sb[sb.Length - 1]);
        }
    }
}
