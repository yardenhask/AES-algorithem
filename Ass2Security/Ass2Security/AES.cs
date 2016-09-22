using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ass2Security
{
    abstract class AES
    {

        public byte[,] key;




/*
        public byte[,] generateRandomKeyMatrix()
        {
            byte[,] res = new byte[4, 4];

            Random rand = new Random();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    res[i, j] = Convert.ToByte(rand.Next(0, 255));

            return res;


        }*/

        public abstract List<byte[,]> encrypt(List<byte[,]> input);

        public abstract List<byte[,]> decrypt(List<byte[,]> cipher);
        public abstract List<byte[,]> getKey(List<byte[,]> plaintext, List<byte[,]> cipher);

        public byte[,] subBytes(byte[,] input)
        {
            byte[,] output = new byte[input.GetLength(0), input.GetLength(1)];
            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
              /*      int x = Int32.Parse((BitConverter.ToString(new byte[] { input[i, j] }).ToCharArray()[0]).ToString(), System.Globalization.NumberStyles.HexNumber);
                    int y = Int32.Parse((BitConverter.ToString(new byte[] { input[i, j] }).ToCharArray()[1]).ToString(), System.Globalization.NumberStyles.HexNumber);
                    output[i, j] = (byte)RijndaelTables.S_BOX[x, y];*/
                    output[i, j] = (byte)RijndaelTables.S_BOX[(input[i, j] >> 4), (input[i, j] & 0x0f)];
                }
            }
            return output;
        }

        public byte[,] subBytesInv(byte[,] input)
        {
            byte[,] output = new byte[input.GetLength(0), input.GetLength(1)];
            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    int x = Int32.Parse((BitConverter.ToString(new byte[] { input[i, j] }).ToCharArray()[0]).ToString(), System.Globalization.NumberStyles.HexNumber);
                    int y = Int32.Parse((BitConverter.ToString(new byte[] { input[i, j] }).ToCharArray()[1]).ToString(), System.Globalization.NumberStyles.HexNumber);
                    output[i, j] = (byte)RijndaelTables.INVERSE_S_BOX[x, y];
                }
            }
            return output;
        }

        public byte[,] shiftRows(byte[,] input)
        {
            byte[,] output = new byte[input.GetLength(0), input.GetLength(1)];
            for (int i = 0; i < input.GetLength(0); i++)
            {
                int k = 0;
                for (int j = i; j < input.GetLength(1); j++)
                {
                    output[i, k] = input[i, j];
                    k++;
                }
                for (int j = 0; j < i; j++)
                {
                    output[i, k] = input[i, j];
                    k++;
                }
            }
            return output;
        }

        public byte[,] shiftRowsInv(byte[,] input)
        {
            byte[,] output = new byte[input.GetLength(0), input.GetLength(1)];
            for (int i = 0; i < input.GetLength(0); i++)
            {
                int k =0;
                for (int j = input.GetLength(1) - i ; j < input.GetLength(1); j++)
                {
                    output[i, k] = input[i, j];
                    k++;
                }
                for (int j = 0; j < input.GetLength(1) - i; j++)
                {
                    output[i, k] = input[i, j];
                    k++;
                }
                
                
            }
            return output;
        }

        public byte[,] mixColumns(byte[,] input)
        {
            byte[,] output = new byte[input.GetLength(0), input.GetLength(1)];
            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    byte a = multTwoBytes(RijndaelTables.mixColumnsMultMatrix[i, 0], input[0, j]);
                    byte b = multTwoBytes(RijndaelTables.mixColumnsMultMatrix[i, 1], input[1, j]);
                    byte c = multTwoBytes(RijndaelTables.mixColumnsMultMatrix[i, 2], input[2, j]);
                    byte d = multTwoBytes(RijndaelTables.mixColumnsMultMatrix[i, 3], input[3, j]);
                    output[i, j] = (byte)(a ^ b ^ c ^ d);
                }
            }
            return output;
        }

      
        public byte[,] mixColumnsInv(byte[,] input)
        {
            byte[,] output = new byte[input.GetLength(0), input.GetLength(1)];
            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    byte a = multTwoBytes(RijndaelTables.mixColumnsMultMatrix_inv[i, 0], input[0, j]);
                    byte b = multTwoBytes(RijndaelTables.mixColumnsMultMatrix_inv[i, 1], input[1, j]);
                    byte c = multTwoBytes(RijndaelTables.mixColumnsMultMatrix_inv[i, 2], input[2, j]);
                    byte d = multTwoBytes(RijndaelTables.mixColumnsMultMatrix_inv[i, 3], input[3, j]);
                    output[i, j] = (byte)(a ^ b ^ c ^ d);
                }
            }
            return output;
        }

        public byte multTwoBytes(byte b1, byte b2)
        {
            if (b1 == 1)
            {
                return b2;
            }
            else if (b1 == 2)
            {
                return RijndaelTables.RF_M2[(int)b2];
            }
            else if (b1 == 3)
            {
                return RijndaelTables.RF_M3[(int)b2];
            }
            else if (b1 == 9)
            {
                return RijndaelTables.RF_M9[(int)b2];
            }
            else if (b1 == 11)
            {
                return RijndaelTables.RF_M11[(int)b2];
            }
            else if (b1 == 13)
            {
                return RijndaelTables.RF_M13[(int)b2];
            }
            else if (b1 == 14)
            {
                return RijndaelTables.RF_M14[(int)b2];
            }
            return 1;

        }

        public byte[,] addRoundKey(byte[,] input)
        {
            byte[,] output = new byte[input.GetLength(0), input.GetLength(1)];

            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    output[i, j] = (byte)(input[i, j] ^ key[i, j]);
                }
            }

            return output;

        }

        public byte[,] twoMatrixXor(byte[,] m1, byte[,] m2)
        {
            byte[,] output = new byte[m1.GetLength(0), m1.GetLength(1)];

            for (int i = 0; i < m1.GetLength(0); i++)
            {
                for (int j = 0; j < m1.GetLength(1); j++)
                {
                    output[i, j] = (byte)(m1[i, j] ^ m2[i, j]);
                }
            }

            return output;
        }

        public void setKey(byte[,] key)
        {
            this.key = key;
        }
    }
}
