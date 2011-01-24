using System;
using System.Collections.Generic;
using System.Linq;

namespace iSynaptic.Commons.Collections.Generic
{
    public class SmartLoop<T>
    {
        private readonly List<T> _Items;

        private Func<T, bool> _IgnorePredicate;
        private List<T> _IgnoreItems;

        private Action<List<T>, T> _Action;

        private Action<IEnumerable<T>> _BeforeAllAction;
        private Action<IEnumerable<T>> _AfterAllAction;
        private Action<T, T> _BetweenAction;
        private Action _NoneAction;

        public SmartLoop(IEnumerable<T> items)
        {
            Guard.NotNull(items, "items");
            _Items = items.ToList();
        }

        public SmartLoop<T> Each(Action<T> action)
        {
            Guard.NotNull(action, "action");
            _Action = _Action.FollowedBy((l, x) => action(x));

            return this;
        }

        public SmartLoop<T> OnlyOne(Action<T> action)
        {
            Guard.NotNull(action, "action");
            _Action = _Action.FollowedBy((l, x) => { if (l.Count == 1) action(x); });

            return this;
        }

        public SmartLoop<T> MoreThanOne(Action<T> action)
        {
            Guard.NotNull(action, "action");
            _Action = _Action.FollowedBy((l, x) => { if (l.Count > 1) action(x); });

            return this;
        }

        public SmartLoop<T> Ignore(params T[] items)
        {
            Guard.NotNullOrEmpty(items, "items");
            IgnoreItems.AddRange(items);

            return this;
        }

        public SmartLoop<T> Ignore(Func<T, bool> ignorePredicate)
        {
            Guard.NotNull(ignorePredicate, "ignorePredicate");
            _IgnorePredicate = _IgnorePredicate == null
                ? ignorePredicate
                : _IgnorePredicate.Or(ignorePredicate);

            return this;
        }

        public SmartLoop<T> When(T item, Action<T> action)
        {
            return When(x => EqualityComparer<T>.Default.Equals(x, item), action);
        }

        public SmartLoop<T> When(Func<T, bool> predicate, Action<T> action)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(action, "action");

            action = action.MakeConditional(predicate);

            _Action = _Action.FollowedBy((l, x) => action(x));

            return this;
        }

        public SmartLoop<T> BeforeAll(Action<IEnumerable<T>> action)
        {
            Guard.NotNull(action, "action");
            _BeforeAllAction = _BeforeAllAction.FollowedBy(action);

            return this;
        }

        public SmartLoop<T> AfterAll(Action<IEnumerable<T>> action)
        {
            Guard.NotNull(action, "action");
            _AfterAllAction = _AfterAllAction.FollowedBy(action);

            return this;
        }

        public SmartLoop<T> None(Action action)
        {
            Guard.NotNull(action, "action");
            _NoneAction = _NoneAction.FollowedBy(action);

            return this;
        }

        public SmartLoop<T> Between(Action<T, T> action)
        {
            Guard.NotNull(action, "action");
            _BetweenAction = _BetweenAction.FollowedBy(action);

            return this;
        }

        protected List<T> IgnoreItems
        {
            get { return _IgnoreItems ?? (_IgnoreItems = new List<T>()); }
        }

        public void Execute()
        {
            var finalItems = _Items
                .Where(x => _IgnorePredicate == null || _IgnorePredicate(x) != true)
                .Where(x => IgnoreItems.Contains(x) != true)
                .ToList();

            if (finalItems.Count <= 0)
            {
                if (_NoneAction != null)
                    _NoneAction();

                return;
            }

            if (_BeforeAllAction != null)
                _BeforeAllAction(finalItems);

            for (int i = 0; i < finalItems.Count; i++)
            {
                var item = finalItems[i];

                if(_Action != null)
                    _Action(finalItems, item);

                if (_BetweenAction != null && (finalItems.Count - 1) != i) // not last one
                    _BetweenAction(finalItems[i], finalItems[i + 1]);
            }

            if (_AfterAllAction != null)
                _AfterAllAction(finalItems);
        }
    }

}
