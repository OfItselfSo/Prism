using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

using MediaFoundation;
using MediaFoundation.Misc;
using MediaFoundation.Transform;
using TantaCommon;

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

/// Large parts of this code are derived from the samples which ship with the MF.Net dll. These are 
/// in turn derived from the original Microsoft samples. These have been placed in the public domain 
/// without copyright. The original copyright statement is below

/// *****************************************************************************
/// Original Copyright Statement - Released to public domain
/// While the underlying library is covered by LGPL or BSD, this sample is released
/// as public domain.  It is distributed in the hope that it will be useful, but
/// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
/// or FITNESS FOR A PARTICULAR PURPOSE.
/// ******************************************************************************

/// This file implements a Synchronous Media Foundation Transform (MFT)
/// which detects solid red circles as they pass through the transform.
/// 
/// The detected circles are marked on the screen with a black cross.
/// EmguCV is used as the detection mechanism and this transform serves
/// to demonstrate a method of integrating EmguCV into a Window Media 
/// Foundation pipeline.
/// 
/// This transform only supports one media type (ARGB) and the input and
/// output types must both be this.
/// 
/// This class uses the TantaMFTBase_Sync base class and much of the 
/// standard processing is handled there. This base class is an only
/// slightly modified version of the MFTBase class which ships with the 
/// MF.Net samples. The MFTBase class (and hence TantaMFTBase_Sync) is
/// designed to factor out all of the common code required to build an 
/// Synchronous MFT in C# MF.Net. 
/// 
/// In the interests of simplicity, this particular MFT is not designed
/// to be "independent" of the rest of the application. In other words,
/// it will not be placed in an independent assembly (DLL) it will not 
/// be COM visible or registered with MFTRegister so that other applications
/// can use it. This MFT is expected to be instantiated with a standard
/// C# new operator and simply given to the topology as a binary
/// 

namespace Prism6
{
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// <summary>
    /// An MFT to demonstrate how to use EmguCV to detect solid red circles in video frames. 
    /// 
    /// This MFT can handle 1 media type (ARGB). You will also note that it
    /// hard codes the support for this type
    /// 
    /// </summary>
    public sealed class MFTDetectCircles_Sync : TantaMFTBase_Sync
    {
        // Format information
        private int m_imageWidthInPixels;
        private int m_imageHeightInPixels;
        private int m_cbImageSize;              // Image size, in bytes.
        private int m_lStrideIfContiguous;
        private int m_FrameCount;               // only used to have something to write on the screen

        // only used to if we write the text onto the video buffer. This functionality is actually commented out
        // but someone might want it and this code has been left in to demonstrate how to create the components with out 
        // memory leaks. We do not want to have to create these for each frame so we create it once and re-use it each time
        private static readonly SolidBrush m_transparentBrush = new SolidBrush(Color.FromArgb(96, 0, 0, 255));
        private Font m_fontOverlay;
        private Font m_transparentFont;
 
        // this list of the guids of the media subtypes we support. The input format must be the same
        // as the output format 
        private readonly Guid[] m_MediaSubtypes = new Guid[] { MFMediaType.RGB32 };

        // we do not support interlacing. If the Media Type proposed by the client says
        // it "might be interlaced" we set this flag. If interlaced frames are passe in, we will reject them 
        private bool m_MightBeInterlaced;

        // these are used by EmguCV for the Circle detection and marking
        public const int CENTROID_CROSS_BAR_LEN = 10;
        public static Pen blackPen = new Pen(Color.Black, 1);
        public static MCvScalar BLUE_RANGE_LOW = new MCvScalar(150, 0, 0);
        public static MCvScalar BLUE_RANGE_HIGH = new MCvScalar(255, 150, 150);
        public static MCvScalar GREEN_RANGE_LOW = new MCvScalar(0, 150, 0);
        public static MCvScalar GREEN_RANGE_HIGH = new MCvScalar(150, 255, 150);
        public static MCvScalar RED_RANGE_LOW = new MCvScalar(0, 0, 150);
        public static MCvScalar RED_RANGE_HIGH = new MCvScalar(150, 150, 255);
        public static MCvScalar SCALAR_BLUE = new MCvScalar(255, 0, 0);
        public static MCvScalar SCALAR_RED = new MCvScalar(0, 0, 255);
        public static MCvScalar SCALAR_TEST = new MCvScalar(100, 101, 102);

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public MFTDetectCircles_Sync() : base()
        {
            // init this now
            m_FrameCount = 0;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Destructor
        /// </summary>
        ~MFTDetectCircles_Sync()
        {
            SafeRelease(m_transparentBrush);
            SafeRelease(m_fontOverlay);
            SafeRelease(m_transparentFont);
        }


        // ########################################################################
        // ##### TantaMFTBase_Sync Overrides, all child classes must implement these
        // ########################################################################

        #region Overrides

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Returns a value indicating if the proposed input type is acceptable to 
        /// this MFT.
        /// 
        /// An override of the abstract version in TantaMFTBase_Sync. 
        /// </summary>
        /// <param name="pmt">The type to check.  Should never be null.</param>
        /// <returns>S_Ok if the type is valid or MF_E_INVALIDTYPE.</returns>
        override protected HResult OnCheckInputType(IMFMediaType pmt)
        {
            HResult hr;

            // We assume the input type will get checked first
            if (OutputType == null)
            {
                // we do not have an output type, check that the proposed
                // input type is acceptable
                hr = OnCheckMediaType(pmt);
            }
            else
            {
                // we have an output type
                hr = TantaWMFUtils.IsMediaTypeIdentical(pmt, OutputType);
            }

            return hr;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Return settings to describe input stream. This should get the buffer 
        /// requirements and other information for an input stream. 
        /// (see IMFTransform::GetInputStreamInfo).
        /// 
        /// An override of the abstract version in TantaMFTBase_Sync. 
        /// </summary>
        /// <param name="pStreamInfo">The struct where the parameters get set.</param>
        override protected void OnGetInputStreamInfo(ref MFTInputStreamInfo pStreamInfo)
        {
            // return the image size
            pStreamInfo.cbSize = m_cbImageSize;
            // MFT_INPUT_STREAM_WHOLE_SAMPLES - Each media sample(IMFSample interface) of 
            //    input data from the MFT contains complete, unbroken units of data. 
            // MFT_INPUT_STREAM_SINGLE_SAMPLE_PER_BUFFER -  Each input sample contains 
            //    exactly one unit of data 
            // MFT_INPUT_STREAM_FIXED_SAMPLE_SIZE - All input samples are the same size.
            // MFT_INPUT_STREAM_PROCESSES_IN_PLACE - The MFT can perform in-place processing.
            //     In this mode, the MFT directly modifies the input buffer. When the client calls 
            //     ProcessOutput, the same sample that was delivered to this stream is returned in 
            //     the output stream that has a matching stream identifier. This flag implies that 
            //     the MFT holds onto the input buffer, so this flag cannot be combined with the 
            //     MFT_INPUT_STREAM_DOES_NOT_ADDREF flag. If this flag is present, the MFT must 
            //     set the MFT_OUTPUT_STREAM_PROVIDES_SAMPLES or MFT_OUTPUT_STREAM_CAN_PROVIDE_SAMPLES 
            //     flag for the output stream that corresponds to this input stream. 
            pStreamInfo.dwFlags = MFTInputStreamInfoFlags.WholeSamples |
                                  MFTInputStreamInfoFlags.FixedSampleSize |
                                  MFTInputStreamInfoFlags.SingleSamplePerBuffer |
                                  MFTInputStreamInfoFlags.ProcessesInPlace;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Return settings to describe output stream. This should get the buffer 
        /// requirements and other information for an output stream. 
        /// (see IMFTransform::GetOutputStreamInfo).
        /// 
        /// An override of the abstract version in TantaMFTBase_Sync. 
        /// </summary>
        /// <param name="pStreamInfo">The struct where the parameters get set.</param>
        override protected void OnGetOutputStreamInfo(ref MFTOutputStreamInfo pStreamInfo)
        {
            // return the image size
            pStreamInfo.cbSize = m_cbImageSize;

            // MFT_OUTPUT_STREAM_WHOLE_SAMPLES - Each media sample(IMFSample interface) of 
            //    output data from the MFT contains complete, unbroken units of data. 
            // MFT_OUTPUT_STREAM_SINGLE_SAMPLE_PER_BUFFER -  Each output sample contains 
            //    exactly one unit of data 
            // MFT_OUTPUT_STREAM_FIXED_SAMPLE_SIZE - All output samples are the same size.
            // MFT_OUTPUT_STREAM_PROVIDES_SAMPLES - The MFT provides the output samples 
            //    for this stream, either by allocating them internally or by operating 
            //    directly on the input samples. The MFT cannot use output samples provided 
            //    by the client for this stream. If this flag is not set, the MFT must 
            //    set cbSize to a nonzero value in the MFT_OUTPUT_STREAM_INFO structure, 
            //    so that the client can allocate the correct buffer size. For more information,
            //    see IMFTransform::GetOutputStreamInfo. This flag cannot be combined with 
            //    the MFT_OUTPUT_STREAM_CAN_PROVIDE_SAMPLES flag.
            pStreamInfo.dwFlags = MFTOutputStreamInfoFlags.WholeSamples |
                                  MFTOutputStreamInfoFlags.SingleSamplePerBuffer |
                                  MFTOutputStreamInfoFlags.FixedSampleSize |
                                  MFTOutputStreamInfoFlags.ProvidesSamples;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// This is the routine that performs the transform. Assumes InputSample is set.
        ///
        /// An override of the abstract version in TantaMFTBase_Sync. 
        /// </summary>
        /// <param name="outputSampleDataStruct">The structure to populate with output data.</param>
        /// <returns>S_Ok unless error.</returns>
        protected override HResult OnProcessOutput(ref MFTOutputDataBuffer outputSampleDataStruct)
        {
            HResult hr = HResult.S_OK;
            IMFMediaBuffer outputMediaBuffer = null;

            // we are processing in place, the input sample is the output sample, the media buffer of the
            // input sample is the media buffer of the output sample.

            try
            {
                // Get the data buffer from the input sample. If the sample contains more than one buffer, 
                // this method copies the data from the original buffers into a new buffer, and replaces 
                // the original buffer list with the new buffer. The new buffer is returned in the inputMediaBuffer parameter.
                // If the sample contains a single buffer, this method returns a pointer to the original buffer. 
                // In typical use, most samples do not contain multiple buffers.
                hr = InputSample.ConvertToContiguousBuffer(out outputMediaBuffer);
                if (hr != HResult.S_OK)
                {
                    throw new Exception("OnProcessOutput call to InputSample.ConvertToContiguousBuffer failed. Err=" + hr.ToString());
                }

                // now that we have an output buffer, do the work to write text on them.
                // Writing into outputMediaBuffer will write to the approprate location in the outputSample
                DetectCirclesInBuffer(outputMediaBuffer);

                // Set status flags.
                outputSampleDataStruct.dwStatus = MFTOutputDataBufferFlags.None;
                // The output sample is the input sample. We get a new IUnknown for the Input
                // sample since we are going to release it below. The client will release this 
                // new IUnknown
                outputSampleDataStruct.pSample = Marshal.GetIUnknownForObject(InputSample);

            }
            finally
            {
                // clean up
                SafeRelease(outputMediaBuffer);

                // Release the current input sample so we can get another one.
                // the act of setting it to null releases it because the property
                // is coded that way
                InputSample = null;
            } 

            return HResult.S_OK;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// The MFT defines a list of available media types for each input stream
        /// and orders them by preference. This method enumerates the available
        /// media types for an input stream. 
        /// 
        /// Many clients will just "try it on" with their preferred media type
        /// and if/when that gets rejected will start enumerating the types the
        /// transform prefers in order to see if they have one in common
        ///
        /// An override of the virtual version in TantaMFTBase_Sync. 
        /// </summary>
        /// <param name="dwTypeIndex">The (zero-based) index of the type.</param>
        /// <param name="pInputType">The input type supported by the MFT.</param>
        /// <returns>S_Ok unless error.</returns>
        protected override HResult OnEnumInputTypes(int dwTypeIndex, out IMFMediaType pInputType)
        {
            return TantaWMFUtils.CreatePartialMediaType(dwTypeIndex, MFMediaType.Video, m_MediaSubtypes, out pInputType);
        }

        #endregion

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        ///  Called when the input type gets set. We record the basic image 
        ///  size and format information here.
        ///  
        ///  Expects the InputType variable to have been set. This will have been
        ///  done in the base class immediately before this routine gets called
        ///
        ///  An override of the virtual stub in TantaMFTBase_Sync. 
        /// </summary>
        override protected void OnSetInputType()
        {
            HResult hr;
            float fSize;

            // init some things
            m_imageWidthInPixels = 0;
            m_imageHeightInPixels = 0;
            m_cbImageSize = 0;
            m_lStrideIfContiguous = 0;

            // get this now, the type can be null to clear
            IMFMediaType pmt = InputType;
            if (pmt == null)
            {
                // Since the input must be set before the output, nulling the 
                // input must also clear the output.  Note that nulling the 
                // input is only valid if we are not actively streaming.
                OutputType = null;
                return;
            }

            // get the image width and height in pixels. These will become 
            // very important later when the time comes to size and center the 
            // text we will write on the screen.

            // Note that changing the size of the image on the screen, by resizing
            // the EVR control does not change this image size. The EVR will 
            // remove various rows and columns as necssary for display purposes
            hr = MFExtern.MFGetAttributeSize(pmt, MFAttributesClsid.MF_MT_FRAME_SIZE, out m_imageWidthInPixels, out m_imageHeightInPixels);
            if (hr != HResult.S_OK)
            {
                throw new Exception("OnSetInputType call to get the image size failed failed. Err=" + hr.ToString());
            }

            // get the image size
            hr = MFExtern.MFGetAttributeSize(pmt, MFAttributesClsid.MF_MT_FRAME_SIZE, out m_imageWidthInPixels, out m_imageHeightInPixels);
            if (hr != HResult.S_OK)
            {
                throw new Exception("OnSetInputType call to get MFGetAttributeSize failed failed. Err=" + hr.ToString());
            }

            // get the image stride. The stride is the number of bytes from one row of pixels in memory 
            // to the next row of pixels in memory. If padding bytes are present, the stride is wider 
            // than the width of the image.
            hr = pmt.GetUINT32(MFAttributesClsid.MF_MT_DEFAULT_STRIDE, out m_lStrideIfContiguous);
            if (hr != HResult.S_OK)
            {
                throw new Exception("OnSetInputType call to MF_MT_DEFAULT_STRIDE failed failed. Err=" + hr.ToString());
            }

            // Calculate the image size (including padding)
            m_cbImageSize = m_imageHeightInPixels * m_lStrideIfContiguous;

            // now perform the initial setup of the fonts we will use to draw the text.
            // since this information does not change (without a format change event)
            // this is done once here, rather than over and over again for each frame

            // first the overlay font which we use for the main centered text
            // scale the font size in some portion to the video image
            fSize = 9;
            fSize *= (m_imageWidthInPixels / 64.0f);

            // clean up
            if (m_fontOverlay != null)  m_fontOverlay.Dispose();

            // create the font
            m_fontOverlay = new Font(
                "Times New Roman", 
                fSize, 
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point);

            // now the transparent font for the frame count in the 
            // bottom right hand corner
            // scale the font size in some portion to the video image
            fSize = 5;
            fSize *= (m_imageWidthInPixels / 64.0f);

            if (m_transparentFont != null) m_transparentFont.Dispose();

            m_transparentFont = new Font(
                "Tahoma", 
                fSize, 
                System.Drawing.FontStyle.Bold, 
                System.Drawing.GraphicsUnit.Point);

            // If the output type isn't set yet, we can pre-populate it, 
            // since output must always exactly equal input.  This can 
            // save a (tiny) bit of time in negotiating types.

            OnSetOutputType();

        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        ///  Called when the output type should be set. Since our output type must be the 
        ///  same as the input type we just create the output type as a copy of 
        ///  the input type here
        ///  
        ///  Expects the InputType variable to have been set.
        ///
        ///  An override of the virtual stub in TantaMFTBase_Sync. 
        /// </summary>
        protected override void OnSetOutputType()
        {
            // If the output type is null or is being reset to null (by 
            // dynamic format change), pre-populate it.
            if (InputType != null && OutputType == null)
            {
                OutputType = CreateOutputFromInput();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Processes the input. Most of the transformation happens in OnProcessOutput
        /// so all we need to do here is check to see if the sample is interlaced and, if
        /// it is, discard it.
        /// 
        /// Expects InputSample to be set.
        /// </summary>
        /// <returns>S_Ok or E_FAIL.</returns>
        protected override HResult OnProcessInput()
        {
            HResult hr = HResult.S_OK;

            // While we accept types that *might* be interlaced, if we actually receive
            // an interlaced sample, reject it.
            if (m_MightBeInterlaced == true)
            {
                int ix;

                // Returns a bool: true = interlaced, false = progressive
                hr = InputSample.GetUINT32(MFAttributesClsid.MFSampleExtension_Interlaced, out ix);
                if (hr != HResult.S_OK || ix != 0)
                {
                    hr = HResult.E_FAIL;
                }
            }

            return hr;
        }

        #region Helpers

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Validates a media type for this transform. Since both input and output types must be
        /// the same, they both call this routine.
        /// </summary>
        /// <param name="pmt">The media type to validate.</param>
        /// <returns>S_Ok or MF_E_INVALIDTYPE.</returns>
        private HResult OnCheckMediaType(IMFMediaType pmt)
        {
            int interlace;
            HResult hr = HResult.S_OK;

            // see if the media type is one of our list of acceptable subtypes
            hr = TantaWMFUtils.CheckMediaType(pmt, MFMediaType.Video, m_MediaSubtypes);
            if (hr != HResult.S_OK)
            {
                return HResult.MF_E_INVALIDTYPE;
            }

            // Video must be progressive frames. Set this now
            m_MightBeInterlaced = false;

            // get the interlace mode
            hr = pmt.GetUINT32(MFAttributesClsid.MF_MT_INTERLACE_MODE, out interlace);
            if (hr != HResult.S_OK)
            {
                throw new Exception("OnCheckMediaType call to getting the interlace mode failed. Err=" + hr.ToString());
            }
            // set it now
            MFVideoInterlaceMode im = (MFVideoInterlaceMode)interlace;

            // Mostly we only accept Progressive.
            if (im == MFVideoInterlaceMode.Progressive) return HResult.S_OK;
            // If the type MIGHT be interlaced, we'll accept it.
            if (im == MFVideoInterlaceMode.MixedInterlaceOrProgressive)
            {
                // But we will check to see if any samples actually
                // are interlaced, and reject them.
                m_MightBeInterlaced = true;

                return HResult.S_OK;
            }
            // not a valid option
            return HResult.MF_E_INVALIDTYPE;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Detect Circles in the output buffer
        /// </summary>
        /// <param name="outputMediaBuffer">Output buffer</param>
        private void DetectCirclesInBuffer(IMFMediaBuffer outputMediaBuffer)
        {
            IntPtr destRawDataPtr = IntPtr.Zero;			// Destination buffer.
            int destStride=0;	                            // Destination stride.
            bool destIs2D = false;

            try
            {
                // Lock the output buffer. Use the IMF2DBuffer interface  
                // (if available) as it is faster
                if ((outputMediaBuffer is IMF2DBuffer) == false)
                {
                    // not an IMF2DBuffer - get the raw data from the IMFMediaBuffer 
                    int maxLen =0;
                    int currentLen = 0;
                    TantaWMFUtils.LockIMFMediaBufferAndGetRawData(outputMediaBuffer,  out destRawDataPtr, out maxLen, out currentLen);
                    // the stride is always this. The Lock function does not return it
                    destStride = m_lStrideIfContiguous;
                }
                else
                {
                    // we are an IMF2DBuffer, we get the stride here as well
                    TantaWMFUtils.LockIMF2DBufferAndGetRawData((outputMediaBuffer as IMF2DBuffer), out destRawDataPtr, out destStride);
                    destIs2D = true;
                }

                // count this now. We only use this to write it on the screen
                m_FrameCount++;

                // We could eventually offer the ability to write on other formats depending on the 
                // current media type. We have this hardcoded to ARGB for now
                DetectCirclesInImageOfTypeRGB32( destRawDataPtr, 
                                destStride,
                                m_imageWidthInPixels, 
                                m_imageHeightInPixels);

                // Set the data size on the output buffer. It probably is already there
                // since the output buffer is the input buffer
                HResult hr = outputMediaBuffer.SetCurrentLength(m_cbImageSize);
                if (hr != HResult.S_OK)
                {
                    throw new Exception("DetectCirclesInBuffer call to outputMediaBuffer.SetCurrentLength failed. Err=" + hr.ToString());
                }
            }
            finally
            {
                // we MUST unlock
                if(destIs2D == false) TantaWMFUtils.UnLockIMFMediaBuffer(outputMediaBuffer);
                else TantaWMFUtils.UnLockIMF2DBuffer((outputMediaBuffer as IMF2DBuffer));
           }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Detect circles ARGB formatted image and mark them. We use an EmguCV Mat()
        /// object for this. 
        /// </summary>
        /// <param name="pDest">Pointer to the destination buffer.</param>
        /// <param name="lDestStride">Stride of the destination buffer, in bytes.</param>
        /// <param name="dwWidthInPixels">Frame width in pixels.</param>
        /// <param name="dwHeightInPixels">Frame height, in pixels.</param>
        private unsafe void DetectCirclesInImageOfTypeRGB32(
            IntPtr pDest,
            int lDestStride,
            int dwWidthInPixels,
            int dwHeightInPixels
            )
        {

            // Although the actual data is down in unmanaged memory
            // we do not need to use "unsafe" access to get at it. 
            // The new BitMap() call does this for us. This is probably
            // only useful in this sort of rare circumstance. Normally
            // you have to copy it about. 

            // Get a bitmap wrapper around the video data.
            using (Bitmap bitmapFrame = new Bitmap(m_imageWidthInPixels, m_imageHeightInPixels, m_lStrideIfContiguous, PixelFormat.Format32bppRgb, pDest))
            {
                // get a graphics object and Mat() object
                using (Graphics graphicsObj = Graphics.FromImage(bitmapFrame))
                using (Mat bitmapAsMat = new Mat(bitmapFrame.Height, bitmapFrame.Width, DepthType.Cv8U, 3))
                {
                    // populate the Mat() object
                    bitmapFrame.ToMat(bitmapAsMat);
                    // now find the circles. Note it is the EmguCV Hough Circle detection step that is 
                    // taking all the CPU in the call below. It would be fairly trivial to only detect
                    // circles at some user settable rate per second and permit the remainder of the 
                    // frames to go unprocessed.
                    CircleF[] circles = FindCircles(bitmapAsMat);

                    // lets draw some crosses on the center of each circle of a specified color
                    foreach (CircleF circleCoord in circles)
                    {
                        // yes, this is the right way around
                        int row = Convert.ToInt32(circleCoord.Center.Y);
                        int col = Convert.ToInt32(circleCoord.Center.X);

                        // get the pixel values. Note that GetValues is an extension method on Mat() See MatExtension.cs
                        byte[] pixelValue = bitmapAsMat.GetValues(row, col);

                        // test the color is red, if not try next circle. Note we are in a transform here
                        // and NOT in the form thread so we CANNOT be accessing the screen here - very bad things
                        // will happen if you do that. However since this transform is created with a "new MFTDetectCircles_Sync()" 
                        // call it would be trivial to pass in information at creation time regarding which color 
                        // of circles should be detected
                        if (IsBGRPixelInRange(pixelValue, RED_RANGE_LOW, RED_RANGE_HIGH) == false) continue;
                   //    if (IsBGRPixelInRange(pixelValue, GREEN_RANGE_LOW, GREEN_RANGE_HIGH) == false) continue;
                   //    if (IsBGRPixelInRange(pixelValue, BLUE_RANGE_LOW, BLUE_RANGE_HIGH) == false) continue;

                        // draw the cross
                        DrawCrossOnPoint(graphicsObj, new Point(Convert.ToInt32(circleCoord.Center.X), Convert.ToInt32(circleCoord.Center.Y)), CENTROID_CROSS_BAR_LEN, blackPen);
                    }

                    // I have commented it out here, but you can also draw text on the bitmap (and hence output frame)
                    // using the technique below. All of the C# Graphics object tools are available :-)
                    // The strings to display.
                    //          string sString1 = "Hello!";
                    //          string sString2 = m_FrameCount.ToString();

                    //using (Graphics g = Graphics.FromImage(bitmapFrame))
                    //{
                    //    float sLeft;
                    //    float sTop;
                    //    SizeF d;

                    //    // String1 goes right in the middle of the video using the overlay font created earlier
                    //    d = g.MeasureString(sString1, m_fontOverlay);

                    //    sLeft = (m_imageWidthInPixels - d.Width) / 2.0f;
                    //    sTop = (m_imageHeightInPixels - d.Height) / 2.0f;

                    //    g.DrawString(sString1, m_fontOverlay, System.Drawing.Brushes.Red, sLeft, sTop, StringFormat.GenericTypographic);

                    //    // Add a frame number in the bottom right using the transparent font created earlier
                    //    d = g.MeasureString(sString2, m_transparentFont);

                    //    sLeft = (m_imageWidthInPixels - d.Width) - 10.0f;
                    //    sTop = (m_imageHeightInPixels - d.Height) - 10.0f;

                    //    g.DrawString(sString2, m_transparentFont, m_transparentBrush, sLeft, sTop, StringFormat.GenericTypographic);

                    //}

                } // bottom of using (Graphics graphicsObj
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
        /// <param name="graphicsObj">the grapics object to draw on</param>
        /// <param name="armLength">the lenght of the arm in the cross</param>
        /// <param name="centerPoint">the centerpoint of the cross</param>
        /// <param name="colorOfCross">the color of the cross</param>
        public void DrawCrossOnPoint(Graphics graphicsObj, Point centerPoint, int armLength, Pen colorOfCross)
        {
            if (graphicsObj == null) return;
            if (centerPoint == null) return;
            if (armLength <= 0) return;

            // apparently we do not need bounds checking here. The call to CvInvoke.Line does it
            Point horizStartPoint = new Point(centerPoint.X - armLength, centerPoint.Y);
            Point horizEndPoint = new Point(centerPoint.X + armLength, centerPoint.Y);
            Point vertStartPoint = new Point(centerPoint.X, centerPoint.Y - armLength);
            Point vertEndPoint = new Point(centerPoint.X, centerPoint.Y + armLength);

            // draw the horizontal line
            graphicsObj.DrawLine(colorOfCross, horizStartPoint, horizEndPoint);
            // draw the vertical line
            graphicsObj.DrawLine(colorOfCross, vertStartPoint, vertEndPoint);
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
        #endregion

    }

}
