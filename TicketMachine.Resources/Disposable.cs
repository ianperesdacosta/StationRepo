using System;

namespace TicketMachine.Resources
{
	public class Disposable : IDisposable
	{
		private bool disposed = false; 
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
