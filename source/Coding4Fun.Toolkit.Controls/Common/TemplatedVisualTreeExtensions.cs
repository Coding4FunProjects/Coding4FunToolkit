// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

// This is from the Silverlight Toolkit Nov 2011

using System.Collections.Generic;
using System.Linq;

#if WINDOWS_STORE || WINDOWS_PHONE_APP

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#elif WINDOWS_PHONE

using System.Windows;
using System.Windows.Controls;

#endif

namespace Coding4Fun.Toolkit.Controls
{
	/// <summary>
	/// A static class providing methods for working with the visual tree using generics.  
	/// </summary>
	public static class TemplatedVisualTreeExtensions
	{

		#region GetFirstLogicalChildByType<T>(...)

		/// <summary>
		/// Retrieves the first logical child of a specified type using a 
		/// breadth-first search.  A visual element is assumed to be a logical 
		/// child of another visual element if they are in the same namescope.
		/// For performance reasons this method manually manages the queue 
		/// instead of using recursion.
		/// </summary>
		/// <param name="parent">The parent framework element.</param>
		/// <param name="applyTemplates">Specifies whether to apply templates on the traversed framework elements</param>
		/// <returns>The first logical child of the framework element of the specified type.</returns>
		public static T GetFirstLogicalChildByType<T>(this FrameworkElement parent, bool applyTemplates)
			where T : FrameworkElement
		{
			var queue = new Queue<FrameworkElement>();
			queue.Enqueue(parent);

			while (queue.Count > 0)
			{
				var element = queue.Dequeue();
				var elementAsControl = element as Control;

				if (applyTemplates && elementAsControl != null)
				{
					elementAsControl.ApplyTemplate();
				}

				if (element is T && element != parent)
				{
					return (T) element;
				}

				foreach (var visualChild in element.GetVisualChildren().OfType<FrameworkElement>())
				{
					queue.Enqueue(visualChild);
				}
			}

			return null;
		}

		#endregion

		#region GetLogicalChildrenByType<T>(...)

		/// <summary>
		/// Retrieves all the logical children of a specified type using a 
		/// breadth-first search.  A visual element is assumed to be a logical 
		/// child of another visual element if they are in the same namescope.
		/// For performance reasons this method manually manages the queue 
		/// instead of using recursion.
		/// </summary>
		/// <param name="parent">The parent framework element.</param>
		/// <param name="applyTemplates">Specifies whether to apply templates on the traversed framework elements</param>
		/// <returns>The logical children of the framework element of the specified type.</returns>
		public static IEnumerable<T> GetLogicalChildrenByType<T>(this FrameworkElement parent, bool applyTemplates)
			where T : FrameworkElement
		{
			if (applyTemplates && parent is Control)
			{
				((Control) parent).ApplyTemplate();
			}

			var queue = new Queue<FrameworkElement>(parent.GetVisualChildren().OfType<FrameworkElement>());

			while (queue.Count > 0)
			{
				var element = queue.Dequeue();

				if (applyTemplates && element is Control)
				{
					((Control) element).ApplyTemplate();
				}

				if (element is T)
				{
					yield return (T) element;
				}

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
		/// <param name="applyTemplates">Specifies whether to apply templates on the traversed framework elements</param>
		/// <returns>An enumerator of the ancestors.</returns>
		public static IEnumerable<T> GetVisualAncestorsByType<T>(this FrameworkElement node,
		                                                                        bool applyTemplates)
			where T : FrameworkElement
		{
			FrameworkElement parent = node.GetVisualParent();

			while (parent != null)
			{
				if (applyTemplates && parent is Control)
				{
					((Control)parent).ApplyTemplate();
				}

				if (parent is T)
				{
					yield return parent as T;
				}

				parent = parent.GetVisualParent();
			}
		}
	}
}