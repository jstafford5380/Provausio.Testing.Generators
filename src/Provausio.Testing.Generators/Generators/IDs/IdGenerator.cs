using System;
using SimpleBase;
using XidNet;

namespace Provausio.Testing.Generators.Generators.IDs
{
    internal class IdGenerator
    {
        public string Generate(IdFormat format = IdFormat.Xid)
        {
            var id = string.Empty;
            switch (format)
            {
                case IdFormat.Integer:
                    id = GenerateInteger;
                    break;
                case IdFormat.Guid:
                    id = GenerateGuid;
                    break;
                case IdFormat.Xid:
                    id = GenerateXid;
                    break;
                case IdFormat.Base58:
                    id = GenerateBase58;
                    break;
            }

            return id;
        }

        private string GenerateGuid => Guid.NewGuid().ToString();

        private string GenerateXid => Xid.NewXid().ToString();

        private string GenerateBase58 => Base58.Ripple.Encode(Guid.NewGuid().ToByteArray());

        private string GenerateInteger => Math.Abs(Xid.NewXid().GetHashCode() % 0xF4240).ToString();
    }

    public enum IdFormat
    {
        Integer,
        Guid,
        Xid,
        Base58
    }
}
