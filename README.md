[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/samuel-lucas6/Committing-ChaCha20-Poly1305/blob/main/LICENSE)

# Committing-ChaCha20-Poly1305
A committing ChaCha20-Poly1305 implementation using [libsodium](https://doc.libsodium.org/). This library supports both [ChaCha20](https://doc.libsodium.org/advanced/stream_ciphers/chacha20) and [XChaCha20](https://doc.libsodium.org/advanced/stream_ciphers/xchacha20).

## Should I use this?
**No**, this is just a demo. This implementation does not even support the IETF version of ChaCha20-Poly1305. 

If you want key commitment, then you should **not** be using ChaCha20-Poly1305 or XChaCha20-Poly1305. Instead, you should use Encrypt-then-MAC with [HMAC](https://tools.ietf.org/html/rfc2104), [BLAKE2](https://www.blake2.net/), or [BLAKE3](https://github.com/BLAKE3-team/BLAKE3). An example of how to do this can be found [here](https://github.com/samuel-lucas6/ChaCha20-BLAKE2b).
