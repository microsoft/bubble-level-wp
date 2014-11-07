/**
 *  Copyright © 2011 Digia Plc
 *  Copyright © 2011-2014 Microsoft Mobile
 *
 *  All rights reserved.
 *
 *  Nokia and Nokia Connecting People are registered trademarks of
 *  Nokia Corporation.
 *  Java and all Java-based marks are trademarks or registered
 *  trademarks of
 *  Sun Microsystems, Inc. Other product and company names
 *  mentioned herein may be
 *  trademarks or trade names of their respective owners.
 *
 *
 *  Subject to the conditions below, you may, without charge:
 *
 *  ·  Use, copy, modify and/or merge copies of this software and
 *     associated documentation files (the "Software")
 *
 *  ·  Publish, distribute, sub-licence and/or sell new software
 *     derived from or incorporating the Software.
 *
 *
 *  This file, unmodified, shall be included with all copies or
 *  substantial portions
 *  of the Software that are distributed in source code form.
 *
 *  The Software cannot constitute the primary value of any new
 *  software derived
 *  from or incorporating the Software.
 *
 *  Any person dealing with the Software shall not misrepresent
 *  the source of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY
 *  KIND, EXPRESS OR IMPLIED,
 *  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 *  MERCHANTABILITY, FITNESS FOR A
 *  PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT
 *  HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 *  WHETHER IN AN ACTION
 *  OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 *  CONNECTION WITH THE
 *  SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Input;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;

namespace BubbleLevel
{
    /// <summary>
    /// Handles the application logic for accelerometer sensor reading, retrieveing / storing
    /// of calibration factor, calibrate button press handler and starting the sign flip
    /// animations.
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {
        protected Accelerometer m_Accelometer = new Accelerometer();
        protected double m_Angle = 0;
        protected double m_CalibrationFactor = 0;
        protected IsolatedStorageSettings m_Settings = IsolatedStorageSettings.ApplicationSettings;
        protected const string CALIBRATION_SETTING_KEY = "CalibrationFactor";

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set up the handler for accelerometer sensor data.
            m_Accelometer.ReadingChanged += new EventHandler<AccelerometerReadingEventArgs>(AccelerometerReadingChanged);

        }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Read the calibration factor from isolated storage settings
            if (m_Settings.Contains(CALIBRATION_SETTING_KEY))
                m_CalibrationFactor = (double)m_Settings[CALIBRATION_SETTING_KEY];

            try
            {
                // Start the accelerometer sensor.
                m_Accelometer.Start();
            }
            catch (AccelerometerFailedException exception)
            {
                // Normally you wouldn't like to show the actual exception to the user
                MessageBox.Show("Failed to start the accelerometer sensor: " + exception.ToString());
            }
        }


        /// <summary>
        /// Flips the sign over.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FlipSign(object sender, MouseButtonEventArgs e)
        {
            if (SignFrontProjection.RotationX > 0)
            {
                ResetSign(this, null);
            }
            else
            {
                SignFlipStoryBoard.Begin();
            }
        }

        /// <summary>
        /// Flips the sign so that the front side is visible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ResetSign(object sender, MouseButtonEventArgs e)
        {
            SignResetStoryBoard.Begin();
        }


        /// <summary>
        /// Handler for the accelerometer reading changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains the accelerometer reading value.</param>
        public void AccelerometerReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            // Dispatch the accelerometer reading to UI thread
            Deployment.Current.Dispatcher.BeginInvoke(() => UpdateBubble(e));
        }


        /// <summary>
        /// Updates the position of the bubble in the glass tube.
        /// </summary>
        /// <param name="e">Contains the accelerometer reading value.</param>
        protected void UpdateBubble(AccelerometerReadingEventArgs e)
        {
            const double RADIANS_TO_DEGREE = 57.2957795;
            double divider = Math.Sqrt(e.X * e.X + e.Y * e.Y + e.Z * e.Z);

            // Calculating the angle + using low pass factor 20 %.
            // Values from all three accelerometers are used to get more precise reading on y-axis.
            m_Angle += (Math.Acos(e.Y / divider) * RADIANS_TO_DEGREE - 90 - m_Angle) * 0.2;

            double angle;

            // Depending on the orientation, invert the accelerometer value
            if (Orientation == PageOrientation.LandscapeLeft)
            {
                angle = -m_Angle + m_CalibrationFactor;
            }
            else
            {
                angle = m_Angle - m_CalibrationFactor;
            }

            const double MAX_ANGLE = 20.0;

            // Restrict the angle value to the range -20 and 20 degrees.
            if (angle > MAX_ANGLE)
            {
                angle = MAX_ANGLE;
            }
            else if (angle < -MAX_ANGLE)
            {
                angle = -MAX_ANGLE;
            }

            // Set the bubble position.
            BubbleTransform.X = angle / MAX_ANGLE * (Reflection.Width / 2 - Bubble.Width / 2);
        }


        /// <summary>
        /// Handler for the Calibrate button click.
        /// </summary>
        private void CalibrateButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the current angle as reference angle
            m_CalibrationFactor = m_Angle;

            // Store the calibration factor to the isolated storage.
            // If the setting already exists, just update the value. If it does not exist yet, it will be created
            m_Settings[CALIBRATION_SETTING_KEY] = m_CalibrationFactor;
        }
    }
}