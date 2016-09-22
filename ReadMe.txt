AES1- one iteration of AES (message m-> subBytes-> shiftRows-> mixColumns-> addRoundKey)
AES3- three iterations of AES without performing mixColumns and subBytes.

Usage:
     Encryption/Decryption interface:
	 –a <AES1/AES3> : denotes the algorithm to use
	 –e : instruction to encrypt the input file
	 –d: instruction to decrypt the input file
	 –k <path>: path to the key(s) , the key should be 128 bit for aes1 or 384 bit (128*3) for aes3. The latter should be divided into 3 separate keys.
	 –i <input file path>: a path to a file we want to encrypt/decrypt
	 –o <output file path>: a path to the output file
         aes –a <AES1 or AES3> -e/-d –k <path-to-key-file > -i <path-to-input-file> -o <path-to-output-file>
     Hacking (breaking) interface:
	 –a <AES1/AES3> : denotes the algorithm to break
	 –b: instruction to break the encryption algorithm
	 –m <path>: denotes the path to the plain-text message
	 –c <path>: denotes the path to the cipher-text message
	 –o <path>: a path to the output file with the key(s) found.
         aes –a <AES1 or AES3> -b –m <path-to-message> –c <path-to-cipher> -o < output-path>
examples:
aes.exe  –a AES1 –b –m message.txt –c cypther.txt –o keyFound.txt
aes.exe –a AES3 –e –k key.txt –i message.txt –o cypther.txt
