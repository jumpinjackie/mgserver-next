using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OSGeo.MapGuide
{
    partial class ResourceIdentifier
    {
        /// <summary>
        /// Parses the given resource id string to a <see cref="ResourceIdentifer"/>
        /// </summary>
        /// <param name="resIdStr"></param>
        /// <returns></returns>
        public static ResourceIdentifier Parse(string resIdStr)
        {
            var parts = resIdStr.Split(new[] { "://" }, StringSplitOptions.RemoveEmptyEntries);
            var subParts = resIdStr.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var resName = subParts[subParts.Length - 1];
            var resParts = resName.Split('.');

            var resId = new ResourceIdentifier
            {
                RepositoryType = parts[0],
                Path = string.Join("/", subParts.Skip(1).Take(subParts.Length - 2)),
                Name = resParts[0],
                ResourceType = resParts[1]
            };
            return resId;
        }
    }

    public class MgDataReader
    {
        public static async Task<MgDataReader> Create(Grpc.Core.AsyncServerStreamingCall<DataRecord> stream, CancellationToken token)
        {
            var first = await stream.ResponseStream.MoveNext(token);
            if (first)
            {
                var rec = stream.ResponseStream.Current;
                //Per our protobuf spec, the first record here will have either an error or class definition header
                if (rec.Error != null)
                {
                    throw new Exception(rec.Error.Message);
                }
                return new MgDataReader(rec.Header, stream.ResponseStream, token);
            }
            throw new Exception("No result");
        }

        private CancellationToken _token;
        private Grpc.Core.IAsyncStreamReader<DataRecord> _stream;
        private Dictionary<string, PropertyValue> _current;

        public DataRecordHeader Header { get; }

        private MgDataReader(DataRecordHeader header, Grpc.Core.IAsyncStreamReader<DataRecord> stream, CancellationToken token)
        {
            _current = new Dictionary<string, PropertyValue>();
            _token = token;
            _stream = stream;
            this.Header = header;
        }

        public async Task<bool> ReadNextAsync()
        {
            var res = await this._stream.MoveNext(_token);
            if (res)
            {
                var rec = this._stream.Current;
                foreach (var val in rec.Values)
                {
                    _current[val.Name] = val;
                }
            }
            return res;
        }

        private static T GetValue<T>(Dictionary<string, PropertyValue> dict, string name, PropertyValue.PropertyValueOneofCase expectedType, Func<PropertyValue, T> reader)
        {
            if (!dict.ContainsKey(name))
            {
                throw new Exception($"Property not found: {name}");
            }

            var prop = dict[name];
            if (prop.PropertyValueCase != expectedType)
                throw new Exception($"Expected type of {expectedType}. Got: {prop.PropertyValueCase}");

            return reader(prop);
        }

        public void Close() { }

        public bool IsNull(string name) => _current.ContainsKey(name) && _current[name].PropertyValueCase == PropertyValue.PropertyValueOneofCase.None;

        public byte GetByte(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.ByteValue, pv => (byte)pv.ByteValue);

        public bool GetBoolean(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.BoolValue, pv => pv.BoolValue);

        public string GetDateTime(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.DateTimeValue, pv => pv.DateTimeValue);

        public double GetDecimal(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.DecimalValue, pv => pv.DecimalValue);

        public double GetDouble(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.DoubleValue, pv => pv.DoubleValue);

        public short GetInt16(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.Int16Value, pv => (short)pv.Int16Value);

        public int GetInt32(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.Int32Value, pv => pv.Int32Value);

        public long GetInt64(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.Int64Value, pv => pv.Int64Value);

        public float GetSingle(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.SingleValue, pv => pv.SingleValue);

        public string GetString(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.StringValue, pv => pv.StringValue);
    }

    public class MgFeatureReader
    {
        public static async Task<MgFeatureReader> Create(Grpc.Core.AsyncServerStreamingCall<FeatureRecord> stream, CancellationToken token)
        {
            var first = await stream.ResponseStream.MoveNext(token);
            if (first)
            {
                var rec = stream.ResponseStream.Current;
                //Per our protobuf spec, the first record here will have either an error or class definition header
                if (rec.Error != null)
                {
                    throw new Exception(rec.Error.Message);
                }
                return new MgFeatureReader(rec.Header, stream.ResponseStream, token);
            }
            throw new Exception("No result");
        }

        private CancellationToken _token;
        private Grpc.Core.IAsyncStreamReader<FeatureRecord> _stream;
        private Dictionary<string, PropertyValue> _current;

        public ClassDefinition ClassDefinition { get; }

        private MgFeatureReader(ClassDefinition header, Grpc.Core.IAsyncStreamReader<FeatureRecord> stream, CancellationToken token)
        {
            _current = new Dictionary<string, PropertyValue>();
            _token = token;
            _stream = stream;
            this.ClassDefinition = header;
        }

        public async Task<bool> ReadNextAsync()
        {
            var res = await this._stream.MoveNext(_token);
            if (res)
            {
                var rec = this._stream.Current;
                foreach (var val in rec.Values)
                {
                    _current[val.Name] = val;
                }
            }
            return res;
        }

        private static T GetValue<T>(Dictionary<string, PropertyValue> dict, string name, PropertyValue.PropertyValueOneofCase expectedType, Func<PropertyValue, T> reader)
        {
            if (!dict.ContainsKey(name))
            {
                throw new Exception($"Property not found: {name}");
            }

            var prop = dict[name];
            if (prop.PropertyValueCase != expectedType)
                throw new Exception($"Expected type of {expectedType}. Got: {prop.PropertyValueCase}");

            return reader(prop);
        }

        public void Close() { }

        public bool IsNull(string name) => _current.ContainsKey(name) && _current[name].PropertyValueCase == PropertyValue.PropertyValueOneofCase.None;

        public byte GetByte(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.ByteValue, pv => (byte)pv.ByteValue);

        public bool GetBoolean(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.BoolValue, pv => pv.BoolValue);

        public string GetDateTime(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.DateTimeValue, pv => pv.DateTimeValue);

        public double GetDecimal(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.DecimalValue, pv => pv.DecimalValue);

        public double GetDouble(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.DoubleValue, pv => pv.DoubleValue);

        public short GetInt16(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.Int16Value, pv => (short)pv.Int16Value);

        public int GetInt32(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.Int32Value, pv => pv.Int32Value);

        public long GetInt64(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.Int64Value, pv => pv.Int64Value);

        public float GetSingle(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.SingleValue, pv => pv.SingleValue);

        public string GetString(string name) => GetValue(_current, name, PropertyValue.PropertyValueOneofCase.StringValue, pv => pv.StringValue);
    }
}