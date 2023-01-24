namespace Sequence_Alignment
{
    internal class NeedlemanWunsch : BaseAlgorithm
    {
        public NeedlemanWunsch() : base()
        {
        }

        public void FillMatrix()
        {
            for (int i = 0; i <= FirstSequence.Length ; i++)
            {
                if (i == 0)
                {
                    scoreMatrix[i, 0] = 0;//SimilarityMatrix[Nucleotides[FirstSequence[i]], Nucleotides['_']];
                }
                else
                {
                    scoreMatrix[i, 0] = scoreMatrix[i - 1, 0] + SimilarityMatrix[Nucleotides[FirstSequence[i-1]], Nucleotides['_']];
                }
            }

            for (int j = 0; j <= SecondSequence.Length; j++)
            {
                if (j == 0)
                {
                    scoreMatrix[0, j] = 0;// SimilarityMatrix[Nucleotides['_'], Nucleotides[SecondSequence[j]]];
                }
                else
                {
                    scoreMatrix[0, j] = scoreMatrix[0, j - 1] + SimilarityMatrix[Nucleotides['_'], Nucleotides[SecondSequence[j-1]]];
                }
            }

            for (int i = 1; i <= FirstSequence.Length; i++)
            {
                char sequenceOneChar = FirstSequence[i - 1];
                for (int j = 1; j <= SecondSequence.Length; j++)
                { 
                    char sequenceTwoChar = SecondSequence[j - 1];
                    int match = scoreMatrix[i - 1, j - 1] + SimilarityMatrix[Nucleotides[sequenceOneChar], Nucleotides[sequenceTwoChar]];
                    int delete = scoreMatrix[i - 1, j] + SimilarityMatrix[Nucleotides[sequenceOneChar], Nucleotides['_']];
                    int insert = scoreMatrix[i, j - 1] + SimilarityMatrix[Nucleotides['_'], Nucleotides[sequenceTwoChar]];
                    scoreMatrix[i, j] = Math.Max(Math.Max(delete, insert), match);
                }
            }
        }

        public void AlignSequences()
        {
            string firstAlignedSequence = "";
            string secondAlignedSequence = "";
            int i = FirstSequence.Length;
            int j = SecondSequence.Length;

            while (i > 0 || j > 0)
            {
                if (i > 0 && j > 0 )
                {
                    char sequenceOneChar = FirstSequence[i - 1];
                    char sequenceTwoChar = SecondSequence[j - 1];
                    if (scoreMatrix[i, j] == scoreMatrix[i - 1, j - 1] + SimilarityMatrix[Nucleotides[sequenceOneChar], Nucleotides[sequenceTwoChar]])
                    {
                        firstAlignedSequence = sequenceOneChar + firstAlignedSequence;
                        secondAlignedSequence = sequenceTwoChar + secondAlignedSequence;
                        i--;
                        j--;
                        continue;
                    }
                }
                if (i > 0 && scoreMatrix[i, j] == scoreMatrix[i - 1, j] + DistanceMatrix[Nucleotides[FirstSequence[i-1]], Nucleotides['_']])
                {
                    firstAlignedSequence = FirstSequence[i-1] + firstAlignedSequence;
                    secondAlignedSequence = "_" + secondAlignedSequence;
                    i--;
                }
                else
                {
                    char sequenceTwoChar = SecondSequence[j - 1];
                    firstAlignedSequence = "_" + firstAlignedSequence;
                    secondAlignedSequence = sequenceTwoChar + secondAlignedSequence;
                    j--;
                }
            }
            FirstAlignedSequence = firstAlignedSequence;
            SecondAlignedSequence = secondAlignedSequence;
        }

        public void PrintResults()
        {
            Console.WriteLine("Sequences to allign:");
            Console.WriteLine(FirstSequence);
            Console.WriteLine(SecondSequence);

            Console.WriteLine("\nSolution matrix:");

            for (int i = 0; i < scoreMatrix.GetLength(0); i++)
            {
                if (i == 0)
                {
                    Console.Write("\t\t");
                    
                    for (int j = 0; j < scoreMatrix.GetLength(1); j++)
                    {
                        if (j != 0)
                        {
                            Console.Write(SecondSequence[j - 1] + "\t");
                        }
                    }

                    Console.WriteLine();
                }

                if (i != 0)
                {
                    Console.Write(FirstSequence[i - 1]);
                }

                Console.Write("\t");

                for (int j = 0; j < scoreMatrix.GetLength(1); j++)
                {
                    Console.Write(scoreMatrix[i, j] + "\t");
                }

                Console.WriteLine();
            }

            Console.WriteLine("\nGlobal Optimal Alignment:");

            Console.WriteLine(FirstAlignedSequence);
            Console.WriteLine(SecondAlignedSequence);

            Console.WriteLine("\nSimilarity: " + scoreMatrix[scoreMatrix.GetLength(0) - 1, scoreMatrix.GetLength(1) - 1]);
            Console.WriteLine("\nEdit distance: " + editDistance);
        }

        public void Run()
        {
            FillMatrix();
            AlignSequences();
            CalculateSimilarityAndEditDistance();
            PrintResults();
        }
    }
}