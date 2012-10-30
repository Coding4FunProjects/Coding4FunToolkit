﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Coding4Fun.Toolkit.Controls
{
    [ContentProperty("Content")]
    public class Tile : Button
    {
        public Tile()
		{
            DefaultStyleKey = typeof(Tile);
		}

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Tile), new PropertyMetadata(""));

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextWrapping.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(Tile), new PropertyMetadata(TextWrapping.NoWrap));
    }
}
