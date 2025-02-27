// unset

using Solnet.Wallet.Utilities;
using System;
using System.Diagnostics;

namespace Solnet.Wallet
{
    /// <summary>
    /// Implements the private key functionality.
    /// </summary>
    [DebuggerDisplay("Key = {" + nameof(Key) + "}")]
    public class PrivateKey
    {
        /// <summary>
        /// Private key length.
        /// </summary>
        private const int PrivateKeyLength = 64;

        private string _key;

        /// <summary>
        /// The key as base-58 encoded string.
        /// </summary>
        public string Key
        {
            get
            {
                if (_key == null)
                {
                    Key = Encoders.Base58.EncodeData(KeyBytes);
                }
                return _key;
            }
            set { _key = value; }
        }


        private byte[] _keyBytes;

        /// <summary>
        /// The bytes of the key.
        /// </summary>
        public byte[] KeyBytes
        {
            get
            {
                if (_keyBytes == null)
                {
                    KeyBytes = Encoders.Base58.DecodeData(Key);
                }
                return _keyBytes;
            }
            set { _keyBytes = value; }
        }


        /// <summary>
        /// Initialize the public key from the given byte array.
        /// </summary>
        /// <param name="key">The public key as byte array.</param>
        public PrivateKey(byte[] key)
        {
            if (key.Length != PrivateKeyLength)
                throw new ArgumentException("invalid key length", nameof(key));
            KeyBytes = new byte[PrivateKeyLength];
            Array.Copy(key, KeyBytes, PrivateKeyLength);
        }

        /// <summary>
        /// Initialize the public key from the given string.
        /// </summary>
        /// <param name="key">The public key as base58 encoded string.</param>
        public PrivateKey(string key)
        {
            Key = key;
        }

        /// <summary>
        /// Initialize the public key from the given string.
        /// </summary>
        /// <param name="key">The public key as base58 encoded string.</param>
        public PrivateKey(ReadOnlySpan<byte> key)
        {
            if (key.Length != PrivateKeyLength)
                throw new ArgumentException("invalid key length", nameof(key));
            KeyBytes = new byte[PrivateKeyLength];
            key.CopyTo(KeyBytes.AsSpan());
        }

        /// <summary>
        /// Conversion between a <see cref="PrivateKey"/> object and the corresponding base-58 encoded private key.
        /// </summary>
        /// <param name="privateKey">The PrivateKey object.</param>
        /// <returns>The base-58 encoded private key.</returns>
        public static implicit operator string(PrivateKey privateKey) => privateKey.Key;
        
        /// <summary>
        /// Conversion between a base-58 encoded private key and the <see cref="PrivateKey"/> object.
        /// </summary>
        /// <param name="address">The base-58 encoded private key.</param>
        /// <returns>The PrivateKey object.</returns>
        public static explicit operator PrivateKey(string address) => new (address);
        
        /// <summary>
        /// Conversion between a <see cref="PrivateKey"/> object and the private key as a byte array.
        /// </summary>
        /// <param name="privateKey">The PrivateKey object.</param>
        /// <returns>The private key as a byte array.</returns>
        public static implicit operator byte[](PrivateKey privateKey) => privateKey.KeyBytes;
        
        /// <summary>
        /// Conversion between a private key as a byte array and the corresponding <see cref="PrivateKey"/> object.
        /// </summary>
        /// <param name="keyBytes">The private key as a byte array.</param>
        /// <returns>The PrivateKey object.</returns>
        public static explicit operator PrivateKey(byte[] keyBytes) => new (keyBytes);
    }
}