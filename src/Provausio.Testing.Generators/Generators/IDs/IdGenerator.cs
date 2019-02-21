using System;
using SimpleBase;
using XidNet;

namespace Provausio.Testing.Generators.Generators.IDs
{
    internal class IdGenerator
    {
        public string Generate(IdType type = IdType.Xid)
        {
            var id = string.Empty;
            switch (type)
            {
                case IdType.Integer:
                    id = GenerateInteger;
                    break;
                case IdType.Guid:
                    id = GenerateGuid;
                    break;
                case IdType.Xid:
                    id = GenerateXid;
                    break;
                case IdType.Base58:
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

    public enum IdType
    {
        Integer,
        Guid,
        Xid,
        Base58
    }
}
