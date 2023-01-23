// 1.Optimal alignment of two sequences I
// Write a program that for a pair of amino acid sequences calculates:
// - the edit distance and the similarity, and
// - the best local and global alignments.
// (You don't have to use gap penalty functions.)
// Data should be read from text files. Data includes: 2 sequences, a similarity matrix, and a
// distance matrix (both matrices of size 5 × 5, since there are 4 nucleotides and space).
// The output of the program should contain the optimal alignment, its edit distance and
// similarity.

using Sequence_Alignment;

namespace SequenceAlignment
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            NeedlemanWunsch globalAligner = new NeedlemanWunsch();
            globalAligner.Run();
            Console.WriteLine("\nxxxxxxxxxxxxxxx\n");
            SmithWaterman localAligner = new SmithWaterman();
            localAligner.Run();
        }
    }
}