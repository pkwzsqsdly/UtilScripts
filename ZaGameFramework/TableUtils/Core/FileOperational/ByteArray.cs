using System;

namespace LG.TableUtil.FileOperational
{

	public class ByteArray
	{
		private const int defaultSize = 64 * 1024;
		private byte[] bytes;
		private int cursor;

		public int Cursor{ get{ return cursor; } }
		public int ByteSize { get{ return bytes.Length; } } 
		public bool IsReadDone { get { return cursor>= bytes.Length; } }
		public byte[] GetBytes()
		{
			byte[] bList = new byte[cursor];
			Array.Copy( bytes, 0, bList, 0, cursor );
			return bList;
		}

		public ByteArray(byte[] bts)
		{
			bytes = bts;
		}

		public ByteArray(int len){
			bytes = new byte[len];
		}

		public ByteArray(){
			bytes = new byte[defaultSize];
		}


		//read byte
		public byte ReadByte()
		{
			if ( cursor >=0 && cursor < ByteSize )
			{
				int size = sizeof(byte);
				if ( (cursor+size) > ByteSize )
				{
					return 0;
				}
				byte bData = bytes[cursor];
				cursor += size;
				return bData;
			}
			return 0;
		}
		
		//read sbyte
		public sbyte ReadSbyte()
		{
			if ( cursor >=0 && cursor < ByteSize )
			{
				int size = sizeof(sbyte);
				if ( (cursor+size) > ByteSize )
				{
					return 0;
				}
				sbyte bData = (sbyte)bytes[cursor];
				cursor += size;
				return bData;
			}
			return 0;
		}
		
		//read short
		public short ReadShort()
		{
			if ( cursor >=0 && cursor < ByteSize )
			{
				int size = sizeof(short);
				if ( (cursor+size) > ByteSize )
				{
					return 0;
				}
				return ReadShortUnlimite();
			}
			return 0;
		}
		
		public ushort ReadUShort()
		{
			if ( cursor >=0 && cursor < ByteSize )
			{
				int size = sizeof(ushort);
				if ( (cursor+size) > ByteSize )
				{
					return 0;
				}
				return ReadUShortUnlimite();
			}
			return 0;
		}
		
		//read int
		public int ReadInt()
		{
			if ( cursor >=0 && cursor < ByteSize )
			{
				int size = sizeof(int);
				if ( (cursor+size) > ByteSize )
				{
					return 0;
				}
				return ReadInt32Unlimite();
			}
			return 0;
		}
		
		//read uint
		public uint ReadUInt32()
		{
			if ( cursor >=0 && cursor < ByteSize )
			{
				int size = sizeof(uint);
				if ( (cursor+size) > ByteSize )
				{
					return 0;
				}
				return ReadUInt32Unlimite();
			}
			return 0;
		}
		
		//read int64
		public long ReadInt64()
		{
			if ( cursor >=0 && cursor < ByteSize )
			{
				int size = sizeof(long);
				if ( cursor + size > ByteSize )
				{
					return 0;
				}
				long value = BitConverter.ToInt64( bytes, cursor );
				cursor += size;
				//if ( BitConverter.IsLittleEndian )
				// return System.Net.IPAddress.NetworkToHostOrder(value);
				return value;
			}
			return 0;
		}

		public float ReadFloat()
		{
			if (cursor >= 0 && cursor < ByteSize)
			{
				int size = sizeof(float);
				if (cursor + size > ByteSize)
				{
					return 0;
				}
				float fValue = BitConverter.ToSingle(bytes, cursor);
				cursor += size;
				//if ( BitConverter.IsLittleEndian )
				return fValue;
			}
			return 0;
		}
		
		//read string
		public string ReadString()
		{
			if ( cursor >=0 && cursor < ByteSize )
			{
				int nBlobLen = 0 ;
				byte[] bDataTemp = ReadBlob(out nBlobLen);
				if ( bDataTemp != null && nBlobLen > 0 )
					return System.Text.Encoding.UTF8.GetString(bDataTemp,0,nBlobLen);
			}
			return "";
		}
		
		//read blob
		public byte[] ReadBlob(out int nBlobLength )
		{
			if ( cursor >=0 && cursor < ByteSize )
			{
				//first read bloglength
				int nBlobLenT = ReadInt();
				if ( (cursor+nBlobLenT) > ByteSize )
				{
					nBlobLength=0;
					return null;
				}
				
				//send read byte[]
				if ( nBlobLenT > 0 )
				{
					nBlobLength = nBlobLenT;
					
					byte[] bList = new byte[nBlobLength];
					Array.Copy( bytes, cursor, bList, 0, nBlobLength );
					
					cursor += nBlobLength;
					return bList;
				}
			}
			nBlobLength=0;
			return null;
		}

		//read short Unlimite
		private short ReadShortUnlimite()
		{
			int size = sizeof(short);
			//LDebug.Log("nPos:"+cursor);
			short value = BitConverter.ToInt16( bytes, cursor );
			cursor += size;
			//LDebug.Log("Short:"+value);
			// return System.Net.IPAddress.NetworkToHostOrder(value);
			return value;
		}
		
		//read ushort Unlimite
		private ushort ReadUShortUnlimite()
		{
			int size = sizeof(ushort);
			//LDebug.Log("nPos:"+cursor);
			short value = BitConverter.ToInt16( bytes, cursor );
			cursor += size;
			//LDebug.Log("Short:"+value);
			// return (ushort)System.Net.IPAddress.NetworkToHostOrder(value);
			return (ushort)value;
		}
		
		private int ReadInt32Unlimite()
		{
			int size = sizeof(int);
			int value = BitConverter.ToInt32( bytes, cursor );
			cursor += size;
			// return System.Net.IPAddress.NetworkToHostOrder(value);
			return value;
		}
		
		private uint ReadUInt32Unlimite()
		{
			int size = sizeof(uint);
			int value = (int)(BitConverter.ToUInt32( bytes, cursor ));
			cursor += size;
			return (uint)value;
			// return (uint)System.Net.IPAddress.NetworkToHostOrder(value);
		}

		private float ReadFloatUnlimite()
		{
			int size = sizeof(float);
			float fValue = (float)(BitConverter.ToSingle(bytes, cursor));
			cursor += size;
			return fValue;
		}

		//Write byte
		public bool WriteByte(byte bValue)
		{
			int size = sizeof(byte);
			ResizeBuffer(cursor + size);
			bytes[cursor] = bValue;
			cursor += size;
			return true;
		}
		
		//Wite short
		public bool WriteShort(short value)
		{
			int size = sizeof(short);
			ResizeBuffer(cursor + size);
			// value = System.Net.IPAddress.HostToNetworkOrder(value);
			byte[] bDataListT = BitConverter.GetBytes( value );
			Array.Copy( bDataListT, 0, bytes, cursor, sizeof(short) );
			cursor += size;
			return true;
		}
		
		//Wite int
		public bool WriteInt(int value)
		{
			int size = sizeof(int);
			ResizeBuffer(cursor + size);
			// value = System.Net.IPAddress.HostToNetworkOrder(value);
			byte[] bDataListT = BitConverter.GetBytes( value );
			Array.Copy( bDataListT, 0, bytes, cursor, sizeof(int) );
			cursor += size;
			return true;
		}
		
		//Wite int64
		public bool WriteInt64(long value)
		{
			int size = sizeof(long);
			ResizeBuffer(cursor + size);
			// value = System.Net.IPAddress.HostToNetworkOrder(value);
			byte[] bDataListT = BitConverter.GetBytes( value );
			Array.Copy( bDataListT, 0, bytes, cursor, sizeof(long) );
			cursor += size;
			return true;
		}

		public bool WriteFloat(float fValue)
		{
			int size = sizeof(float);
			ResizeBuffer(cursor + size);
			byte[] bDataListT = BitConverter.GetBytes(fValue);
			Array.Copy(bDataListT, 0, bytes, cursor, sizeof(float));
			cursor += size;
			return true;
		}
		//Write string
		public bool	WriteString(String strValue)
		{
			byte[] bWriteString = System.Text.Encoding.UTF8.GetBytes(strValue);
			return WriteBlob( bWriteString, (int)bWriteString.Length );
		}
		
		//Write blob
		public bool WriteBlob( byte[] bBlobList, int nBlobLength )
		{
			int nAllSize = (int)(nBlobLength * sizeof(byte));
			ResizeBuffer(cursor + sizeof(int) + nAllSize);
			WriteInt( nBlobLength );
			Array.Copy(bBlobList,0,bytes, cursor, nAllSize );
			cursor += nAllSize;
			return true;
		}

		private void ResizeBuffer(int newLength)
		{
			if(newLength > ByteSize)
			{
				var newBuffer = new byte[newLength];
				Array.Copy(bytes, 0, newBuffer, 0, cursor);
				bytes = newBuffer;
			}
		}

		
	}
}