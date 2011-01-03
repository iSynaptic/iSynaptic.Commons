﻿using System;

namespace iSynaptic.Commons.Runtime.Serialization
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public class CloneReferenceOnlyAttribute : Attribute
    {
    }
}