using System;
using System.IO.Ports;
using System.ComponentModel;
using System.IO;

namespace gnux.Utils
{
	public sealed class Serial : IDisposable
	{
		private SerialPort _port;
		private const int BUFFER_SIZE = 256;
		
		public struct StateObj
		{
			public byte[] data;
		}
		
		public delegate void OnDataReceivedDelegate (byte[] buffer);//, int size);
		
		public event OnDataReceivedDelegate OnDataReceived;

		public Serial (string portName, int baudRate = 9600, Parity parity=Parity.None, int dataBits=8, StopBits stopBits=StopBits.One)
		{
			_port = new SerialPort (portName, baudRate, parity, dataBits, stopBits);
		}
		
		public void Open ()
		{
			lock (_port) {
				if (!_port.IsOpen)
					_port.Open ();
			}
		}
		
		public void Close ()
		{
			lock (_port)
				if (_port.IsOpen)
					_port.Close ();
		}
		
		public bool IsOpen{ get { return _port.IsOpen; } }
		
		public static string[] GetPortNames ()
		{
			return SerialPort.GetPortNames ();
		}
		
		private void ReadCallback (IAsyncResult ar)
		{
			
			try {
				int read;
				lock (_port)
					read = _port.BaseStream.EndRead (ar);
				StateObj state = (StateObj)ar.AsyncState;
				byte[] data = new byte[read];
				Array.Copy (state.data, data, read);
				if (OnDataReceived != null)
					OnDataReceived (data);
			
				StartRead ();
			} catch (IOException) {
				
			}
				
			
		}
		
		private void StartRead ()
		{
			StateObj state = new StateObj ();
			state.data = new byte[BUFFER_SIZE];
			lock (_port)
				_port.BaseStream.BeginRead (state.data, 0, BUFFER_SIZE, new AsyncCallback (ReadCallback), state);
		}
	
		
//		public int Read (Byte[] buffer, int offset, int count)
//		{
//			lock (_port) {
//				return _port.Read (buffer, offset, count);
//			}
//		}
//		
//		public int Read (Char[] buffer, int offset, int count)
//		{
//			lock (_port) {
//				return _port.Read (buffer, offset, count);
//			}
//		}
//		
//		public int ReadByte ()
//		{
//			lock (_port)
//				return _port.ReadByte ();
//		}
//		
//		public int ReadChar ()
//		{
//			lock (_port)
//				return _port.ReadChar ();
//		}
//	
//		public string ReadExisting ()
//		{
//			lock (_port)
//				return _port.ReadExisting ();
//		}
//				
//		public string ReadLine ()
//		{
//			lock (_port)
//				return _port.ReadLine ();
//		}
//		
//		public string ReadTo (string value)
//		{
//			lock (_port)
//				return _port.ReadTo (value);
//		}
//		
//		public void Write (string str)
//		{
//			lock (_port)
//				_port.Write (str);
//		}
//		
//		public void Write (Byte[] buffer, int offset, int count)
//		{
//			lock (_port)
//				_port.Write (buffer, offset, count);
//		}
//
//		public void Write (Char[] buffer, int offset, int count)
//		{
//			lock (_port)
//				_port.Write (buffer, offset, count);
//		}
//
//		public void WriteLine (string text)
//		{
//			lock (_port)
//				_port.WriteLine (text);
//		}
	
		#region IDisposable implementation
		void IDisposable.Dispose ()
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

