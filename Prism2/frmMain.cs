using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

/// +------------------------------------------------------------------------------------------------------------------------------+
/// ¦                                                   TERMS OF USE: MIT License                                                  ¦
/// +------------------------------------------------------------------------------------------------------------------------------¦
/// ¦Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation    ¦
/// ¦files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,    ¦
/// ¦modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software¦
/// ¦is furnished to do so, subject to the following conditions:                                                                   ¦
/// ¦                                                                                                                              ¦
/// ¦The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.¦
/// ¦                                                                                                                              ¦
/// ¦THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE          ¦
/// ¦WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR         ¦
/// ¦COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,   ¦
/// ¦ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                         ¦
/// +------------------------------------------------------------------------------------------------------------------------------+

namespace Prism2
{
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// <summary>
    /// The main form for the Prism2 application. This application will detect 
    /// circles or squares of a specific color in an image. 
    /// 
    /// This is essentially the same as Prism1 except that it uses Image<TColor, TDepth>
    /// objects as the image carrier object instead of Mat() 
    /// 
    /// This code is not especially sophisticated and is basically a learning tool to 
    /// enable the author to get a feel for EmguCV. 
    /// 
    /// </summary>
    public partial class frmMain : Form
    {

        public const string DATA_DIR = @"C:\Dump\PrismData";

        public const int CENTROID_CROSS_BAR_LEN = 10;
        public static MCvScalar COLOR_WHITE = new MCvScalar(255, 255, 255);
        public static MCvScalar COLOR_BLACK = new MCvScalar(0, 0, 0);
        public static MCvScalar BLUE_RANGE_LOW = new MCvScalar(150, 0, 0);
        public static MCvScalar BLUE_RANGE_HIGH = new MCvScalar(255, 150, 150);
        public static MCvScalar GREEN_RANGE_LOW = new MCvScalar(0, 150, 0);
        public static MCvScalar GREEN_RANGE_HIGH = new MCvScalar(150, 255, 150);
        public static MCvScalar RED_RANGE_LOW = new MCvScalar(0, 0, 150);
        public static MCvScalar RED_RANGE_HIGH = new MCvScalar(150, 150, 255);

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public frmMain()
        {
            InitializeComponent();

            // this makes the picture box scroll bars appear if necessary
            pictureBoxImageToProcess.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Process a file according the the current onscreen parameters
        /// </summary>
        /// <param name="filenameToProcess">the name of the file we process</param>
        private void ProcessFile(string filenameToProcess)
        {
            if ((filenameToProcess == null) || (filenameToProcess.Length <= 3)) throw new Exception("invalid or null file name");

            if (radioButtonObjectCircles.Checked == true)
            {
                // look for circles
                MarkCenterOfSolidCirclesByColor(filenameToProcess);
            }
            else
            {
                // look for squares
                MarkCenterOfSolidSquaresByColor(filenameToProcess);
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Code to identify squares in the image. Will look for specific colors
        /// according to the onscreen parameters and will mark the center of those found
        /// </summary>
        /// <param name="filenameToProcess">the name of the file we process</param>
        private void MarkCenterOfSolidSquaresByColor(string filenameToProcess)
        {
            if ((filenameToProcess == null) || (filenameToProcess.Length <= 3)) throw new Exception("invalid or null file name");

            Image<Bgr, Byte> inputImage = new  Image<Bgr, Byte>(filenameToProcess);
            List<RotatedRect> squares = FindSquares(inputImage);
            if (squares == null) return;

             // lets draw some crosses on the center of each circle of a specified color
            foreach (RotatedRect squareCoord in squares)
            {
                // yes, this is the right way around
                int row = Convert.ToInt32(squareCoord.Center.Y);
                int col = Convert.ToInt32(squareCoord.Center.X);

                // get the pixel values
                // the direct access below is the fastest way I can find to read a pixel value 
                // from an image. See Prism3 for a comparison test
                byte[] pixelValue = new byte[3];
                pixelValue[0] = inputImage.Data[row, col, 0]; // 1000000 iterations = 11ms the group of three
                pixelValue[1] = inputImage.Data[row, col, 1];
                pixelValue[2] = inputImage.Data[row, col, 2];

                if (radioButtonColorRed.Checked == true)
                {
                    // test the color is red, if not try next
                    if (IsBGRPixelInRange(pixelValue, RED_RANGE_LOW, RED_RANGE_HIGH) == false) continue;
                }
                else if (radioButtonColorGreen.Checked == true)
                {
                    if (IsBGRPixelInRange(pixelValue, GREEN_RANGE_LOW, GREEN_RANGE_HIGH) == false) continue;
                }
                else if (radioButtonColorBlue.Checked == true)
                {
                    if (IsBGRPixelInRange(pixelValue, BLUE_RANGE_LOW, BLUE_RANGE_HIGH) == false) continue;
                }
                else
                {
                    // must be all
                }

                // draw the cross
                DrawCrossOnPoint(inputImage, new Point(Convert.ToInt32(squareCoord.Center.X), Convert.ToInt32(squareCoord.Center.Y)), CENTROID_CROSS_BAR_LEN, COLOR_BLACK);
            }

            // and display it
            pictureBoxImageToProcess.Image = inputImage.ToBitmap();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Find the centers of all squares in the input image. Derived from the open
        /// source code at:
        /// https://www.emgu.com/wiki/index.php/Shape_(Triangle,_Rectangle,_Circle,_Line)_Detection_in_CSharp
        /// </summary>
        /// <param name="inputImage">the input image</param>
        /// <returns> List<RotatedRect> structs</returns>
        public List<RotatedRect> FindSquares( Image<Bgr, Byte> inputImage)
        {
            List<RotatedRect> boxList = new List<RotatedRect>(); //a box is a rotated rectangle
            double cannyThreshold = 180; // was 180.0;

            if (inputImage == null) throw new Exception("Null image provided");

            using ( Image<Gray, Byte> grayImage = new  Image<Gray, Byte>(inputImage.Width, inputImage.Height))
            using (Image<Gray, Byte> cannyEdges = new Image<Gray, Byte>(inputImage.Width, inputImage.Height))
            {
                //Convert the image to grayscale
                CvInvoke.CvtColor(inputImage, grayImage, ColorConversion.Bgr2Gray);

                //Remove noise
                CvInvoke.GaussianBlur(grayImage, grayImage, new Size(3, 3), 1);

                // Canny and edge detection
                double cannyThresholdLinking = 120.0;
                CvInvoke.Canny(grayImage, cannyEdges, cannyThreshold, cannyThresholdLinking);
                LineSegment2D[] lines = CvInvoke.HoughLinesP(
                    cannyEdges,
                    1, //Distance resolution in pixel-related units
                     Math.PI / 45.0, //Angle resolution measured in radians.
                    20, //threshold
                    30, //min Line width
                    10); //gap between lines

                // Find rectangles
                using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
                {
                    CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                    int count = contours.Size;
                    for (int i = 0; i < count; i++)
                    {
                        using (VectorOfPoint contour = contours[i])
                        using (VectorOfPoint approxContour = new VectorOfPoint())
                        {
                            CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
                            //only consider contours with area greater than 250
                            if (CvInvoke.ContourArea(approxContour, false) > 250) 
                            {
                                //The contour must have 4 vertices
                                if (approxContour.Size != 4) continue;

                                // determine if all the angles in the contour are within [80, 100] degree
                                bool isRectangle = true;
                                Point[] pts = approxContour.ToArray();
                                LineSegment2D[] edges = PointCollection.PolyLine(pts, true);
                                for (int j = 0; j < edges.Length; j++)
                                {
                                    double angle =  Math.Abs(edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                                    if (angle < 80 || angle > 100)
                                    {
                                        isRectangle = false;
                                        break;
                                    }
                                }

                                if (isRectangle) boxList.Add(CvInvoke.MinAreaRect(approxContour));
                                
                            }
                        }
                    }
                }
            }
            return boxList;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Code to identify circles in the image. Will look for specific colors
        /// according to the onscreen parameters and will mark the center of those found
        /// </summary>
        /// <param name="filenameToProcess">the name of the file we process</param>
        private void MarkCenterOfSolidCirclesByColor(string filenameToProcess)
        {
            if ((filenameToProcess == null) || (filenameToProcess.Length <= 3)) throw new Exception("invalid or null file name");

            Image<Bgr, Byte> inputImage = new  Image<Bgr, Byte>(filenameToProcess);
            CircleF[] circles = FindCircles(inputImage);
            if (circles == null) return;

            // lets draw some crosses on the center of each circle of a specified color
            foreach (CircleF circleCoord in circles)
            {
                // yes, this is the right way around
                int row = Convert.ToInt32(circleCoord.Center.Y);
                int col = Convert.ToInt32(circleCoord.Center.X);

                // get the pixel values
                // the direct access below is the fastest way I can find to read a pixel value 
                // from an image. See Prism3 for a comparison test
                byte[] pixelValue = new byte[3];
                pixelValue[0] = inputImage.Data[row, col, 0]; // 1000000 iterations = 11ms the group of three
                pixelValue[1] = inputImage.Data[row, col, 1];
                pixelValue[2] = inputImage.Data[row, col, 2];

                if (radioButtonColorRed.Checked == true)
                {
                    // test the color is red, if not try next
                    if (IsBGRPixelInRange(pixelValue, RED_RANGE_LOW, RED_RANGE_HIGH) == false) continue;
                }
                else if (radioButtonColorGreen.Checked == true)
                {
                    if (IsBGRPixelInRange(pixelValue, GREEN_RANGE_LOW, GREEN_RANGE_HIGH) == false) continue;
                }
                else if (radioButtonColorBlue.Checked == true)
                {
                    if (IsBGRPixelInRange(pixelValue, BLUE_RANGE_LOW, BLUE_RANGE_HIGH) == false) continue;
                }
                else
                {
                    // must be all
                }

                // draw the cross
                DrawCrossOnPoint(inputImage, new Point(Convert.ToInt32(circleCoord.Center.X), Convert.ToInt32(circleCoord.Center.Y)), CENTROID_CROSS_BAR_LEN, COLOR_BLACK);
            }

            // and display it
            pictureBoxImageToProcess.Image = inputImage.ToBitmap();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Find the centers of all circles in the input image. Derived from the open
        /// source code at:
        /// https://www.emgu.com/wiki/index.php/Shape_(Triangle,_Rectangle,_Circle,_Line)_Detection_in_CSharp
        /// </summary>
        /// <param name="inputImage">the input image</param>
        /// <returns>an array of CircleF structs</returns>
        public CircleF[] FindCircles( Image<Bgr, Byte> inputImage)
        {
            if (inputImage == null) throw new Exception("Null image provided");

            using ( Image<Gray, Byte> grayImage = new  Image<Gray, Byte>(inputImage.Width, inputImage.Height))
            {
                //Convert the image to grayscale and filter out the noise
                CvInvoke.CvtColor(inputImage, grayImage, ColorConversion.Bgr2Gray);

                //Remove noise
                CvInvoke.GaussianBlur(grayImage, grayImage, new Size(3, 3), 1);

                // circle detection using Hough algo
                double cannyThreshold = 60; // was 180.0;
                double circleAccumulatorThreshold = 120;
                CircleF[] circles = CvInvoke.HoughCircles(grayImage, HoughModes.Gradient, 2.0, 20.0, cannyThreshold, circleAccumulatorThreshold, 5);
                return circles;
            }

        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Detect if a pixel is within a specified color range
        /// </summary>
        /// <param name="pixelValue">3 byte BGR pixel value</param>
        /// <param name="bgrRangeLow">a struct containing the bgr value to test against</param>
        /// <param name="bgrRangeHigh">a struct containing the bgr value to test against</param>
        /// <returns>true - in range, false - is not</returns>
        public bool IsBGRPixelInRange(byte[] pixelValue, MCvScalar bgrRangeLow, MCvScalar bgrRangeHigh)
        {
            if (pixelValue == null) return false;
            if (pixelValue.Length != 3) return false;

            if (pixelValue[0] < bgrRangeLow.V0) return false;
            if (pixelValue[1] < bgrRangeLow.V1) return false;
            if (pixelValue[2] < bgrRangeLow.V2) return false;
            if (pixelValue[0] > bgrRangeHigh.V0) return false;
            if (pixelValue[1] > bgrRangeHigh.V1) return false;
            if (pixelValue[2] > bgrRangeHigh.V2) return false;

            return true;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Draws a cross at a point in a specific color and length
        /// </summary>
        /// <param name="inputImage">the input image</param>
        /// <param name="armLength">the lenght of the arm in the cross</param>
        /// <param name="centerPoint">the centerpoint of the cross</param>
        /// <param name="colorOfCross">the color of the cross</param>
        public void DrawCrossOnPoint( Image<Bgr, Byte> inputImage, Point centerPoint, int armLength, MCvScalar colorOfCross)
        {
            if (inputImage == null) return;
            if (centerPoint == null) return;
            if (armLength <= 0) return;

            // apparently we do not need bounds checking here. The call to CvInvoke.Line does it
            Point horizStartPoint = new Point(centerPoint.X - armLength, centerPoint.Y);
            Point horizEndPoint = new Point(centerPoint.X + armLength, centerPoint.Y);
            Point vertStartPoint = new Point(centerPoint.X, centerPoint.Y - armLength);
            Point vertEndPoint = new Point(centerPoint.X, centerPoint.Y + armLength);

            // draw the horizontal line
            CvInvoke.Line(inputImage, horizStartPoint, horizEndPoint, colorOfCross, 1);
            // draw the vertical line
            CvInvoke.Line(inputImage, vertStartPoint, vertEndPoint, colorOfCross, 1);
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Open a file for processing.
        /// 
        /// </summary>
        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            // open a file
            OpenFileDialog Openfile = new OpenFileDialog();
            Openfile.InitialDirectory = DATA_DIR;
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                // record it
                textBoxCurrentFile.Text = Openfile.FileName;
                // clear what we have
                pictureBoxImageToProcess.Image = null;
                // process it
                ProcessFile(Openfile.FileName);
            }
            else
            {

            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handles a change of state on the color button
        /// 
        /// </summary>
        private void radioButtonColorRed_CheckedChanged(object sender, EventArgs e)
        {
            // do we have a current filename
            if ((textBoxCurrentFile.Text == null) || (textBoxCurrentFile.Text.Length <= 3)) return;
            // yes we do, process it
            ProcessFile(textBoxCurrentFile.Text);
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handles a change of state on the color button
        /// 
        /// </summary>
        private void radioButtonColorGreen_CheckedChanged(object sender, EventArgs e)
        {
            // do we have a current filename
            if ((textBoxCurrentFile.Text == null) || (textBoxCurrentFile.Text.Length <= 3)) return;
            // yes we do, process it
            ProcessFile(textBoxCurrentFile.Text);
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handles a change of state on the color button
        /// 
        /// </summary>
        private void radioButtonColorBlue_CheckedChanged(object sender, EventArgs e)
        {
            // do we have a current filename
            if ((textBoxCurrentFile.Text == null) || (textBoxCurrentFile.Text.Length <= 3)) return;
            // yes we do, process it
            ProcessFile(textBoxCurrentFile.Text);
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handles a change of state on the color button
        /// 
        /// </summary>
        private void radioButtonColorAll_CheckedChanged(object sender, EventArgs e)
        {
            // do we have a current filename
            if ((textBoxCurrentFile.Text == null) || (textBoxCurrentFile.Text.Length <= 3)) return;
            // yes we do, process it
            ProcessFile(textBoxCurrentFile.Text);
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handles a change of state on the object button
        /// 
        /// </summary>
        private void radioButtonObjectSquares_CheckedChanged(object sender, EventArgs e)
        {
            // do we have a current filename
            if ((textBoxCurrentFile.Text == null) || (textBoxCurrentFile.Text.Length <= 3)) return;
            // yes we do, process it
            ProcessFile(textBoxCurrentFile.Text);
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handles a change of state on the object button
        /// 
        /// </summary>
        private void radioButtonObjectCircles_CheckedChanged(object sender, EventArgs e)
        {
            // do we have a current filename
            if ((textBoxCurrentFile.Text == null) || (textBoxCurrentFile.Text.Length <= 3)) return;
            // yes we do, process it
            ProcessFile(textBoxCurrentFile.Text);
        }
    }
}
