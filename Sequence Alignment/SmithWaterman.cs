﻿namespace Sequence_Alignment
{
    internal class SmithWaterman : BaseAlgorithm
    {
        public enum ScoreEnum
        {
            Match = 1,
            Mismatch = -1,
            Gap = -1
        }

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
            int matchValue = 0;

            for (int i = 1; i < scoreMatrix.GetLength(0); i++)
            {
                for (int j = 1; j < scoreMatrix.GetLength(1); j++)
                {
                    if (FirstSequence[i - 1] == SecondSequence[j - 1])
                    {
                        matchValue = (int)ScoreEnum.Match;
                    }
                    else
                    {
                        matchValue = (int)ScoreEnum.Mismatch;
                    }
                    int diagonalScore = scoreMatrix[i - 1, j - 1] + matchValue;
                    int verticalScore = scoreMatrix[i - 1, j] + (int)ScoreEnum.Gap;
                    int horizontalScore = scoreMatrix[i, j - 1] + (int)ScoreEnum.Gap;

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
                alignedSequenceOne = alignedSequenceOne + currentAlignedSequenceOne;
                alignedSequenceTwo = alignedSequenceTwo + currentAlignedSequenceTwo;
            }
            AlignedSequences = new AlignedSequencePair
            {
                FirstAlignedSequence = alignedSequenceOne.Reverse(),
                SecondAlignedSequence = alignedSequenceTwo.Reverse()
            };
        }

        public void PrintResults()
        {
            // Print all possible alignment
            Console.WriteLine("\nLocal Optimal Alignment:");

            Console.WriteLine(AlignedSequences.FirstAlignedSequence);
            Console.WriteLine(AlignedSequences.SecondAlignedSequence);

            Console.WriteLine("----------");
            Console.WriteLine("\nSimilarity: " + similarity);
            Console.WriteLine("\nEdit distance: " + editDistance);
            Console.WriteLine("----------");
        }

        public void Run()
        {
            AlignSequences();
            CalculateSimilarityAndEditDistance();
            PrintResults();
        }
    }
}