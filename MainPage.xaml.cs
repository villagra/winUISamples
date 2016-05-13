using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CompositionExamples
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        SpriteVisual _visual;
        Compositor _compositor;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            btn1.Tapped += Btn1_Tapped;
            btn2.Tapped += Btn2_Tapped;

            //Colorbloomanimation
            colorBloomHelper = new ColorBloomTransitionHelper(containerColorBloom);
            colorBloomHelper.ColorBloomTransitionCompleted += ColorBloomTransitionCompleted;
        }

        ColorBloomTransitionHelper colorBloomHelper;
        private void Btn2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var animation = _compositor.CreateVector2KeyFrameAnimation();
            var easing = _compositor.CreateLinearEasingFunction();
            
            animation.InsertKeyFrame(1.0f, new Vector2((float)this.ActualWidth, 10), easing);
            animation.Duration = TimeSpan.FromMilliseconds(150);

            _visual.CenterPoint = new Vector3(_visual.Size.X / 2.0f, _visual.Size.Y / 2.0f, 0);
            _visual.StartAnimation(nameof(_visual.Size), animation);            
        }

        private void ColorBloomTransitionCompleted(object sender, EventArgs e)
        {
            pnlContainer.Background = new SolidColorBrush(Colors.Red);
        }

        private void Btn1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SpriteVisual sourceSprite = imgOriginal.SpriteVisual;
            GeneralTransform coordinate = imgOriginal.TransformToVisual(this);
            Point position = coordinate.TransformPoint(new Point(0, 0));


            var initialBounds = new Windows.Foundation.Rect()  // maps to a rectangle the size of the header 
            {
                Width = sourceSprite.Size.X,
                Height = sourceSprite.Size.Y,
                X = position.X,
                Y = position.Y
            };

            var finalBounds = new Windows.Foundation.Rect()  // maps to a rectangle the size of the header 
            {
                Width = Window.Current.Bounds.Width + 500,
                Height = Window.Current.Bounds.Height + 500,
                X = 15,
                Y = 15
            };
            //var finalBounds = Window.Current.Bounds;  // maps to the bounds of the current window 
            colorBloomHelper.Start(Colors.Red, initialBounds, finalBounds);

            var _scopeBatch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            //copy the origin image


            var imageCopyVisual = _compositor.CreateSpriteVisual();
            imageCopyVisual.Brush = imgOriginal.SurfaceBrush;
            imageCopyVisual.Size = sourceSprite.Size; //new Vector2(500, 500);            
            imageCopyVisual.Offset = new Vector3((float)position.X, (float)position.Y, 0);

            ElementCompositionPreview.SetElementChildVisual(imageContainer, imageCopyVisual);


            var easing = _compositor.CreateLinearEasingFunction();
            /* RED BLOCK ANIMATION
            var animation = _compositor.CreateVector2KeyFrameAnimation();            
            //animation.InsertKeyFrame(0.0f, new Vector2(150, 150));
            var rootVisual = ElementCompositionPreview.GetElementVisual(pnlRoot);
           
            animation.InsertKeyFrame(1.0f, new Vector2((float)this.ActualWidth, (float)this.ActualHeight), easing);
            animation.Duration = TimeSpan.FromMilliseconds(150);

            _visual.CenterPoint = new Vector3(_visual.Size.X / 2.0f, _visual.Size.Y / 2.0f, 0);
            _visual.StartAnimation(nameof(_visual.Size), animation);
            */

            //imageSizeAnimageion            
            var iamgeSizeanimation = _compositor.CreateVector2KeyFrameAnimation();
            iamgeSizeanimation.InsertKeyFrame(1.0f, new Vector2(400, 300), easing);
            iamgeSizeanimation.Duration = TimeSpan.FromMilliseconds(250);
            imageCopyVisual.StartAnimation(nameof(imageCopyVisual.Size), iamgeSizeanimation);

            var offsetAnimation = _compositor.CreateVector3KeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1.0f, new Vector3(25,25,0), easing);
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(250);
            imageCopyVisual.StartAnimation(nameof(imageCopyVisual.Offset), offsetAnimation);

            _scopeBatch.Completed += ScopeBatch_Completed;
            _scopeBatch.End();

        }

        private void ScopeBatch_Completed(object sender, CompositionBatchCompletedEventArgs args)
        {
            var x = "completed";
            //gridYellowVisual.Opacity = 1f;
        }

        Visual gridYellowVisual;
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _visual = _compositor.CreateSpriteVisual();

            _visual.Size = new Vector2((float)this.ActualWidth, 10);
            //_visual.Offset = new Vector3(0, 0, 0);
            _visual.Brush = _compositor.CreateColorBrush(Colors.Red);

            ElementCompositionPreview.SetElementChildVisual(container, _visual);

            //gridYellowVisual = ElementCompositionPreview.GetElementVisual(gridYellow);
            //gridYellowVisual.Opacity = 0f;
        }
    }
}
