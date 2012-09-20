// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Resources;

namespace Microsoft.Phone.Controls
{
    /// <summary>
    /// Provides
    /// <see cref="T:Microsoft.Phone.Controls.ITransition"/>s
    /// for transition families and modes.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    internal static class Transitions
    {
        /// <summary>
        /// The cached XAML read from the Storyboard resources.
        /// </summary>
        private static Dictionary<string, string> _storyboardXamlCache;

        /// <summary>
        /// Creates a
        /// <see cref="T:Microsoft.Phone.Controls.ITransition"/>
        /// for a transition family, transition mode, and
        /// <see cref="T:System.Windows.UIElement"/>.
        /// </summary>
        /// <typeparam name="T">The type of the transition mode.</typeparam>
        /// <param name="element">The <see cref="T:System.Windows.UIElement"/>.</param>
        /// <param name="name">The transition family.</param>
        /// <param name="mode">The transition mode.</param>
        /// <returns>The <see cref="T:Microsoft.Phone.Controls.ITransition"/>.</returns>
        private static ITransition GetEnumStoryboard<T>(UIElement element, string name, T mode)
        {
            string key = name + Enum.GetName(typeof(T), mode);
            Storyboard storyboard = GetStoryboard(key);
            if (storyboard == null)
            {
                return null;
            }
            Storyboard.SetTarget(storyboard, element);
            return new Transition(element, storyboard);
        }

        /// <summary>
        /// Creates a
        /// <see cref="T:System.Windows.Media.Storyboard"/>
        /// for a particular transition family and transition mode.
        /// </summary>
        /// <param name="name">The transition family and transition mode.</param>
        /// <returns>The <see cref="T:System.Windows.Media.Storyboard"/>.</returns>
        private static Storyboard GetStoryboard(string name)
        {
            if (_storyboardXamlCache == null)
            {
                _storyboardXamlCache = new Dictionary<string, string>();
            }
            string xaml = null;
            if (_storyboardXamlCache.ContainsKey(name))
            {
                xaml = _storyboardXamlCache[name];
            }
            else
            {
                string path = "/Microsoft.Phone.Controls.Toolkit;component/Transitions/Storyboards/" + name + ".xaml";
                Uri uri = new Uri(path, UriKind.Relative);
                StreamResourceInfo streamResourceInfo = Application.GetResourceStream(uri);
                using (StreamReader streamReader = new StreamReader(streamResourceInfo.Stream))
                {
                    xaml = streamReader.ReadToEnd();
                    _storyboardXamlCache[name] = xaml;
                }
            }
            return XamlReader.Load(xaml) as Storyboard;
        }

        /// <summary>
        /// Creates an
        /// <see cref="T:Microsoft.Phone.Controls.ITransition"/>
        /// for a
        /// <see cref="T:System.Windows.UIElement"/>
        /// for the roll transition.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Windows.UIElement"/>.</param>
        /// <returns>The <see cref="T:Microsoft.Phone.Controls.ITransition"/>.</returns>
        public static ITransition Roll(UIElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            Storyboard storyboard = GetStoryboard("Roll");
            Storyboard.SetTarget(storyboard, element);
            element.Projection = new PlaneProjection { CenterOfRotationX = 0.5, CenterOfRotationY = 0.5 };
            return new Transition(element, storyboard);
        }

        /// <summary>
        /// Creates an
        /// <see cref="T:Microsoft.Phone.Controls.ITransition"/>
        /// for a
        /// <see cref="T:System.Windows.UIElement"/>
        /// for the rotate transition family.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Windows.UIElement"/>.</param>
        /// <param name="rotateTransitionMode">The transition mode.</param>
        /// <returns>The <see cref="T:Microsoft.Phone.Controls.ITransition"/>.</returns>
        public static ITransition Rotate(UIElement element, RotateTransitionMode rotateTransitionMode)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (!Enum.IsDefined(typeof(RotateTransitionMode), rotateTransitionMode))
            {
                throw new ArgumentOutOfRangeException("rotateTransitionMode");
            }
            element.Projection = new PlaneProjection { CenterOfRotationX = 0.5, CenterOfRotationY = 0.5 };
            return GetEnumStoryboard<RotateTransitionMode>(element, "Rotate", rotateTransitionMode);
        }

        /// <summary>
        /// Creates an
        /// <see cref="T:Microsoft.Phone.Controls.ITransition"/>
        /// for a
        /// <see cref="T:System.Windows.UIElement"/>
        /// for the slide transition family.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Windows.UIElement"/>.</param>
        /// <param name="slideTransitionMode">The transition mode.</param>
        /// <returns>The <see cref="T:Microsoft.Phone.Controls.ITransition"/>.</returns>
        public static ITransition Slide(UIElement element, SlideTransitionMode slideTransitionMode)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (!Enum.IsDefined(typeof(SlideTransitionMode), slideTransitionMode))
            {
                throw new ArgumentOutOfRangeException("slideTransitionMode");
            }
            element.RenderTransform = new TranslateTransform();
            return GetEnumStoryboard<SlideTransitionMode>(element, string.Empty, slideTransitionMode);
        }

        /// <summary>
        /// Creates an
        /// <see cref="T:Microsoft.Phone.Controls.ITransition"/>
        /// for a
        /// <see cref="T:System.Windows.UIElement"/>
        /// for the swivel transition family.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Windows.UIElement"/>.</param>
        /// <param name="swivelTransitionMode">The transition mode.</param>
        /// <returns>The <see cref="T:Microsoft.Phone.Controls.ITransition"/>.</returns>
        public static ITransition Swivel(UIElement element, SwivelTransitionMode swivelTransitionMode)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (!Enum.IsDefined(typeof(SwivelTransitionMode), swivelTransitionMode))
            {
                throw new ArgumentOutOfRangeException("swivelTransitionMode");
            }
            element.Projection = new PlaneProjection();
            return GetEnumStoryboard<SwivelTransitionMode>(element, "Swivel", swivelTransitionMode);
        }

        /// <summary>
        /// Creates an
        /// <see cref="T:Microsoft.Phone.Controls.ITransition"/>
        /// for a
        /// <see cref="T:System.Windows.UIElement"/>
        /// for the turnstile transition family.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Windows.UIElement"/>.</param>
        /// <param name="turnstileTransitionMode">The transition mode.</param>
        /// <returns>The <see cref="T:Microsoft.Phone.Controls.ITransition"/>.</returns>
        public static ITransition Turnstile(UIElement element, TurnstileTransitionMode turnstileTransitionMode)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (!Enum.IsDefined(typeof(TurnstileTransitionMode), turnstileTransitionMode))
            {
                throw new ArgumentOutOfRangeException("turnstileTransitionMode");
            }
            element.Projection = new PlaneProjection { CenterOfRotationX = 0 };
            return GetEnumStoryboard<TurnstileTransitionMode>(element, "Turnstile", turnstileTransitionMode);
        }
    }
}