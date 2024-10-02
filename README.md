# JWT Generator

Creates and validates a JWT token.

1. Generate private and public key pair
   1. Optionally use [jwkeygen.io](https://jwkeygen.io/)
   2. For the key parameters select SIGN / ASYMMETRIC / ECDSA / P-256 / include key Id
2. Add the private key to user secrets
    - `dotnet user-secrets set GreenTinOptions:PrivateKey "<PRIVATE_KEY>"`

## Terminology

### JWKS
JSON Web Key Sets is a standard for publishing and distributing cryptographic keys in a JSON format. JWKS facilitate the sharing of public keys that can be used to verify the signatures of JSON Web Tokens (JWTs) and encrypt or decrypt messages. Structure of a JWKS;
```json
{
  "keys": [
    {
      "kty": "RSA",
      "use": "sig",
      "kid": "12345",
      "alg": "RS256",
      "n": "xxxxx",
      "e": "AQAB"
    }
  ]
}
```

JWKSs are intended to be publicly hosted somewhere, e.g. https://my-proj.my-company.com/.well-known/jwks.json

### JWT
JSON Web Tokens let identity travel the web securely. A JWT has header, payload and signature parts, each is base 64 encoded and separated by dots. They are self-contained, portable and don't require server side storage. However they are vulnerable to theft, can provide full-access to resources if intercepted, and can suffer from poor performance due to large payload.
JWTs provide a scalable way to handle authentication, authorisation and information exchange.


#### Header
Typically consists of type and the signing algorithm;
```json
{ 
  "alg": "HS256", 
  "typ": "JWT"
}
```
#### Payload
```json
{ 
  "uid": "john.doe@larch.com", 
  "aud": "green-tin", 
  "issuer": "LarchInc.com", 
  "exp": "1727840445"
}
```
JWT payloads can be encrypted using JSON web encryption most implementations use signed but not encrypted tokens, so the data can be read if intercepted. Sensitive information should never travel in a JWT payload unless encrypted first.

#### Signature
To create the signature requires the header and payload, a secret and the algorithm.
Signing a token is done to ensure the token has not been tampered with, and can be done in two ways;
- *Symmetric algorithms* like HMAC or HS256 use a shared secret key for both signing and verification.
- *Asymmetric algorithms* such as RSA use a public/ private key pair where the private key signs the token and the public key verifies the token. Has the benefit of verifying the sender is who they say they are.
