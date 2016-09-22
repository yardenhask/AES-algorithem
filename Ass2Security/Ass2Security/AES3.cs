using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ass2Security
{
    class AES3 : AES
    {
        public List<byte[,]> keyList;

        public AES3(/*byte[,] key1, byte[,] key2, byte[,] key3*/)
        {

            keyList = new List<byte[,]>();

      /*      keyList.Add(key1);
            keyList.Add(key2);
            keyList.Add(key3);*/

        }

        public void setKey(List<byte[,]> keys)
        {
            this.keyList = keys;
        }

        public override List<byte[,]> encrypt(List<byte[,]> input)
        {
            List<byte[,]> cypher = new List<byte[,]>();
            for (int i = 0; i < input.Count; i++)
            {
                cypher.Add(encrypt(input[i]));
            }
            return cypher;
        }
        public byte[,] encrypt(byte[,] input)
        {

            byte[,] output = new byte[input.GetLength(0), input.GetLength(1)];

            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    output[i, j] = input[i, j];
                }
            }


            
            for (int i = 0; i < 3; i++)
            {
                this.key = keyList[i];
                output = addRoundKey(shiftRows(output));
            }

            return output;
        }

        //decrypt
        public override List<byte[,]> decrypt(List<byte[,]> cipher)
        {
            List<byte[,]> ans = new List<byte[,]>();
            for (int i = 0; i < cipher.Count; i++)
            {
                ans.Add(decrypt(cipher[i]));
            }
            return ans;
        }

        public byte[,] decrypt(byte[,] cipher)
        {
            byte[,] input = new byte[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    input[i, j] = cipher[i, j];
                }
            }
            for (int i = 0; i < 3; i++)
            {
                this.key = keyList[2-i];
                input = shiftRowsInv(addRoundKey(input));
            }
            return input;
        }

        public override List<byte[,]> getKey(List<byte[,]> plaintext, List<byte[,]> cipher)
        {
            List<byte[,]> keys = new List<byte[,]>();
            keys.Add(new byte[4, 4]);
            Random rand = new Random();
            for (int i = 0; i < plaintext[0].GetLength(0); i++)
            {
                for (int j = 0; j < plaintext[0].GetLength(1); j++)
                {
                    keys[0][i, j] = Convert.ToByte(rand.Next(0, 255));
                }
            }
            keys.Add(shiftRows(keys[0]));

            byte[,] afterFirstKey = twoMatrixXor(shiftRows(plaintext[0]), keys[0]);
            byte[,] afterSecondKey = twoMatrixXor(shiftRows(afterFirstKey), keys[1]);
            byte[,] afterThirdKey = twoMatrixXor(shiftRows(afterSecondKey),/*shiftRows*/(cipher[0]));
            keys.Add(afterThirdKey);

            return keys;
        }

    }
}
