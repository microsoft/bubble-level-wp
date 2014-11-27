BubbleLevel Silverlight Example
===============================

BubbleLevel Silverlight is a simple WP7 Silverlight application that uses
accelerometer sensor information to calculate the inclination of the device
and presents this as a traditional Bubble level. The application provides a
calibration feature to handle any possible errors in accelerometer readings.

The application is rewrite of the QtBubbleLevel application written in Qt for
Symbian and Maemo devices.


PREREQUISITIES
-------------------------------------------------------------------------------

- C# basics
- Development environment 'Microsoft Visual Studio 2010 Express for Windows
  Phone'


KNOWN ISSUES
-------------------------------------------------------------------------------

None.
  
  
BUILD & INSTALLATION INSTRUCTIONS
-------------------------------------------------------------------------------

**Preparations**

Make sure you have the following installed:
 * Windows 7
 * Microsoft Visual Studio 2010 Express for Windows Phone
 * The Windows Phone Software Development Kit (SDK) 7.1
   http://go.microsoft.com/?linkid=9772716

**Build on Microsoft Visual Studio**

Please refer to:
http://msdn.microsoft.com/en-us/library/ff928362.aspx


**Deploy to Windows Phone 7**

Please refer to:
http://msdn.microsoft.com/en-us/library/gg588378.aspx

    
RUNNING THE APPLICATION
-------------------------------------------------------------------------------

Launch Bubble Level on your device. The bubble in the glass tube will show 
the level of the device's x-axis relative to gravity. When the device is 
tilted to either direction 20 degrees or more, the bubble will be at the 
corresponding end of the tube.

The accelerometer sensor of the device can be calibrated by pressing the tool
icon beside the QtBubbleLevel sign, placing the device on a level surface, and 
tapping the Calibrate button. The calibration data will be saved into the
device's memory and it will be applied on the next startup of the application.

	
ACCELEROMETER
-------------------------------------------------------------------------------
The application heavily bases on accelerometer sensor. Please follow the link 
below for instruction about using accelerometer in applcations. Accelerometer can 
be also tested now on the Windows Phone Emulator.

http://msdn.microsoft.com/en-us/library/ff431810.aspx


COMPATIBILITY
-------------------------------------------------------------------------------

- Windows Phone 7

Tested on: 
- Nokia Lumia 800
	
Developed with:
- Microsoft Visual Studio 2010 Express for Windows Phone
	

LICENCE
-------------------------------------------------------------------------------
You can find license details in Licence.txt file provided with this project or
online at
https://github.com/Microsoft/bubble-level-wp/blob/master/License.txt


CHANGE HISTORY
-------------------------------------------------------------------------------

1.1 Code level improvements
1.0 First version
