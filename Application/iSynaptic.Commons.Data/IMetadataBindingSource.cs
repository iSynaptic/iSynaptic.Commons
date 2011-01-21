﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataBindingSource
    {
        IEnumerable<IMetadataBinding<TMetadata, TSubject>> GetBindingsFor<TMetadata, TSubject>(MetadataRequest<TMetadata, TSubject> request);
    }
}
