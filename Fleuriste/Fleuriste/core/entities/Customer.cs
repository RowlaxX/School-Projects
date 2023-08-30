using BDD.Core.Attributes;
using BDD.Core.Relations;
using System;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Media.Animation;

namespace BDD.Core.Entities
{
    [Table("customer")]
    public class Customer : Entity
    {
        private Customer() { }

        [Column("name")]
        public string? Name { get; private set; }
        [Column("firstname")]
        public string? FirstName { get; private set; }
        [Column("phone")]
        public string? Phone { get; private set; }
        [Column("email")]
        public string? Email { get; private set; }
        [Column("password")]
        public byte[]? Password { get; private set; }
        [Column("salt")]
        public byte[]? Salt { get; private set; }

        [OneToMany("idCustomer")]
        public OneToManyRelation<Order> Orders { get; private set; }
        [OneToMany("idCustomer")]
        public OneToManyRelation<Address> Addresses { get; private set; }

        private static readonly int KEY_SIZE = 64;
        private static readonly int ITERATION = 50000;

        private static byte[] Hash(byte[] salt, string tohash)
        {
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(tohash), salt, ITERATION, HashAlgorithmName.SHA512, KEY_SIZE);
        }

        public void SetPassword(string password)
        {
            Edit("salt", RandomNumberGenerator.GetBytes(KEY_SIZE));
            Edit("password", Hash(Salt, password));
        }

        public bool TestPassword(string password)
        {
            if (Password == null || Salt == null)
                return false;

            return Password.SequenceEqual(Hash(Salt, password));
        }
    }
}
