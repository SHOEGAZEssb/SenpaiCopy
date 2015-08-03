using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace SenpaiCopy
{
	public class InputBindingTrigger : TriggerBase<FrameworkElement>, ICommand
	{
		public static readonly DependencyProperty InputBindingProperty =
			DependencyProperty.Register("InputBinding", typeof(InputBinding)
				, typeof(InputBindingTrigger)
				, new UIPropertyMetadata(null));

		public InputBinding InputBinding
		{
			get { return (InputBinding)GetValue(InputBindingProperty); }
			set { SetValue(InputBindingProperty, value); }
		}

		public event EventHandler CanExecuteChanged = delegate { };

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			InvokeActions(parameter);
		}
	}
}
