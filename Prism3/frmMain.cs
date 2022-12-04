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
using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.UI; 

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

namespace Prism3
{
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// <summary>
    /// The main form for the Prism3 application. This application is designed
    /// to test out the timings of various access methods on Mat() and Image() 
    /// objects.
    /// 
    /// This code is not especially sophisticated and is basically a learning tool to 
    /// enable the author to get a feel for EmguCV. 
    /// 
    /// </summary>
    public partial class frmMain : Form
    {
        // hard coded for now
        public const string DATA_DIR = @"C:\Dump\PrismData";

        private const int SMALL_NUMBER_OF_TESTS = 1000;
        private const int LARGE_NUMBER_OF_TESTS = 1000000;

        public static MCvScalar SCALAR_GREEN = new MCvScalar(0, 255, 0);
        public static MCvScalar SCALAR_BLUE = new MCvScalar(255, 0, 0);
        public static MCvScalar SCALAR_RED = new MCvScalar(0, 0, 255);

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public frmMain()
        {
            InitializeComponent();

        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Process a file according the the current onscreen parameters
        /// </summary>
        /// <param name="filenameToProcess">the name of the file we process</param>
        private unsafe void ProcessFile(string filenameToProcess)
        {
            Stopwatch watch = null;
            string testDesc = "";
            string testCode = "";
            int iterations = SMALL_NUMBER_OF_TESTS;
            StringBuilder sb = new StringBuilder();

            if ((filenameToProcess == null) || (filenameToProcess.Length <= 3)) throw new Exception("invalid or null file name");

            // clear it down
            richTextBoxResults.Text = "";

            // get the input file as a Mat()
            Mat matImage = new Mat(filenameToProcess, Emgu.CV.CvEnum.ImreadModes.Unchanged);

            // get the input file as an Image<Bgr, Byte>
            Image<Bgr, Byte> bgrImage = new Image<Bgr, Byte>(filenameToProcess);

            // set up an image viewer in case we want to spot check the output
            // just search down the code and you can see how to implement it
            // they are commented out by default
            ImageViewer viewer = new ImageViewer(); //create an image viewer

            // try some pixel read access methods 
            byte[] pixelValue = new byte[3];
            Bgr outBGR = new Bgr();
            byte[,,] arrayValue = new byte[1, 1, 3];
            Byte[] byteArray = null;
            Byte[,,] dataArray = null;
            Image<Bgr, Byte> imgDummy = null;
            //Matrix<Byte> matrixDummy = null;
            Mat grayMatDummy = new Mat();
            Mat yuvMatDummy = new Mat();
            Image<Gray, Byte> grayImageDummy = grayMatDummy.ToImage<Gray, Byte>();
            Image<Ycc, Byte> yccImageDummy = new Image<Ycc, Byte>(bgrImage.Width, bgrImage.Height);
            Mat testMatCv8U = new Mat(512, 512, DepthType.Cv8U, 3);
            Mat testMatCv16U = new Mat(512, 512, DepthType.Cv16U, 3);
            Mat testMatCv32S = new Mat(512, 512, DepthType.Cv32S, 3);

            Image<Bgr, byte> testImageByte = new Image<Bgr, byte>(512, 512);
            Image<Bgr, Int16> testImageInt16 = new Image<Bgr, Int16>(512, 512);
            Image<Bgr, Int32> testImageInt32 = new Image<Bgr, Int32>(512, 512);

            sb.Append("EmguCV tests on img: " + filenameToProcess + "\r\n");
            sb.Append("Image Size: Width=" + bgrImage.Width.ToString() + ", Height=" +bgrImage.Height.ToString() + "\r\n");
            sb.Append("\r\n");


            sb.Append("\r\n");
            sb.Append("########################################################################\r\n");
            sb.Append("Tests reading a 3 byte pixel from a Mat()\r\n");
            sb.Append("########################################################################\r\n");
            sb.Append("\r\n");

            // test
            testDesc = "access via recommended conversion to Image and then fast Image<> access";
            testCode = "single call to: matImage.ToImage<Bgr, Byte>(); then 3x pixelValue[?] = bgrImage.Bytes[?];";
            iterations = SMALL_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                imgDummy = matImage.ToImage<Bgr, Byte>();
                pixelValue[0] = imgDummy.Data[0, 0, 0];
                pixelValue[1] = imgDummy.Data[0, 0, 1];
                pixelValue[2] = imgDummy.Data[0, 0, 2];
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations*1000)/(watch.ElapsedMilliseconds) }");
            sb.Append("\r\n\r\n");

            // test
            testDesc = "access via recommended conversion to Matrix and then direct Matrix access";
            testCode = "appears to be buggy, skipped";
            //    testCode = "single call to:new Matrix<Byte>(matImage.Rows, matImage.Cols, matImage.NumberOfChannels);";
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                //matrixDummy = new Matrix<Byte>(matImage.Rows, matImage.Cols, matImage.NumberOfChannels);
                //matImage.CopyTo(matrixDummy);
                //var xx = matrixDummy.Data[0, 0];
                break;
            }
            watch.Stop();
           // sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: 0");
            sb.Append("\r\n\r\n");

            // test            
            // this technique cannot be trusted - seems to corrupt memory occasionally on larger copies

            testDesc = "copy via Interop Marshal.Copy using pointers";
            testCode = "single call to: Marshal.Copy(matImage.DataPointer, pixelValue, 0, 3);";
            iterations = LARGE_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                Marshal.Copy(matImage.DataPointer, pixelValue, 0, 3);
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations*1000)/(watch.ElapsedMilliseconds) }");
            sb.Append("\r\nNOTE: this technique cannot be trusted - seems to corrupt memory occasionally on larger copies");
            sb.Append("\r\n\r\n");


            sb.Append("\r\n");
            sb.Append("########################################################################\r\n");
            sb.Append("Tests reading a 3 byte pixel from an Image<Bgr, Byte>\r\n");
            sb.Append("########################################################################\r\n");
            sb.Append("\r\n");

            // test
            testDesc = "recommended access via Image[,]";
            testCode = "single call to: outBGR = bgrImage[row, col];";
            iterations = LARGE_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                outBGR = bgrImage[100, 100]; 
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations*1000)/(watch.ElapsedMilliseconds) }");
            sb.Append("\r\n\r\n");

            // test
            testDesc = "access via the Image<>.bytes[]";
            testCode = "sequence of three: pixelValue[?] = bgrImage.Bytes[?];";
            iterations = SMALL_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                pixelValue[0] = bgrImage.Bytes[0];
                pixelValue[1] = bgrImage.Bytes[1];
                pixelValue[2] = bgrImage.Bytes[2];
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations*1000)/(watch.ElapsedMilliseconds) }");
            sb.Append("\r\n\r\n");

            // test
            testDesc = "direct access via Image<>.Data[,,]";
            testCode = "sequence of three: pixelValue[?] = bgrImage.Data[row, col, ?];";
            iterations = LARGE_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                pixelValue[0] = bgrImage.Data[0, 0, 0];
                pixelValue[1] = bgrImage.Data[0, 0, 1];
                pixelValue[2] = bgrImage.Data[0, 0, 2];
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations*1000)/(watch.ElapsedMilliseconds) }");
            sb.Append("\r\n\r\n");

            // test
            testDesc = "array copy via Array.Copy()";
            testCode = "single call to: Array.Copy(bgrImage.Data, 0, arrayValue, 0, 3);";
            iterations = LARGE_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                Array.Copy(bgrImage.Data, 0, arrayValue, 0, 3); 
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations*1000)/(watch.ElapsedMilliseconds) }");
            sb.Append("\r\n\r\n");

            // test
            //NOTE: this technique cannot be trusted -seems to corrupt memory occasionally on larger copies testDesc = "copy via Interop Marshal.Copy using pointers";

            testCode = "single call to: Marshal.Copy(bgrImage.Ptr, pixelValue, 0, 3);;";
            iterations = LARGE_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                Marshal.Copy(bgrImage.Ptr, pixelValue, 0, 3);
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations*1000)/(watch.ElapsedMilliseconds) }");
            sb.Append("\r\nNOTE: this technique cannot be trusted - seems to corrupt memory occasionally on larger copies");
            sb.Append("\r\n\r\n");

            sb.Append("\r\n");
            sb.Append("########################################################################\r\n");
            sb.Append("Tests setting Image<> and Mat() to a solid color \r\n");
            sb.Append("########################################################################\r\n");
            sb.Append("\r\n");

            // test
            testDesc = "Set Cv8U Mat to Solid Color";
            testCode = "single call to:  testMatCv8U.SetTo(SCALAR_RED)";
            iterations = SMALL_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                // Set the image
                testMatCv8U.SetTo(SCALAR_RED);
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            sb.Append("\r\n\r\n");

            //viewer.Image = testMatCv8U;
            //viewer.ShowDialog(); //show the image viewer
            // test
            testDesc = "Set Cv32S Mat to Solid Color";
            testCode = "single call to:  testMatCv32S.SetTo(SCALAR_RED)";
            iterations = SMALL_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                // Set the image
                testMatCv32S.SetTo(SCALAR_RED);
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            sb.Append("\r\n\r\n");

            //viewer.Image = testMatCv8U;
            //viewer.ShowDialog(); //show the image viewer

            // test
            testDesc = "Set 8 bit Image<> to Solid Color";
            testCode = "single call to: testImageByte.SetValue(SCALAR_RED)";
            iterations = SMALL_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                // Set the image
                testImageByte.SetValue(SCALAR_RED);
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            sb.Append("\r\n\r\n");

            // test
            testDesc = "Set 32 bit Image<> to Solid Color";
            testCode = "single call to: testImageInt32.SetValue(SCALAR_RED)";
            iterations = SMALL_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                // Set the image
                testImageInt32.SetValue(SCALAR_RED);
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            sb.Append("\r\n\r\n");

            //viewer.Image = testMatCv8U;
            //viewer.ShowDialog(); //show the image viewer

            sb.Append("\r\n");
            sb.Append("########################################################################\r\n");
            sb.Append("Tests converting Image<> and Mat() from one format to another \r\n");
            sb.Append("########################################################################\r\n");
            sb.Append("\r\n");

            // test
            testDesc = "convert Mat from Bgr to Bgr2Gray";
            testCode = "single call to:  CvInvoke.CvtColor(matImage, matImage, ColorConversion.Bgr2Gray);";
            iterations = SMALL_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                //Convert the image to grayscale 
                CvInvoke.CvtColor(matImage, grayMatDummy, ColorConversion.Bgr2Gray);
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations*1000)/(watch.ElapsedMilliseconds) }");
            sb.Append("\r\n\r\n");

            //viewer.Image = grayMatDummy;
            //viewer.ShowDialog(); //show the image viewer

            // test
            testDesc = "convert Image<Bgr, byte> from Bgr to Bgr2Gray";
            testCode = "single call to:  CvInvoke.CvtColor(bgrImage, grayImageDummy, ColorConversion.Bgr2Gray);";
            iterations = SMALL_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                //Convert the image to grayscale 
                CvInvoke.CvtColor(bgrImage, grayImageDummy, ColorConversion.Bgr2Gray);
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations*1000)/(watch.ElapsedMilliseconds) }");
            sb.Append("\r\n\r\n");

            // test
            testDesc = "convert Mat from Bgr to YUV";
            testCode = "single call to:  CvInvoke.CvtColor(matImage, yuvMatDummy, ColorConversion.Bgr2Yuv";
            iterations = SMALL_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                //Convert the image to YUV 
                CvInvoke.CvtColor(matImage, yuvMatDummy, ColorConversion.Bgr2Yuv);

                // NOTE the colors do not translate correctly in this conversion

                // convert back, the Bgr colors do convert back to what they should be
                // CvInvoke.CvtColor(yuvMatDummy, matImage, ColorConversion.Yuv2Bgr);
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            sb.Append("\r\n" + "NOTE: the colors do not seem to translate correctly in this conversion");
            sb.Append("\r\n\r\n");

            //viewer.Image = yuvMatDummy;
            //viewer.Image = matImage;
            //viewer.ShowDialog(); //show the image viewer

            // test
            testDesc = "convert Image from Bgr to YUV";
            testCode = "single call to: CvInvoke.CvtColor(bgrImage, yccImageDummy, ColorConversion.Bgr2Yuv);";
            iterations = SMALL_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                //Convert the image to YUV 
                CvInvoke.CvtColor(bgrImage, yccImageDummy, ColorConversion.Bgr2Yuv);

                // NOTE the colors do not translate correctly in this conversion

                // convert back, the Bgr colors do convert back to what they should be
                // CvInvoke.CvtColor(yccImageDummy, bgrImage, ColorConversion.Yuv2Bgr);
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            sb.Append("\r\n" + "NOTE: the colors do not seem to translate correctly in this conversion");
            sb.Append("\r\n\r\n");

            //viewer.Image = yccImageDummy;
            //viewer.Image = bgrImage;
            //viewer.ShowDialog(); //show the image viewer

            sb.Append("\r\n");
            sb.Append("########################################################################\r\n");
            sb.Append("Tests getting an array of data from Image<> and Mat() \r\n");
            sb.Append("########################################################################\r\n");
            sb.Append("\r\n");

            // test
            testDesc = "Get 8 bit Byte[] from Mat() via RawData()";
            testCode = "single call to: byteArray = matImage.GetRawData();";
            iterations = SMALL_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                // Set the image
                byteArray = matImage.GetRawData();
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            sb.Append("\r\n\r\n");

            //viewer.Image = imgDummy;
            //viewer.ShowDialog(); //show the image viewer

            //// test            
            // this technique cannot be trusted - seems to corrupt memory occasionally

            //testDesc = "Get Byte[] from 8 bit Mat() Via Marshal.Copy";
            //testCode = "Single call to:  Marshal.Copy(matImage.Ptr, targetArray, 0, matImage.Height * matImage.Width * matImage.NumberOfChannels)";
            //iterations = SMALL_NUMBER_OF_TESTS;
            //watch = Stopwatch.StartNew();
            //Byte[] targetArray = new Byte[matImage.Height * matImage.Width * matImage.NumberOfChannels]; 
            //for (int i = 0; i < iterations; i++)
            //{
            //    Marshal.Copy(matImage.Ptr, targetArray, 0, matImage.Height * matImage.Width * matImage.NumberOfChannels);
            //}
            //watch.Stop();
            //sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            //sb.Append("\r\nNOTE: this technique cannot be trusted - seems to corrupt memory occasionally on larger copies");
            //sb.Append("\r\n\r\n");

            // test
            testDesc = "Get 8 bit Byte[] from Image<> via Bytes";
            testCode = "single call to: byteArray = bgrImage.Bytes;";
            iterations = SMALL_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            dataArray = bgrImage.Data;
            for (int i = 0; i < iterations; i++)
            {
                // Set the image
                byteArray = bgrImage.Bytes;
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            sb.Append("\r\n\r\n");

            //viewer.Image = imgDummy;
            //viewer.ShowDialog(); //show the image viewer

            // test
            // this technique cannot be trusted - seems to corrupt memory occasionally
            //testDesc = "Get Byte[] from 8 bit Image<Bgr,Byte> Via Marshal.Copy";
            //testCode = "Single call to: Marshal.Copy(bgrImage.Ptr, targetArray, 0, matImage.Height * matImage.Width * matImage.NumberOfChannels)";
            //iterations = SMALL_NUMBER_OF_TESTS;
            //int[] targetArray1 = new int[bgrImage.Height * bgrImage.Width * bgrImage.NumberOfChannels];
            //watch = Stopwatch.StartNew();
            //for (int i = 0; i < iterations; i++)
            //{
            //    Marshal.Copy(bgrImage.Ptr, targetArray1, 0, bgrImage.Height * bgrImage.Width * bgrImage.NumberOfChannels);
            //}
            //watch.Stop();
            //sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            //sb.Append("\r\nNOTE: this technique cannot be trusted - seems to corrupt memory occasionally on larger copies");
            //sb.Append("\r\n\r\n");

            sb.Append("\r\n");
            sb.Append("########################################################################\r\n");
            sb.Append("Tests loading Image<> and Mat() from arrays \r\n");
            sb.Append("########################################################################\r\n");
            sb.Append("\r\n");

            // test
            testDesc = "Populate 8 bit Image<Bgr, Byte> from Byte[,,] Via Array Copy";
            testCode = "single call to: Array.Copy(dataArray, 0, imgDummy.Data, 0, (bgrImage.Height * bgrImage.Width * bgrImage.NumberOfChannels));";
            iterations = SMALL_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            dataArray = bgrImage.Data;
            for (int i = 0; i < iterations; i++)
            {
                // Set the image
                Array.Copy(dataArray, 0, imgDummy.Data, 0, (bgrImage.Height * bgrImage.Width * bgrImage.NumberOfChannels));
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            sb.Append("\r\n\r\n");

            //viewer.Image = imgDummy;
            //viewer.ShowDialog(); //show the image viewer

            // test
            testDesc = "Populate 8 bit Image<Bgr, Byte> from Byte[] Via Pinned Memory";
            testCode = "Setup Pinned Memory and single call to: imgDummy = new Image<Bgr, Byte>(matImage.Height, matImage.Width, 0, ptr);";
            iterations = LARGE_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            byteArray = matImage.GetRawData();
            for (int i = 0; i < iterations; i++)
            {
                // Set the image
                GCHandle pArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
                IntPtr ptr = pArray.AddrOfPinnedObject();
                imgDummy = new Image<Bgr, Byte>(matImage.Height, matImage.Width, 0, ptr);
                pArray.Free();
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            sb.Append("\r\nNOTE: includes the time to setup the Pinned Memory");
            sb.Append("\r\n\r\n");

            //viewer.Image = imgDummy;
            //viewer.ShowDialog(); //show the image viewer

            // test
            testDesc = "Populate 8 bit Mat() from Byte[] Via Pinned Memory";
            testCode = "single call to: testMat4 = new Mat(matImage.Height, matImage.Width, DepthType.Cv8U, 3, pointer, 0);";
            iterations = LARGE_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            byteArray = matImage.GetRawData();
            GCHandle pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();
            for (int i = 0; i < iterations; i++)
            {
                // Set the image
                testMatCv8U = new Mat(matImage.Height, matImage.Width, DepthType.Cv8U, 3, pointer, 0);
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            sb.Append("\r\nNOTE: does not include the time to setup the Pinned Memory");
            sb.Append("\r\n\r\n");

            //viewer.Image = testMatCv8U;
            //viewer.ShowDialog(); //show the image viewer
            pinnedArray.Free();

            // test
            testDesc = "Populate 8 bit Mat() from Byte[] Via Pinned Memory";
            testCode = "Setup pinned memory and single call to: testMat4 = new Mat(matImage.Height, matImage.Width, DepthType.Cv8U, 3, pointer, 0);";
            iterations = LARGE_NUMBER_OF_TESTS;
            watch = Stopwatch.StartNew();
            byteArray = matImage.GetRawData();
            for (int i = 0; i < iterations; i++)
            {
                // Set the image
                GCHandle pArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
                IntPtr ptr = pArray.AddrOfPinnedObject();
                testMatCv8U = new Mat(matImage.Height, matImage.Width, DepthType.Cv8U, 3, ptr, 0);
                pArray.Free();
            }
            watch.Stop();
            sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            sb.Append("\r\nNOTE: includes the time to setup the Pinned Memory");
            sb.Append("\r\n\r\n");

            // viewer.Image = testMatCv8U;
            // viewer.ShowDialog(); //show the image viewer

            //// test
            // this technique cannot be trusted - seems to corrupt memory occasionally

            //testDesc = "Populate 8 bit Mat() from Byte[] Via Marshal.Copy";
            //testCode = "Single call to: Marshal.Copy(byteArray, 0, testMatCv8U.Ptr, (matImage.Height * matImage.Width * matImage.NumberOfChannels))";
            //iterations = LARGE_NUMBER_OF_TESTS;
            //watch = Stopwatch.StartNew();
            //byteArray = matImage.GetRawData();
            //for (int i = 0; i < iterations; i++)
            //{
            //    Marshal.Copy(byteArray, 0, testMatCv8U.Ptr, (matImage.Height * matImage.Width * matImage.NumberOfChannels));                
            //}
            //watch.Stop();
            //sb.Append(testDesc + "\r\n" + testCode + "\r\n" + $"Iterations: {iterations}, Execution Time: {watch.ElapsedMilliseconds} ms, Operations/Sec: { (iterations * 1000) / (watch.ElapsedMilliseconds) }");
            //sb.Append("\r\nNOTE: this technique cannot be trusted - seems to corrupt memory occasionally on larger copies");
            //sb.Append("\r\n\r\n");

            //viewer.Image = testMatCv8U;
            //viewer.ShowDialog(); //show the image viewer

            // display it
            richTextBoxResults.Text = sb.ToString();
        }

        private unsafe void buttonTest_Click(object sender, EventArgs e)
        {


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
                // process it
                ProcessFile(Openfile.FileName);
            }
            else
            {

            }
        }

    }
}
