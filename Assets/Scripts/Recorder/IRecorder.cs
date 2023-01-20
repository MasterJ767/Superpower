using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recorder {
public interface IRecorder {
    public IEnumerator Record();
    public IEnumerator Rewind(bool self, float time);
    public void ToggleRewind(bool value);
}
}