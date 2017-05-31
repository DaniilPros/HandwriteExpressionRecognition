using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.UI.Input.Inking;
using Windows.UI.Text.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HWRE.Core;
using HWRE.UWP.Helpers;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace HWRE.UWP.Controls
{
    public sealed partial class RecognizerControl : UserControl
    {
        private readonly InkRecognizerContainer _inkRecognizerContainer = null;
        private readonly IReadOnlyList<InkRecognizer> _recoView = null;
        private Language _previousInputLanguage = null;
        private readonly CoreTextServicesManager _textServiceManager = null;
        public RecognizerControl()
        {
            this.InitializeComponent();
            const double penSize = 4;
            // Initialize drawing attributes. These are used in inking mode.
            var drawingAttributes = new InkDrawingAttributes
            {
                Color = Windows.UI.Colors.Red,
                Size = new Windows.Foundation.Size(penSize, penSize),
                IgnorePressure = false,
                FitToCurve = true
            };


            // Show the available recognizers
            _inkRecognizerContainer = new InkRecognizerContainer();
            _recoView = _inkRecognizerContainer.GetRecognizers();
            // Set the text services so we can query when language changes
            _textServiceManager = CoreTextServicesManager.GetForCurrentView();
            _textServiceManager.InputLanguageChanged += TextServiceManager_InputLanguageChanged;

            SetDefaultRecognizerByCurrentInputMethodLanguageTag();

            // Initialize the InkCanvas
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            InkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen | Windows.UI.Core.CoreInputDeviceTypes.Touch;
        }

        async void OnRecognizeAsync(object sender, RoutedEventArgs e)
        {
            var currentStrokes = InkCanvas.InkPresenter.StrokeContainer.GetStrokes();
            if (currentStrokes.Count > 0)
            {
                var recognitionResults = await _inkRecognizerContainer.RecognizeAsync(InkCanvas.InkPresenter.StrokeContainer, InkRecognitionTarget.All);
                var str = string.Empty;
                if (recognitionResults.Count > 0)
                {
                    try
                    {
                        str = recognitionResults.Aggregate(string.Empty, (current, r) => current + (" " + r.GetTextCandidates()[0]));
                        var expressionParser = new ExpressionParser(str);
                        ResulTextBlock.Text = expressionParser.Value;
                    }
                    catch (Exception exception)
                    {
                        ResulTextBlock.Text = "Error in " + str;
                    }


                }
                else
                {

                }
            }
            else
            {

            }
        }

        void OnClear(object sender, RoutedEventArgs e)
        {
            InkCanvas.InkPresenter.StrokeContainer.Clear();
        }

        bool SetRecognizerByName(string recognizerName)
        {
            bool recognizerFound = false;

            foreach (InkRecognizer reco in _recoView)
            {
                if (recognizerName == reco.Name)
                {
                    _inkRecognizerContainer.SetDefaultRecognizer(reco);
                    recognizerFound = true;
                    break;
                }
            }

            if (!recognizerFound)
            {

            }

            return recognizerFound;
        }

        private void TextServiceManager_InputLanguageChanged(CoreTextServicesManager sender, object args)
        {
            SetDefaultRecognizerByCurrentInputMethodLanguageTag();
        }

        private void SetDefaultRecognizerByCurrentInputMethodLanguageTag()
        {
            // Query recognizer name based on current input method language tag (bcp47 tag)
            Language currentInputLanguage = _textServiceManager.InputLanguage;

            if (currentInputLanguage != _previousInputLanguage)
            {
                // try query with the full BCP47 name
                string recognizerName = RecognizerHelper.LanguageTagToRecognizerName(currentInputLanguage.LanguageTag);

                if (recognizerName != string.Empty)
                {
                    for (int index = 0; index < _recoView.Count; index++)
                    {
                        if (_recoView[index].Name == recognizerName)
                        {
                            _inkRecognizerContainer.SetDefaultRecognizer(_recoView[index]);
                            _previousInputLanguage = currentInputLanguage;
                            break;
                        }
                    }
                }
            }
        }

        private void RecognizerControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            InkCanvas.Width = Width;
            InkCanvas.Height = Height - 100;
        }
    }
}
