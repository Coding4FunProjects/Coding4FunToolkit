// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Windows;

namespace Microsoft.Phone.Controls
{
    /// <summary>
    /// Provides roll <see cref="T:Microsoft.Phone.Controls.ITransition"/>s.
    /// </summary>
	internal class RollTransition : TransitionElement
    {
        /// <summary>
        /// Creates a new
        /// <see cref="T:Microsoft.Phone.Controls.ITransition"/>
        /// for a
        /// <see cref="T:System.Windows.UIElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Windows.UIElement"/>.</param>
        /// <returns>The <see cref="T:Microsoft.Phone.Controls.ITransition"/>.</returns>
        public override ITransition GetTransition(UIElement element)
        {
            return Transitions.Roll(element);
        }
    }
}