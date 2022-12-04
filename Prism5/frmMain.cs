using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

namespace Prism5
{
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// <summary>
    /// The main form for the Prism5 application. 
    /// 
    /// This application sources video frames from a webcam and displays them on 
    /// the screen. It also (optionally) detects circles of various colors and 
    /// can record the video information to a file as an .mp4 
    /// 
    /// A number of options for frame size and video format type are provided and
    /// cross hairs at the center of the identified circles are recorded to the 
    /// output file as well.
    /// 
    /// </summary>
    public partial class mainFrm : Form
    {
        // we are looking only at the first webcam
        // if you have multiple webcams you might want to change this index
        private const int DEFAULT_WEBCAM_INDEX = 0;

        // some global vars
        private VideoCapture webcamVideoCaptureObj = null;
        private Mat workingFrame;
        private VideoWriter videoWriterObj = null;

        // our output dir and file
        private const string DEFAULT_OUT_DIR = @"C:\Dump\PrismData";
        private const string DEFAULT_OUT_FILE = @"Prism5.mp4";

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
        public mainFrm()
        {
            InitializeComponent();

            // these are hard coded for now
            textBoxOutputDir.Text = DEFAULT_OUT_DIR;
            textBoxOutputFile.Text = DEFAULT_OUT_FILE;

            textBoxNowRecording.Text = "Not recording, It can take quite some time (1 min+) for the capture image to appear once the Capture has been started!";

        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Start the videoCapture
        /// </summary>
        /// <param name="backendAPIToUse">the backend API to use</param>
        /// <param name="screenSize">the width x height of the screen</param>
        /// <param name="framesPerSec">the frames per second value</param>
        private void StartVideoCapture(VideoCapture.API backendAPIToUse, Size screenSize, int framesPerSec)
        { 
            CvInvoke.UseOpenCL = false;
            try
            {
                // create the VideoCapture object. This takes an extraordinarily long time
                // around 30 seconds on my system with WMF backend, much faster with DShow 
                // however you cannot record the data to disk with DShow - OpenCV does not
                // support it. 
                webcamVideoCaptureObj = new VideoCapture(ActiveWebCamIndex, backendAPIToUse);
                // now set some values on it, first the Frames per second
                webcamVideoCaptureObj.Set(Emgu.CV.CvEnum.CapProp.Fps, framesPerSec);
                // set the screensize. Note the available selections are just some of the ones
                // from my Logitech C920 Webcam. There are many others and each WebCam will have
                // different ones
                webcamVideoCaptureObj.Set(Emgu.CV.CvEnum.CapProp.FrameWidth, screenSize.Width);
                webcamVideoCaptureObj.Set(Emgu.CV.CvEnum.CapProp.FrameHeight, screenSize.Height);

                // ok this is where it starts to get odd. Webcams can output in a variety of video formats
                // the Logitech C920 can do YUY2, MJPG, H264 and NV12. Not all sizes and FPS values
                // are available for all formats. WMF can give you a nice list of all of the combinations
                // available - See the Prism6 sample program - however it does not look like OpenCV
                // and hence EmguCV can provide such a list. So you just have to chuck your preferred
                // parameters in and hope for the best.
                
                // It seems that OpenCV is basically taking the supplied information and negotiating 
                // with WMF to set up a video pipeline. This coupled with WMFs tendency to auto-load 
                // codecs in order to make things work so means you never really know what format you
                // are getting. You are supposed to be able to specify the frame format using a
                // FourCC code as shown below. I have no idea if it is really working so I have just
                // left it at YUY2 and not made it a selectable option on the screen.

                // leave it set to YUY2 - a very common format
                int fourCCCode = VideoWriter.Fourcc('Y', 'U', 'Y', '2');
                //int fourCCCode = VideoWriter.Fourcc('H', '2', '6', '4');
                //int fourCCCode = VideoWriter.Fourcc('M', 'J', 'P', 'G');
                webcamVideoCaptureObj.Set(Emgu.CV.CvEnum.CapProp.FourCC, fourCCCode);

                // give the handler to the VideoCapture object
                webcamVideoCaptureObj.ImageGrabbed += ProcessFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
            workingFrame = new Mat();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Stop the videoCapture
        /// </summary>
        private void StopVideoCapture()
        {
            CloseDownAndReleaseObjects();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Process each frame and display each stage of the processing in a 
        /// separate image on the main form.
        /// </summary>
        private void ProcessFrame(object sender, EventArgs arg)
        {
            if ((webcamVideoCaptureObj != null) && (webcamVideoCaptureObj.Ptr != IntPtr.Zero))
            {
                // get the frame from the webcam
                webcamVideoCaptureObj.Retrieve(workingFrame, 0);

                // now look for solid circles and mark them
                MarkCenterOfSolidCirclesByColor(workingFrame);

                // display it on the screen
                captureImageBox.Image = workingFrame;

                // do we want to write out
                if (videoWriterObj != null)
                {
                    // give the frame to the video writer
                    videoWriterObj.Write(workingFrame);
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Code to identify circles in the image. Will look for specific colors
        /// according to the onscreen parameters and will mark the center of those found
        /// </summary>
        /// <param name="imageToProcess">the image we process</param>
        private void MarkCenterOfSolidCirclesByColor(Mat imageToProcess)
        {
            if (imageToProcess == null)  throw new Exception("invalid or null image");

            CircleF[] circles = FindCircles(imageToProcess);
            if (circles == null) return;

            // lets draw some crosses on the center of each circle of a specified color
            foreach (CircleF circleCoord in circles)
            {
                // yes, this is the right way around
                int row = Convert.ToInt32(circleCoord.Center.Y);
                int col = Convert.ToInt32(circleCoord.Center.X);

                // get the pixel values. Note that GetValues is an extension method on Mat() See MatExtension.cs
                byte[] pixelValue = imageToProcess.GetValues(row, col);

                // are we detecting a color
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
                DrawCrossOnPoint(imageToProcess, new Point(Convert.ToInt32(circleCoord.Center.X), Convert.ToInt32(circleCoord.Center.Y)), CENTROID_CROSS_BAR_LEN, COLOR_BLACK);
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
        public void DrawCrossOnPoint(Mat inputImage, Point centerPoint, int armLength, MCvScalar colorOfCross)
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
        /// Find the centers of all circles in the input image. Derived from the open
        /// source code at:
        /// https://www.emgu.com/wiki/index.php/Shape_(Triangle,_Rectangle,_Circle,_Line)_Detection_in_CSharp
        /// </summary>
        /// <param name="imageToProcess">the input image</param>
        /// <returns>an array of CircleF structs</returns>
        public CircleF[] FindCircles(Mat imageToProcess)
        {
            if (imageToProcess == null) throw new Exception("Null image provided");

            using (Mat grayImage = new Mat())
            {
                //Convert the image to grayscale and filter out the noise
                CvInvoke.CvtColor(imageToProcess, grayImage, ColorConversion.Bgr2Gray);

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
        /// Handle presses on the Capture button
        /// </summary>
        private void buttonCapture_Click(object sender, EventArgs e)
        {
            // have we got a video capture object?
            if (webcamVideoCaptureObj != null)
            {
                // yes, shut things down now 
                CloseDownAndReleaseObjects();
                // now set up to restart the capture
                buttonCapture.Text = "Start Capture";
            }
            else // webcamVideoCaptureObj == null
            {
                // no, start things up
                Cursor.Current = Cursors.WaitCursor;
                StartVideoCapture(BackendAPI, ScreenSize, FramesPerSec);
                Cursor.Current = Cursors.Default;
                webcamVideoCaptureObj.Start();
                //now set up to stop the capture
                buttonCapture.Text = "Stop Capture";
             }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle presses on the Record button
        /// </summary>
        private void buttonRecordToDisk_Click(object sender, EventArgs e)
        {
            // we have to have this 
            if (webcamVideoCaptureObj == null) return;

            // recording is only available in WMF mode so get the current backend
            var backend = webcamVideoCaptureObj.Get(Emgu.CV.CvEnum.CapProp.Backend);
            if (webcamVideoCaptureObj.Get(Emgu.CV.CvEnum.CapProp.Backend) != (int)VideoCapture.API.Msmf)
            {
                throw new Exception("Recording only available in WMF mode");
            }

            // have we already got one?, if so clean up
            if (videoWriterObj != null)
            {
                var tmpVR = videoWriterObj;
                videoWriterObj = null;
                tmpVR.Dispose();
                // now set up to restart the record
                buttonRecordToDisk.Text = "Start Recording";
                textBoxNowRecording.Text = "Not recording";
            }
            else
            {
                // do we have an output directory
                if(Directory.Exists(OutputDir) == false)
                {
                    throw new Exception("Output directory of >"+OutputDir+"< does not exist");
                }
                // create the destination file path
                string destination = Path.Combine(OutputDir, OutputFile);


                // get the params from our webcam so we can set up the recorder
                var fps = webcamVideoCaptureObj.Get(Emgu.CV.CvEnum.CapProp.Fps);
                int fourcc = Convert.ToInt32(webcamVideoCaptureObj.Get(Emgu.CV.CvEnum.CapProp.FourCC));
                int frameHeight = Convert.ToInt32(webcamVideoCaptureObj.Get(Emgu.CV.CvEnum.CapProp.FrameHeight));
                int frameWidth = Convert.ToInt32(webcamVideoCaptureObj.Get(Emgu.CV.CvEnum.CapProp.FrameWidth));

                // create the writer. It will get fed frames as necessary
                videoWriterObj = new VideoWriter(destination, VideoWriter.Fourcc('H', '2', '6', '4'), fps, new Size(frameWidth, frameHeight), true);
                // now set up to stop the record
                buttonRecordToDisk.Text = "Stop Recording";
                textBoxNowRecording.Text ="Now recording to: " + destination;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Clean up on exit. Called out of the form Dispose()
        /// in the Designer 
        /// </summary>
        private void CloseDownAndReleaseObjects()
        {
            if (webcamVideoCaptureObj != null)
            {
                webcamVideoCaptureObj.Dispose();
                webcamVideoCaptureObj = null;
            }

            if (videoWriterObj != null)
            {
                videoWriterObj.Dispose();
                videoWriterObj = null;

            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Get/Set the the output Dir
        /// </summary>
        private string OutputDir
        {
            get
            {
                // hardcoded for now
                return DEFAULT_OUT_DIR;
            }
            set
            {
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Get/Set the the output Dir
        /// </summary>
        private string OutputFile
        {
            get
            {
                // hardcoded for now
                return DEFAULT_OUT_FILE;
            }
            set
            {
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Get/Set the the active WebCam to use
        /// </summary>
        private int ActiveWebCamIndex
        {
            get
            {
                // hardcoded for now
                return DEFAULT_WEBCAM_INDEX;
            }
            set
            {               
            }
        }


        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Get/Set the backend API to use for the video capture object. This is the 
        /// underlying technology that sources the frames
        /// </summary>
        private VideoCapture.API BackendAPI
        {
            get
            {
                // there are lots of these we only check for two
                if (radioButtonAPI_DShow.Checked == true) return VideoCapture.API.DShow;
                else if (radioButtonAPI_WMF.Checked == true) return VideoCapture.API.Msmf;
                else return VideoCapture.API.Msmf;
            }
            set
            {
                if (value == VideoCapture.API.DShow) radioButtonAPI_DShow.Checked = true;
                else if (value == VideoCapture.API.Msmf) radioButtonAPI_WMF.Checked = true;
                else radioButtonAPI_WMF.Checked = true;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Get/Set the screensize to use
        /// </summary>
        private Size ScreenSize
        {
            get
            {
                // these are just some of the ones from a Logitech C920 camera, they
                // differ from camera  to camera
                if (radioButtonSize_160x120.Checked == true) return new Size(160, 120);
                else if (radioButtonSize_640x480.Checked == true) return new Size(640, 480);
                else if (radioButtonSize_800x600.Checked == true) return new Size(800, 600);
                else if (radioButtonSize_1280x720.Checked == true) return new Size(1280, 1720);
                else if (radioButtonSize_1920x1080.Checked == true) return new Size(1920, 1080);
                else if (radioButtonSize_2034x1536.Checked == true) return new Size(2034, 1536);
                else return new Size(640, 480);
            }
            set
            {
                if ((value.Width == 160) && (value.Height == 120)) radioButtonSize_160x120.Checked = true;
                else if ((value.Width == 160) && (value.Height == 120)) radioButtonSize_640x480.Checked = true;
                else if ((value.Width == 800) && (value.Height == 600)) radioButtonSize_800x600.Checked = true;
                else if ((value.Width == 1280) && (value.Height == 720)) radioButtonSize_1280x720.Checked = true;
                else if ((value.Width == 1920) && (value.Height == 1080)) radioButtonSize_1920x1080.Checked = true;
                else if ((value.Width == 2034) && (value.Height == 1536)) radioButtonSize_2034x1536.Checked = true;
                else radioButtonSize_640x480.Checked = true;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Get/Set the frames per second to use
        /// </summary>
        private int FramesPerSec
        {
            get
            {
                // these are just some of the ones from a Logitech C920 camera, they
                // differ from camera  to camera
                if (radioButtonFPS_10.Checked == true) return 10;
                else if (radioButtonFPS_20.Checked == true) return 20;
                else if (radioButtonFPS_30.Checked == true) return 30;
                else return 20;
            }
            set
            {
                if (value == 10) radioButtonFPS_10.Checked = true;
                else if (value == 20) radioButtonFPS_20.Checked = true;
                else if (value == 30) radioButtonFPS_30.Checked = true;
                else radioButtonFPS_20.Checked = true;
            }
        }
    }
}
