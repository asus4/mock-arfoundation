using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MockARFoundation.Internal
{
    public interface IMockCamera : System.IDisposable
    {
        Texture texture { get; }
        bool isPrepared { get; }
    }
}
