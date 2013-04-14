using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace testMargin
{
	public class TestControl : Control
	{
		public TestControl()
        {
			DefaultStyleKey = typeof(TestControl);
        }

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			var top = (RowDefinition)GetTemplateChild("topRow");
			var bottom = (RowDefinition)GetTemplateChild("bottomRow");

			
		}
	}
}
