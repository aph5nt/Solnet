using System;
using Solnet.KeyStore.Exceptions;
using Solnet.KeyStore.Serialization;
using Solnet.KeyStore.Model;
using Solnet.Util;

namespace Solnet.KeyStore.Services
{
    public class KeyStorePbkdf2Service : KeyStoreServiceBase<Pbkdf2Params>
    {
        public const string KdfType = "pbkdf2";

        public KeyStorePbkdf2Service()
        {
        }

        public KeyStorePbkdf2Service(IRandomBytesGenerator randomBytesGenerator, KeyStoreCrypto keyStoreCrypto) : base(
            randomBytesGenerator, keyStoreCrypto)
        {
        }

        public KeyStorePbkdf2Service(IRandomBytesGenerator randomBytesGenerator) : base(randomBytesGenerator)
        {
        }

        protected override byte[] GenerateDerivedKey(string password, byte[] salt, Pbkdf2Params kdfParams)
        {
            return KeyStoreCrypto.GeneratePbkdf2Sha256DerivedKey(password, salt, kdfParams.Count, kdfParams.Dklen);
        }

        protected override Pbkdf2Params GetDefaultParams()
        {
            return new Pbkdf2Params{Dklen = 32, Count = 262144, Prf = "hmac-sha256"};
        }

        public override KeyStore<Pbkdf2Params> DeserializeKeyStoreFromJson(string json)
        {
            return JsonKeyStorePbkdf2Serialiser.DeserialisePbkdf2(json);
        }

        public override string SerializeKeyStoreToJson(KeyStore<Pbkdf2Params> keyStore)
        {
            return JsonKeyStorePbkdf2Serialiser.SerialisePbkdf2(keyStore);
        }

        public override byte[] DecryptKeyStore(string password, KeyStore<Pbkdf2Params> keyStore)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (keyStore == null) throw new ArgumentNullException(nameof(keyStore));

            return KeyStoreCrypto.DecryptPbkdf2Sha256(password, keyStore.Crypto.Mac.HexToByteArray(),
                keyStore.Crypto.CipherParams.Iv.HexToByteArray(),
                keyStore.Crypto.CipherText.HexToByteArray(),
                keyStore.Crypto.Kdfparams.Count,
                keyStore.Crypto.Kdfparams.Salt.HexToByteArray(),
                keyStore.Crypto.Kdfparams.Dklen);
        }

        public override string GetKdfType()
        {
            return KdfType;
        }
    }
}