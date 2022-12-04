using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

/// +------------------------------------------------------------------------------------------------------------------------------+
/// ¦                                                   TERMS OF USE: StackOverflow                                                ¦
/// +------------------------------------------------------------------------------------------------------------------------------¦
/// ¦This code has pretty much been clipped straight from the 3 channel eng3ls answer at                                           ¦
/// ¦   https://stackoverflow.com/questions/32255440/how-can-i-get-and-set-pixel-values-of-an-emgucv-image-imageimitation            ¦
/// ¦                                                                                                                              ¦
/// ¦The license is whatever StackOverflow is using for that question/response. You should probably check it if this is a          ¦
/// ¦   concern for you.                                                                                                           ¦
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
    /// Dynamic extension helper class for EmguCV Image Objects. Allows you to
    /// get and set pixel values from a EmguCV Image object. It is the existence of 
    /// this extension that makes a call to GetValues() on a Image object work.
    ///  
    ///    byte[] pixelValue = inputImage.GetValues(row, col);
    ///    
    /// This code appears to be about a factor of 5 slower than just copy and
    /// pasting it directly inline where you need it. 
    /// 
    /// </summary>
    public static class ImageExtension
    {
        public static dynamic GetValues(this Image<Bgr, Byte> image, int row, int col)
        {
            byte[] pixelValue = new byte[3];
            pixelValue[0] = image.Data[row, col, 0];   
            pixelValue[1] = image.Data[row, col, 1];   
            pixelValue[2] = image.Data[row, col, 2];
            return pixelValue;
        }

 
    }
}
