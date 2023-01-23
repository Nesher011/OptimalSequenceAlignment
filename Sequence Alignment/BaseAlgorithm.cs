namespace Sequence_Alignment
{
    internal class BaseAlgorithm
    {
        public const string SequenceOneFileName = "Seq1.txt";
        public const string SequenceTwoFileName = "Seq2.txt";
        public const string SimilarityMatrixFileName = "SimMat.csv";
        public const string DistanceMatrixFileName = "DisMat.csv";

        public AlignedSequencePair AlignedSequences { get; set; }

        public string FirstSequence { get; set; } = string.Empty;
        public string SecondSequence { get; set; } = string.Empty;

        public IDictionary<char, int> Nucleotides = new Dictionary<char, int>
        {
            { 'A', 0 },
            {'C',1 },
            {'G',2 },
            {'T',3 },
            {'_',4 }
        };

        // MATRIX
        //   A C G T _
        // A
        // C
        // G
        // T
        // _

        public int[,] SimilarityMatrix;

        public int[,] DistanceMatrix;

        public string[][] matrix;

        public int[,] scoreMatrix;
        public int[,] tracingMatrix;

        public int editDistance = 0;
        public float similarity = 0;

        public BaseAlgorithm()
        {
            AlignedSequences = new AlignedSequencePair();
            SimilarityMatrix = new int[5, 5];
            DistanceMatrix = new int[5, 5];
            ReadSequencesFromFiles();
            scoreMatrix = new int[FirstSequence.Length + 1, SecondSequence.Length + 1];
            tracingMatrix = new int[FirstSequence.Length + 1, SecondSequence.Length + 1];
        }

        public void CalculateSimilarityAndEditDistance()
        {
            int perfectSimilarity = 0;
            for (int i = 0; i < AlignedSequences.FirstAlignedSequence.Length; i++)
            {
                editDistance += DistanceMatrix[Nucleotides[AlignedSequences.FirstAlignedSequence[i]], Nucleotides[AlignedSequences.SecondAlignedSequence[i]]];
                perfectSimilarity += SimilarityMatrix[Nucleotides[AlignedSequences.FirstAlignedSequence[i]], Nucleotides[AlignedSequences.FirstAlignedSequence[i]]];
                similarity += SimilarityMatrix[Nucleotides[AlignedSequences.FirstAlignedSequence[i]], Nucleotides[AlignedSequences.SecondAlignedSequence[i]]];
            }
            similarity /= perfectSimilarity;
        }

        public void ReadSequencesFromFiles()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = appDirectory.Substring(0, appDirectory.IndexOf("bin"));
            if (File.Exists(fullPath + SequenceOneFileName) && File.Exists(fullPath + SequenceTwoFileName))
            {
                FirstSequence = File.ReadAllText(fullPath + SequenceOneFileName);
                SecondSequence = File.ReadAllText(fullPath + SequenceTwoFileName);
                //Console.WriteLine("First sequence: " + FirstSequence);
                //Console.WriteLine("Second sequence: " + SecondSequence);
            }
            if (File.Exists(fullPath + SimilarityMatrixFileName))
            {
                matrix = File.ReadAllLines(fullPath + SimilarityMatrixFileName).Select(x => x.Split(',')).ToArray();
                for (int i = 0; i < matrix.Length; i++)
                {
                    for (int j = 0; j < matrix[i].Length; j++)
                    {
                        SimilarityMatrix[i, j] = int.Parse(matrix[i][j]);
                    }
                }
                //Console.WriteLine("Similarity Matrix was loaded successfully!");
            }
            if (File.Exists(fullPath + DistanceMatrixFileName))
            {
                matrix = File.ReadAllLines(fullPath + SimilarityMatrixFileName).Select(x => x.Split(',')).ToArray();
                for (int i = 0; i < matrix.Length; i++)
                {
                    for (int j = 0; j < matrix[i].Length; j++)
                    {
                        DistanceMatrix[i, j] = int.Parse(matrix[i][j]);
                    }
                }
                //Console.WriteLine("Distance Matrix was loaded successfully!");
            }
        }
    }
}