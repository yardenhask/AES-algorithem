using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ass2Security
{
    class AES1 : AES
    {

        public AES1(/*byte[,] key*/)
        {
            this.key = key;
        }
        public byte[,] encrypt(byte[,] input)
        {

            return addRoundKey(mixColumns(shiftRows(subBytes(input))));
        }

        public override List<byte[,]> encrypt(List<byte[,]> input)
        {
            List<byte[,]> cipher = new List<byte[,]>();
            for (int i = 0; i < input.Count; i++)
            {
                cipher.Add(encrypt(input[i]));
            }
            return cipher;
        }
        public override List<byte[,]> getKey(List<byte[,]> plaintext, List<byte[,]> cipher)
        {
            List<byte[,]> ans = new List<byte[,]>();
            ans.Add(getKey(plaintext[0], cipher[0]));
            return ans;
        }
        public byte[,] getKey(byte[,] plaintext, byte[,] cipher)
        {
            return twoMatrixXor((mixColumns(shiftRows(subBytes(plaintext)))), cipher);
        }

        public byte[,] decrypt(byte[,] cipher)
        {
            return subBytesInv(shiftRowsInv(mixColumnsInv(twoMatrixXor(cipher, key))));
        }

        public override List<byte[,]> decrypt(List<byte[,]> cipher)
        {
            List<byte[,]> ans = new List<byte[,]>();
            for (int i = 0; i < cipher.Count; i++)
            {
                ans.Add(decrypt(cipher[i]));
            }
            return ans;
        }

    }
}
