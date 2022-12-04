
####
#### The Prism6 Project
####

This project displays the video stream from a video device attached
to the PC (a webcam) on the screen. It also, optionally, writes this
video stream to a file - thus recording it. 

This app creates a WMF Media Session, Pipeline Media Source
and Media Sink. The video device is the media source, and the 
Enhanced Video Renderer is the media sink. 

A Synchronous Transform is inserted into the topology between
the source and sink. Normally this transform just passes the 
input sample to the output. If a sink writer has been configured
on it then it will also present the sample it is processing to
the sink writer which then writes it to disk - thus recording it.

A second transform is injected into the pipeline just after the 
media source in order to detect solid red circles in the frames.
These circles, once detected, are marked with a black cross. The 
black cross marking will be propagated through the pipeline and 
also saved out to the disk (if enabled).

The intent is to demonstrate the end-to-end operation of a pipeline
with a transform in the middle acting as a "sample grabber" which
presents pipeline based media data to non pipeline objects. The 
circle detection transform uses EmguCV for the detection processing
and that transform serves as a demonstration of a way in which 
EmguCV might be integrated into an WMF pipeline.

