using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ass2Security
{
    class Program
    {
        static void Main(string[] args)
        {
            
            List<byte[,]> in1 = getBytesFromFile(@"outputs/ofinal");
            List<byte[,]> in2 = getBytesFromFile(@"outputs/message");

            for (int t = 0; t < in1.Count; t++)
            {
                byte[,] temp1 = in1[t];
                byte[,] temp2 = in2[t];
                for (int j = 0; j < temp1.GetLength(0); j++)
                {
                    for (int k = 0; k < temp1.GetLength(1); k++)
                    {
                        if (temp1[j, k] != temp2[j, k])
                            Console.WriteLine("deiff");
                    }
                }
            }
            Console.WriteLine("done");
            Console.ReadKey();
            

            bool bug = false;
            bool breaking = false;
            for (int k = 0; k < args.Length; k++)
            {
                if (args[k] == "-b")
                {
                    breaking = true;
                    break;
                }
            }

            #region aes
            int i=0;
            bool found=false;

            //finind aes1/aes3
            AES aes = new AES1();
            while (i < args.Length && !found)
            {
                if (args[i] == "-a")
                {
                    if (i + 1 >= args.Length)
                        bug = true;
                    else if (args[i + 1] == "AES1")
                    {
                        aes = new AES1();
                    }
                    else if (args[i + 1] == "AES3") aes = new AES3();
                    else bug = true;
                    found = true;
                }
                i ++;
            }
            
            #endregion

            #region output path
            i = 0;
            found = false;

            //find path
            String path = "";
            while (i < args.Length && !found)
            {
                if (args[i] == "-o")
                {
                    if (i + 1 >= args.Length)
                        bug = true;
                    else
                    {
                        path = args[i + 1];
                        found = true;
                    }
                }
                i++;
            }
            List<byte[,]> result = new List<byte[,]>(); 
            #endregion

            #region encrypt/decrypt
            if (!breaking)
            {
                i = 0;
                found = false;

                //finding key
                List<byte[,]> key = new List<byte[,]>(); ;
                while (i < args.Length && !found)
                {
                    if (args[i] == "-k")
                    {
                        if (i + 1 >= args.Length)
                            bug = true;
                        else
                        {
                             key = getBytesFromFile(args[i + 1]);
                             found = true;
                        }
                    }
                    i++;
                }
                if (key==null || key.Count==0) bug=true;
                if (!bug)
                {
                    if (aes is AES3)
                      ((AES3)(aes)).setKey(key);
                    else 
                      aes.setKey(key[0]);
                }

                i = 0;
                found = false;

                //finding plaintext/cipher
                List<byte[,]> input = new List<byte[,]>(); ;
                while (i < args.Length && !found)
                {
                    if (args[i] == "-i")
                    {
                        if (i + 1 >= args.Length)
                            bug = true;
                        else
                        {
                            input = getBytesFromFile(args[i + 1]);
                            found = true;
                        }
                    }
                    i++;
                }
                if (input == null || input.Count==0) bug = true;

                i = 0;
                found = false;

                if (!bug)
                {
                    //encrypt/decrypt
                    while (i < args.Length && !found)
                    {
                        if (args[i] == "-d")
                        {
                            result = aes.decrypt(input);
                            found = true;
                        }
                        else if (args[i] == "-e")
                        {
                            result = aes.encrypt(input);
                            found = true;
                        }
                        i++;
                    }
                }

            }
            #endregion
            #region breaking
            else
            {
                i = 0;
                found = false;

                //plaintext
                List<byte[,]> plaintext = new List<byte[,]>(); ;
                while (i < args.Length && !found)
                {
                    if (args[i] == "-m")
                    {
                        if (i + 1 >= args.Length)
                            bug = true;
                        else
                        {
                            plaintext = getBytesFromFile(args[i + 1]);
                            found = true;
                        }
                    }
                    i++;
                }
                if (plaintext == null || plaintext.Count==0) bug = true;

                i = 0;
                found = false;

                //plaintext
                List<byte[,]> cipher = new List<byte[,]>(); ;
                while (i < args.Length && !found)
                {
                    if (args[i] == "-c")
                    {
                        if (i + 1 >= args.Length)
                            bug = true;
                        else
                        {
                            cipher = getBytesFromFile(args[i + 1]);
                            found = true;
                        }
                    }
                    i++;
                }
                if (cipher == null || cipher.Count==0) bug = true;
                if (!bug)
                   result = aes.getKey(plaintext, cipher);
            }
            
            
            #endregion

            if (!bug)
            {
                writeBytesToFile(path, result);
            }
            else Console.WriteLine("bug");
        //    Console.ReadKey();
        }

        public static List<byte[,]> getBytesFromFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);

            StreamReader sr = new StreamReader(fs);
            List<byte[,]> listOfBytesMatrix = new List<byte[,]>();

            int numOfMatrix = (int)fs.Length / 16;


            for (int k = 0; k < numOfMatrix; k++)
            {
                listOfBytesMatrix.Add(new byte[4, 4]);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        listOfBytesMatrix[k][j, i] = (byte)fs.ReadByte();
                    }
                }
            }
            return listOfBytesMatrix;
        }

        public static void writeBytesToFile(string path, List<byte[,]> output)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);

            StreamWriter sw = new StreamWriter(fs);

            for (int k = 0; k < output.Count; k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        fs.WriteByte(output[k][j, i]);
                    }
                }
            }
        }

    /*    public static List<bool[,]>[,] compareTwo16CellsKeyMatrix(List<bool[,]>[,] key1, List<bool[,]>[,] key2)
        {
            List<bool[,]>[,] intersectionKey = new List<bool[,]>[4, 4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    foreach (bool[,] cellOption1 in key1[i, j])
                    {
                        foreach (bool[,] cellOption2 in key2[i, j])
                        {
                            if (compareTwo3KeysMatrix3X8(cellOption1, cellOption2))
                                intersectionKey[i, j].Add(cellOption1);
                        }
                    }
                }
            }



            return intersectionKey;
        }
    
        public static bool compareTwo3KeysMatrix3X8(bool[,] key1, bool[,] key2)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (key1[i, j] != key2[i, j])
                        return false;
                }
            }

            return true;

        }

        */
    }
}
