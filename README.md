[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/samuel-lucas6/Committing-ChaCha20-Poly1305/blob/main/LICENSE)

# Committing-ChaCha20-Poly1305
Several committing ChaCha20-Poly1305 implementations made using [libsodium](https://doc.libsodium.org/). There are examples for both [ChaCha20](https://doc.libsodium.org/advanced/stream_ciphers/chacha20) and [XChaCha20](https://doc.libsodium.org/advanced/stream_ciphers/xchacha20).

## Should I use any of these?
Not yet because the [libsodium-core](https://github.com/tabrath/libsodium-core) library is no longer being maintained and contains a nonce size [error](https://github.com/tabrath/libsodium-core/blob/master/src/Sodium.Core/SecretAeadChaCha20Poly1305IETF.cs) for the IETF version of ChaCha20-Poly1305, which makes it unusable. I am working on my own libsodium binding called [Geralt](https://github.com/samuel-lucas6/Geralt) that will eventually replace libsodium-core and fix this problem.

However, in the meantime, you should be able to use this code to produce your own committing ChaCha20-Poly1305 implementation.

## How do these implementations work?
- Method 1 involves using ChaCha20/XChaCha20 and Poly1305 separately with two distinct keys derived from the same master key. This means that the IETF version of ChaCha20-Poly1305 is not supported. **This method is not recommended.**
- Method 2 also derives two separate keys from the same master key but prepends a BLAKE2b 'robustness tag' to the ciphertext. This tag gets verified before decryption. **This is the recommended approach.**
- Method 3 employs the same method as method 2 but uses a single key for both encryption and authentication, which avoids the need for key derivation but is generally not considered good practice.

## Any advice on key commitment?
If you want key commitment, then you should use Encrypt-then-MAC with [HMAC](https://tools.ietf.org/html/rfc2104), [BLAKE2](https://www.blake2.net/), or [BLAKE3](https://github.com/BLAKE3-team/BLAKE3). BLAKE2 is your best bet because it is faster than HMAC and has a higher security margin than BLAKE3. An example of how to do this can be found [here](https://github.com/samuel-lucas6/ChaCha20-BLAKE2b).
