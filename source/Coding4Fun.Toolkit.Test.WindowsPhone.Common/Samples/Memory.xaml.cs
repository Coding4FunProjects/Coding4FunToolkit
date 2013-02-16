using System;
using System.Collections.Generic;
using System.Windows;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples
{
    public partial class Memory : PhoneApplicationPage
    {
        public Memory()
        {
            InitializeComponent();
        }

	    readonly Stack<Byte[]> _memoryTwoMb = new Stack<byte[]>();
	    readonly Stack<Byte[]> _memoryOneHundredMb = new Stack<byte[]>();

        private void Add2MbClick(object sender, RoutedEventArgs e)
        {
	        PushStack(_memoryTwoMb, 2);
        }

	    private void Remove2MbClick(object sender, RoutedEventArgs e)
	    {
			PopStack(_memoryTwoMb);
	    }

		private void Add100MbClick(object sender, RoutedEventArgs e)
		{
			PushStack(_memoryOneHundredMb, 100);
		}

		private void Remove100MbClick(object sender, RoutedEventArgs e)
		{
			PopStack(_memoryOneHundredMb);
		}

	    private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

		private static byte[] CreateMemorySegment(int totalMb)
		{
			return new Byte[1024 * 1024 * totalMb];
		}

		private static void PopStack(Stack<Byte[]> data)
		{
			if (data.Count > 0)
				data.Pop();

			GC.Collect();
		}

		private static void PushStack(Stack<Byte[]> data, int totalMb)
		{
			data.Push(CreateMemorySegment(totalMb));
		}
    }
}