using System;
using System.Windows.Input;

namespace SenpaiCopy
{
	/// <summary>
	/// Class that executes a command. Mainly used for key shortcuts.
	/// </summary>
	public class KeyCommand : ICommand
	{
		/// <summary>
		/// Method to execute on command execute.
		/// </summary>
		private Action<object> _execute;

		/// <summary>
		/// Event that triggers when the CanExecuteChanged status of the command changes.
		/// </summary>
		public event EventHandler CanExecuteChanged = delegate { };

		/// <summary>
		/// Ctor.
		/// </summary>
		/// <param name="execute">Method to execute upon command call.</param>
		public KeyCommand(Action<object> execute)
		{
			_execute = execute;
		}

		/// <summary>
		/// Gets if the command can be executed.
		/// </summary>
		/// <param name="parameter">Ignored.</param>
		/// <returns>Bool that indicates if the command can be executed.</returns>
		public bool CanExecute(object parameter)
		{
			return true;
		}

		/// <summary>
		/// Executes the command method with the given <paramref name="param"/>.
		/// </summary>
		/// <param name="param">Parameter for the method that will be executed.</param>
		public void Execute(object param)
		{
			_execute(param);
		}
	}
}