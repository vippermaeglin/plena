using ModulusFE.Sockets;

namespace M4.DataServer.Interface.ProtocolStructs
{
    public class AuthenticationPS : IParserStruct
    {
        public int ValueCount { get; set; }
        public string[] Values;

        #region Overrides of IParserStruct

        public override int Id
        {
            get { return StructsIds.Authentication_Id; }
        }

        public override void WriteBytes(byte[] buffer, int offset)
        {
            StartW(buffer, offset);
            _bw.Write(ValueCount);
            foreach (var value in Values)
            {
                _bw.WriteEx(value);
            }
            StopW();
        }

        public override void ReadBytes(byte[] buffer, int offset)
        {
            StartR(buffer, offset);
            ValueCount = _br.ReadInt32();
            if (ValueCount > 0)
            {
                Values = new string[ValueCount];
                for (int i = 0; i < Values.Length; i++)
                {
                    Values[i] = _br.ReadString();
                }
            }
            StopR();
        }

        #endregion
    }

    public enum ConnectionType
    {
        DataFeeder,
        Multiplexer,
        Connector
    }

    public class DataServerInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }

        public bool Equals(DataServerInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name) && Equals(other.Address, Address) && other.Port == Port;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(DataServerInfo)) return false;
            return Equals((DataServerInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Name != null ? Name.GetHashCode() : 0);
                result = (result * 397) ^ (Address != null ? Address.GetHashCode() : 0);
                result = (result * 397) ^ Port;
                return result;
            }
        }
    }

    public class AuthenticationAnswerPS : IParserStruct
    {
        #region Overrides of IParserStruct

        public string ClientId { get; set; }
        public DataServerInfo[] DataServers { get; set; }
        public int ConnectionType { get; set; }

        public override int Id
        {
            get { return StructsIds.AuthenticationAnswer_Id; }
        }

        public override void WriteBytes(byte[] buffer, int offset)
        {
            StartW(buffer, offset);

            _bw.WriteEx(ClientId);
            _bw.Write(DataServers.Length);
            _bw.Write(ConnectionType);
            foreach (var info in DataServers)
            {
                _bw.WriteEx(info.Name);
                _bw.WriteEx(info.Address);
                _bw.Write(info.Port);
            }

            StopW();
        }

        public override void ReadBytes(byte[] buffer, int offset)
        {
            StartR(buffer, offset);

            ClientId = _br.ReadString();
            int count = _br.ReadInt32();
            ConnectionType = _br.ReadInt32();
            DataServers = new DataServerInfo[count];
            for (int i = 0; i < count; i++)
            {
                DataServers[i] = new DataServerInfo
                                   {
                                       Name = _br.ReadString(),
                                       Address = _br.ReadString(),
                                       Port = _br.ReadInt32()
                                   };
            }

            StopR();
        }

        #endregion
    }
}
