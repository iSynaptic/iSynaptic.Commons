﻿using System;
using System.Collections.Generic;
using System.Text;

using iSynaptic.Commons.AOP;

namespace iSynaptic.Commons.AOP
{
    public class UnitOfWorkStub : UnitOfWork<object, UnitOfWorkStub>
    {
        private Action<object> _ProcessHandler = null;

        public UnitOfWorkStub()
        {
        }

        public UnitOfWorkStub(Action<object> processHandler)
        {
            _ProcessHandler = processHandler;
        }

        protected override void Process(IEnumerable<object> items)
        {
            foreach (var item in items)
            {
                if (_ProcessHandler != null)
                    _ProcessHandler(item);
            }
        }

        public static UnitOfWorkStub Current
        {
            get { return GetCurrentScope(); }
        }
    }
}