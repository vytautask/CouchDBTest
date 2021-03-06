using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LoveSeat;

namespace ObjectModel
{
	public abstract class CPrimitive : JObject, IDisposable
	{
        private bool _isDisposed = false;

		public CPrimitive ()
		{
		}		
		
        public bool IsDisposed
        {
            get { return _isDisposed; }
            set { _isDisposed = value; }
        }

		public abstract void WriteJson(JsonWriter writer);
		public abstract void ReadJson(JObject obj);

        protected virtual void Dispose(bool disposing)
		{
			if (!IsDisposed)
			{
				if (disposing)
				{
                    
				}

				IsDisposed = false;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~CPrimitive()
		{
			Dispose(false);
		}
	}
}

