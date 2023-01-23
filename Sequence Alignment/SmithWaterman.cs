namespace Sequence_Alignment
{
    internal class SmithWaterman : BaseAlgorithm
    {
        public enum TraceEnum
        {
            Stop = 0,
            Left = 1,
            Up = 2,
            Diagonal = 3
        }

        public SmithWaterman() : base()
        {
        }

        public void AlignSequences()
        {
            int maxScore = -1;
            int maxRowIndex = -1;
            int maxColumnIndex = -1;
            for (int i = 1; i < scoreMatrix.GetLength(0); i++)
            {
                for (int j = 1; j < scoreMatrix.GetLength(1); j++)
                {
                    int matchValue;
                    int gapValue;
                    matchValue = SimilarityMatrix[Nucleotides[FirstSequence[i - 1]], Nucleotides[SecondSequence[j - 1]]];
                    gapValue = SimilarityMatrix[Nucleotides['_'], Nucleotides[SecondSequence[j - 1]]];

                    int diagonalScore = scoreMatrix[i - 1, j - 1] + matchValue;
                    int verticalScore = scoreMatrix[i - 1, j] + gapValue;
                    int horizontalScore = scoreMatrix[i, j - 1] + gapValue;

                    scoreMatrix[i, j] = Math.Max(Math.Max(diagonalScore, verticalScore), horizontalScore);

                    if (scoreMatrix[i, j] == 0)
                    {
                        tracingMatrix[i, j] = (int)TraceEnum.Stop;
                    }
                    else if (scoreMatrix[i, j] == horizontalScore)
                    {
                        tracingMatrix[i, j] = (int)TraceEnum.Left;
                    }
                    else if (scoreMatrix[i, j] == verticalScore)
                    {
                        tracingMatrix[i, j] = (int)TraceEnum.Up;
                    }
                    else if (scoreMatrix[i, j] == diagonalScore)
                    {
                        tracingMatrix[i, j] = (int)TraceEnum.Diagonal;
                    }
                    if (scoreMatrix[i, j] >= maxScore)
                    {
                        maxRowIndex = i;
                        maxColumnIndex = j;
                        maxScore = scoreMatrix[i, j];
                    }
                }
            }
            string alignedSequenceOne = string.Empty;
            string alignedSequenceTwo = string.Empty;
            string currentAlignedSequenceOne = string.Empty;
            string currentAlignedSequenceTwo = string.Empty;
            int maxRow = maxRowIndex;
            int maxColumn = maxColumnIndex;

            while (tracingMatrix[maxRow, maxColumn] != (int)TraceEnum.Stop)
            {
                if (tracingMatrix[maxRow, maxColumn] == (int)TraceEnum.Diagonal)
                {
                    currentAlignedSequenceOne = FirstSequence[maxRow - 1].ToString();
                    currentAlignedSequenceTwo = SecondSequence[maxColumn - 1].ToString();
                    maxRow--;
                    maxColumn--;
                }
                else if (tracingMatrix[maxRow, maxColumn] == (int)TraceEnum.Up)
                {
                    currentAlignedSequenceOne = FirstSequence[maxRow - 1].ToString();
                    currentAlignedSequenceTwo = "_";
                    maxRow--;
                }
                else if (tracingMatrix[maxRow, maxColumn] == (int)TraceEnum.Left)
                {
                    currentAlignedSequenceOne = "_";
                    currentAlignedSequenceTwo = SecondSequence[maxColumn - 1].ToString();
                    maxColumn--;
                }
                alignedSequenceOne += currentAlignedSequenceOne;
                alignedSequenceTwo += currentAlignedSequenceTwo;
            }
            AlignedSequences = new AlignedSequencePair
            {
                FirstAlignedSequence = alignedSequenceOne.Reverse(),
                SecondAlignedSequence = alignedSequenceTwo.Reverse()
            };
        }

        public void PrintResults()
        {
            Console.WriteLine("Solution matrix:");

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

            Console.WriteLine("\nLocal Optimal Alignment:");

            Console.WriteLine(AlignedSequences.FirstAlignedSequence);
            Console.WriteLine(AlignedSequences.SecondAlignedSequence);

            Console.WriteLine("\nSimilarity: " + similarity);
            Console.WriteLine("\nEdit distance: " + editDistance);
        }

        public void Run()
        {
            AlignSequences();
            CalculateSimilarityAndEditDistance();
            PrintResults();
        }
    }
}