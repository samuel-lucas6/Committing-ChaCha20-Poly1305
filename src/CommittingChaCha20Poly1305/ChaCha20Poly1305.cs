using System.Security.Cryptography;
using Sodium;

/*
    Committing ChaCha20-Poly1305: A committing AEAD implementation.
    Copyright (c) 2021 Samuel Lucas

    Permission is hereby granted, free of charge, to any person obtaining a copy of
    this software and associated documentation files (the "Software"), to deal in
    the Software without restriction, including without limitation the rights to
    use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
    the Software, and to permit persons to whom the Software is furnished to do so,
    subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

namespace CommittingChaCha20Poly1305
{
    public static class ChaCha20Poly1305
    {
        /// <summary>Encrypts a message using committing ChaCha20-Poly1305.</summary>
        /// <remarks>Never reuse a nonce with the same key. A counter nonce is strongly recommended.</remarks>
        /// <param name="message">The message to encrypt.</param>
        /// <param name="nonce">The 8 byte nonce.</param>
        /// <param name="key">The 32 byte key.</param>
        /// <param name="additionalData">Optional additional data to authenticate.</param>
        /// <returns>The ciphertext and tag.</returns>
        public static byte[] Encrypt(byte[] message, byte[] nonce, byte[] key, byte[] additionalData = null)
        {
            ParameterValidation.Message(message);
            ParameterValidation.Nonce(nonce, Constants.ChaChaNonceLength);
            ParameterValidation.Key(key, Constants.KeyLength);
            additionalData = ParameterValidation.AdditionalData(additionalData);
            (byte[] encryptionKey, byte[] macKey) = KeyDerivation.DeriveKeys(nonce, key);
            byte[] ciphertext = StreamEncryption.EncryptChaCha20(message, nonce, encryptionKey);
            byte[] tagMessage = Arrays.Concat(additionalData, ciphertext, BitConversion.GetBytes(additionalData.Length), BitConversion.GetBytes(ciphertext.Length));
            byte[] tag = OneTimeAuth.Sign(tagMessage, macKey);
            return Arrays.Concat(ciphertext, tag);
        }

        /// <summary>Decrypts a ciphertext message using committing ChaCha20-Poly1305.</summary>
        /// <param name="ciphertext">The ciphertext to decrypt.</param>
        /// <param name="nonce">The 8 byte nonce.</param>
        /// <param name="key">The 32 byte key.</param>
        /// <param name="additionalData">Optional additional data to authenticate.</param>
        /// <returns>The decrypted message.</returns>
        public static byte[] Decrypt(byte[] ciphertext, byte[] nonce, byte[] key, byte[] additionalData = null)
        {
            ParameterValidation.Ciphertext(ciphertext);
            ParameterValidation.Nonce(nonce, Constants.ChaChaNonceLength);
            ParameterValidation.Key(key, Constants.KeyLength);
            additionalData = ParameterValidation.AdditionalData(additionalData);
            (byte[] encryptionKey, byte[] macKey) = KeyDerivation.DeriveKeys(nonce, key);
            byte[] tag = Tag.Read(ciphertext);
            ciphertext = Tag.Remove(ciphertext);
            byte[] tagMessage = Arrays.Concat(additionalData, ciphertext, BitConversion.GetBytes(additionalData.Length), BitConversion.GetBytes(ciphertext.Length));
            byte[] computedTag = OneTimeAuth.Sign(tagMessage, macKey);
            bool validTag = Utilities.Compare(tag, computedTag);
            return !validTag ? throw new CryptographicException() : StreamEncryption.DecryptChaCha20(ciphertext, nonce, encryptionKey);
        }
    }
}
