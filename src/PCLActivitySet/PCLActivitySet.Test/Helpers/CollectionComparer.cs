using System;
using System.Collections.Generic;
using Ploeh.SemanticComparison.Fluent;

namespace PCLActivitySet.Test.Helpers
{
    public class CollectionComparer<TItem>
    {
        private readonly Func<TItem, TItem, bool> _itemComparer;

        public CollectionComparer()
        {
            this._itemComparer = null;
        }

        public CollectionComparer(Func<TItem, TItem, bool> itemComparer)
        {
            this._itemComparer = itemComparer;
        }

        public bool Equals(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            bool xHasNextValue = false;
            bool yHasNextValue = false;

            if (y == null)
                return x == null;
            if (x == null)
                return false;

            using (IEnumerator<TItem> xIter = x.GetEnumerator())
            {
                using (IEnumerator<TItem> yIter = y.GetEnumerator())
                {
                    while (true)
                    {
                        xHasNextValue = xIter.MoveNext();
                        yHasNextValue = yIter.MoveNext();
                        if (xHasNextValue && yHasNextValue)
                        {
                            if (!this.ItemEquals(xIter.Current, yIter.Current))
                                return false;
                        }
                        else
                            break;
                    }
                }
            }

            return !xHasNextValue && !yHasNextValue;
        }

        private bool ItemEquals(TItem x, TItem y)
        {
            if (this._itemComparer != null)
                return this._itemComparer(x, y);

            var xLikeness = x.AsSource().OfLikeness<TItem>();
            return xLikeness.Equals(y);
        }
    }
}
