# Prism
A suite of six image recognition test solutions which demonstrate various aspects of EmguCV library (a C# OpenCV Interface).

Each Prism application is coded in C# as a Windows Form and is embedded in its own Visual Studio solution which is both complete and standalone. The Prism project page at at [http://www.OfItselfSo.com/Prism](http://www.OfItselfSo.com/Prism) contains a more detailed discussion of each application. The Prism code is released as open source under the MIT License.

## The Prism Applications

- **Prism1**
    - An application to open a static image file and detect circles and squares of a solid color. This version uses Mat() objects as the image carrier. 
- **Prism2**
    - An application to open a static image file and detect circles and squares of a solid color. This version is the same as Prism1 but uses the older, now deprecated, Image<TColor, TDepth>() objects as the image carrier. 
- **Prism3**
    - An application which conducts speed tests on various pixel access methods for Mat() and Image<>() objects. 
- **Prism4**
    - An application which captures video from a webcam using the VideoCapture() object and displays it on the screen. 
- **Prism5**
    - An application which captures video from a webcam using the VideoCapture() object, displays it on the screen and can (optionally) write the webcam stream to disk as an mp4. This code also detects solid circles of various colors in the webcam stream. 
- **Prism6**
    - A full C# Windows Media Foundation application which captures video from a webcam using a WMF pipeline, displays it on the screen and, optionally, writes the stream to disk as an mp4. This code integrates EmguCV into a Windows Media Foundation transform in order to detect solid red circles in the webcam stream. 			

Please note that the primary purpose of these test applications was to get some experience using EmguCV. As such none of the Prism apps attempt to tune the image recognition algorythms. That code has pretty much just been clipped verbatim from the readily available sample sources. It is simply assumed that the recognition aspect of the applications could be made to work better when that is required. All the current Prism applications really do is recognise clean circles and squares of pure primary red, blue and green colors. 

