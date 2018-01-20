using System;

namespace Hunter.Agent
{
    /// <summary>
    /// Represents an ObjectId (see also BsonObjectId).
    /// </summary>
    public struct MongoID : IEquatable<MongoID>
    {
        // private static fields
        private static MongoID __emptyInstance = default(MongoID);
        private static int __staticMachine = (GetMachineHash() + GetAppDomainId()) & 0x00ffffff;
        private static short __staticPid = GetPid();
        private static int __staticIncrement = (new Random()).Next();

        // private fields
        private long _a;
        private int _b;
        private int _c;

        /// <summary>
        /// Initializes a new instance of the ObjectId class.
        /// </summary>
        /// <param name="timestamp">The timestamp (expressed as a DateTime).</param>
        /// <param name="machine">The machine hash.</param>
        /// <param name="pid">The PID.</param>
        /// <param name="increment">The increment.</param>
        public MongoID(DateTime timestamp, int machine, short pid, int increment)
            : this(GetTimestampFromDateTime(timestamp), machine, pid, increment)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ObjectId class.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="machine">The machine hash.</param>
        /// <param name="pid">The PID.</param>
        /// <param name="increment">The increment.</param>
        public MongoID(long timestamp, int machine, short pid, int increment)
        {
            if ((machine & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException("machine", "The machine value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }
            if ((increment & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException("increment", "The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }

            _a = timestamp;
            _b = (machine << 8) | (((int)pid >> 8) & 0xff);
            _c = ((int)pid << 24) | increment;
        }

        /// <summary>
        /// Initializes a new instance of the ObjectId class.
        /// </summary>
        /// <param name="value">The value.</param>
        public MongoID(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            this = Parse(value);
        }

        // public static properties
        /// <summary>
        /// Gets an instance of ObjectId where the value is empty.
        /// </summary>
        public static MongoID Empty
        {
            get { return __emptyInstance; }
        }

        // public properties
        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        public long Timestamp
        {
            get { return _a; }
        }

        /// <summary>
        /// Gets the machine.
        /// </summary>
        public int Machine
        {
            get { return (_b >> 8) & 0xffffff; }
        }

        /// <summary>
        /// Gets the PID.
        /// </summary>
        public short Pid
        {
            get { return (short)(((_b << 8) & 0xff00) | ((_c >> 24) & 0x00ff)); }
        }

        /// <summary>
        /// Gets the increment.
        /// </summary>
        public int Increment
        {
            get { return _c & 0xffffff; }
        }

        /// <summary>
        /// Gets the creation time (derived from the timestamp).
        /// </summary>
        public DateTime CreationTime
        {
            get { return new DateTime(this._a * 10000000); }
        }

        /// <summary>
        /// Compares two ObjectIds.
        /// </summary>
        /// <param name="lhs">The first ObjectId.</param>
        /// <param name="rhs">The other ObjectId.</param>
        /// <returns>True if the two ObjectIds are equal.</returns>
        public static bool operator ==(MongoID lhs, MongoID rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Compares two ObjectIds.
        /// </summary>
        /// <param name="lhs">The first ObjectId.</param>
        /// <param name="rhs">The other ObjectId.</param>
        /// <returns>True if the two ObjectIds are not equal.</returns>
        public static bool operator !=(MongoID lhs, MongoID rhs)
        {
            return !(lhs == rhs);
        }

        // public static methods
        /// <summary>
        /// Generates a new ObjectId with a unique value.
        /// </summary>
        /// <returns>An ObjectId.</returns>
        public static MongoID GenerateNewId()
        {
            return GenerateNewId(GetTimestampFromDateTime(DateTime.Now));
            //return GenerateNewId(DateTime.Now.Ticks / 10000000);
        }

        /// <summary>
        /// Generates a new ObjectId with a unique value (with the timestamp component based on a given DateTime).
        /// </summary>
        /// <param name="timestamp">The timestamp component (expressed as a DateTime).</param>
        /// <returns>An ObjectId.</returns>
        public static MongoID GenerateNewId(DateTime timestamp)
        {
            return GenerateNewId(GetTimestampFromDateTime(timestamp));
        }

        /// <summary>
        /// Generates a new ObjectId with a unique value (with the given timestamp).
        /// </summary>
        /// <param name="timestamp">The timestamp component.</param>
        /// <returns>An ObjectId.</returns>
        public static MongoID GenerateNewId(long timestamp)
        {
            int increment = System.Threading.Interlocked.Increment(ref __staticIncrement) & 0x00ffffff; // only use low order 3 bytes
            return new MongoID(timestamp, __staticMachine, __staticPid, increment);
        }

        /// <summary>
        /// Parses a string and creates a new ObjectId.
        /// </summary>
        /// <param name="s">The string value.</param>
        /// <returns>A ObjectId.</returns>
        public static MongoID Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            else if (s.Length != 26)
            {
                throw new FormatException(String.Format("'{0}' is not a valid 26 digit hex string.", s));
            }

            var id = default(MongoID);
            id._a = System.Convert.ToInt64(s.Substring(0, 10), 16);
            id._b = System.Convert.ToInt32(s.Substring(10, 8), 16);
            id._c = System.Convert.ToInt32(s.Substring(18, 8), 16);
            return id;
        }

        /// <summary>
        /// Tries to parse a string and create a new ObjectId.
        /// </summary>
        /// <param name="s">The string value.</param>
        /// <param name="objectId">The new ObjectId.</param>
        /// <returns>True if the string was parsed successfully.</returns>
        public static bool TryParse(string s, out MongoID objectId)
        {
            try
            {
                objectId = Parse(s);
                return true;
            }
            catch (System.Exception)
            {
                objectId = default(MongoID);
                return false;
            }

        }

        // private static methods
        private static int GetAppDomainId()
        {
            return AppDomain.CurrentDomain.Id;
        }

        /// <summary>
        /// Gets the current process id.  This method exists because of how CAS operates on the call stack, checking
        /// for permissions before executing the method.  Hence, if we inlined this call, the calling method would not execute
        /// before throwing an exception requiring the try/catch at an even higher level that we don't necessarily control.
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private static int GetCurrentProcessId()
        {
            return System.Diagnostics.Process.GetCurrentProcess().Id;
        }

        private static int GetMachineHash()
        {
            // use instead of Dns.HostName so it will work offline
            var machineName = GetMachineName();
            return 0x00ffffff & machineName.GetHashCode(); // use first 3 bytes of hash
        }

        private static string GetMachineName()
        {
            return Environment.GetEnvironmentVariable("COMPUTERNAME") ?? "";
        }

        private static short GetPid()
        {
            try
            {
                return (short)GetCurrentProcessId(); // use low order two bytes only
            }
            catch (System.Security.SecurityException)
            {
                return 0;
            }
        }

        private static long GetTimestampFromDateTime(DateTime timestamp)
        {
            var secondsSinceEpoch = timestamp.Ticks / 10000000;
            //if (secondsSinceEpoch < uint.MinValue || secondsSinceEpoch > uint.MaxValue)
            //{
            //    throw new ArgumentOutOfRangeException("timestamp");
            //}
            //return (int)secondsSinceEpoch;
            return secondsSinceEpoch;
        }

        /// <summary>
        /// Compares this ObjectId to another ObjectId.
        /// </summary>
        /// <param name="rhs">The other ObjectId.</param>
        /// <returns>True if the two ObjectIds are equal.</returns>
        public bool Equals(MongoID rhs)
        {
            return
                _a == rhs._a &&
                _b == rhs._b &&
                _c == rhs._c;
        }

        /// <summary>
        /// Compares this ObjectId to another object.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>True if the other object is an ObjectId and equal to this one.</returns>
        public override bool Equals(object obj)
        {
            if (obj is MongoID)
            {
                return Equals((MongoID)obj);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            int hash = 17;
            hash = 37 * hash + _a.GetHashCode();
            hash = 37 * hash + _b.GetHashCode();
            hash = 37 * hash + _c.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            //return String.Format("{0}{1}{2}", this._a.ToString("X10"), this._b.ToString("X8"), this._c.ToString("X8"));
            return this._a.ToString("X10") + this._b.ToString("X8") + this._c.ToString("X8");
        }
    }
}
