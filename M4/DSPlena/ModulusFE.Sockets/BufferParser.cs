using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ModulusFE.Sockets
{
    public class BufferParser


    {
        public class BufferParserException : Exception
        {
            public BufferParserException(string message)
                : base(message)
            {
            }
        }

        public delegate void StructReadeHandler(BufferParser sender, List<IParserStruct> structure);
        public event StructReadeHandler StructRead;
        private List<IParserStruct> StructsRead = new List<IParserStruct>();
        private List<DateTime> TimesRead = new List<DateTime>();
        private readonly MemoryStream _stream = new MemoryStream(1024);

        public Dictionary<int, Type> Structs;
        public bool StandaloneUsage;

        public bool IsHistorical = false;

        public long Size()
        {
            return _stream.Length;
        }




        public List<IParserStruct> GetStructures()
        {
            List<IParserStruct> ret;
            lock(StructsRead)
            {
                ret=new List<IParserStruct>(StructsRead);
                StructsRead.Clear();
            }
            return ret;
        }



        public void Write(byte[] data, int offset, int length)
        {
            lock (_stream)
            {
                _stream.Write(data, offset, length);
            }
        }


        private readonly byte[] _buffer = new byte[2*1024];
        public void Parse()
        {
            lock (_stream)
            {
                int step = 0;
                int structLength = -1;



                IParserStruct currentStruct = null;








                //reset stream Position, After writing it points to the end of stream
                _stream.Position = 0;








                while (_stream.Position + 8 <= _stream.Length) //8 cause we need at least 8 bytes to start
                {
                    if (step == 0)
                    {
                        //read Id - it must be always at first place
                        _stream.Read(_buffer, 0, 8); //8 bytes for StructId and its Length
                        int structId = BitConverter.ToInt32(_buffer, 0);


                        Type currentStructType;
                        if (!Structs.TryGetValue(structId, out currentStructType))
                        {
                            Debug.WriteLine(
                                string.Format(
                                    "ERROR Getting value for structID {2} - Stream Length: {0}. Standalone: {1}",
                                    _stream.Length, StandaloneUsage, structId));
                            _stream.SetLength(0);
                            _stream.Seek(0, SeekOrigin.Begin);
                            return;
                            //throw new BufferParserException(string.Format("Structure with ID = {0} is not supported.", structId));
                        }



                        currentStruct = (IParserStruct)Activator.CreateInstance(currentStructType);








                        //read struct Length
                        structLength = BitConverter.ToInt32(_buffer, 4);










                        step = 1;
                        continue;
                    }
                    if (step == 1)
                    {
                        if (currentStruct == null || structLength == -1)
                            throw new BufferParserException("Got to read the structure, but not created.");
                        if ((structLength + _stream.Position <= _stream.Length)&&(structLength + _stream.Position <= _buffer.Length))
                        {
                            _stream.Read(_buffer, 0, structLength);
                            currentStruct.ReadBytes(_buffer, 0);
                            //make an additional check to ensure struct was read OK
                            //if (!currentStruct.CrcOk())
                            //  throw new BufferParserException("Structure was read, but its CRC is wrong.");


                            //if is ok, raise the event that structure was read succesfully
                            if(!IsHistorical)StructRead(this, new List<IParserStruct>(){currentStruct});
                            else
                            {
                                lock(StructsRead)StructsRead.Add(currentStruct);
                            }


















                            //go to read another structure
                            step = 0;
                        }
                        else
                        {
                            break; //nothing to read
                        }
                    }
                }
                if (step == 1)
                    _stream.Position -= 8;



                int shiftBufferLength = (int)(_stream.Length - _stream.Position);
                if (shiftBufferLength > 0)
                {
                    _stream.Read(_buffer, 0, shiftBufferLength);
                    _stream.SetLength(0);
                    _stream.Seek(0, SeekOrigin.Begin);
                    _stream.Write(_buffer, 0, shiftBufferLength);
                }
                else
                {
                    _stream.SetLength(0);
                    _stream.Seek(0, SeekOrigin.Begin);
                }
                


            }





        }

















    }

}

