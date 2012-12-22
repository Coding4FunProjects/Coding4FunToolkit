// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

// This is from the Silverlight Toolkit Nov 2011

using System.Collections.Generic;
using System.Linq;

#if WINDOWS_STORE

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

#elif WINDOWS_PHONE

using System.Windows;
using System.Windows.Media;

#endif

namespace Coding4Fun.Toolkit.Controls
{
    /// <summary>
    /// A static class providing methods for working with the visual tree.  
    /// </summary>
    public static class VisualTreeExtensions
    {
        #region GetVisualChildren(...)
        /// <summary>
        /// Retrieves all the visual children of a framework element.
        /// </summary>
        /// <param name="parent">The parent framework element.</param>
        /// <returns>The visual children of the framework element.</returns>
        public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject parent)
        {
            var childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (var counter = 0; counter < childCount; counter++)
            {
                yield return VisualTreeHelper.GetChild(parent, counter);
            }
        }
        #endregion

        #region GetLogicalChildrenBreadthFirst(...)
        /// <summary>
        /// Retrieves all the logical children of a framework element using a 
        /// breadth-first search.  A visual element is assumed to be a logical 
        /// child of another visual element if they are in the same namescope.
        /// For performance reasons this method manually manages the queue 
        /// instead of using recursion.
        /// </summary>
        /// <param name="parent">The parent framework element.</param>
        /// <returns>The logical children of the framework element.</returns>
        public static IEnumerable<FrameworkElement> GetLogicalChildrenBreadthFirst(this FrameworkElement parent)
        {
            var queue = new Queue<FrameworkElement>(parent.GetVisualChildren().OfType<FrameworkElement>());

            while (queue.Count > 0)
            {
                var element = queue.Dequeue();

                yield return element;

                foreach (var visualChild in element.GetVisualChildren().OfType<FrameworkElement>())
                {
                    queue.Enqueue(visualChild);
                }
            }
        }
        #endregion

		/// <summary>
		/// Gets the ancestors of the element, up to the root.
		/// </summary>
		/// <param name="node">The element to start from.</param>
		/// <returns>An enumerator of the ancestors.</returns>
		public static IEnumerable<FrameworkElement> GetVisualAncestors(this FrameworkElement node)
		{
			FrameworkElement parent = node.GetVisualParent();
			while (parent != null)
			{
				yield return parent;
				parent = parent.GetVisualParent();
			}
		}

		/// <summary>
		/// Gets the visual parent of the element.
		/// </summary>
		/// <param name="node">The element to check.</param>
		/// <returns>The visual parent.</returns>
		public static FrameworkElement GetVisualParent(this FrameworkElement node)
		{
			return VisualTreeHelper.GetParent(node) as FrameworkElement;
		}
    }
}