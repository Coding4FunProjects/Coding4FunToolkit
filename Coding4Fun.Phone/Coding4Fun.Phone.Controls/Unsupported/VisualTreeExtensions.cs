// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Clarity.Phone.Extensions
{

    /// <summary>
    /// Provides useful extensions for working with the visual tree.
    /// </summary>
    /// <remarks>
    /// Since many of these extension methods are declared on types like
    /// DependencyObject high up in the class hierarchy, we've placed them in
    /// the Primitives namespace which is less likely to be imported for normal
    /// scenarios.
    /// </remarks>
    /// <QualityBand>Experimental</QualityBand>
    public  static class VisualTreeExtensions
    {
        /// <summary>
        /// Get the visual tree ancestors of an element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The visual tree ancestors of the element.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualAncestors(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return GetVisualAncestorsAndSelfIterator(element).Skip(1);
        }

        /// <summary>
        /// Get the visual tree ancestors of an element and the element itself.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The visual tree ancestors of an element and the element itself.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualAncestorsAndSelf(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return GetVisualAncestorsAndSelfIterator(element);
        }

        /// <summary>
        /// Get the visual tree ancestors of an element and the element itself.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The visual tree ancestors of an element and the element itself.
        /// </returns>
        private static IEnumerable<DependencyObject> GetVisualAncestorsAndSelfIterator(DependencyObject element)
        {
            Debug.Assert(element != null, "element should not be null!");

            for (DependencyObject obj = element;
                    obj != null;
                    obj = VisualTreeHelper.GetParent(obj))
            {
                yield return obj;
            }
        }

        /// <summary>
        /// Gets the visual children of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetVisualChildren<T>(this DependencyObject target)
            where T : DependencyObject
        {
            return GetVisualChildren(target).Where(child => child is T).Cast<T>();
        }

    	/// <summary>
    	/// Gets the visual children of type T.
    	/// </summary>
    	/// <typeparam name="T"></typeparam>
    	/// <param name="target"></param>
    	/// <param name="strict"></param>
    	/// <returns></returns>
    	public static IEnumerable<T> GetVisualChildren<T>(this DependencyObject target, bool strict)
            where T : DependencyObject
        {
            return GetVisualChildren(target, strict).Where(child => child is T).Cast<T>();
        }

        /// <summary>
        /// Gets the visual children.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="strict">Prevents the search from navigating the logical tree; eg. ContentControl.Content</param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject target, bool strict)
        {
            int count = VisualTreeHelper.GetChildrenCount(target);
            if (count == 0)
            {
                if (!strict && target is ContentControl)
                {
                    var child = ((ContentControl)target).Content as DependencyObject;
                    if (child != null)
                    {
                        yield return child;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    yield return VisualTreeHelper.GetChild(target, i);
                }
            }
            yield break;
        }

        /// <summary>
        /// Get the visual tree children of an element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The visual tree children of an element.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return GetVisualChildrenAndSelfIterator(element).Skip(1);
        }

        /// <summary>
        /// Get the visual tree children of an element and the element itself.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The visual tree children of an element and the element itself.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualChildrenAndSelf(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return GetVisualChildrenAndSelfIterator(element);
        }

        /// <summary>
        /// Get the visual tree children of an element and the element itself.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The visual tree children of an element and the element itself.
        /// </returns>
        private static IEnumerable<DependencyObject> GetVisualChildrenAndSelfIterator(this DependencyObject element)
        {
            Debug.Assert(element != null, "element should not be null!");

            yield return element;

            int count = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < count; i++)
            {
                yield return VisualTreeHelper.GetChild(element, i);
            }
        }

    	/// <summary>
        /// Get the visual tree descendants of an element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The visual tree descendants of an element.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualDescendants(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return GetVisualDescendantsAndSelfIterator(element).Skip(1);
        }

        /// <summary>
        /// Get the visual tree descendants of an element and the element
        /// itself.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The visual tree descendants of an element and the element itself.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualDescendantsAndSelf(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return GetVisualDescendantsAndSelfIterator(element);
        }

        /// <summary>
        /// Get the visual tree descendants of an element and the element
        /// itself.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The visual tree descendants of an element and the element itself.
        /// </returns>
        private static IEnumerable<DependencyObject> GetVisualDescendantsAndSelfIterator(DependencyObject element)
        {
            var remaining = new Queue<DependencyObject>();
            remaining.Enqueue(element);

            while (remaining.Count > 0)
            {
                var obj = remaining.Dequeue();
                yield return obj;

                foreach (var child in obj.GetVisualChildren())
                {
                    remaining.Enqueue(child);
                }
            }
        }

        /// <summary>
        /// Get the visual tree siblings of an element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The visual tree siblings of an element.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualSiblings(this DependencyObject element)
        {
            return element
                .GetVisualSiblingsAndSelf()
                .Where(p => p != element);
        }

        /// <summary>
        /// Get the visual tree siblings of an element and the element itself.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The visual tree siblings of an element and the element itself.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualSiblingsAndSelf(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            DependencyObject parent = VisualTreeHelper.GetParent(element);
            return parent == null ?
                Enumerable.Empty<DependencyObject>() :
                parent.GetVisualChildren();
        }

        /// <summary>
        /// Get the bounds of an element relative to another element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="otherElement">
        /// The element relative to the other element.
        /// </param>
        /// <returns>
        /// The bounds of the element relative to another element, or null if
        /// the elements are not related.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="otherElement"/> is null.
        /// </exception>
        public static Rect? GetBoundsRelativeTo(this FrameworkElement element, UIElement otherElement)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
        	
			if (otherElement == null)
        	{
        		throw new ArgumentNullException("otherElement");
        	}

        	try
            {
                Point origin, bottom;
                var transform = element.TransformToVisual(otherElement);
                if (transform != null &&
                    transform.TryTransform(new Point(), out origin) &&
                    transform.TryTransform(new Point(element.ActualWidth, element.ActualHeight), out bottom))
                {
                    return new Rect(origin, bottom);
                }
            }
            catch (ArgumentException)
            {
                // Ignore any exceptions thrown while trying to transform
            }

            return null;
        }

        public static TChildItem FindVisualChild<TChildItem>(DependencyObject obj) where TChildItem : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is TChildItem)
                {
                    return (TChildItem)child;
                }
            	
				var childOfChild = FindVisualChild<TChildItem>(child);

            	if (childOfChild != null)
            	{
            		return childOfChild;
            	}
            }

            return null;
        }

        /// <summary>
        /// Equivalent of FindName, but works on the visual tree to go through templates, etc.
        /// </summary>
        /// <param name="root">The node to search from</param>
        /// <param name="name">The name to look for</param>
        /// <returns>The found node, or null if not found</returns>
        public static FrameworkElement FindVisualChild(this FrameworkElement root, string name)
        {
            var temp = root.FindName(name) as FrameworkElement;
            if (temp != null)
                return temp;

            foreach (FrameworkElement element in root.GetVisualChildren())
            {
                temp = element.FindName(name) as FrameworkElement;
                if (temp != null)
                    return temp;
            }

            return null;
        }

        public static IEnumerable<DependencyObject> GetVisuals(this DependencyObject root)
        {
            int count = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                yield return child;
                foreach (var descendants in child.GetVisuals())
                {
                    yield return descendants;
                }
            }
        }

        /// <summary>
        /// Gets a visual child of the element
        /// </summary>
        /// <param name="node">The element to check</param>
        /// <param name="index">The index of the child</param>
        /// <returns>The found child</returns>
        public static FrameworkElement GetVisualChild(this FrameworkElement node, int index)
        {
            return VisualTreeHelper.GetChild(node, index) as FrameworkElement;
        }


        /// <summary>
        /// Gets the visual parent of the element
        /// </summary>
        /// <param name="node">The element to check</param>
        /// <returns>The visual parent</returns>
        public static FrameworkElement GetVisualParent(this FrameworkElement node)
        {
            return VisualTreeHelper.GetParent(node) as FrameworkElement;
        }

        /// <summary>
        /// Gets the VisualStateGroup with the given name, looking up the visual tree
        /// </summary>
        /// <param name="root">Element to start from</param>
        /// <param name="groupName">Name of the group to look for</param>
        /// <param name="searchAncestors">Whether or not to look up the tree</param>
        /// <returns>The group, if found</returns>
        public static VisualStateGroup GetVisualStateGroup(this FrameworkElement root, string groupName, bool searchAncestors)
        {
            var groups = VisualStateManager.GetVisualStateGroups(root);
        	
			foreach (var o in groups)
			{
				var group = o as VisualStateGroup;

				if (group != null && group.Name == groupName)
					return group;
			}

        	if (searchAncestors)
            {
                var parent = root.GetVisualParent();

                if (parent != null)
                    return parent.GetVisualStateGroup(groupName, true);
            }

            return null;
        }

    	/// <summary>
        /// Returns a render transform of the specified type from the element, creating it if necessary
        /// </summary>
        /// <typeparam name="TRequestedTransform">The type of transform (Rotate, Translate, etc)</typeparam>
        /// <param name="element">The element to check</param>
        /// <param name="mode">The mode to use for creating transforms, if not found</param>
        /// <returns>The specified transform, or null if not found and not created</returns>
        public static TRequestedTransform GetTransform<TRequestedTransform>(this UIElement element, TransformCreationMode mode) where TRequestedTransform : Transform, new()
        {
            //if (element == null)
            //    return null;

            Transform originalTransform = element.RenderTransform;
            TRequestedTransform requestedTransform;

        	// Current transform is null -- create if necessary and return
            if (originalTransform == null)
            {
                if ((mode & TransformCreationMode.Create) == TransformCreationMode.Create)
                {
                    requestedTransform = new TRequestedTransform();
                    element.RenderTransform = requestedTransform;
                    return requestedTransform;
                }

                return null;
            }

            // Transform is exactly what we want -- return it
            requestedTransform = originalTransform as TRequestedTransform;

            if (requestedTransform != null)
                return requestedTransform;


            // The existing transform is matrix transform - overwrite if necessary and return
            var matrixTransform = originalTransform as MatrixTransform;

            if (matrixTransform != null)
            {
                if (matrixTransform.Matrix.IsIdentity
                  && (mode & TransformCreationMode.Create) == TransformCreationMode.Create
                  && (mode & TransformCreationMode.IgnoreIdentityMatrix) == TransformCreationMode.IgnoreIdentityMatrix)
                {
                    requestedTransform = new TRequestedTransform();
                    element.RenderTransform = requestedTransform;
                    return requestedTransform;
                }

                return null;
            }

            // Transform is actually a group -- check for the requested type
            var transformGroup = originalTransform as TransformGroup;
            if (transformGroup != null)
            {
            	foreach (var child in transformGroup.Children)
            	{
					// Child is the right type -- return it
            		if (child is TRequestedTransform)
            			return child as TRequestedTransform;
            	}

            	// Right type was not found, but we are OK to add it
                if ((mode & TransformCreationMode.AddToGroup) == TransformCreationMode.AddToGroup)
                {
                    requestedTransform = new TRequestedTransform();
                    transformGroup.Children.Add(requestedTransform);
                    return requestedTransform;
                }

                return null;
            }

            // Current ransform is not a group and is not what we want;
            // create a new group containing the existing transform and the new one
            if ((mode & TransformCreationMode.CombineIntoGroup) == TransformCreationMode.CombineIntoGroup)
            {
                transformGroup = new TransformGroup();
                transformGroup.Children.Add(originalTransform);
                transformGroup.Children.Add(requestedTransform);
                element.RenderTransform = transformGroup;

                return requestedTransform;
            }

            return null;
        }

        /// <summary>
        /// Returns a string representation of a property path needed to update a Storyboard
        /// </summary>
        /// <param name="element">The element to get the path for</param>
        /// <param name="subProperty">The property of the transform</param>
        /// <typeparam name="TRequestedType">The type of transform to look fo</typeparam>
        /// <returns>A property path</returns>
        public static string GetTransformPropertyPath<TRequestedType>(this FrameworkElement element, string subProperty) where TRequestedType : Transform
        {
            var t = element.RenderTransform;
            if (t is TRequestedType)
                return String.Format("(RenderTransform).({0}.{1})", typeof(TRequestedType).Name, subProperty);

        	if (t is TransformGroup)
        	{
        		var g = t as TransformGroup;
        		for (var i = 0; i < g.Children.Count; i++)
        		{
        			if (g.Children[i] is TRequestedType)
        				return String.Format("(RenderTransform).(TransformGroup.Children)[" + i + "].({0}.{1})",
        				                     typeof(TRequestedType).Name, subProperty);
        		}
        	}

        	return "";
        }

        /// <summary>
        /// Returns a plane projection, creating it if necessary
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="create">Whether or not to create the projection if it doesn't already exist</param>
        /// <returns>The plane project, or null if not found / created</returns>
        public static PlaneProjection GetPlaneProjection(this UIElement element, bool create)
        {
            Projection originalProjection = element.Projection;
            PlaneProjection projection = null;

            // Projection is already a plane projection; return it
            if (originalProjection is PlaneProjection)
                return originalProjection as PlaneProjection;

            // Projection is null; create it if necessary
            if (originalProjection == null)
            {
                if (create)
                {
                    projection = new PlaneProjection();
                    element.Projection = projection;
                }
            }

            // Note that if the project is a Matrix projection, it will not be
            // changed and null will be returned.
            return projection;
        }

        /// <summary>
        /// Perform an action when the element's LayoutUpdated event fires.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="action">The action to perform.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="action"/> is null.
        /// </exception>
        public static void InvokeOnLayoutUpdated(this FrameworkElement element, Action action)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
        	
			if (action == null)
        	{
        		throw new ArgumentNullException("action");
        	}

        	// Create an event handler that unhooks itself before calling the
            // action and then attach it to the LayoutUpdated event.
            EventHandler handler = null;
            handler = (s, e) =>
            {
                //TODO: is this the right thing to do?
                //Deployment.Current.Dispatcher.BeginInvoke(() => { element.LayoutUpdated -= handler; });
                element.LayoutUpdated -= handler;

                action();
            };
            element.LayoutUpdated += handler;
        }

        /// <summary>
        /// Retrieves all the logical children of a framework element using a 
        /// breadth-first search. For performance reasons this method manually 
        /// manages the stack instead of using recursion.
        /// </summary>
        /// <param name="parent">The parent framework element.</param>
        /// <returns>The logical children of the framework element.</returns>
        internal static IEnumerable<FrameworkElement> GetLogicalChildren(this FrameworkElement parent)
        {
            var popup = parent as Popup;
            if (popup != null)
            {
                var popupChild = popup.Child as FrameworkElement;
                if (popupChild != null)
                {
                    yield return popupChild;
                }
            }

            // If control is an items control return all children using the 
            // Item container generator.
            var itemsControl = parent as ItemsControl;
            if (itemsControl != null)
            {
                foreach (var logicalChild in
                    Enumerable
                        .Range(0, itemsControl.Items.Count)
                        .Select(index => itemsControl.ItemContainerGenerator.ContainerFromIndex(index))
                        .OfType<FrameworkElement>())
                {
                    yield return logicalChild;
                }
            }

            var queue = new Queue<FrameworkElement>(parent.GetVisualChildren().OfType<FrameworkElement>());

            while (queue.Count > 0)
            {
                var element = queue.Dequeue();

                if (element.Parent == parent || element is UserControl)
                {
                    yield return element;
                }
                else
                {
                    foreach (var visualChild in element.GetVisualChildren().OfType<FrameworkElement>())
                    {
                        queue.Enqueue(visualChild);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Possible modes for creating a transform
    /// </summary>
    [Flags]
    public enum TransformCreationMode
    {
        /// <summary>
        /// Don't try and create a transform if it doesn't already exist
        /// </summary>
        None = 0,

        /// <summary>
        /// Create a transform if none exists
        /// </summary>
        Create = 1,

        /// <summary>
        /// Create and add to an existing group
        /// </summary>
        AddToGroup = 2,

        /// <summary>
        /// Create a group and combine with existing transform; may break existing animations
        /// </summary>
        CombineIntoGroup = 4,

        /// <summary>
        /// Treat identity matrix as if it wasn't there; may break existing animations
        /// </summary>
        IgnoreIdentityMatrix = 8,

        /// <summary>
        /// Create a new transform or add to group
        /// </summary>
        CreateOrAddAndIgnoreMatrix = Create | AddToGroup | IgnoreIdentityMatrix,

        /// <summary>
        /// Default behaviour, equivalent to CreateOrAddAndIgnoreMatrix
        /// </summary>
        Default = CreateOrAddAndIgnoreMatrix,
    }
}