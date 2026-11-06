using System;
using System.Collections.Generic;

namespace AuctionMerge
{
    // Pure rules: no scene objects, animation timing or input dependencies.
    public sealed class MergeGameState
    {
        public struct BoardItem
        {
            public int Chain;
            public int Level;
            public int Lock;
            public bool IsEmpty => Level == 0 && Lock == 0;
            public bool IsCard => Level > 0 && Lock == 0;
            public static BoardItem Card(int chain, int level) => new BoardItem { Chain = chain, Level = level };
            public static BoardItem Padlock(int collection) => new BoardItem { Lock = collection + 1 };
        }

        readonly BoardItem[] cells;
        readonly bool[] delivered = new bool[9];
        readonly bool[] unlocked = new bool[3];
        public int Stage { get; private set; }
        public bool Finished => Stage == 3;
        public int CellCount => cells.Length;
        public BoardItem this[int index] => cells[index];
        public bool Delivered(int chain) => delivered[chain];
        public bool Unlocked(int collection) => unlocked[collection];
        public bool KeyAvailable(int collection) => collection == Stage && Complete(collection) && !unlocked[collection];

        public MergeGameState(BoardItem[] initial)
        {
            cells = (BoardItem[])initial.Clone();
            // The scene already has the skeleton's cape and wand completed.
            delivered[1] = delivered[2] = true;
        }

        public bool Complete(int collection)
        {
            int first = collection * 3;
            return delivered[first] && delivered[first + 1] && delivered[first + 2];
        }

        public bool CanMerge(int source, int target)
        {
            if (!Valid(source) || !Valid(target) || source == target || Finished) return false;
            BoardItem a = cells[source], b = cells[target];
            return a.IsCard && b.IsCard && a.Chain == b.Chain && a.Level == b.Level && a.Level < 3;
        }

        public bool MoveOrMerge(int source, int target, out bool merged)
        {
            merged = false;
            if (!Valid(source) || !Valid(target) || source == target || Finished || !cells[source].IsCard) return false;
            if (cells[target].IsEmpty)
            {
                cells[target] = cells[source];
                cells[source] = default;
                return true;
            }
            if (!CanMerge(source, target)) return false;
            cells[target] = BoardItem.Card(cells[target].Chain, cells[target].Level + 1);
            cells[source] = default;
            merged = true;
            return true;
        }

        public bool Deliver(int source, int collection)
        {
            if (!Valid(source) || collection != Stage || Finished) return false;
            BoardItem BoardItem = cells[source];
            if (!BoardItem.IsCard || BoardItem.Level != 3 || BoardItem.Chain / 3 != collection || delivered[BoardItem.Chain]) return false;
            delivered[BoardItem.Chain] = true;
            cells[source] = default;
            return true;
        }

        // Validate capacity before committing: rewards cannot be lost if the board is full.
        public bool Unlock(int collection, int target, Random random, out int[] spawned)
        {
            spawned = Array.Empty<int>();
            if (!Valid(target) || !KeyAvailable(collection) || cells[target].Lock != collection + 1) return false;
            var free = new List<int>();
            for (int i = 0; i < cells.Length; i++) if (cells[i].IsEmpty) free.Add(i);
            if (collection < 2 && free.Count < 12) return false;
            unlocked[collection] = true;
            Stage++;
            if (Finished) return true;
            for (int i = free.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                int swap = free[i]; free[i] = free[j]; free[j] = swap;
            }
            spawned = free.GetRange(0, 12).ToArray();
            for (int i = 0; i < spawned.Length; i++) cells[spawned[i]] = BoardItem.Card(Stage * 3 + i / 4, 1);
            return true;
        }

        bool Valid(int cell) => cell >= 0 && cell < cells.Length;
    }
}
