// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Windows;

namespace Microsoft.Phone.Controls
{
    /// <summary>
    /// Provides turnstile <see cref="T:Microsoft.Phone.Controls.ITransition"/>s.
    /// </summary>
	internal class TurnstileTransition : TransitionElement
    {
        /// <summary>
        /// The
        /// <see cref="T:System.Windows.DependencyProperty"/>
        /// for the
        /// <see cref="T:Microsoft.Phone.Controls.TurnstileTransitionMode"/>.
        /// </summary>
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(TurnstileTransitionMode), typeof(TurnstileTransition), null);

        /// <summary>
        /// The <see cref="T:Microsoft.Phone.Controls.TurnstileTransitionMode"/>.
        /// </summary>
        public TurnstileTransitionMode Mode
        {
            get
            {
                return (TurnstileTransitionMode)GetValue(ModeProperty);
            }
            set
            {
                SetValue(ModeProperty, value);
            }
        }

        /// <summary>
        /// Creates a new
        /// <see cref="T:Microsoft.Phone.Controls.ITransition"/>
        /// for a
        /// <see cref="T:System.Windows.UIElement"/>.
        /// Saves and clears the existing
        /// <see cref="F:System.Windows.UIElement.ProjectionProperty"/>
        /// value before the start of the transition, then restores it after it is stopped or completed.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Windows.UIElement"/>.</param>
        /// <returns>The <see cref="T:Microsoft.Phone.Controls.ITransition"/>.</returns>
        public override ITransition GetTransition(UIElement element)
        {
            return Transitions.Turnstile(element, Mode);
        }
    }
}