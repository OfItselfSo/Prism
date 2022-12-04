using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

/// +------------------------------------------------------------------------------------------------------------------------------+
/// ¦                                       TERMS OF USE: EmguDualLicense                                                          ¦
/// +------------------------------------------------------------------------------------------------------------------------------¦
/// ¦                                                                                                                              ¦
/// ¦ This code is derived from the Emgu Corporation Samples. Below is a summary - see the Emgu GitHub repo for more information   ¦
/// ¦                                                                                                                              ¦
/// ¦ Emgu Corporation use a Dual License business model for its software                                                          ¦
/// + development library and offers licenses for two distinct purposes - open                                                     ¦
/// + source and commercial development                                                                                            ¦
/// ¦                                                                                                                              ¦
/// + 1. If you wish to use the open source license with an EMGU Corporation                                                       ¦
/// + product, this software is licensed under GPL V3. You agrees that you                                                         ¦
/// + will contribute all your source code to the open source community                                                            ¦
/// + and you will give them the right to share it with everyone too. The                                                          ¦
/// + complete GPL v3 terms are included in Section 2.                                                                             ¦
/// ¦                                                                                                                              ¦
/// + 2. If you want to have a commercial advantage by having a closed source                                                      ¦ 
/// + solution, you must purchase an appropriate commercial licenses from                                                          ¦
/// + EMGU Corporation. By purchasing a commercial license, you are no longer                                                      ¦
/// + obligated to publish your source code. You can use Emgu CV with the                                                          ¦
/// + commercial license terms listed in Section 3.                                                                                ¦
/// ¦                                                                                                                              ¦
/// ¦THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE          ¦
/// ¦WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR         ¦
/// ¦COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,   ¦
/// ¦ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                         ¦
/// +------------------------------------------------------------------------------------------------------------------------------+

namespace Prism4
{
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// <summary>
    /// The main form for the Prism4 application. This application is just a 
    /// minimally modified version of the CameraCapture example Emgu.CV.Example.sln 
    /// available from the EmguCV GitHub repo. 
    /// 
    /// I could not get the Emgu.CV.Example.sln to compile. It contains a lot of other
    /// things besides the CameraCapture solution and there were many, many compile 
    /// errors. 
    /// 
    /// This code is the clipped out CameraCapture code from those examples and is set 
    /// up as a standalone solution. It does compile and run and found the only webcam
    /// without issue. It is very slow to start (maybe 10-15 seconds) but runs without
    /// lag once started.  
    /// 
    /// This code has been modified as little as possible and is just a learning tool to 
    /// enable the author to get a feel for EmguCV. 
    /// 
    /// </summary>
    public partial class mainFrm : Form
    {
        private VideoCapture _capture = null;
        private bool _captureInProgress;
        private Mat _frame;
        private Mat _grayFrame;
        private Mat _smallGrayFrame;
        private Mat _smoothedGrayFrame;
        private Mat _cannyFrame;

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public mainFrm()
        {
            InitializeComponent();

            CvInvoke.UseOpenCL = false;
            try
            {
                _capture = new VideoCapture();
                // give the handler to the VideoCapture object
                _capture.ImageGrabbed += ProcessFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
            _frame = new Mat();
            _grayFrame = new Mat();
            _smallGrayFrame = new Mat();
            _smoothedGrayFrame = new Mat();
            _cannyFrame = new Mat();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Process each frame and display each stage of the processing in a 
        /// separate image on the main form.
        /// </summary>
        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame, 0);

                CvInvoke.CvtColor(_frame, _grayFrame, ColorConversion.Bgr2Gray);

                CvInvoke.PyrDown(_grayFrame, _smallGrayFrame);

                CvInvoke.PyrUp(_smallGrayFrame, _smoothedGrayFrame);

                CvInvoke.Canny(_smoothedGrayFrame, _cannyFrame, 100, 60);

                captureImageBox.Image = _frame;
                grayscaleImageBox.Image = _grayFrame;
                smoothedGrayscaleImageBox.Image = _smoothedGrayFrame;
                cannyImageBox.Image = _cannyFrame;


            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle presses on the Capture button
        /// </summary>
        private void captureButtonClick(object sender, EventArgs e)
        {
            if (_capture != null)
            {
                if (_captureInProgress)
                {  //stop the capture
                    captureButton.Text = "Start Capture";
                    _capture.Pause();
                }
                else
                {
                    //start the capture
                    captureButton.Text = "Stop";
                    _capture.Start();
                }

                _captureInProgress = !_captureInProgress;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle presses on the FlipHorizontal button
        /// </summary>
        private void FlipHorizontalButtonClick(object sender, EventArgs e)
        {
            if (_capture != null) _capture.FlipHorizontal = !_capture.FlipHorizontal;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle presses on the FlipVertical button
        /// </summary>
        private void FlipVerticalButtonClick(object sender, EventArgs e)
        {
            if (_capture != null) _capture.FlipVertical = !_capture.FlipVertical;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Clean up on exit. Called out of the override void Dispose(bool disposing)
        /// in the Designer 
        /// </summary>
        private void ReleaseData()
        {
            if (_capture != null)
                _capture.Dispose();
        }

    }
}
